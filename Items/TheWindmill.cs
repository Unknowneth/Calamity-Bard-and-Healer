using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class TheWindmill : ScytheItem
	{
		public override void SetStaticDefaults() => base.SetStaticDefaultsToScythe();
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.crit = 2;
			base.Item.damage = 16;
			this.scytheSoulCharge = 2;
			base.Item.width = 54;
			base.Item.height = 54;
			base.Item.value = Item.sellPrice(silver: 80);
			base.Item.rare = 3;
			base.Item.scale = 1.15f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.TheWindmill>();
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
				p = ModContent.ProjectileType<Projectiles.Whirlwind>();
				if(player.ownedProjectileCounts[p] > 0) return false;
				p = Projectile.NewProjectile(source, position, Vector2.UnitX * player.direction, p, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("AerialiteBar").Type, 9).AddIngredient(824, 3).AddTile(305).Register();
		}
	}
}