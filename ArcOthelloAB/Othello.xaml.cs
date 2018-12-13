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

        public Othello(Window parent)
        {
            this.parent = parent;
            InitializeComponent();

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Button btn = new Button();
                    btn.Content = i + " , " + j;
                    int btnX = i;
                    int btnY = j;
                    btn.Click += (sender, e) =>
                    {
                        buttonAction1(btnX, btnY);
                    };
                    this.GameGrid.Children.Add(btn);
                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);
                }
            }
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

        protected void buttonAction1(int x, int y)
        {
            MessageBox.Show(this, "Hello, button : " + x + " , " + y);
        }
    }
}
