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
