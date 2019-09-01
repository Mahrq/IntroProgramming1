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

namespace u3204958_Games
{
    /// <summary>
    /// File:           MainWindow.xaml
    /// Description:    Interaction logic for MainWindow.xaml
    /// Author:         Mark Mendoza
    /// Date:           21/08/2019
    /// Version:        1.0
    /// Notes:
    /// TODO:           Make better exception handling methods.
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //Plays a game depending on what is selected in the drop box
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxItem item = (ComboBoxItem)GamesDropDown.SelectedItem;
                GameLauncher.LaunchGame(item);
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Environment.Exit(0);
            }
        }
        //Exit the application on clicking the quit button
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
