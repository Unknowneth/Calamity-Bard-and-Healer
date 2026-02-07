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
	public class Supercluster : BardProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override string GlowTexture => "CatalystMod/Assets/Glow";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			Main.projFrames[base.Projectile.type] = 5;
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 28;
			base.Projectile.height = 28;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft = 300;
			base.Projectile.extraUpdates = 1;
		}
		public override void AI() {
			base.Projectile.direction = base.Projectile.velocity.X > 0 ? 1 : -1;
			if(base.Projectile.timeLeft > 210) base.Projectile.velocity = base.Projectile.velocity.RotatedBy(base.Projectile.direction * MathHelper.PiOver4 * -0.05f);
			else if(base.Projectile.ai[1] == 0f && Main.myPlayer == base.Projectile.owner) {
				float maxRange = 960f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					base.Projectile.ai[1] = i + 1;
				}
				if(base.Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
			else if(base.Projectile.ai[1] > 0f) if(!Main.npc[(int)base.Projectile.ai[1] - 1].CanBeChasedBy(this, false)) base.Projectile.ai[0] = base.Projectile.ai[1] = 0f;
			else base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(Main.npc[(int)base.Projectile.ai[1] - 1].Center - base.Projectile.Center), MathHelper.Min(++base.Projectile.ai[0] / 120f, 1f))) * base.Projectile.velocity.Length();
			if(++base.Projectile.frameCounter >= 15) base.Projectile.frameCounter = 0;
			base.Projectile.frame = (int)(base.Projectile.frameCounter / 3f);
			base.Projectile.rotation += base.Projectile.direction * 0.1f;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) target.AddBuff(catalyst.Find<ModBuff>("AstralBlight").Type, 240);
		}
		public override void OnKill(int timeLeft) {
			if(Main.myPlayer == base.Projectile.owner) for(int i = 2; i < 6; i++) {
				int p = Projectile.NewProjectile(base.Projectile.GetSource_Death(), base.Projectile.Center, Main.rand.NextVector2CircularEdge(i, i), ModContent.ProjectileType<Projectiles.SuperclusterStar>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, base.Projectile.Center, null);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			float fade = (float)MathHelper.Min(base.Projectile.timeLeft, 15) / 15f;
			lightColor = new Color(220, 95, 210, 0) * fade;
			Vector2 origin = texture.Size() * 0.5f;
			for(int k = 1; k < base.Projectile.oldPos.Length; k++) Main.EntitySpriteDraw(texture, base.Projectile.oldPos[k] + base.Projectile.Size * 0.5f - Main.screenPosition, null, Color.Lerp(lightColor, new Color(55, 55, 55, 0), (float)k / (float)base.Projectile.oldPos.Length), base.Projectile.oldRot[k], origin, base.Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, origin, base.Projectile.scale * fade, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = new Color(255, 255, 255, 0) * fade;
			origin = new Vector2(texture.Width, texture.Height / Main.projFrames[base.Projectile.type]) * 0.5f;
			for(int k = 1; k < base.Projectile.oldPos.Length; k++) {
				int frames = base.Projectile.frame - k;
				while(frames < 0) frames += Main.projFrames[base.Projectile.type];
				Main.EntitySpriteDraw(texture, base.Projectile.oldPos[k] + base.Projectile.Size * 0.5f - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[base.Projectile.type] * frames, texture.Width, texture.Height / Main.projFrames[base.Projectile.type]), Color.Lerp(lightColor, new Color(55, 55, 55, 0), (float)k / (float)base.Projectile.oldPos.Length), base.Projectile.oldRot[k], origin, base.Projectile.scale * MathHelper.Lerp(1f, 0.5f, (float)k / (float)base.Projectile.oldPos.Length) * fade, SpriteEffects.None, 0);
			}
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[base.Projectile.type] * base.Projectile.frame, texture.Width, texture.Height / Main.projFrames[base.Projectile.type]), lightColor, base.Projectile.rotation, origin, base.Projectile.scale * fade, SpriteEffects.None, 0);
			return false;
		}
	}
}