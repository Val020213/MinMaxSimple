using System.Diagnostics;
using System;
using System.IO;
using System.Collections.Generic;
public class PlayTree
{
    public int Score;
    public Coords Coords;
    public int Alfa;
    public int Beta;
    public TicTacToe Game;
    public List<PlayTree> Childs;

    public PlayTree(TicTacToe game)
    {
        Game = game.Clone();
        Score = 0;
        Coords = new Coords(-1, -1);
        Alfa = int.MinValue;
        Beta = int.MaxValue;
        Childs = new List<PlayTree>();
    }
    public PlayTree? SearchBestGame(TicTacToe game)
    {
        if (Childs.Count == 0) return null;

        int Score = Childs[0].Score;
        Coords coords = Childs[0].Coords;
        PlayTree best = Childs[0];

        for (int i = 1; i < Childs.Count; i++)
        {
            if (Childs[i].Score > Score)
            {
                Score = Childs[i].Score;
                coords = Childs[i].Coords;
                best = Childs[i];
            }
        }
        return best;
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
public class MinMaxPlayerMemory
{
    Mark Player;
    Mark Counter;
    PlayTree Root;

    public MinMaxPlayerMemory(Mark player, Mark counter)
    {
        Player = player;
        Counter = counter;
        Root = new PlayTree(new TicTacToe());
    }
    public void Propagation(TicTacToe game)
    {
        PlayTree? newRoot = Root.SearchGame(game);

        if (newRoot != null) Root = newRoot;

        else
        {
            Root = new PlayTree(game);
            MiniMax(Root, true);
            return;
        }

    }
    public void Play(TicTacToe game)
    {
        Propagation(game);
        PlayTree? best = Root.SearchBestGame(game);
        if (best == null) throw new Exception("No best game found!");
        game.Play(best.Coords.i, best.Coords.j, Player);
        Thread.Sleep(1000);
    }

    private int FinalMove(TicTacToe SimulationGame)
    {
        Mark winner = SimulationGame.Win();
        if (winner == Mark.None) throw new Exception("Game hasn't finished yet!");
        return (winner == Player) ? 1 : (winner == Counter) ? -1 : 0;
    }

    void MiniMax(PlayTree currentTree, bool IsMax)
    {
        int bestScore = IsMax ? int.MinValue : int.MaxValue;
        Coords bestMove = new Coords(-1, -1);

        for (int i = 0; i < currentTree.Game.Board.GetLength(0); i++)
        {
            for (int j = 0; j < currentTree.Game.Board.GetLength(1); j++)
            {
                if (!currentTree.Game.CanPlay(i, j)) continue;

                PlayTree SimulationChild = new PlayTree(currentTree.Game);
                SimulationChild.Game.Play(i, j, IsMax ? Player : Counter);
                currentTree.Childs.Add(SimulationChild);
                int CounterScore = 0;

                if (SimulationChild.Game.Win() == Mark.None)
                {
                    MiniMax(SimulationChild, !IsMax);
                    CounterScore = SimulationChild.Score;
                }
                else
                {
                    SimulationChild.Score = FinalMove(SimulationChild.Game);
                    CounterScore = SimulationChild.Score;
                }

                if (IsMax && CounterScore > bestScore)
                {
                    //currentTree.Alfa = CounterScore;
                    bestScore = CounterScore;
                    bestMove = new Coords(i, j);
                }
                else if (!IsMax && CounterScore < bestScore)
                {
                    //currentTree.Beta = CounterScore;
                    bestScore = CounterScore;
                    bestMove = new Coords(i, j);
                }
            }
        }
        currentTree.Score = bestScore;
        currentTree.Coords = bestMove;
    }
}

public class MaxiMinPlayer
{
    Mark Player;
    Mark Counter;

    public MaxiMinPlayer(Mark player, Mark counter)
    {
        Player = player;
        Counter = counter;
    }

    public void Play(TicTacToe game)
    {
        TicTacToe TicTacToeGame = game;
        (int, Coords) bestMove = MiniMax(TicTacToeGame, false);
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