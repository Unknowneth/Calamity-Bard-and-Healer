using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class GodNote : BardProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityHunt");
		public override string Texture => "ThoriumMod/Empty";
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 1, base.Projectile.type);
			mor.Call("addElementProj", 8, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			Projectile.width = 28;
			Projectile.height = 18;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = BardDamage.Instance;
			Projectile.timeLeft = 300;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 1;
		}
		public override void AI() {;
			if(Projectile.alpha > 0) Projectile.alpha -= 17;
			int target = -1;
			float maxRange = 2000f;
			foreach(NPC npc in Main.ActiveNPCs) if(npc.CanBeChasedBy(Projectile, false) && npc.Distance(Projectile.Center) < maxRange) {
				maxRange = npc.Distance(Projectile.Center);
				target = npc.whoAmI;
			}
			if(target >= 0) {
				Projectile.velocity += Vector2.Normalize(Main.npc[target].Center - Projectile.Center);
				Projectile.velocity *= 0.95f;
			}
			else {
				Projectile.velocity.Y += 0.2f;
				Projectile.velocity *= 0.98f;
			}
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center + Projectile.velocity * 0.5f, Projectile.velocity * 0.5f, Main.hslToRgb(Projectile.ai[0] % 1f, 1f, 0.5f), 10, Main.rand.NextFloat(0.7f, 0.9f) * Projectile.scale, 0.5f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4), true, 0f, true));
		}
		public override void OnKill(int timeLeft) {
			for(int i = 0; i < 15; i++) {
				GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center + Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(i, i), Projectile.velocity * 0.5f, Main.hslToRgb(Projectile.ai[0] % 1f, 1f, 0.5f), 10, Main.rand.NextFloat(0.7f, 0.9f) * Projectile.scale, 0.5f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4), true, 0f, true));
				ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings { PositionInWorld = Projectile.Center, MovementVector = Main.rand.NextVector2Circular(i, i), UniqueInfoPiece = (byte)(Main.rgbToHsl(Main.hslToRgb(Projectile.ai[0] % 1f, 1f, 0.5f)).X * 255f)});
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			string[] Texture = new string[] {
				"WholeNote",
				"HalfNote",
				"QuarterNote",
				"EighthNote"
			};
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("CalamityBardHealer/Projectiles/" + Texture[(int)Projectile.ai[1]]);
			lightColor = Main.hslToRgb(Projectile.ai[0] % 1f, 1f, 0.5f) with { A = 0 };
			if(Main.myPlayer != Projectile.owner) lightColor *= Projectile.Opacity;
			for(int l = 0; l < 7; l++) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition + Vector2.UnitY.RotatedBy(MathHelper.TwoPi / 3f * (l - 1) + (l > 3 ? MathHelper.TwoPi / 6f : 0f) + (Main.GlobalTimeWrappedHourly * MathHelper.TwoPi + MathHelper.PiOver4 * 0.2f)) * MathHelper.Max(l, 1), null, lightColor * (l > 3 ? 0.15f : 0.45f), 0f, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}