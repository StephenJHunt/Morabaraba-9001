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
            MorabarabaManager manager = new MorabarabaManager();
            Assert.That(manager.currPlayer.playerID == Player.X);
        }
        [Test]
        public void CowsCanOnlyBePlayedOnEmptySpaces()
        {
            Board b = Substitute.For<Board>();
            b.board["A1"] = new Cell(Player.O);
            b.board["A4"] = new Cell(Player.None);
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4");
            b.Place(x);
            Assert.That(b.board["A1"].getState == Player.O && b.board["A4"].getState == Player.X);
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
            x.playerID.Returns(Player.X);
            b.board["A1"] = new Cell(x.playerID);
            x.getMove(Arg.Any<string>()).Returns( "A1", "G7", "A4");
            b.Move(x);
            x.Received(3).getMove(Arg.Any<string>());
        }
        [Test]
        public void CowCanOnlyMoveToEmptySpace()
        {
            Board b = new Board();
            b.board["A1"] = new Cell(Player.X);
            b.board["A4"] = new Cell(Player.X);
            b.board["D1"] = new Cell(Player.O);
            b.board["B2"] = new Cell(Player.None);
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getMove(Arg.Any<string>()).Returns("A1", "A1", "A4", "D1", "B2");
            b.Move(x);
            x.Received(5).getMove(Arg.Any<string>());
        }
        [Test]
        public void MovingDoesNotChangeCowNumbers()
        {

            Board b = new Board();
            b.board["A4"] = new Cell(Player.X);
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getMove(Arg.Any<string>()).Returns("A4", "A1");
            int old = b.numCows(x.playerID);
            b.Move(x);
            Assert.That(b.numCows(x.playerID) == old);
        }
        //flying
        [Test]
        public void CowsCanMoveToAnyEmptySpaceWhenOnly3OfThatPlayersCowsRemain()
        {
            Board b = new Board();
            b.board["A1"] = new Cell(Player.X);
            b.board["A4"] = new Cell(Player.X);
            b.board["A7"] = new Cell(Player.X);
            b.board["B2"] = new Cell(Player.X);
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);

            Assert.That(b.numCows(x.playerID) > 3);
            x.getMove(Arg.Any<string>()).Returns("A1", "G1", "D1");
            b.Move(x);
            x.Received(3).getMove(Arg.Any<string>());

            b.board["D1"] = new Cell(Player.None);
            Assert.That(b.numCows(x.playerID) == 3);

            x.getMove(Arg.Any<string>()).Returns("A4", "G1", "A1");
            b.Move(x);
            x.Received(5).getMove(Arg.Any<string>());
            Assert.That(b.board["G1"].getState == x.playerID);
            Assert.That(b.board["A1"].getState == Player.None);
        }
        //general
        [Test]
        public void MillIsFormedBy3SameCowsInALine()
        {
            Board b = new Board();
            b.board["A1"] = new Cell(Player.X);
            Assert.That(b.isInMill("A1") == false);
            b.board["A4"] = new Cell(Player.X);
            Assert.That(b.isInMill("A1") == false);
            b.board["A7"] = new Cell(Player.X);
            Assert.That(b.isInMill("A1") == true);
        }
        [Test]
        public void MillNotFormedWhenNotSamePlayer()
        {
            Board b = new Board();
            b.board["A1"] = new Cell(Player.X);
            b.board["A4"] = new Cell(Player.O);
            b.board["A7"] = new Cell(Player.X);
            Assert.That(!b.isInMill("A1") && !b.isInMill("A4") && !b.isInMill("A7"));
        }
        [Test]
        public void MillNotFormedWhenConnectionsDoNotFormLine()
        {
            Board b = new Board();
            b.board["A1"] = new Cell(Player.X);
            b.board["A4"] = new Cell(Player.X);
            b.board["B4"] = new Cell(Player.X);
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
            Board b = new Board();
            IPlayer o = Substitute.For<IPlayer>();
            //o.playerID = Player.O;
            o.playerID.Returns(Player.O);
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            Assert.That(x.getOpponent() == Player.X);

            //o.getMove(Arg.Any<string>()).Returns("G1", "G4");
            //b.Place(o);
            //b.Place(o);
            //x.getMove(Arg.Any<string>()).Returns("A1");
            //b.Place(x);
            //o.getMove(Arg.Any<string>()).Returns("G7", "A1");
            //b.Place(o);

            //o.getMove(Arg.Any<string>()).Returns("G1");
            //b.Shoot(o);
            //x.getMove(Arg.Any<string>()).Returns("A7", "D3");
            //b.Place(x);
            Assert.That(1 == 2);//force fail to remind us to fix
        }
        [Test]
        public void CannotShootOwnCows()//baka!
        {
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
            Assert.That(1 == 2);//force fail to remind us to fix
        }
        [Test]
        public void CannotShootEmptySpace()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID = Player.X;
            IPlayer o = Substitute.For<IPlayer>();
            o.playerID = Player.O;

            x.getMove(Arg.Any<string>()).Returns("G1");
            b.Place(x);

            o.getMove(Arg.Any<string>()).Returns("A1","A4","A7","B2","G1");
            b.Place(o);
            b.Place(o);
            b.Place(o);

            Assert.That(b.board["B2"].getState == Player.None && b.board["G1"].getState == Player.None);
        }
        [Test]
        public void ShotCowsRemovedFromBoard()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID = Player.X;
            IPlayer o = Substitute.For<IPlayer>();
            o.playerID = Player.O;

            x.getMove(Arg.Any<string>()).Returns("G1");
            b.Place(x);

            o.getMove(Arg.Any<string>()).Returns("A1", "A4", "A7", "G1");
            b.Place(o);
            b.Place(o);
            b.Place(o);

            Assert.That(b.board["G1"].getState == Player.None);
        }
        [Test]
        public void WinIfOpponentCannotMove()
        {
            MorabarabaManager manager = Substitute.For<MorabarabaManager>();
            IBoard b = manager.gameBoard;
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID = Player.X;
            o.playerID = Player.O;

            x.getMove(Arg.Any<string>()).Returns("A1", "G1", "A7", "G7");
            o.getMove(Arg.Any<string>()).Returns("D1", "A4", "B2", "B6", "D7", "F6", "G4", "F2");//surrounds X pieces without making mills
            b.Place(x);
            b.Place(x);
            b.Place(x);
            b.Place(x);
            b.Place(o);
            b.Place(o);
            b.Place(o);
            b.Place(o);
            b.Place(o);
            b.Place(o);
            b.Place(o);
            b.Place(o);


            Assert.That(manager.movingPhase(), Is.EqualTo("O wins!"));
        }
        [Test]
        public void WinIfOpponentHas2OrLessCowsLeftAfterPlacement()
        {
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
    }
}
