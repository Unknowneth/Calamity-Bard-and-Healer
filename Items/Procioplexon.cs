using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Utilities;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class Procioplexon : ScytheItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityEntropy");
		private int swingDirection = 0;
		internal int spin = 0;
		public override void SetStaticDefaults() {
			base.SetStaticDefaultsToScythe();
			Terraria.ID.ItemID.Sets.SkipsInitialUseSound[base.Item.type] = true; 
		}
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.damage = 5500;
			this.scytheSoulCharge = 4;
			base.Item.width = 90;
			base.Item.height = 154;
			base.Item.value = 12000;
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) base.Item.rare = entropy.Find<ModRarity>("AbyssalBlue").Type;
			else base.Item.rare = 11;
			base.Item.holdStyle = 6;
			base.Item.useStyle = 100;
			base.Item.noUseGraphic = false;
			base.Item.useTime = 28;
			base.Item.useAnimation = 28;
			base.Item.scale = 1.25f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.Procioplexon>();
			if(ModContent.GetInstance<BalanceConfig>().radiant != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().radiant * (float)base.Item.damage, 1);
			if(ModContent.GetInstance<BalanceConfig>().soulEssence != 1) this.scytheSoulCharge = (int)MathHelper.Clamp(ModContent.GetInstance<BalanceConfig>().soulEssence * this.scytheSoulCharge, 0, 5);
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(swingDirection != -1 && swingDirection != 1) swingDirection = 1;
			if(Main.myPlayer == player.whoAmI) {
				float attackTime = player.itemAnimationMax > 0 ? player.itemAnimationMax > player.itemTimeMax ? player.itemTimeMax : player.itemAnimationMax : Item.useAnimation;
				int z = Projectile.NewProjectile(source, position, Vector2.Normalize(Main.MouseWorld - player.MountedCenter), type, damage, knockback, player.whoAmI, attackTime, attackTime, player.GetAdjustedItemScale(Item));
				Main.projectile[z].direction = swingDirection;
				NetMessage.SendData(27, -1, -1, null, z);
			}
			swingDirection = -swingDirection;
			if(++spin > 3) spin = 1;
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.PhoenicianBeak>()).AddIngredient(entropy.Find<ModItem>("WyrmTooth").Type, 10).AddIngredient(calamity.Find<ModItem>("ShadowspecBar").Type, 5).AddTile(entropy.Find<ModTile>("AbyssalAltarTile").Type).Register();
		}
		public override void HoldStyle(Player player, Rectangle itemFrame) => player.itemLocation += new Vector2(-24f, 36f) * player.Directions;
		public override void UseStyle(Player player, Rectangle itemFrame) => player.itemLocation = Vector2.Zero;
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override bool MeleePrefix() => true;
	}
}