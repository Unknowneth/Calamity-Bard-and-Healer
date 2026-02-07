using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class MilkyWay : ScytheItem
	{
		public override void SetStaticDefaults() {
			base.SetStaticDefaultsToScythe();
			Terraria.ID.ItemID.Sets.ItemsThatAllowRepeatedRightClick[base.Item.type] = true;
		}
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.crit = 46;
			base.Item.damage = 290;
			this.scytheSoulCharge = 2;
			base.Item.width = 112;
			base.Item.height = 102;
			base.Item.scale = 1.2f;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("CosmicPurple").Type;
			else base.Item.rare = 11;
			base.Item.value = Item.sellPrice(gold: 28);
			base.Item.shoot = ModContent.ProjectileType<Projectiles.MilkyWay>();
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = -1;
				if(player.altFunctionUse == 2) p = Projectile.NewProjectile(source, position, Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * -320f, type, damage, knockback, player.whoAmI, (Main.rand.Next(2, 5) + 1) * 0.1f, player.itemTime);
				else p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.Syzygy>()).AddIngredient(calamity.Find<ModItem>("CosmiliteBar").Type, 8).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
		public override bool AltFunctionUse(Player player) => true;
	}
}