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
}
public class CeritoCruz : BoardGame
{
    public override Mark[,] Board { get; } = new Mark[3, 3];

//agua
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
}

public class MinMaxPlayer
{
    public Mark Mark { get; }
    public BoardGame Game { get; }
    public MinMaxPlayer(Mark mark, BoardGame game)
    {
        Mark = mark;
        Game = game;
    }

    public (int,int) Play()
    {
        int bestScore = int.MinValue;
        (int, int) bestMove = (-1, -1);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Game.Board[i, j] == Mark.None)
                {
                    Game.Play(i, j, Mark);
                    int score = Minimax(Game, false);
                    Game.Play(i, j, Mark.None);
                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (i, j);
                    }
                }
            }
        }
        return bestMove;
    }

    private int Minimax(BoardGame game, bool isMaximizing)
    {
        if (game.Win())
            return isMaximizing ? -1 : 1;

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (game.Board[i, j] == Mark.None)
                {
                    game.Play(i, j, isMaximizing ? Mark.X : Mark.O);
                    int score = Minimax(game, !isMaximizing);
                    game.Play(i, j, Mark.None);
                    bestScore = isMaximizing ? Math.Max(score, bestScore) : Math.Min(score, bestScore);
                }
            }
        }
        return bestScore;
    }
}