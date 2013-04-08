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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SevenWondersGUI
{
    /// <summary>
    /// Interaction logic for ResourceBuying.xaml
    /// </summary>
    public partial class ResourceWindow : Window
    {
        Resources rc;

        public ResourceWindow(Resources res)
        {
            this.rc = res;
            InitializeComponent();
            createButtons();
            createValueFields();            
        }
        //when i = 0:you, i=1:left, i=2:right
        private void createButtons()
        {
            int skip = 0;
            int[] currentTotals = new int[] { 0, 0, 0, 0, 0, 0, 0 };

            for(int i =0; i < 3; i++)
            {
                currentTotals = getCurrentTotals(i);
                for (int j = 0; j < 7; j++)
                {
//                    System.Console.WriteLine("Current Resource Total: " + currentTotals[j]+ " i = " + i);
                    Button b = new Button();
                    b.Content = "+";
                    b.Width   = 25;
                    b.Height = 27;
                    Grid.SetColumn(b, j);
                    Grid.SetRow(b, (i+1+skip));
                    if (currentTotals[j] > 0)
                    {
                        b.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        b.Visibility = Visibility.Hidden;
                    }
                    mainGrid.Children.Add(b);
                }
                skip += 1;
            }
        }
        //when i = 0:you, i=1:left, i=2:right
        private void createValueFields()
        {
            int skip = 0;
            int[] currentTotals;

            for (int i = 0; i < 3; i++)
            {
                currentTotals = getCurrentTotals(i);
                for (int j = 0; j < 7; j++)
                {
                    Label l = new Label();

                    l.Content = currentTotals[j];
                    l.Width = 25;
                    l.Height = 27;
                    Grid.SetColumn(l, j);
                    Grid.SetRow(l, (i + 2 + skip));
                    mainGrid.Children.Add(l);
                }
                skip += 1;
            }
        }

        //when i = 0:you, i=1:left, i=2:right
        private int[] getCurrentTotals(int i)
        {
            if (i == 0)
            {
                return rc.getCenter();
            }
            else
            if (i == 1)
            {
                return rc.getLeft();
            }
            else
            {
                return rc.getRight();
            }
        }
    }
}
