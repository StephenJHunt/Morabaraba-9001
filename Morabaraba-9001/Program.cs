using System;
using System.Collections.Generic;

namespace Morabaraba_9001
{
    interface IBoard
    {
        ICow occupant(string pos);
        IEnumerable<int> Cows(Player player);
        void Move(string piecePos, string movePos);
        void Shoot(string shootPos);
        void makeFlying(Player player);
    }
    interface ICow
    {
        bool isMovable();
        bool isInMill();
        void Move(string movePos);
        string getPosition();
        CowStatus GetStatus();
    }
    interface IPlayer
    {
        Player player { get; }
        string getMove(string prompt);

    }
    public enum Player { X, O }
    public enum CowStatus {Unplaced, Placed, Flying, Shot}
    interface IGameManager
    {
        void startGame();
        void placingPhase();
        void movingPhase();
        void endGame();

    }
    public class invalidMoveException : ApplicationException { }
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
