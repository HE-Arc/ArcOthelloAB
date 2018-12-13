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

namespace ArcOthelloAB
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Button btn = new Button();
                    btn.Content = "ok" + i + j;
                    //btn.Click += (sender, e) => buttonAction1(sender, e, i,j);
                    int btnX = i;
                    int btnY = j;
                    btn.Click += (sender, e) =>
                    {
                        buttonAction1(btnX, btnY);
                    };
                    this.mainGrid.Children.Add(btn);
                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);
                }
            }
        }

        protected void buttonAction1(int x, int y)
        {
            MessageBox.Show(this, "Hello, button : " + x + " , " + y);
        }
    }
}
