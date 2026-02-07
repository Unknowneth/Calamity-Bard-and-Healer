using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class BurningBeat : ModProjectile
	{
		public override string Texture => "CalamityBardHealer/Projectiles/Slash_1";
		public override string GlowTexture => "CalamityBardHealer/Projectiles/Slash_2";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 15, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("BardDamage");
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.extraUpdates = 3;
			Projectile.timeLeft = 300;
			Projectile.alpha = 255;
		}
		public override void AI() {
			if(Projectile.ai[1] < 0f) Projectile.Kill();
			else if(Projectile.ai[1] == 0f && Main.myPlayer == Projectile.owner) {
				float maxRange = 1600f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					Projectile.ai[1] = i + 1;
				}
				if(Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else if(Projectile.ai[1] > 0f) if(!Main.npc[(int)Projectile.ai[1] - 1].CanBeChasedBy(this, false)) Projectile.ai[0] = Projectile.ai[1] = 0f;
			else Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 60f, 1f) * 0.75f)) * Projectile.velocity.Length();
			if(Projectile.timeLeft % 2 == 0) ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.FlameWaders, new ParticleOrchestraSettings {PositionInWorld = Projectile.Center + Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * 0.5f, MovementVector = Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * 0.2f - Vector2.Normalize(Projectile.velocity) * 0.8f}, Projectile.owner);
			else GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center + Projectile.velocity * 0.5f, Projectile.velocity * 0.5f, Color.DarkOrange, 8, Main.rand.NextFloat(0.6f, 0.9f) * Projectile.scale, 0.9f, Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2), true, 0f, true));
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == Projectile.owner) {
				Projectile.ai[1] = -1f;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) target.AddBuff(thorium.Find<ModBuff>("Tuned").Type, 300);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Dragonfire").Type, 300);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			lightColor = Color.DarkOrange * 0.01f;
			lightColor.A = 0;
			lightColor *= Projectile.timeLeft * 0.05f;
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale * 0.9f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale * 0.9f, SpriteEffects.FlipVertically, 0);
			lightColor = Color.Gold * 0.01f;
			lightColor.A = 0;
			lightColor *= Projectile.timeLeft * 0.05f;
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale * 1.1f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.velocity.ToRotation(), texture.Size() / 2, Projectile.scale * 1.1f, SpriteEffects.FlipVertically, 0);
			return false;
		}
	}
}