using System;
/// <summary>
/// File:           GameGraphics.cs
/// Description:    Draws the hangman picture as well as cover the mystery Words
/// Author:         Mark Mendoza
/// Date:           25/08/2019
/// Version:        1.0
/// Notes:          
/// </summary>
namespace Activity5
{
    class GameGraphics
    {
        /// <summary>
        /// Draws the hangman to the console.
        /// </summary>
        /// <param name="hangmanPic">Array containing the hangman characters</param>
        /// <param name="hangManWord">The hidden word with various properties to help with drawing</param>
        public void DrawGraphics(char[] hangmanPic, HangManMysteryWord hangManWord)
        {
            //Extract properties from the class passed
            char[] mysteryWord = hangManWord.Word.ToCharArray();
            MysteryTopic mysteryTopic = hangManWord.Topic;
            bool[] mysteryWordCover = hangManWord.WordCover;
            //Clear console to redraw hangman
            Console.Clear();
            //Write each character in the hangman char array
            for (int i = 0; i < hangmanPic.Length; i++)
            {   
                Console.Write(hangmanPic[i]);
            }
            //Writes the clue on what the word is
            Console.WriteLine("\n\nTry to guess the mystery {0}\n", mysteryTopic.ToString());
            //Iterate through each character of the hidden word
            for (int i = 0; i < mysteryWord.Length; i++)
            {
                //if the cover is true at the index then it will hide the character
                if (mysteryWordCover[i])
                {
                    mysteryWord[i] = '_';
                }
                Console.Write(mysteryWord[i]);
            }
            Console.WriteLine("\n");
        }
    }
}
