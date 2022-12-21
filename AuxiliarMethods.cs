
public static class Helper
{
    public static Coords ReadCoords()
    {
        System.Console.Write("Enter row: ");
        int row = int.Parse(ReadCorrect());

        System.Console.Write("Enter col: ");
        int col = int.Parse(ReadCorrect());

        return new Coords(row, col);
    }

    public static string ReadCorrect()
    {

        string? temp = null;

        while (temp == null)
        {
            temp = System.Console.ReadLine();
            if (!NumberTryParse(temp))
            {
                temp = null;
                System.Console.WriteLine("Try again!");
            }
        }
        return temp;
    }
    public static bool NumberTryParse(string? temp)
    {
        if (temp == null) return false;

        try
        {
            int error = int.Parse(temp);
            if (error < 0 || error > 2)
            {
                System.Console.WriteLine("Invalid input! Must be a number between 0 and 2!");
                return false;
            }
        }
        catch (Exception)
        {
            System.Console.WriteLine("Invalid input! Must be a number");
            return false;
        }

        return true;
    }
}