using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class OmnicidesLaw : ModProjectile
	{
		public override string GlowTexture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			Main.projFrames[Projectile.type] = 4;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 11, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 28;
			Projectile.height = 28;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 240;
			Projectile.ArmorPenetration = 40;
		}
		public override void AI() {
			int who = 0;
			float maxRange = 400f;
			for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Projectile.Center, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Projectile.Center) < maxRange) {
				maxRange = Main.npc[i].Distance(Projectile.Center);
				who = i + 1;
			}
			if(Projectile.ai[2] < 120f) Projectile.ai[2]++;
			if(Projectile.ai[1] < -20f) Projectile.ai[1] = 20f;
			else if(Projectile.ai[1] < 20f) Projectile.ai[1]++;
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[0]) * MathHelper.Lerp(1f, 0f, Projectile.ai[2] / 120f) * Projectile.ai[1] / 20f);
			if(who > 0) Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[who - 1].Center - Projectile.Center), Projectile.ai[2] / 120f * 0.25f)) * Projectile.velocity.Length();
			if(++Projectile.frameCounter > 6) {
				if(++Projectile.frame > Main.projFrames[Projectile.type] - 1) Projectile.frame = 0;
				Projectile.frameCounter = 0;
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.spriteDirection = Projectile.velocity.X > 0 ? 1 : -1;
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(Projectile.Center + Projectile.velocity * 0.5f, Projectile.velocity * 0.5f, new Color(154, 186, 74, 255), 5, Main.rand.NextFloat(0.7f, 0.9f) * Projectile.scale, 0.5f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4), true, 0f, true));
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Plague").Type, 180);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)Projectile.alpha / 255f) * (MathHelper.Min(Projectile.timeLeft, 15) / 15);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), Color.White * fade, Projectile.rotation + MathHelper.PiOver2 * (Projectile.spriteDirection - 1), new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) * 0.5f, Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}