using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;

namespace CalamityBardHealer.Items
{
	public class BloomingSaintessStatue : ModItem
	{
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 36;
			Item.height = 52;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("Turquoise").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 24);
			Item.maxStack = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player) {
			player.statLifeMax2 += 20;
			player.statManaMax2 += 20;
			player.GetAttackSpeed(ThoriumDamageBase<HealerTool>.Instance) += 0.12f;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 2;
			player.GetModPlayer<ThorlamityPlayer>().saintessStatue = true;
			player.GetDamage(HealerDamage.Instance) += 0.14f;
			player.GetAttackSpeed(HealerDamage.Instance) += 0.06f;
			player.GetCritChance(HealerDamage.Instance) += 8;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("ArchangelHeart").Type).AddIngredient(thorium.Find<ModItem>("ArchDemonCurse").Type).AddIngredient(calamity.Find<ModItem>("UelibloomBar").Type, 4).AddTile(412).Register();
		}
	}
}