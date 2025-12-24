# Notes to self

## 2025 Day 3 - Batteries

First I tried to solve it by all possible combinations. That worked for part 1 (with a small length of 2), but for part 2 the length was 12 batteries. Way too large to solve it by testing all combinations. So I had to switch it, by starting to iterate over the amount of batteries instead of figuring all combinations.

The key insight was to use a greedy approach: for each battery position, find the largest available digit from the remaining positions, ensuring there are enough positions left for the remaining batteries. This avoided the exponential complexity of generating all combinations.

## 2021 Day 6 - Lanternfish

Almost the same problem as Day 3 2025. For part 1 (80 days) all combinations was doable, but for part 2 (256 days) not. Had to switch from tracking individual fish to tracking counts of fish at each timer state. Instead of simulating each fish individually, I used a dictionary to count how many fish were at each stage (0-8 days), then just shifted the counts each day.

## 2025 Day 7 - Water flow

Classic water flow simulation. `^` blocks/deflects flow to both sides. Instead of walking full paths (part A style), just track the X-column counts: when hitting `^`, increment flow at `x-1` and `x+1` and clear `x`. This avoids path-walking and makes part B much faster.
Key bits that mattered:
- Part A: actually walking the stream, marking `|` on the grid and splitting until both sides stop.
- Part B: drop the walk; keep an array `b[x]` of counts. When a `^` is found with incoming flow at `x` (the column index holding the current flow amount), add the flow to `x-1` and `x+1`, set `b[x]=0`, and increment the split counter if `b[x]>0` before clearing.
- Initialize `b[startX]=1` where the `S` is; every other column starts at 0. Sum of `b` after processing is total water reaching the bottom.

## 2025 Day 9 - Grid visualization

The grid that part 2 produces is way too large to simply just print it in the Console output. Decided to divide all coordinates by a factor of 500 and used `Math.Floor` to make it whole ints. That way I could print it and debug visually (and found the error pretty quickly). The scaling down approach made it possible to see the overall pattern and structure without getting lost in massive coordinate values.

## 2025 Day 11 - Flag-aware memoized DFS (summary)

Problem summary (short): count all paths in a directed graph from a start node to the special node `"out"` where a valid path must visit both nodes named `"fft"` and `"dac"` at least once. The graph is represented as a map of node -> neighbors (string[]). The naive recursion explores exponentially many paths.

Key idea: memoize sub-results but the number of valid completions from a node depends on whether `fft` and/or `dac` have already been seen on the path. Cache must therefore be parameterized by these two boolean flags.

Cache design
- Use a cache keyed on the current node (or `string.Join(",", currentNeighbors)` as in the code) and store an array of four values: one value for each flag combination.
- Index mapping (bitmask): `idx = (visitedFourier ? 1 : 0) | (visitedDac ? 2 : 0)` gives 0..3.
- Each cache slot stores the count of valid completions for the corresponding flag-state. Use a sentinel (e.g. -1) for "not cached".

Algorithm outline (recursive DFS with memo):
- function Solve(patterns, currentNeighbors, visitedFourier, visitedDac):
  - key = join(currentNeighbors)
  - idx = (visitedFourier ? 1 : 0) | (visitedDac ? 2 : 0)
  - if cache[key] exists and cache[key][idx] != -1: return cache[key][idx]
  - if currentNeighbors[0] == "out":
    - result = (visitedFourier && visitedDac) ? 1 : 0
    - cache[key][idx] = result; return result
  - total = 0
  - for each next in currentNeighbors:
    - if not patterns.ContainsKey(next) continue
    - newF = visitedFourier || next == "fft"
    - newD = visitedDac || next == "dac"
    - total += Solve(patterns, patterns[next], newF, newD)
  - cache[key][idx] = total; return total

Small example
- patterns:
  - `out: ` (terminal)
  - `a: fft b`
  - `b: dac out`

Start: `a` with flags (false,false)
- Solve(a, F=false,D=false): iterate neighbors `fft` and `b`
  - `fft` → neighbors unknown or terminal: visiting `fft` sets F=true; recurse
  - `b` → visiting `b` may reach `dac` which sets D=true
- Eventually `out` is reached with both flags true -> counts as 1

Cache at runtime (conceptual) after evaluating `out`:
- FlowCache["out"] = [0,0,0,1]  // counts for [none, fft-only, dac-only, both]

This pattern generalizes: when memoizing DFS you must include all relevant path-dependent state in the cache key (often a small bitmask of booleans) so cached sub-results remain correct and reusable.
