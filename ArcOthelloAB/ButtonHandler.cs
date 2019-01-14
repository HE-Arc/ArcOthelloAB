using System;
using System.Windows;
using System.Windows.Controls;

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
        private readonly int TOTAL_ROW;
        private readonly int TOTAL_COLLUMN;

        private SquareStatus currentPlayer;
        private readonly TimeHandler timeHandler;
        private readonly Window parent;

        public ButtonHandler(Window parent, Grid gameGrid, UIElement[,] buttons, TimeHandler timeHandler, int TOTAL_ROW = 7, int TOTAL_COLLUMN = 9)
        {
            this.parent = parent;
            this.gameGrid = gameGrid;
            this.buttons = buttons;
            this.timeHandler = timeHandler;
            this.TOTAL_ROW = TOTAL_ROW;
            this.TOTAL_COLLUMN = TOTAL_COLLUMN;

            currentPlayer = SquareStatus.WhitePawn; // White Pawn starts by default

            setupButtons();
        }

        private void setupButtons()
        {
            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    Button btn = new Button();

                    int btnX = i;
                    int btnY = j;
                    btn.Click += (sender, e) =>
                    {
                        buttonAction(btnX, btnY);
                    };
                    gameGrid.Children.Add(btn); // add button as children to gamegrid
                    Grid.SetRow(btn, j); // search the btn contained in a grid and setup a row position
                    Grid.SetColumn(btn, i); // setup a column

                    buttons[btnX, btnY] = btn;

                    // initiate property of each button
                    btn.SetValue(IsAvailableProperty, false);
                    setButtonState(btn, SquareStatus.NoPawn); // Add corresponding style
                }
            }

            // setup initial board
            setButtonState((Button)buttons[3, 3], SquareStatus.WhitePawn);
            setButtonState((Button)buttons[4, 3], SquareStatus.BlackPawn);
            setButtonState((Button)buttons[3, 4], SquareStatus.BlackPawn);
            setButtonState((Button)buttons[4, 4], SquareStatus.WhitePawn);

            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    if ((SquareStatus)buttons[i, j].GetValue(CurrentStatus) == SquareStatus.NoPawn)
                        UpdateButtonAvailability(i, j, currentPlayer);
                }
            }
        }

        /// <summary>
        /// Get an UIElement of the board grid according to it's row and column
        /// </summary>
        /// <param name="column">column indice</param>
        /// <param name="row">row indice</param>
        /// <returns>UIElement of the specified column and row</returns>
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

        /// <summary>
        /// Action of each buttons, will activate only if the the button is avaible for play
        /// 
        /// Will place a pawn of current player in the button
        /// will then modify surrounding pawn according to othelo rules
        /// </summary>
        /// <param name=x> x coordonate of button</param>
        /// <param name=y> y coordonate of button</param>
        public bool buttonAction(int x, int y, SquareStatus player = SquareStatus.NoPawn)
        {
            if (player == SquareStatus.NoPawn)
                player = currentPlayer;

            Button button = (Button)buttons[x, y];
            
            if((bool)button.GetValue(IsAvailableProperty) && (SquareStatus)button.GetValue(CurrentStatus) == SquareStatus.NoPawn)
            {
                setButtonState(button, player);

                int[,] directionToCheck = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

                for (int i = 0; i < 8; i++)
                {
                    int dirX = directionToCheck[i, 0];
                    int dirY = directionToCheck[i, 1];
                    if (CheckOtherPawnFromDirection(dirX, dirY, x, y, player))
                        setOtherPawnFromDirection(dirX, dirY, x, y, player);
                }

                changePlayer();// change the current player and update button

                if (!checkButtonsAvailability()) // if the player can't play anywhere, change player again
                    changePlayer();
            }
            return true;
        }

        /// <summary>
        /// Give the other player the right to play
        /// Update buttons playability accordingly
        /// </summary>
        private void changePlayer()
        {
            if (currentPlayer == SquareStatus.BlackPawn)
                currentPlayer = SquareStatus.WhitePawn;
            else
                currentPlayer = SquareStatus.BlackPawn;

            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    if ((SquareStatus)buttons[i, j].GetValue(CurrentStatus) == SquareStatus.NoPawn)
                        UpdateButtonAvailability(i, j, currentPlayer);
                }
            }

            // Change player timer
            timeHandler.Switch();
        }

        /// <summary>
        /// Set the state of a button and change it's image accordingly
        /// </summary>
        /// <param name=button> button to be modified</param>
        /// <param name=newStatus> new status of the button</param>
        private void setButtonState(Button button, SquareStatus newStatus)
        {
            button.SetValue(CurrentStatus, newStatus);
            UpdateButtonStyle(button);
        }

        /// <summary>
        /// Updating button style considering its square status
        /// </summary>
        /// <param name="button">Button of the board game</param>
        private void UpdateButtonStyle(Button button)
        {
            SquareStatus status = (SquareStatus)button.GetValue(CurrentStatus);
            switch (status)
            {
                case SquareStatus.BlackPawn:
                    button.Style = (Style) Application.Current.FindResource("BlackPawn");
                    break;
                case SquareStatus.WhitePawn:
                    button.Style = (Style)Application.Current.FindResource("WhitePawn");
                    break;
                case SquareStatus.NoPawn:   // Same style for both
                default:
                    bool avaibleProperty = Convert.ToBoolean(button.GetValue(IsAvailableProperty));
                    if (avaibleProperty)
                    {
                        if (currentPlayer == SquareStatus.WhitePawn)
                            button.Style = (Style)Application.Current.FindResource("PlayableWhite");
                        else
                            button.Style = (Style)Application.Current.FindResource("PlayableBlack");
                    }
                    else
                        button.Style = (Style)Application.Current.FindResource("NoPawn");
                    break;
            }
        }

        /// <summary>
        /// Change the state of a square
        /// </summary>
        /// <param name="x">x coordonate</param>
        /// <param name="y">y coordonate</param>
        /// <param name="newStatus">new SquareStatus of the square</param>
        public void SetButtonState(int x, int y, SquareStatus newStatus)
        {
            setButtonState((Button) buttons[x, y], newStatus);
        }

        /// <summary>
        /// Will check in every direction to see if this square given as argument is playable
        /// Will then update his status according to the result of the search
        /// </summary>
        /// <param name=x> x position of the current square to check</param>
        /// <param name=y> y position of the current square to check</param>
        /// <param name=currentPlayer> status of the current player</param>
        private void UpdateButtonAvailability(int x, int y, SquareStatus currentPlayer)
        {
            int[,] directionToCheck = { { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, -1 }, { 0, 1 }, { 1, -1 }, { 1, 0 }, { 1, 1 } };

            bool isPlayable = false;

            for (int i = 0; i < 8; i++)
            {
                int dirX = directionToCheck[i, 0];
                int dirY = directionToCheck[i, 1];
                if (CheckOtherPawnFromDirection(dirX, dirY, x, y, currentPlayer))
                    isPlayable = true;
            }
            Button button = (Button) buttons[x, y];
            button.SetValue(IsAvailableProperty, isPlayable);
            UpdateButtonStyle(button);
        }

        /// <summary>
        /// check if at least one button is available to play
        /// </summary>
        /// <returns>true if there is one to play, false if there isn't</returns>
        private bool checkButtonsAvailability()
        {
            for (int i = 0; i < TOTAL_COLLUMN; i++)
            {
                for (int j = 0; j < TOTAL_ROW; j++)
                {
                    if ((bool)buttons[i, j].GetValue(IsAvailableProperty) == true && (SquareStatus)buttons[i, j].GetValue(CurrentStatus) == SquareStatus.NoPawn)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Will check in a given direction if the a pawn can be placed on the current empty square
        /// </summary>
        /// <param name=dirX> direction ot move for each step in x coordonate</param>
        /// <param name=dirY> direction ot move for each step in y coordonate</param>
        /// <param name=x> x coordonate of clicked button</param>
        /// <param name=y> y coordonate of clicked button</param>
        /// <param name=currentStatus> status of the current player</param>
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
        /// Will modify pawn status in a given direction
        /// </summary>
        /// <param name=dirX> direction ot move for each step in x coordonate</param>
        /// <param name=dirY> direction ot move for each step in y coordonate</param>
        /// <param name=x> x coordonate of clicked button</param>
        /// <param name=y> y coordonate of clicked button</param>
        /// <param name=currentStatus> status of the current player</param>
        private void setOtherPawnFromDirection(int dirX, int dirY, int x, int y, SquareStatus currentStatus)
        {
            x += dirX;
            y += dirY;

            // get the color of the current ennemy
            SquareStatus opponentStatus;
            if (currentStatus == SquareStatus.BlackPawn)
                opponentStatus = SquareStatus.WhitePawn;
            else
                opponentStatus = SquareStatus.BlackPawn;

            Button currentButton = (Button) buttons[x, y];
            SquareStatus nextPawnStatus = (SquareStatus)currentButton.GetValue(CurrentStatus);

            while (nextPawnStatus == opponentStatus)
            {
                setButtonState(currentButton, currentStatus);
                x += dirX;
                y += dirY;
                currentButton = (Button) buttons[x, y];
                nextPawnStatus = (SquareStatus)currentButton.GetValue(CurrentStatus);
            }
        }

        /// <summary>
        /// Count the score of a pawn type
        /// </summary>
        /// <param name=pawnType>pawn type (WHITE_PAWN or BLACK_PAWN)</param>
        /// <returns>score of pawnType</returns>
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
        /// </summary>
        /// <param name=pawnType>x and y position of square in grid</param>
        /// <returns>status of square at the [x,y] coordonate</returns>
        public SquareStatus getStatus(int x, int y)
        {
            return (SquareStatus)buttons[x, y].GetValue(CurrentStatus);
        }

        /// <summary>
        /// Get the playability of a square
        /// </summary>
        /// <param name=pawnType>x and y position of square in grid</param>
        /// <returns>playability of square at the [x,y] coordonate</returns>
        public bool getPlayability(int x, int y)
        {
            return (bool)buttons[x, y].GetValue(IsAvailableProperty);
        }
    }
}
