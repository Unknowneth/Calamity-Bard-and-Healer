using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class SavingGrace : ThoriumItem
	{
		public override void SetStaticDefaults() {
			Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(2, 8));
			Item.staff[base.Item.type] = true;
		}
		public override void SetDefaults() {
			this.healType = HealType.Ally;
			this.healAmount = 24;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			this.isHealer = true;
			base.Item.DamageType = ThoriumDamageBase<HealerTool>.Instance;
			base.Item.mana = 25;
			base.Item.width = 54;
			base.Item.height = 54;
			base.Item.useTime = 18;
			base.Item.useAnimation = 18;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 1f;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("CosmicPurple").Type;
			else base.Item.rare = 11;
			base.Item.value = Item.sellPrice(gold: 28);
			base.Item.UseSound = SoundID.Item8;
			base.Item.autoReuse = false;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.SavingGrace>();
			base.Item.shootSpeed = 16f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) for(int i = -1; i <= 1; i++) if(i != 0) {
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * base.Item.Size, velocity.RotatedBy(i * MathHelper.PiOver4 * 0.5f), type, damage, knockback, player.whoAmI, -i);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(thorium.Find<ModItem>("CelestialWand").Type).AddIngredient(thorium.Find<ModItem>("DivineStaff").Type).AddIngredient(calamity.Find<ModItem>("CosmiliteBar").Type, 8).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
	}
}