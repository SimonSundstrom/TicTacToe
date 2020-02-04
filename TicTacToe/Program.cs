using System;

//Board
//Initiate Board
//Win conditions
//Game states: Start, Player1 turn, Player 2 turn, Game over
//Marks
//Square
namespace TicTacToe
{
    public enum State
    {
        START,
        PLAYER1TURN,
        PLAYER2TURN,
        GAMEOVER
    }
    class Program
    {
        private int[] board;
        private const int PLAYER1 = 1;
        private const int PLAYER2 = 2;
        private int activePlayer;
        private bool gameOver;
        private int turn;
        private string test;
        private int hej;

        public delegate bool ConditionHandler(int x, int y);


        ConditionHandler conditionHandler;



        static void Main(string[] args)
        {
            Program program = new Program();
            program.HandleStates(State.START);

        }

        private void HandleStates(State state)
        {
            switch (state)
            {
                case State.START:
                    Console.WriteLine("Current State: START");
                    Console.WriteLine("Write Enter to start: ");
                    string input = Console.ReadLine();

                    if (input == "Enter")
                    {
                        InitializeGame();
                        state = activePlayer == 1 ? State.PLAYER1TURN : State.PLAYER2TURN;
                    }

                    break;
                case State.PLAYER1TURN:
                    Console.WriteLine("Current State: PLAYER1TURN");
                    activePlayer = PLAYER1;
                    Move();
                    state = gameOver == true ? State.GAMEOVER : State.PLAYER2TURN;
                    break;
                case State.PLAYER2TURN:
                    Console.WriteLine("Current State: PLAYER2TURN");
                    activePlayer = PLAYER2;
                    Move();
                    state = gameOver == true ? State.GAMEOVER : State.PLAYER1TURN;
                    break;
                case State.GAMEOVER:
                    Console.WriteLine("Current State: GAMEOVER");
                    state = State.START;
                    break;

            }
            HandleStates(state);
        }


        //Create Board
        private void CreateBoard()
        {
            board = new int[9];
        }
        private void InitializeGame()
        {



            gameOver = false;
            turn = 0;
            activePlayer = new Random().Next(1, 3);

            CreateBoard();
            DrawBoard();
        }


        private void Move()
        {
            turn++;
            int move = ReadInput();
            CheckWinConditionOld(move);
            //CheckWinConditionNew(move);

            DrawBoard();
            //Check win conditions
            //Horizontal
        }

        private void CheckWinConditionOld(int move)
        {
            board[move] = activePlayer;
            if (CheckRow(move, activePlayer) || CheckCol(move, activePlayer) || CheckDiagonal(move, activePlayer))
            {
                Console.WriteLine(activePlayer + " has won");
                gameOver = true;
            }
            else if (turn == 9)
            {
                Console.WriteLine("It´s a draw!");
                gameOver = true;
            }
            else
            {
                gameOver = false;
            }
        }

        private void CheckWinConditionNew(int move)
        {
            board[move] = activePlayer;
            conditionHandler = new ConditionHandler(CheckCol);
            conditionHandler += CheckDiagonal;
            conditionHandler += CheckRow;

            Delegate[] checkMethods = conditionHandler.GetInvocationList();

            foreach (ConditionHandler method in checkMethods)
            {
                if (method(move, activePlayer))
                {
                    Console.WriteLine(activePlayer + " has won");
                    gameOver = true;
                    return;
                }
            }
            if (gameOver == false && turn == 9)
            {
                Console.WriteLine("It´s a draw!");
                gameOver = true;
            }

        }


        private int ReadInput()
        {

            int move = 0;
            bool allowedMove = true;
            do
            {
                Console.WriteLine(activePlayer + " turn, make a move!");

                allowedMove = CheckInput(ref move, ref allowedMove);
            } while (allowedMove == false);


            return move;

        }

        private bool CheckInput(ref int move, ref bool allowedMove)
        {

            try
            {
                string moveString = Console.ReadLine();
                move = int.Parse(moveString);
                if (move < 0 || move > 8 || board[move] != 0)
                {
                    return false;
                }

            }
            catch (FormatException)
            {
                Console.WriteLine("Unable to convert '{0}'.");
                return false;
            }
            catch (OverflowException)
            {
                Console.WriteLine("'{0}' is out of range of the Int32 type.");
                return false;
            }
            finally
            {

            }
            return true;

        }

        private bool CheckRow(int move, int activePlayer)
        {
            Console.WriteLine("CheckRow");
            int row = move / 3;

            int firstSquareInRow = 0;
            switch (row)
            {
                case 0:
                    firstSquareInRow = 0;
                    break;
                case 1:
                    firstSquareInRow = 3;
                    break;
                case 2:
                    firstSquareInRow = 6;
                    break;
            }

            int activePlayerMarksInTheRow = 0;

            for (int i = firstSquareInRow; firstSquareInRow + 3 > i; i++)
            {
                if (board[i] == activePlayer)
                {
                    activePlayerMarksInTheRow++;
                }
            }
            if (activePlayerMarksInTheRow == 3)
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        private bool CheckCol(int move, int activePlayer)
        {
            Console.WriteLine("CheckCol");

            int col = move % 3;

            int firstSquareInCol = 0;
            switch (col)
            {
                case 0:
                    firstSquareInCol = 0;
                    break;
                case 1:
                    firstSquareInCol = 1;
                    break;
                case 2:
                    firstSquareInCol = 2;
                    break;
            }

            int activePlayerMarksInTheRow = 0;

            for (int i = firstSquareInCol; firstSquareInCol + 7 > i; i = i + 3)
            {
                if (board[i] == activePlayer)
                {
                    activePlayerMarksInTheRow++;
                }
            }
            if (activePlayerMarksInTheRow == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckDiagonal(int move, int activePlayer)
        {
            Console.WriteLine("CheckDiagonal");

            int activePlayerMarksInTheRow = 0;

            for (int i = 0; 8 >= i; i = i + 4)
            {
                if (board[i] == activePlayer)
                {
                    activePlayerMarksInTheRow++;
                }
            }
            if (activePlayerMarksInTheRow == 3)
            {
                return true;
            }
            else
            {
                activePlayerMarksInTheRow = 0;
                for (int i = 2; 6 >= i; i = i + 2)
                {
                    if (board[i] == activePlayer)
                    {
                        activePlayerMarksInTheRow++;
                    }
                }

            }
            if (activePlayerMarksInTheRow == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Draw board
        private void DrawBoard()
        {
            for (int i = 0; i < board.Length; i++)
            {
                if (i == 3 || i == 6)
                {
                    Console.WriteLine();
                }
                Console.Write(board[i] + " | ");
            }
            Console.WriteLine();
        }
    }



}

