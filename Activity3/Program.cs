using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectUtility;
/// <summary>
/// File:           Program.cs
/// Description:    Smileys and Grumpys game where the user has to pick the 3 smileys in a 3x3 grid
///                 within 6 attempts.
/// Author:         Mark Mendoza
/// Date:           18/08/2019
/// Version:        1.0
/// Notes:
/// TODO:           Pretty up title screen.
/// </summary>
namespace Activity3
{
    class Program
    {
        static void Main(string[] args)
        {
            //Loop conditionals
            bool appRunning = true;
            bool gameRunning = true;
            //Required instances
            GameMenu gameMenu = new GameMenu();
            SmileyGrumpyGame smileyGrumpyGame = new SmileyGrumpyGame();
            //Introduce user to game and display instructions
            smileyGrumpyGame.Intro();
            //Application session
            while (appRunning)
            {
                //Set up game for new round
                smileyGrumpyGame.GameSetUp();
                //Game session
                while (gameRunning)
                {
                    smileyGrumpyGame.GameCycle(ref gameRunning);
                }
                //Display game statistics
                smileyGrumpyGame.GameConclusion();
                //Ask user to play another game
                gameMenu.AskUserPlayAgain(ref gameRunning, ref appRunning);
            }
            //Thank user for playing before closing the application.
            smileyGrumpyGame.Outro();
        }
    }
}
