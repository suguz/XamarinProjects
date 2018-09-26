using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TicTacToe
{
    public partial class MainPage : ContentPage
    {
        #region Enum
        public enum Piece
        {
            Empty = 0,
            Cross,
            Circle
        }
        #endregion

        #region Fields
        private Piece[] _board;
        private bool _gameOver;
        private Dictionary<int, Button> _boardIndexMapping;
        private readonly Piece _cpuPiece = Piece.Circle; // set CPU as second player. To set as first player, use Piece.Cross instead
        private Piece _currentPieceTurn;
        private readonly static Dictionary<Piece, string> _pieceSymbol = new Dictionary<Piece, string>
        {
            { Piece.Cross, "X" },
            { Piece.Circle, "O" },
            { Piece.Empty, string.Empty }
        };
        #endregion

        //Label label1;
        public MainPage()
        {
            InitializeComponent();
            initBoardMapping();
            
            Piece humanPiece = _cpuPiece == Piece.Cross ? Piece.Circle : Piece.Cross;
            //label1 = this.FindByName<Label>("label1");
            label1.Text = string.Format("{0} = Human   {1} = CPU", _pieceSymbol[humanPiece], _pieceSymbol[_cpuPiece]);
            newGame();
        }

        private void newGame()
        {
            _gameOver = false;
            _board = new Piece[9];

            _currentPieceTurn = Piece.Cross;
            foreach (int boardIndex in _boardIndexMapping.Keys)
            {
                Button btn = _boardIndexMapping[boardIndex];
                if (btn != null)
                {
                    btn.Text = string.Empty;
                }
            }

            //if the starting player is non Human, then immediately takes turn
            if (_currentPieceTurn == _cpuPiece)
            {
                takeCpuTurn(_board, _cpuPiece);
            }
        }

        private void initBoardMapping()
        {
            _boardIndexMapping = new Dictionary<int, Button>
            {
                {0, button1},
                {1, button2},
                {2, button3},
                {3, button4},
                {4, button5},
                {5, button6},
                {6, button7},
                {7, button8},
                {8, button9}
            };
        }

        public int GetNextMove(Piece[] board, Piece yourPiece)
        {
            // START YOUR CODE HERE

            // just scan for any available move
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == Piece.Empty)
                    return i;
            }
            throw new ArgumentException("Nowhere left to move!");
        }

        private void takeCpuTurn(Piece[] board, Piece yourPiece)
        {
            try
            {
                int nextMove = GetNextMove(board, yourPiece);
                if (nextMove > -1)
                {
                    takeTurn(board, nextMove);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Exception Error", ex.Message, "OK");
                //MessageBox.Show(ex.Message);
            }
        }

        private void takeTurn(Button btn)
        {
            int boardIndex;

            if (btn != null)
                boardIndex = getBoardIndexFromButton(btn);
            else
                boardIndex = -1;

            takeTurn(_board, boardIndex);
        }

        private void takeTurn(Piece[] board, int boardIndex)
        {
            if (boardIndex >= 0 && board[boardIndex] == Piece.Empty)
            {
                _board[boardIndex] = _currentPieceTurn;
                _boardIndexMapping[boardIndex].Text = _pieceSymbol[_currentPieceTurn];
                checkWinner(_board);

                _currentPieceTurn = _currentPieceTurn == Piece.Cross ? Piece.Circle : Piece.Cross;

                // if the next turn is CPU and game is not over
                if (_currentPieceTurn == _cpuPiece && !_gameOver)
                {
                    takeCpuTurn(_board, _cpuPiece);
                }
            }
        }

        private void checkWinner(Piece[] board)
        {
            bool state1 = board[0] != Piece.Empty && board[0] == board[1] && board[1] == board[2];
            bool state2 = board[0] != Piece.Empty && board[0] == board[3] && board[3] == board[6];
            bool state3 = board[0] != Piece.Empty && board[0] == board[4] && board[4] == board[8];

            bool state4 = board[1] != Piece.Empty && board[1] == board[4] && board[4] == board[7];
            bool state5 = board[2] != Piece.Empty && board[2] == board[4] && board[4] == board[6];
            bool state6 = board[2] != Piece.Empty && board[2] == board[5] && board[5] == board[8];

            bool state7 = board[3] != Piece.Empty && board[3] == board[4] && board[4] == board[5];
            bool state8 = board[6] != Piece.Empty && board[6] == board[7] && board[7] == board[8];

            bool winnerState = state1 || state2 || state3 || state4 || state5 || state6 || state7 || state8;
            if (winnerState && !_gameOver)
            {
                _gameOver = true;
                string winner = string.Empty;
                if (state1 || state2 || state3)
                {
                    winner = board[0].ToString();
                }
                else if (state4)
                {
                    winner = board[1].ToString();
                }
                else if (state5 || state6)
                {
                    winner = board[2].ToString();
                }
                else if (state7)
                {
                    winner = board[3].ToString();
                }
                else if (state8)
                {
                    winner = board[6].ToString();
                }
                //MessageBox.Show(this, string.Format("Game Over, {0} win the game!", winner));
                DisplayAlert("Game Over", string.Format("Game Over, {0} win the game!", winner), "OK");
            }
        }

        private int getBoardIndexFromButton(Button button)
        {
            foreach (int boardIndex in _boardIndexMapping.Keys)
            {
                if (_boardIndexMapping[boardIndex] == button)
                {
                    return boardIndex;
                }
            }
            return -1;
        }
        private void button1_Clicked(object sender, EventArgs e)
        {
            if (_gameOver)
            {
                return;
            }

            if (_currentPieceTurn == _cpuPiece)
            {
                //MessageBox.Show("Please wait until your turn!");
                DisplayAlert("Hold on", "Please wait until your turn!", "OK");
                return;
            }

            takeTurn(sender as Button);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            newGame();
        }
    }
}
