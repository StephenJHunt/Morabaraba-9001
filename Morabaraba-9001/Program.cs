using System;
using System.Collections.Generic;
using System.Linq;

namespace Morabaraba_9001
{
    


    public interface IBoard
    {
        List<ICell> Cows(Player player);
        void Move(IPlayer player);
        void Shoot(IPlayer player);
        List<ICell> getNeighbours(string pos);
        ICell getCell(string pos);
        bool isMovable(string pos);
        bool isInMill(string pos);
        bool allInMill(Player player);
    }
    public interface ICell
    {
        Player getState { get; }
        void changeState(Player changedState);
    }
    public interface IPlayer
    {
        Player playerID { get; }
        string getMove(string prompt);
        Player getOpponent();
    }
    //public enum CellState { X, O, Empty }
    public enum Player { X, O, None }
    public interface IGameManager
    {
        void startGame();
        void placingPhase();
        void movingPhase();
        void endGame();

    }
    public class invalidMoveException : ApplicationException { }
    public class Cell : ICell
    {
        Player state;
        public Player getState => state;

        public Cell()
        {
            state = Player.None;
        }

        public void changeState(Player changedState)
        {
            state = changedState;
        }
    }
    public class Board : IBoard
    {
        public static string[] validPositions = new string[] {"A1", "A4", "A7", "B2", "B4", "B6" , "C3", "C4", "C5", "D1", "D2", "D3", "D5", "D6", "D7", "E3", "E4", "E5", "F2", "F4", "F6", "G1", "G4", "G7" };
        public static Dictionary<string, List<string>> neighbours = new Dictionary<string, List<string>> {
            { "A1", new List<string> { "A4", "D1", "B2" } },
            { "A4", new List<string> { "A1", "B4", "A7" } },
            { "A7", new List<string> { "A4", "B6", "D7" } },
            { "B2", new List<string> { "A1", "D2", "C3", "B4" } },
            { "B4", new List<string> { "B2", "A4", "C4", "B6" } },
            { "B6", new List<string> { "B4", "C5", "D6", "A7" } },
            { "C3", new List<string> { "B2", "C4", "D3" } },
            { "C4", new List<string> { "C3", "B4", "C5" } },
            { "C5", new List<string> { "C4", "D5", "B6" } },
            { "D1", new List<string> { "A1", "G1", "D2" } },
            { "D2", new List<string> { "D1", "F2", "D3", "B2" } },
            { "D3", new List<string> { "D2", "E3", "C3" } },
            { "D5", new List<string> { "E5", "D6", "C5" } },
            { "D6", new List<string> { "D5", "F6", "D7", "B6" } },
            { "D7", new List<string> { "D6", "G7", "A7" } },
            { "E3", new List<string> { "F2", "E4", "D3" } },
            { "E4", new List<string> { "E3", "F4", "E5" } },
            { "E5", new List<string> { "E4", "F6", "D5" } },
            { "F2", new List<string> { "G1", "F4", "E3", "D2" } },
            { "F4", new List<string> { "F2", "G4", "F6", "E4" } },
            { "F6", new List<string> { "F4", "G7", "D6", "E5" } },
            { "G1", new List<string> { "D1", "G4", "F2" } },
            { "G4", new List<string> { "G1", "F4", "G7" } },
            { "G7", new List<string> { "G4", "F6", "D7" } }
        };
        public static List<string[]> mills = new List<List<string>>
        {
            new string[] {"A1", "A4", "A7"},
            new string[] {"B2", "B4", "B6"},
            new string[] {"C3", "C4", "C5"},
            new string[] {"D1", "D2", "D3"},
            new string[] {"D5", "D6", "D7"},
            new string[] {"E3", "E4", "E5"},
            new string[] {"F2", "F4", "F6"},
            new string[] {"G1", "G4", "G7"},

            new string[] {"A1", "D1", "G1"},
            new string[] {"B2", "D2", "F2"},
            new string[] {"C3", "D3", "E3"},
            new string[] {"A4", "B4", "C4"},
            new string[] {"E4", "F4", "G4"},
            new string[] {"C5", "D5", "E5"},
            new string[] {"B6", "D6", "F6"},
            new string[] {"A7", "D7", "G7"},

            new string[] {"A1", "B2", "C3"},
            new string[] {"G1", "F2", "E3"},
            new string[] {"G7", "F6", "E5"},
            new string[] { "A7", "B6", "C5"}
        }; 
        public Dictionary<string, ICell> board = new Dictionary<string, ICell>();
        public Board()
        {
            foreach (string pos in validPositions)//initialising board with empty values
            {
                board.Add(pos, new Cell());
            }
        }
        public List<ICell> Cows(Player player)
        {
            var query = from cell in board.Values.ToList()
                        where cell.getState == player
                        select cell;
            return query.ToList();
        }

