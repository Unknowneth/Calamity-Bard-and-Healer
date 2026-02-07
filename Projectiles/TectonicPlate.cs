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
	public class TectonicPlate : BardProjectile
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 5, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 28;
			base.Projectile.height = 28;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 10;
			base.Projectile.timeLeft = 60;
			base.Projectile.extraUpdates = 2;
			base.Projectile.ArmorPenetration = 25;
		}
		public override void AI() {
			if(base.Projectile.localAI[1] == 0f && base.Projectile.localAI[2] == 0f) {
				base.Projectile.localAI[1] = base.Projectile.Center.X;
				base.Projectile.localAI[2] = base.Projectile.Center.Y;
			}
			base.Projectile.Center = Vector2.Lerp(new Vector2(base.Projectile.ai[1], base.Projectile.ai[2]), new Vector2(base.Projectile.localAI[1], base.Projectile.localAI[2]), (float)base.Projectile.timeLeft / 60f) + base.Projectile.velocity.RotatedBy(MathHelper.PiOver2 * Projectile.ai[0]) * Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)base.Projectile.timeLeft / 60f).Y * 160f;
			base.Projectile.rotation += Projectile.ai[0];

		}
		public override void OnKill(int timeLeft) {
			if(base.Projectile.ai[0] > 0f) {
				if(Main.myPlayer != base.Projectile.owner) return;
				int p = Projectile.NewProjectile(base.Projectile.GetSource_Death(), base.Projectile.Center, base.Projectile.velocity, ModContent.ProjectileType<Projectiles.TectonicCollision>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner);
				NetMessage.SendData(27, -1, -1, null, p);
				return;
			}
			for(int j = 0; j < 25; j++) {
				int d = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 6, 0f, 0f, 100, j % 2 == 0 ? Color.OrangeRed : Color.DarkBlue, 1.7f);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity *= 5f;
				d = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 6, 0f, 0f, 100, j % 2 != 0 ? Color.OrangeRed : Color.DarkBlue, 1f);
				Main.dust[d].velocity *= 2f;
			}
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, base.Projectile.Center, null);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 1; i < base.Projectile.oldPos.Length; i++) {
				float colorLerp = (float)i / (float)base.Projectile.oldPos.Length;
				colorLerp *= colorLerp;
				lightColor = Color.Lerp(Color.OrangeRed, Color.Blue, colorLerp) * MathHelper.Lerp(0.8f * (float)MathHelper.Min(15, base.Projectile.timeLeft) / 15f, 0f, colorLerp);
				lightColor.A = 0;
				Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + base.Projectile.Size * 0.5f - Main.screenPosition, null, lightColor, base.Projectile.oldRot[i], texture.Size() / 2, base.Projectile.scale, SpriteEffects.None, 0);
			}
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, Color.White, base.Projectile.velocity.ToRotation() + base.Projectile.rotation, texture.Size() / 2, base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override bool ShouldUpdatePosition() => false;
	}
}