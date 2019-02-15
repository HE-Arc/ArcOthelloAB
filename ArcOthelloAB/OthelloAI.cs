﻿using System;
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
            //TODO
            return null;
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
            /// <summary>
            /// Constructor of MoveNode
            /// </summary>
            public MoveNode()
            {
                // TODO
            }

            /// <summary>
            /// Eval function of a node
            /// </summary>
            /// <returns>score of the node</returns>
            public int Eval()
            {
                // TODO
                return 0;
            }

            /// <summary>
            /// Return if the game has ended with this move or not
            /// </summary>
            /// <returns>final state boolean</returns>
            public bool Final()
            {
                // TODO
                return true;
            }

            /// <summary>
            /// Returns applicable operators of node
            /// </summary>
            /// <returns></returns>
            public List<Tuple<int, int>> GetPossibleOperators()
            {
                return null;
            }

            /// <summary>
            /// Apply a move operator to the actual node. Return a new MoveNode.
            /// </summary>
            /// <param name="move_operator"></param>
            /// <returns></returns>
            public MoveNode Apply(Tuple<int, int> move_operator)
            {
                return null;
            }
        }
    }
}
