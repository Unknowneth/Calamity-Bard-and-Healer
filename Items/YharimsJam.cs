using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Items
{
	public class YharimsJam : ModItem
	{
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 38;
			Item.height = 44;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else Item.rare = 10;
			Item.value = Item.sellPrice(gold: 30);
			Item.maxStack = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<ThorlamityPlayer>().yharimsJam = true;
			int p = ModContent.ProjectileType<Projectiles.YharimsJam>();
			int z = player.ownedProjectileCounts[p];
			if(z > 0 || Main.myPlayer != player.whoAmI) return;
			z = Projectile.NewProjectile(player.GetSource_Misc("YharimsJam"), player.MountedCenter, player.velocity, p, (int)player.GetDamage(ThoriumMod.BardDamage.Instance).ApplyTo(930f), 0f, player.whoAmI, z);
			NetMessage.SendData(27, -1, -1, null, z);
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.NoisebringerGoliath>()).AddIngredient(calamity.Find<ModItem>("AuricBar").Type, 12).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
	}
}