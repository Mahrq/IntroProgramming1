using System;
using System.Collections.Generic;
using ProjectUtility;
/// <summary>
/// File:           BullsAndCowsGame.cs
/// Description:    Class stores the logic of the game's flow.
/// Author:         Mark Mendoza
/// Date:           24/08/2019
/// Version:        2.0
/// Notes:          Added harder modes to the standard game
/// TODO:           
/// </summary>
namespace Activity2
{
    public class BullsAndCowsGame : IGameModel
    {
        //Game stats
        private Difficulty gameDifficulty;
        private int guessesLeft;
        private int sizeToGuess;
        private int bulls = 0;
        private int cows = 0;
        private int gamesWon = 0;
        private int gamesLost = 0;
        //Storage for both player's number
        private char[] playerNumber;
        private char[] cpuNumber;
        private Random random = new Random();
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
            Console.WriteLine("Welcome to Bulls and Cows\n" +
                "Try guess my numbers\n" +
                "Bulls = Correct number, correct position\n" +
                "Cows = Correct number, different position\n" +
                "Good Luck!\n" +
                "Press enter to continue\n\n");
            Console.ReadLine();
        }
        /// <summary>
        /// GameSetUp():
        ///     Sets up the game for a new round by making the user choose a difficulty
        ///     which determines what numbers the computer generates.
        ///     
        /// Steps:
        ///     -Ask user to pick a difficulty
        ///     -Evaluate difficulty chosen
        ///     -Generates the cpu's number
        /// </summary>
        public void GameSetUp()
        {
            Console.Clear();
            gameDifficulty = SelectDifficulty();
            switch (gameDifficulty)
            {
                case Difficulty.Normal:
                    guessesLeft = 8;
                    sizeToGuess = 4;
                    break;
                case Difficulty.Hard:
                    guessesLeft = 10;
                    sizeToGuess = 6;
                    break;
                case Difficulty.Expert:
                    guessesLeft = 12;
                    sizeToGuess = 8;
                    break;
                default:
                    guessesLeft = 8;
                    sizeToGuess = 4;
                    break;
            }
            cpuNumber = GenerateCpuNumbers(sizeToGuess);
            Console.Clear();
        }
        /// <summary>
        /// GameCycle():
        ///     Algorithim for one game cycle according to the rules of the game.
        ///     
        /// Arguments:
        ///     -Reference to the condition of the while loop that this method is in
        /// 
        /// Steps:
        ///     #1 Input cycle
        ///     -Display amount of attempts the user has to guess the number
        ///     -Get valid input from user
        ///     -Subtract input attempts by 1
        ///     -Compare user's numbers and computer's numbers
        ///     -Calculate bulls and cows score, then print bulls and cows score to console
        ///     -Determine Loop condition
        ///     
        ///     #2 Loop condition
        ///     -If user's numbers matches computer's, then increase win count, terminate game loop.
        ///     -If user's attempts is 0, then increase lose count, terminate game loop.
        ///     -If user's numbers don't match and still has attempts then repeat the input cycle
        /// </summary>
        public void GameCycle(ref bool gameLoop)
        {
            //Display amount of attempts the user has to guess the number
            Console.WriteLine("\nRemaining Attempts: {0}", guessesLeft.ToString());
            //Get input from user
            playerNumber = GetUserInput(sizeToGuess);
            guessesLeft--;
            //Compare the 2 sets of numbers to determine bulls and cows score
            CalculateBullsCows(playerNumber, cpuNumber, out bulls, out cows);
            //Display the hints to the user for the next attempt
            Console.WriteLine("\nBulls: {0}\nCows: {1}", bulls.ToString(), cows.ToString());
            //Win condition
            if (bulls == sizeToGuess)
            {
                Console.WriteLine("\nYou Win!");
                Console.ReadLine();
                gamesWon++;
                gameLoop = false;
            }
            //Lose condition
            else if (guessesLeft <= 0)
            {
                //Display the cpu's numbers
                Console.Write("\nYou Lose!\n" +
                    "My numbers were: ");

                for (int i = 0; i < cpuNumber.Length; i++)
                {
                    Console.Write("{0}", cpuNumber[i]);
                }
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
        /// SelectDifficulty():
        ///     Ask user to select a difficulty
        ///     
        /// Steps:
        ///     -Display instructions to user.
        ///     -Get user input.
        ///     -Return the slected mode.
        /// </summary>
        /// <returns></returns>
        private Difficulty SelectDifficulty()
        {
            Console.WriteLine("Choose Your Difficulty:" +
                "\n\n1 = Normal" +
                "\n2 = Hard" +
                "\n3 = Expert");
            int userInput = InputValidation.ValidateInput(1, 3);
            switch (userInput)
            {
                case 1:
                    return Difficulty.Normal;
                case 2:
                    return Difficulty.Hard;
                case 3:
                    return Difficulty.Expert;
                default:
                    return Difficulty.Normal;
            }
        }
        /// <summary>
        /// GetUserInput():
        ///     Allows the user to enter a guess and checks for valid input.
        ///     
        /// Arguments:
        ///     -Size of the character array of which the user will have to input this amount.
        /// 
        /// Steps:
        ///     -Get user input.
        ///     -Check if input containts any characters.
        ///     -Check if number exceeds 32bit Integer.
        ///     -Check if any character contains 0.
        ///     -Check if if the length of the input matches the size required.
        ///     -If input passes all checks then return the input.
        /// </summary>
        /// <param name="inputArraySize">Will return array of the size</param>
        /// <returns></returns>
        private char[] GetUserInput(int inputArraySize)
        {
            char[] temp = new char[inputArraySize];
            int userInput;
            Console.WriteLine("Please Enter {0} numerical digits between 1-9", inputArraySize.ToString());
            //Loop until valid input
            for (; ; )
            {
                try
                {
                    //Trying to parse an integer makes it easier to catch non numeric inputs.
                    userInput = (Int32.Parse(Console.ReadLine()));
                    temp = userInput.ToString().ToCharArray();
                    //Intercepts inputs that contain '0' characters
                    if (HasZero(temp))
                    {
                        Console.WriteLine("Digits can not have a 0!\nPlease enter {0} numerical digits between 1-9", inputArraySize.ToString());
                    }
                    else
                    {
                        //Intercepts inputs for numbers with more digits than expected.
                        if (temp.Length > inputArraySize)
                        {
                            Console.WriteLine("Too many digits!\nPlease enter {0} numerical digits between 1-9", inputArraySize.ToString());
                        }
                        //Intercepts inputs for numbers with less digits than expected.
                        else if (temp.Length < inputArraySize)
                        {
                            Console.WriteLine("Need more digits!\nPlease enter {0} numerical digits between 1-9", inputArraySize.ToString());
                        }
                        //Successful input
                        else
                        {
                            return temp;
                        }
                    }
                }
                //Intercepts non numeric inputs.
                catch (System.FormatException)
                {
                    Console.WriteLine("Numbers only!\nPlease enter {0} numerical digits between 1-9", inputArraySize.ToString());
                }
                //Catch overflow if trying to parse beyond max int value.
                catch (System.OverflowException)
                {
                    Console.WriteLine("Too many digits!\nPlease enter {0} numerical digits between 1-9", inputArraySize.ToString());
                }
            }
        }
        /// <summary>
        /// GenerateCpuNumbers()
        ///     Generates a specified amount of numbers and stores each digit into an array index.
        ///     Each digit is between 1-9 and is unique.
        ///     
        /// Arguments:
        ///     -The amount of numbers the computer wil generate.
        /// 
        /// Steps:
        ///     -Create array of specified size.
        ///     -Iterate through the array.
        ///     -Randomly generate a number between 1 - 9
        ///     -If the generated number has already been used, then generate a new number.
        ///     -Else the number is assigned into that element of the array.
        ///     -Return array when finished iterating.
        /// </summary>
        /// <param name="inputArraySize">Will return array of the size</param>
        /// <returns></returns>
        private char[] GenerateCpuNumbers(int inputArraySize)
        {
            char[] temp = new char[inputArraySize];
            char generatedChar;
            List<char> storedNumbers = new List<char>();
            for (int i = 0; i < temp.Length; i++)
            {
                for(; ; )
                {
                    generatedChar = random.Next(1, 10).ToString().ToCharArray()[0];
                    if (!InputValidation.CheckAlreadyUsedInput<char>(storedNumbers, generatedChar))
                    {
                        storedNumbers.Add(generatedChar);
                        temp[i] = generatedChar;
                        break;
                    }
                }
            }

            return temp;
        }
        /// <summary>
        /// CalculateBullsCows():
        ///     Compares both the player and cpu arrays. Numbers equal in the same index will output bull 
        ///     while equal numbers in different index will output cow.
        /// 
        /// Arguments:
        ///     -The Player's input numbers.
        ///     -The computer's generated numbers.
        ///     -Reference to a bull counter.
        ///     -Reference to a cow counter.
        /// 
        /// Steps:
        ///     -Iterate through the player's numbers 
        ///     -For each player's number, iterate through the Computer's numbers.
        ///     -If the number is the same at the same index of the computer's number, award a bull point.
        ///     -IF the number is the same at a different index of the computer's number, award a cow point.
        /// </summary>
        private void CalculateBullsCows(char[] firstArray, char[] secondArray, out int bull, out int cow)
        {
            //Resets score for the new guess
            bull = 0;
            cow = 0;

            for (int i = 0; i < firstArray.Length; i++)
            {
                for (int j = 0; j < secondArray.Length; j++)
                {
                    //Check for bulls (Same number in same index)
                    if (i == j)
                    {
                        if (firstArray[i].Equals(secondArray[j]))
                        {
                            bull++;
                        }
                    }
                    //Check for cows (Go through other index checking for same number)
                    else if (firstArray[i].Equals(secondArray[j]))
                    {
                        cow++;
                    }
                }
            }
        }
        /// <summary>
        /// HasZero():
        ///     Returns true if an array has a 0 in one of its index.
        /// 
        /// Arguments:
        ///     -A character array to be evaluated.
        /// 
        /// Steps:
        ///     -Iterate through each element of the array.
        ///     -Return true the moment the '0' character is present.
        ///     -Return false if there aren't any '0' characters.
        /// </summary>
        private bool HasZero(char[] arrayToCheck)
        {
            for (int i = 0; i < arrayToCheck.Length; i++)
            {
                if (arrayToCheck[i] == '0')
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Mode for game set up
        /// </summary>
        private enum Difficulty
        {
            Normal,
            Hard,
            Expert
        }
    }
}
