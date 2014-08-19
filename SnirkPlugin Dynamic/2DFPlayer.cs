using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using Terraria;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Contains plugin-specific data for players.
    /// </summary>
    class PlayerData
    {
        #region Basic Indexing

        /// <summary>
        /// The index of the player in the Terraria player array.
        /// </summary>
        public int Index { get; private set; }
        /// <summary>
        /// The TSPlayer data for the player.
        /// </summary>
        public TSPlayer TSPlayer { get { return TShock.Players[Index]; } }
        /// <summary>
        /// The Terraria Player data for the player.
        /// </summary>
        public Player Player { get { return Main.player[Index]; } }

        #endregion

        #region General

        /// <summary>
        /// The spawn point of the player.
        /// </summary>
        public Point SpawnPoint;

        /// <summary>
        /// The old name of the player.
        /// </summary>
        public string OldName { get; private set; }

        /// <summary>
        /// Whether the player is renamed.
        /// </summary>
        public bool IsRenamed { get { return Player.name == OldName; } }

        /// <summary>
        /// Whether the player is pig-latined
        /// </summary>
        public bool IsPigLatined;

        #endregion

        #region Utility

        /// <summary>
        /// Determines if this PlayerData belongs to a 2DForts staff member - can also check if Modmin is null
        /// </summary>
        /// <param name="onlyAdmin">Whether to disclude moderators from the query.</param>
        public bool IsStaff(bool onlyAdmin = false)
        {
            if (onlyAdmin) return TSPlayer.Group.HasPermission("2dforts.admin");
            return TSPlayer.Group.HasPermission("2dforts.mod") || TSPlayer.Group.HasPermission("2dforts.admin");
        }

        #endregion

        #region Persistance

        /// <summary>
        /// Specific data for modmins only.
        /// </summary>
        public ModminData Modmin;
        /// <summary>
        /// User account tracked data.
        /// </summary>
        public UserPersistantData UserData;
        /// <summary>
        /// Player GUID tracked data.
        /// </summary>
        public PlayerPersistantData PlayerData;

        #endregion

        public void LogIn()
        {
            // If player is mod, get the mod data or create a new one

            // Get the possible player data or create a new one
        }

        public void LogOut()
        {
            // If player is mod, save the mod data

            // If player data or user data should be saved, save them
        }

        public void Rename(string newName, string source)
        {
            // Just change the name
            if (IsRenamed)
            {

            }
        }

        public PlayerData(int index, bool startupAdd)
        {
            Index = index;

            // Get player-persistant data.
        }
    }
}
