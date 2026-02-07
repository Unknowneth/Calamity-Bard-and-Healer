using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;

namespace CalamityBardHealer.Items
{
	public class OmniSpeaker : ModItem
	{
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 46;
			Item.height = 50;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("CosmicPurple").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 28);
			Item.maxStack = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.15f;
			player.GetCritChance(BardDamage.Instance) += 15;
			player.GetAttackSpeed(BardDamage.Instance) += 0.15f;
			player.GetModPlayer<ThoriumPlayer>().accBrassMute2 = true;
			player.GetModPlayer<ThoriumPlayer>().accWindHoming = true;
			player.GetModPlayer<ThoriumPlayer>().bardBounceBonus += 3;
			player.GetModPlayer<ThorlamityPlayer>().omniSpeaker = true;
			player.GetModPlayer<ThoriumPlayer>().bardRangeBoost += 750;
			player.GetModPlayer<ThoriumPlayer>().bardBuffDuration += 120;
			player.GetModPlayer<ThoriumPlayer>().accPercussionTuner2 = true;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("TerrariumSurroundSound").Type).AddIngredient(ModContent.ItemType<Items.TreeWhispererAmulet>()).AddIngredient(3467, 8).AddIngredient(calamity.Find<ModItem>("GalacticaSingularity").Type, 4).AddIngredient(calamity.Find<ModItem>("AscendantSpiritEssence").Type, 4).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
	}
}