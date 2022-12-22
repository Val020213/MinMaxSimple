using System;
using System.IO;
using System.Collections.Generic;

const string Separator = "\n==========================================\n";

System.Console.WriteLine("Game started!" + Separator);

TicTacToe Game = new TicTacToe();

System.Console.WriteLine("X : you\nO : PC" + Separator);
//MinMaxPlayerMemory Chamocle = new MinMaxPlayerMemory(Mark.O, Mark.X);
//MaxiMinPlayer Pelotudo = new MaxiMinPlayer(Mark.X, Mark.O);
MinMaxPlayer Chamocle = new MinMaxPlayer(Mark.O, Mark.X);

bool first = true;

while (true)
{
    Game.PrintBoard();
    System.Console.WriteLine(Game.Win());
    if (Game.Win() != Mark.None)
    {
        System.Console.WriteLine((Game.Win() == Mark.X) ? "We have Winner!!" : "Game over!");
        break;
    }

    if (first)
    {
        System.Console.WriteLine("Your turn!");
        Coords coords = Helper.ReadCoords();

        while (!Game.CanPlay(coords.i, coords.j))
        {
            System.Console.WriteLine("Invalid input! You can't play there! {0} {1}", coords.i, coords.j);
            coords = Helper.ReadCoords();
        }

        Game.Play(coords.i, coords.j, Mark.X);
        first = false;
    }
    else
    {
        System.Console.WriteLine("PC turn!");
        Chamocle.Play(Game);
        first = true;
    }

}