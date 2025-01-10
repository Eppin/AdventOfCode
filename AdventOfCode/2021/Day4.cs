namespace AdventOfCode._2021;

public class Day4 : Day
{
    public Day4() : base()
    {
    }

    [Answer("41503", Regular)]
    public override object SolveA()
    {
        var numbers = GetSplitInput()[0]
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(s => int.Parse($"{s}"))
        .ToArray();

        var boards = GetBoards();

        foreach (var number in numbers)
        {
            foreach (var board in boards)
            {
                foreach (var t in board)
                {
                    for (var j = 0; j < board.Length; j++)
                    {
                        if (!t[j].Marked && t[j].Number.Equals(number))
                        {
                            t[j].Marked = true;
                        }

                        if (!GetBingo(board))
                            continue;

                        var unmarkedCount = GetUnmarkedCount(board);

                        return $"{unmarkedCount * number}";
                    }
                }
            }
        }

        throw new Exception("Impossible..");
    }

    [Answer("3178", Regular)]
    public override object SolveB()
    {
        var numbers = GetSplitInput()[0]
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(s => int.Parse($"{s}"))
            .ToArray();

        var boards = GetBoards();

        var results = new List<(int index, int number, int unmarked)>();

        foreach (var number in numbers.Select((number, index) => (index, number)))
        {
            foreach (var board in boards)
            {
                if (GetBingo(board))
                    continue;

                foreach (var t in board)
                {
                    for (var j = 0; j < board.Length; j++)
                    {
                        if (!t[j].Marked && t[j].Number.Equals(number.number))
                        {
                            t[j].Marked = true;
                        }

                        if (GetBingo(board))
                        {
                            var unmarkedCount = GetUnmarkedCount(board);
                            results.Add((number.index, number.number, unmarkedCount));

                            break;
                        }
                    }
                }
            }
        }

        var lastWin = results.MaxBy(x => x.index);

        return $"{lastWin.number * lastWin.unmarked}";
    }


    private List<(int Number, bool Marked)[][]> GetBoards()
    {
        var lines = GetSplitInput()
            .Skip(1)
            .Where(str => !string.IsNullOrWhiteSpace(str))
            .ToArray();

        var boards = new List<(int, bool)[][]>();

        for (var i = 0; i < lines.Length; i += 5)
        {
            var l1 = GetBoardLine(lines[i]);
            var l2 = GetBoardLine(lines[i + 1]);
            var l3 = GetBoardLine(lines[i + 2]);
            var l4 = GetBoardLine(lines[i + 3]);
            var l5 = GetBoardLine(lines[i + 4]);

            boards.Add([l1, l2, l3, l4, l5]);
        }

        return boards;
    }

    private static (int, bool)[] GetBoardLine(string line)
    {
        return line
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => (int.Parse($"{s}"), false))
            .ToArray();
    }

    private static bool GetBingo((int, bool)[][] board)
    {
        for (var i = 0; i < board[0].Length; i++)
        {
            if (board[0][i].Item2 && board[1][i].Item2 && board[2][i].Item2 && board[3][i].Item2 && board[4][i].Item2)
                return true;
        }

        for (var i = 0; i < board[0].Length; i++)
        {
            if (board[i][0].Item2 && board[i][1].Item2 && board[i][2].Item2 && board[i][3].Item2 && board[i][4].Item2)
                return true;
        }

        return false;
    }

    private static int GetUnmarkedCount((int, bool)[][] board)
    {
        var result = 0;

        for (var i = 0; i < board[0].Length; i++)
        {
            for (var j = 0; j < board[0].Length; j++)
            {
                if (!board[i][j].Item2)
                    result += board[i][j].Item1;
            }
        }

        return result;
    }
}
