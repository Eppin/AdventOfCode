# Notes to self

## 2025 Day 3 - Batteries

First I tried to solve it by all possible combinations. That worked for part 1 (with a small length of 2), but for part 2 the length was 12 batteries. Way too large to solve it by testing all combinations. So I had to switch it, by starting to iterate over the amount of batteries instead of figuring all combinations.

The key insight was to use a greedy approach: for each battery position, find the largest available digit from the remaining positions, ensuring there are enough positions left for the remaining batteries. This avoided the exponential complexity of generating all combinations.

## 2021 Day 6 - Lanternfish

Almost the same problem as Day 3 2025. For part 1 (80 days) all combinations was doable, but for part 2 (256 days) not. Had to switch from tracking individual fish to tracking counts of fish at each timer state. Instead of simulating each fish individually, I used a dictionary to count how many fish were at each stage (0-8 days), then just shifted the counts each day.
