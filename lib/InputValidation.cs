using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Class library namespace containing commonly used methods throughout the project
/// </summary>
namespace ProjectUtility
{
    /// <summary>
    /// File:           InputValidation.cs
    /// Description:    Class contains methods used in validating the user's input.
    /// Author:         Mark Mendoza
    /// Date:           20/08/2019
    /// Version:        1.0
    /// Notes:
    /// TODO:           
    /// </summary>
    public static class InputValidation
    {
        /// <summary>
        /// ValidateYesNo(): 
        ///     Method returns a 'y' or 'n' char used for validating yes or no inputs.
        /// 
        /// Steps:
        ///     -Try get user input
        ///     -If input is a 'y' or 'n' char then return the char
        ///     -Otherwise print to console error, prompt user to make another input
        /// </summary>
        public static string ValidateYesNo()
        {
            string input;
            //Keep looping until user inputs a valid answer.
            for (; ; )
            {
                input = Console.ReadLine();
                if (input == "y" || input == "n")
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Invalid input, please enter 'y' or 'n'");
                }
            }
        }
        /// <summary>
        /// ValidateInput(): 
        ///     Method returns an integer value defined by user input while also validating their input.
        ///     Overloaded version from the string variation of this method.
        /// 
        /// Arguments:
        ///     -The minimum integer input allowed from the user. Default 0
        ///     -The Maximum integer input allowed from the user. Default 9
        ///     
        /// Steps:
        ///     -Try get user input
        ///     -Check if user input is all numeric
        ///     -Check if user input is over the max range
        ///     -Check if user input is under the minimum range
        ///     -If the input passes all checks then it is a successful input and returns the integer input
        ///     -Otherwise print to console error, prompt user to make another input.
        /// </summary>
        public static int ValidateInput(int rangeMin = 0, int rangeMax = 9)
        {
            int userInput;
            for (; ; )
            {
                //Try get user input, catching for non numeric exceptions
                try
                {
                    userInput = (Int32.Parse(Console.ReadLine()));
                    //Catch out of range inputs upper bound
                    if (userInput > rangeMax)
                    {
                        Console.WriteLine("Too High!\nPlease enter a number between {0}-{1}", rangeMin.ToString(), rangeMax.ToString());
                    }
                    //Catch out of range inputs lower bound
                    else if (userInput < rangeMin)
                    {
                        Console.WriteLine("Too Low!\nPlease enter a number between {0}-{1}", rangeMin.ToString(), rangeMax.ToString());
                    }
                    //Successful input
                    else
                    {
                        return userInput;
                    }
                }
                //Catch non numeric inputs.
                catch (System.FormatException)
                {
                    Console.WriteLine("Numbers Only!\nPlease enter a number between {0}-{1}", rangeMin.ToString(), rangeMax.ToString());
                }
                //Catch overflow if trying to parse beyond max int value.
                catch (System.OverflowException)
                {
                    Console.WriteLine("Too High!\nPlease enter a number between {0}-{1}", rangeMin.ToString(), rangeMax.ToString());
                }
            }
        }
        /// <summary>
        /// CheckAlreadyUsedInput(): 
        ///     Returns true if the user's current input has already been used in previous valid inputs.
        ///     Input can be in any type
        /// 
        /// Arguments:
        ///     -A list containing the previously stored inputs of the user
        ///     -The current input of the user
        ///     
        /// Steps:
        ///     -Iterate through each item of the list
        ///     -If the input equals any item in the list, return true
        ///     -Return false, if finished iterating through the list with no matches
        /// </summary>
        public static bool CheckAlreadyUsedInput<T>(List<T> storedInputs, T input)
        {
            for (int i = 0; i < storedInputs.Count; i++)
            {
                //Imediately returns the moment the input equals the index.
                if (input.Equals(storedInputs[i]))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// ValidateInput():
        ///     Validate a string input. Prevent user from entering empty input or a string of white spaces.
        /// 
        /// Steps:
        ///     -Try get user input
        ///     -Check if input contains anything
        ///     -Check if input contains only white spaces
        ///     -If the input passes all checks then it is a successful input and returns the string input
        ///     -Otherwise print to console error, prompt user to make another input
        /// </summary>
        public static string ValidateInput()
        {
            string userInput;
            for (; ; )
            {
                //Ask user for input
                userInput = Console.ReadLine();
                //Catch null or empty input
                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("You didn't enter anything!\n");
                }
                //Catch if the user input only blank characters
                else if (userInput.All(char.IsWhiteSpace))
                {
                    Console.WriteLine("You didn't enter anything!\n");
                }
                //Successful input
                else
                {
                    return userInput;
                }
            }
        }
        /// <summary>
        /// ValidateLetter():
        ///     Validate a letter input. Prevent user from entering empty input, numbers or a blank space
        /// 
        /// Arguments:
        ///     -A selection mode that determines whether the input is converted to an upper or lower case or left alone before returning
        /// 
        /// Steps:
        ///     #1  Validating Input
        ///     -Try get user input
        ///     -Check if input is a letter
        ///     -Check if input is more than 1 character
        ///     -Check if input is empty
        ///     -Check if input contains white space
        ///     -If input passes all checks then move to Step 2,
        ///     -Otherwise print to console error, prompt user to make another input
        ///     #2  Returning Input
        ///     -Evaluate the argument passed
        ///     -If upper casing, then return the input as upper case character
        ///     -If lower casing, then return the input as lower case character
        ///     -If default, then return the input as it was
        /// </summary>
        public static char ValidateLetter(LetterCase letterCasing = LetterCase.Default)
        {
            string userInput;
            for (; ; )
            {
                userInput = Console.ReadLine();
                //Accept letters only
                if (userInput.All(char.IsLetter))
                {
                    //Cause user to input again if more than 1 char
                    if (userInput.Length > 1)
                    {
                        Console.WriteLine("Too many characters!\nPlease enter a letter from A to Z");
                    }
                    //Cause user to input again if they didn't enter anything
                    else if (userInput.Length == 0)
                    {
                        Console.WriteLine("You didn't enter anything!\nPlease enter a letter from A to Z");
                    }
                    //Cause user to input again if they entered blank spaces
                    else if (userInput[0] == ' ')
                    {
                        Console.WriteLine("You didn't enter anything!\nPlease enter a letter from A to Z");
                    }
                    //Successful input
                    else
                    {
                        switch (letterCasing)
                        {
                            case LetterCase.Upper:
                                //Check if input is already uppercase before returning
                                if (!userInput.All(char.IsUpper))
                                {
                                    return userInput.ToUpper()[0];
                                }
                                return userInput[0];
                            case LetterCase.Lower:
                                //Check if input is already lowercase before returning
                                if (!userInput.All(char.IsLower))
                                {
                                    return userInput.ToLower()[0];
                                }
                                return userInput[0];
                            case LetterCase.Default:
                                return userInput[0];
                            default:
                                return userInput[0];
                        }
                    }
                }
                //Catch numeric and non alphabetical input.
                else
                {
                    Console.WriteLine("Letters only!\nPlease enter a letter from A to Z");
                }
            }
        }
    }
    /// <summary>
    /// Options for returning string or char values in desired case
    /// </summary>
    public enum LetterCase
    {
        Upper,
        Lower,
        Default
    }
}
