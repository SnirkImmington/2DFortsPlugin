using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TShockAPI;
using TShockAPI.DB;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Interface for commands that must be invoked on a target spot.
    /// </summary>
    interface ITarget
    {
        /// <summary>
        /// Gets the X coord of the target.
        /// </summary>
        float GetX();
        /// <summary>
        /// Gets the Y coord of the target.
        /// </summary>
        float GetY();

        /// <summary>
        /// Gets some information about the target.
        /// </summary>
        string GetInfo();
    }

    /// <summary>
    /// ITarget class for players.
    /// </summary>
    class PlayerTarget : ITarget
    {
        private TSPlayer Target;

        public int GetX() { return Target.TileX * 16; }
        public int GetY() { return Target.TileY * 16; }

        public string GetInfo()
        {
            return "Player {0} at {1}, {2}".SFormat(Target.Name, GetX(), GetY());
        }
    
        public PlayerTarget(TSPlayer ply)
        {
            Target = ply;
        }
    }

    /// <summary>
    /// ITarget implementation for warps.
    /// </summary>
    class WarpTarget : ITarget
    {
        private Warp Warp;

        // TODO check TShock /warp command.
        public float GetX() { return Warp.Position.X * 16; }
        public float GetY() { return Warp.Position.Y * 16; }

        public string GetInfo()
        {
            return "Warp {0} (at {1}, {2})".SFormat(Warp.Name, Warp.Position.X, Warp.Position.Y);
        }
    
        public WarpTarget(Warp warp) { Warp = warp; }
    }

    /// <summary>
    /// Contains targeting information for point-based targets.
    /// </summary>
    class PointTarget : ITarget
    {
        private Vector2 Point { get; set; }
        public float GetX() { return Point.X; }
        public float GetY() { return Point.Y; }

        public PointTarget(Vector2 point) { Point = point; }

        public string GetInfo()
        { return "position {0}".SFormat(Point.ToGPSPoint().ToGPSForm()); }
    }

    /// <summary>
    /// ITarget information for a region center.
    /// </summary>
    public class RegionTarget : ITarget
    {
        private Region region;
        public float GetX() { return region.Area.Center.X * 16; }
        public float GetY() { return region.Area.Center.Y * 16; }

        public RegionTarget(Region reg) { region = reg; }

        public string GetInfo() { return "center of region {0} (at {1}, {2})".SFormat(region.Name, GetX(), GetY()); }
    }

    public class WarpPlateTarget : ITarget
    {

    }

    public class UserPointTarget : ITarget
    {

    }
}
