using System;
using NUnit.Framework;
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
                if (cell.getState != CellState.Empty)
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
            Assert.That(Board.Xpieces == 12);
            Assert.That(Board.Ypieces == 12);
        }
        [Test]
        public void CowsCannotBeMovedDuringPlacement()
        {

        }
        //moving
        [Test]
        public void ANormalCowCanOnlyMoveToAConnectedSpace(ICell cowCell, ICell moveCell)
        {
            Board b = new Board();
            Assert.That(b.getNeighbours(cowCell).Contains(moveCell));
        }
        [Test]
        public void CowCanOnlyMoveToEmptySpace(ICell cowCell, ICell moveCell)
        {

        }
        [Test]
        public void MovingDoesNotChangeCowNumbers()
        {

        }
        //flying
        [Test]
        public void CowsCanMoveToAnyEmptySpaceWhenOnly3OfThatPlayersCowsRemain()
        {

        }
        //general
        [Test]
        public void MillIsFormedBy3SameCowsInALine()
        {

        }
        [Test]
        public void MillNotFormedWhenNotSamePlayer()
        {

        }
        [Test]
        public void MillNotFormedWhenConnectionsDoNotFormLine()
        {

        }
        [Test]
        public void ShootingOnlyPossibleOnMillCreation()
        {

        }
        [Test]
        public void CowInMillWhenOtherCowsOfSamePlayerNotInMillCannotBeShot()
        {

        }
        [Test]
        public void CowInMillWhenAllPlayerCowsInMillCanBeShot()
        {

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
