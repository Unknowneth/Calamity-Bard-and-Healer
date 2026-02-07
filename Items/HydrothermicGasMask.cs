using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class HydrothermicGasMask : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 22;
			Item.rare = 8;
			Item.value = Item.sellPrice(gold: 12);
			Item.defense = 17;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.39f;
			player.GetDamage(DamageClass.Generic) -= 0.27f;
			player.GetCritChance(HealerDamage.Instance) += 6;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 4;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetModPlayer<CalamityPlayer>().ataxiaBlaze = true;
			player.GetModPlayer<ThorlamityPlayer>().ataxiaHealer = true;
			player.GetDamage(HealerDamage.Instance) += 0.28f;
			player.GetAttackSpeed(HealerDamage.Instance) += 0.09f;
			player.GetAttackSpeed(ThoriumDamageBase<HealerTool>.Instance) += 0.09f;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 2;
			player.setBonus = Terraria.Localization.Language.GetText("Mods.CalamityBardHealer.Items.HydrothermicGasMask.SetBonus").Format(ModLoader.GetMod("ThoriumMod").Call("GetHealerHealBonus", player)).ToString() + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.Hardmode.HydrothermicArmor.CommonSetBonus");
		}
		public override void ArmorSetShadows(Player player) {
			player.armorEffectDrawOutlines = true;
			player.GetModPlayer<CalamityPlayer>().hydrothermalSmoke = true;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("ScoriaBar").Type, 7).AddIngredient(calamity.Find<ModItem>("EssenceofHavoc").Type, 1).AddTile(134).Register();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("HydrothermicArmor").Type && legs.type == calamity.Find<ModItem>("HydrothermicSubligar").Type : false;
	}
}