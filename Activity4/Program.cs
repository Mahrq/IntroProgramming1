using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectUtility;
/// <summary>
/// File:           Program.cs
/// Description:    Game of Random Racer where 2 entities race to the end of a line.
/// Author:         Mark Mendoza
/// Date:           20/08/2019
/// Version:        1.0
/// Notes:
/// TODO:           
/// </summary>
namespace Activity4
{
    class Program
    {
        static void Main(string[] args)
        {
            //Loop Conditionals
            bool appRunning = true;
            bool gameRunning = true;
            //Required instances
            RandomRacerGame randomRacerGame = new RandomRacerGame();
            GameMenu gameMenu = new GameMenu();           
            //Introduce user to game
            randomRacerGame.Intro();
            //Application session
            while (appRunning)
            {
                //Set up a new game round.
                randomRacerGame.GameSetUp();
                //Game session
                while (gameRunning)
                {
                    randomRacerGame.GameCycle(ref gameRunning);
                }               
                //Display Score
                randomRacerGame.GameConclusion();
                //Ask user to play another game
                gameMenu.AskUserPlayAgain(ref gameRunning, ref appRunning);
            }
            //Thank user for playing before exiting the application.
            randomRacerGame.Outro();
        }
    }
}
