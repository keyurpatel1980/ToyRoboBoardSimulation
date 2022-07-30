using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobotSimulation
{
    public class DrawGrid
    {
        public static void Draw(int rows, int columns, int x, int y, string direction)
        {
            StringBuilder sb = new StringBuilder();
            Console.WriteLine();
            for (int c = columns - 1; c >= 0; c--)
            {
                for (int r = 0; r < rows; r++)
                {
                    if (r == x && c == y)
                        sb.Append($" *({r},{c}){direction.First()} ");
                    else
                        sb.Append($"  ({r},{c})  ");

                }
                Console.WriteLine(sb.ToString());
                Console.WriteLine();
                sb.Clear();
            }
        }
    }
}
