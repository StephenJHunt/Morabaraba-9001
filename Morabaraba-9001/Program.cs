using System;
using System.Collections.Generic;
using System.Linq;

namespace Morabaraba_9001
{
    public interface IRef
    {
        bool isValidPlacement(string pos, IPlayer player, IBoard board);
        bool isValidPickUp(string pos, IPlayer player, IBoard board);
        bool isValidPutDown(string piecePos, string placePos, IPlayer player, IBoard board);
        bool isValidShot(string pos, IPlayer player, IBoard board);
        bool inPlacing(IPlayer player1, IPlayer player2);
        bool inMoving(IPlayer player1, IPlayer player2, IBoard board);
    }
    public interface IBoard
    {
        Dictionary<string, ICell> board {get; }
        int numCows(Player player);

        PlaceResult Place(IPlayer player, IRef referee);

        MoveResult Move(IPlayer player, IRef referee);

        ShootResult Shoot(IPlayer player, IRef referee);

        List<ICell> getNeighbours(string pos);

        ICell getCell(string pos);

        bool isMovable(string pos);

        bool isInMill(string pos);

        bool allInMill(Player player);

        void Display(string extraDisplay);

        bool canPlay(IPlayer player);
    }

    public interface ICell
    {
        Player getState { get; }

        void changeState(Player changedState);
    }

    public interface IPlayer
    {
        Player playerID { get; set; }

        string getMove(string prompt);

        Player getOpponent();

        int stones { get; }

        void reduceStones();

        void setID(Player ID);
    }

    //public enum CellState { X, O, Empty }
    public enum Player { X, O, None }
    public enum PlaceAction {Place, Shoot}
    public enum PlaceResult {Invalid, MillMade, Done}
    public enum ShootResult {Invalid, Done}
    public enum MoveAction {Move, Shoot}
    public enum MoveResult {InvalidPickUp, InvalidPutDown, MillMade, Done}
    public interface IGameManager
    {
        void placingPhase();

        string movingPhase();
    }

    public class invalidMoveException : ApplicationException { }

    public class Cell : ICell
    {
        private Player state;
        public Player getState => state;

        public Cell(Player startState)
        {
            state = startState;
        }

