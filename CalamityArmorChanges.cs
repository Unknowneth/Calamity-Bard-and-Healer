using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using ThoriumMod;
using System.Collections.Generic;

namespace CalamityBardHealer
{
	public class CalamityArmorChanges : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstatiation) => (item.ModItem?.Mod.Name == "CalamityMod" && (item.ModItem?.Name == "AstralBreastplate" || item.ModItem?.Name == "AstralHelm" || item.ModItem?.Name == "DemonshadeGreaves" || item.ModItem?.Name == "DemonshadeHelm" || item.ModItem?.Name == "WulfrumJacket")) || (item.ModItem?.Mod.Name == "CalamityEntropy" && (item.ModItem?.Name == "VoidFaquirBodyArmor" || item.ModItem?.Name == "VoidFaquirCuises"));
		public override void UpdateEquip(Item item, Player player) {
			if(item.ModItem?.Name == "AstralBreastplate") {
				player.GetModPlayer<ThoriumPlayer>().bardResourceMax2 += 6;
				player.GetModPlayer<ThoriumPlayer>().healBonus += 4;
			}
			else if(item.ModItem?.Name == "AstralHelm") player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.12f;
			else if(item.ModItem?.Name == "DemonshadeGreaves") {
				player.GetModPlayer<ThoriumPlayer>().bardResourceMax2 += 15;
				player.GetModPlayer<ThoriumPlayer>().healBonus += 25;
				player.GetModPlayer<ThoriumPlayer>().bardRangeBoost += 500;
				player.GetModPlayer<ThoriumPlayer>().bardBuffDuration += 360;
				player.GetModPlayer<ThoriumPlayer>().bardHomingSpeedBonus += 0.6f;
				player.GetModPlayer<ThoriumPlayer>().bardHomingRangeBonus += 0.6f;
				player.GetModPlayer<ThoriumPlayer>().bardBounceBonus += 6;
			}
			else if(item.ModItem?.Name == "DemonshadeHelm") {
				player.GetDamage(HealerDamage.Instance) += 0.3f;
				player.GetDamage(BardDamage.Instance) += 0.3f;
				player.GetAttackSpeed(HealerTool.Instance) += 0.35f;
				player.GetAttackSpeed(HealerDamage.Instance) += 0.35f;
				player.GetAttackSpeed(BardDamage.Instance) += 0.35f;
				player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.55f;
				player.GetModPlayer<ThoriumPlayer>().bardResourceDropBoost += 0.55f;
			}
			else if(item.ModItem?.Name == "WulfrumJacket") player.GetModPlayer<ThoriumPlayer>().bardRangeBoost += 125;
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if(item.ModItem?.Mod.Name == "CalamityEntropy" && (item.ModItem?.Name == "VoidFaquirBodyArmor" || item.ModItem?.Name == "VoidFaquirCuises") && (Main.LocalPlayer.armor[0].ModItem?.Name == "VoidFaquirBiretta" || Main.LocalPlayer.armor[0].ModItem?.Name == "VoidFaquirChapeau") && Main.LocalPlayer.armor[1].ModItem?.Name == "VoidFaquirBodyArmor" && Main.LocalPlayer.armor[2].ModItem?.Name == "VoidFaquirCuises") tooltips.Add(new TooltipLine(item.ModItem.Mod, "Armor Bonus", Language.GetTextValue("Mods.CalamityBardHealer.Items." + Main.LocalPlayer.armor[0].ModItem?.Name + ".SetBonus")));
			string newTooltip = Language.GetTextValue("Mods.CalamityBardHealer.ItemDescriptions." + item.ModItem?.Name);
			if(string.IsNullOrWhiteSpace(newTooltip) || newTooltip.Contains("Mods.CalamityBardHealer.ItemDescriptions.")) return;
			foreach(TooltipLine m in tooltips) if(m.Mod == "Terraria" && m.Name == "Tooltip0") m.Text = newTooltip + "\n" + m.Text;
		}
	}
}