using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class SingularityStar : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override string Texture => "CalamityBardHealer/Projectiles/Sparkle";
		public override string GlowTexture => "CatalystMod/Assets/Glow";
		public override void SetStaticDefaults() {
			Terraria.ID.ProjectileID.Sets.TrailCacheLength[Type] = 14;
			Terraria.ID.ProjectileID.Sets.TrailingMode[Type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.ArmorPenetration = 100;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 28;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 2;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			if(Projectile.ai[1] == 0f && Main.myPlayer == Projectile.owner) {
				float maxRange = 960f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					Projectile.ai[1] = i + 1;
				}
				if(Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else if(Projectile.ai[1] > 0f) if(!Main.npc[(int)Projectile.ai[1] - 1].CanBeChasedBy(this, false)) Projectile.ai[0] = Projectile.ai[1] = 0f;
			else Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 120f, 1f))) * Projectile.velocity.Length();
			int dustType = 6;
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) dustType = catalyst.Find<ModDust>("MonoDust2").Type;
			if(Projectile.timeLeft % Projectile.MaxUpdates == 0) for(int i = 0; i < 2; i++) if(!Main.rand.NextBool(3)) {
				float mult = (float)i * 0.5f;
				Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(Projectile.width / 2, Projectile.height / 2) + Projectile.velocity * mult, dustType, null, 0, default(Color), 1f);
				dust.noGravity = true;
				dust.velocity *= 0.1f;
				dust.scale = 0.9f;
				dust.fadeIn = 0.1f;
				dust.alpha = 100;
				dust.color = (!Main.rand.NextBool(3) ? new Color(255, 233, 2, 50) : new Color(220, 95, 210, 50));
				if(Projectile.oldVelocity != Vector2.Zero) dust.velocity -= Vector2.Normalize(Projectile.oldVelocity);
			}
			Projectile.rotation += Projectile.velocity.X < 0 ? -1f : 1f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == Projectile.owner) {
				Projectile.ai[0] = 0f;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) target.AddBuff(catalyst.Find<ModBuff>("AstralBlight").Type, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			float fade = (float)MathHelper.Min(15, Projectile.timeLeft) / 15f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Vector2 origin = texture.Size() / 2f;
			int trailLength = Projectile.oldPos.Length;
			for(int i = 0; i < trailLength; i++) Main.spriteBatch.Draw(texture, Projectile.oldPos[i] + Projectile.Size * 0.5f - Main.screenPosition, null, new Color(220, 95, 210, 0) * (1f - 1f / (float)trailLength * (float)i) * fade, Projectile.rotation, origin, Projectile.scale * (1f - 1f / (float)trailLength * (float)i) * fade, 0, 0f);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			origin = texture.Size() / 2f;
			lightColor = new Color(255, 233, 2, 0) * fade; 
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, origin, Projectile.scale * fade, SpriteEffects.None, 0);
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			bool trailHit = false;
			for(int i = 0; i < Projectile.oldPos.Length; i++) trailHit |= targetHitbox.Intersects(new Rectangle((int)Projectile.oldPos[i].X, (int)Projectile.oldPos[i].Y, projHitbox.Width, projHitbox.Height));
			return projHitbox.Intersects(targetHitbox) || trailHit;
		}
	}
}