using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    class CWCommentator
    {
        static class Strings
        {
            static const string[] DeathBySpike = new string[]
            {
                "{0} is killed by the spikes!",
                "{0} impales on the spikes!",
            };
            
            static const string[] DeathByLava = new string[]
            {

            };

            static const string[] DeathByArena = new string[]
            {
                "{0} is killed by the arena",
                "{0} misses a jump there",
            };

            static const string[] StartKillStreak = new string[]
            {

            };

            static const string[] PushedAhead = new string[]
            {
                "pushed ahead",
                "is in the lead",
                "is now ahead",
            };


    }
}
