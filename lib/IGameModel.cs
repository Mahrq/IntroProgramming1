using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectUtility
{
    /// <summary>
    /// File:           IGameModel.cs
    /// Description:    Interface set up to serve as a template for the game's structure.
    ///                 Providing methods that would store the game's logic and sequencing.
    /// Author:         Mark Mendoza.
    /// Date:           25/08/2019
    /// Notes:          
    /// </summary>
    public interface IGameModel
    {
        /// <summary>
        /// Intro():
        ///     Introduction behaviour of the game's code should go here.
        ///     Behaviours such as introducing the game title and game intructions to the user.
        /// </summary>
        void Intro();
        /// <summary>
        /// GameSetUp():
        ///     Set up behaviour for the game's code should go here.
        ///     Behaviours such as resetting required variables for a new game round.   
        /// </summary>
        void GameSetUp();
        /// <summary>
        /// GameCycle():
        ///     The logic and sequencing of the game's flow should go here.
        ///     Behaviours such as interacting with the user and doing game calculations.
        ///     Method should be placed in a while loop.
        ///     Method should have a way to change the condition of the referenced loop.
        /// 
        /// Arguments:
        ///     -Reference to the condition of the while loop that this method is in
        /// </summary>
        /// <param name="gameLoop"></param>
        void GameCycle(ref bool gameLoop);
        /// <summary>
        /// GameConclusion():
        ///     Conclusion behaviour after the game has ended. should go here.
        ///     Behaviours such as displaying the score and other game stats
        /// </summary>
        void GameConclusion();
        /// <summary>
        /// Outro():
        ///     Outro behaviour of the game's code should go here.
        ///     Behaviours that need to run before terminating the program.
        /// </summary>
        void Outro();
    }
}
