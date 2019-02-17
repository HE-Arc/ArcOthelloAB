using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public AIBoard()
        {
            board = new int[TOTAL_COLLUMN, TOTAL_ROW];
            othelloAI = OthelloAI_AB.GetInstance();
            FillBoard();
        }

        public int[,] GetBoard()
        {
            return board;
        }

        public string GetName()
        {
            return NAME;
        }

        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            return othelloAI.GetNextMove(game, level, whiteTurn);
        }

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

        public bool IsPlayable(int column, int line, bool isWhite)
        {
            return GameLogic.checkPlayableSquare(board, column, line, isWhite);
        }

        public bool PlayMove(int column, int line, bool isWhite)
        {
            if(IsPlayable(column, line, isWhite))
            {
                board = GameLogic.playMove(board, column, line, isWhite);
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

    class OthelloAI_AB
    {
        private static OthelloAI_AB instance;

        private OthelloAI_AB()
        {
            //
        }

        /// <summary>
        /// Provide the instance of OthelloAI
        /// </summary>
        /// <returns></returns>
        public static OthelloAI_AB GetInstance()
        {
            if (instance == null)
                instance = new OthelloAI_AB();
            return instance;
        }

        /// <summary>
        /// Give the best next move for a player
        /// </summary>
        /// <param name="game">two dimensional int array as board</param>
        /// <param name="level">depth of search for AI</param>
        /// <param name="whiteTurn">if it's white turn or not</param>
        /// <returns>Best move to play</returns>
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            MoveNode root = new MoveNode(game, whiteTurn);
            int minOrMax = -1;
            int parentValue = 0;
            Tuple<int, Tuple<int, int>> bestMove = AlphaBeta(root, level, minOrMax, parentValue);
            return bestMove.Item2;
        }

        /// <summary>
        /// MinMax function of alpha beta algorithm
        /// </summary>
        /// <param name="root"></param>
        /// <param name="depth"></param>
        /// <param name="minOrMax">1 for maximize / -1 for minimize</param>
        /// <param name="parentValue"></param>
        /// <returns></returns>
        private Tuple<int, Tuple<int, int>> AlphaBeta(MoveNode root, int depth, int minOrMax, int parentValue)
        {
            if (minOrMax == 1)
                return Maximize(root, Int32.MaxValue, depth);
            else if (minOrMax == -1)
                return Minimize(root, Int32.MinValue, depth);
            return new Tuple<int, Tuple<int, int>>(0, new Tuple<int, int>(-1, -1));
        }

        /// <summary>
        /// Max function of alpha beta algorithm
        /// </summary>
        /// <param name="root">MoveNode root of maximize</param>
        /// <param name="parentMin"></param>
        /// <param name="depth">depth to search</param>
        /// <returns>Tuple containing the score and the move to maximize</returns>
        private Tuple<int, Tuple<int, int>> Maximize(MoveNode root, int parentMin, int depth)
        {
            if (depth == 0 || root.Final())
            {
                return new Tuple<int, Tuple<int, int>>(root.Eval(), new Tuple<int, int>(-1, -1));
            }

            int maxValue = Int32.MinValue;
            Tuple<int, int> maxMove = new Tuple<int, int>(-1, -1);
            foreach (var move in root.GetPossibleOperators())
            {
                MoveNode child = root.Apply(move);
                Tuple<int, Tuple<int, int>> minimization = Minimize(child, maxValue, depth - 1);
                if (minimization.Item1 > maxValue)
                {
                    maxValue = minimization.Item1;
                    maxMove = move;
                    if (maxValue > parentMin)
                    {
                        break;
                    }
                }
            }
            return new Tuple<int, Tuple<int, int>>(maxValue, maxMove);
        }

        /// <summary>
        /// Min function of alpha beta algorithm
        /// </summary>
        /// <param name="root">MoveNode root of minimize</param>
        /// <param name="parentMax">max value of the parent</param>
        /// <param name="depth">depth to search</param>
        /// <returns>Tuple containing the score and the move to minimize</returns>
        private Tuple<int, Tuple<int, int>> Minimize(MoveNode root, int parentMax, int depth)
        {
            if (depth == 0 || root.Final())
            {
                return new Tuple<int, Tuple<int, int>>(root.Eval(), new Tuple<int, int>(-1, -1));
            }

            int minValue = Int32.MaxValue;
            Tuple<int, int> minMove = new Tuple<int, int>(-1, -1);
            foreach (var move in root.GetPossibleOperators())
            {
                MoveNode child = root.Apply(move);
                Tuple<int, Tuple<int, int>> maximization = Maximize(child, minValue, depth - 1);
                if (maximization.Item1 < minValue)
                {
                    minValue = maximization.Item1;
                    minMove = move;
                    if (minValue < parentMax)
                    {
                        break;
                    }
                }
            }
            return new Tuple<int, Tuple<int, int>>(minValue, minMove);
        }

        /// <summary>
        /// MoveNode class represent a node for the AI alpahbeta algorithm
        /// </summary>
        private class MoveNode
        {
            private int[,] game;
            public bool isWhiteTurn;

            // evaluation tool
            private int previousMobility;
            private int currentMobility;

            /// <summary>
            /// Construct a move node using a game board
            /// </summary>
            /// <param name="game">game board</param>
            public MoveNode(int[,] game, bool isWhiteTurn, int previousMobility = 0)
            {
                this.game = game;
                this.isWhiteTurn = isWhiteTurn;
                this.previousMobility = previousMobility;
                currentMobility = GetPossibleOperators().Count();
            }

            /// <summary>
            /// Eval function of a node
            /// 
            /// the score equal number of pawn on the board of current player
            /// minus number of pawn of opponent on the board
            /// </summary>
            /// <returns>score of the node</returns>
            public int Eval()
            {
                int score;

                int playerPawn;
                int opponentPawn;
                if(isWhiteTurn)
                {
                    playerPawn = 0;
                    opponentPawn = 1;
                }
                else
                {
                    playerPawn = 1;
                    opponentPawn = 0;
                }

                // pawn count
                // count the number of pawn owned by the player and substract it by the number of ooponent pawn
                int pawnCountEvaluation = 0;

                foreach (int pawn in game)
                {
                    if (pawn == playerPawn)
                        pawnCountEvaluation++;
                    if (pawn == opponentPawn)
                        pawnCountEvaluation--;
                }

                // mobility count
                // substract the player number of possible move by the number of possible move the opponent had previously
                int mobilityDifference = currentMobility - previousMobility;

                // corner count
                // check how many corner the player own and substract it by the number of corner owned by opponent
                int cornerCount = 0;
                int[,] cornerSquare = { { 0, 0 }, { 0, 6 }, { 8, 0 }, { 8, 6 } };
                for (int i = 0; i < 4; i++)
                {
                    int x = cornerSquare[i, 0];
                    int y = cornerSquare[i, 1];
                    if (game[x, y] == playerPawn)
                        cornerCount++;
                    if (game[x, y] == opponentPawn)
                        cornerCount--;
                }

                // Ponderation Matrix
                // Each case has a ponderation of importance to have a pawn on it
                int ponderateScore = 0;
                int[,] ponderationMatrix = {
                    {20, -3, 11, 8, 8, 8, 11, -3, 20 },
                    {-3, -7, -4, 1, 1, 1, -4, -7, -3 },
                    {11, -7, -4, 2, 2, -4, 1, -4, 11 },
                    {8, 1, 2, -3, -3, 2, 1, 1, 8 },
                    {11, -4, 2, -3, -3, 2, 1, -4, 11 },
                    {-3, -7, -4, 2, 2, -4, -4, -7, -3 },
                    {20, -3, 11, 8, 8, 8, 11, -3, 20 }
                };
                int pawnColor = (isWhiteTurn ? 0 : 1);
                for(int i = 0; i < game.GetLength(0); i++)
                {
                    for (int j = 0; j < game.GetLength(1); j++)
                    {
                        //if(game[i, j] == pawnColor)
                        //    ponderateScore += ponderationMatrix[i, j];
                    }
                }


                // final score
                score = (int)(pawnCountEvaluation / 5 + mobilityDifference + cornerCount * 50 + ponderateScore);
                // mobility is 5 time more important than pawn count
                // corner count varying between[-4;4] and being more important, it is multiplied by 50
                // ponderateScore
                

                if (Final()) // if the node is final, then it means there's a winner, will heavely change Ai choice
                {
                    if (score > 0)
                        return Int32.MaxValue;
                    if (score < 0)
                        return Int32.MinValue;
                }
                return score;
            }

            /// <summary>
            /// Return if the game has ended with this move or not
            /// </summary>
            /// <returns>final state boolean</returns>
            public bool Final()
            {
                return !GetPossibleOperators().Any(); // if there's no possible move return true
            }

            /// <summary>
            /// Returns applicable operators of node
            /// </summary>
            /// <returns>List of possible operators</returns>
            public List<Tuple<int, int>> GetPossibleOperators()
            {
                List<Tuple<int, int>> possibleOperators = new List<Tuple<int, int>>();

                int collumnLenght = game.GetLength(0);
                int rowLenght = game.GetLength(1);
                for (int i = 0; i < collumnLenght; i++)
                {
                    for (int j = 0; j < rowLenght; j++)
                    {
                        if (game[i, j] == -1)
                        {
                            if (GameLogic.checkPlayableSquare(game,i, j, isWhiteTurn))
                                possibleOperators.Add(new Tuple<int, int>(i, j));
                        }
                    }
                }

                return possibleOperators;
            }

            /// <summary>
            /// Apply a move operator to the actual node. Return a new MoveNode.
            /// 
            /// The move must be a valid move.
            /// </summary>
            /// <param name="move_operator"></param>
            /// <returns>The resulting MoveNode</returns>
            public MoveNode Apply(Tuple<int, int> move_operator)
            {
                int x = move_operator.Item1;
                int y = move_operator.Item2;

                int[,] newGame = GameLogic.playMove(game, x, y, isWhiteTurn);

                int currentMobility = GetPossibleOperators().Count();

                bool childTurn = !isWhiteTurn; // child turn will be the inverse of current turn
                MoveNode child = new MoveNode(newGame, childTurn, currentMobility);
                return child;
            }
        }

        
    }


    class GameLogic
    {

        /// <summary>
        /// Check if a given square is playable
        /// 
        /// return true if it is
        /// </summary>
        /// <param name="x">x position of the current square to check</param>
        /// <param name="y">y position of the current square to check</param>
        /// <returns>if the square is playable or not</returns>
        public static bool checkPlayableSquare(int[,] game, int x, int y, bool isWhiteTurn)
        {
            int[,] directionToCheck = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            bool isPlayable = false;

            for (int i = 0; i < 8; i++)
            {
                int dirX = directionToCheck[i, 0];
                int dirY = directionToCheck[i, 1];
                if (CheckOtherPawnFromDirection(game, dirX, dirY, x, y, isWhiteTurn))
                    isPlayable = true;
            }
            return isPlayable;
        }

        /// <summary>
        /// Play a move in a given oard at a given coordonate
        /// </summary>
        /// <param name="game"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isWhite"></param>
        /// <returns>A new gameboard with the played move</returns>
        public static int[,] playMove(int[,] game, int x, int y, bool isWhite)
        {
            int[,] newGame = new int[game.GetLength(0), game.GetLength(1)];
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int j = 0; j < game.GetLength(1); j++)
                {
                    newGame[i, j] = game[i, j];
                }
            }

            int playerValue;
            int opponentValue;
            if (isWhite)
            {
                playerValue = 0;
                opponentValue = 1;
            }
            else
            {
                playerValue = 1;
                opponentValue = 0;
            }

            newGame[x, y] = playerValue;

            int[,] directionToCheck = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            for (int i = 0; i < 8; i++)
            {
                int dirX = directionToCheck[i, 0];
                int dirY = directionToCheck[i, 1];
                if (CheckOtherPawnFromDirection(game,dirX, dirY, x, y, isWhite))
                {
                    int nextX = x + dirX;
                    int nextY = y + dirY;

                    int nextSquareValue = newGame[nextX, nextY];

                    // as long as ennemy pawn are alligned, will change their status to the current player
                    while (nextSquareValue == opponentValue)
                    {
                        newGame[nextX, nextY] = playerValue;
                        nextX += dirX;
                        nextY += dirY;
                        nextSquareValue = newGame[nextX, nextY];
                    }
                }
            }
            return newGame;
        }



        /// <summary>
        /// Will check in a given direction if the a pawn can be placed on the current empty square
        /// </summary>
        /// <param name="dirX">direction ot move for each step in x coordonate</param>
        /// <param name="dirY"> direction ot move for each step in y coordonate</param>
        /// <param name="x"> x coordonate of clicked button</param>
        /// <param name="y"> y coordonate of clicked button</param>
        /// <returns>if the pawn is placable or not</returns>
        private static bool CheckOtherPawnFromDirection(int[,] game, int dirX, int dirY, int x, int y, bool isWhiteTurn)
        {
            if (dirX == 0 && dirY == 0)
                return false;
            x += dirX; // verification will advance in the given direction
            y += dirY;

            // if the next square is out of the grid
            if (x < 0 || x >= game.GetLength(0) || y < 0 || y >= game.GetLength(1))
                return false;

            int nextSquareValue = game[x, y];

            int playerValue;
            int opponentValue;
            if (isWhiteTurn)
            {
                playerValue = 0;
                opponentValue = 1;
            }
            else
            {
                playerValue = 1;
                opponentValue = 0;
            }

            bool hasAtLeastOneEnnemy = false; // if there isn't a single ennemy and the search stoped then it must not return false

            // as long as there is only ennemy pawn alligned
            while (nextSquareValue == opponentValue)
            {
                hasAtLeastOneEnnemy = true;
                x += dirX;
                y += dirY;

                if (x < 0 || x >= game.GetLength(0) || y < 0 || y >= game.GetLength(1))
                    return false;

                nextSquareValue = game[x, y];
            }

            // when a checked pawn isn't ennemy pawn, if it one of his pawn, he can place his pawn in the checked square
            if (nextSquareValue == playerValue && hasAtLeastOneEnnemy)
                return true;
            else
                return false;
        }
    }
}
