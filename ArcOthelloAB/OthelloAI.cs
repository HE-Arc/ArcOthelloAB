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
            bool minOrMax = whiteTurn;  // TODO need to see when we must min or max
            int parentValue = 0; // TODO 0 or 1 for begin search value
            Tuple<int, Tuple<int, int>> bestMove = AlphaBeta(root, level, minOrMax, parentValue);
            return bestMove.Item2;
        }

        /// <summary>
        /// MinMax function of alpha beta algorithm
        /// </summary>
        /// <param name="root"></param>
        /// <param name="depth"></param>
        /// <param name="minOrMax"></param>
        /// <param name="parentValue"></param>
        /// <returns></returns>
        private Tuple<int, Tuple<int, int>> AlphaBeta(MoveNode root, int depth, bool minOrMax, int parentValue)
        {
            if (depth == 0 || root.Final())
                return new Tuple<int, Tuple<int, int>>(root.Eval(), null);
            int optVal = 0;
            if(minOrMax) // if true -> maximaze
            {
                optVal = Int32.MinValue;
            }
            else    // else minimize
            {
                optVal = Int32.MaxValue;
            }
            Tuple<int, int> moveOperator = null;

            foreach(var move in root.GetPossibleOperators())
            {
                // search th best move
            }

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
            // TODO
            return null;
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
            // TODO
            return null;
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
                List<Tuple<int, int>> possibleOperators = new List<Tuple<int, int>>();

                int collumnLenght = game.GetLength(0);
                int rowLenght = game.GetLength(1);
                for (int i = 0; i < collumnLenght; i++)
                {
                    for (int j = 0; j < rowLenght; j++)
                    {
                        if(game[i,j] == 0)
                        {
                            if (checkPlayableSquare())
                                possibleOperators.Add(new Tuple<int, int>(x, y));
                        }
                    }
                }

                return possibleOperators;
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


            //////////////////////
            //      PRIVATE     //
            //////////////////////

            /// <summary>
            /// Check if a given square is playable
            /// 
            /// return true if it is
            /// </summary>
            /// <param name="x">x position of the current square to check</param>
            /// <param name="y">y position of the current square to check</param>
            /// <returns></returns>
            private bool checkPlayableSquare(int x, int y)
            {
                int[,] directionToCheck = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

                bool isPlayable = false;

                for (int i = 0; i < 8; i++)
                {
                    int dirX = directionToCheck[i, 0];
                    int dirY = directionToCheck[i, 1];
                    if (CheckOtherPawnFromDirection(dirX, dirY, x, y))
                        isPlayable = true;
                }
                return isPlayable;
            }

            /// <summary>
            /// Will check in a given direction if the a pawn can be placed on the current empty square
            /// </summary>
            /// <param name=dirX> direction ot move for each step in x coordonate</param>
            /// <param name=dirY> direction ot move for each step in y coordonate</param>
            /// <param name=x> x coordonate of clicked button</param>
            /// <param name=y> y coordonate of clicked button</param>
            /// <param name=currentStatus> status of the current player</param>
            private bool CheckOtherPawnFromDirection(int dirX, int dirY, int x, int y)
            {
                if (dirX == 0 && dirY == 0)
                    return false;
                x += dirX; // verification will advance in the given direction
                y += dirY;

                // if the next square is out of the grid
                if (x < 0 || x >= game.GetLength(0) || y < 0 || y >= game.GetLength(1))
                    return false;

                int nextSquareValue = board[x, y];

                int playerValue;
                int opponentValue;
                if (isWhiteTurn)
                {
                    playerValue = 1;
                    opponentValue = 2;
                }
                else
                {
                    playerValue = 2;
                    opponentValue = 1;
                }

                SquareStatus nextPawnStatus = (SquareStatus)currentButton.GetValue(CurrentStatus);

                bool hasAtLeastOneEnnemy = false; // if there isn't a single ennemy and the search stoped then it must not return false

                // as long as there is only ennemy pawn alligned
                while (nextSquareValue == opponentValue)
                {
                    hasAtLeastOneEnnemy = true;
                    x += dirX;
                    y += dirY;

                    if (x < 0 || x >= TOTAL_COLLUMN || y < 0 || y >= TOTAL_ROW)
                        return false;

                    nextSquareValue = board[x, y];
                }

                // when a checked pawn isn't ennemy pawn, if it one of his pawn, he can place his pawn in the checked square
                if (nextSquareValue == playerValue && hasAtLeastOneEnnemy)
                    return true;
                else
                    return false;
            }
        }
    }
}