        public ICell getCell(string pos)
        {
            return board[pos];
        }



        public List<ICell> getNeighbours(string pos)
        {
            List<ICell> neighbourList = new List<ICell>();
            foreach (string npos in neighbours[pos])
            {
                neighbourList.Add(getCell(npos));
            }
            return neighbourList;
        }

        public void Move(IPlayer player)
        {
            string piecePos, placePos;
            while (true)
            {
                piecePos = player.getMove("Select piece to move: ");
                if (board[piecePos].getState == player.playerID)
                {

                    if (isMovable(piecePos))
                        break;
                }
            }

            while (true)
            {
                placePos = player.getMove("Select position to place" + piecePos + ": ");
                if (board[piecePos].getState == player.playerID)
                {
                    
                }
            }

            board[placePos].changeState(board[piecePos].getState);
            board[piecePos].changeState(Player.None);
        }
        
        public void Shoot(IPlayer player)
        {
            string shootPos;
            while (true)
            {
                shootPos = player.getMove("Select piece to move: ");
                if (board[shootPos].getState == player.getOpponent())
                {
                    break;
                }
            }
            board[shootPos].changeState(Player.None);
        }

        public bool isMovable(string pos)
        {
            List<ICell> emptyNeighbours =
            (from cell in getNeighbours(pos)
             where cell.getState == Player.None
             select cell).ToList();
            return emptyNeighbours.Count() > 0;
        }

        public bool allInMill(Player player)
        {
            foreach (string pos in board.Keys)
            {
                if (board[pos].getState == player && !isInMill(board[pos]))
                    return false; 
            }
            return true;
        }

        public bool isInMill(string pos)
        {
            List<string[]> relevantmills = mills.Where(mill => mill.Contains(pos));
            List<ICell[]> millStates = relevantmills.Select(mill => mill.Select(p => getCell(p)));
            
            foreach (ICell[] mill in millStates)
            {
                if ((mill[0] == Player.X && mill[1] == Player.X && mill[2] == Player.X) ||
                    (mill[0] == Player.O && mill[1] == Player.O && mill[2] == Player.O))
                {
                    return true;
                }
            }
            return false;
        }
    }
    

    public class MorabarabaManager : IGameManager
    {
        
        public void endGame()
        {
            throw new NotImplementedException();
        }

        public void movingPhase()
        {
            throw new NotImplementedException();
        }

        public void placingPhase()
        {
            throw new NotImplementedException();
        }

        public void startGame()
        {
            throw new NotImplementedException();
        }
    }
    public class GamePlayer : IPlayer
    {
        public GamePlayer(Player player)
        {
            gameplayer = player;
        }
        Player gameplayer;
        public Player playerID => gameplayer;
        public Player getOpponent()
        {
            if (gameplayer == Player.X)
            {
                return Player.O;
            }
            return Player.X;
        }
        public string getMove(string prompt)
        {
            string input;
            while (true)
            {
                Console.Write(prompt);
                input = Console.ReadLine().ToUpper();
                if (Board.validPositions.Contains(input))
                {
                    break;
                }
                Console.WriteLine("Please choose a valid position. you'll get snuggles ^w^ :3");
            }
            return input;
        }
    }
    class Program
    {
        

        static void Main(string[] args)
        {
            
        }
    }
}
