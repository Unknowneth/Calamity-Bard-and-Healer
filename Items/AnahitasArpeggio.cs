using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Audio;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using System;

namespace CalamityBardHealer.Items
{
	public class AnahitasArpeggio : BardItem
	{
		public override string Texture => "CalamityMod/Items/Weapons/Magic/AnahitasArpeggio";
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<LifeRegeneration>(2, 0);
			this.Empowerments.AddInfo<DamageReduction>(2, 0);
			this.Empowerments.AddInfo<AquaticAbility>(3, 0);
		}
		public override void SetBardDefaults() {
			base.Item.CloneDefaults(ModLoader.GetMod("CalamityMod").Find<ModItem>("AnahitasArpeggio").Type);
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.mana = 0;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.AnahitasArpeggioNote>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/AnahitasArpeggio") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.InspirationCost = 1;
			if(ModLoader.HasMod("Look")) base.Item.holdStyle = 0;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer != player.whoAmI) return false;
			Vector2 mouseDist = Main.MouseWorld - player.MountedCenter;
			float soundMult = mouseDist.Length() / ((float)Main.screenHeight / 2f);
			if(soundMult > 1f) soundMult = 1f;
			float soundPitch = soundMult * 2f - 1f;
			soundPitch = MathHelper.Clamp(soundPitch, -1f, 1f);
			velocity.X += Main.rand.NextFloat(-0.75f, 0.75f);
			velocity.Y += Main.rand.NextFloat(-0.75f, 0.75f);
			velocity.X *= soundMult + 0.25f;
			velocity.Y *= soundMult + 0.25f;
			int p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, soundPitch, 0f, 0f);
			NetMessage.SendData(27, -1, -1, null, p);
			return false;
		}
		public override void UseItemFrame(Player player) {
			if(ModLoader.HasMod("Look") && player.compositeFrontArm.enabled && player.itemAnimation > 0) {
				float attackTime = (float)player.itemAnimation / (float)player.itemAnimationMax;
				player.compositeFrontArm.stretch = (attackTime > 0.66f || attackTime < 0.33f) ? Player.CompositeArmStretchAmount.ThreeQuarters : Player.CompositeArmStretchAmount.Quarter;
				player.compositeFrontArm.rotation -= attackTime * MathHelper.PiOver4 * 0.1f * player.direction * player.gravDir;
			}
			this.HoldItemFrame(player);
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += (new Vector2(-16, 12) * player.Directions).RotatedBy(player.itemRotation);
	}
}