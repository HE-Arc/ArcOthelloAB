﻿using System;
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
    }
}
