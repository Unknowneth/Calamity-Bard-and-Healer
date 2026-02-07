using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;


namespace CalamityBardHealer
{
	public class ItemBalancing : GlobalItem
	{
		public override bool AppliesToEntity(Item item, bool lateInstatiation) {
			if(item.ModItem?.Mod.Name != "ThoriumMod") return false;
			if(item.useAmmo == AmmoID.Bullet) return true;
			string n;
			if(item.ModItem is ModItem m) n = m.Name;
			else return false;
			if(n == "EclipseFang" || n == "EssenceofFlame" || n == "WhirlpoolSaber") return true;
			if(ItemID.Sets.Spears[item.type]) return ModLoader.HasMod("ThoriumRework") ? n != "EnergyStormPartisan" && n != "IllumiteSpear" && n != "ValadiumSpear" && n != "FleshSkewer" && n != "DragonTalon" && n != "DreadFork" && n != "DemonBloodSpear" : true;
			if(n == "ChampionSwiftBlade" || n == "ValadiumSlicer" || n == "LodeStoneClaymore" || n == "TerrariumSaber" || n == "TitanSword" || n == "IllumiteBlade" || n == "ToothOfTheConsumer" || n == "DragonTooth" || n == "DemonBloodSword" || n == "DreadRazor" || n == "PrimesFury" || n == "SoulRender") return !ModLoader.HasMod("ThoriumRework");
			return n.EndsWith("Phasesaber") && item.CountsAsClass(DamageClass.Melee);
		}
		public override void SetDefaults(Item item) {
			if(item.CountsAsClass(DamageClass.Melee)) {
				if(!(item.ModItem?.Name.EndsWith("Phasesaber") ?? false)) item.damage *= 2;
				if(!ItemID.Sets.Spears[item.type]) item.scale *= 1.1f + item.rare * 0.05f;
			}
			else if(item.useAmmo == AmmoID.Bullet) ModLoader.GetMod("CalamityMod").Call("SetFirePointBlank", item, true);
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
			if(!(item.ModItem?.Name.EndsWith("Phasesaber") ?? false)) return;
			string newTooltip = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.ItemDescriptions." + item.ModItem?.Name);
			if(string.IsNullOrWhiteSpace(newTooltip) || newTooltip.Contains("Mods.CalamityBardHealer.ItemDescriptions.")) return;
			foreach(TooltipLine m in tooltips) if(m.Mod == "Terraria" && m.Name == "Knockback") m.Text += "\n" + newTooltip;
		}
		public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers) {
			if(item.ModItem?.Name.EndsWith("Phasesaber") ?? false) modifiers.ArmorPenetration += target.defense;
		}
	}
}