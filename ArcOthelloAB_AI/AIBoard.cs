// ===============================
// AIBoard
//
// PROJECT: ArcOthelloAB_AI
// AUTHORS: Arzul Paul & Biloni Kim Aurore
// DATE: 17.02.19
// ORGANISATION: HE-Arc Ingénierie, Neuchâtel
// ===============================

using System;

namespace ArcOthelloAB_AI
{
    class AIBoard : IPlayable.IPlayable
    {
        private static readonly string NAME = "ArcOthelloAB";

        private int[,] board;

        // Game Properties
        private static int TOTAL_ROW = 7;
        private static int TOTAL_COLLUMN = 9;

        private OthelloAI_AB othelloAI;

        /// <summary>
        /// Constructor of AIBoard
        /// </summary>
        public AIBoard()
        {
            board = new int[TOTAL_COLLUMN, TOTAL_ROW];
            othelloAI = OthelloAI_AB.GetInstance();
            FillBoard();
        }

        /// <summary>
        /// Give the actual board state
        /// </summary>
        /// <returns></returns>
        public int[,] GetBoard()
        {
            return board;
        }

        /// <summary>
        /// Give the name of the board
        /// </summary>
        /// <returns>String name of the board</returns>
        public string GetName()
        {
            return NAME;
        }

        /// <summary>
        /// Give the next move to play for a player using an AI
        /// </summary>
        /// <param name="game">actual board game</param>
        /// <param name="level">level of search deep</param>
        /// <param name="whiteTurn">true if the turn is white, false if the turn is black</param>
        /// <returns></returns>
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            return othelloAI.GetNextMove(game, level, whiteTurn);
        }

        /// <summary>
        /// Give the white score for the actual state of the game
        /// </summary>
        /// <returns>white score</returns>
        public int GetWhiteScore()
        {
            int count = 0;
            foreach (int square in board)
            {
                if (square == 1)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Give the black score for the actual state of the game
        /// </summary>
        /// <returns>black score</returns>
        public int GetBlackScore()
        {
            int count = 0;
            foreach (int square in board)
            {
                if (square == 1)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Check if a square is playable or not
        /// </summary>
        /// <param name="column">indice of the column</param>
        /// <param name="line">indice of the line</param>
        /// <param name="isWhite">true if the turn is white, false if the turn is black</param>
        /// <returns>true if the square is playable</returns>
        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return GameLogic.CheckPlayableSquare(board, column, line, isWhite);
        }

        /// <summary>
        /// Place a pawn in the board
        /// </summary>
        /// <param name="column">indice of the column</param>
        /// <param name="line">indice of the line</param>
        /// <param name="isWhite">true if the turn is white, false if the turn is black</param>
        /// <returns>if the placement succeed</returns>
        public bool PlayMove(int column, int line, bool isWhite)
        {
            if(IsPlayable(column, line, isWhite))
            {
                board = GameLogic.PlayMove(board, column, line, isWhite);
                return true;
            }
            return false;
        }

        /// <summary>
        /// fill the gameBoard with the initial pawn
        /// </summary>
        private void FillBoard()
        {
            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    board[i, j] = -1;
                }
            }

            board[3, 3] = 0;
            board[4, 3] = 1;
            board[3, 4] = 1;
            board[4, 4] = 0;
        }
    } 
}
