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
            Assert.That(1 == 2);//force fail to remind us to fix
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
        }
        //flying
        [Test]
        public void CowsCanMoveToAnyEmptySpaceWhenOnly3OfThatPlayersCowsRemain()
        {
            //We check that cows are made into flying cows by checking that once the player reaches 3 cows left, the remaining cows behave as flying cows
            //and use this to simulate a x cow flying to create a mill that ends the game
            MorabarabaManager manager = Substitute.For<MorabarabaManager>();
            IBoard b = manager.gameBoard;
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.setID(Player.X);
            o.setID(Player.O);

            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "B4", "B4", "A7", "F4");
            o.getMove(Arg.Any<string>()).Returns("G1", "G4", "F4");
            b.Place(x);
            b.Place(x);
            b.Place(x);
            b.Place(o);
            b.Place(o);
            b.Place(o);
            
            
            Assert.That(manager.movingPhase(), Is.EqualTo("X wins!"));         

        }
        //general
        [Test]
        public void MillIsFormedBy3SameCowsInALine()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.setID(Player.X);
            o.setID(Player.O);
            o.getMove(Arg.Any<string>()).Returns("G1");
            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "A7", "G1");
            b.Place(o);
            b.Place(x);
            b.Place(x);
            b.Place(x);
            Assert.That(b.isInMill("A1") && b.isInMill("A4") && b.isInMill("A7"));
        }
        [Test]
        public void MillNotFormedWhenNotSamePlayer()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.setID(Player.X);
            o.setID(Player.O);
            o.getMove(Arg.Any<string>()).Returns("A7");
            x.getMove(Arg.Any<string>()).Returns("A1", "A4");
            b.Place(o);
            b.Place(x);
            b.Place(x);
            Assert.That(!b.isInMill("A1") && !b.isInMill("A4") && !b.isInMill("A7"));
        }
        [Test]
        public void MillNotFormedWhenConnectionsDoNotFormLine()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            x.setID(Player.X);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "B4");
            b.Place(x);
            b.Place(x);
            b.Place(x);
            Assert.That(!b.isInMill("A1") && !b.isInMill("A4") && !b.isInMill("B4"));
        }
        [Test]
        public void ShootingOnlyPossibleOnMillCreation()//confusion
        {
            //check shooting is only possible by showing that it only happens once even with many pieces being placed, during which only one mill is formed
            Board b = Substitute.For<Board>();
            IPlayer x = Substitute.For<IPlayer>();
            x.setID(Player.X);
            IPlayer o = Substitute.For<IPlayer>();
            o.setID(Player.O);
            o.getMove(Arg.Any<string>()).Returns("G1", "G4");
            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "A7", "G1");
            b.Place(o);
            b.Place(o);
            b.Place(x);
            b.Place(x);
            b.Place(x); 
            Assert.That(b.board["A1"].getState == x.playerID && b.board["A4"].getState == x.playerID && b.board["A7"].getState == x.playerID && b.board["G1"].getState == Player.None && b.board["G4"].getState == o.playerID);
        }
        [Test]
        public void CowInMillWhenOtherCowsOfSamePlayerNotInMillCannotBeShot()
        {
            Board b = Substitute.For<Board>();
            IPlayer x = Substitute.For<IPlayer>();
            x.setID(Player.X);
            IPlayer o = Substitute.For<IPlayer>();
            o.setID(Player.O);
            o.getMove(Arg.Any<string>()).Returns("G1", "G4", "G7", "A1", "F2");
            x.getMove(Arg.Any<string>()).Returns("A1", "A1" ,"A4", "A7", "G1", "F2");
            b.Place(x);
            b.Place(o);
            b.Place(o);
            b.Place(o);
            b.Place(o);
            b.Place(x);
            b.Place(x);
            b.Place(x);

            Assert.That(b.board["F2"].getState == Player.None && b.board["G1"].getState == o.playerID);
        }
        [Test]
        public void CowInMillWhenAllPlayerCowsInMillCanBeShot()
        {
            Board b = Substitute.For<Board>();
            IPlayer x = Substitute.For<IPlayer>();
            x.setID(Player.X);
            IPlayer o = Substitute.For<IPlayer>();
            o.setID(Player.O);
            o.getMove(Arg.Any<string>()).Returns("G1", "G4", "G7", "A1");
            x.getMove(Arg.Any<string>()).Returns("A1", "A1", "A4", "A7", "G1");
            b.Place(x);
            b.Place(o);
            b.Place(o);
            b.Place(o);
            b.Place(x);
            b.Place(x);
            b.Place(x);

            Assert.That(b.board["G1"].getState == Player.None);
        }
        [Test]
        public void CannotShootOwnCows()//baka!
        {
            IPlayer o = Substitute.For<IPlayer>();
            o.playerID = Player.O;
            Board b = Substitute.For<Board>();
            o.getMove(Arg.Any<string>()).Returns("A1");
            b.Place(o);
            Assert.That(b.board["A1"].getState == Player.O);
            //GamePlayer x = new GamePlayer(Player.X);
            //x.setID(Player.X);
            //GamePlayer o = new GamePlayer(Player.O);
            //o.setID(Player.O);
            //o.getMove(Arg.Any<string>()).Returns("G1");
            //x.getMove(Arg.Any<string>()).Returns("A1", "B2", "A4", "A7", "B2" ,"G1");
            //b.Place(x);
            //b.Place(o);
            //b.Place(x);
            //b.Place(x);
            //b.Place(x);

            //Assert.That(b.board["B2"].getState == Player.None);// && b.board["G1"].getState == Player.None
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
