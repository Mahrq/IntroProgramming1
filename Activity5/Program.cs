using ProjectUtility;
/// <summary>
/// File:           Program.cs
/// Description:    Plays a game of hangman.
///                 User has 6 chances to guess a correct letter before losing.
///                 A body part will be drawn for each incorrect guess.
/// Author:         Mark Mendoza
/// Date:           25/08/2019
/// Version:        1.0
/// Notes:          
/// TODO:           Add more Topics other than mystery Planets
/// </summary>
namespace Activity5
{
    class Program
    {
        static void Main(string[] args)
        {
            //Loop conditionals
            bool appRunning = true;
            bool gameRunning = true;
            //Required Instances
            HangManGame hangManGame = new HangManGame();
            GameMenu gameMenu = new GameMenu();
            //Introduce user to the game and display instructions.
            hangManGame.Intro();
            while (appRunning)
            {
                //Set up game for new round
                hangManGame.GameSetUp();
                //Game session
                while (gameRunning)
                {
                    hangManGame.GameCycle(ref gameRunning);
                }
                //Display results of the game.
                hangManGame.GameConclusion();
                //Ask user to play another game
                gameMenu.AskUserPlayAgain(ref gameRunning, ref appRunning);
            }
            //Thank user for playing before exiting the application.
            hangManGame.Outro();
        }
    }
}
