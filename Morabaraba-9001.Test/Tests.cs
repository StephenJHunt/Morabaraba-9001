﻿using System;
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



        static object[] ConnectedMoves =
        {
            new object[] {"A1", "A4", true},
            new object[] {"A1", "D1", true},
            new object[] {"A1", "B2", true},

            new object[] {"A1", "A1", false},
            new object[] {"A1", "A7", false},
            new object[] {"A1", "B4", false},
            new object[] {"A1", "B6", false},
            new object[] {"A1", "C3", false},
            new object[] {"A1", "C4", false},
            new object[] {"A1", "C5", false},
            new object[] {"A1", "D2", false},
            new object[] {"A1", "D3", false},
            new object[] {"A1", "D5", false},
            new object[] {"A1", "D6", false},
            new object[] {"A1", "D7", false},
            new object[] {"A1", "E3", false},
            new object[] {"A1", "E4", false},
            new object[] {"A1", "E5", false},
            new object[] {"A1", "F2", false},
            new object[] {"A1", "F4", false},
            new object[] {"A1", "F6", false},
            new object[] {"A1", "G1", false},
            new object[] {"A1", "G4", false},
            new object[] {"A1", "G7", false},


            new object[] {"A4", "A1", true},
            new object[] {"A4", "B4", true},
            new object[] {"A4", "A7", true},

            new object[] {"A4", "A4", false},
            new object[] {"A4", "B2", false},
            new object[] {"A4", "B6", false},
            new object[] {"A4", "C3", false},
            new object[] {"A4", "C4", false},
            new object[] {"A4", "C5", false},
            new object[] {"A4", "D1", false},
            new object[] {"A4", "D2", false},
            new object[] {"A4", "D3", false},
            new object[] {"A4", "D5", false},
            new object[] {"A4", "D6", false},
            new object[] {"A4", "D7", false},
            new object[] {"A4", "E3", false},
            new object[] {"A4", "E4", false},
            new object[] {"A4", "E5", false},
            new object[] {"A4", "F2", false},
            new object[] {"A4", "F4", false},
            new object[] {"A4", "F6", false},
            new object[] {"A4", "G1", false},
            new object[] {"A4", "G4", false},
            new object[] {"A4", "G7", false},


            new object[] {"A7", "A4", true},
            new object[] {"A7", "B6", true},
            new object[] {"A7", "D7", true},

            new object[] {"A7", "A1", false},
            new object[] {"A7", "A7", false},
            new object[] {"A7", "B2", false},
            new object[] {"A7", "B4", false},
            new object[] {"A7", "C3", false},
            new object[] {"A7", "C4", false},
            new object[] {"A7", "C5", false},
            new object[] {"A7", "D1", false},
            new object[] {"A7", "D2", false},
            new object[] {"A7", "D3", false},
            new object[] {"A7", "D5", false},
            new object[] {"A7", "D6", false},
            new object[] {"A7", "E3", false},
            new object[] {"A7", "E4", false},
            new object[] {"A7", "E5", false},
            new object[] {"A7", "F2", false},
            new object[] {"A7", "F4", false},
            new object[] {"A7", "F6", false},
            new object[] {"A7", "G1", false},
            new object[] {"A7", "G4", false},
            new object[] {"A7", "G7", false},


            new object[] {"B2", "A1", true},
            new object[] {"B2", "D2", true},
            new object[] {"B2", "C3", true},
            new object[] {"B2", "B4", true},
            
            new object[] {"B2", "A4", false},
            new object[] {"B2", "A7", false},
            new object[] {"B2", "B2", false},
            new object[] {"B2", "B6", false},
            new object[] {"B2", "C4", false},
            new object[] {"B2", "C5", false},
            new object[] {"B2", "D1", false},
            new object[] {"B2", "D3", false},
            new object[] {"B2", "D5", false},
            new object[] {"B2", "D6", false},
            new object[] {"B2", "D7", false},
            new object[] {"B2", "E3", false},
            new object[] {"B2", "E4", false},
            new object[] {"B2", "E5", false},
            new object[] {"B2", "F2", false},
            new object[] {"B2", "F4", false},
            new object[] {"B2", "F6", false},
            new object[] {"B2", "G1", false},
            new object[] {"B2", "G4", false},
            new object[] {"B2", "G7", false},


            new object[] {"B4", "B2", true},
            new object[] {"B4", "A4", true},
            new object[] {"B4", "C4", true},
            new object[] {"B4", "B6", true},

            new object[] {"B4", "A1", false},
            new object[] {"B4", "A7", false},
            new object[] {"B4", "B4", false},
            new object[] {"B4", "C3", false},
            new object[] {"B4", "C5", false},
            new object[] {"B4", "D1", false},
            new object[] {"B4", "D2", false},
            new object[] {"B4", "D3", false},
            new object[] {"B4", "D5", false},
            new object[] {"B4", "D6", false},
            new object[] {"B4", "D7", false},
            new object[] {"B4", "E3", false},
            new object[] {"B4", "E4", false},
            new object[] {"B4", "E5", false},
            new object[] {"B4", "F2", false},
            new object[] {"B4", "F4", false},
            new object[] {"B4", "F6", false},
            new object[] {"B4", "G1", false},
            new object[] {"B4", "G4", false},
            new object[] {"B4", "G7", false},


            new object[] {"B6", "B4", true},
            new object[] {"B6", "C5", true},
            new object[] {"B6", "D6", true},
            new object[] {"B6", "A7", true},

            new object[] {"B6", "A1", false},
            new object[] {"B6", "A4", false},
            new object[] {"B6", "B2", false},
            new object[] {"B6", "B6", false},
            new object[] {"B6", "C3", false},
            new object[] {"B6", "C4", false},
            new object[] {"B6", "D1", false},
            new object[] {"B6", "D2", false},
            new object[] {"B6", "D3", false},
            new object[] {"B6", "D5", false},
            new object[] {"B6", "D7", false},
            new object[] {"B6", "E3", false},
            new object[] {"B6", "E4", false},
            new object[] {"B6", "E5", false},
            new object[] {"B6", "F2", false},
            new object[] {"B6", "F4", false},
            new object[] {"B6", "F6", false},
            new object[] {"B6", "G1", false},
            new object[] {"B6", "G4", false},
            new object[] {"B6", "G7", false},


            new object[] {"C3", "B2", true},
            new object[] {"C3", "C4", true},
            new object[] {"C3", "D3", true},

            new object[] {"C3", "A1", false},
            new object[] {"C3", "A4", false},
            new object[] {"C3", "A7", false},
            new object[] {"C3", "B4", false},
            new object[] {"C3", "B6", false},
            new object[] {"C3", "C3", false},
            new object[] {"C3", "C5", false},
            new object[] {"C3", "D1", false},
            new object[] {"C3", "D2", false},
            new object[] {"C3", "D5", false},
            new object[] {"C3", "D6", false},
            new object[] {"C3", "D7", false},
            new object[] {"C3", "E3", false},
            new object[] {"C3", "E4", false},
            new object[] {"C3", "E5", false},
            new object[] {"C3", "F2", false},
            new object[] {"C3", "F4", false},
            new object[] {"C3", "F6", false},
            new object[] {"C3", "G1", false},
            new object[] {"C3", "G4", false},
            new object[] {"C3", "G7", false},


            new object[] {"C4", "C3", true},
            new object[] {"C4", "B4", true},
            new object[] {"C4", "C5", true},

            new object[] {"C4", "A1", false},
            new object[] {"C4", "A4", false},
            new object[] {"C4", "A7", false},
            new object[] {"C4", "B2", false},
            new object[] {"C4", "B6", false},
            new object[] {"C4", "C4", false},
            new object[] {"C4", "D1", false},
            new object[] {"C4", "D2", false},
            new object[] {"C4", "D3", false},
            new object[] {"C4", "D5", false},
            new object[] {"C4", "D6", false},
            new object[] {"C4", "D7", false},
            new object[] {"C4", "E3", false},
            new object[] {"C4", "E4", false},
            new object[] {"C4", "E5", false},
            new object[] {"C4", "F2", false},
            new object[] {"C4", "F4", false},
            new object[] {"C4", "F6", false},
            new object[] {"C4", "G1", false},
            new object[] {"C4", "G4", false},
            new object[] {"C4", "G7", false},


            new object[] {"C5", "C4", true},
            new object[] {"C5", "D5", true},
            new object[] {"C5", "B6", true},

            new object[] {"C5", "A1", false},
            new object[] {"C5", "A4", false},
            new object[] {"C5", "A7", false},
            new object[] {"C5", "B2", false},
            new object[] {"C5", "B4", false},
            new object[] {"C5", "C3", false},
            new object[] {"C5", "C5", false},
            new object[] {"C5", "D1", false},
            new object[] {"C5", "D2", false},
            new object[] {"C5", "D3", false},
            new object[] {"C5", "D6", false},
            new object[] {"C5", "D7", false},
            new object[] {"C5", "E3", false},
            new object[] {"C5", "E4", false},
            new object[] {"C5", "E5", false},
            new object[] {"C5", "F2", false},
            new object[] {"C5", "F4", false},
            new object[] {"C5", "F6", false},
            new object[] {"C5", "G1", false},
            new object[] {"C5", "G4", false},
            new object[] {"C5", "G7", false},


            new object[] {"D1", "A1", true},
            new object[] {"D1", "G1", true},
            new object[] {"D1", "D2", true},
            
            new object[] {"D1", "A4", false},
            new object[] {"D1", "A7", false},
            new object[] {"D1", "B2", false},
            new object[] {"D1", "B4", false},
            new object[] {"D1", "B6", false},
            new object[] {"D1", "C3", false},
            new object[] {"D1", "C4", false},
            new object[] {"D1", "C5", false},
            new object[] {"D1", "D1", false},
            new object[] {"D1", "D3", false},
            new object[] {"D1", "D5", false},
            new object[] {"D1", "D6", false},
            new object[] {"D1", "D7", false},
            new object[] {"D1", "E3", false},
            new object[] {"D1", "E4", false},
            new object[] {"D1", "E5", false},
            new object[] {"D1", "F2", false},
            new object[] {"D1", "F4", false},
            new object[] {"D1", "F6", false},
            new object[] {"D1", "G4", false},
            new object[] {"D1", "G7", false},


            new object[] {"D2", "D1", true},
            new object[] {"D2", "F2", true},
            new object[] {"D2", "D3", true},
            new object[] {"D2", "B2", true},

            new object[] {"D2", "A1", false},
            new object[] {"D2", "A4", false},
            new object[] {"D2", "A7", false},
            new object[] {"D2", "B4", false},
            new object[] {"D2", "B6", false},
            new object[] {"D2", "C3", false},
            new object[] {"D2", "C4", false},
            new object[] {"D2", "C5", false},
            new object[] {"D2", "D2", false},
            new object[] {"D2", "D5", false},
            new object[] {"D2", "D6", false},
            new object[] {"D2", "D7", false},
            new object[] {"D2", "E3", false},
            new object[] {"D2", "E4", false},
            new object[] {"D2", "E5", false},
            new object[] {"D2", "F4", false},
            new object[] {"D2", "F6", false},
            new object[] {"D2", "G1", false},
            new object[] {"D2", "G4", false},
            new object[] {"D2", "G7", false},


            new object[] {"D3", "D2", true},
            new object[] {"D3", "E3", true},
            new object[] {"D3", "C3", true},

            new object[] {"D3", "A1", false},
            new object[] {"D3", "A4", false},
            new object[] {"D3", "A7", false},
            new object[] {"D3", "B2", false},
            new object[] {"D3", "B4", false},
            new object[] {"D3", "B6", false},
            new object[] {"D3", "C4", false},
            new object[] {"D3", "C5", false},
            new object[] {"D3", "D1", false},
            new object[] {"D3", "D3", false},
            new object[] {"D3", "D5", false},
            new object[] {"D3", "D6", false},
            new object[] {"D3", "D7", false},
            new object[] {"D3", "E4", false},
            new object[] {"D3", "E5", false},
            new object[] {"D3", "F2", false},
            new object[] {"D3", "F4", false},
            new object[] {"D3", "F6", false},
            new object[] {"D3", "G1", false},
            new object[] {"D3", "G4", false},
            new object[] {"D3", "G7", false},


            new object[] {"D5", "E5", true},
            new object[] {"D5", "D6", true},
            new object[] {"D5", "C5", true},

            new object[] {"D5", "A1", false},
            new object[] {"D5", "A4", false},
            new object[] {"D5", "A7", false},
            new object[] {"D5", "B2", false},
            new object[] {"D5", "B4", false},
            new object[] {"D5", "B6", false},
            new object[] {"D5", "C3", false},
            new object[] {"D5", "C4", false},
            new object[] {"D5", "D1", false},
            new object[] {"D5", "D2", false},
            new object[] {"D5", "D3", false},
            new object[] {"D5", "D5", false},
            new object[] {"D5", "D7", false},
            new object[] {"D5", "E3", false},
            new object[] {"D5", "E4", false},
            new object[] {"D5", "F2", false},
            new object[] {"D5", "F4", false},
            new object[] {"D5", "F6", false},
            new object[] {"D5", "G1", false},
            new object[] {"D5", "G4", false},
            new object[] {"D5", "G7", false},


            new object[] {"D6", "D5", true},
            new object[] {"D6", "F6", true},
            new object[] {"D6", "D7", true},
            new object[] {"D6", "B6", true},

            new object[] {"D6", "A1", false},
            new object[] {"D6", "A4", false},
            new object[] {"D6", "A7", false},
            new object[] {"D6", "B2", false},
            new object[] {"D6", "B4", false},
            new object[] {"D6", "C3", false},
            new object[] {"D6", "C4", false},
            new object[] {"D6", "C5", false},
            new object[] {"D6", "D1", false},
            new object[] {"D6", "D2", false},
            new object[] {"D6", "D3", false},
            new object[] {"D6", "D6", false},
            new object[] {"D6", "E3", false},
            new object[] {"D6", "E4", false},
            new object[] {"D6", "E5", false},
            new object[] {"D6", "F2", false},
            new object[] {"D6", "F4", false},
            new object[] {"D6", "G1", false},
            new object[] {"D6", "G4", false},
            new object[] {"D6", "G7", false},


            new object[] {"D7", "D6", true},
            new object[] {"D7", "G7", true},
            new object[] {"D7", "A7", true},

            new object[] {"D7", "A1", false},
            new object[] {"D7", "A4", false},
            new object[] {"D7", "B2", false},
            new object[] {"D7", "B4", false},
            new object[] {"D7", "B6", false},
            new object[] {"D7", "C3", false},
            new object[] {"D7", "C4", false},
            new object[] {"D7", "C5", false},
            new object[] {"D7", "D1", false},
            new object[] {"D7", "D2", false},
            new object[] {"D7", "D3", false},
            new object[] {"D7", "D5", false},
            new object[] {"D7", "D7", false},
            new object[] {"D7", "E3", false},
            new object[] {"D7", "E4", false},
            new object[] {"D7", "E5", false},
            new object[] {"D7", "F2", false},
            new object[] {"D7", "F4", false},
            new object[] {"D7", "F6", false},
            new object[] {"D7", "G1", false},
            new object[] {"D7", "G4", false},


            new object[] {"E3", "F2", true},
            new object[] {"E3", "E4", true},
            new object[] {"E3", "D3", true},

            new object[] {"E3", "A1", false},
            new object[] {"E3", "A4", false},
            new object[] {"E3", "A7", false},
            new object[] {"E3", "B2", false},
            new object[] {"E3", "B4", false},
            new object[] {"E3", "B6", false},
            new object[] {"E3", "C3", false},
            new object[] {"E3", "C4", false},
            new object[] {"E3", "C5", false},
            new object[] {"E3", "D1", false},
            new object[] {"E3", "D2", false},
            new object[] {"E3", "D5", false},
            new object[] {"E3", "D6", false},
            new object[] {"E3", "D7", false},
            new object[] {"E3", "E3", false},
            new object[] {"E3", "E5", false},
            new object[] {"E3", "F4", false},
            new object[] {"E3", "F6", false},
            new object[] {"E3", "G1", false},
            new object[] {"E3", "G4", false},
            new object[] {"E3", "G7", false},


            new object[] {"E4", "E3", true},
            new object[] {"E4", "F4", true},
            new object[] {"E4", "E5", true},

            new object[] {"E4", "A1", false},
            new object[] {"E4", "A4", false},
            new object[] {"E4", "A7", false},
            new object[] {"E4", "B2", false},
            new object[] {"E4", "B4", false},
            new object[] {"E4", "B6", false},
            new object[] {"E4", "C3", false},
            new object[] {"E4", "C4", false},
            new object[] {"E4", "C5", false},
            new object[] {"E4", "D1", false},
            new object[] {"E4", "D2", false},
            new object[] {"E4", "D3", false},
            new object[] {"E4", "D5", false},
            new object[] {"E4", "D6", false},
            new object[] {"E4", "D7", false},
            new object[] {"E4", "E4", false},
            new object[] {"E4", "F2", false},
            new object[] {"E4", "F6", false},
            new object[] {"E4", "G1", false},
            new object[] {"E4", "G4", false},
            new object[] {"E4", "G7", false},


            new object[] {"E5", "E4", true},
            new object[] {"E5", "F6", true},
            new object[] {"E5", "D5", true},

            new object[] {"E5", "A1", false},
            new object[] {"E5", "A4", false},
            new object[] {"E5", "A7", false},
            new object[] {"E5", "B2", false},
            new object[] {"E5", "B4", false},
            new object[] {"E5", "B6", false},
            new object[] {"E5", "C3", false},
            new object[] {"E5", "C4", false},
            new object[] {"E5", "C5", false},
            new object[] {"E5", "D1", false},
            new object[] {"E5", "D2", false},
            new object[] {"E5", "D3", false},
            new object[] {"E5", "D6", false},
            new object[] {"E5", "D7", false},
            new object[] {"E5", "E3", false},
            new object[] {"E5", "E5", false},
            new object[] {"E5", "F2", false},
            new object[] {"E5", "F4", false},
            new object[] {"E5", "G1", false},
            new object[] {"E5", "G4", false},
            new object[] {"E5", "G7", false},


            new object[] {"F2", "G1", true},
            new object[] {"F2", "F4", true},
            new object[] {"F2", "E3", true},
            new object[] {"F2", "D2", true},

            new object[] {"F2", "A1", false},
            new object[] {"F2", "A4", false},
            new object[] {"F2", "A7", false},
            new object[] {"F2", "B2", false},
            new object[] {"F2", "B4", false},
            new object[] {"F2", "B6", false},
            new object[] {"F2", "C3", false},
            new object[] {"F2", "C4", false},
            new object[] {"F2", "C5", false},
            new object[] {"F2", "D1", false},
            new object[] {"F2", "D3", false},
            new object[] {"F2", "D5", false},
            new object[] {"F2", "D6", false},
            new object[] {"F2", "D7", false},
            new object[] {"F2", "E4", false},
            new object[] {"F2", "E5", false},
            new object[] {"F2", "F2", false},
            new object[] {"F2", "F6", false},
            new object[] {"F2", "G4", false},
            new object[] {"F2", "G7", false},


            new object[] {"F4", "F2", true},
            new object[] {"F4", "G4", true},
            new object[] {"F4", "F6", true},
            new object[] {"F4", "E4", true},

            new object[] {"F4", "A1", false},
            new object[] {"F4", "A4", false},
            new object[] {"F4", "A7", false},
            new object[] {"F4", "B2", false},
            new object[] {"F4", "B4", false},
            new object[] {"F4", "B6", false},
            new object[] {"F4", "C3", false},
            new object[] {"F4", "C4", false},
            new object[] {"F4", "C5", false},
            new object[] {"F4", "D1", false},
            new object[] {"F4", "D2", false},
            new object[] {"F4", "D3", false},
            new object[] {"F4", "D5", false},
            new object[] {"F4", "D6", false},
            new object[] {"F4", "D7", false},
            new object[] {"F4", "E3", false},
            new object[] {"F4", "E5", false},
            new object[] {"F4", "F4", false},
            new object[] {"F4", "G1", false},
            new object[] {"F4", "G7", false},


            new object[] {"F6", "F4", true},
            new object[] {"F6", "G7", true},
            new object[] {"F6", "D6", true},
            new object[] {"F6", "E5", true},

            new object[] {"F6", "A1", false},
            new object[] {"F6", "A4", false},
            new object[] {"F6", "A7", false},
            new object[] {"F6", "B2", false},
            new object[] {"F6", "B4", false},
            new object[] {"F6", "B6", false},
            new object[] {"F6", "C3", false},
            new object[] {"F6", "C4", false},
            new object[] {"F6", "C5", false},
            new object[] {"F6", "D1", false},
            new object[] {"F6", "D2", false},
            new object[] {"F6", "D3", false},
            new object[] {"F6", "D5", false},
            new object[] {"F6", "D7", false},
            new object[] {"F6", "E3", false},
            new object[] {"F6", "E4", false},
            new object[] {"F6", "F2", false},
            new object[] {"F6", "F6", false},
            new object[] {"F6", "G1", false},
            new object[] {"F6", "G4", false},


            new object[] {"G1", "D1", true},
            new object[] {"G1", "G4", true},
            new object[] {"G1", "F2", true},

            new object[] {"G1", "A1", false},
            new object[] {"G1", "A4", false},
            new object[] {"G1", "A7", false},
            new object[] {"G1", "B2", false},
            new object[] {"G1", "B4", false},
            new object[] {"G1", "B6", false},
            new object[] {"G1", "C3", false},
            new object[] {"G1", "C4", false},
            new object[] {"G1", "C5", false},
            new object[] {"G1", "D2", false},
            new object[] {"G1", "D3", false},
            new object[] {"G1", "D5", false},
            new object[] {"G1", "D6", false},
            new object[] {"G1", "D7", false},
            new object[] {"G1", "E3", false},
            new object[] {"G1", "E4", false},
            new object[] {"G1", "E5", false},
            new object[] {"G1", "F4", false},
            new object[] {"G1", "F6", false},
            new object[] {"G1", "G1", false},
            new object[] {"G1", "G7", false},


            new object[] {"G4", "G1", true},
            new object[] {"G4", "F4", true},
            new object[] {"G4", "G7", true},

            new object[] {"G4", "A1", false},
            new object[] {"G4", "A4", false},
            new object[] {"G4", "A7", false},
            new object[] {"G4", "B2", false},
            new object[] {"G4", "B4", false},
            new object[] {"G4", "B6", false},
            new object[] {"G4", "C3", false},
            new object[] {"G4", "C4", false},
            new object[] {"G4", "C5", false},
            new object[] {"G4", "D1", false},
            new object[] {"G4", "D2", false},
            new object[] {"G4", "D3", false},
            new object[] {"G4", "D5", false},
            new object[] {"G4", "D6", false},
            new object[] {"G4", "D7", false},
            new object[] {"G4", "E3", false},
            new object[] {"G4", "E4", false},
            new object[] {"G4", "E5", false},
            new object[] {"G4", "F2", false},
            new object[] {"G4", "F6", false},
            new object[] {"G4", "G4", false},


            new object[] {"G7", "G4", true},
            new object[] {"G7", "F6", true},
            new object[] {"G7", "D7", true},

            new object[] {"G7", "A1", false},
            new object[] {"G7", "A4", false},
            new object[] {"G7", "A7", false},
            new object[] {"G7", "B2", false},
            new object[] {"G7", "B4", false},
            new object[] {"G7", "B6", false},
            new object[] {"G7", "C3", false},
            new object[] {"G7", "C4", false},
            new object[] {"G7", "C5", false},
            new object[] {"G7", "D1", false},
            new object[] {"G7", "D2", false},
            new object[] {"G7", "D3", false},
            new object[] {"G7", "D5", false},
            new object[] {"G7", "D6", false},
            new object[] {"G7", "E3", false},
            new object[] {"G7", "E4", false},
            new object[] {"G7", "E5", false},
            new object[] {"G7", "F2", false},
            new object[] {"G7", "F4", false},
            new object[] {"G7", "G1", false},
            new object[] {"G7", "G7", false}
        };//THIS IS WHAT YOU WANTED 
          //YOU MADE US DO THIS!!!!!!! </3
        [Test]
        [TestCaseSource(nameof(ConnectedMoves))]
        public void ANormalCowCanOnlyMoveToAConnectedSpace(string pos, string neighbour, bool expected)//TODO
        {
            IRef referee = new MReferee();
            IBoard b = Substitute.For<Board>();
            b.getCellState(pos).Returns(Player.X);
            b.getCellState(neighbour).Returns(Player.None);
            IPlayer x = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            x.stones.Returns(12);
            Assert.That(referee.isValidPutDown(pos, neighbour, x, b) == expected);
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
            IRef referee = new MReferee();
            IBoard b = Substitute.For<IBoard>();
            IPlayer x = Substitute.For<IPlayer>();
            IPlayer o = Substitute.For<IPlayer>();
            x.playerID.Returns(Player.X);
            o.playerID.Returns(Player.O);
            x.stones.Returns(0);
            o.stones.Returns(0);
            b.numCows(x.playerID).Returns(2);
            b.numCows(o.playerID).Returns(3);
            Assert.That(referee.inPlacing(x, o) == false);
            Assert.That(referee.getGameState(x, o, b) == GameResult.Player2);
        }

        [Test]
        public void killmepls()
        {

        }
    }
}
