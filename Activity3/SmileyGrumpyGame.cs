using System;
using System.Collections.Generic;
using ProjectUtility;
/// <summary>
/// File:           BullsAndCowsGame.cs
/// Description:    Class stores the logic of the game's flow.
/// Author:         Mark Mendoza
/// Date:           24/08/2019
/// Last Updated:   20/09/2019
/// Version:        2.0
/// Notes:          Changed input type to accepting row and column input instead of 1-9.
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
        private RowColumnConverter rowColumnConverter = new RowColumnConverter();
        /// <summary>
        /// Intro():
        ///     Introduce user to the game and provide instructions
        ///     
        ///Steps:
        ///     -Print to the console, welcome message and instructions
        ///     -Read user input
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
        /// GameSetUp():
        ///     Set up game for new round.
        /// 
        /// Steps:
        ///     -Generate the cover that hides the grid.
        ///     -Generate the smile and grumpy grid.
        ///     -Reset remaining attempts.
        ///     -Reset smile count.
        ///     -Reset grumpy count.
        ///     -Clear stored inputs.
        ///     -Draw the grid.
        /// </summary>
        public void GameSetUp()
        {
            //Generate cover to hide the grid
            gridCover = GenerateCover(9);
            //Generate 3 smileys and 6 grumpys
            facesCover = GenerateSmileys(9);
            //Reset remaining attempts
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
        /// GameCycle():
        ///     Algorithim for one game cycle
        /// 
        /// Arguments:
        ///     -A reference to a bool depicting a game loop.
        ///     
        /// Steps:
        ///     1# Input Cycle
        ///     -Get input from user.
        ///     -Translate input to the corresponding grid slot.
        ///     -Store that input to prevent inputting it again.
        ///     -Determine if the slot contained a smile or grumpy.
        ///     -Refresh grid.
        ///     
        ///     2# Loop Condition.
        ///     -If smile count is 3, then player wins, terminate loop.
        ///     -If remaining attempts is 0, then player loses, refresh grid, terminate loop.
        /// </summary>
        public void GameCycle(ref bool gameLoop)
        {
            //Display score and remaining attempts
            Console.WriteLine("\n\nSmileys: {0}" +
                "\nGrumpys: {1}" +
                "\nAttempts: {2}"
                , smileCount, grumpyCount, attemptsRemaining);
            //Ask user to for input
            //Get data from the table
            userInput = GetUserInput(storedInputs, rowColumnConverter.RowColToIntTable);
            storedInputs.Add(userInput);
            //Turn off the cover at the index which will reveal a face
            gridCover[userInput] = false;
            //Calculate points depending on which face was revealed
            switch (facesCover[userInput])
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
        /// GameConclusion():
        ///     Display the results of the entire game session
        ///     
        /// Steps:
        ///     -Display the total games won and lost.
        ///     -Wait input from user.
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
        /// Outro():
        ///     Thank user for playing before exiting the application.
        ///     
        /// Steps:
        ///     -Display exiting message.
        ///     -Wait input from user.
        /// </summary>
        public void Outro()
        {
            Console.Clear();
            Console.WriteLine("\nThanks for playing!\nPress enter to exit the game");
            Console.ReadLine();
        }
        /// <summary>
        /// GenerateSmileys()
        ///     Generates an array of faces that contain 3 smileys and 6 grumpys
        /// 
        /// Arguments:
        ///     -Size of the array that is returned.
        /// 
        /// Steps:
        ///     -Create array of specified size.
        ///     -Iterate through the array.
        ///     -Use random generator to pick between smile or grumpy.
        ///     -Set chosen face to element of array.
        ///     -If generated 3 smiles, force grumpy for the rest of the iteration.
        ///     -If not enough smiles before the end of iteration, force smiles.
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
        /// GenerateCover():
        ///     Generates an array of bools that act as a switch to affect other arrays.
        ///     
        /// Arguments:
        ///     -Size of the array to be generated.
        /// 
        /// Steps:
        ///     -Create bool array of specific size.
        ///     -Iterate through the array.
        ///     -Set each element to true.
        ///     -Return array.
        /// </summary>
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
        /// GetUserInput();
        ///     Gets valid input from user to use to uncover a slot in the grid.
        /// 
        /// Arguments:
        ///     -A list of previous valid inputs to compare if the same option is chosen again.
        ///     -A table that translates the input to a value
        /// 
        /// Steps:
        ///     -Try get user to input a row.
        ///     -Try get user to input a column.
        ///     -Convert both inputs into a key.
        ///     -Use key to get an output from the table.
        ///     -If the output has already been chosen before then repeat from the first step.
        /// </summary>
        private int GetUserInput(List<int> previousInputs, Dictionary<string, int> table)
        {
            int temp;
            char[] input = new char[2];
            for(; ; )
            {
                Console.WriteLine("\nPick A row");
                input[(int)GridSelection.Row] = InputValidation.ValidateInput(1, 3).ToString()[0];
                Console.WriteLine("\nPick A column");
                input[(int)GridSelection.Column] = InputValidation.ValidateInput(1, 3).ToString()[0];
                if (table.TryGetValue(new string(input), out temp) && !InputValidation.CheckAlreadyUsedInput(previousInputs, temp))
                {
                    return temp;
                }
                else
                {
                    Console.WriteLine("You've already chosen this slot");
                }
            }
        }
        /// <summary>
        /// Selection mode for inputting grid co-ordinance.
        /// </summary>
        private enum GridSelection
        {
            Row,
            Column
        }
    }
    /// <summary>
    /// Class contains a dictionary with key value pairs representing a row/column input and array indexers.
    /// </summary>
    class RowColumnConverter
    {
        public Dictionary<string, int> RowColToIntTable { get; private set; }

        public RowColumnConverter()
        {
            RowColToIntTable = FillTable();
        }
        /// <summary>
        /// Creates a dictionary that has keys that represent the row and column with a value that
        /// represents an index of an array.
        /// </summary>
        private Dictionary<string, int> FillTable()
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();
            temp.Add("11", 0);
            temp.Add("12", 1);
            temp.Add("13", 2);
            temp.Add("21", 3);
            temp.Add("22", 4);
            temp.Add("23", 5);
            temp.Add("31", 6);
            temp.Add("32", 7);
            temp.Add("33", 8);
            return temp;
        }
    }
    //Used to index emoticons
    public enum Face
    {
        Smile,
        Grumpy
    }
}
