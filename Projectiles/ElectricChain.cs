using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class ElectricChain : ModProjectile
	{
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 7, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 4;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 180;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
		}
		public override void AI() {
			if(Projectile.timeLeft % 2 == 0) {
				if(Projectile.timeLeft % 4 == 0) Projectile.ai[1] = Main.rand.Next(-40, 40);
				else Projectile.ai[1] *= -1;
			}
			Projectile.velocity = Vector2.UnitX.RotatedBy(Projectile.ai[0] + MathHelper.ToRadians(Projectile.ai[1])) * Projectile.velocity.Length();
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == Projectile.owner) {
				float maxRange = 800f;
				for(int i = 0; i < Main.maxNPCs; i++) if(i != target.whoAmI && Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Projectile.Center, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(Projectile.Center) < maxRange) {
					maxRange = Main.npc[i].Distance(Projectile.Center);
					Projectile.ai[0] = (Main.npc[i].Center + Main.npc[i].velocity - Projectile.Center).ToRotation();
				}
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int k = 0; k < Projectile.oldPos.Length - 1; k++) if(!Projectile.oldPos[k].HasNaNs() && !Projectile.oldPos[k + 1].HasNaNs() && Projectile.oldPos[k] != Vector2.Zero && Projectile.oldPos[k + 1] != Vector2.Zero) Main.EntitySpriteDraw(texture, Vector2.Lerp(Projectile.oldPos[k], Projectile.oldPos[k + 1], 0.5f) - Main.screenPosition, null, new Color(255, 255, 255, 0) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length), (Projectile.oldPos[k] - Projectile.oldPos[k + 1]).ToRotation(), texture.Size() / 2, new Vector2(Vector2.Distance(Projectile.oldPos[k], Projectile.oldPos[k + 1]), (Projectile.scale - k * 0.02f) * 1.5f), SpriteEffects.None, 0);
			return false;
		}
	}
}