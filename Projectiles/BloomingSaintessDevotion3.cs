using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class BloomingSaintessDevotion3 : ModProjectile
	{
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 10, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.extraUpdates = 1;
			Projectile.alpha = 255;
			Projectile.timeLeft = 60;
		}
		public override void AI() {
			if(Projectile.ai[1] == 0f) {
				if(Main.myPlayer == Projectile.owner && Projectile.timeLeft > 5) {
					float maxRange = 160f;
					for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Main.npc[i].Distance(Projectile.Center) < maxRange) {
						maxRange = Main.npc[i].Distance(Projectile.Center);
						Projectile.ai[1] = i + 1;
					}
					if(Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
				}
				if(Projectile.ai[1] == 0f && Projectile.timeLeft < 50 && Projectile.timeLeft > 5) Projectile.timeLeft = 5;
			}
			else if(!Main.npc[(int)Projectile.ai[1] - 1].CanBeChasedBy(this, false)) Projectile.ai[0] = Projectile.ai[1] = 0f;
			else Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[(int)Projectile.ai[1] - 1].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[0] / 60f, 1f) * 0.75f)) * Projectile.velocity.Length() * 1.005f;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.Violet * (MathHelper.Min(Projectile.timeLeft, 15) / 15f);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver4, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver4, texture.Size() * 0.5f, Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}