using System;
using NUnit.Framework;
using System.Linq;

namespace Morabaraba_9001.Test
{
	[TestFixture]
    [Test]
    public void AtStartBoardIsEmpty()
    {
        Board b = new Board();
        bool isEmpty = true;
        foreach (ICell cell in b.board.Values)
        {
            if (cell.CellState != Empty)
            {
                isEmpty = false;
            }
        }
        Assert.That(isEmpty);
    }
    [Test]
    public void PlayerXStartsFirst()//Player X is our equivalent for the dark cows player
    {

    }
    [Test]
    public void CowsCanOnlyBePlayedOnEmptySpaces()
    {

    }
    [Test]
    public void AMaximumOf12PlacementsPerPlayerAreAllowed()
    {

    }
    [Test]
    public void CowsCannotBeMovedDuringPlacement()
    {

    }
    public class Tests
    {
		//tests
    }
}
