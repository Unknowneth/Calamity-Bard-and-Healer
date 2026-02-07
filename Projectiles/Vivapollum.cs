using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class Vivapollum : BardProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityEntropy");
		public override string Texture => "ThoriumMod/Projectiles/Boss/BubbleBomb";
		public override string GlowTexture => "CalamityEntropy/Assets/Extra/Glow";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 64;
			base.Projectile.height = 64;
			base.Projectile.aiStyle = -1;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = false;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.extraUpdates = 2;
			base.Projectile.timeLeft = 600;
			base.Projectile.alpha = 255;
		}
		public override void AI() {
			if(base.Projectile.alpha > 0) base.Projectile.alpha -= 17;
			base.Projectile.velocity *= 1.01f;
			base.Projectile.rotation += Projectile.direction * 0.1f;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) target.AddBuff(entropy.Find<ModBuff>("LifeOppress").Type, 600);
		}
		public override void OnKill(int timeLeft) {
			if(Main.myPlayer == Projectile.owner) NetMessage.SendData(27, -1, -1, null, Projectile.NewProjectile(Projectile.GetSource_FromAI(), base.Projectile.Center, Main.rand.NextVector2CircularEdge(12f, 12f) * Main.rand.Next(9, 12) * 0.1f, ModContent.ProjectileType<Projectiles.VivapollumExplosion>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner));
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.SlateBlue * Projectile.Opacity;
			lightColor.A = 0;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() * 0.5f, base.Projectile.scale * Projectile.Opacity, SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor * 0.8f, base.Projectile.rotation, texture.Size() * 0.5f, base.Projectile.scale * Projectile.Opacity * 0.8f, SpriteEffects.None, 0);
			return false;
		}
	}
}