using System;
using System.Collections.Generic;
using ProjectUtility;
/// <summary>
/// File:           HangManGame.cs
/// Description:    Handles the game's logic and flow.
/// Author:         Mark Mendoza
/// Date:           25/08/2019
/// Version:        1.0
/// Notes:          https://unicode-table.com/en/#2550
///                 stockYardParts: ║ ╩ │ ═ ╗ ╔
/// TODO:           Pretty up title
/// </summary>
namespace Activity5
{
    public class HangManGame : IGameModel
    {
        //Required instances
        private HangManMysteryWord hiddenWord = new HangManMysteryWord();
        private List<char> storedInputs = new List<char>();
        private GameGraphics gameGraphics = new GameGraphics();
        //Game stats
        private MysteryTopic selectedTopic = MysteryTopic.Planet;
        private char inputGuess;
        private int wrongGuesses = 0;
        private int loseCondition = 6;
        private int deathCount = 0;
        private int winCount = 0;
        //Array used to track progress of the game and update the hangman picture
        private char[] fullPicCurrent;
        //Desired drawn picture with man hidden
        private string fullPicNew = @" ╔════╗
 ║    │
 ║     
 ║      
 ║      
 ║   
═╩═";
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
            Console.WriteLine("Welcome to Hangman!" +
                "\nGuess the letters to uncover the hidden word!" +
                "\n\nPress enter to continue");
            Console.ReadLine();
        }
        /// <summary>
        /// GameSetUp():
        ///     Set up game for a new round
        ///     
        /// Steps:
        ///     -Get user to select from a topic.
        ///     -Generate a hidden from the chosen topic
        ///     -Reset wrong guesses count to 0.
        ///     -Reset the hang man picture.
        ///     -Clear previously stored input.
        /// </summary>
        public void GameSetUp()
        {
            Console.Clear();
            //Instruct user to choose a topic which determines the kind of words that will be hidden
            Console.WriteLine("Choose a word topic:" +
                "\n1 = Planets" +
                "\n2 = Gods");
            //Cast numeric input into enum
            selectedTopic = (MysteryTopic)InputValidation.ValidateInput((int)MysteryTopic.Planet, (int)MysteryTopic.God);
            //Generate the properties of the hidden word depending on the enum passed.
            hiddenWord.ChooseNewWord(selectedTopic);
            //Reset counter
            wrongGuesses = 0;
            //Reset picture to clear any body parts from the previous game
            fullPicCurrent = HangmanRefresh(fullPicNew.ToCharArray(), wrongGuesses);
            //Clear any stored inputs
            if (storedInputs.Count > 0)
            {
                storedInputs.Clear();
            }       
        }
        /// <summary>
        /// GameCycle():
        ///     Algorithim for the game cycle of the game.
        /// 
        /// Arguments:
        ///     -A reference to a bool depicting a game loop.
        ///     
        /// Steps:
        ///     1# Input Cycle
        ///     -Draw the hangman game.
        ///     -Ask user for letter input.
        ///     -Compare the input with the hidden word.
        ///     -If input was wrong then increase wrong guess
        ///     -Refresh the information for the next draw cycle.
        ///     
        ///     2# Loop Cycle
        ///     -If all the letters a revealed then the player has won, terminate loop.
        ///     -If the player reaches too many wrong guesses then the game is lost,
        ///     reveal the hidden word, terminate loop.
        ///     -Else continue input cycle.
        /// </summary>
        /// <param name="gameLoop"></param>
        public void GameCycle(ref bool gameLoop)
        {
            //Draw/Update the hangman game
            gameGraphics.DrawGraphics(fullPicCurrent, hiddenWord);
            Console.WriteLine("\nEnter a letter");
            //Ask user input
            //Method double validates if it was a letter input and if it was an input already chosen
            inputGuess = GetUserLetter(storedInputs);
            //Add successful inputs to the list
            storedInputs.Add(inputGuess);
            //Compare input
            //Method also affects properties within the class that is passed
            if (!CorrectGuess(ref hiddenWord, inputGuess))
            {
                wrongGuesses++;
            }
            //Update char array with new information
            fullPicCurrent = HangmanRefresh(fullPicCurrent, wrongGuesses);
            //Lose Condition
            //Number of wrong guesses exceed the lose condition
            if (wrongGuesses >= loseCondition)
            {
                //Turn off all the covers for the current game
                for (int i = 0; i < hiddenWord.WordCover.Length; i++)
                {
                    hiddenWord.WordCover[i] = false;
                }
                //Redraw the full hangman to reveal the remaining hidden letters
                gameGraphics.DrawGraphics(fullPicCurrent, hiddenWord);
                Console.WriteLine("Oh dear, You are dead!");
                Console.ReadLine();
                deathCount++;
                gameLoop = false;
            }
            //Win Condition
            //If all covers are disabled then it's a win
            else if (FoundAllLetters(hiddenWord.WordCover))
            {
                gameGraphics.DrawGraphics(fullPicCurrent, hiddenWord);
                Console.WriteLine("You revealed the hidden word!");
                Console.ReadLine();
                winCount++;
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
            Console.WriteLine("Game Stats:" +
                "\nWords Revealed: {0}" +
                "\nMen Hung:       {1}", winCount.ToString(), deathCount.ToString());
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
            Console.WriteLine("Thanks for playing!\nPress enter to exit the game");
            Console.ReadLine();
        }
        /// <summary>
        /// Changes parts of the current hangman depending on the number of wrong guesses
        /// </summary>
        /// <param name="currentPic">Current progress of the hangman char array</param>
        /// <param name="incorrectGuessCount">Counter for determining which parts get changed</param>
        /// <returns></returns>
        private char[] HangmanRefresh(char[] currentPic, int incorrectGuessCount)
        {
            char[] temp = currentPic;
            switch (incorrectGuessCount)
            {
                case 1:
                    temp[(int)BodyPart.Head] = 'O';
                    break;
                case 2:
                    temp[(int)BodyPart.Body] = '│';
                    break;
                case 3:
                    temp[(int)BodyPart.RightArm] = '/';
                    break;
                case 4:
                    temp[(int)BodyPart.LeftArm] = '\\';
                    break;
                case 5:
                    temp[(int)BodyPart.RightLeg] = '/';
                    break;
                case 6:
                    temp[(int)BodyPart.LeftLeg] = '\\';
                    break;
                default:
                    break;
            }
            return temp;
        }
        /// <summary>
        /// Determines if a correct guess was made.
        /// Compares the hidden word with the input by iterating through each letter of the hidden word.
        /// </summary>
        /// <param name="currentWord">current inctance of the hidden word</param>
        /// <param name="currentGuess">most recent input of the user</param>
        /// <returns></returns>
        private bool CorrectGuess(ref HangManMysteryWord currentWord, char currentGuess)
        {
            int revealedLetters = 0;
            //Iterate through each letter of the hidden word and compare the current guess.
            for (int i = 0; i < currentWord.Word.ToCharArray().Length; i++)
            {
                //Increase revealed letters counter by 1 if there is a match
                if (currentGuess.Equals(currentWord.Word.ToCharArray()[i]))
                {
                    //Turn off the cover at the index where there was a match to ready the next game render
                    currentWord.WordCover[i] = false;
                    revealedLetters++;
                }
            }
            //Correct guess if at least 1 letter was revealed
            if (revealedLetters > 0)
            {
                return true;
            }
            //Return false if none were revealed
            return false;
        }
        /// <summary>
        /// Victory condition for the game, returns true if all letters are found.
        /// Iterates through a cover and checks if all false;
        /// </summary>
        /// <param name="cover">Word cover for the game</param>
        /// <returns></returns>
        private bool FoundAllLetters(bool[] cover)
        {
            int findCount = 0;
            //Count all false values in the array
            for (int i = 0; i < cover.Length; i++)
            {   
                //Increase counter if false
                if (cover[i].Equals(false))
                {
                    findCount++;
                }
            }
            //Counter should equal the length to return true
            if (findCount == cover.Length)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Validates the user's input and checks if there were any previous that were the same.
        /// </summary>
        /// <param name="storedInputs">List of successful inputs</param>
        /// <returns></returns>
        private char GetUserLetter(List<char> storedInputs)
        {
            char userInput;
            for (; ; )
            {
                //Get input, method already validates if it's a letter input
                userInput = InputValidation.ValidateLetter(LetterCase.Upper);
                //Cause user to input again if current input matches any previous inputs
                if (InputValidation.CheckAlreadyUsedInput(storedInputs, userInput))
                {
                    Console.WriteLine("You've alredy chosen this Letter!\nPlease enter a letter");
                }
                //Successful input
                else
                {
                    return userInput;
                }
            }
        }
        /// <summary>
        /// Indexer for hangman picture array
        /// </summary>
        public enum BodyPart
        {
            //Values obtained by iterating through the char array that makes up the picture
            //and outputting the char at the index.
            Head = 24,
            RightArm = 32,
            Body = 33,
            LeftArm = 34,
            RightLeg = 42,
            LeftLeg = 44
        }
    }
    /// <summary>
    /// Option for user to pick, affects pool of words that is chosen
    /// </summary>
    public enum MysteryTopic
    {
        Planet = 1,
        God,
    }
    /// <summary>
    /// Class contains the hidden word and its propertise to help with the game interaction.
    /// </summary>
    class HangManMysteryWord
    {
        //Chosen word that the user will have to try and reveal
        public string Word { get; private set; }
        //Cover that will hide each letter of the word
        public bool[] WordCover { get; set; }
        //Topic that the word belongs to
        public MysteryTopic Topic { get; private set; }
        private string[] wordBank = new string[9];
        private Random random = new Random();
        /// <summary>
        ///Default values for constructer, Call ChooseNewWord to change the properties
        /// </summary>
        public HangManMysteryWord()
        {
            Word = "MERCURY";
            WordCover = GenerateCover(Word.Length);
            Topic = MysteryTopic.Planet;
        }
        /// <summary>
        /// Change the properties of this class depending on the enum passed.
        /// </summary>
        /// <param name="chosenTopic"></param>
        public void ChooseNewWord(MysteryTopic chosenTopic)
        {
            switch (chosenTopic)
            {
                case MysteryTopic.Planet:
                    wordBank[0] = "MERCURY";
                    wordBank[1] = "VENUS";
                    wordBank[2] = "EARTH";
                    wordBank[3] = "MARS";
                    wordBank[4] = "JUPITER";
                    wordBank[5] = "SATURN";
                    wordBank[6] = "URANUS";
                    wordBank[7] = "NEPTUNE";
                    wordBank[8] = "PLUTO";
                    //Randomly select from word bank
                    Word = wordBank[random.Next(0, (wordBank.Length))];
                    //Cover size will be the length of the word 
                    WordCover = GenerateCover(Word.Length);
                    Topic = chosenTopic;
                    break;
                case MysteryTopic.God:
                    wordBank[1] = "HADES";
                    wordBank[2] = "APOLLO";
                    wordBank[3] = "ODIN";
                    wordBank[4] = "AEGIR";
                    wordBank[5] = "RAIJIN";
                    wordBank[6] = "AMATERASU";
                    wordBank[0] = "VISHNU";
                    wordBank[7] = "SHIVA";
                    wordBank[8] = "ANUBIS";
                    Word = wordBank[random.Next(0, (wordBank.Length))];
                    WordCover = GenerateCover(Word.Length);
                    Topic = chosenTopic;
                    break;
                default:
                    break;
            }
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

    }
}
