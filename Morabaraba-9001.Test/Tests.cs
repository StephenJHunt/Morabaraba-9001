using System;
using NUnit.Framework;
using NSubstitute;
using System.Linq;

namespace Morabaraba_9001.Test
{
	[TestFixture]
    
    public class Tests
    {
        //placing
        [Test]
        public void AtStartBoardIsEmpty()
        {
            Board b = new Board();
            bool isEmpty = true;
            foreach (ICell cell in b.board.Values)
            {
                if (cell.getState != Player.None)
                {
                    isEmpty = false;
                }
            }
            Assert.That(isEmpty);
        }
        [Test]
        public void PlayerXStartsFirst()//Player X is our equivalent for the dark cows player
        {
            MorabarabaManager manager = Substitute.For<MorabarabaManager>();
            Assert.That(manager.currPlayer.playerID == Player.X);
        }
        [Test]
        public void CowsCanOnlyBePlayedOnEmptySpaces()
        {

            //X places on A1
            //O tries to place on A1, will need to re-enter a new (empty) position which will be A4
            //If the test works A1 will be X and A4 will be O
            //This shows that a player's piece will not be replaced and that a piece can be placed normally on an empty position
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.setID(Player.X);
            o.setID(Player.O);

            x.getMove(Arg.Any<string>()).Returns("A1");
            o.getMove(Arg.Any<string>()).Returns("A1", "A4");

            b.Place(x);
            b.Place(o);
            Assert.That(b.board["A1"].getState == x.playerID && b.board["A4"].getState == o.playerID);
        }
        [Test]
        public void AMaximumOf12PlacementsPerPlayerAreAllowed()
        {
            GamePlayer Player1 = new GamePlayer(Player.X);
            Assert.That(Player1.stones == 12);
            GamePlayer Player2 = new GamePlayer(Player.O);
            Assert.That(Player2.stones == 12);
        }
        [Test]
        public void CowsCannotBeMovedDuringPlacement()//leave for later
        {
            //MorabarabaManager gm = Substitute.For<MorabarabaManager>();
            //gm.xPlayer.getMove(Arg.Any<string>()).Returns("A1", "A7", "B4", "C3", "C5", "D2", "D5", "D7", "E4", "F2", "F6", "G4");
            //gm.oPlayer.getMove(Arg.Any<string>()).Returns("A4", "B2", "B6", "C4", "D1", "D3", "D6", "E3", "E5", "F4", "G1", "G7");

            //IBoard b = gm.gameBoard;
            //gm.placingPhase();

            //GamePlayer player = new GamePlayer(Player.None);
            //b.DidNotReceiveWithAnyArgs().Move(player);
        }
        //moving
        [Test]
        public void ANormalCowCanOnlyMoveToAConnectedSpace()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            x.setID(Player.X);
            x.getMove(Arg.Any<string>()).Returns("A4", "A4", "G7", "A7");
            b.Place(x);
            int num = b.numCows(x.playerID);
            b.Move(x);
            Assert.That(b.board["A4"].getState == Player.None && b.board["G7"].getState == Player.None && b.board["A7"].getState == Player.X);
        }
        [Test]
        public void CowCanOnlyMoveToEmptySpace()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            x.setID(Player.X);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "A4", "A1", "A7");
            b.Place(x);
            b.Place(x);
            int num = b.numCows(x.playerID);
            b.Move(x);
            Assert.That(b.board["A4"].getState == Player.None && b.board["A1"].getState == Player.X && b.board["A7"].getState == Player.X);
        }
        [Test]
        public void MovingDoesNotChangeCowNumbers()
        {

            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            x.getMove(Arg.Any<string>()).Returns("A4", "A4", "A1");
            b.Place(x);
            int num = b.numCows(x.playerID);
            b.Move(x);
            Assert.That(num == b.numCows(x.playerID));
            //Board b = new Board();
            //Player p = Player.O;
            //GamePlayer pl = new GamePlayer(p);
            //int beforeMovePieces = b.numCows(p);
            //b.Move(pl);
            //int afterMovePieces = b.numCows(p);
            //Assert.That(beforeMovePieces == afterMovePieces);
            ////Assert.That(1 == 2 );
        }
        //flying
        [Test]
        public void CowsCanMoveToAnyEmptySpaceWhenOnly3OfThatPlayersCowsRemain()
        {
            //check isflying when plater reaches 3 cows
            //Assert.That(1 == 2 );//y
        }
        //general
        [Test]
        public void MillIsFormedBy3SameCowsInALine()
        {
            //Assert.That isinMill returns true if given a cow that forms a mill
            //Assert.That(1 == 2 );//y
        }
        [Test]
        public void MillNotFormedWhenNotSamePlayer()
        {
            //Assert.That isInMill only checks one player
            //Assert.That(1 == 2 );//y
        }
        [Test]
        public void MillNotFormedWhenConnectionsDoNotFormLine()
        {
            //Assert.That mill list exists and is not null
            //Assert.That(1 == 2 );//y
        }
        [Test]
        public void ShootingOnlyPossibleOnMillCreation()
        {
            //Board b = Substitute.For<Board>();
            //somehow check that shoot is called only when move makes a mill
            //Assert.That(1 == 2 );
        }
        [Test]
        public void CowInMillWhenOtherCowsOfSamePlayerNotInMillCannotBeShot()
        {
            //Board b = Substitute.For<Board>();
            //Player p = new Player();
            //GamePlayer player = new GamePlayer(p);
            //foreach (ICell cow in b.Cows(player))
            //{
            //    if (!cow.isInMill)
            //    {
            //        b.DidNotRecieveWithAnyArgs().Shoot();
            //    }
            //}
            //Assert.That(1 == 2 );//y
        }
        [Test]
        public void CowInMillWhenAllPlayerCowsInMillCanBeShot()
        {
            //Board b = new Board();
            //GamePlayer player = new GamePlayer();
            //foreach (ICell cow in b.Cows(player))
            //{
            //    if (!cow.isInMill)
            //    {
            //        b.ReceivedWithAnyArgs.Shoot();
            //    }
            //}
            //Assert.That(1 == 2 );//y
        }
        [Test]
        public void CannotShootOwnCows()//baka!
        {
            //Assert.That(1 == 2 );//y
        }
        [Test]
        public void CannotShootEmptySpace()
        {
            //Assert.That(1 == 2 );//y
        }
        [Test]
        public void ShotCowsRemovedFromBoard()
        {
            //Assert.That(1 == 2 );
        }
        [Test]
        public void WinIfOpponentCannotMove()
        {
            //Assert.That(1 == 2 );
        }
        [Test]
        public void WinIfOpponentHas2OrLessCowsLeftAfterPlacement()
        {
            //Assert.That(1 == 2 );
        }
    }
}
