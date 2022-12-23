using System.Diagnostics;
using System;
using System.IO;
using System.Collections.Generic;
public class PlayTree
{
    public Mark Player;
    public Mark Counter;
    public int Score;
    bool IsMax;
    public Coords Coords;
    public TicTacToe Game;
    public List<PlayTree> Childs;

    public PlayTree(TicTacToe game, Mark player, Mark counter, bool isMax = true)
    {
        Game = game.Clone();
        Player = player;
        Counter = counter;
        Score = (isMax) ? int.MinValue : int.MaxValue;
        IsMax = isMax;
        Coords = new Coords(-1, -1);
        Childs = new List<PlayTree>();

    }

    private int FinalMove()
    {
        Mark winner = Game.Win();
        if (winner == Mark.None) throw new Exception("Game hasn't finished yet!");
        return (winner == Player) ? 1 : (winner == Counter) ? -1 : 0;
    }

    public void PropagationMiniMax()
    {
        if (Game.Win() != Mark.None)
        {
            Score = FinalMove();
            return;
        }

        for (int i = 0; i < Game.Board.GetLength(0); i++)
        {
            for (int j = 0; j < Game.Board.GetLength(1); j++)
            {
                if (!Game.CanPlay(i, j)) continue;

                PlayTree Simulation = new PlayTree(Game, Player, Counter, !IsMax);
                // AddChild(Simulation);
                Simulation.Coords = new Coords(i, j);
                Simulation.Game.Play(i, j, IsMax ? Player : Counter);

                Simulation.PropagationMiniMax();

                if (IsMax && Simulation.Score > Score)
                {
                    AddChild(Simulation); // alfa
                    Score = Simulation.Score;
                }
                else if (!IsMax && Simulation.Score < Score)
                {
                    if (!IsMax) AddChild(Simulation); //non beta
                    Score = Simulation.Score;
                }
            }
        }
    }
    public void AddChild(PlayTree child)
    {
        Childs.Add(child);
    }
    public PlayTree SearchBestGame()
    {
        if (Childs.Count == 0) throw new Exception("No childs!");
        // if (IsMax) System.Console.WriteLine("This is a Max node!");

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
        // best.Game.PrintBoard();
        return best;
    }
    public PlayTree? SearchGame(TicTacToe game) //En el mismo nivel
    {
        return SearchGame(game, this);
    }
    private PlayTree? SearchGame(TicTacToe game, PlayTree current)
    {
        if (game.EqualsBoards(current.Game)) return current;
        //System.Console.WriteLine("Current Tree Boards");
        //current.Game.PrintBoard();

        foreach (PlayTree child in Childs)
        {
            //System.Console.WriteLine("childs :");
            //child.Game.PrintBoard();
            if (child.Game.EqualsBoards(game)) return child;
        }
        return null;
    }

}

public class MiniMaxLoco
{
    public PlayTree Root;
    public Mark Player;
    public Mark Counter;

    public MiniMaxLoco(Mark player, Mark counter)
    {
        Player = player;
        Counter = counter;
        Root = new PlayTree(new TicTacToe(), player, counter);
    }

    public void Play(TicTacToe game)
    {
        Root = Root.SearchGame(game);

        if (Root == null)
        {
            System.Console.WriteLine("No se encontro el juego en el arbol, se creara uno nuevo");
            Root = new PlayTree(game, Player, Counter);
            Root.PropagationMiniMax();
        }

        Root = Root.SearchBestGame();
        Thread.Sleep(1000);
        game.Play(Root.Coords.i, Root.Coords.j, Player);
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