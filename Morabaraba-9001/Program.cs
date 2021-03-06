﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Morabaraba_9001
{
    public enum Player { X, O, None }
    public enum PlaceAction { Place, Shoot }
    public enum PlaceResult { Invalid, MillMade, Done }
    public enum ShootResult { Invalid, Done }
    public enum MoveAction { Move, Shoot }
    public enum MoveResult { InvalidPickUp, InvalidPutDown, MillMade, Done }
    public enum GameResult { NotWon, Player1, Player2 }

    public interface IRef
    {
        bool isValidPlacement(string pos, IPlayer player, IBoard board);
        bool isValidPickUp(string pos, IPlayer player, IBoard board);
        bool isValidPutDown(string piecePos, string placePos, IPlayer player, IBoard board);
        bool isValidShot(string pos, IPlayer player, IBoard board);
        bool inPlacing(IPlayer player1, IPlayer player2);
        GameResult getGameState(IPlayer player1, IPlayer player2, IBoard board);
    }
    public interface IBoard
    {
        Dictionary<string, ICell> getBoard();
        Player getCellState(string pos);
        ICell getCell(string pos);
        int numCows(Player player);

        PlaceResult Place(IPlayer player, IRef referee);

        MoveResult Move(IPlayer player, IRef referee);

        ShootResult Shoot(IPlayer player, IRef referee);

        List<Player> getNeighbours(string pos);

        bool isMovable(string pos);

        bool isInMill(string pos);

        bool allInMill(Player player);

        void Display(string extraDisplay);

        bool canPlay(IPlayer player);
    }

    public interface ICell
    {
        Player getState();
        string ToString();
        void changeState(Player changedState);
    }

    public interface IPlayer
    {
        Player playerID { get;}

        string getMove(string prompt);

        Player getOpponent();

        int stones { get; }
        void Placed();
        string ToString();
    }


    public interface IGameManager
    {
        void placingPhase();

        GameResult movingPhase();
    }

    public class invalidMoveException : ApplicationException { }

    public class Cell : ICell
    {
        private Player state;
        public Player getState() { return state; }

        public Cell(Player startState)
        {
            state = startState;
        }

        public void changeState(Player changedState)
        {
            state = changedState;
        }

        public override string ToString()
        {
            if (state == Player.O)
                return "O";
            else if (state == Player.X)
                return "X";
            return " ";
        }
    }

    public class Board : IBoard
    {
        public static string[] validPositions = new string[] { "A1", "A4", "A7", "B2", "B4", "B6", "C3", "C4", "C5", "D1", "D2", "D3", "D5", "D6", "D7", "E3", "E4", "E5", "F2", "F4", "F6", "G1", "G4", "G7" };

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

        public static List<string[]> mills = new List<string[]>
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

        private Dictionary<string, ICell> board = new Dictionary<string, ICell>();

        public Board()
        {
            foreach (string pos in validPositions)//initialising board with empty values
            {
                board.Add(pos, new Cell(Player.None));
            }
        }

        public virtual Dictionary<string, ICell> getBoard()
        {
            return board;
        }

        public virtual Player getCellState(string pos)
        {
            return board[pos].getState();
        }

        public virtual int numCows(Player player)
        {
            int count = 0;
            foreach (string pos in validPositions)
            {
                if (getCellState(pos) == player)
                    count++;
            }
            return count;
            //IEnumerable<ICell> query =
            //    from cell in board.Values.ToList()
            //    where cell.getState() == player
            //    select cell;
            //return query.Count();
        }

        public ICell getCell(string pos)
        {
            return board[pos];
        }

        public List<Player> getNeighbours(string pos)
        {
            List<Player> neighbourList = new List<Player>();
            foreach (string npos in neighbours[pos])
            {
                neighbourList.Add(getCellState(npos));
            }
            return neighbourList;
        }

        public virtual PlaceResult Place(IPlayer player, IRef referee)
        {
            string placePos = player.getMove("Select place position: ");
            if (!referee.isValidPlacement(placePos, player, this))
                return PlaceResult.Invalid;

            board[placePos].changeState(player.playerID);
            player.Placed();

            if (isInMill(placePos))
                return PlaceResult.MillMade;
            else
                return PlaceResult.Done;
        }

        public virtual MoveResult Move(IPlayer player, IRef referee)
        {
            string piecePos = player.getMove("Enter piece to move: ");
            if (!referee.isValidPickUp(piecePos, player, this))
                return MoveResult.InvalidPickUp;
            string placePos = player.getMove($@"Place {piecePos} at position: ");
            if (!referee.isValidPutDown(piecePos, placePos, player, this))
                return MoveResult.InvalidPutDown;
            board[piecePos].changeState(Player.None);
            board[placePos].changeState(player.playerID);
            if (isInMill(placePos))
            {
                return MoveResult.MillMade;
            }
            return MoveResult.Done;
        }

        public ShootResult Shoot(IPlayer player, IRef referee)
        {
            string shootPos = player.getMove("Enter a position to shoot: ");
            if (!referee.isValidShot(shootPos, player, this))
                return ShootResult.Invalid;
            board[shootPos].changeState(Player.None);
            return ShootResult.Done;
        }

        public bool isMovable(string pos)
        {
            List<Player> emptyNeighbours =
                (from cellState in getNeighbours(pos)
                 where cellState == Player.None
                 select cellState).ToList();
            return emptyNeighbours.Count > 0;
        }

        public bool canPlay(IPlayer player)
        {
            if (numCows(player.playerID) <= 3)
                return true;
            IEnumerable<string> query =
                from pos in board.Keys
                where getCellState(pos) == player.playerID
                where isMovable(pos)
                select pos;
            return query.Any();
        }

        public bool allInMill(Player player)
        {
            foreach (string pos in board.Keys)
            {
                if (getCellState(pos) == player && !isInMill(pos))
                    return false;
            }
            return true;
        }

        public virtual bool isInMill(string pos)
        {
            List<string[]> relevantmills = mills.Where(mill => mill.Contains(pos)).ToList();
            foreach (Player[] mill in relevantmills.Select(mill => mill.Select(getCellState).ToArray()).ToList())
            {
                if ((mill[0] == Player.X && mill[1] == Player.X && mill[2] == Player.X) ||
                    (mill[0] == Player.O && mill[1] == Player.O && mill[2] == Player.O))
                {
                    return true;
                }
            }
            return false;
        }

        

        public void Display(string extraDisplay)
        {
            Console.Clear();
            string[] cells = board.Values.Select(cell => cell.ToString()).ToArray();
            string dis =
                $@"
    1   2  3   4   5  6   7
A   {cells[0]}----------{cells[1]}----------{cells[2]}
|   | '.       |        .'|
B   |   {cells[3]}------{cells[4]}------{cells[5]}   |
|   |   |'.    |    .'|   |
C   |   |  {cells[6]}---{cells[7]}---{cells[8]}  |   |
|   |   |  |       |  |   |
D   {cells[9]}---{cells[10]}--{cells[11]}       {cells[12]}--{cells[13]}---{cells[14]}
|   |   |  |       |  |   |
E   |   |  {cells[15]}---{cells[16]}---{cells[17]}  |   |
|   |   |.'    |    '.|   |
F   |   {cells[18]}------{cells[19]}------{cells[20]}   |
|   |.'        |       '. |
G   {cells[21]}----------{cells[22]}----------{cells[23]} ";

            Console.WriteLine(extraDisplay);
            Console.WriteLine(dis);
        }


    }

    public class MorabarabaManager : IGameManager
    {
        public MorabarabaManager(IBoard board, IPlayer player1, IPlayer player2, IRef r)
        {
            xPlayer = player1;
            oPlayer = player2;
            currPlayer = xPlayer;
            referee = r;
            gameBoard = board;
        }

        public IPlayer xPlayer, oPlayer;
        public IBoard gameBoard;
        public IPlayer currPlayer;
        public IRef referee;
        public void placingPhase()
        {
            PlaceAction act = PlaceAction.Place;
            while (referee.inPlacing(xPlayer, oPlayer) || act == PlaceAction.Shoot)
            {
                gameBoard.Display($@"X stones:{xPlayer.stones} O stones:{oPlayer.stones}");
                if (act == PlaceAction.Place)
                {
                    switch(gameBoard.Place(currPlayer, referee))
                    {
                        case PlaceResult.Invalid: break;
                        case  PlaceResult.Done:
                            if (currPlayer == xPlayer)
                                currPlayer = oPlayer;
                            else
                                currPlayer = xPlayer;
                            break;
                        case  PlaceResult.MillMade: act = PlaceAction.Shoot;  break;
                    }
                }
                else
                {
                    if (gameBoard.Shoot(currPlayer, referee) == ShootResult.Done)
                    {
                        act = PlaceAction.Place;
                        if (currPlayer == xPlayer)
                            currPlayer = oPlayer;
                        else
                            currPlayer = xPlayer;
                    }
                }
            }
        }

        public GameResult movingPhase()
        {
            GameResult gameState = referee.getGameState(xPlayer, oPlayer, gameBoard);
            MoveAction act = MoveAction.Move;
            currPlayer = xPlayer;
            while (gameState == GameResult.NotWon)
            {
                gameBoard.Display($@"X cows: {gameBoard.numCows(Player.X)} O cows: {gameBoard.numCows(Player.O)}");

                if (act == MoveAction.Move)
                {
                    switch (gameBoard.Move(currPlayer, referee))
                    {
                        case MoveResult.InvalidPickUp: break;
                        case MoveResult.InvalidPutDown: break;
                        case MoveResult.MillMade: act = MoveAction.Shoot;  break;
                        case MoveResult.Done:
                            if (currPlayer == xPlayer)
                                currPlayer = oPlayer;
                            else
                                currPlayer = xPlayer;
                            break;
                    }
                }
                else
                {
                    if (gameBoard.Shoot(currPlayer, referee) == ShootResult.Done)
                    {
                        act = MoveAction.Move;
                        if (currPlayer == xPlayer)
                            currPlayer = oPlayer;
                        else
                            currPlayer = xPlayer;
                    }
                }
                gameState = referee.getGameState(xPlayer, oPlayer, gameBoard);
            }
            gameBoard.Display($@"X cows: {gameBoard.numCows(Player.X)} O cows: {gameBoard.numCows(Player.O)}");
            return gameState;
        }
    }

    public class GamePlayer : IPlayer
    {
        private int numStones;
        private Player gameplayerID;
        public int stones => numStones;

        public GamePlayer(Player player)
        {
            gameplayerID = player;
            numStones = 12;
        }


        public void Placed()
        {
            if (numStones > 0)
                numStones--;
        }

        Player IPlayer.playerID { get => gameplayerID; }

        public Player getOpponent()
        {
            if (gameplayerID == Player.X)
            {
                return Player.O;
            }
            else if (gameplayerID == Player.O)
            {
                return Player.X;
            }
            return Player.None;
        }

        public override string ToString()
        {
            if (gameplayerID == Player.X)
                return "X";
            else
                return "O";
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


    public class MReferee : IRef
    {

        public bool inPlacing(IPlayer player1, IPlayer player2)
        {
            return player1.stones > 0 || player2.stones > 0;
        }

        public GameResult getGameState(IPlayer player1, IPlayer player2, IBoard board)
        {
            if (board.numCows(player1.playerID) < 3 || !board.canPlay(player1))
                return GameResult.Player2;
            if (board.numCows(player2.playerID) < 3 || !board.canPlay(player2))
                return GameResult.Player1;
            return GameResult.NotWon;
        }

        public bool isValidPickUp(string pos, IPlayer player, IBoard board)
        {
            return board.getCellState(pos) == player.playerID 
                    && (board.numCows(player.playerID) == 3 
                        || board.isMovable(pos));
        }

        public bool isValidPlacement(string pos, IPlayer player, IBoard board)
        {

            return board.getCellState(pos)== Player.None
                && player.stones > 0;
        }

        public bool isValidPutDown(string piecePos, string placePos, IPlayer player, IBoard board)
        {
            return (board.numCows(player.playerID) == 3
                    || Board.neighbours[piecePos].Contains(placePos))
                    && board.getCellState(placePos) == Player.None;
        }

        public bool isValidShot(string pos, IPlayer player, IBoard board)
        {
            return board.getCellState(pos) == player.getOpponent()
                   && (!board.isInMill(pos)
                       || board.allInMill(player.getOpponent()));
        }

    }



    internal class Program
    {
        private static void Main(string[] args)
        {
            IGameManager manager = new MorabarabaManager(new Board(), new GamePlayer(Player.X), new GamePlayer(Player.O), new MReferee());
            manager.placingPhase();
            if (manager.movingPhase() == GameResult.Player1)
                Console.WriteLine("X Won!!!");
            else
                Console.WriteLine("O Won!!!");
            Console.ReadLine();
        }
    }
}