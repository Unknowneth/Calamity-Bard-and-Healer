using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class CalamityBellRing : ModProjectile
	{
		public override string Texture => "CalamityMod/Particles/HighResHollowCircleHardEdge";
		public override void SetStaticDefaults() {
			Terraria.ID.ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2048;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 1;
			Projectile.height = 1;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("TrueDamage");
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.timeLeft = 40;
		}
		public override bool PreAI() {
			if(Projectile.timeLeft < 40) return true;
			SoundEngine.PlaySound(new SoundStyle("CalamityBardHealer/Sounds/CalamityBell"), Projectile.Center);
			Main.instance.CameraModifiers.Add(new Terraria.Graphics.CameraModifiers.PunchCameraModifier(Projectile.Center, Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), 16f, 8, 60, 160f * Projectile.ai[0], "Calamity Bell Rings"));
			return false;
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.Violet;
			lightColor.A = 0;
			lightColor *= Projectile.timeLeft * 0.025f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2, MathHelper.SmoothStep(Projectile.ai[0] / 16f, 0f, Projectile.timeLeft * 0.025f), SpriteEffects.None, 0);
			return false;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) {
			float size = MathHelper.SmoothStep(Projectile.ai[0] * 64f, 0f, Projectile.timeLeft * 0.025f);
			hitbox = new Rectangle((int)(Projectile.Center.X - size), (int)(Projectile.Center.Y - size), (int)(size * 2f), (int)(size * 2f));
		}
		public override bool? CanHitNPC(NPC target) => target.Distance(Projectile.Center) > MathHelper.SmoothStep(Projectile.ai[0] * 64f, 0f, Projectile.timeLeft * 0.025f) ? false : null;
		public override bool ShouldUpdatePosition() => false;
	}
}