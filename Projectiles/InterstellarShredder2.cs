using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class InterstellarShredder2 : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Boss/AstralLaser";
		public override string GlowTexture => "CalamityBardHealer/Projectiles/Sparkle";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
			Main.projFrames[base.Projectile.type] = 3;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetBardDefaults() {
			base.Projectile.width = 10;
			base.Projectile.height = 10;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.extraUpdates = 1;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 30;
			base.Projectile.timeLeft = 300;
			base.Projectile.light = 0.25f;
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("AstralInfectionDebuff").Type, 120);
		}
		public override void AI() {
			if(++base.Projectile.frameCounter > 2) {
				if(++base.Projectile.frame >= Main.projFrames[base.Projectile.type]) base.Projectile.frame = 0;
				base.Projectile.frameCounter = 0;
			}
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(base.Projectile.timeLeft, 15) / 15f;
			for(int i = 1; i < base.Projectile.oldPos.Length; i++) {
				int frame = base.Projectile.frame - i;
				while(frame < 0) frame += Main.projFrames[base.Projectile.type];
				Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + base.Projectile.Size * 0.5f - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[base.Projectile.type] * frame, texture.Width, texture.Height / Main.projFrames[base.Projectile.type]), new Color(175, 175, 175, 0) * MathHelper.Lerp(fade, 0f, (float)i / (float)base.Projectile.oldPos.Length), base.Projectile.rotation, new Vector2(texture.Width - texture.Height / Main.projFrames[base.Projectile.type] * 0.5f, texture.Height / Main.projFrames[base.Projectile.type] * 0.5f), base.Projectile.scale * 0.75f, SpriteEffects.None, 0);
			}
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[base.Projectile.type] * base.Projectile.frame, texture.Width, texture.Height / Main.projFrames[base.Projectile.type]), new Color(200, 200, 200, 55) * fade, base.Projectile.rotation, new Vector2(texture.Width - texture.Height / Main.projFrames[base.Projectile.type] * 0.5f, texture.Height / Main.projFrames[base.Projectile.type] * 0.5f), base.Projectile.scale * 0.75f, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 164, 94, 0) * fade, 0f, texture.Size() * 0.5f, Projectile.scale * fade, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(255, 164, 94, 0) * fade, MathHelper.PiOver4, texture.Size() * 0.5f, Projectile.scale * fade * 0.5f, SpriteEffects.None, 0);
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(++base.Projectile.ai[2] < 3f + Main.player[base.Projectile.owner].GetModPlayer<ThoriumPlayer>().bardBounceBonus) {
				if(base.Projectile.velocity.X != oldVelocity.X) base.Projectile.velocity.X = -oldVelocity.X;
				if(base.Projectile.velocity.Y != oldVelocity.Y) base.Projectile.velocity.Y = -oldVelocity.Y;
				return false;
			}
			return true;
		}
	}
}