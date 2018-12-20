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

        /*
         * Status = 0: has no pawn
         * Status = 1: has black pawn
         * Status = 2: has white pawn
         */
        public static readonly DependencyProperty CurrentStatus =
            DependencyProperty.Register(
            "currentStatus", typeof(int),
            typeof(Othello)
            );

        public Othello(Window parent)
        {
            this.parent = parent;
            InitializeComponent();

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
                        ButtonAction1(btnX, btnY);
                    };
                    this.GameGrid.Children.Add(btn);
                    Grid.SetRow(btn, j);
                    Grid.SetColumn(btn, i);
                }
            }
            foreach (UIElement child in this.GameGrid.Children)
            {
                child.SetValue(IsAvailableProperty, false);
            }
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

        private static UIElement GetChildren(Grid grid, int row, int column)
        {
            foreach (UIElement child in grid.Children)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column)
                {
                    return child;
                }
            }
            return null;
        }

        protected void ButtonAction1(int x, int y)
        {
            MessageBox.Show(this, "Hello, button : " + x + " , " + y);
            int xtemp = x+1;
            if (xtemp >= ROW_LENGHT)
                xtemp = 0;
            UIElement adjascentButton = GetChildren(this.GameGrid, xtemp, y);
            MessageBox.Show(this, "adjascent button " + xtemp + "," + y + " isAvailable?: " + adjascentButton.GetValue(IsAvailableProperty));
            adjascentButton.SetValue(IsAvailableProperty, true);
        }
    }
}
