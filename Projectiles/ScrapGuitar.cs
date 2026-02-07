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
	public class ScrapGuitar : BardProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 7, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetBardDefaults() {
			base.Projectile.width = 10;
			base.Projectile.height = 10;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.extraUpdates = 3;
			base.Projectile.penetrate = -1;
			base.Projectile.timeLeft = 300;
			base.Projectile.alpha = 255;
			base.Projectile.usesIDStaticNPCImmunity = true;
			base.Projectile.idStaticNPCHitCooldown = 10;
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			if(base.Projectile.timeLeft % 2 == 0) {
				if(base.Projectile.timeLeft % 4 == 0) base.Projectile.ai[1] = Main.rand.Next(-40, 40);
				else base.Projectile.ai[1] *= -1;
			}
			base.Projectile.velocity = Vector2.UnitX.RotatedBy(base.Projectile.ai[0] + MathHelper.ToRadians(base.Projectile.ai[1])) * base.Projectile.velocity.Length();
			base.Projectile.rotation = base.Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(++base.Projectile.ai[2] < 4 + Main.player[base.Projectile.owner].GetModPlayer<ThoriumPlayer>().bardBounceBonus) {
				Vector2 bounce = base.Projectile.ai[0].ToRotationVector2();
				if(base.Projectile.velocity.X != oldVelocity.X) {
					base.Projectile.velocity.X = -oldVelocity.X;
					bounce.X *= -1f;
				}
				if(base.Projectile.velocity.Y != oldVelocity.Y) {
					base.Projectile.velocity.Y = -oldVelocity.Y;
					bounce.Y *= -1f;
				}
				base.Projectile.ai[0] = bounce.ToRotation();
				base.Projectile.velocity = Vector2.Normalize(base.Projectile.velocity) * oldVelocity.Length();
				if(Main.myPlayer == base.Projectile.owner) {
					float maxRange = 640f;
					for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(base.Projectile.Center, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(base.Projectile.Center) < maxRange) {
						maxRange = Main.npc[i].Distance(base.Projectile.Center);
						base.Projectile.ai[0] = (Main.npc[i].Center + Main.npc[i].velocity - base.Projectile.Center).ToRotation();
					}
					if(maxRange < 640f) NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
				}
				return false;
			}
			return true;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == base.Projectile.owner) if(++base.Projectile.ai[2] < 4 + Main.player[base.Projectile.owner].GetModPlayer<ThoriumPlayer>().bardBounceBonus) {
				float maxRange = 640f;
				for(int i = 0; i < Main.maxNPCs; i++) if(i != target.whoAmI && Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(base.Projectile.Center, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(base.Projectile.Center) < maxRange) {
					maxRange = Main.npc[i].Distance(base.Projectile.Center);
					base.Projectile.ai[0] = (Main.npc[i].Center + Main.npc[i].velocity - base.Projectile.Center).ToRotation();
				}
				NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int k = 0; k < base.Projectile.oldPos.Length - 1; k++) if(!base.Projectile.oldPos[k].HasNaNs() && !base.Projectile.oldPos[k + 1].HasNaNs() && base.Projectile.oldPos[k] != Vector2.Zero && base.Projectile.oldPos[k + 1] != Vector2.Zero) Main.EntitySpriteDraw(texture, Vector2.Lerp(base.Projectile.oldPos[k], base.Projectile.oldPos[k + 1], 0.5f) - Main.screenPosition, null, new Color(255, 255, 255, 0) * ((base.Projectile.oldPos.Length - k) / (float)base.Projectile.oldPos.Length), (base.Projectile.oldPos[k] - base.Projectile.oldPos[k + 1]).ToRotation(), texture.Size() / 2, new Vector2(Vector2.Distance(base.Projectile.oldPos[k], base.Projectile.oldPos[k + 1]), (base.Projectile.scale - k * 0.02f) * 1.5f), SpriteEffects.None, 0);
			return false;
		}
	}
}