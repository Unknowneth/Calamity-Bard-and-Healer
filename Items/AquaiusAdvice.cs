using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class AquaiusAdvice : ThoriumItem
	{
		public override void SetStaticDefaults() => Item.staff[base.Item.type] = true;
		public override void SetDefaults() {
			base.Item.DamageType = HealerDamage.Instance;
			base.Item.damage = 42;
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			base.Item.mana = 16;
			this.radiantLifeCost = 10;
			this.isHealer = true;
			base.Item.width = 62;
			base.Item.height = 62;
			base.Item.useTime = 16;
			base.Item.useAnimation = 16;
			base.Item.knockBack = 2f;
			base.Item.value = Item.sellPrice(gold: 9, silver: 60);
			base.Item.rare = 7;
			base.Item.UseSound = SoundID.Item20;
			base.Item.useStyle = 5;
			base.Item.autoReuse = true;
			base.Item.noMelee = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.AquaiusAdvice>();
			base.Item.shootSpeed = 5f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) for(int i = 0; i < Main.rand.Next(2, 5); i++) {
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * base.Item.Size.Length(), velocity, type, damage, knockback, player.whoAmI, Main.rand.Next(-99, 100) * 0.03f, -Main.rand.Next(20) - 1f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("AquaiteBar").Type, 9).AddIngredient(calamity.Find<ModItem>("SeaRemains").Type, 2).AddIngredient(calamity.Find<ModItem>("DepthCells").Type, 6).AddIngredient(thorium.Find<ModItem>("AbyssalChitin").Type, 3).AddTile(134).Register();
		}
	}
}