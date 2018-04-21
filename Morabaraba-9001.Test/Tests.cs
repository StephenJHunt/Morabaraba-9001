using System;
using NUnit.Framework;
using NSubstitute;
using System.Linq;
using System.Diagnostics;

namespace Morabaraba_9001.Test
{
	[TestFixture]
    
    public class Tests
    {
        //placing
        [Test]
        public void AtStartBoardIsEmpty()
        {
            IBoard b = new Board();

            var query =
                from pos in Board.validPositions
                where b.getCellState(pos) != Player.None
                select pos;
            Assert.That(!query.Any());
        }
        [Test]
        public void PlayerXStartsFirst()//Player X is our equivalent for the dark cows player
        {
            MorabarabaManager manager = new MorabarabaManager(new Board(), new GamePlayer(Player.X), new GamePlayer(Player.O), new MReferee());
            Assert.That(manager.currPlayer.playerID == Player.X);
        }
        [Test]
        public void CowsCanOnlyBePlayedOnEmptySpaces()
        {
            IRef referee = new MReferee();

            IBoard b = Substitute.For<IBoard>();
            b.getCellState("A1").Returns(Player.O);
            b.getCellState("A4").Returns(Player.None);
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.stones.Returns(12);
            Assert.That(!referee.isValidPlacement("A1", x, b) && referee.isValidPlacement("A4", x, b));
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
        public void CowsCannotBeMovedDuringPlacement()
        {
            IRef referee = Substitute.For<IRef>();
            IBoard b = Substitute.For<IBoard>();
            IPlayer pl1 = Substitute.For<IPlayer>();
            IPlayer pl2 = Substitute.For<IPlayer>();

            pl1.playerID.Returns(Player.X);
            pl2.playerID.Returns(Player.O);

            pl1.getMove(Arg.Any<string>()).Returns("A1", "A7", "B4", "C3", "C5", "D2", "D5", "D7", "E4", "F2", "F6", "G4");
            pl2.getMove(Arg.Any<string>()).Returns("A4", "B2", "B6", "C4", "D1", "D3", "D6", "E3", "E5", "F4", "G1", "G7");

            b.Place(Arg.Any<IPlayer>(), Arg.Any<IRef>()).ReturnsForAnyArgs(PlaceResult.Done);

            referee.inPlacing(Arg.Any<IPlayer>(), Arg.Any<IPlayer>()).ReturnsForAnyArgs(true, true, true, true, true, true, true, true, true, true, true, true,
                                                                                        true, true, true, true, true, true, true, true, true, true, true, true, false);

            MorabarabaManager manager = new MorabarabaManager(b, pl1, pl2, referee);
            manager.placingPhase();

            b.DidNotReceive().Move(Arg.Any<IPlayer>(), Arg.Any<IRef>());

        }
        //moving
        [Test]
        public void ANormalCowCanOnlyMoveToAConnectedSpace()//TODO
        {
            //IRef referee = new MReferee();
            //IBoard b = Substitute.For<Board>();
            //b.board["A1"] = new Cell(Player.X);
            //b.board["A4"] = new Cell(Player.None);
            //b.board["A7"] = new Cell(Player.None);
            //IPlayer x = Substitute.For<IPlayer>();
            //x.playerID = Player.X;
            //x.stones.Returns(12);
            //Assert.That(referee.isValidPutDown("A1", "A4", x, b));
            //Assert.That(!referee.isValidPutDown("A1", "A7", x, b));
        }
        [Test]
        public void CowCanOnlyMoveToEmptySpace()
        {
            IRef referee = new MReferee();
            IBoard b = Substitute.For<IBoard>();
            b.getCellState("A1").Returns(Player.X);
            b.getCellState("A4").Returns(Player.O);
            b.getCellState("B2").Returns(Player.None);
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.stones.Returns(12);
            Assert.That(!referee.isValidPutDown("A1", "A4", x, b));
            Assert.That(!referee.isValidPutDown("A1", "A1", x, b));
            Assert.That(referee.isValidPutDown("A1", "B2", x, b));
        }
        [Test]
        public void MovingDoesNotChangeCowNumbers()
        {
            IRef referee = Substitute.For<IRef>();
            referee.isValidPickUp(Arg.Any<string>(), Arg.Any<IPlayer>(), Arg.Any<IBoard>()).ReturnsForAnyArgs(true);
            referee.isValidPlacement(Arg.Any<string>(), Arg.Any<IPlayer>(), Arg.Any<IBoard>()).ReturnsForAnyArgs(true);
            referee.isValidPutDown(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IPlayer>(), Arg.Any<IBoard>()).ReturnsForAnyArgs(true);

            IBoard b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);

            x.getMove(Arg.Any<string>()).Returns("A4", "A1");
            b.Place(x, referee);

            Assert.That(b.numCows(x.playerID) == 1);
            x.getMove(Arg.Any<string>()).Returns("A4", "A1");
            Assert.That(b.Move(x, referee) == MoveResult.Done);
            Assert.That(b.numCows(x.playerID) == 1);
        }
        //flying
        [Test]
        public void CowsCanMoveToAnyEmptySpaceWhenOnly3OfThatPlayersCowsRemain()//TODO
        {
            //IRef referee = new MReferee();
            //IBoard b = Substitute.For<Board>();
            //b.board["A1"] = new Cell(Player.X);
            //b.board["A4"] = new Cell(Player.O);
            //b.board["D1"] = new Cell(Player.O);
            //b.board["B2"] = new Cell(Player.O);
            //b.board["A7"] = new Cell(Player.O);
            //b.board["G1"] = new Cell(Player.None);
            //IPlayer x = Substitute.For<IPlayer>();
            //x.playerID.Returns(Player.X);
            //b.numCows(Player.X).Returns(3);

            //Assert.That(!referee.isValidPutDown("A1", "A4", x, b));
            //Assert.That(!referee.isValidPutDown("A1", "B2", x, b));
            //Assert.That(!referee.isValidPutDown("A1", "D1", x, b));
            //Assert.That(!referee.isValidPutDown("A1", "A7", x, b));
            //Assert.That(referee.isValidPutDown("A1", "G1", x, b));
        }
        //general
        [Test]
        public void MillIsFormedBy3SameCowsInALine()//TODO
        {
            //IBoard b = new Board();
            //b.board["A1"] = new Cell(Player.X);
            //Assert.That(b.isInMill("A1") == false);
            //b.board["A4"] = new Cell(Player.X);
            //Assert.That(b.isInMill("A1") == false);
            //b.board["A7"] = new Cell(Player.X);
            //Assert.That(b.isInMill("A1") == true);
        }
        [Test]
        public void MillNotFormedWhenNotSamePlayer()//TODO
        {
            //IBoard b = new Board();
            //b.board["A1"] = new Cell(Player.X);
            //b.board["A4"] = new Cell(Player.O);
            //b.board["A7"] = new Cell(Player.X);
            //Assert.That(!b.isInMill("A1") && !b.isInMill("A4") && !b.isInMill("A7"));
        }
        [Test]
        public void MillNotFormedWhenConnectionsDoNotFormLine()//TODO
        {
            //IBoard b = new Board();
            //b.board["A1"] = new Cell(Player.X);
            //b.board["A4"] = new Cell(Player.X);
            //b.board["B4"] = new Cell(Player.X);
            //Assert.That(!b.isInMill("A1") && !b.isInMill("A4") && !b.isInMill("B4"));
        }
        [Test]
        public void ShootingOnlyPossibleOnMillCreation()
        {
            //check shooting is only possible by showing that it only happens once even with many pieces being placed, during which only one mill is formed
            IRef referee = Substitute.For<IRef>();
            referee.isValidPlacement(Arg.Any<string>(), Arg.Any<IPlayer>(), Arg.Any<IBoard>()).ReturnsForAnyArgs(true);
            IBoard b = Substitute.ForPartsOf<Board>();
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            IPlayer o = Substitute.For<IPlayer>();
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            o.getMove(Arg.Any<string>()).Returns("G1");
            b.Place(o, referee);
            b.DidNotReceive().Shoot(o, referee);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "A7", "G1");
            b.Place(x, referee);
            b.Place(x, referee);
            b.Place(x, referee);
            b.Received(1).Shoot(x, referee);
            //Assert.That(b.board["A1"].getState == x.playerID &&b.board["A4"].getState == x.playerID && b.board["A7"].getState == x.playerID && b.board["G1"].getState == Player.None);
        }
        [Test]
        public void CowInMillWhenOtherCowsOfSamePlayerNotInMillCannotBeShot()
        {
            IRef referee = new MReferee();
            IBoard b = Substitute.For<IBoard>();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            b.getCellState("A1").Returns(Player.X);
            b.getCellState("A4").Returns(Player.X);
            b.getCellState("A7").Returns(Player.X);
            b.getCellState("B2").Returns(Player.X);
            b.isInMill("A1").Returns(true);
            b.isInMill("A4").Returns(true);
            b.isInMill("A7").Returns(true);
            b.isInMill("B2").Returns(false);

            Assert.That(!referee.isValidShot("A1", o, b));
            Assert.That(!referee.isValidShot("A4", o, b));
            Assert.That(!referee.isValidShot("A7", o, b));
            Assert.That(referee.isValidShot("B2", o, b));


        }
        [Test]
        public void CowInMillWhenAllPlayerCowsInMillCanBeShot()
        {
            IRef referee = new MReferee();
            IBoard b = Substitute.For<IBoard>();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            b.getCellState("A1").Returns(Player.X);
            b.getCellState("A4").Returns(Player.X);
            b.getCellState("A7").Returns(Player.X);
            b.isInMill("A1").Returns(true);
            b.isInMill("A4").Returns(true);
            b.isInMill("A7").Returns(true);
            b.allInMill(x.playerID).Returns(true);

            Assert.That(referee.isValidShot("A1", o, b));
            Assert.That(referee.isValidShot("A4", o, b));
            Assert.That(referee.isValidShot("A7", o, b));
        }
        [Test]
        public void CannotShootOwnCows()//baka!
        {
            IRef referee = new MReferee();
            IBoard b = Substitute.For<IBoard>();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            b.getCellState("A1").Returns(Player.X);
            b.isInMill("A1").Returns(false);

            Assert.That(!referee.isValidShot("A1", x, b));
        }
        [Test]
        public void CannotShootEmptySpace()
        {
            IRef referee = new MReferee();
            IBoard b = Substitute.For<IBoard>();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            b.getCellState("A1").Returns(Player.None);
            b.isInMill("A1").Returns(false);

            Assert.That(!referee.isValidShot("A1", o, b));
        }
        [Test]
        public void ShotCowsRemovedFromBoard()
        {
            //IBoard b = new Board();
            //IPlayer x = Substitute.For<IPlayer>();
            //IPlayer o = Substitute.For<IPlayer>();
            //x.playerID.Returns(Player.X);
            //x.getOpponent().Returns(Player.O);
            //o.playerID.Returns(Player.O);
            //o.getOpponent().Returns(Player.X);

            //o.getMove(Arg.Any<string>()).Returns("A4");//place opponent at A4
            ////b.Place(o);
            //x.getMove(Arg.Any<string>()).Returns("A4");//shoots opponent at A4
            ////b.Shoot(x);

            ////Assert.That(b.board["A4"].getState == Player.None);//check that position of shot cow is now empty
            ////6
        }
        [Test]
        public void WinIfOpponentCannotMove()
        {
            IRef referee = new MReferee();
            IBoard b = Substitute.For<IBoard>();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            o.playerID.Returns(Player.O);
            b.canPlay(x).Returns(false);
            b.canPlay(o).Returns(true);
            Assert.That(referee.getGameState(x, o, b) == GameResult.Player2);
            
        }
        [Test]
        public void WinIfOpponentHas2OrLessCowsLeftAfterPlacement()
        {
            //MorabarabaManager manager = Substitute.For<MorabarabaManager>();
            //IBoard b = manager.gameBoard;
            //IPlayer x = Substitute.For<IPlayer>();
            //IPlayer o = Substitute.For<IPlayer>();
            //x.setID(Player.X);
            //o.setID(Player.O);

            //x.getMove(Arg.Any<string>()).Returns("A1", "A4", "B4", "B4", "A7", "F4");
            //o.getMove(Arg.Any<string>()).Returns("G1", "G4", "F4");
            //b.Place(x);
            //b.Place(x);
            //b.Place(x);
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);


            //Assert.That(manager.movingPhase(), Is.EqualTo("X wins!"));
        }
    }
}
