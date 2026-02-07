using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.Drawing;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class CosmicEncore : ModProjectile
	{
		public override string Texture => "ThoriumMod/Empty";
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if(Projectile.alpha == 255) {
				Vector2 v = Projectile.Center - Main.player[Projectile.owner].Center;
				for(int i = 0; i < (int)v.Length() / 8; i++) {
					Color glowColor2 = Main.hslToRgb(System.Math.Abs(MathHelper.ToRadians(i) / 2f) % 1f, 1f, 0.7f, byte.MaxValue);
					glowColor2.G = (byte)(0.5f * (float)glowColor2.G);
					ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = Projectile.Center - Vector2.Normalize(v) * i * 8, MovementVector = Vector2.Normalize(v) * 0.001f, UniqueInfoPiece = (byte)(Main.rgbToHsl(glowColor2).X * 255f)});
				}
				for(int i = 0; i < 20; i++) {
					Color glowColor2 = Main.hslToRgb(System.Math.Abs(MathHelper.ToRadians(i) * 18f) % 1f, 1f, 0.7f, byte.MaxValue);
					glowColor2.G = (byte)(0.5f * (float)glowColor2.G);
					ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = Projectile.Center, MovementVector = Vector2.Normalize(v).RotatedBy(MathHelper.TwoPi / 20f * i) * Main.rand.Next(8, 13), UniqueInfoPiece = (byte)(Main.rgbToHsl(glowColor2).X * 255f)});
				}
			}
			else if(Projectile.timeLeft > 80 && Projectile.timeLeft < 160 && Main.myPlayer == Projectile.owner && Projectile.timeLeft % 12 == 0) {
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Main.rand.NextVector2Circular(Projectile.Opacity, Projectile.Opacity) * 12.5f, Main.rand.NextVector2CircularEdge(1f, 1f), ModContent.ProjectileType<Projectiles.CosmicEncoreStar>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				NetMessage.SendData(27, -1, -1, null, p);
				Projectile.localAI[1] = 0f;
			}
			if(Projectile.alpha > 0) Projectile.alpha -= 17;
			Color glowColor = Main.hslToRgb(System.Math.Abs(MathHelper.ToRadians(Projectile.timeLeft) * 4f) % 1f, 1f, 0.7f, byte.MaxValue);
			glowColor.G = (byte)(0.5f * (float)glowColor.G);
			Lighting.AddLight(base.Projectile.Center, glowColor.ToVector3());
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, Main.rand.NextVector2Circular(4f, 4f), glowColor, 15, Projectile.Opacity * 1.5f, 0.6f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.3f, true, 0f, true));
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center, Main.rand.NextVector2Circular(4f, 4f), Color.Lerp(Color.DarkOrange, Color.DarkBlue, System.Math.Abs(Vector2.UnitY.RotatedBy(MathHelper.TwoPi * (float)Projectile.timeLeft / 30f).X)), 15, Projectile.Opacity * 1.5f, 0.6f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4) * 0.3f, true, 0f, true));
			ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = Projectile.Center, MovementVector = Main.rand.NextVector2CircularEdge(16, 16f), UniqueInfoPiece = (byte)(Main.rgbToHsl(glowColor).X * 255f)});
		}
		public override bool? CanDamage() => false;
	}
}