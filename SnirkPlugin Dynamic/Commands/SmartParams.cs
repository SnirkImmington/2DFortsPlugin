using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    static class SmartParams
    {
        public static SmartInfo<PlayerData> Player(CommandArgs com, int startIndex = 0)
        {

        }
    }

    class SmartInfo<T>
    {
        public T Value;

        public int Index;

        public SmartInfo(T value, int index)
        {
            Value = value; Index = index;
        }
    }
}
