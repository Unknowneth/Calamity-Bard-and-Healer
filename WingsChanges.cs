using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;
using System.Collections.Generic;

namespace CalamityBardHealer
{
	public class WingsChanges : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstatiation) => ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && (item.type == thorium.Find<ModItem>("DragonWings").Type || item.type == thorium.Find<ModItem>("FleshWings").Type || item.type == thorium.Find<ModItem>("DreadWings").Type || item.type == thorium.Find<ModItem>("DemonBloodWings").Type || item.type == thorium.Find<ModItem>("TitanWings").Type || item.type == thorium.Find<ModItem>("CelestialCarrier").Type || item.type == thorium.Find<ModItem>("ShootingStarTurboTuba").Type || item.type == thorium.Find<ModItem>("WhiteDwarfThrusters").Type || item.type == thorium.Find<ModItem>("TerrariumWings").Type || item.type == thorium.Find<ModItem>("ChampionWing").Type || item.type == thorium.Find<ModItem>("DridersGrace").Type || (item.ModItem is ModItem m && m.Name == "SubspaceWings" && m.Mod.Name == "ThoriumMod"));
		public override void SetDefaults(Item item) {
			if(item.ModItem?.Name == "ChampionWing") ArmorIDs.Wing.Sets.Stats[item.wingSlot] = new WingStats(120, 6.4f, 1f, false, -1f, 1f);
		}
		public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend) {
			if(item.ModItem?.Name != "CelestialCarrier" || item.ModItem?.Name != "ShootingStarTurboTuba" || item.ModItem?.Name != "WhiteDwarfThrusters" || item.ModItem?.Name != "TerrariumWings") return;
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 3f;
			constantAscend = 0.135f;
		}
		public override void UpdateEquip(Item item, Player player) {
			if(item.ModItem?.Mod.Name == "ThoriumMod") {
				if(item.ModItem?.Name == "DragonWings" && player.armor[0].ModItem?.Name == "DragonMask" && player.armor[1].ModItem?.Name == "DragonBreastplate" && player.armor[2].ModItem?.Name == "DragonGreaves") player.GetModPlayer<ThorlamityPlayer>().dragonWings = true;
				if(item.ModItem?.Name == "FleshWings" && player.armor[0].ModItem?.Name == "FleshMask" && player.armor[1].ModItem?.Name == "FleshBody" && player.armor[2].ModItem?.Name == "FleshLegs") player.GetModPlayer<ThorlamityPlayer>().fleshWings = true;
				if(item.ModItem?.Name == "DreadWings" && player.armor[0].ModItem?.Name == "DreadSkull" && player.armor[1].ModItem?.Name == "DreadChestPlate" && player.armor[2].ModItem?.Name == "DreadGreaves") player.GetModPlayer<ThorlamityPlayer>().dreadWings = true;
				if(item.ModItem?.Name == "DemonBloodWings" && player.armor[0].ModItem?.Name == "DemonBloodHelmet" && player.armor[1].ModItem?.Name == "DemonBloodBreastPlate" && player.armor[2].ModItem?.Name == "DemonBloodGreaves") player.GetModPlayer<ThorlamityPlayer>().demonBloodWings = true;
				if(item.ModItem?.Name == "TitanWings" && (player.armor[0].ModItem?.Name == "TitanHelmet" || player.armor[0].ModItem?.Name == "TitanHeadgear" || player.armor[0].ModItem?.Name == "TitanMask") && player.armor[1].ModItem?.Name == "TitanBreastplate" && player.armor[2].ModItem?.Name == "TitanGreaves") {
					player.GetModPlayer<ThoriumPlayer>().thoriumEndurance += 0.15f;
					player.GetDamage(DamageClass.Generic) += 0.15f;
				}
				if(item.ModItem?.Name == "CelestialCarrier" && player.armor[0].ModItem?.Name == "CelestialCrown" && player.armor[1].ModItem?.Name == "CelestialVestment" && player.armor[2].ModItem?.Name == "CelestialLeggings") player.GetModPlayer<ThoriumPlayer>().healBonus += 4;
				if(item.ModItem?.Name == "ShootingStarTurboTuba" && player.armor[0].ModItem?.Name == "ShootingStarHat" && player.armor[1].ModItem?.Name == "ShootingStarShirt" && player.armor[2].ModItem?.Name == "ShootingStarBoots") player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.15f;
				if(item.ModItem?.Name == "WhiteDwarfThrusters" && player.armor[0].ModItem?.Name == "WhiteDwarfMask" && player.armor[1].ModItem?.Name == "WhiteDwarfGuard" && player.armor[2].ModItem?.Name == "WhiteDwarfGreaves") player.GetModPlayer<ThoriumPlayer>().throwerExhaustionMax += 600;
				if(item.ModItem?.Name == "TerrariumWings" && player.armor[0].ModItem?.Name == "TerrariumHelmet" && player.armor[1].ModItem?.Name == "TerrariumBreastPlate" && player.armor[2].ModItem?.Name == "TerrariumGreaves" && player.miscCounter % 2 == 0 && player.wingTime > 0) player.wingTime++;
				if(item.ModItem?.Name == "DridersGrace" && player.armor[0].type == 2370 && player.armor[1].type == 2371 && player.armor[2].type == 2372) player.maxMinions++;
				if(item.ModItem?.Name == "ChampionWing") player.GetDamage(DamageClass.Generic) += 0.1f;
			}
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if(item.ModItem?.Mod.Name != "ThoriumMod") return;
			var a = ModLoader.GetMod("CalamityMod").Find<ModItem>(item.ModItem?.Name == "ChampionWing" || item.ModItem?.Name == "DridersGrace" ? "SkylineWings" : item.ModItem?.Name == "FleshWings" || item.ModItem?.Name == "DragonWings" || item.ModItem?.Name == "DreadWings" || item.ModItem?.Name == "DemonBloodWings" || item.ModItem?.Name == "SubspaceWings" ? "StarlightWings" : "ExodusWings");
			a.Item.wingSlot = item.wingSlot;
			a.ModifyTooltips(tooltips);
			string newTooltip = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.ItemDescriptions." + item.ModItem?.Name);
			if(string.IsNullOrWhiteSpace(newTooltip) || newTooltip.Contains("Mods.CalamityBardHealer.ItemDescriptions.")) return;
			foreach(TooltipLine m in tooltips) if(m.Mod == "Terraria" && m.Name == "Tooltip0") m.Text += "\n" + newTooltip;
		}
	}
}