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
        public void CowsCannotBeMovedDuringPlacement()//leave for later
        {
            IRef referee = Substitute.For<IRef>();
            
            IBoard b = Substitute.For<Board>();
            MorabarabaManager manager = Substitute.For<MorabarabaManager>();
            //placing phase (need some way to break the loop in it for a test)
            IPlayer pl = Substitute.For<IPlayer>();
            pl.playerID.Returns(Player.X);
            
            IPlayer pl2 = Substitute.For<IPlayer>();
            pl2.playerID.Returns(Player.O);


            pl.getMove(Arg.Any<string>()).Returns("A1", "A7", "B4", "C3", "C5", "D2", "D5", "D7", "E4", "F2", "F6", "G4");
            pl2.getMove(Arg.Any<string>()).Returns("A4", "B2", "B6", "C4", "D1", "D3", "D6", "E3", "E5", "F4", "G1", "G7");

            b.DidNotReceive().Move(Arg.Any<IPlayer>(), Arg.Any<IRef>());
            //manager.placingPhase();
            //placing phase
            //pl.getMove(Arg.Any<string>()).Returns("A1", "A7", "B4", "C3", "C5", "D2", "D5", "D7", "E4", "F2", "F6", "G4");//get a place input and a move input
            //pl.stones.Returns(12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0);
            //pl2.getMove(Arg.Any<string>()).Returns("A4", "B2", "B6", "C4", "D1", "D3", "D6", "E3", "E5", "F4", "G1", "G7");
            //pl.stones.Returns(12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0);
            //manager.xPlayer = pl;
            //manager.oPlayer = pl;
            //manager.gameBoard = b;
            //manager.placingPhase();
            //pl.getMove(Arg.Any<string>()).Returns("A1", "A1", "A4");//get a place input and a move input
            //b.Place(pl);//place the cow
            //b.DidNotReceiveWithAnyArgs().Move(pl);//no moves made when placing a cow
        }
        //moving
        [Test]
        public void ANormalCowCanOnlyMoveToAConnectedSpace()
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
            //IRef referee = new MReferee();
            //IBoard b = Substitute.For<Board>();
            //b.board["A1"] = new Cell(Player.X);
            //b.board["A4"] = new Cell(Player.O);
            //b.board["B2"] = new Cell(Player.None);
            //IPlayer x = Substitute.For<IPlayer>();
            //x.playerID = Player.X;
            //x.stones.Returns(12);
            //Assert.That(!referee.isValidPutDown("A1", "A4", x, b));
            //Assert.That(!referee.isValidPutDown("A1", "A1", x, b));
            //Assert.That(referee.isValidPutDown("A1", "B2", x, b));
        }
        [Test]
        public void MovingDoesNotChangeCowNumbers()
        {
            //IRef referee = Substitute.For<IRef>();
            //referee.isValidPickUp(Arg.Any<string>(), Arg.Any<IPlayer>(), Arg.Any<IBoard>()).ReturnsForAnyArgs(true);
            //referee.isValidPutDown(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IPlayer>(), Arg.Any<IBoard>()).ReturnsForAnyArgs(true);

            //IBoard b = new Board();
            //b.board["A4"] = new Cell(Player.X);

            //IPlayer x = Substitute.For<IPlayer>();
            //x.playerID.Returns(Player.X);

            //x.getMove(Arg.Any<string>()).Returns("A4", "A1");
            //int old = b.numCows(x.playerID);
            //b.Move(x, referee);

            //Assert.That(b.numCows(x.playerID) == old);
        }
        //flying
        [Test]
        public void CowsCanMoveToAnyEmptySpaceWhenOnly3OfThatPlayersCowsRemain()
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
        public void MillIsFormedBy3SameCowsInALine()
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
        public void MillNotFormedWhenNotSamePlayer()
        {
            //IBoard b = new Board();
            //b.board["A1"] = new Cell(Player.X);
            //b.board["A4"] = new Cell(Player.O);
            //b.board["A7"] = new Cell(Player.X);
            //Assert.That(!b.isInMill("A1") && !b.isInMill("A4") && !b.isInMill("A7"));
        }
        [Test]
        public void MillNotFormedWhenConnectionsDoNotFormLine()
        {
            //IBoard b = new Board();
            //b.board["A1"] = new Cell(Player.X);
            //b.board["A4"] = new Cell(Player.X);
            //b.board["B4"] = new Cell(Player.X);
            //Assert.That(!b.isInMill("A1") && !b.isInMill("A4") && !b.isInMill("B4"));
        }
        [Test]
        public void ShootingOnlyPossibleOnMillCreation()//confusion
        {
            //check shooting is only possible by showing that it only happens once even with many pieces being placed, during which only one mill is formed
            IBoard b = new Board();
            
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            IPlayer o = Substitute.For<IPlayer>();
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            o.getMove(Arg.Any<string>()).Returns("G1");
            //b.Place(o);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "A7", "G1");
            //b.Place(x);
            //b.Place(x);
            //b.Place(x);
            //Assert.That(b.board["A1"].getState == x.playerID &&b.board["A4"].getState == x.playerID && b.board["A7"].getState == x.playerID && b.board["G1"].getState == Player.None);
        }
        [Test]
        public void CowInMillWhenOtherCowsOfSamePlayerNotInMillCannotBeShot()
        {
            IBoard b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            x.getMove(Arg.Any<string>()).Returns("A1");//placing an X piece for O to take
            //b.Place(x);

            o.getMove(Arg.Any<string>()).Returns("G1", "G4", "F2", "G7", "A1");//Making O mill with one piece placed outside and shooting X at A1
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);

            x.getMove(Arg.Any<string>()).Returns("A1", "A4", "A7", "G1", "F2");//Making X mill, trying to shoot G1 in mill, failing and shooting F2, which is not in mill
            //b.Place(x);
            //b.Place(x);
            //b.Place(x);

            //Assert.That(b.board["G1"].getState == o.playerID && b.board["F2"].getState == Player.None);//test that the cow in a mill couldnt be shot and the one out could and was
            //2
        }
        [Test]
        public void CowInMillWhenAllPlayerCowsInMillCanBeShot()
        {
            IBoard b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            x.getMove(Arg.Any<string>()).Returns("A1");//placing an X piece for O to take
            //b.Place(x);

            o.getMove(Arg.Any<string>()).Returns("G1", "G4", "G7", "A1");//Making O mill and shooting X at A1
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);

            x.getMove(Arg.Any<string>()).Returns("G1");//get shoot input
            //b.Shoot(x);

            //Assert.That(b.board["G1"].getState == Player.None);//test that the cow in a mill could be shot and was

            //3
        }
        [Test]
        public void CannotShootOwnCows()//baka!
        {
            IBoard b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            x.getMove(Arg.Any<string>()).Returns("A1");//set own piece to try shoot at A1
            //b.Place(x);
            o.getMove(Arg.Any<string>()).Returns("A4");//place opponent at A4
            //b.Place(o);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4");//tries to shoot own cow at A1 then shoots opponent at A4 to break out of loop
            //b.Shoot(x);

           // Assert.That(b.board["A1"].getState == x.playerID && b.board["A4"].getState == Player.None);//check that own piece is untouched and opponent is shot
            //4
        }
        [Test]
        public void CannotShootEmptySpace()
        {
            IBoard b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);
            
            o.getMove(Arg.Any<string>()).Returns("A4");//place opponent at A4
            //b.Place(o);
            x.getMove(Arg.Any<string>()).Returns("A1", "A4");//tries to shoot empty cell at A1 then shoots opponent at A4 to break out of loop
            //b.Shoot(x);

            //Assert.That(b.board["A1"].getState == Player.None && b.board["A4"].getState == Player.None);//check that A1 was untouched and player was able to still shoot an opponent at A4
            //5
        }
        [Test]
        public void ShotCowsRemovedFromBoard()
        {
            IBoard b = new Board();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.getOpponent().Returns(Player.O);
            o.playerID.Returns(Player.O);
            o.getOpponent().Returns(Player.X);

            o.getMove(Arg.Any<string>()).Returns("A4");//place opponent at A4
            //b.Place(o);
            x.getMove(Arg.Any<string>()).Returns("A4");//shoots opponent at A4
            //b.Shoot(x);

            //Assert.That(b.board["A4"].getState == Player.None);//check that position of shot cow is now empty
            //6
        }
        [Test]
        public void WinIfOpponentCannotMove()
        {
            //MorabarabaManager manager = Substitute.For<MorabarabaManager>();
            //IBoard b = manager.gameBoard;
            //IPlayer x = Substitute.For<IPlayer>();
            //IPlayer o = Substitute.For<IPlayer>();
            //x.playerID = Player.X;
            //o.playerID = Player.O;

            //x.getMove(Arg.Any<string>()).Returns("A1", "G1", "A7", "G7");
            //o.getMove(Arg.Any<string>()).Returns("D1", "A4", "B2", "B6", "D7", "F6", "G4", "F2");//surrounds X pieces without making mills
            //b.Place(x);
            //b.Place(x);
            //b.Place(x);
            //b.Place(x);
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);
            //b.Place(o);


            //Assert.That(manager.movingPhase(), Is.EqualTo("O wins!"));
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
