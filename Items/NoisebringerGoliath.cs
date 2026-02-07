using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Items
{
	public class NoisebringerGoliath : ModItem
	{
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 36;
			Item.height = 44;
			Item.rare = 8;
			Item.value = Item.sellPrice(gold: 12);
			Item.maxStack = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<ThorlamityPlayer>().noisebringerGoliath = true;
			int p = ModContent.ProjectileType<Projectiles.NoisebringerGoliath>();
			int z = player.ownedProjectileCounts[p];
			if(z >= 2 || Main.myPlayer != player.whoAmI) return;
			z = Projectile.NewProjectile(player.GetSource_Misc("Noisebringer Goliath"), player.MountedCenter, player.velocity, p, (int)player.GetDamage(ThoriumMod.BardDamage.Instance).ApplyTo(150f), 0f, player.whoAmI, z);
			NetMessage.SendData(27, -1, -1, null, z);
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) {
				CreateRecipe().AddIngredient(thorium.Find<ModItem>("Subwoofer").Type, 4).AddIngredient(calamity.Find<ModItem>("PlagueCellCanister").Type, 16).AddIngredient(thorium.Find<ModItem>("BioMatter").Type, 8).AddTile(thorium.Find<ModTile>("SoulForgeNew").Type).Register();
				CreateRecipe().AddIngredient(ModContent.ItemType<Items.ScreamingClam>()).AddIngredient(calamity.Find<ModItem>("PlagueCellCanister").Type, 16).AddIngredient(thorium.Find<ModItem>("BioMatter").Type, 8).AddTile(thorium.Find<ModTile>("SoulForgeNew").Type).Register();
			}
		}
	}
}