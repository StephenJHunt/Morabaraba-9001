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
            //int counter = 0;
            //Board b = Substitute.For<Board>();
            //IPlayer x = Substitute.For<IPlayer>();
            //MorabarabaManager manager = Substitute.For<MorabarabaManager>();
            //
            //b.DidNotReceiveWithAnyArgs().Move(x);
            //1
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
            Board b = new Board();
            
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            IPlayer o = Substitute.For<IPlayer>();
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            o.getMove(Arg.Any<string>()).Returns("G1");
            b.Place(o);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "A7", "G1");
            b.Place(x);
            b.Place(x);
            b.Place(x);
            Assert.That(b.board["A1"].getState == x.playerID &&b.board["A4"].getState == x.playerID && b.board["A7"].getState == x.playerID && b.board["G1"].getState == Player.None);
        }
        [Test]
        public void CowInMillWhenOtherCowsOfSamePlayerNotInMillCannotBeShot()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            x.getMove(Arg.Any<string>()).Returns("A1");//placing an X piece for O to take
            b.Place(x);

            o.getMove(Arg.Any<string>()).Returns("G1", "G4", "F2", "G7", "A1");//Making O mill with one piece placed outside and shooting X at A1
            b.Place(o);
            b.Place(o);
            b.Place(o);
            b.Place(o);

            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "A7", "G1", "F2");//Making X mill, trying to shoot G1 in mill, failing and shooting F2, which is not in mill
            b.Place(x);
            b.Place(x);
            b.Place(x);

            Assert.That(b.board["G1"].getState == o.playerID && b.board["F2"].getState == Player.None);//test that the cow in a mill couldnt be shot and the one out could and was
            //2
        }
        [Test]
        public void CowInMillWhenAllPlayerCowsInMillCanBeShot()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            x.getMove(Arg.Any<string>()).Returns("A1");//placing an X piece for O to take
            b.Place(x);

            o.getMove(Arg.Any<string>()).Returns("G1", "G4", "G7", "A1");//Making O mill and shooting X at A1
            b.Place(o);
            b.Place(o);
            b.Place(o);

            x.getMove(Arg.Any<string>()).Returns("G1");//get shoot input
            b.Shoot(x);

            Assert.That(b.board["G1"].getState == Player.None);//test that the cow in a mill could be shot and was

            //3
        }
        [Test]
        public void CannotShootOwnCows()//baka!
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            x.getMove(Arg.Any<string>()).Returns("A1");//set own piece to try shoot at A1
            b.Place(x);
            o.getMove(Arg.Any<string>()).Returns("A4");//place opponent at A4
            b.Place(o);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4");//tries to shoot own cow at A1 then shoots opponent at A4 to break out of loop
            b.Shoot(x);

            Assert.That(b.board["A1"].getState == x.playerID && b.board["A4"].getState == Player.None);//check that own piece is untouched and opponent is shot
            //4
        }
        [Test]
        public void CannotShootEmptySpace()
        {
            Board b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);
            
            o.getMove(Arg.Any<string>()).Returns("A4");//place opponent at A4
            b.Place(o);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4");//tries to shoot empty cell at A1 then shoots opponent at A4 to break out of loop
            b.Shoot(x);

            Assert.That(b.board["A1"].getState == Player.None && b.board["A4"].getState == Player.None);//check that A1 was untouched and player was able to still shoot an opponent at A4
            //5
        }
        [Test]
        public void ShotCowsRemovedFromBoard()
        {
            
            //6
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
