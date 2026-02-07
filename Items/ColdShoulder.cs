using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class ColdShoulder : ScytheItem
	{
		public override void SetStaticDefaults() {
			base.SetStaticDefaultsToScythe();
			Terraria.ID.ItemID.Sets.ItemsThatAllowRepeatedRightClick[base.Item.type] = true;
		}
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.crit = 6;
			base.Item.damage = 69;
			this.scytheSoulCharge = 2;
			base.Item.width = 76;
			base.Item.height = 76;
			base.Item.value = Item.sellPrice(gold: 4, silver: 80);
			base.Item.rare = 5;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.ColdShoulder>();
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = -1;
				if(player.altFunctionUse == 2) p = Projectile.NewProjectile(source, position, Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * -224f, type, damage + (int)(damage * 0.3f), knockback, player.whoAmI, (Main.rand.Next(2, 4) + 1) * 0.1f, player.itemTime);
				else p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("CryonicBar").Type, 12).AddTile(134).Register();
		}
		public override float UseTimeMultiplier(Player player) => player.altFunctionUse == 2 ? 2f : 1f;
		public override float UseAnimationMultiplier(Player player) => player.altFunctionUse == 2 ? 2f : 1f;
		public override bool AltFunctionUse(Player player) => true;
	}
}