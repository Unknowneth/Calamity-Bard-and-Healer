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
	public class BloomingSaintessDevotion : ThoriumItem
	{
		public override void SetStaticDefaults() => Item.staff[base.Item.type] = true;
		public override void SetDefaults() {
			base.Item.DamageType = HealerDamage.Instance;
			base.Item.damage = 169;
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			base.Item.mana = 25;
			this.radiantLifeCost = 15;
			this.isHealer = true;
			base.Item.width = 66;
			base.Item.height = 66;
			base.Item.useTime = 42;
			base.Item.useAnimation = 42;
			base.Item.knockBack = 2f;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("Turquoise").Type;
			else base.Item.rare = 11;
			base.Item.value = Item.sellPrice(gold: 24);
			base.Item.UseSound = SoundID.Item8;
			base.Item.useStyle = 5;
			base.Item.autoReuse = true;
			base.Item.noMelee = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.BloomingSaintessDevotion>();
			base.Item.shootSpeed = 14f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * (base.Item.Size.Length() - 4f), velocity, type, damage, knockback, player.whoAmI, Main.rand.Next(-99, 100) * 0.01f, 64f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("PaganGrasp").Type).AddIngredient(thorium.Find<ModItem>("LifeAndDeath").Type).AddIngredient(calamity.Find<ModItem>("UelibloomBar").Type, 7).AddTile(412).Register();
		}
	}
}