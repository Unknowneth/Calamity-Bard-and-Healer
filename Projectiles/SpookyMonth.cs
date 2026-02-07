using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class SpookyMonth : BardProjectile
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 6, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 40;
			base.Projectile.height = 40;
			base.Projectile.aiStyle = -1;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.extraUpdates = 2;
			base.Projectile.timeLeft = 180;
		}
		public override void AI() {
			int who = 0;
			float maxRange = 1600f;
			for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(base.Projectile.Center, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(base.Projectile.Center) < maxRange) {
				maxRange = Main.npc[i].Distance(base.Projectile.Center);
				who = i + 1;
			}
			if(base.Projectile.ai[2] < 60f) base.Projectile.ai[2]++;
			if(base.Projectile.ai[1] < -20f) base.Projectile.ai[1] = 20f;
			else if(base.Projectile.ai[1] < 20f) base.Projectile.ai[1]++;
			base.Projectile.velocity = base.Projectile.velocity.RotatedBy(MathHelper.ToRadians(base.Projectile.ai[0]) * MathHelper.Lerp(1f, 0f, base.Projectile.ai[2] / 60f) * base.Projectile.ai[1] / 20f);
			if(who > 0) base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(Main.npc[who - 1].Center - base.Projectile.Center), base.Projectile.ai[2] / 60f * 0.5f)) * base.Projectile.velocity.Length();
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
			base.Projectile.spriteDirection = base.Projectile.velocity.X > 0 ? 1 : -1;
			GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(base.Projectile.Center + base.Projectile.velocity * 0.5f, base.Projectile.velocity * 0.5f, Color.DarkOrange, 10, Main.rand.NextFloat(0.7f, 0.9f) * base.Projectile.scale, 0.5f, Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4), true, 0f, true));
		}
		/*public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Plague").Type, 180);
		}*/
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)base.Projectile.alpha / 255f) * (MathHelper.Min(base.Projectile.timeLeft, 15) / 15);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, Color.White * fade, base.Projectile.rotation + MathHelper.PiOver2 * (base.Projectile.spriteDirection - 1), texture.Size() * 0.5f, base.Projectile.scale, base.Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}