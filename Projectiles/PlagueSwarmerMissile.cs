using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class PlagueSwarmerMissile : BardProjectile
	{
		public override string GlowTexture => "Terraria/Images/Extra_98";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 11, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 14;
			base.Projectile.height = 14;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = true;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 40;
			base.Projectile.timeLeft = 300;
			base.Projectile.extraUpdates = 2;
			base.Projectile.alpha = 255;
			base.Projectile.ArmorPenetration = 20;
		}
		public override void AI() {
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
			if(base.Projectile.ai[2] <= 0f) {
				if(base.Projectile.ai[0] > 0f && base.Projectile.Distance(Main.npc[(int)base.Projectile.ai[0] - 1].Center) > 16f && Main.npc[(int)base.Projectile.ai[0] - 1].active) base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(Main.npc[(int)base.Projectile.ai[0] - 1].Center - base.Projectile.Center), MathHelper.Min(++base.Projectile.ai[1] / 120f, 1f))) * base.Projectile.velocity.Length();
				else base.Projectile.ai[1] = 0f;
				base.Projectile.tileCollide = base.Projectile.ai[1] == 0f;
				return;
			}
			else if(Main.myPlayer == base.Projectile.owner && base.Projectile.ai[0] == 0f) {
				float maxRange = 160f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Main.MouseWorld, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					base.Projectile.ai[0] = i + 1;
				}
				if(base.Projectile.ai[0] > 0f && Main.npc[(int)base.Projectile.ai[0] - 1].active) {
					NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
					return;
				}
				else if(base.Projectile.Distance(Main.MouseWorld) < 16f) {
					base.Projectile.ai[0] = 0f;
					base.Projectile.ai[1] = 0f;
					base.Projectile.ai[2] = 0f;
					NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
					return;
				}
				else base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Main.MouseWorld - base.Projectile.Center), Vector2.Normalize(base.Projectile.velocity), --base.Projectile.ai[2] / 60f)) * base.Projectile.velocity.Length();
				NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
			else if(base.Projectile.ai[0] > 0f && Main.npc[(int)base.Projectile.ai[0] - 1].active) base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Main.npc[(int)base.Projectile.ai[0] - 1].Center - base.Projectile.Center), Vector2.Normalize(base.Projectile.velocity), --base.Projectile.ai[2] / 60f)) * base.Projectile.velocity.Length();
			if(base.Projectile.ai[2] <= 0f) base.Projectile.ai[0] = 0f;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			base.Projectile.velocity = base.Projectile.velocity.RotatedBy(Main.rand.Next(-10, 11) * 0.0157f);
			base.Projectile.ai[0] = target.whoAmI + 1;
			base.Projectile.ai[1] = 0f;
			base.Projectile.ai[2] = 0f;
			NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Plague").Type, 120);
		}
		public override void OnKill(int timeLeft) {
			for(int j = 0; j < 15; j++) {
				int d = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 89, 0f, 0f, 100, default(Color), 1.7f);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 5f;
				d = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 89, 0f, 0f, 100, default(Color), 1f);
				Main.dust[d].velocity *= 2f;
			}
			Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item14, base.Projectile.Center, null);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Texture2D glowTexture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(glowTexture, base.Projectile.Center - base.Projectile.rotation.ToRotationVector2() * texture.Width * 0.5f - Main.screenPosition, new Rectangle(0, glowTexture.Height / 2, glowTexture.Width, glowTexture.Height / 2), new Color(231, 220, 90, 0), base.Projectile.rotation + MathHelper.PiOver2, new Vector2(glowTexture.Width / 2, 0), new Vector2(0.8f, 1.2f - Vector2.UnitY.RotatedBy((Projectile.whoAmI + Main.GlobalTimeWrappedHourly) * MathHelper.TwoPi).X * 0.1f) * base.Projectile.scale * 0.6f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(glowTexture, base.Projectile.Center - base.Projectile.rotation.ToRotationVector2() * texture.Width * 0.5f - Main.screenPosition, new Rectangle(0, glowTexture.Height / 2, glowTexture.Width, glowTexture.Height / 2), new Color(79, 100, 95, 0), base.Projectile.rotation + MathHelper.PiOver2, new Vector2(glowTexture.Width / 2, 0), new Vector2(0.8f, 1.2f + Vector2.UnitY.RotatedBy((Projectile.whoAmI + Main.GlobalTimeWrappedHourly) * MathHelper.TwoPi).Y * 0.1f) * base.Projectile.scale * 0.4f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}