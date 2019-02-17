// ===============================
// OthelloAI_AB
//
// PROJECT: ArcOthelloAB_AI
// AUTHORS: Arzul Paul & Biloni Kim Aurore
// DATE: 17.02.19
// ORGANISATION: HE-Arc Ingénierie, Neuchâtel
// ===============================

using System;
using System.Collections.Generic;
using System.Linq;

namespace ArcOthelloAB_AI
{
    /// <summary>
    /// Class containing the AI as a singleton
    /// </summary>
    class OthelloAI_AB
    {
        private static OthelloAI_AB instance;

        private OthelloAI_AB()
        {
            //
        }

        /// <summary>
        /// Provide the instance of OthelloAI_AB
        /// </summary>
        /// <returns>single instance of the class</returns>
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
        /// <returns>Best move to play as a tuple</returns>
        public Tuple<int, int> GetNextMove(int[,] game, int level, bool whiteTurn)
        {
            // Create the root node corresponding as the actual state game
            MoveNode root = new MoveNode(game, whiteTurn);

            int minOrMax = -1;  // We start to minimize the opponent layer
            int parentValue = 0;

            // Call the AlphaBeta algorithm to gest the best move
            Tuple<int, Tuple<int, int>> bestMove = AlphaBeta(root, level, minOrMax, parentValue);

            return bestMove.Item2;
        }

        /// <summary>
        /// MinMax function of alpha beta algorithm
        /// </summary>
        /// <param name="root">Root MoveNode</param>
        /// <param name="depth">Depth to explore the possibilities</param>
        /// <param name="minOrMax">1 for maximize / -1 for minimize</param>
        /// <param name="parentValue">parent value</param>
        /// <returns>the best move found as a tuple containing the score and the move</returns>
        private Tuple<int, Tuple<int, int>> AlphaBeta(MoveNode root, int depth, int minOrMax, int parentValue)
        {
            // If it's a maximize layer
            if (minOrMax == 1)
                return Maximize(root, Int32.MaxValue, depth);
            // If it's a minimize layer
            else if (minOrMax == -1)
                return Minimize(root, Int32.MinValue, depth);
            // Return a invalid move as default
            return new Tuple<int, Tuple<int, int>>(0, new Tuple<int, int>(-1, -1));
        }

        /// <summary>
        /// Max function of alpha beta algorithm
        /// </summary>
        /// <param name="root">MoveNode root of maximize</param>
        /// <param name="parentMin">min value of the parent</param>
        /// <param name="depth">depth to search</param>
        /// <returns>Tuple containing the score and the move to maximize</returns>
        private Tuple<int, Tuple<int, int>> Maximize(MoveNode root, int parentMin, int depth)
        {
            // If the last layer has been reached or if the state is finale
            if (depth == 0 || root.Final())
            {
                // Return the score of the state and the PASS move
                return new Tuple<int, Tuple<int, int>>(root.Eval(), new Tuple<int, int>(-1, -1));
            }

            int maxValue = Int32.MinValue;
            Tuple<int, int> maxMove = new Tuple<int, int>(-1, -1);
            // Search the maximal score state in the childs
            foreach (var move in root.GetPossibleOperators())
            {
                // Get the child
                MoveNode child = root.Apply(move);
                // Minimize the child
                Tuple<int, Tuple<int, int>> minimization = Minimize(child, maxValue, depth - 1);
                // If the child is max
                if (minimization.Item1 > maxValue)
                {
                    // Save the score value and the move
                    maxValue = minimization.Item1;
                    maxMove = move;
                    
                    // If the child is better than its parent
                    if (maxValue > parentMin)
                    {
                        // The search is stopped
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
            // If the last layer has been reached or if the state is finale
            if (depth == 0 || root.Final())
            {
                // Return the score of the state and the PASS move
                return new Tuple<int, Tuple<int, int>>(root.Eval(), new Tuple<int, int>(-1, -1));
            }

            int minValue = Int32.MaxValue;
            Tuple<int, int> minMove = new Tuple<int, int>(-1, -1);
            // Search the minimal score state in the childs
            foreach (var move in root.GetPossibleOperators())
            {
                // Get the child
                MoveNode child = root.Apply(move);
                // Maximize the child
                Tuple<int, Tuple<int, int>> maximization = Maximize(child, minValue, depth - 1);
                // If the child is min
                if (maximization.Item1 < minValue)
                {
                    // Save the score value and the move
                    minValue = maximization.Item1;
                    minMove = move;

                    // If the child is better than its parent
                    if (minValue < parentMax)
                    {
                        // The search is stopped
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
            private int previousMobility; // mobility describes the number of possible move a player has
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

                // set pawn number for each player according to current player turn
                int playerPawn;
                int opponentPawn;
                if (isWhiteTurn)
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
                // count the number of pawn owned by the player and substract it by the number of oponent pawn
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
                // check how many corner the player owns and substract it by the number of corner owned by opponent
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

                // final score
                score = (int)(pawnCountEvaluation + mobilityDifference * 5 + cornerCount * 200);
                // mobility is 5 time more important than pawn count
                // corner count varying between[-4;4] and being more important, it is multiplied by 200


                if (Final()) // if the node is final
                {
                    // if the score is positif
                    if (score > 0)
                        // the state is a win
                        return Int32.MaxValue;
                    // if the score is negatif
                    if (score < 0)
                        // the state is a loose
                        return Int32.MinValue;
                }

                // return the final score
                return score;
            }

            /// <summary>
            /// Return if the game has ended with this move or not
            /// </summary>
            /// <returns>final state boolean</returns>
            public bool Final()
            {
                // if there's no possible move return true
                return !GetPossibleOperators().Any();
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
                        if (game[i, j] == -1) // check only empty square
                        {
                            if (GameLogic.CheckPlayableSquare(game, i, j, isWhiteTurn))
                                possibleOperators.Add(new Tuple<int, int>(i, j));
                        }
                    }
                }
                return possibleOperators;
            }

            /// <summary>
            /// Apply a move operator to the actual node. Return a new MoveNode.
            /// The move must be a valid move.
            /// </summary>
            /// <param name="move_operator"></param>
            /// <returns>The resulting MoveNode</returns>
            public MoveNode Apply(Tuple<int, int> move_operator)
            {
                int x = move_operator.Item1;
                int y = move_operator.Item2;

                int[,] newGame = GameLogic.PlayMove(game, x, y, isWhiteTurn);

                int currentMobility = GetPossibleOperators().Count();

                bool childTurn = !isWhiteTurn; // child turn will be the inverse of current turn
                MoveNode child = new MoveNode(newGame, childTurn, currentMobility);
                return child;
            }
        }
    }
}
