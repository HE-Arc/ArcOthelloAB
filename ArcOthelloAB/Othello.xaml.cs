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
using Microsoft.Win32;
using System.ComponentModel;

namespace ArcOthelloAB
{
    /// <summary>
    /// Logique d'interaction pour Othello.xaml
    /// </summary>
    public partial class Othello : Window, IPlayable.IPlayable, INotifyPropertyChanged
    {
        private Window parent;

        private static int TOTAL_ROW = 7;
        private static int TOTAL_COLLUMN = 9;

        private UIElement[,] buttons;

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

        private int timePlayedBlack;    // Seconds
        public int TimePlayedBlack
        {
            get { return timePlayedBlack; }
            set { timePlayedBlack = value; RaisePropertyChanged("TimePlayedBlack"); }
        }

        private int timePlayedWhite;    // Seconds
        public int TimePlayedWhite
        {
            get { return timePlayedWhite; }
            set { timePlayedWhite = value; RaisePropertyChanged("TimePlayedWhite"); }
        }

        void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Construct an Othello Game
        /// </summary>
        /// <param name="parent">parent Window</param>
        public Othello(Window parent)
        {
            this.parent = parent;
            InitializeComponent();

            setupButtons();

            timePlayedWhite = 0;
            timePlayedBlack = 0;
        }

        /// <summary>
        /// Construct an Othello Game from a game file
        /// </summary>
        /// <param name="parent">parent Window</param>
        /// <param name="filePath">path of the file game</param>
        public Othello(Window parent, string filePath) : this(parent)
        {
            //LoadFromFile(filePath);
        }

        private void setupButtons()
        {
            buttons = new UIElement[TOTAL_COLLUMN, TOTAL_ROW];

            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
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

                    buttons[btnX, btnY] = GetButton(btnX, btnY);
                }
            }

            // initiate property of each button
            foreach (UIElement child in this.GameGrid.Children)
            {
                child.SetValue(IsAvailableProperty, false);
                child.SetValue(CurrentStatus, Status.NoPawn);
            }

            // setup initial board
            buttons[3, 3].SetValue(CurrentStatus, Status.WhitePawn);
            buttons[4, 3].SetValue(CurrentStatus, Status.BlackPawn);
            buttons[3, 4].SetValue(CurrentStatus, Status.BlackPawn);
            buttons[4, 4].SetValue(CurrentStatus, Status.WhitePawn);

            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    if((Status)buttons[i,j].GetValue(CurrentStatus) == Status.NoPawn)
                        UpdateButtonAvailability(i, j);
                }
            }
        }
        /*
         * get an UIElement of the board grid according to it's row and column
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

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO show new window
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // TODO Change settings
                InitialDirectory = "c:\\",
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };
            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;

                MessageBox.Show(this, "Path : " + filePath);

                //othelloBoard.LoadFromFile(filePath);
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


        /*
         * temporary
         */
        protected void ButtonAction1(int x, int y)
        {
            UIElement currentutton = buttons[x, y];
            MessageBox.Show(this, "Hello, button : " + x + " , " + y + "  I am : " + currentutton.GetValue(CurrentStatus));
            MessageBox.Show(this, " can be played? : " + currentutton.GetValue(IsAvailableProperty));
        }

        /*
         * Coordonate of square to check ar given as argument
         * 
         * Will check in every direction to see if this square is playable
         * Will then update his status according to the result of the search
         */
        private void UpdateButtonAvailability (int x, int y)
        {
            int[,] directionToCheck = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            bool isPlayable = false;

            for (int i = 0; i<8; i++)
            {
                int dirX = directionToCheck[i, 0];
                int dirY = directionToCheck[i, 1];
                if (CheckOtherPawnFromDirection(dirX, dirY, x, y, Status.BlackPawn))
                    isPlayable = true;
            }
            if (isPlayable)
                buttons[x,y].SetValue(IsAvailableProperty, true);
            else
                buttons[x, y].SetValue(IsAvailableProperty, false);
        }

        /**
         * Will check in a given direction if the a pawn can be placed on the current empty square
         * 
         * The direction is given by a movement in x and y: dirX and dirY argument
         * These value should be {-1,0,1} with at least one of the two not set at 0
         * 
         * the position of empty square is given by x and y argument
         */
        private bool CheckOtherPawnFromDirection (int dirX, int dirY, int x, int y, Status currentStatus)
        {
            if (dirX == 0 && dirY == 0)
                return false;
            x += dirX; // verification will advance in the given direction
            y += dirY;

            // if the next square is out of the grid
            if (x < 0 || x >= TOTAL_COLLUMN || y < 0 || y >= TOTAL_ROW)
                return false;
            UIElement currentButton = buttons[x, y];

            // get the color of the current ennemy
            Status opponentStatus;
            if (currentStatus == Status.BlackPawn)
                opponentStatus = Status.WhitePawn;
            else
                opponentStatus = Status.BlackPawn;

            Status nextPawnStatus = (Status)currentButton.GetValue(CurrentStatus);

            bool hasAtLeastOneEnnemy = false;

            // as long as there is only ennemy pawn alligned
            while (nextPawnStatus == opponentStatus)
            {
                hasAtLeastOneEnnemy = true;
                x += dirX;
                y += dirY;

                if (x < 0 || x >= TOTAL_COLLUMN || y < 0 || y >= TOTAL_ROW)
                    return false;
                currentButton = buttons[x, y];
                nextPawnStatus = (Status)currentButton.GetValue(CurrentStatus);
            }

            // when a checked pawn isn't ennemy pawn, if it one of his pawn, he can place his pawn in the checked square
            if (nextPawnStatus == currentStatus && hasAtLeastOneEnnemy)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Load a game from a file
        /// <param name=path>path of the game file</param>
        /// <returns>success</returns>
        /// </summary>
        private bool LoadFromFile(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save a game in a specific file
        /// <param name=path>path of the game file</param>
        /// <returns>success</returns>
        /// </summary>
        private bool SaveInFile(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Count the score of a pawn type
        /// <param name=pawnType>pawn type (WHITE_PAWN or BLACK_PAWN)</param>
        /// <returns>score of pawnType</returns>
        /// </summary>
        private int GetScore(Status pawnStatus)
        {
            int score = 0;
            foreach (var button in buttons)
            {
                if ((Status)button.GetValue(CurrentStatus) == pawnStatus)
                    score++;
            }
            return score;
        }

        ///
        /// IPlayable functions
        ///

        public int[,] GetBoard()
        {
            int[,] intBoard = new int[TOTAL_COLLUMN, TOTAL_ROW];

            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    switch ((Status)buttons[i, j].GetValue(CurrentStatus))
                    {
                        case Status.NoPawn:
                            intBoard[i, j] = 0;
                            break;
                        case Status.WhitePawn:
                            intBoard[i, j] = 1;
                            break;
                        case Status.BlackPawn:
                            intBoard[i, j] = 2;
                            break;
                    }
                }
            }
            return intBoard;
        }

        public string GetName()
        {
            return "OthelloBoard";
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        public int GetWhiteScore()
        {
            return GetScore(Status.WhitePawn);
        }

        public int GetBlackScore()
        {
            return GetScore(Status.BlackPawn);
        }

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }
    }
}
