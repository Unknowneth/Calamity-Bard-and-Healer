using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class VictideAmmoniteHat : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 40;
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 40);
			Item.defense = 4;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.05f;
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.05f;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetModPlayer<CalamityPlayer>().victideSet = true;
			player.ignoreWater = true;
			if(Collision.DrownCollision(player.position, player.width, player.height, player.gravDir, false)) {
				player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.1f;
				player.lifeRegen += 3;
			}
			player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.VictideAmmoniteHat.SetBonus") + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.PreHardmode.VictideBreastplate.CommonSetBonus");
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("SeaRemains").Type, 3).AddTile(16).Register();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("VictideBreastplate").Type && legs.type == calamity.Find<ModItem>("VictideGreaves").Type : false;
	}
}