        public void changeState(Player changedState)
        {
            state = changedState;
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

        Dictionary<string, ICell> IBoard.board => board;

        public Board()
        {
            foreach (string pos in validPositions)//initialising board with empty values
            {
                board.Add(pos, new Cell(Player.None));
            }
        }

        public int numCows(Player player)
        {
            IEnumerable<ICell> query =
                from cell in board.Values.ToList()
                where cell.getState == player
                select cell;
            return query.Count();
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

        public PlaceResult Place(IPlayer player, IRef referee)
        {
            string placePos = player.getMove("Select place position: ");
            if (!referee.isValidPlacement(placePos, player, this))
                return PlaceResult.Invalid;

            board[placePos].changeState(player.playerID);
            player.reduceStones();

            if (isInMill(placePos))
                return PlaceResult.MillMade;
            else
                return PlaceResult.Done;
        }

        public MoveResult Move(IPlayer player, IRef referee)
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
            string shootPos = player.getMove("Enter a position to shoot");
            if (!referee.isValidShot(shootPos, player, this))
                return ShootResult.Invalid;
            board[shootPos].changeState(Player.None);
            return ShootResult.Done;
        }

        public bool isMovable(string pos)
        {
            List<ICell> emptyNeighbours =
                (from cell in getNeighbours(pos)
                 where cell.getState == Player.None
                 select cell).ToList();
            return emptyNeighbours.Count > 0;
        }

        public bool canPlay(IPlayer player)
        {
            if (numCows(player.playerID) <= 3)
                return true;
            IEnumerable<string> query =
                from pos in board.Keys
                where board[pos].getState == player.playerID
                where isMovable(pos)
                select pos;
            return query.Any();
        }

        public bool allInMill(Player player)
        {
            foreach (string pos in board.Keys)
            {
                if (board[pos].getState == player && !isInMill(pos))
                    return false;
            }
            return true;
        }

        public bool isInMill(string pos)
        {
            List<string[]> relevantmills = mills.Where(mill => mill.Contains(pos)).ToList();
            foreach (ICell[] mill in relevantmills.Select(mill => mill.Select(getCell).ToArray()).ToList())
            {
                if ((mill[0].getState == Player.X && mill[1].getState == Player.X && mill[2].getState == Player.X) ||
                    (mill[0].getState == Player.O && mill[1].getState == Player.O && mill[2].getState == Player.O))
                {
                    return true;
                }
            }
            return false;
        }

        public string playerToString(Player player)
        {
            if (player == Player.O)
                return "O";
            else if (player == Player.X)
                return "X";
            return " ";
        }

        public void Display(string extraDisplay)
        {
            Console.Clear();
            string[] cells = board.Values.Select(cell => playerToString(cell.getState)).ToArray();
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
            while (referee.inPlacing(xPlayer, oPlayer))
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

        public string movingPhase()
        {
            currPlayer = xPlayer;
            while (true)
            {
                gameBoard.Display($@"X cows: {gameBoard.numCows(Player.X)} O cows: {gameBoard.numCows(Player.O)}");

                if (gameBoard.numCows(Player.X) < 3 || !gameBoard.canPlay(xPlayer))
                {
                    return "O wins!";
                }
                if (gameBoard.numCows(Player.O) < 3 || !gameBoard.canPlay(oPlayer))
                {
                    return "X wins!";
                }

                gameBoard.Move(currPlayer);

                if (currPlayer == xPlayer)
                    currPlayer = oPlayer;
                else
                    currPlayer = xPlayer;
            }
        }
    }

    public class GamePlayer : IPlayer
    {
        public GamePlayer(Player player)
        {
            gameplayer = player;
            numStones = 12;
        }

        public void setID(Player ID)
        {
            gameplayer = ID;
        }

        private int numStones;


        private Player gameplayer;

        public int stones => numStones;

        public void reduceStones()
        {
            numStones--;
        }

        //public Player playerID => gameplayer;

        Player IPlayer.playerID { get => gameplayer; set => gameplayer = value; }

        public Player getOpponent()
        {
            if (this.gameplayer == Player.X)
            {
                return Player.O;
            }
            else if (this.gameplayer == Player.O)
            {
                return Player.X;
            }
            return Player.None;
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
        public bool inMoving(IPlayer player1, IPlayer player2, IBoard board)
        {
            return board.numCows(player1.playerID) >= 3
                   && board.numCows(player2.playerID) >= 3;
        }

        public bool inPlacing(IPlayer player1, IPlayer player2)
        {
            return player1.stones > 0 || player2.stones > 0;
        }

        public bool isValidPickUp(string pos, IPlayer player, IBoard board)
        {
            return board.board[pos].getState == player.playerID 
                    && (board.numCows(player.playerID) == 3 
                        || board.isMovable(pos));
        }

        public bool isValidPlacement(string pos, IPlayer player, IBoard board)
        {

            return board.board[pos].getState == Player.None
                && player.stones > 0;
        }

        public bool isValidPutDown(string piecePos, string placePos, IPlayer player, IBoard board)
        {
            return (board.numCows(player.playerID) == 3
                    || Board.neighbours[piecePos].Contains(placePos))
                    && board.board[placePos].getState == Player.None;
        }

        public bool isValidShot(string pos, IPlayer player, IBoard board)
        {
            return board.board[pos].getState == player.getOpponent()
                   && (!board.isInMill(pos)
                       || board.allInMill(player.getOpponent()));
        }
    }

    public class InputHandler
    {
        public static string PickUpInput(IPlayer player, IBoard gboard)
        {
            string piecePos;
            while (true)
            {
                piecePos = player.getMove("Select piece to move: ");
                if (gboard.board[piecePos].getState == player.playerID)
                {
                    if (gboard.numCows(player.playerID) == 3 || gboard.isMovable(piecePos))
                        return piecePos;
                }
                gboard.Display("");
                Console.WriteLine("Please select a valid piece");
            }

            
        }

        public static string PutDownInput(string piecePos, IPlayer player, IBoard gboard)
        {
            string placePos;
            while (true)
            {
                placePos = player.getMove("Select position to place " + piecePos + ": ");
                if ((gboard.numCows(player.playerID) == 3 || Board.neighbours[piecePos].Contains(placePos)) && gboard.board[placePos].getState == Player.None)
                {
                    return placePos;
                }
                gboard.Display("");
                Console.WriteLine("Please select a valid position");
            }
        }

        public static string PlaceInput(IPlayer player, IBoard gboard)
        {
            string placePos;
            while (true)
            {
                placePos = player.getMove("Select position to place your piece: ");
                if (gboard.board[placePos].getState == Player.None)
                {
                    return placePos;
                }
                Console.WriteLine("Please select a valid position");
            }
        }

        public static string ShootInput(IPlayer player, IBoard gboard)
        {
            string shootPos;
            while (true)
            {
                shootPos = player.getMove("Select piece to shoot: ");
                if (gboard.board[shootPos].getState == player.getOpponent() && (!gboard.isInMill(shootPos) || gboard.allInMill(player.getOpponent())))
                {
                    return shootPos;
                }
                Console.WriteLine("Please select a valid piece to shoot");
            }
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            IGameManager manager = new MorabarabaManager(new Board(), new GamePlayer(Player.X), new GamePlayer(Player.O), new MReferee());
            manager.placingPhase();
            Console.WriteLine(manager.movingPhase());
            Console.ReadLine();
        }
    }
}