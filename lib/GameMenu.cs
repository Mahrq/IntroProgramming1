using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Class library namespace containing commonly used methods throughout the project.
/// </summary>
namespace ProjectUtility
{
    /// <summary>
    /// File:           GameMenu.cs
    /// Description:    Automate commonly used user input tasks.
    /// Author:         Mark Mendoza
    /// Date:           20/08/2019
    /// Version:        1.0
    /// Notes:
    /// TODO:           
    /// </summary>
    public class GameMenu
    {
        /// <summary>
        /// Ask user to play another game session or close the application
        /// </summary>
        /// <param name="gameSession">bool maintaining the game session</param>
        /// <param name="appSession">bool maintaining the application session</param>
        public void AskUserPlayAgain(ref bool gameSession, ref bool appSession)
        {
            string userInput;
            Console.WriteLine("\nWould you like to play again? y/n");
            userInput = InputValidation.ValidateYesNo();
            if (userInput == "y")
            {
                gameSession = true;
            }
            else if (userInput == "n")
            {
                appSession = false;
            }
        }
    }
}
