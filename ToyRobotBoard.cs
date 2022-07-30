using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobotSimulation
{
    public class ToyRobotBoard
    {
        private readonly int maxX;
        private readonly int maxY;
        private readonly int moveByUnit;
        private readonly ICommandParser commandParser;

        private RoboState State { get; set; }

        public ToyRobotBoard(ICommandParser commandParser, int maxX, int maxY, int moveByUnit)
        {
            this.maxX = maxX;
            this.maxY = maxY;
            this.moveByUnit = moveByUnit;
            State = new RoboState();
            this.commandParser = commandParser;
        }
        public void StartSimulation()
        {
            WelcomePrompt(); // show instruction on start
            while (true) // keep running simulation
            {
                string cmdText = PromptCommand(); // get prompt from user

                var command = commandParser.ParseCommand(cmdText.ToUpper(), maxX, maxY);
                while (command == null || !command.CommandTokensValid)
                {
                    cmdText = PromptCommand(); // invalid command so prompt again till valid command
                    command = commandParser.ParseCommand(cmdText.ToUpper(), maxX, maxY);
                }
                ExecuteCommand(command); // command is valid, execute command
            }
        }
        private void WelcomePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Please enter Command");
            Console.WriteLine("Here is the list of valid command");
            Console.WriteLine($"Please place toy robot on the board of size: {maxX} x {maxY}");
            Console.WriteLine("Please use zero-based index for coordinates");
            Console.WriteLine("PLACE X,Y,DIRECTION");
            Console.WriteLine("MOVE");
            Console.WriteLine("LEFT");
            Console.WriteLine("RIGHT");
            Console.WriteLine("REPORT");
            Console.WriteLine();
        }
        private string PromptCommand()
        {
            string cmdText = string.Empty;
            while (string.IsNullOrWhiteSpace(cmdText))
            {
                Console.WriteLine();
                Console.WriteLine("Please enter Command");
                Console.WriteLine();
                cmdText = Console.ReadLine();
            }
            return cmdText;
        }
        public void ExecuteCommand(Command command)
        {
            if (!command.CommandTokensValid)
                return;

            if (command.IsFirstTokenInvalid)
                return;
            // try to place it on board, have x & y coordinates & direction
            // change State of the board
            // A robot that is not on the table can choose to ignore the MOVE, LEFT, RIGHT and REPORT commands.

            if (command.CommandVerb == CommandVerb.PLACE)
                HandlePlace(command);
            else if (command.CommandVerb == CommandVerb.LEFT)
                HandleLeft(command);
            else if (command.CommandVerb == CommandVerb.RIGHT)
                HandleRight(command);
            else if (command.CommandVerb == CommandVerb.MOVE)
                HandleMove();
            else if (command.CommandVerb == CommandVerb.REPORT)
                HandleReport();
        }
        private void HandlePlace(Command command)
        {
            // we know that latest command has valid tokens
            // Change the state
            State.PlacedOnBoard = true;  // first token must be valid for all the commands

            if (!command.IsSecondTokenInvalid)
                State.XCord = command.XCord;

            if (!command.IsThirdTokenInvalid)
                State.YCord = command.YCord;

            if (!command.IsFourthTokenInvalid && command.Direction != Direction.NONE)
                State.CurrentDirection = command.Direction;
        }
        private void HandleLeft(Command command)
        {
            if (command.CommandVerb == CommandVerb.LEFT && State.PlacedOnBoard && State.HasDirection)
                State.CurrentDirection = FindDirection90DegreesLeft(); //LEFT will rotate the robot 90 degrees in the specified direction without changing the position of the robot.
        }
        private void HandleRight(Command command)
        {
            if (command.CommandVerb == CommandVerb.RIGHT && State.PlacedOnBoard && State.HasDirection)
                State.CurrentDirection = FindDirection90DegreesRight(); //RIGHT will rotate the robot 90 degrees in the specified direction without changing the position of the robot.
        }
        private void HandleReport()
        {
            // must be on the board
            // REPORT will announce the X,Y and orientation of the robot. 
            if (ValidateForMoveAndReport())
                return;
            Console.WriteLine();
            Console.WriteLine($"Robot state is {State.XCord.Value},{State.YCord.Value},{State.CurrentDirection.ToString()}");
            DrawGrid.Draw(maxX, maxY, State.XCord.Value, State.YCord.Value, State.CurrentDirection.ToString());
        }
        private void HandleMove()
        {
            if (ValidateForMoveAndReport())
                return;
            // (South,West) = (0,0) (South,East) = (5,0)
            // (North,West) = (0,5) (North,East) = (5,5)
            if (State.CurrentDirection == Direction.EAST)
            {
                Coordinate coordinate = new Coordinate { Y = State.YCord.Value };
                coordinate.X = State.XCord.Value + moveByUnit;
                HandleMoveCoordinate(coordinate);
            }
            else if (State.CurrentDirection == Direction.WEST)
            {
                Coordinate coordinate = new Coordinate { Y = State.YCord.Value };
                coordinate.X = State.XCord.Value - moveByUnit;
                HandleMoveCoordinate(coordinate);
            }
            else if (State.CurrentDirection == Direction.NORTH)
            {
                Coordinate coordinate = new Coordinate { X = State.XCord.Value };
                coordinate.Y = State.YCord.Value + moveByUnit;
                HandleMoveCoordinate(coordinate);
            }
            else if (State.CurrentDirection == Direction.SOUTH)
            {
                Coordinate coordinate = new Coordinate { X = State.XCord.Value };
                coordinate.Y = State.YCord.Value - moveByUnit;
                HandleMoveCoordinate(coordinate);
            }
        }
        private void HandleMoveCoordinate(Coordinate coordinate)
        {
            // (South,West) = (0,0) (South,East) = (5,0)
            // (North,West) = (0,5) (North,East) = (5,5)
            if (State.CurrentDirection == Direction.EAST || State.CurrentDirection == Direction.WEST)
            {
                if (coordinate.X < 0 || coordinate.X >= maxX)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Invalid Move to (x,y) => ({coordinate.X},{coordinate.Y})");
                }
                else
                {
                    State.XCord = coordinate.X;
                }
            }
            else if (State.CurrentDirection == Direction.NORTH || State.CurrentDirection == Direction.SOUTH)
            {
                if (coordinate.Y < 0 || coordinate.Y >= maxY)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Invalid Move to (x,y) => ({coordinate.X},{coordinate.Y})");
                }
                else
                {
                    State.YCord = coordinate.Y;
                }
            }
        }
        private bool ValidateForMoveAndReport()
        {
            bool invalidState = false;
            if (!State.PlacedOnBoard)
            {
                invalidState = true;
                Console.WriteLine();
                Console.WriteLine("Robot is not placed on the board.");
            }

            if (!State.HasDirection)
            {
                invalidState = true;
                Console.WriteLine();
                Console.WriteLine("Robot does not have orientation");
            }

            if (!State.XCord.HasValue)
            {
                invalidState = true;
                Console.WriteLine();
                Console.WriteLine("Robot does not have X coordinate");
            }
            if (!State.YCord.HasValue)
            {
                invalidState = true;
                Console.WriteLine();
                Console.WriteLine("Robot does not have Y coordinate");
            }


            return invalidState;
        }
        private Direction FindDirection90DegreesLeft()
        {
            // LEFT ~ Anti-Clockwise
            Direction newDirection = State.CurrentDirection;
            switch (State.CurrentDirection)
            {
                case Direction.NORTH:
                    newDirection = Direction.WEST;
                    break;
                case Direction.WEST:
                    newDirection = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    newDirection = Direction.EAST;
                    break;
                case Direction.EAST:
                    newDirection = Direction.NORTH;
                    break;
            }

            return newDirection;
        }
        private Direction FindDirection90DegreesRight()
        {
            // RIGHT ~ Clockwise
            Direction newDirection = State.CurrentDirection;
            switch (State.CurrentDirection)
            {
                case Direction.NORTH:
                    newDirection = Direction.EAST;
                    break;
                case Direction.EAST:
                    newDirection = Direction.SOUTH;
                    break;
                case Direction.SOUTH:
                    newDirection = Direction.WEST;
                    break;
                case Direction.WEST:
                    newDirection = Direction.NORTH;
                    break;
            }

            return newDirection;
        }

        public RoboState GetStateCopy()
        {
            // we don't want original state to be accessible outside of this class. Used for testing
            return new RoboState { PlacedOnBoard = State.PlacedOnBoard, CurrentDirection = State.CurrentDirection, XCord = State.XCord, YCord = State.YCord };
        }


    } // end of class
}
