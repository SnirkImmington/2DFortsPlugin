using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using TShockAPI;

namespace SnirkPlugin_Dynamic
{
    static class GameUtils
    {
        private static Item WaterCamdle = TShock.Utils.GetItemByName("Water Candle")[0];
        private static Item HealthCCrystal = TShock.Utils.GetItemByName("Heath Crystal")[0];
        private static Item ManaCrystal = TShock.Utils.GetItemByName("Mana Crystal")[0];

        public static bool IsNewChar(Player ply)
        {

        }

        public static bool IsCleanChar(Player ply, bool waterCandle = true, bool vanity = false)
        {
            foreach (var item in ply.inventory)
            {
                if (item == null || item.netID == 0) continue;
                if (waterCandle && item.netID == WaterCamdle.netID) continue;
                if (vanity && IsVainClothing(item)) continue;
            }
        }

        public static bool IsVainClothing(Item it)
        {
            return it.wornArmor && it.defense == 0;
        }

        public static int GetRepHealth(Player ply)
        {
            return GetRepStat(ply, ply.statLifeMax, HealthCCrystal.netID);
        }

        public static int GetRepMana(Player ply)
        {
            return GetRepStat(ply, ply.statLifeMax, ManaCrystal.netID);
        }

        private static int GetRepStat(Player ply, int start, int addID)
        {
            foreach (var item in ply.inventory)
                if (item.netID == addID)
                    start += 20;
            return start;
        }

        private static bool InventoryEqual(ICollection<Item> inv, ICollection<Item> player)
        {

        }

        public static CWClass GetPlayerClass(Player ply)
        {
            if (IsCleanChar(ply)) return null;

            // Get the items the player has from the inventory
            var items = ply.inventory.Where(it => it != null && it.netID != 0 && it.netID != WaterCamdle.netID);
            
            // First, check the health
            var health = GetRepHealth(ply);
            var mana = GetRepMana(ply);

            // Check each class
            foreach (var cwclass in CWConfig.Classes)
            {
                // If it's greater than, ignore, if less than just wait for player
                // to finish with the chest.
                if (cwclass.MaxHealth != health || cwclass.MaxMana != mana) continue;


            }
        }
    }
}
