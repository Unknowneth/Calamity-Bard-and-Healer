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
	public class FlightoftheGoliath : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<EmpowermentProlongation>(3, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 54;
			base.Item.height = 32;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
			base.Item.useTime = 16;
			base.Item.useAnimation = 16;
			base.Item.damage = 45;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = Item.sellPrice(gold: 12);
			base.Item.rare = 8;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 6f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.PlagueSwarmerMissile>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.scale = 0.8f;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/FlightoftheGoliath") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.InspirationCost = 2;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			int j = Main.rand.Next(3);
			if(Main.myPlayer == player.whoAmI) for(int i = -1; i <= 1; i++) if(i != 0) {
				j += i;
				if(j > 2) j = 0;
				else if(j < 0) j = 2;
				if(j > 1) position = new Vector2(12);
				else if(j < 1) position = new Vector2(24, 10);
				else position = new Vector2(36, 8);
				position *= base.Item.scale;
				int p = Projectile.NewProjectile(source, player.MountedCenter + (position * player.Directions).RotatedBy(player.fullRotation), Vector2.UnitX.RotatedBy(player.fullRotation + MathHelper.ToRadians(j + (i + 1) * 7.5f) - Main.rand.NextFloat(MathHelper.PiOver4) * 0.2f) * player.direction * velocity.Length(), type, damage, knockback, player.whoAmI, 0f, 0f, 60f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void UseItemFrame(Player player) {
			float attackTime = (float)player.itemAnimation / (float)player.itemAnimationMax;
			if(attackTime < 0.3f && attackTime > 0.1f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Quarter;
			else if(attackTime < 0.8f && attackTime > 0.5f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Full;
			else player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
			player.compositeBackArm.rotation -= Vector2.UnitY.RotatedBy(attackTime * MathHelper.Pi).X * MathHelper.PiOver4 / 3f * player.direction;
			this.HoldItemFrame(player);
		}
		public override void HoldItemFrame(Player player) => player.itemLocation -= new Vector2(4f, 1f) * player.Directions;
	}
}