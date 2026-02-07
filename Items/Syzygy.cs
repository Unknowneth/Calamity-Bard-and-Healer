using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class Syzygy : ScytheItem
	{
		public override void SetStaticDefaults() {
			base.SetStaticDefaultsToScythe();
			Terraria.ID.ItemID.Sets.ItemsThatAllowRepeatedRightClick[base.Item.type] = true;
		}
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.crit = 6;
			base.Item.damage = 156;
			this.scytheSoulCharge = 3;
			base.Item.width = 84;
			base.Item.height = 84;
			base.Item.value = Item.sellPrice(gold: 22);
			base.Item.rare = 11;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.Syzygy>();
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) if(player.altFunctionUse == 2) {
				int p = Projectile.NewProjectile(source, position, Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * -320f, type, damage, knockback, player.whoAmI, (Main.rand.Next(2, 4) + 1) * 0.1f, player.itemTime);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			else {
				int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
				for(int i = 0; i < Main.rand.Next(2) + 2; i++) {
					p = Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<Projectiles.ElementalSyzygy>(), damage, knockback, player.whoAmI, Main.rand.NextBool(2) ? -10f : 10f, Main.rand.NextFloat(MathHelper.TwoPi) - MathHelper.Pi, (Main.rand.NextBool(2) ? -24f : 16f) * Main.rand.Next(4, 17) * (i * 0.5f + 1));
					NetMessage.SendData(27, -1, -1, null, p);
				}
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("TerraScythe").Type).AddIngredient(3467, 5).AddIngredient(calamity.Find<ModItem>("LifeAlloy").Type, 5).AddIngredient(calamity.Find<ModItem>("GalacticaSingularity").Type, 5).AddTile(412).Register();
		}
		public override float UseTimeMultiplier(Player player) => player.altFunctionUse == 2 ? 1.5f : 1f;
		public override float UseAnimationMultiplier(Player player) => player.altFunctionUse == 2 ? 1.5f : 1f;
		public override bool AltFunctionUse(Player player) => true;
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}