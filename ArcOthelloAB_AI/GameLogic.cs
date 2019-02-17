// ===============================
// GameLogic
//
// PROJECT: ArcOthelloAB_AI
// AUTHORS: Arzul Paul & Biloni Kim Aurore
// DATE: 17.02.19
// ORGANISATION: HE-Arc Ingénierie, Neuchâtel
// ===============================

namespace ArcOthelloAB_AI
{
    /// <summary>
    /// Class used for the othello game logic
    /// </summary>
    internal static class GameLogic
    {
        /// <summary>
        /// Check if a given square is playable
        /// </summary>
        /// <param name="x">x position of the current square to check</param>
        /// <param name="y">y position of the current square to check</param>
        /// <returns>boolean as true for playable</returns>
        public static bool CheckPlayableSquare(int[,] game, int x, int y, bool isWhiteTurn)
        {
            int[,] directionToCheck = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } }; // the eight cardinal direction to check in the board

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
        /// Play a move in a given board at a given coordonate
        /// </summary>
        /// <param name="game">two dimensional int array</param>
        /// <param name="x">x coordonate</param>
        /// <param name="y">y coordonate</param>
        /// <param name="isWhite">boolean as true if the turn is white</param>
        /// <returns>A new gameboard with the played move</returns>
        public static int[,] PlayMove(int[,] game, int x, int y, bool isWhite)
        {
            // Copy the new array
            int[,] newGame = new int[game.GetLength(0), game.GetLength(1)];
            for (int i = 0; i < game.GetLength(0); i++)
            {
                for (int j = 0; j < game.GetLength(1); j++)
                {
                    newGame[i, j] = game[i, j];
                }
            }

            // Attribut the colors to the players
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

            // Place the pawn in the coordonate
            newGame[x, y] = playerValue;

            // Reverse all the possible pawns to the player color
            int[,] directionToCheck = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };  // the eight cardinal direction to check in the board
            for (int i = 0; i < 8; i++)
            {
                int dirX = directionToCheck[i, 0];
                int dirY = directionToCheck[i, 1];
                if (CheckOtherPawnFromDirection(game, dirX, dirY, x, y, isWhite)) // if a direction allowed for a move to be played, then apply modification to said direction
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
        /// Check in a given direction if the a pawn can be placed on the current empty square
        /// </summary>
        /// <param name="dirX">direction ot move for each step in x coordonate</param>
        /// <param name="dirY"> direction ot move for each step in y coordonate</param>
        /// <param name="x">x coordonate</param>
        /// <param name="y">y coordonate</param>
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
