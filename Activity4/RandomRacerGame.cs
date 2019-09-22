using System;
using System.Threading;
using ProjectUtility;
/// <summary>
/// File:           RandomRacerGame.cs
/// Description:    Class stores the logic of the game's flow.
/// Author:         Mark Mendoza
/// Date:           31/08/2019
/// Version:        2.0
/// Notes:          Added gambling mechanic, where you place a bet on who wins instead of
///                 always siding with the human character.
/// TODO:           
/// </summary>
namespace Activity4
{
    public class RandomRacerGame : IGameModel
    {
        private int gamesPlayed = 0;
        private RandomRacerPlayer[] racers = new RandomRacerPlayer[2];
        private int[] racerPositions = new int[2];
        private WinScenario winnerOutcome;
        private WinScenario playerBetOn;
        private int playerMoneyPool = 1000;
        private int playerBetAmount;
        private bool hasMoney = true;
        private bool currentlyRacing = false;
        private int finishLine = 49;
        private GameGraphics gameGraphics = new GameGraphics();
        private Random random = new Random();
        /// <summary>
        /// Construct class to initialise players and cache their positions into an array.
        /// </summary>
        public RandomRacerGame()
        {
            for (int i = 0; i < racers.Length; i++)
            {
                racers[i] = new RandomRacerPlayer();
                racerPositions[i] = racers[i].Position;
            }
        }
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
            Console.WriteLine("Welcome To Random Racer!" +
                        "\nPlace your bet on Human or Cpu and hope you win Big Money" +
                        "\nGood Luck!" +
                        "\n\nPress enter to continue");
            Console.ReadLine();
        }
        /// <summary>
        /// GameSetUp():
        ///     Set up the necessary components before starting a new round.
        ///     
        /// Steps:
        ///     -If player has money then Set up.
        ///     -Reset both player's positions.
        ///     -Ask user to make a bet on a player.
        ///     -Ask user to input an amount to bet.
        ///     -Subtract bet amount from money pool.
        ///     -Draw the game.
        ///     -Ask user to start game.
        ///     -Else proceed to exit the game.
        /// </summary>
        public void GameSetUp()
        {
            Console.Clear();
            //Set all player's position to 0.
            if (hasMoney)
            {
                for (int i = 0; i < racers.Length; i++)
                {
                    racerPositions[i] = 0;
                    /*Optional: Randomise min and max step of each player*/
                    //racers[i].RandomiseStepRange();
                }
                //Ask player to make a bet
                Console.WriteLine("Place your bet!" +
                    "\n1 = Human" +
                    "\n2 = Cpu");
                playerBetOn = (WinScenario)InputValidation.ValidateInput(1, 2);
                Console.WriteLine("How much do you want to bet?" +
                    "\nYour Balance: {0:C}", playerMoneyPool);
                playerBetAmount = InputValidation.ValidateInput(1, playerMoneyPool);
                //Subtract bet from moeny pool
                playerMoneyPool -= playerBetAmount;
                //Draw the game.
                gameGraphics.DrawGraphics(racerPositions[(int)Entity.Human], racerPositions[(int)Entity.Cpu]);
                Console.WriteLine("\nBetting On: {0}\t\tStaking:{1:C}", playerBetOn, playerBetAmount);
                Console.WriteLine("\nPress enter to start the race");
                Console.ReadLine();
                currentlyRacing = true;
            }
            else
            {
                Console.WriteLine("You have no more money!");
                hasMoney = false;

            }

        }
        /// <summary>
        /// GameCycle():
        ///     Algorithim for one game cycle. The game plays automatically until a winner is declared.
        ///     
        /// Arguments:
        ///     -A reference to a bool depicting a game loop.
        ///     
        /// Steps:
        ///     1# Draw Cycle
        ///     -Every 1 second update game.
        ///     -Randomly generate the next step for each player.
        ///     -Redraw the game with the updated positions.
        ///     -Check if any player reached the finish line.
        ///     
        ///     2# Pay out
        ///     -If the player that reaches the finish line matches the user's bet
        ///     then pay out 2x the amount bet.
        ///     -If both players reach the finish line then the match is a draw,
        ///     Pay back the amount the user bet.
        ///     -Else player loses bet.
        ///     -Terminate loop.
        /// </summary>
        public void GameCycle(ref bool gameLoop)
        {
            if (hasMoney)
            {
                while (currentlyRacing)
                {
                    Thread.Sleep(1000);
                    //Generate the next step the players take
                    for (int i = 0; i < racerPositions.Length; i++)
                    {
                        racerPositions[i] += random.Next(racers[i].StepMin, racers[i].StepMax);
                    }
                    //Prevent overflowing from the finish line
                    for (int i = 0; i < racerPositions.Length; i++)
                    {
                        if (racerPositions[i] >= finishLine)
                        {
                            racerPositions[i] = finishLine;
                        }
                    }
                    //Refresh the new positions of the players
                    gameGraphics.DrawGraphics(racerPositions[(int)Entity.Human], racerPositions[(int)Entity.Cpu]);
                    Console.WriteLine("\nBetting On: {0}\t\tStaking:{1:C}", playerBetOn, playerBetAmount);
                    //Determine winner
                    currentlyRacing = DetermineRaceWinner(finishLine, racerPositions, ref winnerOutcome);
                    
                }
                //Player recieves double their bet for winning
                if (playerBetOn.Equals(winnerOutcome))
                {
                    playerMoneyPool += playerBetAmount * 2;
                    Console.WriteLine("\nCongratulations! You Win {0:C}", (playerBetAmount * 2));
                    //Prevent integer overflow by capping money at 1 billion - 1
                    if (playerMoneyPool >= 999999999)
                    {
                        playerMoneyPool = 999999999;
                    }
                }
                //Player gets their bet back for draw
                else if (winnerOutcome == WinScenario.Draw)
                {
                    playerMoneyPool += playerBetAmount;
                    Console.WriteLine("\nIt's a Draw!");
                }
                //Player loses and doesn't get money back
                else
                {
                    Console.WriteLine("\nUnlucky! You Lose!");
                }
            } 
            gameLoop = false;
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
            if (hasMoney)
            {
                gamesPlayed++;
                Console.WriteLine("\nRace No. {0} Results\n" +
                    "Human: {1}\n" +
                    "Cpu: {2}",
                    gamesPlayed.ToString(),
                    racers[(int)Entity.Human].Score.ToString(),
                    racers[(int)Entity.Cpu].Score.ToString());
                Console.ReadLine();
                if (playerMoneyPool <= 0)
                {
                    hasMoney = false;
                }
            }
        }
        /// <summary>
        /// Outro():
        ///     Thank user for playing before exiting the application.
        ///     
        /// Steps:
        ///     -Display exiting message.
        ///     -Wait input from user.
        ///     -If exiting with 0 money, then launch website.
        /// </summary>
        public void Outro()
        {
            if (hasMoney)
            {
                Console.Clear();
                Console.WriteLine("Thanks for playing!\nPress enter to exit the game");
                Console.ReadLine();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Thanks for playing!\nPress enter to exit the game and get help");
                Console.ReadLine();
                System.Diagnostics.Process.Start("https://www.gamblinghelponline.org.au/");
            }

        }
        /// <summary>
        /// Determine whether there is a winner or not in the current cycle of the game.
        /// </summary>
        /// <param name="goal">Goal that would indicate a winner</param>
        /// <param name="playerPositions">positions that would be compared to the goal </param>
        /// <returns></returns>
        private bool DetermineRaceWinner(int goal, int[] playerPositions, ref WinScenario winnerOfRace)
        {
            //Count the number of players that reach the goal, if more than 1 then it's a draw.
            int reachedGoalCount = 0;
            for (int i = 0; i < playerPositions.Length; i++)
            {
                if (playerPositions[i].Equals(goal))
                {
                    reachedGoalCount++;
                }
            }
            //Draw
            if (reachedGoalCount > 1)
            {                
                winnerOfRace = WinScenario.Draw;
                return false;
            }
            //Human wins
            else if (playerPositions[(int)Entity.Human].Equals(goal))
            {                
                racers[(int)Entity.Human].Score++;
                winnerOfRace = WinScenario.Human;
                return false;
            }
            //Cpu wins
            else if (playerPositions[(int)Entity.Cpu].Equals(goal))
            {
                racers[(int)Entity.Cpu].Score++;
                winnerOfRace = WinScenario.Cpu;
                return false;
            }
            //Continue
            else
            {
                return true;
            }
        }
        /// <summary>
        /// Indexer for players
        /// </summary>
        private enum Entity
        {
            Human,
            Cpu
        }
        /// <summary>
        /// The various win scenarios in each loop of the race;
        /// </summary>
        private enum WinScenario
        {
            Human = 1,
            Cpu,
            Draw,
        }
        /// <summary>
        /// Class stores the properties needed for each character to function in the game 
        /// </summary>
        private class RandomRacerPlayer
        {
            public int Score { get; set; }
            public int Position { get; set; }
            public int StepMin { get; private set; }
            public int StepMax { get; private set; }
            private Random random = new Random();
            /// <summary>
            /// Sets default value of the class when constructed.
            /// </summary>
            public RandomRacerPlayer()
            {
                Score = 0;
                Position = 0;
                StepMin = 1;
                StepMax = 10;
            }
            /// <summary>
            /// Randomise the stepping range of the character. Best used on new round of the game.
            /// </summary>
            public void RandomiseStepRange()
            {
                StepMin = random.Next(0, 4);
                StepMax = random.Next(6, 10);
            }
        }
    }
}
