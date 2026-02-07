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
	public class SlagFurysIntent : ThoriumItem
	{
		public override void SetStaticDefaults() => Item.staff[base.Item.type] = true;
		public override void SetDefaults() {
			base.Item.DamageType = HealerDamage.Instance;
			base.Item.damage = 28;
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			base.Item.mana = 10;
			this.radiantLifeCost = 5;
			this.isHealer = true;
			base.Item.width = 70;
			base.Item.height = 70;
			base.Item.useTime = 7;
			base.Item.useAnimation = 7;
			base.Item.knockBack = 2f;
			base.Item.value = Item.sellPrice(gold: 4, silver: 80);
			base.Item.rare = 5;
			base.Item.UseSound = SoundID.Item20;
			base.Item.useStyle = 5;
			base.Item.autoReuse = true;
			base.Item.noMelee = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.SlagFurysIntent>();
			base.Item.shootSpeed = 5f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) for(int i = 0; i < Main.rand.Next(2, 4); i++) {
				float rotOff = Main.rand.Next(-99, 100) * 0.005f;
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * (base.Item.Size.Length() - 12f), velocity.RotatedBy(rotOff * -0.75f), type, damage, knockback, player.whoAmI, rotOff, -Main.rand.Next(20) - 1f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("UnholyCore").Type, 7).AddIngredient(thorium.Find<ModItem>("SoulofPlight").Type, 7).AddTile(134).Register();
		}
	}
}