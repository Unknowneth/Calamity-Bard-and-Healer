using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class ArcticReinforcement : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<FlightTime>(2, 0);
			this.Empowerments.AddInfo<JumpHeight>(2, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 38;
			base.Item.height = 38;
			base.Item.holdStyle = 6;
			base.Item.useStyle = 14;
			base.Item.useTime = 24;
			base.Item.useAnimation = 24;
			base.Item.damage = 33;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 1.5f;
			base.Item.value = Item.sellPrice(gold: 7, silver: 20);
			base.Item.rare = 5;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 14f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.ArcticReinforcement>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/ArcticReinforcement") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.InspirationCost = 1;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int p = Projectile.NewProjectile(source, position, Vector2.Zero, type, 0, 0f, player.whoAmI, player.itemAnimationMax, player.itemAnimationMax, Vector2.Distance(Main.MouseWorld, player.MountedCenter) + 16f);
				NetMessage.SendData(27, -1, -1, null, p);
				for(int i = 0; i < 3; i++) {
					p = Projectile.NewProjectile(source, position, -velocity.RotatedBy(MathHelper.ToRadians(120f) * i) * 0.3f, ModContent.ProjectileType<Projectiles.ArcticReinforcement2>(), damage, knockback, player.whoAmI, 0f, 0f, i);
					NetMessage.SendData(27, -1, -1, null, p);
				}
			}
			return false;
		}
	}
}