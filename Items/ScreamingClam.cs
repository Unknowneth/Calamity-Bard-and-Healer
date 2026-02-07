using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Items
{
	public class ScreamingClam : ModItem
	{
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 34;
			Item.rare = 2;
			Item.value = Item.sellPrice(silver: 80);
			Item.maxStack = 1;
			Item.accessory = true;
		}
		public override void UpdateEquip(Player player) {
			player.GetModPlayer<ThorlamityPlayer>().screamingClam = true;
			int p = ModContent.ProjectileType<Projectiles.ScreamingClam>();
			int z = player.ownedProjectileCounts[p];
			if(z > 0 || Main.myPlayer != player.whoAmI) return;
			z = Projectile.NewProjectile(player.GetSource_Misc("Screaming Clam"), player.MountedCenter, player.velocity, p, (int)player.GetDamage(ThoriumMod.BardDamage.Instance).ApplyTo(22f), 0f, player.whoAmI, z);
			NetMessage.SendData(27, -1, -1, null, z);
		}
	}
}