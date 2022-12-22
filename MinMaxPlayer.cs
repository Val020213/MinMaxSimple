using System.Diagnostics;
using System;
using System.IO;
using System.Collections.Generic;
public class PlayTree
{
    int Alfa;
    int Beta;
    public TicTacToe Game;
    public List<PlayTree> Childs;

    public PlayTree(TicTacToe game)
    {
        Game = game.Clone();
        Alfa = int.MinValue;
        Beta = int.MaxValue;
        Childs = new List<PlayTree>();
    }

    public PlayTree? SearchGame(TicTacToe game)
    {
        return SearchGame(game, this);
    }
    private PlayTree? SearchGame(TicTacToe game, PlayTree current)
    {
        if (game.EqualsBoards(current.Game)) return current;

        foreach (PlayTree child in Childs)
        {
            PlayTree? temp = SearchGame(game, child);
            if (temp != null) return temp;
        }
        return null;
    }
    private void AlfaBeta()
    {
        if (Alfa == 0) Alfa = int.MinValue;
        if (Beta == 0) Beta = int.MaxValue;
        throw new NotImplementedException();
    }

}
public class MinMaxPlayer
{
    Mark Player;
    Mark Counter;

    public MinMaxPlayer(Mark player, Mark counter)
    {
        Player = player;
        Counter = counter;
    }

    public void Play(TicTacToe game)
    {
        TicTacToe TicTacToeGame = game;
        (int, Coords) bestMove = MiniMax(TicTacToeGame, true);
        Thread.Sleep(1000);
        TicTacToeGame.Play(bestMove.Item2.i, bestMove.Item2.j, Player);
    }

    private int FinalMove(TicTacToe SimulationGame)
    {
        Mark winner = SimulationGame.Win();
        if (winner == Mark.None) throw new Exception("Game hasn't finished yet!");
        return (winner == Player) ? 1 : (winner == Counter) ? -1 : 0;
    }

    public (int, Coords) MiniMax(TicTacToe game, bool IsMax)
    {
        int bestScore = IsMax ? int.MinValue : int.MaxValue;
        Coords bestMove = new Coords(-1, -1);

        for (int i = 0; i < game.Board.GetLength(0); i++)
        {
            for (int j = 0; j < game.Board.GetLength(1); j++)
            {
                if (!game.CanPlay(i, j)) continue;

                TicTacToe SimulationGame = game.Clone();
                SimulationGame.Play(i, j, IsMax ? Player : Counter);

                int CounterScore = 0;

                if (SimulationGame.Win() == Mark.None)
                    CounterScore = MiniMax(SimulationGame, !IsMax).Item1;

                else
                    CounterScore = FinalMove(SimulationGame);

                if (IsMax && CounterScore > bestScore)
                {
                    bestScore = CounterScore;
                    bestMove = new Coords(i, j);
                }
                else if (!IsMax && CounterScore < bestScore)
                {
                    bestScore = CounterScore;
                    bestMove = new Coords(i, j);
                }
            }
        }
        return (bestScore, bestMove);
    }
}