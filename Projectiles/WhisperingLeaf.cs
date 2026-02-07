using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class WhisperingLeaf : BardProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_227";
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 10, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 18;
			base.Projectile.height = 18;
			base.Projectile.aiStyle = -1;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = false;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.extraUpdates = 1;
			base.Projectile.alpha = 255;
			base.Projectile.timeLeft = 300;
		}
		public override void AI() {
			if(base.Projectile.alpha > 0) base.Projectile.alpha -= 15;
			if(base.Projectile.timeLeft > 240) {
				base.Projectile.rotation = base.Projectile.velocity.ToRotation();
				return;
			}
			if(base.Projectile.ai[1] == 0f && Main.myPlayer == base.Projectile.owner) {
				float maxRange = 640f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Main.npc[i].Distance(base.Projectile.Center) < maxRange) {
					maxRange = Main.npc[i].Distance(base.Projectile.Center);
					base.Projectile.ai[1] = i + 1;
				}
				if(base.Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
			else if(base.Projectile.ai[1] > 0f) if(!Main.npc[(int)base.Projectile.ai[1] - 1].CanBeChasedBy(this, false)) base.Projectile.ai[0] = base.Projectile.ai[1] = 0f;
			else base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(Main.npc[(int)base.Projectile.ai[1] - 1].Center - base.Projectile.Center), MathHelper.Min(++base.Projectile.ai[0] / 60f, 1f) * 0.75f)) * base.Projectile.velocity.Length();
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)base.Projectile.alpha / 255f) * (MathHelper.Min(base.Projectile.timeLeft, 30) / 30);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, Color.Gold * fade, base.Projectile.rotation, texture.Size() * 0.5f, base.Projectile.scale * fade * 0.8f, SpriteEffects.FlipHorizontally, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, new Color(200, 200, 150, 50) * fade, base.Projectile.rotation, texture.Size() * 0.5f, base.Projectile.scale * fade * 0.8f, SpriteEffects.FlipHorizontally, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => base.Projectile.timeLeft < 240;
	}
}