using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectUtility;
using Activity1;
using Activity2;
using Activity3;
using Activity4;
using Activity5;
/// <summary>
/// File:           Program.cs
/// Description:    Test program to consolidate all 5 projects into one program.
/// Author:         Mark Mendoza.
/// Date:           25/08/2019
/// Notes:          
/// </summary>
namespace Final
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Loop conditionals
            bool appRunning = true;
            bool gameRunning = false;
            bool gamePlaying = false;
            //Required instances
            IGameModel[] gameModels = { new MadLibsGame(),
                                        new BullsAndCowsGame(),
                                        new SmileyGrumpyGame(),
                                        new RandomRacerGame(),
                                        new HangManGame()};
            GameMenu gameMenu = new GameMenu();
            //Set default value to an instance
            IGameModel gameToPlay = gameModels[0];
            GameSelect gameSelect;
            //Application session
            while (appRunning)
            {
                //Introduce user to collection of games and instruct them to pick an option
                Console.Clear();
                Console.WriteLine("Welcome to my selection of awesome games!" +
                    "\n\nChoose a game to play or quit the application." +
                    "\n1 = Mad Libs" +
                    "\n2 = Bulls and cows" +
                    "\n3 = Smileys and Grumpys" +
                    "\n4 = Random Racer" +
                    "\n5 = HangMan" +
                    "\n6 = Exit ");
                //User's input will be used to return an enum aswell as use that enum to index the gameModels array
                gameSelect = (GameSelect)(InputValidation.ValidateInput(1, 6) - 1);
                //GameSelect.Exit will always cause out of range exception since its enum value is out of the range of the array
                if (gameSelect == GameSelect.Exit)
                {
                    //Closes the app
                    appRunning = false;
                    gameRunning = false;
                    gamePlaying = false;
                }
                else
                {
                    //Assign the selected game
                    gameToPlay = gameModels[(int)gameSelect];
                    Console.Clear();
                    //Start intro of the chosen game
                    gameToPlay.Intro();
                    gameRunning = true;
                    gamePlaying = true;
                }
                //Session of the current game
                while (gameRunning)
                {
                    //Set up game for new round
                    gameToPlay.GameSetUp();
                    //Session of the game round
                    while (gamePlaying)
                    {
                        //User plays the game round
                        gameToPlay.GameCycle(ref gamePlaying);
                    }
                    //End session of the game round, usually displaying results
                    gameToPlay.GameConclusion();
                    //Ask user to play another round or return to the game selection screen
                    gameMenu.AskUserPlayAgain(ref gamePlaying, ref gameRunning);
                }
            }
            //Thank user for playing before exiting application.
            Console.Clear();
            Console.WriteLine("Thanks for playing!\nPress enter to exit the application");
            Console.ReadLine();
        }
        /// <summary>
        /// Game models indexer
        /// </summary>
        enum GameSelect
        {
            MadLibs,
            BullsAndCows,
            SmileysAndGrumpys,
            RandomRacer,
            HangMan,
            Exit
        }
    }
}
