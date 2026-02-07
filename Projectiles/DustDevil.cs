using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class DustDevil : BardProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("InfernumMode");
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 6;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 5, base.Projectile.type);
			mor.Call("addElementProj", 6, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 360;
			Projectile.alpha = 255;
			Projectile.DamageType = BardDamage.Instance;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.extraUpdates = 1;
		}
		public override void AI() {
			Projectile.frameCounter++;
			Projectile.frame = Projectile.frameCounter / 5 % Main.projFrames[Type];
			Projectile.rotation = Projectile.velocity.X * 0.01f;
			if(Projectile.timeLeft < 8) Projectile.Opacity = MathHelper.Clamp(Projectile.Opacity - 0.125f, 0f, 1f);
			else Projectile.Opacity = MathHelper.Clamp(Projectile.Opacity + 0.125f, 0f, 1f);
			if(Projectile.ai[0] == -1f) Projectile.velocity *= 0.94f;
			else if(Projectile.ai[2] == 1f) {
				float maxRange = 400f;
				int target = -1;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Main.npc[i].Distance(Projectile.Center) < maxRange) {
					maxRange = Main.npc[i].Distance(Projectile.Center);
					target = i;
				}
				if(target >= 0) Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[target].Center - Projectile.Center), 0.1f)) * Projectile.velocity.Length();
			}
			if(Projectile.owner != Main.myPlayer || Projectile.velocity.Length() > 1f) return;
			Projectile.velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center) * Projectile.ai[1];
			Projectile.ai[0] = 0f;
			Projectile.ai[1] = 0f;
			NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawPosition = Projectile.Center - Main.screenPosition;
			Rectangle frame = texture.Frame(1, Main.projFrames[Type], 0, Projectile.frame);
			Vector2 origin = frame.Size() * 0.5f;
			Color mainBackColor = Projectile.ai[2] == 1f ? new Color(0f, 0.9f, 1f, 0f) : new Color(1f, 1f, 1f, 0f);
			Color backlightColor = Projectile.GetAlpha(mainBackColor) * Lighting.Brightness((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f)) * 0.65f;
			for(int i = 0; i < 6; i++) Main.spriteBatch.Draw(texture, drawPosition + (MathHelper.TwoPi * i / 6f).ToRotationVector2() * (Projectile.ai[2] == 1f ? 5f : 3f), frame, backlightColor, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(texture, drawPosition, frame, Projectile.GetAlpha(Projectile.ai[2] == 1f ? Color.Cyan : lightColor), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
	}
}
