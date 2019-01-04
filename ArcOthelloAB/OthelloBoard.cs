using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPlayable;

namespace ArcOthelloAB
{
    class OthelloBoard : IPlayable.IPlayable
    {
        private static OthelloBoard instance;

        private static int TOTAL_ROW = 7;
        private static int TOTAL_COLLUMN = 9;

        private static int NO_PAWN = 0;
        private static int WHITE_PAWN = 1;
        private static int BLACK_PAWN = 2;

        private int[,] Board;

        private OthelloBoard()
        {
            Board = new int[TOTAL_COLLUMN, TOTAL_ROW];

        }

        public static OthelloBoard getInstance()
        {
            if(instance == null)
            {
                instance = new OthelloBoard();
            }
            return instance;
        }

        ///
        /// Public functions
        ///

        /// <summary>
        /// Load a game from a file
        /// <param name=path>path of the game file</param>
        /// <returns>success</returns>
        /// </summary>
        public bool LoadFromFile(string path)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save a game in a specific file
        /// <param name=path>path of the game file</param>
        /// <returns>success</returns>
        /// </summary>
        public bool SaveInFile(string path)
        {
            throw new NotImplementedException();
        }

        ///
        /// Private functions
        ///

        /// <summary>
        /// Count the score of a pawn type
        /// <param name=pawnType>pawn type (WHITE_PAWN or BLACK_PAWN)</param>
        /// <returns>score of pawnType</returns>
        /// </summary>
        private int GetScore(int pawnType)
        {
            int score = 0;
            foreach (var item in Board)
            {
                if (item == pawnType)
                    score++;
            }
            return score;
        }

        ///
        /// IPlayable functions
        ///

        public int[,] GetBoard()
        {
            return Board;
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
            return GetScore(WHITE_PAWN);
        }

        public int GetBlackScore()
        {
            return GetScore(BLACK_PAWN);
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
