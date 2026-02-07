using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class SoulSplicer : ScytheItem
	{
		public override void SetStaticDefaults() {
			base.SetStaticDefaultsToScythe();
		}
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.crit = 12;
			base.Item.damage = 72;
			this.scytheSoulCharge = 2;
			base.Item.width = 72;
			base.Item.height = 58;
			base.Item.value = Item.sellPrice(gold: 12);
			base.Item.rare = 8;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.SoulSplicer>();
			base.Item.useTime /= 2;
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
				if(player.itemAnimation < player.itemAnimationMax) return false;
				p = Projectile.NewProjectile(source, position, Main.rand.NextVector2CircularEdge(8f, 8f) * Main.rand.NextFloat(0.75f, 1.5f), ModContent.ProjectileType<Projectiles.SoulSplicerApparition>(), damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
	}
}