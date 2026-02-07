using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class CottonMouthGas : ModProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Rogue/ScourgeVenomCloud";
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 10;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
			mor.Call("addElementProj", 11, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.ArmorPenetration = 20;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
			Projectile.timeLeft = 120;
		}
		public override void AI() {
			if(++Projectile.frameCounter >= 12) Projectile.frameCounter = 0;
			Projectile.frame = (int)MathHelper.Lerp(Main.projFrames[Type], 1f, (float)Projectile.timeLeft / 120f) - 1;
			Projectile.velocity *= 0.96f;
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.direction != 0) Projectile.direction = Projectile.velocity.X > 0 ? 1 : -1;
			Projectile.spriteDirection = Projectile.direction;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(70, 120);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("SulphuricPoisoning").Type, 60);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = new Color(255, 125, 0, 0);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / Main.projFrames[Projectile.type] * Projectile.frame, texture.Width, texture.Height / Main.projFrames[Projectile.type]), lightColor, Projectile.rotation - (Projectile.spriteDirection - 1) * MathHelper.PiOver2, new Vector2(texture.Width, texture.Height / Main.projFrames[Projectile.type]) / 2, Projectile.scale * MathHelper.Lerp(1.5f, 0.5f, (float)Projectile.timeLeft / 120f), Projectile.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
	}
}