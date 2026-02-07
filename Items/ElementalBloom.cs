using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Face)]
	public class ElementalBloom : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[Item.faceSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 44;
			Item.height = 44;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("CosmicPurple").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 28);
			Item.maxStack = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player) {
			player.statLifeMax2 += 40;
			player.statManaMax2 += 20;
			player.GetAttackSpeed(ThoriumDamageBase<HealerTool>.Instance) += 0.15f;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 2;
			player.GetModPlayer<ThoriumPlayer>().accHexingTalisman = true;
			player.GetModPlayer<ThorlamityPlayer>().elementalBloom = true;
			player.GetDamage(HealerDamage.Instance) += 0.2f;
			player.GetAttackSpeed(HealerDamage.Instance) += 0.1f;
			player.GetCritChance(HealerDamage.Instance) += 12;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.BloomingSaintessStatue>()).AddIngredient(thorium.Find<ModItem>("HexingTalisman").Type).AddIngredient(3467, 8).AddIngredient(calamity.Find<ModItem>("GalacticaSingularity").Type, 4).AddIngredient(calamity.Find<ModItem>("AscendantSpiritEssence").Type, 4).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
	}
}