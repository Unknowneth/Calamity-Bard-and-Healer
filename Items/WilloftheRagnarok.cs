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
	public class WilloftheRagnarok : ThoriumItem
	{
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(3, 8));
			Item.staff[base.Item.type] = true;
		}
		public override void SetDefaults() {
			base.Item.DamageType = HealerDamage.Instance;
			base.Item.damage = 222;
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			base.Item.mana = 40;
			this.radiantLifeCost = 20;
			this.isHealer = true;
			base.Item.width = 76;
			base.Item.height = 76;
			base.Item.useTime = 42;
			base.Item.useAnimation = 42;
			base.Item.knockBack = 2f;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			base.Item.UseSound = SoundID.Item20;
			base.Item.useStyle = 5;
			base.Item.autoReuse = true;
			base.Item.noMelee = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.WilloftheRagnarok>();
			base.Item.shootSpeed = 10f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && Main.myPlayer == player.whoAmI) for(int i = -1; i <= 1; i++) if(i != 0) {
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * base.Item.Size.Length(), velocity.RotatedBy(i * MathHelper.PiOver2), type, damage, knockback, player.whoAmI, (-velocity).ToRotation(), i);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("Lucidity").Type).AddIngredient(ModContent.ItemType<Items.SlagFurysIntent>()).AddIngredient(ModContent.ItemType<Items.AquaiusAdvice>()).AddIngredient(ModContent.ItemType<Items.OmnicidesLaw>()).AddIngredient(calamity.Find<ModItem>("AuricBar").Type, 7).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}