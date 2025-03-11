namespace Lumememm;

public partial class TicTacToePage : ContentPage
{
    private Grid grid;
    private Button newGameButton;
    private Button randomPlayerButton;
    private Button toggleBotButton;
    private Label statusLabel;
    private string currentPlayer = "X";
    private string[,] gameBoard = new string[3, 3];
    private bool gameOver = false;
    private bool isBotEnabled = true;
    private Random random = new Random();

    public TicTacToePage()
    {
        BackgroundImageSource = "play.png";

        grid = new Grid
        {
            BackgroundColor = Color.FromArgb("#333333")
        };

        for (int i = 0; i < 3; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        }

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                var cellButton = new Button
                {
                    FontSize = 40,
                    TextColor = Color.FromArgb("#ffffff"),
                    BackgroundColor = (row + col) % 2 == 0 ? Color.FromArgb("#e62291") : Color.FromArgb("#000000")
                };
                cellButton.Clicked += CellButton_Clicked;
                grid.Children.Add(cellButton);

                Grid.SetRow(cellButton, row);
                Grid.SetColumn(cellButton, col);

                gameBoard[row, col] = string.Empty;
            }
        }

        newGameButton = new Button
        {
            Text = "New Game",
            BackgroundColor = Color.FromArgb("#b314e3"),
            TextColor = Color.FromArgb("#ffffff"),
            FontSize = 15
        };
        newGameButton.Clicked += NewGameButton_Clicked;

        randomPlayerButton = new Button
        {
            Text = "Random Player",
            BackgroundColor = Color.FromArgb("#b314e3"),
            TextColor = Color.FromArgb("#ffffff"),
            FontSize = 15
        };
        randomPlayerButton.Clicked += RandomPlayerButton_Clicked;

        toggleBotButton = new Button
        {
            Text = isBotEnabled ? "Disable Bot" : "Enable Bot",
            BackgroundColor = Color.FromArgb("#b314e3"),
            TextColor = Color.FromArgb("#ffffff"),
            FontSize = 15
        };
        toggleBotButton.Clicked += ToggleBotButton_Clicked;

        statusLabel = new Label
        {
            Text = "Player X's Turn",
            FontSize = 15,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Color.FromArgb("#ffffff")
        };

        var buttonLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Children = { randomPlayerButton, newGameButton, toggleBotButton }
        };

        var mainLayout = new StackLayout
        {
            Children = { grid, statusLabel, buttonLayout }
        };

        Content = mainLayout;
    }

    private void ToggleBotButton_Clicked(object sender, EventArgs e)
    {
        isBotEnabled = !isBotEnabled;
        toggleBotButton.Text = isBotEnabled ? "Disable Bot" : "Enable Bot";
    }

    private void CellButton_Clicked(object sender, EventArgs e)
    {
        if (gameOver) return;

        var button = sender as Button;
        var position = GetButtonPosition(button);
        int row = position.Item1;
        int col = position.Item2;

        if (gameBoard[row, col] == string.Empty)
        {
            button.Text = currentPlayer;
            gameBoard[row, col] = currentPlayer;

            if (CheckWinner())
            {
                statusLabel.Text = $"{currentPlayer} Wins!";
                gameOver = true;
                DisplayAlert("Game Over", $"{currentPlayer} wins! Do you want to play again?", "Yes", "No");
                return;
            }
            else if (IsBoardFull())
            {
                statusLabel.Text = "It's a Draw!";
                gameOver = true;
                DisplayAlert("Game Over", "It's a draw! Do you want to play again?", "Yes", "No");
                return;
            }

            currentPlayer = (currentPlayer == "X") ? "O" : "X";
            statusLabel.Text = $"Player {currentPlayer}'s Turn";

            if (isBotEnabled && currentPlayer == "O" && !gameOver)
            {
                MakeBotMove();
            }
        }
    }

    private void NewGameButton_Clicked(object sender, EventArgs e)
    {
        gameOver = false;
        currentPlayer = "X";
        statusLabel.Text = "Player X's Turn";

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                gameBoard[row, col] = string.Empty;

                foreach (var child in grid.Children)
                {
                    if (child is Button button && Grid.GetRow(button) == row && Grid.GetColumn(button) == col)
                    {
                        button.Text = string.Empty;
                        break;
                    }
                }
            }
        }
    }

    private void RandomPlayerButton_Clicked(object sender, EventArgs e)
    {
        currentPlayer = random.Next(0, 2) == 0 ? "X" : "O";
        statusLabel.Text = $"Player {currentPlayer}'s Turn";

        if (isBotEnabled && currentPlayer == "O")
        {
            MakeBotMove();
        }
    }


    private void MakeBotMove()
    {
        if (gameOver || !isBotEnabled) return;

        List<Tuple<int, int>> emptyCells = new List<Tuple<int, int>>();

        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (gameBoard[row, col] == string.Empty)
                {
                    emptyCells.Add(new Tuple<int, int>(row, col));
                }
            }
        }

        if (emptyCells.Count == 0) return;

        var move = emptyCells[random.Next(emptyCells.Count)];
        int botRow = move.Item1;
        int botCol = move.Item2;

        foreach (var child in grid.Children)
        {
            if (child is Button button && Grid.GetRow(button) == botRow && Grid.GetColumn(button) == botCol)
            {
                button.Text = "O";
                gameBoard[botRow, botCol] = "O";
                break;
            }
        }

        if (CheckWinner())
        {
            statusLabel.Text = "O Wins!";
            gameOver = true;
            DisplayAlert("Game Over", "O wins! Do you want to play again?", "Yes", "No");
            return;
        }
        else if (IsBoardFull())
        {
            statusLabel.Text = "It's a Draw!";
            gameOver = true;
            DisplayAlert("Game Over", "It's a draw! Do you want to play again?", "Yes", "No");
            return;
        }

        currentPlayer = "X";
        statusLabel.Text = "Player X's Turn";
    }

    private Tuple<int, int> GetButtonPosition(Button button)
    {
        int row = Grid.GetRow(button);
        int col = Grid.GetColumn(button);
        return Tuple.Create(row, col);
    }

    private bool CheckWinner()
    {
        for (int i = 0; i < 3; i++)
        {
            if (gameBoard[i, 0] == currentPlayer && gameBoard[i, 1] == currentPlayer && gameBoard[i, 2] == currentPlayer)
                return true;
            if (gameBoard[0, i] == currentPlayer && gameBoard[1, i] == currentPlayer && gameBoard[2, i] == currentPlayer)
                return true;
        }
        if (gameBoard[0, 0] == currentPlayer && gameBoard[1, 1] == currentPlayer && gameBoard[2, 2] == currentPlayer)
            return true;
        if (gameBoard[0, 2] == currentPlayer && gameBoard[1, 1] == currentPlayer && gameBoard[2, 0] == currentPlayer)
            return true;

        return false;
    }

    private bool IsBoardFull()
    {
        foreach (var cell in gameBoard)
        {
            if (string.IsNullOrEmpty(cell))
                return false;
        }
        return true;
    }
}
