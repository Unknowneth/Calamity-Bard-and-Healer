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
	public class PurgatoriumPandemonium : ThoriumItem
	{
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(3, 4));
			Item.staff[base.Item.type] = true;
		}
		public override void SetDefaults() {
			base.Item.DamageType = HealerDamage.Instance;
			base.Item.damage = 333;
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			base.Item.mana = 27;
			base.Item.crit = 16;
			this.radiantLifeCost = 9;
			this.isHealer = true;
			this.healAmount = 9;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healType = HealType.Ally;
			this.healDisplay = true;
			base.Item.width = 88;
			base.Item.height = 88;
			base.Item.useTime = 27;
			base.Item.useAnimation = 27;
			base.Item.knockBack = 1f;
			base.Item.value = Item.sellPrice(gold: 26);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("PureGreen").Type;
			base.Item.UseSound = SoundID.Item20;
			base.Item.useStyle = 5;
			base.Item.autoReuse = true;
			base.Item.noMelee = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.PurgedSoul>();
			base.Item.shootSpeed = 10f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium) && Main.myPlayer == player.whoAmI) for(int i = 0; i < 6; i++) {
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * base.Item.Size.Length(), velocity + Main.rand.NextVector2CircularEdge(1, i).RotatedBy(velocity.ToRotation()), type, damage, knockback, player.whoAmI, 0f, 0f, i % 2 == 0 ? 1f : 0f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("LifeAndDeath").Type).AddIngredient(calamity.Find<ModItem>("RuinousSoul").Type, 2).AddTile(412).Register();
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}