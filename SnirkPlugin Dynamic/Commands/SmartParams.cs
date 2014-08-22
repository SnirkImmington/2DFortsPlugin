using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using Terraria;

namespace SnirkPlugin_Dynamic
{
    static class SmartParams
    {
        public static SmartInfo<TSPlayer> TSPlayer(CommandArgs com, int startIndex = 0)
        {
            return MatchList(com, TShock.Utils.FindPlayer, startIndex);
        }
        public static SmartInfo<NPC> NPC(CommandArgs com, int startIndex = 0)
        {
            return MatchList(com, TShock.Utils.GetNPCByIdOrName, startIndex);
        }
        public static SmartInfo<Item> Item(CommandArgs com, int startIndex = 0)
        {
            return MatchList(com, TShock.Utils.GetItemByIdOrName, startIndex);
        }
        public static SmartInfo<int> Buff(CommandArgs com, int startIndex = 0)
        {
            return MatchList(com, TShock.Utils.GetBuffByName, startIndex);
        }
        public static SmartInfo<T> MatchList<T>(CommandArgs com, Func<string, List<T>> finder, int startIndex = 0)
        {
            return null;
        }
    }

    class SmartInfo<T>
    {
        public T Value;

        public int Matched;

        public int EndIndex;

        public SmartInfo(T value, int index)
        {
            Value = value; EndIndex = index;
            Matched = 1;
        }

        public SmartInfo(int matched)
        {
            Matched = matched;
        }
    }
}
