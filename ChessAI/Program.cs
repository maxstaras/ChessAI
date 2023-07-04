using gpuNeuralNet2;
using System;
using System.ComponentModel;

namespace ChessAI // Note: actual namespace depends on the project name.
{

    public class Square
    {
        public Piece? piece;
        public bool empty;
        public Square( Piece? pieceValue, bool isEmpty)
        {
            piece = pieceValue;
            
            empty = isEmpty;
        }
    }
    public class Piece
    {
        public PieceType type;
        public bool white;
        public int x, y;
        public bool pinned = false;

        public Piece(bool white, PieceType type, int x, int y)
        {
            this.type = type;
            this.white = white;
            this.x = x;
            this.y = y;
        }
    }

    public enum PieceType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        King,
        Queen
    }

    public struct Move
    {
        public Piece piece;
        public int toX, toY;
    }

    internal class Program
    {

        public string boardFEN = "";
        public static bool playingWhite;

        public static Square[,] board;
        public static List<Piece> pieces;

        public int blackChecks;
        public int whiteChecks;

        int[] knightOffsetsX = { -2, -2, -1, 1, 2, 2, 1, -1 };
        int[] knightOffsetsY = { -1, 1, 2, 2, 1, -1, -2, -2 };

        public static Dictionary<string, PieceType> FENLinks = new Dictionary<string, PieceType>() { { "r", PieceType.Rook }, { "n", PieceType.Knight }, { "k", PieceType.King }, { "q", PieceType.Queen }, { "b", PieceType.Bishop }, { "p", PieceType.Pawn } };
        static void Main(string[] args)
        {
            loadFEN("8/8/8/4p1K1/2k1P3/8/8/8");
            playingWhite = true;
            Console.WriteLine(getFEN());
        }

        public static void loadFEN(string FEN)
        {
            clearBoard();
            int posX = 0;
            int posY = 7;
            foreach (char c in FEN.ToCharArray())
            {
                if (c == '/') { posX = 0; posY--; }
                else
                {
                    if (int.TryParse(c.ToString(), out int val))
                    {
                        posX += val;
                    }
                    else
                    {
                        string lower = c.ToString().ToLower();
                        Piece p = new Piece(lower != c.ToString(), FENLinks[lower], posX, posY);
                        pieces.Add(p);
                        board[posX, posY] = new Square(p, false);
                        posX++;
                    }




                }


            }
        }


        public static string getFEN()
        {
            string output = "";
            for (int y = 7; y >= 0; y--)
            {
                int spaceCounter = 0;
                for (int x = 0; x < 8; x++)
                {
                    if (board[x, y].empty)
                    {
                        spaceCounter++;
                    }
                    else
                    {
                        output += spaceCounter > 0 ? spaceCounter : "";
                        spaceCounter = 0;
                        PieceType p = board[x, y].piece.ThrowIfNull().type;
                        string code = FENLinks.ReverseLookup(p)[0];
                        output += board[x, y].piece.ThrowIfNull().white ? code.ToUpper() : code.ToLower();
                    }
                }
                output += spaceCounter > 0 ? spaceCounter : "";
                output += y == 0 ? "" : "/";
            }
            return output;
        }

        public static void clearBoard()
        {
            pieces = new List<Piece>();
            board = new Square[8, 8];
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    board[x, y] = new Square(null, true);
                }
            }
        }
        public List<Move> findAllLegalMoves()
        {
            calculatePinsAndChecks();
            List<Move> moves = new List<Move>();
            foreach (Piece p in pieces)
            {
                if (p.white == playingWhite)
                {
                    switch (p.type)
                    {
                        case PieceType.Pawn:
                            {
                                if (playingWhite)
                                {
                                    if (!squareOccupied(p.x, p.y + 1))
                                    {

                                    }
                                }
                                else
                                {

                                }
                                break;
                            }
                        case PieceType.Knight:
                            {
                                break;
                            }
                        case PieceType.Rook:
                            {
                                break;
                            }
                        case PieceType.Bishop:
                            {
                                break;
                            }
                        case PieceType.Queen:
                            {
                                break;
                            }
                        case PieceType.King:
                            {
                                break;
                            }
                    }
                }
                
                
            }
            return moves;
        }

        public void calculatePinsAndChecks()
        {
            foreach (Piece p in pieces)
            {
                if (p.type == PieceType.King)
                {
                    // Search for Knight checks

                    for (int i = 0; i < 8; i++)
                    {
                        if (squareOccupied(knightOffsetsX[i], knightOffsetsY[i]))
                        {
                            if (board[knightOffsetsX[i], knightOffsetsY[i]].piece.ThrowIfNull().type == PieceType.Knight && board[knightOffsetsX[i], knightOffsetsY[i]].piece.ThrowIfNull().white != p.white)
                            {
                                if (p.white) { whiteChecks++; }
                                else { blackChecks++; }
                            }
                        }
                    }
                    
                }
                
            }
        }

        public bool squareOccupied(int posX, int posY)
        {
            if (posX < 8 && posX >= 0 && posY < 8 && posY >= 0)
            {
                return !board[posX, posY].empty;
            }
            return true;
        }
    }
}
