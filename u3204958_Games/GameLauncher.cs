using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace u3204958_Games
{
    /// <summary>
    /// File:           GameLauncher.cs
    /// Description:    Class containing methods that launch certain processes.
    /// Author:         Mark Mendoza
    /// Date:           21/08/2019
    /// Version:        1.0
    /// Notes:
    /// TODO:           Find better way to retern a file path.
    /// </summary>
    class GameLauncher
    {
        /// <summary>
        /// Starts a process within the project files.
        /// </summary>
        /// <param name="selectedItem">Drop down selected item.</param>
        public static void LaunchGame(ComboBoxItem selectedItem)
        {
            string path = GamePath(selectedItem);
            Process.Start(path);
        }
        /// <summary>
        /// Evaluates the selected item's name and returns a file path.
        /// </summary>
        /// <param name="selectedItem">Drop down selected item.</param>
        /// <returns></returns>
        static string GamePath(ComboBoxItem selectedItem)
        {
            if (selectedItem.Name == "MadLibs")
            {
                return @"Activity1.exe";
            }
            if (selectedItem.Name == "BullsCows")
            {
                return @"Activity2.exe";
            }
            if (selectedItem.Name == "SmileGrumpy")
            {
                return @"Activity3.exe";
            }
            if (selectedItem.Name == "RandomRacer")
            {
                return @"Activity4.exe";
            }
            if (selectedItem.Name == "Hangman")
            {
                return @"Activity5.exe";
            }
            return @"Activity1.exe";
        }
    }
}
