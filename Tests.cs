using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobotSimulation
{
    public class Tests
    {
        private readonly int maxX = 6;
        private readonly int maxY = 6;
        private readonly int moveByUnit = 1;
        private CommandParser commandParser = new CommandParser();
        private ToyRobotBoard board;
        public Tests()
        {
            commandParser = new CommandParser();
            board = new ToyRobotBoard(commandParser, maxX, maxY, moveByUnit);
        }
        public void Test_Command_Place_Success()
        {
            // Arrange
            string cmd = "PLACE 1,2,EAST";
            var command = commandParser.ParseCommand(cmd.ToUpper(), maxX, maxY);

            // Act
            board.ExecuteCommand(command);

            // Assert
            // state should be
            var state = board.GetStateCopy();
            if (state.PlacedOnBoard) // Robot is expected to be placed on board
                Console.WriteLine("Test Passed: Robot is placed on the board");

            if (state.XCord.HasValue && state.XCord.Value == 1) // Robot X Coordinate is expected to be 1
                Console.WriteLine($"Test Passed: Robot X Coordinate is {state.XCord.Value}");

            if (state.YCord.HasValue && state.YCord.Value == 2) // Robot Y Coordinate is expected to be 2
                Console.WriteLine($"Test Passed: Robot Y Coordinate is {state.YCord.Value}");

            if (state.CurrentDirection == Direction.EAST) // Robot current direction is expected to be EAST
                Console.WriteLine($"Test Passed: Robot current direction is {state.CurrentDirection}");
        }
        public void Test_Command_Place_Fail()
        {
            // Arrange
            string cmd = "PLACEx 1,2,EAST";
            var command = commandParser.ParseCommand(cmd.ToUpper(), maxX, maxY);

            // Act
            board.ExecuteCommand(command);

            // Assert
            // state should be
            var state = board.GetStateCopy();
            if (!state.PlacedOnBoard) // Robot is expected to be NOT placed on board
                Console.WriteLine("Test Passed: Robot is NOT placed on the board");

            if (!state.XCord.HasValue) // Robot X Coordinate is expected to have invalid value
                Console.WriteLine($"Test Passed: Robot X Coordinate is NOT present");

            if (!state.YCord.HasValue) // Robot Y Coordinate is expected to to have invalid value
                Console.WriteLine($"Test Passed: Robot Y Coordinate is NOT present");

            if (state.CurrentDirection == Direction.NONE) // Robot current direction is expected to be NONE
                Console.WriteLine($"Test Passed: Robot current direction is {state.CurrentDirection}");
        }
        public void Test_Command_Move_Success()
        {
            // Arrange
            string cmdPlace = "PLACE 1,2,EAST";
            string cmdMove = "MOVE";
            var commandPlace = commandParser.ParseCommand(cmdPlace.ToUpper(), maxX, maxY);
            var commandMove = commandParser.ParseCommand(cmdMove.ToUpper(), maxX, maxY);
            // Act
            board.ExecuteCommand(commandPlace);
            board.ExecuteCommand(commandMove);


            // Assert
            // state should be
            var state = board.GetStateCopy();
            if (state.PlacedOnBoard) // Robot is expected to be placed on board
                Console.WriteLine("Test Passed: Robot is placed on the board");

            if (state.XCord.HasValue && state.XCord.Value == 2) // Robot X Coordinate is expected to be 2
                Console.WriteLine($"Test Passed: Robot X Coordinate is {state.XCord.Value}");

            if (state.YCord.HasValue && state.YCord.Value == 2) // Robot Y Coordinate is expected to be 2
                Console.WriteLine($"Test Passed: Robot Y Coordinate is {state.YCord.Value}");

            if (state.CurrentDirection == Direction.EAST) // Robot current direction is expected to be EAST
                Console.WriteLine($"Test Passed: Robot current direction is {state.CurrentDirection}");
        }
        public void Test_Command_LEFT_Success()
        {
            // Arrange
            string cmdPlace = "PLACE 1,2,EAST";
            string cmdLeft = "LEFT";
            var commandPlace = commandParser.ParseCommand(cmdPlace.ToUpper(), maxX, maxY);
            var commandMove = commandParser.ParseCommand(cmdLeft.ToUpper(), maxX, maxY);
            // Act
            board.ExecuteCommand(commandPlace);
            board.ExecuteCommand(commandMove);


            // Assert
            // state should be
            var state = board.GetStateCopy();
            if (state.PlacedOnBoard) // Robot is expected to be placed on board
                Console.WriteLine("Test Passed: Robot is placed on the board");

            if (state.XCord.HasValue && state.XCord.Value == 1) // Robot X Coordinate is expected to be 1
                Console.WriteLine($"Test Passed: Robot X Coordinate is {state.XCord.Value}");

            if (state.YCord.HasValue && state.YCord.Value == 2) // Robot Y Coordinate is expected to be 2
                Console.WriteLine($"Test Passed: Robot Y Coordinate is {state.YCord.Value}");

            if (state.CurrentDirection == Direction.NORTH) // Robot current direction is expected to be NORTH
                Console.WriteLine($"Test Passed: Robot current direction is {state.CurrentDirection}");
        }
        public void Test_Command_Place_Left_Move_Success()
        {
            // Arrange
            string cmdPlace = "PLACE 1,2,EAST";
            string cmdLeft = "LEFT";
            string cmdMove = "MOVE";
            var commandPlace = commandParser.ParseCommand(cmdPlace.ToUpper(), maxX, maxY);
            var commandLeft = commandParser.ParseCommand(cmdLeft.ToUpper(), maxX, maxY);
            var commandMove = commandParser.ParseCommand(cmdMove.ToUpper(), maxX, maxY);
            // Act
            board.ExecuteCommand(commandPlace);
            board.ExecuteCommand(commandLeft);
            board.ExecuteCommand(commandMove);

            // Assert
            // state should be
            var state = board.GetStateCopy();
            if (state.PlacedOnBoard) // Robot is expected to be placed on board
                Console.WriteLine("Test Passed: Robot is placed on the board");

            if (state.XCord.HasValue && state.XCord.Value == 1) // Robot X Coordinate is expected to be 1
                Console.WriteLine($"Test Passed: Robot X Coordinate is {state.XCord.Value}");

            if (state.YCord.HasValue && state.YCord.Value == 3) // Robot Y Coordinate is expected to be 3
                Console.WriteLine($"Test Passed: Robot Y Coordinate is {state.YCord.Value}");

            if (state.CurrentDirection == Direction.NORTH) // Robot current direction is expected to be NORTH
                Console.WriteLine($"Test Passed: Robot current direction is {state.CurrentDirection}");
        }
        public void Test_Command_Place_Left_Move_Fail()
        {
            // Arrange
            string cmdPlace = "PLACE 1,5,EAST";
            string cmdLeft = "LEFT";
            string cmdMove = "MOVE";
            var commandPlace = commandParser.ParseCommand(cmdPlace.ToUpper(), maxX, maxY);
            var commandLeft = commandParser.ParseCommand(cmdLeft.ToUpper(), maxX, maxY);
            var commandMove = commandParser.ParseCommand(cmdMove.ToUpper(), maxX, maxY);
            // Act
            board.ExecuteCommand(commandPlace);
            board.ExecuteCommand(commandLeft);
            board.ExecuteCommand(commandMove);

            // Assert
            // state should be
            var state = board.GetStateCopy();
            if (state.PlacedOnBoard) // Robot is expected to be placed on board
                Console.WriteLine("Test Passed: Robot is placed on the board");

            if (state.XCord.HasValue && state.XCord.Value == 1) // Robot X Coordinate is expected to be 1
                Console.WriteLine($"Test Passed: Robot X Coordinate is {state.XCord.Value}");

            if (state.YCord.HasValue && state.YCord.Value == 5) // Robot Y Coordinate is expected to be 5. We will NOT move
                Console.WriteLine($"Test Passed: Robot Y Coordinate is {state.YCord.Value}");

            if (state.CurrentDirection == Direction.NORTH) // Robot current direction is expected to be NORTH
                Console.WriteLine($"Test Passed: Robot current direction is {state.CurrentDirection}");
        }
        public void Test_Command_Place_Left_Move_Report_Sucess()
        {
            // Arrange
            string cmdPlace = "PLACE 1,4,EAST";
            string cmdLeft = "LEFT";
            string cmdMove = "MOVE";
            string cmdReport = "REPORT";

            var commandPlace = commandParser.ParseCommand(cmdPlace.ToUpper(), maxX, maxY);
            var commandLeft = commandParser.ParseCommand(cmdLeft.ToUpper(), maxX, maxY);
            var commandMove = commandParser.ParseCommand(cmdMove.ToUpper(), maxX, maxY);
            var commandReport = commandParser.ParseCommand(cmdReport.ToUpper(), maxX, maxY);
            // Act
            board.ExecuteCommand(commandPlace);
            board.ExecuteCommand(commandLeft);
            board.ExecuteCommand(commandMove);
            board.ExecuteCommand(commandReport);

            // Assert
            // state should be
            var state = board.GetStateCopy();
            if (state.PlacedOnBoard) // Robot is expected to be placed on board
                Console.WriteLine("Test Passed: Robot is placed on the board");

            if (state.XCord.HasValue && state.XCord.Value == 1) // Robot X Coordinate is expected to be 1
                Console.WriteLine($"Test Passed: Robot X Coordinate is {state.XCord.Value}");

            if (state.YCord.HasValue && state.YCord.Value == 5) // Robot Y Coordinate is expected to be 5.
                Console.WriteLine($"Test Passed: Robot Y Coordinate is {state.YCord.Value}");

            if (state.CurrentDirection == Direction.NORTH) // Robot current direction is expected to be NORTH
                Console.WriteLine($"Test Passed: Robot current direction is {state.CurrentDirection}");
        }


    } // end of class
}
