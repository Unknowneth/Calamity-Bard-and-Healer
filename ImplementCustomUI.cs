using Terraria.ModLoader;
using ThoriumMod.UI.ResourceBars;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace CalamityBardHealer
{
	[Autoload(true, Side = (ModSide)1)]
	public class ImplementCustomUI : ModSystem
	{
		public override void Load() {
			try {
				if(ModLoader.GetMod("ThoriumMod").Code.GetTypes().First(t => t.Name == "ThoriumInterfaceResources").GetField("Resources", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) is List<InterfaceResource> ir) {
					ir.Add(new UI.CosmicSong());
					ir.Add(new UI.DoomsdayCatharsis());
				}
			}
			catch {
			}
		}
	}
}