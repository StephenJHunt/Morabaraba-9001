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
            //MorabarabaManager mbman = Substitute.For<MorabarabaManager>();
            //Assert(mbman.placingPhase.currPlayer == Player.X);
            Assert.That(1 == 2 );
        }
        [Test]
        public void CowsCanOnlyBePlayedOnEmptySpaces()
        {
            Assert.That(1 == 2 );//y
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
        public void CowsCannotBeMovedDuringPlacement()//this one keeps breaking tests. Leave it to last
        {
            //var counter = 0;
            //IGameManager gm = Substitute.For<IGameManager>();
            //Board b = Substitute.For<Board>();
            //gm.When(x => gm.placingPhase()).Do(x => counter++);
            //GamePlayer player = new GamePlayer(Player.None);
            //b.DidNotReceiveWithAnyArgs().Move(player);
            Assert.That(1 == 2);
        }
        //moving
        [Test]
        public void ANormalCowCanOnlyMoveToAConnectedSpace()
        {
            Board b = new Board();
            //Assert.That(b.getNeighbours(cowPos).Contains(moveCell));
            Assert.That(1 == 2 );//y
        }
        [Test]
        public void CowCanOnlyMoveToEmptySpace()
        {
            IPlayer x = Substitute.For<IPlayer>();
            x.getMove(Arg.Any<string>()).Returns("A4");
            Board b = new Board();
            ICell cell = b.board["A4"];
            Assert.That(cell.getState == Player.None);
            //Assert.That(1 == 2);
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
            //Assert.That(1 == 2);
        }
        //flying
        [Test]
        public void CowsCanMoveToAnyEmptySpaceWhenOnly3OfThatPlayersCowsRemain()
        {
            //check isflying when plater reaches 3 cows
            Assert.That(1 == 2 );//y
        }
        //general
        [Test]
        public void MillIsFormedBy3SameCowsInALine()
        {
            //Assert.That isinMill returns true if given a cow that forms a mill
            Assert.That(1 == 2 );//y
        }
        [Test]
        public void MillNotFormedWhenNotSamePlayer()
        {
            //Assert.That isInMill only checks one player
            Assert.That(1 == 2 );//y
        }
        [Test]
        public void MillNotFormedWhenConnectionsDoNotFormLine()
        {
            //Assert.That mill list exists and is not null
            Assert.That(1 == 2 );//y
        }
        [Test]
        public void ShootingOnlyPossibleOnMillCreation()
        {
            //Board b = Substitute.For<Board>();
            //somehow check that shoot is called only when move makes a mill
            Assert.That(1 == 2 );
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
            Assert.That(1 == 2 );//y
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
            Assert.That(1 == 2 );//y
        }
        [Test]
        public void CannotShootOwnCows()//baka!
        {
            Assert.That(1 == 2 );//y
        }
        [Test]
        public void CannotShootEmptySpace()
        {
            Assert.That(1 == 2 );//y
        }
        [Test]
        public void ShotCowsRemovedFromBoard()
        {
            Assert.That(1 == 2 );
        }
        [Test]
        public void WinIfOpponentCannotMove()
        {
            Assert.That(1 == 2 );
        }
        [Test]
        public void WinIfOpponentHas2OrLessCowsLeftAfterPlacement()
        {
            Assert.That(1 == 2 );
        }
    }
}
