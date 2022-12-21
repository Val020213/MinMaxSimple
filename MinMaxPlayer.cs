using System;
using System.IO;
using System.Collections.Generic;

public interface IGame
{
    bool Win();
}
public enum Mark
{
    None,
    X,
    O,
}
public abstract class BoardGame : IGame
{
    public abstract Mark[,] Board { get; }
    public abstract bool Play(int i, int j, Mark mark);
    public abstract bool Win();
    public abstract BoardGame Clone();

}
public class CeritoCruz : BoardGame
{
    public override Mark[,] Board { get; } = new Mark[3, 3];

    public CeritoCruz()
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

    private void PrintBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(Board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }


    public override bool Win()
    {
        for (int i = 0; i < 3; i++)
        {
            if (AreEquals(Board[i, 0], Board[i, 1], Board[i, 2]) || AreEquals(Board[0, i], Board[1, i], Board[2, i]))
                return true;
        }

        if (AreEquals(Board[0, 0], Board[1, 1], Board[2, 2]) || AreEquals(Board[0, 2], Board[1, 1], Board[2, 0]))
            return true;

        return false;
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
    public override bool Play(int i, int j, Mark mark) => (Board[i, j] != Mark.None) ? false : (Board[i, j] = mark) != Mark.None;

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
        CeritoCruz Clon = new CeritoCruz();
        Clon.CopyBoard(Board);
        return Clon;
    }
}

public class MinMaxPlayer
{
    public Mark Mark { get; }
    public BoardGame Game { get; }
    public MinMaxPlayer(Mark mark, BoardGame game)
    {
        Mark = mark;
        Game = game.Clone();
    }

    private int FinalMove(Mark playerMark)
    {
        Mark winner = Game.Win() ? playerMark : Mark.None;

        if (winner == Mark.None)
            throw new InvalidOperationException($"Error in FinalMove, winner is None, {playerMark} , game has not ended");

        return winner == Mark ? 1 : -1;
    }


}