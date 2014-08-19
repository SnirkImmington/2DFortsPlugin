using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerrariaApi.Server;
using Terraria;

namespace SnirkPlugin_Dynamic
{
    /// <summary>
    /// Code for the small TerrariaPlugin object used by the dynamic plugin for things
    /// </summary>
    [ApiVersion(1, 16)]
    class DynamicPlugin : TerrariaPlugin
    {
        public override string Name { get { return "SnirkPlugin Dynamic"; } }
        public override string Author { get { return "Snirk Immington"; } }
        public override string Description { get { return "Liveupdateable, Open Source module for Snirk's 2DForts plugin"; } }

        public override void Initialize() { }
        protected override void Dispose(bool disposing) { base.Dispose(disposing); }
        public DynamicPlugin(Main terrMain) : base(terrMain) { }
    }
}
