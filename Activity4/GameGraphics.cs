using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// File:           GameGraphics.cs
/// Description:    Class handles the rendering of the game and provides methods to update 
/// Author:         Mark Mendoza
/// Date:           20/08/2019
/// Version:        1.0
/// Notes:          Current draw method only accounts for 2 players.
/// TODO:           Change draw method to be able to take any amount of players.
/// </summary>
namespace Activity4
{
    class GameGraphics
    {
        private char[] graphicChars = { 'H', 'C', '-', '|' };
        private char[] humanTrack = new char[50];
        private char[] cpuTrack = new char[50];
        /// <summary>
        /// Draws the 2 tracks and divider using character arrays with parameters used for indexing to
        /// determine the position of the players.
        /// </summary>
        /// <param name="p1Pos">Player 1 position</param>
        /// <param name="p2Pos">Player 2 position</param>
        public void DrawGraphics(int p1Pos, int p2Pos)
        {
            Console.Clear();

            //Draw player1 track
            humanTrack = TrackFiller(humanTrack, GraphicsChar.Human, p1Pos);
            for (int i = 0; i < humanTrack.Length; i++)
            {
                Console.Write("{0}", humanTrack[i]);
            }
            //Draw the divider
            Console.WriteLine("\n");
            for (int i = 0; i < 51; i++)
            {
                if (i % 10 == 0)
                {
                    Console.Write("{0}", graphicChars[(int)GraphicsChar.Track10th]);
                }
                else
                {
                    Console.Write("{0}", graphicChars[(int)GraphicsChar.Track]);
                }
            }
            Console.WriteLine("\n");
            //Draw player2 track
            cpuTrack = TrackFiller(cpuTrack, GraphicsChar.Cpu, p2Pos);
            for (int i = 0; i < cpuTrack.Length; i++)
            {
                Console.Write("{0}", cpuTrack[i]);
            }
            Console.WriteLine("\n");
        }
        /// <summary>
        /// Return an array of characters that will help in drawing the track and the character's position
        /// </summary>
        /// <param name="track">char array that contains a player character</param>
        /// <param name="player">player entity </param>
        /// <param name="playerPosition">player position </param>
        /// <returns></returns>
        private char[] TrackFiller(char[] track, GraphicsChar player, int playerPosition)
        {
            for (int i = 0; i < track.Length; i++)
            {
                if (i == playerPosition)
                {
                    track[i] = graphicChars[(int)player];
                }
                else
                {
                    track[i] = graphicChars[(int)GraphicsChar.Track];
                }
            }
            return track;
        }
        //Indexer for the character array representing the game's graphics.
        enum GraphicsChar
        {
            Human,
            Cpu,
            Track,
            Track10th
        }
    }
}
