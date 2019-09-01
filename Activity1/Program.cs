using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectUtility;
/// <summary>
/// File:           Program.cs
/// Description:    Simulates a game of "Mad Libs" with 3 stories to choose from.
///                 Game can be narrated by system voice if any are installed.
///                 User can save their generated story into a text file.
/// Author:         Mark Mendoza
/// Date:           17/08/2019
/// Version:        1.0
/// Notes:
/// TODO:
/// </summary>
namespace Activity1
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Loop Conditionals
            bool appRunning = true;
            bool gameRunning = true;
            //Required Instances
            MadLibsGame madLibsGame = new MadLibsGame();
            GameMenu gameMenu = new GameMenu();

            //Intro to game, tell user instructions
            madLibsGame.Intro();
            while (appRunning)
            {
                //Set up new round of game
                madLibsGame.GameSetUp();
                //Game session
                while (gameRunning)
                {
                    madLibsGame.GameCycle(ref gameRunning);
                }
                //Display results from playing the game
                madLibsGame.GameConclusion();
                //Ask user if they want to play another round.
                gameMenu.AskUserPlayAgain(ref gameRunning, ref appRunning);
            }
            //Thank user for playing before closing the application.
            madLibsGame.Outro();
        }
    }
}
