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
using System.Windows.Shapes;

namespace ArcOthelloAB
{
    /// <summary>
    /// Logique d'interaction pour Othello.xaml
    /// </summary>
    public partial class Othello : Window
    {
        private Window parent;

        private static int ROW_LENGHT = 7;
        private static int COLLUMN_LENGHT = 9;

        public static readonly DependencyProperty IsAvailableProperty =
            DependencyProperty.Register(
            "isAvailable", typeof(Boolean),
            typeof(Othello)
            );

        enum Status { NoPawn=0, BlackPawn=1, WhitePawn=2 };
        // Uses the Status enum for the state
        public static readonly DependencyProperty CurrentStatus =
            DependencyProperty.Register(
            "currentStatus", typeof(Status),
            typeof(Othello)
            );

        public Othello(Window parent)
        {
            this.parent = parent;
            InitializeComponent();

            setupButtons();
        }

        private void setupButtons()
        {
            for (int i = 0; i < COLLUMN_LENGHT; i++)
            {
                for (int j = 0; j < ROW_LENGHT; j++)
                {
                    Button btn = new Button();
                    btn.Content = i + " , " + j;
                    int btnX = i;
                    int btnY = j;
                    btn.Click += (sender, e) =>
                    {
                        ButtonAction1(btnX, btnY); // temporary
                    };
                    this.GameGrid.Children.Add(btn); // add button as children to gamegrid
                    Grid.SetRow(btn, j); // search the btn contained in a grid and setup a row position
                    Grid.SetColumn(btn, i); // setup a column
                }
            }

            // initiate property of each button
            foreach (UIElement child in this.GameGrid.Children)
            {
                child.SetValue(IsAvailableProperty, false);
                child.SetValue(CurrentStatus, Status.NoPawn);
            }

            // setup initial board
            GetButton(4, 4).SetValue(CurrentStatus, Status.WhitePawn);
            GetButton(5, 4).SetValue(CurrentStatus, Status.BlackPawn);
            GetButton(4, 5).SetValue(CurrentStatus, Status.BlackPawn);
            GetButton(5, 5).SetValue(CurrentStatus, Status.WhitePawn);
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO show new window
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            // TODO load a new game
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            parent.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            parent.Show();
        }

        /*
         * get an element of the board grid according to it's row and column
         */
        private UIElement GetButton(int column, int row)
        {
            Grid grid = this.GameGrid;
            foreach (UIElement child in grid.Children)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column)
                {
                    return child;
                }
            }
            return null;
        }

        /*
         * temporary
         */
        protected void ButtonAction1(int x, int y)
        {
            MessageBox.Show(this, "Hello, button : " + x + " , " + y);
            int xtemp = x+1;
            if (xtemp >= COLLUMN_LENGHT)
                xtemp = 0;
            UIElement adjascentButton = GetButton(xtemp, y);
            MessageBox.Show(this, "adjascent button " + xtemp + "," + y + " isAvailable?: " + adjascentButton.GetValue(IsAvailableProperty));
            adjascentButton.SetValue(IsAvailableProperty, true);
        }
    }
}
