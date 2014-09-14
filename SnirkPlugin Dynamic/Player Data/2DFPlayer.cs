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

        /// <summary>
        /// A list of permabuffs for the player.
        /// </summary>
        public List<int> Permabuffs;

        /// <summary>
        /// The type of thing searched.
        /// </summary>
        public string SearchType;

        /// <summary>
        /// The results of the search.
        /// </summary>
        public string[] SearchResults;

        #endregion

        #region Utility

        /// <summary>
        /// Determines if this PlayerData belongs to a 2DForts staff member - can also check if Modmin is null
        /// </summary>
        /// <param name="onlyAdmin">Whether to disclude moderators from the query.</param>
        public bool IsStaff(bool onlyAdmin = false)
        {
            if (onlyAdmin) return TSPlayer.Group.HasPermission(ComUtils.AdminPermission);
            return TSPlayer.Group.HasPermission(ComUtils.ModPermission) || TSPlayer.Group.HasPermission(ComUtils.AdminPermission);
        }

        /// <summary>
        /// Determines whether this player is a donor.
        /// </summary>
        /// <returns></returns>
        public bool IsDonor
        {
            get { return TSPlayer.Group.Name.EndsWith("Donor"); }
        }

        #endregion

        #region Class Warfare
        
        /// <summary>
        /// Player specific Class Warfare data.
        /// </summary>
        public CWData CW;

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

        #endregion

        #region Log in/out

        /// <summary>
        /// Logs in the player data - reads
        /// persistant info from the database.
        /// </summary>
        public void LogIn()
        {
            // If player is mod, get the mod data or create a new one

            // Get the possible player data or create a new one
        }

        /// <summary>
        /// Logs out the player data - saves things.
        /// </summary>
        public void LogOut()
        {
            // If player is mod, save the mod data
            if (Modmin != null && Modmin.ShouldSave())
            {

            }

            // If player data or user data should be saved, save them
            if (UserData.ShouldSave())
            {

            }
        }

        #endregion

        #region Util Methods

        /// <summary>
        /// Renames the player.
        /// </summary>
        /// <param name="newName">The new name to give</param>
        /// <param name="source">The name of the renamer for logs.</param>
        public void Rename(string newName, string source)
        {
            // Just change the name
            if (IsRenamed)
            {
                OldName = Player.name;
                Logs.ConsoleData("Rename - {0} was renamed to {1} by {2}.", Player.name, newName, source);
                Player.name = newName;
            }
            else 
            {
                Logs.ConsoleData("Rename - {0} (originally {1}) was renamed to {2} by {3}", Player.name, OldName, newName, source);
                Player.name = newName;
            }
        }

        /// <summary>
        /// Reverts the player's name.
        /// </summary>
        public void RevertName()
        {
            Player.name = OldName;
        }

        /// <summary>
        /// If the specific type of message should be sent.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool AcceptsLog(LogType type)
        {
            switch (type)
            {
                case LogType.General: return TSPlayer.DisplayLogs ? Modmin.PluginInfo : false;
                case LogType.Trace: return Modmin.PluginTracing;
                case LogType.Important: return true;
                default: return true;
            }

        }

        /// <summary>
        /// Teleports the player to the CW position they should be at.
        /// </summary>
        public void TeleportCW()
        {
            // If no team
            if (CW == null || CW.Game == null || CW.Game.Arena == null)
            {
                TSPlayer.Teleport(CWConfig.CWStartCoords.X, CWConfig.CWStartCoords.Y);
                TSPlayer.SendSuccessMessage("You have been teleported to the starting position for CW!"); return;
            }
            // else is observing
            if (CW.Observing != null)
            {
                var dest = CW.Observing.Arena.SpawnObserver;
                TSPlayer.Teleport(dest.X, dest.Y);
                TSPlayer.SendSuccessMessage("You have been teleported to the observation platform of your watched CW arena!"); return;
            }
        }

        #endregion

        /// <summary>
        /// Updates the player every second.
        /// </summary>
        public void OnSecond()
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlayerData(int index, bool startupAdd)
        {
            // Save the index.
            Index = index;

            // Set the rename data.
            OldName = Player.name;
        }
    }
}
