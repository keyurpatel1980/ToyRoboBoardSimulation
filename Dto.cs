using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobotSimulation
{
    public class Command
    {
        public CommandVerb CommandVerb { get; set; }
        public int XCord { get; set; }
        public int YCord { get; set; }

        public Direction Direction { get; set; }

        public string CommandText { get; set; }

        public bool CommandTokensValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CommandText)) // command text must NOT be empty
                    return false;
                else if (IsFirstTokenInvalid)
                    return false;
                else if (IsSecondTokenInvalid)
                    return false;
                else if (IsThirdTokenInvalid)
                    return false;
                else if (IsFourthTokenInvalid)
                    return false;
                else
                    return true;
            }
        }
        public bool IsFirstTokenInvalid { get; set; }
        public bool IsSecondTokenInvalid { get; set; }
        public bool IsThirdTokenInvalid { get; set; }
        public bool IsFourthTokenInvalid { get; set; }

        public string FirstTokenError { get; set; }
        public string SecondTokenError { get; set; }
        public string ThirdTokenError { get; set; }
        public string FourthTokenError { get; set; }
    }

    public class CommandToken
    {
        public int? TokenIndex { get; set; }
        public string Token { get; set; }
    }

    public enum CommandVerb
    {
        NONE,
        PLACE,
        MOVE, // One unit current direction
        LEFT, // Anti-Clockwise
        RIGHT, // Clock-Wise
        REPORT
    }
    public enum Direction
    {
        NONE,
        NORTH,
        SOUTH,
        EAST,
        WEST
    }

    public class Coordinate
    {
        public int X { get; set; }
        public int Y { get; set; }

    }

    public class RoboState
    {
        public RoboState()
        {
            CurrentDirection = Direction.NONE;
        }
        public bool PlacedOnBoard { get; set; } // must by placed on board

        // must have direction for MOVE, LEFT, RIGHT, REPORT verbs
        public bool HasDirection
        {
            get
            {
                if (CurrentDirection == Direction.NONE)
                    return false;
                else
                    return true;
            }
        }
        public int? XCord { get; set; } // must have valid value
        public int? YCord { get; set; } // must have valid value

        public Direction CurrentDirection { get; set; }
        /*
        public void SetPlacedOnBoard(bool onBoard)
        {
            PlacedOnBoard = onBoard;
        }

        public void SetXCord(int? xCord  )
        {
            XCord = xCord;
        }

        public void SetYCord(int? yCord)
        {
            YCord = yCord;
        }

        public void SetCurrentDirection(Direction currentDirection)
        {
            CurrentDirection = currentDirection;
        }
        */
    }
}
