using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class GelatinTherapy : ThoriumItem
	{
		public override void SetStaticDefaults() => Item.staff[base.Item.type] = true;
		public override void SetDefaults() {
			this.healType = HealType.AllyAndPlayer;
			this.healAmount = 13;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			this.isHealer = true;
			base.Item.DamageType = ThoriumDamageBase<HealerTool>.Instance;
			base.Item.mana = 25;
			base.Item.width = 56;
			base.Item.height = 56;
			base.Item.useTime = 36;
			base.Item.useAnimation = 36;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 1f;
			base.Item.value = Item.sellPrice(gold: 2, silver: 40);
			base.Item.rare = 4;
			base.Item.UseSound = SoundID.Item20;
			base.Item.autoReuse = false;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.TherapeuticSludge>();
			base.Item.shootSpeed = 10f;
		}
		public override Color? GetAlpha(Color lightColor) {
			lightColor.A = 200;
			return lightColor;
		}
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback) {
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * base.Item.Size.Length();
			if(Collision.CanHitLine(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("PurifiedGel").Type, 15).AddIngredient(calamity.Find<ModItem>("BlightedGel").Type, 15).AddTile(calamity.Find<ModTile>("StaticRefiner").Type).Register();
		}
	}
}