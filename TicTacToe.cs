
public struct Coords
{
    public Coords(int i, int j) => (this.i, this.j) = (i, j);
    public int i, j;
}
public enum Mark
{
    None,
    X,
    O,
}
public abstract class BoardGame
{
    public abstract Mark[,] Board { get; }
    public abstract void Play(int i, int j, Mark mark);
    public abstract bool CanPlay(int i, int j);
    public abstract Mark Win();
    public abstract BoardGame Clone();

}
public class TicTacToe : BoardGame
{
    public override Mark[,] Board { get; } = new Mark[3, 3];
    public TicTacToe()
    {
        ClearBoard();
    }
    private void ClearBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Board[i, j] = Mark.None;
            }
        }
    }
    private string PrintRegulator(Mark mark)
    {
        switch (mark)
        {
            case Mark.None:
                return "|   |";
            case Mark.X:
                return "| X |";
            case Mark.O:
                return "| O |";
            default:
                return "|  |";
        }
    }
    public void PrintBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(PrintRegulator(Board[i, j]));
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
    public override Mark Win()
    {
        for (int i = 0; i < 3; i++)
        {
            if (AreEquals(Board[i, 0], Board[i, 1], Board[i, 2]))
                return Board[i, 0];
            if (AreEquals(Board[0, i], Board[1, i], Board[2, i]))
                return Board[0, i];
        }

        if (AreEquals(Board[0, 0], Board[1, 1], Board[2, 2]) || AreEquals(Board[0, 2], Board[1, 1], Board[2, 0]))
            return Board[1, 1];

        return Mark.None;
    }

    private bool AreEquals(params Mark[] marks)
    {
        for (int i = 1; i < marks.Length; i++)
        {
            if (marks[0] != marks[i])
                return false;
        }
        return true;
    }
    public override bool CanPlay(int i, int j) => (0 <= i && i <= 2 && 0 <= j && j <= 2) && (Board[i, j] == Mark.None);
    public override void Play(int i, int j, Mark mark)
    {
        if (!CanPlay(i, j)) throw new ArgumentException($"Cannot play on {i}, {j}.");

        Board[i, j] = mark;
    }

    private void CopyBoard(Mark[,] Copyfrom)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Board[i, j] = Copyfrom[i, j];
            }
        }
    }
    public override BoardGame Clone()
    {
        TicTacToe Clon = new TicTacToe();
        Clon.CopyBoard(Board);
        return Clon;
    }

    public bool EqualsBoards(BoardGame other) //esto realmente es para comparar los boards
    {
        if (Board.GetLength(0) != other.Board.GetLength(0) || Board.GetLength(1) != other.Board.GetLength(1))
            return false;


        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                if (Board[i, j] != other.Board[i, j])
                    return false;
            }
        }
        return true;
    }
}