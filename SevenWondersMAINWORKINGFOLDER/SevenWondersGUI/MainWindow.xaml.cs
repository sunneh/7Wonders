using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using System.Net.Sockets;
using System.Net;


namespace SevenWondersGUI
{
    // Interaction logic for MainWindow.xaml

    /*
    <Menu Margin="0,0,0,736" Panel.ZIndex="10" Foreground="#FFB94242">
            <Menu.Background>
                <SolidColorBrush />
            </Menu.Background>
            <MenuItem Header="Rules" Name="Menu" Click="rules_Click" Foreground="#FF074E8D">
                <MenuItem Header="S_how Rules"  />
            </MenuItem>
        </Menu>

     * 
    */
    // Main insertion for program
    public partial class MainWindow : Window
    {        

        public MainWindow()
        {            
            InitializeComponent();
            //ScoreWindow s = new ScoreWindow();
            
            StartGameCanvas start = new StartGameCanvas(MainGrid);
        }
        public Grid getGrid()
        {
            return this.MainGrid;
        }

        private void rules_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
        }

        private void mainWindow_Closed(object sender, EventArgs e)
        {
            //ResourceManager.GetInstance().Reset();
            ResourceBuying.GetInstance().reset();
            ResourceBuying.GetInstance().Close();
        }
    }    
}


