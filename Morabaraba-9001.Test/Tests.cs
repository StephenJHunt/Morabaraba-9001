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
            MorabarabaManager mbman = new MorabarabaManager();
            //Assert(mbman.startingPlayer == Player.X);
        }
        [Test]
        public void CowsCanOnlyBePlayedOnEmptySpaces(string input)
        {
            
        }
        [Test]
        public void AMaximumOf12PlacementsPerPlayerAreAllowed()
        {
            //Assert.That(Board.Xpieces == 12);
            //Assert.That(Board.Ypieces == 12);
        }
        [Test]
        public void CowsCannotBeMovedDuringPlacement()
        {
            IGameManager gm = Substitute.For<IGameManager>();
            Board b = new Board();
            gm.placingPhase();
            Player pl = Player.None;
            GamePlayer player = new GamePlayer(pl);
            b.DidNotReceiveWithAnyArgs().Move(player);
        }
        //moving
        [Test]
        public void ANormalCowCanOnlyMoveToAConnectedSpace(string cowPos, ICell moveCell)
        {
            Board b = new Board();
            Assert.That(b.getNeighbours(cowPos).Contains(moveCell));
        }
        [Test]
        public void CowCanOnlyMoveToEmptySpace(string moveCell)
        {
            Board b = new Board();
            ICell cell = b.board[moveCell];
            Assert.That(cell.getState == Player.None);
        }
        [Test]
        public void MovingDoesNotChangeCowNumbers()
        {
            Board b = new Board();
            Player p = Player.O;
            GamePlayer pl = new GamePlayer(p);
            int beforeMovePieces = b.numCows(p);
            b.Move(pl);
            int afterMovePieces = b.numCows(p);
            Assert.That(beforeMovePieces == afterMovePieces);
        }
        //flying
        [Test]
        public void CowsCanMoveToAnyEmptySpaceWhenOnly3OfThatPlayersCowsRemain()
        {
            //check isflying when plater reaches 3 cows
        }
        //general
        [Test]
        public void MillIsFormedBy3SameCowsInALine()
        {
            //Assert.That isinMill returns true if given a cow that forms a mill
        }
        [Test]
        public void MillNotFormedWhenNotSamePlayer()
        {
            //Assert.That isInMill only checks one player
        }
        [Test]
        public void MillNotFormedWhenConnectionsDoNotFormLine()
        {
            //Assert.That mill list exists and is not null
        }
        [Test]
        public void ShootingOnlyPossibleOnMillCreation()
        {
            Board b = Substitute.For<Board>();
            //somehow check that shoot is called only when move makes a mill
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
        }
        [Test]
        public void CannotShootOwnCows()//baka!
        {

        }
        [Test]
        public void CannotShootEmptySpace()
        {

        }
        [Test]
        public void ShotCowsRemovedFromBoard()
        {

        }
        [Test]
        public void WinIfOpponentCannotMove()
        {

        }
        [Test]
        public void WinIfOpponentHas2OrLessCowsLeftAfterPlacement()
        {

        }
    }
}
