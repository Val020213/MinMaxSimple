using System;
using System.IO;
using System.Collections.Generic;

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
    public override bool CanPlay(int i, int j) => (Board[i, j] != Mark.None);
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
}

public class MinMaxPlayer
{
    public Mark Mark { get; }
    public BoardGame InitialGame { get; }
    
    public MinMaxPlayer(Mark mark, BoardGame game)
    {
        Mark = mark;
        InitialGame = game.Clone();
    }
    public Coords Play()
    {
        (int, Coords) result = MinMax(Mark, InitialGame);
        Thread.Sleep(1000);
        return result.Item2;
    }
    private int FinalMove(Mark playerMark, BoardGame Game)
    {
        Mark winner = Game.Win();

        if (winner == Mark.None)
            throw new InvalidOperationException($"Error in FinalMove, winner is None, {playerMark} , game has not ended");

        return winner == playerMark ? 1 : -1;
    }

    private (int, Coords) MinMax(Mark playerMark, BoardGame Game, int value = int.MinValue, bool isMax = true)
    {
        int score = value;
        Coords coords = new Coords(-1, -1);

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (!Game.CanPlay(i, j)) continue;

                BoardGame Simulation = Game.Clone();
                Simulation.Play(i, j, playerMark);

                int counterScore = 0;

                if (Simulation.Win() == Mark.None)
                    (counterScore, _) = MinMax(playerMark == Mark.X ? Mark.O : Mark.X, Simulation, int.MaxValue, !isMax);

                else //parece un poco obvio que si juego y se acaba el juego fue porque gane, pero en otro juego != TicTacToe puede que no sea asi
                    counterScore = FinalMove(playerMark, Simulation);

                if (isMax && counterScore > score)
                {
                    score = counterScore;
                    coords = new Coords(i, j);
                }

                else if (!isMax && counterScore < score)
                {
                    score = counterScore;
                    coords = new Coords(i, j);
                }
            }
        }
        return (score, coords);
    }


    //tree para guardar las jugadas y el score

}