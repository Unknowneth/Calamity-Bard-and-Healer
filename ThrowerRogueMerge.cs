using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalamityBardHealer
{
	public class ThrowerRogueMerge : GlobalItem
	{
		public override bool IsLoadingEnabled(Mod mod) => !ModLoader.HasMod("RagnarokMod") && ModContent.GetInstance<BalanceConfig>().rogueThrowerMerge;
		public override bool AppliesToEntity(Item item, bool lateInstatiation) => (item.ModItem is ThoriumMod.Items.ThoriumItem t && t.isThrower) || item.ModItem is ThoriumMod.Items.ThrownItems.TechniqueItemBase || (item.ModItem?.Name.StartsWith("TideTurner") ?? false);
		public override void SetDefaults(Item item) {
			if(item.DamageType == DamageClass.Throwing) item.DamageType = ModContent.GetInstance<CalamityMod.RogueDamageClass>();
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if(item.accessory || item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0 || item.ModItem is ThoriumMod.Items.ThrownItems.TechniqueItemBase) foreach(TooltipLine m in tooltips) if(m.Text.Contains("Thrower")) m.Text = m.Text.Replace("Thrower", "Rogue");
			else if(m.Text.Contains("Throwing")) m.Text = m.Text.Replace("Throwing", "Rogue");
			else if(m.Text.Contains("thrower")) m.Text = m.Text.Replace("thrower", "rogue");
			else if(m.Text.Contains("thrown damage") || m.Text.Contains("thrown crit") || m.Text.Contains("thrown velocity")) m.Text = m.Text.Replace("thrown", "rogue");
			else if(m.Text.Contains("throwing speed") || m.Text.Contains("throwing velocity")) m.Text = m.Text.Replace("throwing", "rogue throwing");
			else if(m.Text.Contains("throwing")) m.Text = m.Text.Replace("throwing", "rogue");
		}
	}
}