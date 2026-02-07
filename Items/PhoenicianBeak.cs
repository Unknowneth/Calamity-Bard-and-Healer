using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Utilities;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items.HealerItems;

namespace CalamityBardHealer.Items
{
	public class PhoenicianBeak : ScytheItem
	{
		private int swingDirection = 0;
		public override void SetStaticDefaults() {
			base.SetStaticDefaultsToScythe();
			Terraria.ID.ItemID.Sets.SkipsInitialUseSound[base.Item.type] = true; 
		}
		public override void SetDefaults() {
			base.SetDefaultsToScythe();
			base.Item.damage = 880;
			this.scytheSoulCharge = 4;
			base.Item.width = 144;
			base.Item.height = 202;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else base.Item.rare = 10;
			base.Item.holdStyle = 6;
			base.Item.useStyle = 100;
			base.Item.noUseGraphic = false;
			base.Item.useTime = 28;
			base.Item.useAnimation = 28;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.PhoenicianBeak>();
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
			return false;
		}
		public override void HoldStyle(Player player, Rectangle itemFrame) => player.itemLocation += new Vector2(-56f, 42f) * player.Directions;
		public override void UseStyle(Player player, Rectangle itemFrame) => player.itemLocation = Vector2.Zero;
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override bool MeleePrefix() => true;
	}
}