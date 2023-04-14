namespace AdventOfCode;

public class Puzzle
{
    public int Year { get; set; }
    public int Day { get; set; }
    public Type Type { get; set; }

    public Puzzle(int year, int day, Type type)
    {
        Year = year;
        Day = day;
        Type = type;
    }
}