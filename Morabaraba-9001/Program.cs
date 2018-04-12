using System;
using System.Collections.Generic;

namespace Morabaraba_9001
{
    public interface IBoard
    {
        ICow occupant(string pos);
        IEnumerable<int> Cows(Player player);
        void Move(string piecePos, string movePos);
        void Shoot(string shootPos);
        void makeFlying(Player player);
    }
    public interface ICow
    {
        bool isMovable();
        bool isInMill();
        void Move(string movePos);
        string getPosition();
        CowStatus GetStatus();
    }
    public interface IPlayer
    {
        Player player { get; }
        string getMove(string prompt);

    }
    public enum Player { X, O }
    public enum CowStatus {Unplaced, Placed, Flying, Shot}
    public interface IGameManager
    {
        void startGame();
        void placingPhase();
        void movingPhase();
        void endGame();

    }
    public class invalidMoveException : ApplicationException { }
    public class Board : IBoard
    {
        public IEnumerable<int> Cows(Player player)
        {
            throw new NotImplementedException();
        }

        public void makeFlying(Player player)
        {
            throw new NotImplementedException();
        }

        public void Move(string piecePos, string movePos)
        {
            throw new NotImplementedException();
        }

        public ICow occupant(string pos)
        {
            throw new NotImplementedException();
        }

        public void Shoot(string shootPos)
        {
            throw new NotImplementedException();
        }
    }
    public class PowerLevel9000Cow : ICow
    {
        public string getPosition()
        {
            throw new NotImplementedException();
        }

        public CowStatus GetStatus()
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
    public class FlyingPowerLevel9001Cow : PowerLevel9000Cow
    {
        public override void Move(string movePos)
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
        public Player player => throw new NotImplementedException();

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
