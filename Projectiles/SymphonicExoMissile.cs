using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class SymphonicExoMissile : BardProjectile
	{
		public override string GlowTexture => "Terraria/Images/Extra_98";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 6, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 18;
			base.Projectile.height = 18;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 30;
			base.Projectile.timeLeft = 300;
			base.Projectile.extraUpdates = 2;
			base.Projectile.alpha = 255;
		}
		public override void AI() {
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
			if(base.Projectile.timeLeft < 270 || base.Projectile.ai[0] > 0f) {
				if(base.Projectile.ai[0] > 0f && base.Projectile.Distance(Main.npc[(int)base.Projectile.ai[0] - 1].Center) > 16f && Main.npc[(int)base.Projectile.ai[0] - 1].active) base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(Main.npc[(int)base.Projectile.ai[0] - 1].Center - base.Projectile.Center), MathHelper.Min(++base.Projectile.ai[1] / 120f, 1f))) * base.Projectile.velocity.Length();
				else base.Projectile.ai[1] = 0f;
				return;
			}
			else if(Main.myPlayer == base.Projectile.owner) {
				float maxRange = 1600f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					base.Projectile.ai[0] = i + 1;
				}
				if(base.Projectile.ai[0] > 0f && Main.npc[(int)base.Projectile.ai[0] - 1].active) {
					NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
					return;
				}
				else if(base.Projectile.Distance(Main.MouseWorld) > 16f) base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Main.MouseWorld - base.Projectile.Center), Vector2.Normalize(base.Projectile.velocity), (--base.Projectile.timeLeft - 270f) / 30f)) * base.Projectile.velocity.Length();
				NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			base.Projectile.velocity = base.Projectile.velocity.RotatedBy(Main.rand.Next(-10, 11) * 0.0157f);
			base.Projectile.ai[0] = target.whoAmI + 1;
			base.Projectile.ai[1] = 0f;
			NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("MiracleBlight").Type, 600);
		}
		/*public override void OnKill(int timeLeft) {
			for(int j = 0; j < 15; j++) {
				int d = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 89, 0f, 0f, 100, default(Color), 1.7f);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 5f;
				d = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 89, 0f, 0f, 100, default(Color), 1f);
				Main.dust[d].velocity *= 2f;
			}
			Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item14, base.Projectile.Center, null);
		}*/
		public override bool PreDraw(ref Color lightColor) {
			switch(base.Projectile.ai[2]) {
				case 0:
					lightColor = new Color(255, 0, 0, 0);
				break;
				case 1:
					lightColor = new Color(255, 135, 40, 0);
				break;
				case 2:
					lightColor = new Color(250, 215, 0, 0);
				break;
				case 3:
					lightColor = new Color(175, 255, 25, 0);
				break;
				case 4:
					lightColor = new Color(110, 255, 0, 0);
				break;
				case 5:
					lightColor = new Color(0, 255, 255, 0);
				break;
				case 6:
					lightColor = new Color(50, 75, 255, 0);
				break;
				case 7:
					lightColor = new Color(50, 150, 255, 0);
				break;
				case 8:
					lightColor = new Color(125, 75, 255, 0);
				break;
				case 9:
					lightColor = new Color(255, 125, 255, 0);
				break;
				case 10:
					lightColor = new Color(200, 25, 255, 0);
				break;
			}
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(glowTexture, base.Projectile.Center - base.Projectile.rotation.ToRotationVector2() * texture.Width * 0.5f - Main.screenPosition, new Rectangle(0, glowTexture.Height / 2, glowTexture.Width, glowTexture.Height / 2), lightColor, base.Projectile.rotation + MathHelper.PiOver2, new Vector2(glowTexture.Width / 2, 0), new Vector2(0.8f, 1.6f - Vector2.UnitY.RotatedBy((Projectile.whoAmI + Main.GlobalTimeWrappedHourly) * MathHelper.TwoPi).X * 0.1f) * base.Projectile.scale * 0.9f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(glowTexture, base.Projectile.Center - base.Projectile.rotation.ToRotationVector2() * texture.Width * 0.5f - Main.screenPosition, new Rectangle(0, glowTexture.Height / 2, glowTexture.Width, glowTexture.Height / 2), new Color(100, 100, 100, 0), base.Projectile.rotation + MathHelper.PiOver2, new Vector2(glowTexture.Width / 2, 0), new Vector2(0.8f, 1.6f + Vector2.UnitY.RotatedBy((Projectile.whoAmI + Main.GlobalTimeWrappedHourly) * MathHelper.TwoPi).Y * 0.1f) * base.Projectile.scale * 0.7f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, Color.White, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}