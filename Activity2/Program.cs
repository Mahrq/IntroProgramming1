using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectUtility;
/// <summary>
/// File:           Program.cs
/// Description:    A game of "Bulls and Cows" played against the cpu.
///                 Objective of the game is to guess the correct number the cpu chooses with feedback given
///                 back to the player after every guess.
///                 Bull = correct number and position.
///                 Cow = correct number different position.
/// Author:         Mark Mendoza
/// Date:           17/08/2019
/// Version:        1.0
/// Notes:          Made some variables more flexible to anticipate change in game rules e.g.hardmode.
/// TODO:           Make Hardmode.
///                 Pretty up title screen.
/// </summary>
namespace Activity2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Loop conditionals
            bool appRunning = true;
            bool gameRunning = true;
            //Required Instances
            BullsAndCowsGame bullsAndCowsGame = new BullsAndCowsGame();
            GameMenu gameMenu = new GameMenu();
            //Intro to the game with instructions on how to play
            bullsAndCowsGame.Intro();
            //Application session
            while (appRunning)
            {
                //Game set up
                bullsAndCowsGame.GameSetUp();
                //Game session
                while (gameRunning)
                {
                    bullsAndCowsGame.GameCycle(ref gameRunning);
                }
                //Display game statistics
                bullsAndCowsGame.GameConclusion();
                //Ask user to play another game
                gameMenu.AskUserPlayAgain(ref gameRunning, ref appRunning);
            }
            //Thank user for playing before closing the application.
            bullsAndCowsGame.Outro();
        }
    }
}
