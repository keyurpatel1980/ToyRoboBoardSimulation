using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobotSimulation
{
    class Program
    {
        static void Main(string[] args)
        {
            // can use DI to inject dependency
             new ToyRobotBoard(new CommandParser(),maxX:6,maxY:6,moveByUnit: 1).StartSimulation();
            Console.ReadKey();
        }

        private static void RunTests()
        {
            new Tests().Test_Command_Place_Success();
            new Tests().Test_Command_Place_Fail();
            new Tests().Test_Command_Move_Success();
            new Tests().Test_Command_LEFT_Success();
            new Tests().Test_Command_Place_Left_Move_Success();
            new Tests().Test_Command_Place_Left_Move_Fail();
            new Tests().Test_Command_Place_Left_Move_Report_Sucess();
        }
    }
}
