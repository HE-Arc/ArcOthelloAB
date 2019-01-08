using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ArcOthelloAB
{
    class ButtonHandler
    {

        public static readonly DependencyProperty IsAvailableProperty =
            DependencyProperty.Register(
            "isAvailable", typeof(Boolean),
            typeof(Othello)
            );
        public SquareStatus status { get; set; }

        // Uses the SquareStatus enum for the state
        public static readonly DependencyProperty CurrentStatus =
            DependencyProperty.Register(
            "currentStatus", typeof(SquareStatus),
            typeof(Othello)
            );

        private UIElement[,] buttons;
        private Grid gameGrid;

        // Game Properties
        private int TOTAL_ROW;
        private int TOTAL_COLLUMN;

        public ButtonHandler(Grid gameGrid, UIElement[,] buttons, int TOTAL_ROW = 7, int TOTAL_COLLUMN = 9)
        {
            this.gameGrid = gameGrid;
            this.buttons = buttons;
            this.TOTAL_ROW = TOTAL_ROW;
            this.TOTAL_COLLUMN = TOTAL_COLLUMN;

            setupButtons();
        }

        private void setupButtons()
        {
            //buttons = new UIElement[TOTAL_COLLUMN, TOTAL_ROW];

            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    Button btn = new Button();
                    int btnX = i;
                    int btnY = j;
                    btn.Click += (sender, e) =>
                    {
                        //ButtonAction1(btnX, btnY); // temporary
                    };
                    gameGrid.Children.Add(btn); // add button as children to gamegrid
                    Grid.SetRow(btn, j); // search the btn contained in a grid and setup a row position
                    Grid.SetColumn(btn, i); // setup a column

                    buttons[btnX, btnY] = GetButton(btnX, btnY);
                }
            }

            // initiate property of each button
            foreach (UIElement child in gameGrid.Children)
            {
                child.SetValue(IsAvailableProperty, false);
                setButtonState(child, SquareStatus.NoPawn);
            }

            // setup initial board
            setButtonState(buttons[3, 3], SquareStatus.WhitePawn);
            setButtonState(buttons[4, 3], SquareStatus.BlackPawn);
            setButtonState(buttons[3, 4], SquareStatus.BlackPawn);
            setButtonState(buttons[4, 4], SquareStatus.WhitePawn);

            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    if ((SquareStatus)buttons[i, j].GetValue(CurrentStatus) == SquareStatus.NoPawn)
                        UpdateButtonAvailability(i, j);
                }
            }
        }
        /*
         * get an UIElement of the board grid according to it's row and column
         */
        private UIElement GetButton(int column, int row)
        {
            Grid grid = gameGrid;
            foreach (UIElement child in grid.Children)
            {
                if (Grid.GetRow(child) == row && Grid.GetColumn(child) == column)
                {
                    return child;
                }
            }
            return null;
        }

        private void setButtonState(UIElement button, SquareStatus newStatus)
        {
            button.SetValue(CurrentStatus, newStatus);
            SolidColorBrush brush;
            switch (newStatus)
            {
                case SquareStatus.NoPawn:
                    brush = new SolidColorBrush(Colors.Gray);
                    break;
                case SquareStatus.BlackPawn:
                    brush = new SolidColorBrush(Colors.Black);
                    break;
                case SquareStatus.WhitePawn:
                    brush = new SolidColorBrush(Colors.White);
                    break;
                default:
                    brush = new SolidColorBrush(Colors.Gray);
                    break;
            }
            button.GetType().GetProperty("Background").SetValue(button, brush);
        }

        /*
         * Coordonate of square to check ar given as argument
         * 
         * Will check in every direction to see if this square is playable
         * Will then update his status according to the result of the search
         */
        private void UpdateButtonAvailability(int x, int y)
        {
            int[,] directionToCheck = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            bool isPlayable = false;

            for (int i = 0; i < 8; i++)
            {
                int dirX = directionToCheck[i, 0];
                int dirY = directionToCheck[i, 1];
                if (CheckOtherPawnFromDirection(dirX, dirY, x, y, SquareStatus.BlackPawn))
                    isPlayable = true;
            }
            if (isPlayable)
                buttons[x, y].SetValue(IsAvailableProperty, true);
            else
                buttons[x, y].SetValue(IsAvailableProperty, false);
        }



        /**
         * Will check in a given direction if the a pawn can be placed on the current empty square
         * 
         * The direction is given by a movement in x and y: dirX and dirY argument
         * These value should be {-1,0,1} with at least one of the two not set at 0
         * 
         * the position of empty square is given by x and y argument
         */
        private bool CheckOtherPawnFromDirection(int dirX, int dirY, int x, int y, SquareStatus currentStatus)
        {
            if (dirX == 0 && dirY == 0)
                return false;
            x += dirX; // verification will advance in the given direction
            y += dirY;

            // if the next square is out of the grid
            if (x < 0 || x >= TOTAL_COLLUMN || y < 0 || y >= TOTAL_ROW)
                return false;
            UIElement currentButton = buttons[x, y];

            // get the color of the current ennemy
            SquareStatus opponentStatus;
            if (currentStatus == SquareStatus.BlackPawn)
                opponentStatus = SquareStatus.WhitePawn;
            else
                opponentStatus = SquareStatus.BlackPawn;

            SquareStatus nextPawnStatus = (SquareStatus)currentButton.GetValue(CurrentStatus);

            bool hasAtLeastOneEnnemy = false;

            // as long as there is only ennemy pawn alligned
            while (nextPawnStatus == opponentStatus)
            {
                hasAtLeastOneEnnemy = true;
                x += dirX;
                y += dirY;

                if (x < 0 || x >= TOTAL_COLLUMN || y < 0 || y >= TOTAL_ROW)
                    return false;
                currentButton = buttons[x, y];
                nextPawnStatus = (SquareStatus)currentButton.GetValue(CurrentStatus);
            }

            // when a checked pawn isn't ennemy pawn, if it one of his pawn, he can place his pawn in the checked square
            if (nextPawnStatus == currentStatus && hasAtLeastOneEnnemy)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Count the score of a pawn type
        /// <param name=pawnType>pawn type (WHITE_PAWN or BLACK_PAWN)</param>
        /// <returns>score of pawnType</returns>
        /// </summary>
        public int GetScore(SquareStatus pawnStatus)
        {
            int score = 0;
            foreach (var button in buttons)
            {
                if ((SquareStatus)button.GetValue(CurrentStatus) == pawnStatus)
                    score++;
            }
            return score;
        }

        /// <summary>
        /// Get the status of a square
        /// <param name=pawnType>x and y position of square in grid</param>
        /// <returns>status of square at the [x,y] coordonate</returns>
        /// </summary>
        public SquareStatus getStatus(int x, int y)
        {
            return (SquareStatus)buttons[x, y].GetValue(CurrentStatus);
        }
    }


}
