using System;
using System.Collections.Generic;

namespace Morabaraba_9001
{
    public interface IBoard
    {
        List<ICell> Cows(CellState player);
        void Move(string piecePos, string movePos);
        void Shoot(string shootPos);
        List<ICell> getNeighbours(ICell cell);
        ICell getCell(string pos);
        string getMove(string prompt);
    }
    public interface ICell
    {
        CellState getState { get; }
        void changeState(CellState changedState);
        bool isMovable();
        bool isInMill();
        void Move(string movePos);
        string getPosition();
    }
    public interface IPlayer
    {
        Player player { get; }

    }
    public enum CellState { X, O, Empty }
    public enum Player { X, O }
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
        CellState state;
        public CellState getState => state;

        public void changeState(CellState changedState)
        {
            state = changedState;
        }

        public string getPosition()
        {
            throw new NotImplementedException();
        }

        public bool isInMill()
        {
            throw new NotImplementedException();
        }

        public bool isMovable()
        {
            throw new NotImplementedException();
        }

        public void Move(string movePos)
        {
            throw new NotImplementedException();
        }
    }
    public class Board : IBoard
    {
        Dictionary<string, List<string>> neighbours = new Dictionary<string, List<string>> {
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
        Dictionary<string, ICell> board = new Dictionary<string, ICell>();
        public Board()
        {
            string[] positions = new string[] {"A1", "A4", "A7", "B2", "B4", "B6" , "C3", "C4", "C5", "D1", "D2", "D3", "D5", "D6", "D7", "E3", "E4", "E5", "F2", "F4", "F6", "G1", "G4", "G7" };
            foreach (string pos in positions)//initialising board with empty values
            {
                board.Add(pos, new Cell());
            }
        }
        public List<ICell> Cows(CellState player)
        {
            List<ICell> playerCells = new List<ICell>();
            foreach (ICell cell in board.Values)
            {
                if (cell.getState == player)
                {
                    playerCells.Add(cell);
                }
            }
            return playerCells;
        }

        public ICell getCell(string pos)
        {
            return board[pos];
        }

        public string getMove(string prompt)
        {
            string input;
            while (true)
            {
                Console.Write(prompt);
                input = Console.ReadLine().ToUpper();
                if(board.ContainsKey(input))
                {
                    break;
                }
                Console.WriteLine("Please choose a valid position. you'll get snuggles ^w^ :3");
            }
            return input; 
        }

        public List<ICell> getNeighbours(ICell cell)
        {
            List<ICell> neighbourList = new List<ICell>();

            foreach (string pos in neighbours[cell.getPosition()])
            {
                neighbourList.Add(getCell(pos));
            }
            return neighbourList;
        }

        public void Move(string piecePos, string movePos)
        {
            throw new NotImplementedException();
        }
        
        public void Shoot(string shootPos)
        {
            throw new NotImplementedException();
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
        Player gameplayer;
        public Player player => gameplayer;
    }
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
