﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TShockAPI;
using TShockAPI.DB;

namespace SnirkPlugin_Dynamic
{
    [CommandsClass]
    class GeneralCommands
    {
        [BaseCommand(Permissions.canchat, "Kills you with \"Player facepalmed with the force of a supernova.\"",
            "facepalm", "fp", "facedesk", AllowServer = false, DoLog = false)]
        public static void Facepalm(CommandArgs com)
        {
            com.Player.SendSuccessMessage(Extensions.GetRandom(ComUtils.FacepalmUserMessages));
            com.Player.Damage(Extensions.GetRandom(ComUtils.FacepalmMessages)
                .SFormat(ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Self),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.They),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Their),
                        ComUtils.Genderize(com.Player.TPlayer.male, GenderMode.Them)), 9001);
        }

        [BaseCommand(Permissions.canchat, "Finds things nearby.", "nearby", "local", "near", "find")]
        public static void Nearby(CommandArgs com)
        {
            var parser = new CommandParser(com,
                "Usage: /nearby warp{0} - find things nearby".SFormat(com.Player.IsStaff() ? "|region|player|modmin|warpplate|point" : ""));

            var param = parser.PopParameter(); if (param == "") return;
            if (param[param.Length - 2] == 's') param = param.Substring(0, param.Length - 2);

            int distance = 50;
            if (com.Parameters.Count > 1)
            {
                var parseDistance = parser.Parse(false, "Invalid distance! " + parser.Usage, int.Parse); 
                if (parseDistance == null) return;
                if (parseDistance.Value < 0) com.Player.SendErrorMessage("You can't have negative distance, dude.");
                distance = parseDistance.Value;
            }
            int page = 1;
            if (com.Parameters.Count > 2)
            {
                var parsePage = parser.Parse(false, "Invalid page number! " + parser.Usage, int.Parse);
                if (parsePage == null) return;
                if (parsePage.Value < 0) com.Player.SendErrorMessage("You can't have negative page numbers, dude.");
                page = parsePage.Value;
            }

            switch (param.ToLower())
            {
                case "warp":
                case "wp":
                    // Such Linq much fancy very .NET so SQL
                    var warps = from warp in TShock.Warps.Warps
                                where !warp.IsPrivate 
                                && Math.Abs(com.Player.X - warp.Position.X) <= distance 
                                && Math.Abs(com.Player.Y - warp.Position.Y) <= distance
                                select warp.Name; // much ToList() get da count

                    PaginationTools.SendPage(com.Player, page, PaginationTools.BuildLinesFromTerms(warps), 
                        new PaginationTools.Settings()
                        {
                            FooterFormat = "Type /nearby warp {0} for more warps.",
                            HeaderFormat = "Local warps (page {0} of {1}):",
                            NothingToDisplayString = "No players found!"
                        });
                    return;

                case "ply":
                case "player":
                    var players = from player in DynamicMain.Players
                                  where player.Index != com.Player.Index // don't match yerself
                                  && !(player.IsStaff() ? player.Modmin.Indetectable : false)
                                  && Math.Abs(com.Player.X - player.TSPlayer.X) <= distance
                                  && Math.Abs(com.Player.Y - player.TSPlayer.Y) <= distance
                                  select player.TSPlayer.Name;

                    PaginationTools.SendPage(com.Player, page, PaginationTools.BuildLinesFromTerms(players),
                        new PaginationTools.Settings()
                        {
                            HeaderFormat = "Players nearby ({0}/{1}):",
                            FooterFormat = "Type /nearby player {0} for more.",
                            NothingToDisplayString = "There are currently no players nearby."
                        });
                    return;

                case "modmin":
                case "mod":
                case "staff":
                    var staff = from player in DynamicMain.Players
                                where player.IsStaff() && player.Index != com.Player.Index
                                select player.TSPlayer.Name;

                    PaginationTools.SendPage(com.Player, page, PaginationTools.BuildLinesFromTerms(staff),
                        new PaginationTools.Settings()
                        {
                            HeaderFormat = "Staff nearby ({0}/{1}):",
                            FooterFormat = "Type /nearby staff {0} for moar. I never thought you'd need more space but there you go.",
                            NothingToDisplayString = "There are currently no staff nearby."
                        });
                    return;

                case "region":
                case "reg":
                    var rectangle = new Rectangle(Math.Abs(com.Player.TileX - (distance/2)), 
                        Math.Abs(com.Player.TileY - (distance/2)), distance, distance);
                    var regions = from region in TShock.Regions.Regions
                                  where region.WorldID == Terraria.Main.worldID.ToString() 
                                  && region.InArea(rectangle) select region.Name;

                    PaginationTools.SendPage(com.Player, page, PaginationTools.BuildLinesFromTerms(regions),
                        new PaginationTools.Settings()
                        {
                            HeaderFormat = "Regions nearby ({0}/{1}):",
                            FooterFormat = "Type /nearby region {0} for more.",
                            NothingToDisplayString = "There are currently no regions nearby."
                        });
                    return;

                case "warpplate":
                case "wplate":
                    return;

                case "point":
                case "userpoint":
                    var points = from point in com.Player.GetData().Modmin.Points
                                 where Math.Abs(com.Player.X - point.X) <= distance
                                 && Math.Abs(com.Player.Y - point.Y) <= distance
                                 select point.Name;

                    PaginationTools.SendPage(com.Player, page, PaginationTools.BuildLinesFromTerms(points),
                        new PaginationTools.Settings()
                        {
                            HeaderFormat = "Your points nearby ({0}/{1}):",
                            FooterFormat = "Type /nearby point {0} for more.",
                            NothingToDisplayString = "You don't have any defined points nearby."
                        });
                    return;
            }
        }

        #region Donors only

        public static void PreventRename(CommandArgs com)
        {

        }

        #endregion
    }
}
