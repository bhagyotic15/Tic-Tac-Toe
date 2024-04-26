using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class TicTacToeGame
{
    private char[,] TicTacToeBoard;
    private int currentPlayer;
    private int totalMoves;
    private string[] playerNames;

    public TicTacToeGame()
    {
        // Initializing the Tic Tac Toe Board and game 
        TicTacToeBoard = new char[,]
        {
            { '1', '2', '3' },
            { '4', '5', '6' },
            { '7', '8', '9' }
        };
        currentPlayer = 1;
        totalMoves = 0;
        playerNames = new string[2];
    }

    public void StartGame()
    {
        Console.WriteLine("Welcome to Tic Tac Toe Game! \n");
        Console.WriteLine("\n1.The game is played on a grid that's 3 squares by 3 squares.\n\n2.Player 1 is \"X\" and Player 2 is \"O\". Players take turns putting their marks in empty squares.\n\n3.The first player to get 3 of her marks in a row(horizontally, vertically or diagonally) is the winner.\n\n4.When all 9 squares are full, the game is over. If no player has 3 marks in a row, the game ends in a tie.\n\n5.You can put x or o in by typing the number you want to put it at");
        Console.ReadKey(false);
        Console.Clear();
        Console.WriteLine(" $Press Enter to start the game.$ ");
        Console.ReadLine();

        while (true)
        {
            DisplayMenu();

            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
            {
                Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
            }

            if (choice == 3)
            {
                Console.WriteLine("Thank you for playing Tic Tac Toe!");
                break; // Exit the game
            }

            SetGameMode(choice);

            do
            {
                DisplayTicTacToeBoard();
                GetPlayerMove();

                if (CheckWinner())
                {
                    DisplayTicTacToeBoard();
                    Console.WriteLine($"Player {currentPlayer} ({playerNames[currentPlayer]}) wins!");
                    break;
                }

                totalMoves++;

                SwitchPlayer();



                if (totalMoves == 9)
                {
                    DisplayTicTacToeBoard();
                    Console.WriteLine("It's a draw!");
                    break;
                }

                if (currentPlayer == 0 && choice == 1 && totalMoves < 8)
                {
                    // If in single-player mode, let the computer make a move
                    ComputerMove();

                    if (CheckWinner())
                    {
                        DisplayTicTacToeBoard();
                        Console.WriteLine($"computer wins!");
                        break;
                    }
                    SwitchPlayer(); // Only switch if the computer successfully made a move
                }

            } while (true);

            Console.WriteLine("Do you want to play again? (Y/N)");
            char playAgain = char.ToUpper(Console.ReadKey().KeyChar);
            if (playAgain != 'Y')
            {
                Console.WriteLine("Thank you for playing Tic Tac Toe!");
                break;
            }
            else
            {
                ResetGame();
            }
        }
    }

    private void DisplayMenu()
    {
        // Menu for to choose mode 
        Console.Clear();
        Console.WriteLine("\nChoose a game mode:");
        Console.WriteLine("1. Single Player (Player vs Computer)");
        Console.WriteLine("2. Duo Mode (Player vs Player)");
        Console.WriteLine("3. Exit");
    }

    private void SetGameMode(int choice)
    {
        Console.Clear();
        Console.WriteLine(choice == 1 ? "Single Player Mode" : "Duo Mode");
        Console.WriteLine("Press the corresponding keys to make a move on the TicTacToeBoard.");

        if (choice == 2)
        {
            Console.Write("Enter Player 1's name: ");
            playerNames[0] = Console.ReadLine();
            Console.Write("Enter Player 2's name: ");
            playerNames[1] = Console.ReadLine();
        }

        Console.WriteLine($"Player 1 (X) - {playerNames[0]} | Player 2 (O) - {playerNames[1]}\n");
    }

    private void DisplayTicTacToeBoard()
    {
        Console.Clear();
        Console.WriteLine($" {TicTacToeBoard[0, 0]} | {TicTacToeBoard[0, 1]} | {TicTacToeBoard[0, 2]} ");
        Console.WriteLine("-----------");
        Console.WriteLine($" {TicTacToeBoard[1, 0]} | {TicTacToeBoard[1, 1]} | {TicTacToeBoard[1, 2]} ");
        Console.WriteLine("-----------");
        Console.WriteLine($" {TicTacToeBoard[2, 0]} | {TicTacToeBoard[2, 1]} | {TicTacToeBoard[2, 2]} ");
        Console.WriteLine();
        Console.WriteLine($"Player {currentPlayer}'s turn ({playerNames[currentPlayer]}). Enter your move (1-9).");
    }

    private void GetPlayerMove()
    {
        // getting move from the player
        int move;
        while (!int.TryParse(Console.ReadLine(), out move) || !IsValidMove(move))
        {
            Console.WriteLine("Invalid move. Please enter a valid position (1-9).");
        }

        PlaceMarker(move);
    }

    private bool IsValidMove(int move)
    {
        return move >= 1 && move <= 9 && IsPositionAvailable(move);
    }

    private bool IsPositionAvailable(int move)
    {
        int row = (move - 1) / 3;
        int col = (move - 1) % 3;
        return TicTacToeBoard[row, col] != 'X' && TicTacToeBoard[row, col] != 'O';
    }

    private void PlaceMarker(int move)
    {
        char marker = (currentPlayer == 1) ? 'X' : 'O';
        UpdateTicTacToeBoard(move, marker);
    }

    private void UpdateTicTacToeBoard(int move, char marker)
    {
        int row = (move - 1) / 3;
        int col = (move - 1) % 3;
        TicTacToeBoard[row, col] = marker;
    }

    private void SwitchPlayer()
    {
        // Switching the Players turns
        if (currentPlayer == 1) { currentPlayer = 0; }
        else { currentPlayer = 1; }
    }

    private bool CheckWinner()
    {
        // Check rows, columns, and diagonals
        for (int i = 0; i < 3; i++)
        {
            if (WinSituation(TicTacToeBoard[i, 0], TicTacToeBoard[i, 1], TicTacToeBoard[i, 2]) ||
                WinSituation(TicTacToeBoard[0, i], TicTacToeBoard[1, i], TicTacToeBoard[2, i]))
            {
                return true;
            }
        }

        // Check diagonals
        if (WinSituation(TicTacToeBoard[0, 0], TicTacToeBoard[1, 1], TicTacToeBoard[2, 2]) ||
            WinSituation(TicTacToeBoard[0, 2], TicTacToeBoard[1, 1], TicTacToeBoard[2, 0]))
        {
            return true;
        }

        return false;
    }

    private bool WinSituation(char cell1, char cell2, char cell3)
    {
        return cell1 == cell2 && cell2 == cell3;
    }

    private void ComputerMove()
    {
        int move;
        // Try to win
        move = FindBlockingMove();
        Console.WriteLine(move);

        // if there no blocking move, computer choose a random empty cell
        Console.WriteLine($"Computer chose position {move}.");
        PlaceMarker(move);
        Thread.Sleep(2000); // delay to show computers' move
    }


    private List<int> GetEmptyCells() // getting empty cells from the board
    {
        List<int> emptyCells = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (TicTacToeBoard[i, j] != 'X' && TicTacToeBoard[i, j] != 'O')
                {
                    // Convert 2D index to 1D position (1-9)
                    int position = i * 3 + j + 1;
                    emptyCells.Add(position);
                }
            }
        }

        return emptyCells;
    }

    private int FindBlockingMove()
    {
        // Computer choose the randomly empty cell
        Random random = new Random();
        List<int> emptyCells = (GetEmptyCells());
        int move = emptyCells[random.Next(emptyCells.Count)];


        return move;
    }

    private void ResetGame()
    {
        // Reset the Board and game for new game
        TicTacToeBoard = new char[,]
        {

            { '1', '2', '3' },
            { '4', '5', '6' },
            { '7', '8', '9' }
        };

        currentPlayer = 1;
        totalMoves = 0;
        Console.WriteLine("\nPress Enter to start a new game.");
        Console.ReadLine();

    }
}

// Main Function 
class Program
{
    static void Main()
    {
        TicTacToeGame game = new TicTacToeGame();
        game.StartGame();
    }
}