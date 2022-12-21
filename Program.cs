using System;
using System.IO;
using System.Collections.Generic;

const string Separator = "\n==========================================\n";

System.Console.WriteLine("Game started!" + Separator);

TicTacToe Game = new TicTacToe();

MinMaxPlayer virtualPlayer = new MinMaxPlayer(Mark.O, Game);

System.Console.WriteLine("X : you\nO : PC" + Separator);

bool first = true;

while (true)
{
    Game.PrintBoard();

    if (Game.Win() != Mark.None)
    {
        System.Console.WriteLine("Game over!");
        break;
    }

    if (first)
    {
        System.Console.WriteLine("Your turn!");
        Coords coords = Helper.ReadCoords();

        while (!Game.CanPlay(coords.i, coords.j))
        {
            System.Console.WriteLine("Invalid input!");
            coords = Helper.ReadCoords();
        }

        Game.Play(coords.i, coords.j, Mark.X);
        first = false;
    }
    else
    {
        System.Console.WriteLine("PC turn!");
        virtualPlayer.Play();
        first = true;
    }

}