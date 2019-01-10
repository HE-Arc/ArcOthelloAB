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

        // AI properties
        private bool AIPlayer;

        // Timer properties
        private TimeHandler TimeHandlerContext;

        // Score properties
        // TODO Score implementation

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

            // Timer
            TimeHandlerContext = new TimeHandler();
            DataContext = TimeHandlerContext;

            // Generate the board
            buttons = new Button[TOTAL_COLLUMN, TOTAL_ROW];
            buttonHandler = new ButtonHandler(this, this.GameGrid, buttons, TimeHandlerContext);

            // Active (or not) the AI
            if (aiPlayer)
            {
                AIPlayer = aiPlayer;
                // TODO start the AI
            }

            //Start the timer
            TimeHandlerContext.Start();
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

                // Active (or not) the AI
                if (AIPlayer)
                {
                    // TODO start the AI
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
                BtnBack_Click(null, null);
            }
        }

        /// <summary>
        /// Open the settings window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Saving the current game action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(Object sender, RoutedEventArgs e)
        {
            TimeHandlerContext.Stop();
            
            // Configure the file format
            string filePath = string.Empty;
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Filter = FILE_FORMAT,
                FileName = GenerateGameFileName(),
            };

            // User select where to register its game file
            if(saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;

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
                // Continue the current game
                TimeHandlerContext.Start();
            }
        }

        /// <summary>
        /// Loading a file action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            TimeHandlerContext.Stop();

            // Configure file formate
            string filePath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                Filter = FILE_FORMAT,
            };

            // User select a file
            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                
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
                // Continue the current game
                TimeHandlerContext.Start();
            }
        }

        /// <summary>
        /// Window back button action, clossing the event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            // TODO ask user if he wants to save the game
            parent.Show();
            this.Close();
        }
        /// <summary>
        /// Window closing action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            // AI
            string aiLine = lines[2];
            string aiPrefix = String.Format(MONOVALUE_FORMAT, "Enable", "");
            AIPlayer = Convert.ToBoolean(aiLine.Substring(aiPrefix.Length));

            // Time
            string timeWhiteLine = lines[4];
            string timeWhitePrefix = string.Format(MONOVALUE_FORMAT, WHITE, "");
            TimeHandlerContext.TimePlayedWhite = Convert.ToInt32(timeWhiteLine.Substring(timeWhitePrefix.Length));
            string timeBlackLine = lines[5];
            string timeBlackPrefix = string.Format(MONOVALUE_FORMAT, BLACK, "");
            TimeHandlerContext.TimePlayedBlack = Convert.ToInt32(timeBlackLine.Substring(timeBlackPrefix.Length));

            // GAME
            int x, y, squareValue = 0;
            SquareStatus squareStatus;
            foreach (var item in lines.Skip(7))
            {
                // Parse the line
                x = Convert.ToInt32(item.Substring(1, 1));
                y = Convert.ToInt32(item.Substring(3, 1));
                squareValue = Convert.ToInt32(item.Substring(6, 1));
                
                switch(squareValue)
                {
                    case 1:
                        squareStatus = SquareStatus.WhitePawn;
                        break;
                    case 2:
                        squareStatus = SquareStatus.BlackPawn;
                        break;
                    default:
                        squareStatus = SquareStatus.NoPawn;
                        break;
                }

                // Assign the pawn value to the corresponding button
                buttonHandler.SetButtonState(x, y, squareStatus);
            }
        }

        /// <summary>
        /// Validate a game file content
        /// </summary>
        /// <param name="content">file content as a string tab</param>
        /// <returns>if the file is valide or not</returns>
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

        /*
            IPlayable functions
        */

        /// <summary>
        /// Get the game board as a two dimensional int array
        /// </summary>
        /// <returns>the game board as a two dimensional int array</returns>
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

        /// <summary>
        /// Get the name of the game
        /// </summary>
        /// <returns>The name of the game</returns>
        public string GetName()
        {
            return NAME;
        }

        /// <summary>
        /// Give the best next move for the AI to play considering the board game,
        /// the depth of search in the tree of possibles moves and which color of
        /// pawn the AI owns 
        /// </summary>
        /// <param name="game">Game board</param>
        /// <param name="level">Depth of search for the algorithm</param>
        /// <param name="whiteTurn">True if the pawns are white, False if the pawns are black</param>
        /// <returns></returns>
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Give the score of the white pawns owner
        /// </summary>
        /// <returns>score of the white pawns owner</returns>
        public int GetWhiteScore()
        {
            return GetScore(SquareStatus.WhitePawn);
        }

        /// <summary>
        /// Give the score of the black pawns owner
        /// </summary>
        /// <returns>score of the black pawns owner</returns>
        public int GetBlackScore()
        {
            return GetScore(SquareStatus.BlackPawn);
        }

        /// <summary>
        /// Indicate if the square at the given coordonate is playable or not considering the pawn color
        /// </summary>
        /// <param name="column">column of the square</param>
        /// <param name="line">line of the square</param>
        /// <param name="isWhite">True if the pawn color is white, False if the pawn color is black</param>
        /// <returns>if the move is valid or not</returns>
        public bool IsPlayable(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Place a pawn in the square at the given coordonate and return if the move is accepted or not
        /// </summary>
        /// <param name="column">column of the square</param>
        /// <param name="line">line of the square</param>
        /// <param name="isWhite">True if the pawn color is white, False if the pawn color is black</param>
        /// <returns>if the move is accepted or not</returns>
        public bool PlayMove(int column, int line, bool isWhite)
        {
            throw new NotImplementedException();
        }
    }
}
