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
    public partial class Othello : Window, IPlayable.IPlayable
    {
        // UI properties
        private Window parent;
        private UIElement[,] buttons;
        private ButtonHandler buttonHandler;

        // Game Properties
        private static int TOTAL_ROW = 7;
        private static int TOTAL_COLLUMN = 9;

        public SquareStatus status { get; set; }

        private bool AIPlayer;

        private TimeHandler TimeHandlerContext;

        // Files properties
        private static string FILE_FORMAT = "Text file (*.txt)|*.txt";
        private static string DATETIME_FORMAT = "yyyy-MM-dd_HH-mm-ss";
        private static string NAME = "ArcOthelloAB";
        private static string AI_HEADER = "--AI--";
        private static string TIME_HEADER = "--Time--";
        private static string GAME_HEADER = "--Game--";
        private static string WHITE = "w";
        private static string BLACK = "b";
        private static string TIME_DIGITS_FORMAT = "D10";
        private static string MONOVALUE_FORMAT = "{0}:{1}";
        private static string SQUARE_FORMAT = "[{0},{1}]:{2}";

        /// <summary>
        /// Construct an Othello Game
        /// </summary>
        /// <param name="parent">parent Window</param>
        public Othello(Window parent, bool aiPlayer = false)
        {
            this.parent = parent;
            InitializeComponent();

            buttons = new UIElement[TOTAL_COLLUMN, TOTAL_ROW];
            buttonHandler = new ButtonHandler(this.GameGrid, buttons);

            TimeHandlerContext = new TimeHandler();

            DataContext = TimeHandlerContext;
            TimeHandlerContext.Start();

            if (aiPlayer)
            {
                AIPlayer = aiPlayer;
                // start the AI
            }
        }

        /// <summary>
        /// Construct an Othello Game from a game file
        /// </summary>
        /// <param name="parent">parent Window</param>
        /// <param name="filePath">path of the file game</param>
        public Othello(Window parent, string filePath) : this(parent)
        {
            try
            {
                LoadFromFile(filePath);
                // if the file registered an AI, start it
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                BtnBack_Click(null, null);
            }
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            // TODO show new window
        }

        private static string GenerateGameFileName()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ArcOthello_Game_");
            sb.Append(DateTime.Now.ToString(DATETIME_FORMAT));
            sb.Append(".txt");
            return sb.ToString();
        }

        private void BtnSave_Click(Object sender, RoutedEventArgs e)
        {
            TimeHandlerContext.Stop();
            string filePath = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Filter = FILE_FORMAT,
                FileName = GenerateGameFileName(),
            };
            if(saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
                MessageBox.Show(this, "Path : " + filePath);

                try
                {
                    SaveInFile(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message);
                }
            }
            else
            {
                TimeHandlerContext.Start();
            }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            TimeHandlerContext.Stop();
            string filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Filter = FILE_FORMAT,
            };
            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;

                MessageBox.Show(this, "Path : " + filePath);
                
                try
                {
                    LoadFromFile(filePath);
                    TimeHandlerContext.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message);
                }
            }
            else
            {
                TimeHandlerContext.Start();
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

        /// <summary>
        /// Load a game from a file
        /// <param name=path>path of the game file</param>
        /// <returns>success</returns>
        /// </summary>
        private void LoadFromFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            if (!ValidateFile(lines))
                throw new Exception("Invalid File");

            // TODO Parse file
            // TimeHandlerContext.TimePlayedWhite = xxxxx;
            // TimeHandlerContext.TimePlayedBlack = xxxxx;
        }

        private static bool ValidateFile(string[] content)
        {
            if (content.Length != 70)
                return false;
            if (!String.Equals(content[0], NAME))
                return false;
            if (!String.Equals(content[1], AI_HEADER))
                return false;
            if (!String.Equals(content[3], TIME_HEADER))
                return false;
            if (!String.Equals(content[6], GAME_HEADER))
                return false;

            // TODO Imporove validation

            return true;
        }

        /// <summary>
        /// Save a game in a specific file. Throws Exceptions
        /// <param name=path>path of the game file</param>
        /// <returns>success</returns>
        /// </summary>
        private void SaveInFile(string path)
        {
            StringBuilder sb = new StringBuilder();

            // saving the board
            //  Name
            sb.AppendLine(GetName());
            //  AI
            sb.AppendLine(AI_HEADER);
            sb.AppendFormat(MONOVALUE_FORMAT, "Enable", AIPlayer.ToString()).AppendLine();
            //  Time
            sb.AppendLine(TIME_HEADER);
            sb.AppendFormat(MONOVALUE_FORMAT, WHITE, TimeHandlerContext.TimePlayedWhite.ToString(TIME_DIGITS_FORMAT)).AppendLine();
            sb.AppendFormat(MONOVALUE_FORMAT, BLACK, TimeHandlerContext.TimePlayedBlack.ToString(TIME_DIGITS_FORMAT)).AppendLine();
            //  Game
            sb.AppendLine(GAME_HEADER);
            int[,] board = GetBoard();
            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    sb.AppendFormat(SQUARE_FORMAT, i, j, board[i, j]).AppendLine();
                }
            }

            System.IO.File.WriteAllText(path, sb.ToString());
        }

        /// <summary>
        /// Count the score of a pawn type
        /// <param name=pawnType>pawn type (WHITE_PAWN or BLACK_PAWN)</param>
        /// <returns>score of pawnType</returns>
        /// </summary>
        private int GetScore(SquareStatus pawnStatus)
        {
            return buttonHandler.GetScore(pawnStatus);
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
                    switch (buttonHandler.getStatus(i,j))
                    {
                        case SquareStatus.NoPawn:
                            intBoard[i, j] = 0;
                            break;
                        case SquareStatus.WhitePawn:
                            intBoard[i, j] = 1;
                            break;
                        case SquareStatus.BlackPawn:
                            intBoard[i, j] = 2;
                            break;
                    }
                }
            }
            return intBoard;
        }

        public string GetName()
        {
            return NAME;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        public int GetWhiteScore()
        {
            return GetScore(SquareStatus.WhitePawn);
        }

        public int GetBlackScore()
        {
            return GetScore(SquareStatus.BlackPawn);
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
