using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class HydrothermicHat : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 28;
			Item.rare = 8;
			Item.value = Item.sellPrice(gold: 12);
			Item.defense = 13;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.12f;
			player.GetAttackSpeed(BardDamage.Instance) += 0.05f;
			player.GetCritChance(BardDamage.Instance) += 10;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetModPlayer<CalamityPlayer>().ataxiaBlaze = true;
			player.GetModPlayer<ThorlamityPlayer>().ataxiaBard = true;
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.12f;
			player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.HydrothermicHat.SetBonus") + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.Hardmode.HydrothermicArmor.CommonSetBonus");
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