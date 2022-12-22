using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;

public struct Coords
{
    public Coords(int i, int j) => (this.i, this.j) = (i, j);
    public int i, j;

    public static Coords operator +(Coords A, Coords B) => new Coords(A.i + B.i, A.j + B.j);
}
public enum Mark
{
    None,
    X,
    O,
    Empate,
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
                return "|   |";
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
        Mark current = CheckLine();
        if (current != Mark.None) return current;

        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                if (Board[i, j] == Mark.None) continue;
                if (CheckD(new Coords(i, j), new Coords(1, 1), 1, Board[i, j])) return Board[i, j];
                if (CheckD(new Coords(i, j), new Coords(1, -1), 1, Board[i, j])) return Board[i, j];
            }
        }

        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                if (Board[i, j] == Mark.None) return Mark.None;
            }
        }

        return Mark.Empate; //todas las casillas llenas
    }
    public Mark CheckLine()
    {
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            for (int j = 0; j < Board.GetLength(1) - 2; j++)
            {
                if (AreEquals(Board[i, j], Board[i, j + 1], Board[i, j + 2]))
                    return Board[i, j];
            }
        }

        for (int i = 0; i < Board.GetLength(1); i++)
        {
            for (int j = 0; j < Board.GetLength(0) - 2; j++)
            {
                if (AreEquals(Board[j, i], Board[j + 1, i], Board[j + 2, i]))
                    return Board[j, i];
            }
        }

        return Mark.None;
    }
    public bool CheckD(Coords pos, Coords Dirrection, int deph, Mark current)
    {
        if (pos.i < 0 || pos.j < 0 || pos.i >= Board.GetLength(0) || pos.j >= Board.GetLength(1)) return false;

        if (Board[pos.i, pos.j] != current) return false;

        if (deph == 3) return true;

        return CheckD(pos + Dirrection, Dirrection, ++deph, current);
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
    public override TicTacToe Clone()
    {
        TicTacToe Clon = new TicTacToe();
        Clon.CopyBoard(Board);
        return Clon;
    }

    public bool EqualsBoards(TicTacToe other) //esto realmente es para comparar los boards
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