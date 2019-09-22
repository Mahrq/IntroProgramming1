using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.IO;
using System.Windows.Forms;
using ProjectUtility;
/// <summary>
/// File:           MadLibsGame.cs
/// Description:    Class stores the logic of the game's flow.
/// Author:         Mark Mendoza
/// Date:           22/08/2019
/// Version:        2.0
/// Notes:          Added voice narration and saving output to text file.
/// TODO:           Add more mature stories.
///                 Pretty up title screen.
/// </summary>
namespace Activity1
{
    public class MadLibsGame : IGameModel
    {
        private List<string> storedUserInput = new List<string>();
        private string finishedStory;
        private StoryMode selectedStory = StoryMode.General;
        private SpeechSynthesizer synth = new SpeechSynthesizer();
        private MadLibsStorySaver storySaver = new MadLibsStorySaver();
        private bool useVoice;
        /// <summary>
        /// Intro():
        ///     Introduce user to the game and provide instructions.
        ///     Default option to use voice narration from system but will check
        ///     if user has installed voices first.
        /// 
        /// Steps:
        ///     -Display game title and game instructions
        ///     -Check if the there are any system voices installed on the PC.
        ///     -Try use Microsoft Zira Desktop as system voice otherwise use the first installed voice in the PC.   
        /// </summary>
        public void Intro()
        {
            Console.WriteLine("Welcome to Mad Libs!\n" +
                "Answer the questions to generate a story.\n" +
                "Press enter to continue. ");
            //Check if there is any installed voices in the user's device
            if (synth.GetInstalledVoices().Count > 0)
            {
                //Enable voices for use.
                for (int i = 0; i < synth.GetInstalledVoices().Count; i++)
                {
                    synth.GetInstalledVoices()[i].Enabled = true;
                }
                //Defaut output to speakers or headphones
                synth.SetOutputToDefaultAudioDevice();
                //Zira is desired voice for this app
                try
                {
                    synth.SelectVoice("Microsoft Zira Desktop");
                    useVoice = true;
                }
                //Use the first voice in the list if Zira can't be found.
                catch (Exception)
                {
                    if (synth.GetInstalledVoices()[0] != null)
                    {
                        synth.SelectVoice(synth.GetInstalledVoices()[0].VoiceInfo.Name);
                        useVoice = true;
                    }
                    else
                    {
                        synth.Dispose();
                        useVoice = false;
                    }
                }
            }
            //Dispose synth to release resources if there are no installed voices.
            else
            {
                synth.Dispose();
                useVoice = false;
            }
            //System voice will narrate intro
            if (useVoice)
            {
                synth.SpeakAsync("Welcome to Mad Libs!" +
                    "Answer the questions to generate a story.");
                Console.ReadLine();
                synth.SpeakAsyncCancelAll();
            }
            else
            {
                Console.ReadLine();
            }
        }
        /// <summary>
        /// GameSetUp():
        ///     Set up the game for new round.
        /// 
        /// Steps:
        ///     -Clear list for new inputs
        ///     -Ask user to pick a story mode.
        /// </summary>
        public void GameSetUp()
        {
            Console.Clear();
            //Clear list of stored inputs for new round
            if (storedUserInput.Count > 0)
            {
                storedUserInput.Clear();
            }
            //Get user to pick from the 2 stories
            selectedStory = SelectStory();
            Console.Clear();
        }
        /// <summary>
        /// GameCycle():
        ///     Algorithim for one game cycle. The user will be asked a series of questions.
        /// 
        /// Arguments:
        ///     -A reference to a bool depicting a game loop.
        /// 
        /// Steps:
        ///     -Ask User a series of questions where each answer is stored in a list
        ///     -Ask user to continue after finalising the list.
        /// </summary>
        public void GameCycle(ref bool gameLoop)
        {
            storedUserInput = AskQuestionair(selectedStory);
            Console.WriteLine("\nPress enter to generate the story");
            Console.ReadLine();
            gameLoop = false;
        }
        /// <summary>
        /// GameConclusion():
        ///     Display the completed story with the user's answers while the system voice narrates the
        ///     story if enabled.
        ///     Also allows user the option of saving their story into their PC.
        /// 
        /// Steps:
        ///     -Display the Finished Story.
        ///     -Optional system voice will narrate the story.
        ///     -Ask user if they Want to save their story into a text file on their PC.
        /// </summary>
        public void GameConclusion()
        {
            //Generate the completed story using the answers the user inputted to fill the blanks
            finishedStory = GenerateStory(selectedStory, storedUserInput);
            Console.WriteLine(finishedStory);
            Console.WriteLine("\nPress enter to continue");
            //System voice will read out loud the generated story.
            if (useVoice)
            {
                synth.SpeakAsync(finishedStory);
                Console.ReadLine();
                synth.SpeakAsyncCancelAll();
            }
            else
            {
                Console.ReadLine();
            }
            //Asks user if they want to save their generated story into a text file.
            Console.WriteLine("Would you like to save your story? y/n");
            string input = InputValidation.ValidateYesNo();
            if (input == "y")
            {
                storySaver.SaveStory(finishedStory);
                Console.Clear();
            }
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
        /// AskQuestionair():
        ///     Asks the user a series of questions and store each answer into a list.
        /// 
        /// Arguments:
        ///     -Depending on the mode selected will determine which intructions are required of the user.
        /// 
        /// Steps:
        ///     -Create List
        ///     -Display Instructions
        ///     -Get user input.
        ///     -Add input into list.
        ///     -Repeat until all required inputs are supplied.
        ///     -Return the List.
        /// </summary>
        private List<string> AskQuestionair(StoryMode storyMode)
        {
            List<string> temp = new List<string>();
            string userAnswer;
            switch (storyMode)
            {
                case StoryMode.General:
                    Console.WriteLine("Please enter a thing (Plural)");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter an adjective");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a song title");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a celebrity");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a feeling");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a verb");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a place");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a food");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a thing (Plural)");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a name");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    return temp;
                case StoryMode.PG:
                    Console.WriteLine("Please enter a thing (Plural)");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter another thing (Plural)");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter an occupation");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a body part");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter an adjective");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter another adjective");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a Company name");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a verb");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a place");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter an adjective");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a place");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    Console.WriteLine("Please enter a verb (Past tense)");
                    userAnswer = InputValidation.ValidateInput();
                    temp.Add(userAnswer);
                    return temp;
                default:
                    return temp;
            }
        }
        /// <summary>
        /// GenerateStory():
        ///     Create the story filling in the blanks using the user's stored answers.
        ///     
        /// Arguments:
        ///     -Depending on the mode chosen will determine which story template is used.
        ///     -A list containing the users inputs to be used to fill the blanks.
        /// 
        /// Steps:
        ///     -Use List to replace the blanks in the template
        ///     -Return the full string.
        /// </summary>
        private string GenerateStory(StoryMode storyMode, List<string> storedInputs)
        {
            string temp;
            switch (storyMode)
            {
                case StoryMode.General:
                    return temp =
                    $"\nI just got back from a pizza party with {storedInputs[9]} " +
                    $"\nCan you believe we got to eat {storedInputs[1]} pizza in {storedInputs[6]}." +
                    $"\nEveryone got to choose their own toppings. I made '{storedInputs[7]} and {storedInputs[0]}' pizza, which is my favorite!" +
                    $"\nThey even stuffed the crust with {storedInputs[8]}. How {storedInputs[4]}." +
                    $"\nIf that wasn't good enough already, {storedInputs[3]} was there singing {storedInputs[2]}. " +
                    $"\nI was so inspired by the music, I had to get up out of my seat and {storedInputs[5]}.";
                case StoryMode.PG:
                    return temp = 
                    $"\nYou know you've made it when {storedInputs[6]} wants to shut you down." +
                    $"\nSo here are five {storedInputs[4]} tips I took from my days as a {storedInputs[2]} to get to where I am." +
                    $"\nOne. If the other {storedInputs[0]} jumped off the {storedInputs[10]}, do it better." +
                    $"\nTwo. Always wear {storedInputs[5]} underwear in case you're in {storedInputs[8]}." +
                    $"\nThree. When you get to my age, make sure you have {storedInputs[11]} at least once." +
                    $"\nFour. Remember, turning on the {storedInputs[1]} is illegal, unless you're {storedInputs[9]}" +
                    $"\nAnd five. Don't let other people {storedInputs[7]} your {storedInputs[3]}, do it yourself.";
                default:
                    return temp = "Error generating story.";
            }
        }
        /// <summary>
        /// SelectStory():
        ///     Ask user to pick a story mode
        /// 
        /// Steps:
        ///     -Display instructions to user.
        ///     -Get user input.
        ///     -Return the slected mode.
        /// </summary>
        private StoryMode SelectStory()
        {
            Console.WriteLine("Choose your story:" +
                "\n\n1 = Pizza Party (G)" +
                "\n2 = Tips For Success (PG)");
            int userInput = InputValidation.ValidateInput(1, 2);
            switch (userInput)
            {
                case 1:
                    return StoryMode.General;
                case 2:
                    return StoryMode.PG;
                default:
                    return StoryMode.General;
            }
        }
        /// <summary>
        /// Mode for story template
        /// </summary>
        private enum StoryMode
        {
            General,
            PG,
        }
        /// <summary>
        /// Set up a save file system for the game.
        /// </summary>
        private class MadLibsStorySaver
        {
            private SaveFileDialog saveFileDialog = new SaveFileDialog();
            /// <summary>
            /// Set up the default properties for the savefile dialog upon construction
            /// </summary>
            public MadLibsStorySaver()
            {
                saveFileDialog.FileName = "My Mad Libs Story";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.Filter = "Text files(*.txt) | *.txt";
                saveFileDialog.FilterIndex = 0;
                saveFileDialog.RestoreDirectory = true;
                saveFileDialog.OverwritePrompt = true;
            }
            /// <summary>
            /// SaveStory():
            ///     Opens a dialog box where the user can save their Mad Libs story
            ///     to where ever they want.
            ///     Saves it out as .txt file
            ///     
            /// Arguments:
            ///     -The given string will be what is written to the text file.
            /// 
            /// Steps:
            ///     -Open dialogue box.
            ///     -Get user to select a destination for the text file.
            ///     -Write contents into the text file.
            /// </summary>
            /// <param name="contents">Writes the contents into the file</param>
            public void SaveStory(string contents)
            {
                //Opens dialog box
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the filepath of where the user saved their item
                    string savePath = string.Format(@"{0}", saveFileDialog.FileName);
                    //Write to the item
                    using (StreamWriter streamWriter = new StreamWriter(savePath))
                    {
                        streamWriter.WriteLine(contents);
                        streamWriter.Close();
                    }
                }
            }
        }
    }
}
