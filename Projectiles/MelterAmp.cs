using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class MelterAmp : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Magic/MelterAmp";
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() => Main.projFrames[base.Projectile.type] = 3;
		public override void SetBardDefaults() {
			base.Projectile.CloneDefaults(ModLoader.GetMod("CalamityMod").Find<ModProjectile>("MelterAmp").Type);
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			bool flag = base.Projectile.type == ModContent.ProjectileType<Projectiles.MelterAmp>();
			Player player = Main.player[base.Projectile.owner];
			if(flag) {
				if(player.dead) {
					base.Projectile.active = false;
					return;
				}
				if(player.ownedProjectileCounts[ModContent.ProjectileType<MelterAmp>()] > 1) {
					base.Projectile.active = false;
					return;
				}
				if(player.HeldItem.shoot != ModContent.ProjectileType<Projectiles.MelterNote1>()) {
					base.Projectile.active = false;
					return;
				}
			}
			Lighting.AddLight(base.Projectile.Center, 0.75f, 0.75f, 0.75f);
			if(base.Projectile.ai[0] > 0f) {
				base.Projectile.ai[0] += 1f;
				if(base.Projectile.ai[0] > 6f) base.Projectile.ai[0] = 0f;
			}
			if(Main.myPlayer == player.whoAmI && base.Projectile.ai[0] == 0f) {
				Vector2 Velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center) * 16f;
				base.Projectile.ai[0] = 1f;
				int Damage = base.Projectile.damage;
				base.Projectile.netUpdate = true;
				if(Velocity.HasNaNs()) return;
				Projectile.velocity = Velocity;
				int type = -1;
				if(Main.rand.NextBool(2)) {
					Damage = (int)((float)base.Projectile.damage * 1.5f);
					type = ModContent.ProjectileType<Projectiles.MelterNote1>();
				}
				else {
					Velocity *= 1.5f;
					type = ModContent.ProjectileType<Projectiles.MelterNote2>();
				}
				int p = Projectile.NewProjectile(base.Projectile.GetSource_FromThis(), base.Projectile.Center, Velocity, type, Damage, base.Projectile.knockBack, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			if(++base.Projectile.frameCounter > 5) {
				if(++base.Projectile.frame > 2) base.Projectile.frame = 0;
				base.Projectile.frameCounter = 0;
			}
			if(base.Projectile.velocity.X != 0) base.Projectile.spriteDirection = base.Projectile.direction = System.Math.Sign(base.Projectile.velocity.X);
		}
		public override bool ShouldUpdatePosition() => false;
	}
}
