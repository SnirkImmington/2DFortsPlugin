using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Different states of CW games.
    /// </summary>
    enum CWGameState
    {
        /// <summary>
        /// None: No CW game in session.
        /// </summary>
        None = 0,
        /// <summary>
        /// 1/4: Preparing teams
        /// <para>In this stage, players are lining up in the 
        /// top of the CW class chosing room, and are assigned tasks.</para>
        /// <para>This state ends when the player configuration is valid, and
        /// the host does /cwcontinue or similar command.</para>
        /// <para>Players should be in WaitingForPlayers stage.</para>
        /// </summary>
        PreparingTeams,
        /// <summary>
        /// 2/4: Getting Classses:
        /// <para>In this stage, players are acquiring items in the room
        /// and also moving to the arena of their choice.</para>
        /// <para>This state ends when all of the players chose their classes
        /// and are in the arena.</para>
        /// <para>Players should be in the GettingClasses and ReadyToPlay stages.</para>
        /// </summary>
        GettingClasses,
        /// <summary>
        /// 3/4: Playing
        /// <para>In this stage, players play the game.</para>
        /// <para>This stage ends when the game is won or drawn or stopped.</para>
        /// <para>Players should be in the Playing stage.</para>
        /// </summary>
        Playing,
        /// <summary>
        /// 3.25/4: Paused
        /// <para>If the CW game is paused, all the players are teleported/killed back to the spawn zones.
        /// The arenas should close back up so players can't get in.</para>
        /// <para>The stage ends with the host or mod using /cw resume.</para>
        /// <para>Players should remain in the playing stage.</para>
        /// </summary>
        Paused,
        /// <summary>
        /// 4/4: Cleaning Up
        /// <para>In this stage, players are awarded plorts for playing and are
        /// warped to cw. The game statistics are also saved.</para>
        /// <para>This stage ends when the arena is finished being cleared.</para>
        /// <para>Players should be in the CleangingUp stage.</para>
        /// </summary>
        CleaningUp
    }

    /// <summary>
    /// Different states of CW players.
    /// </summary>
    enum CWPlayerState
    {
        /// <summary>
        /// The player is not playing CW.
        /// </summary>
        None = 0,
        /// <summary>
        /// The player is waiting for the game to begin.
        /// </summary>
        WaitingForPlayers,
        /// <summary>
        /// The player is picking their class.
        /// </summary>
        GettingClasses,
        /// <summary>
        /// The player has picked their class.
        /// </summary>
        ReadyToPlay,
        /// <summary>
        /// The player is playing in the CW game.
        /// </summary>
        Playing,
        /// <summary>
        /// The player is cleaning up the game.
        /// </summary>
        CleaningUp
    }
}
