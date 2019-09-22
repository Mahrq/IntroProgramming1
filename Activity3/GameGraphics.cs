using System;
namespace Activity3
{
    /// <summary>
    /// File:           GameGraphics.cs
    /// Description:    Class handles rendering of the game grid.
    /// Author:         Mark Mendoza.
    /// Date:           24/08/2019
    /// Notes:          
    /// </summary>
    class GameGraphics
    {
        string[] emoticons = { ":-D", ">:c" };

        int item = 1;   //For displaying numerical purposes
        int itemCounter = 0; //Indexer
        int rows = 3;   //Grid property
        int column = 3; //Grid property

        /// <summary>
        /// Draws the 3x3 grid of the game.
        /// </summary>
        /// <param name="cover">Array acts as a cover to hide items in the grid.</param>
        /// <param name="faceCover">Array that arranges the smileys and grumpys into the grid.</param>
        public void DrawGrid(bool[] cover, Face[] faceCover)
        {
            //Clear console before drawing new grid
            Console.Clear();
            item = 1;
            itemCounter = 0;
            for (int i = 0; i < column; i++)
            {
                //Draws row
                for (int j = 0; j < rows; j++)
                {
                    //If the cover at the index is true then hide the item
                    if (cover[itemCounter])
                    {
                        Console.Write("\t{0}", item.ToString());
                    }
                    //Display the face at the current grid depending on the index of the face array.
                    else
                    {
                        Console.Write("\t{0}", emoticons[(int)faceCover[itemCounter]]);
                    }
                    item++;
                    itemCounter++;
                    //New line space for the next row.
                    if (j == rows - 1)
                    {
                        Console.WriteLine("\n");
                    }
                }
            }
        }
    }
}
