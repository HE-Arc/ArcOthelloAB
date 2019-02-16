using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcOthelloAB
{
    class OthelloAI
    {
        private static OthelloAI instance;

        private OthelloAI()
        {
            //
        }

        /// <summary>
        /// Provide the instance of OthelloAI
        /// </summary>
        /// <returns></returns>
        public static OthelloAI GetInstance()
        {
            if (instance == null)
                instance = new OthelloAI();
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
            MoveNode root = new MoveNode(game);
            int minOrMax = whiteTurn;  // TODO need to see when we must min or max
            int parentValue = 0; // TODO 0 or 1 for begin search value
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
            return null;
        }

        /// <summary>
        /// Max function of alpha beta algorithm
        /// </summary>
        /// <param name="root">MoveNode root</param>
        /// <param name="parentMin"></param>
        /// <param name="depth">depth to search</param>
        /// <returns></returns>
        private Tuple<int, Tuple<int, int>> Maximize(MoveNode root, MoveNode parentMin, int depth)
        {
            if(depth == 0 || root.Final())
            {
                return new Tuple(root.Eval(), null);
            }

            int maxValue = Int32.MinValue;
            Tuple<int, int> maxMove;
            foreach (var move in root.GetPossibleOperators())
            {
                MoveNode child = root.Apply(move);
                Tuple<int, Tuple<int, int>> minimization = Minimize(child, maxValue, depth - 1);
                if(minimization.Item1 > maxValue)
                {
                    maxValue = minimization.Item1;
                    maxMove = move;
                    if(maxValue > parentMin)
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
        /// <param name="root"></param>
        /// <param name="parentMax"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private Tuple<int, Tuple<int, int>> Minimize(MoveNode root, MoveNode parentMax, int depth)
        {
            if (depth == 0 || root.Final())
            {
                return new Tuple(root.Eval(), null);
            }

            int minValue = Int32.MaxValue;
            Tuple<int, int> minMove;
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
			
            /// <summary>
            /// Construct a move node using a game board
            /// </summary>
            /// <param name="game"></param>
            public MoveNode(int[,] game, bool isWhiteTurn)
            {
                this.game = game;
                this.isWhiteTurn = isWhiteTurn;
            }

            /// <summary>
            /// Constructor of MoveNode
            /// </summary>
            public MoveNode()
            {
                // TODO
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
                int score = 0;
                foreach (int pawn in game)
                {
                    if(isWhiteTurn)
                    {
                        if (pawn == 1)
                            score++;
                        if (pawn == 2)
                            score--;
                    }
                    else
                    {
                        if (pawn == 1)
                            score--;
                        if (pawn == 2)
                            score++;
                    }
                }
                return score;
            }

            /// <summary>
            /// Return if the game has ended with this move or not
            /// </summary>
            /// <returns>final state boolean</returns>
            public bool Final()
            {
                if (!GetPossibleOperators().Any()) // if there's no possible move return true
                    return true;
                return false;
            }

            /// <summary>
            /// Returns applicable operators of node
            /// </summary>
            /// <returns></returns>
            public List<Tuple<int, int>> GetPossibleOperators()
            {
                // TODO, evaluate all square without pawn to see if they're playable, then add to a list
                return null;
            }

            /// <summary>
            /// Apply a move operator to the actual node. Return a new MoveNode.
            /// </summary>
            /// <param name="move_operator"></param>
            /// <returns></returns>
            public MoveNode Apply(Tuple<int, int> move_operator)
            {
                int[,] newGame;

                // TODO, modify game according according to played move

                bool childTurn = (isWhiteTurn + 1) % 2; // child turn will be the inverse of current turn
                MoveNode child = new MoveNode(newGame, childTurn);

                if (!child.GetPossibleOperators().Any()) // if after playing a move, the child has no available move, then it switches turn
                    child.isWhiteTurn = this.isWhiteTurn;  // after switching turn, if there's still no move available, the game ended, function Final() will do this verification

                return child;
            }
        }
    }
}
