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
            static const string[] MinerDown = new string[]
            {
                // {0} = { name | <team> miner }, {1} = he|she, {2} = other team, {3} = miner team
                "{0} down!",
                "{0} is down!",
                "{0} has been taken down!",
                "{0} struck down!",
                "{0} hit, {1}'s down!",
            };
            static const string[] MinerKilled = new string[]
            {
                // {0} = { name | <team> miner }, {1} = enemy team, {2} = killer, {3} = miner team
                "{0} killed by {2}!",
                "{0} is down! Killed by {2}!",
                "{0} has been stopped by {2}!",

                "{2} kills {1}!",
            };
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

        static class Chatter
        {
            static const string[] PullingAhead = new string[]
            {
                // {0} = other name {1} = team {2} = 
                "It looks like the {1} team is pulling ahead!",
                "{0} seems to be taking a lead right now, "
            };

            static const string[] None = new string[] 
            { 
            };
        }

        static class Speech
        {
            
        }
    }

    class ExpresionTree
    {

    }

    class WeightedExpression
    {

    }

    class WeightedExpressionBranch
    {

    }
}
