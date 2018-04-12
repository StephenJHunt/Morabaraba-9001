using System;
using System.Collections.Generic;

namespace Morabaraba_9001
{
    public interface IBoard
    {
        ICell occupant(string pos);
        List<Cell> Cows(CellState player);
        void Move(string piecePos, string movePos);
        void Shoot(string shootPos);
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
        string getMove(string prompt);

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
        Dictionary<string, ICell> board = new Dictionary<string, ICell>();
        public Board()
        {
            string[] positions = new string[] {"A1", "A4", "A7", "B2", "B4", "B6" , "C3", "C4", "C5", "D1", "D2", "D3", "D5", "D6", "D7", "E3", "E4", "E5", "F2", "F4", "F6", "G1", "G4", "G7" };
            foreach (string pos in positions)//initialising board with empty values
            {
                board.Add(pos, new Cell());
            }
        }
        public List<Cell> Cows(CellState player)
        {
            List<Cell> playerCells = new List<Cell>();
            foreach (Cell cell in board.Values)
            {
                if (cell.getState == player)
                {
                    playerCells.Add(cell);
                }
            }
            return playerCells;
        }
        

        public void Move(string piecePos, string movePos)
        {
            throw new NotImplementedException();
        }

        public ICell occupant(string pos)
        {
            throw new NotImplementedException();
        }

        public void Shoot(string shootPos)
        {
            throw new NotImplementedException();
        }
    }
    public class PowerLevel9000Cow : ICell
    {
        public CellState getState => throw new NotImplementedException();

        public void changeState(CellState changedState)
        {
            throw new NotImplementedException();
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

        virtual public void Move(string movePos)
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

        public string getMove(string prompt)
        {
            throw new NotImplementedException();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
