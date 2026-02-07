using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;

namespace CalamityBardHealer.Items
{
	public class TreeWhispererAmulet : ModItem
	{
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 46;
			Item.height = 46;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("Turquoise").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 24);
			Item.maxStack = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.12f;
			player.GetCritChance(BardDamage.Instance) += 12;
			player.GetAttackSpeed(BardDamage.Instance) += 0.12f;
			player.GetModPlayer<ThoriumPlayer>().accBrassMute2 = true;
			player.GetModPlayer<ThoriumPlayer>().accWindHoming = true;
			player.GetModPlayer<ThoriumPlayer>().bardBounceBonus += 2;
			player.GetModPlayer<ThoriumPlayer>().accPercussionTuner2 = true;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddRecipeGroup("CalamityBardHealer:InstrumentAccessories").AddIngredient(calamity.Find<ModItem>("UelibloomBar").Type, 4).AddTile(412).Register();
		}
	}
}