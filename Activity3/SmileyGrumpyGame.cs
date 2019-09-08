using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectUtility;
/// <summary>
/// File:           BullsAndCowsGame.cs
/// Description:    Class stores the logic of the game's flow.
/// Author:         Mark Mendoza
/// Date:           24/08/2019
/// Version:        1.0
/// </summary>
namespace Activity3
{
    public class SmileyGrumpyGame : IGameModel
    {
        //Game stats
        private int smileCount = 0;
        private int grumpyCount = 0;
        private int attemptsRemaining = 6;
        private int gamesWon = 0;
        private int gamesLost = 0;
        //Game mechanics
        private bool[] gridCover;
        private Face[] facesCover;
        private int userInput;
        private List<int> storedInputs = new List<int>();
        //Required instances
        private Random random = new Random();
        private GameGraphics gameGraphics = new GameGraphics();
        /// <summary>
        /// Introduce user to game and display instructions
        /// </summary>
        public void Intro()
        {
            Console.WriteLine("Welcome to Smileys and Grumpys!" +
                "\nFind all 3 smileys in the grid within 6 attempts and you Win!" +
                "\nGood Luck!" +
                "\n\nPress enter to continue");
            Console.ReadLine();
        }
        /// <summary>
        /// Set up game for new round.
        /// </summary>
        public void GameSetUp()
        {
            //Generate cover to hide the grid
            gridCover = GenerateCover(9);
            //Generate 3 smileys and 6 grumpys
            facesCover = GenerateSmileys(9);
            attemptsRemaining = 6;
            //Reset smile and grumpy count
            smileCount = 0;
            grumpyCount = 0;
            //Clear stored inputs for next game.
            if (storedInputs.Count > 0)
            {
                storedInputs.Clear();
            }
            //Draw the grid to console
            gameGraphics.DrawGrid(gridCover, facesCover);
        }
        /// <summary>
        /// Algorithim for one game cycle
        /// </summary>
        /// <param name="gameLoop"></param>
        public void GameCycle(ref bool gameLoop)
        {
            //Display score and remaining attempts
            Console.WriteLine("\n\nSmileys: {0}" +
                "\nGrumpys: {1}" +
                "\nAttempts: {2}"
                , smileCount, grumpyCount, attemptsRemaining);
            //Ask user to for input
            Console.WriteLine("\nChoose a number from the table");
            userInput = GetUserInput(storedInputs);
            storedInputs.Add(userInput);
            //Turn off the cover at the index which will reveal a face
            gridCover[userInput - 1] = false;
            //Calculate points depending on which face was revealed
            switch (facesCover[userInput - 1])
            {
                case Face.Smile:
                    smileCount++;
                    break;
                case Face.Grumpy:
                    grumpyCount++;
                    break;
                default:
                    break;
            }
            attemptsRemaining--;
            //Refresh the grid with the updated covers
            gameGraphics.DrawGrid(gridCover, facesCover);
            //Win Condition
            if (smileCount == 3)
            {
                Console.WriteLine("\nYou found all the smileys! You Win!");
                Console.ReadLine();
                gamesWon++;
                gameLoop = false;
            }
            //Lose Condition
            else if (attemptsRemaining <= 0)
            {
                //Turn off the covers for the rest of the grid
                for (int i = 0; i < gridCover.Length; i++)
                {
                    if (gridCover[i])
                    {
                        gridCover[i] = false;
                    }                   
                }
                //Refresh grid showing all the faces.
                gameGraphics.DrawGrid(gridCover, facesCover);
                Console.WriteLine("\nBad Luck, You didn't find all the smileys! You Lose!");
                Console.ReadLine();
                gamesLost++;
                gameLoop = false;
            }
        }
        /// <summary>
        /// Displays the result of the game
        /// </summary>
        public void GameConclusion()
        {
            Console.WriteLine("\nGame Stats\n" +
                "Wins: {0}\n" +
                "Loss: {1}",
            gamesWon.ToString(), gamesLost.ToString());
            Console.ReadLine();
        }
        /// <summary>
        /// Thanks user for playing before exiting the application.
        /// </summary>
        public void Outro()
        {
            Console.Clear();
            Console.WriteLine("\nThanks for playing!\nPress enter to exit the game");
            Console.ReadLine();
        }
        /// <summary>
        /// Generates an array of faces that contain 3 smileys and 6 grumpys
        /// </summary>
        /// <param name="arraySize">size of the array to be created</param>
        /// <returns></returns>
        private Face[] GenerateSmileys(int arraySize)
        {
            int smileyCount = 0;
            Face[] temp = new Face[arraySize];
            Face tempFace;

            for (int i = 0; i < temp.Length; i++)
            {
                //Randomly generate a face if less than 3 total generated smiles
                if (smileyCount < 3)
                {
                    tempFace = (Face)random.Next((int)Face.Smile, (int)Face.Grumpy + 1);
                    //Increase smile count if smile
                    if (tempFace == Face.Smile)
                    {
                        temp[i] = tempFace;
                        smileyCount++;
                    }
                    //Force assign smiley if not enough smileys have been assigned up until the last indexes of the array
                    else if (i == (temp.Length - (3 - smileyCount)))
                    {
                        temp[i] = Face.Smile;
                        smileyCount++;
                    }
                    //Assign grumpy
                    else
                    {
                        temp[i] = tempFace;
                    }
                }
                //Force assign grumpy if already at 3 smiles
                else
                {
                    temp[i] = Face.Grumpy;
                }
            }
            return temp;
        }
        /// <summary>
        /// Generates an array of bools that act as a switch to affect other arrays.
        /// </summary>
        /// <param name="arraySize">size of the array to be created</param>
        /// <returns></returns>
        private bool[] GenerateCover(int arraySize)
        {
            bool[] temp = new bool[arraySize];
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i] = true;
            }
            return temp;
        }
        /// <summary>
        /// Gets valid input from user to use in game.
        /// </summary>
        /// <param name="previousInputs">A list of stored valid inputs a user has made to prevent duplicate inputs</param>
        /// <returns></returns>
        private int GetUserInput(List<int> previousInputs)
        {
            int input;
            for (; ; )
            {
                //Get input
                input = InputValidation.ValidateNumeric(1, 9);
                //Check if any previous inputs
                if (InputValidation.CheckAlreadyUsedInput(previousInputs, input))
                {
                    Console.WriteLine("You've alredy chosen this number!\nPlease enter a number between 1-9");
                }
                //Successful input
                else
                {
                    return input;
                }
            }
        }
    }
    //Used to index emoticons
    public enum Face
    {
        Smile,
        Grumpy
    }
}
