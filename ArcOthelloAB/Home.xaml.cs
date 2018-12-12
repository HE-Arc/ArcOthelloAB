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
    /// Logique d'interaction pour Home.xaml
    /// </summary>
    public partial class Home : Window
    {
        public Home()
        {
            InitializeComponent();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            new Othello(this).Show();
            Hide();
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void BtnPlayAI_Click(object sender, RoutedEventArgs e)
        {
            // TODO
        }

        private void BtnCredits_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Game made by Mme. Kim Aurore Biloni and M. Paul Arzul for the .NET course at HE-Arc Ingénierie in Neuchâtel (Fall 2018).", "Credits", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
