using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToyRobotSimulation
{
    public static class EnumHelpers<T>
    {
        public static IEnumerable<T> GetValues()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
        public static bool IsPresent(string name)
        {
            return Enum.IsDefined(typeof(T), name);
        }

        public static bool IsPresent(T value)
        {
            return Enum.IsDefined(typeof(T), value);
        }

    }
}
