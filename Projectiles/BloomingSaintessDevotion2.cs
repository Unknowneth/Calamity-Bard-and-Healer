using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class BloomingSaintessDevotion2 : ModProjectile
	{
		public override string GlowTexture => "CalamityBardHealer/Projectiles/BloomingSaintessDevotion3";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 10, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.timeLeft = 40;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
		}
		public override void AI() {
			if(Projectile.frame < 90) Projectile.frame += 3;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Lighting.AddLight(Projectile.Center, Color.Pink.ToVector3());
		}
		public override void OnKill(int timeLeft) {
			if(Main.myPlayer == Projectile.owner) for(int i = 0; i < 8; i++) {
				int p = Projectile.NewProjectile(base.Projectile.GetSource_Death(), Projectile.Center + Vector2.Normalize(Projectile.velocity).RotatedBy(i * MathHelper.PiOver4) * 5f, Vector2.Normalize(Projectile.velocity).RotatedBy(i * MathHelper.PiOver4) * 10f, ModContent.ProjectileType<Projectiles.BloomingSaintessDevotion3>(), Projectile.damage / 2, Projectile.knockBack, Projectile.owner);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item17, Projectile.Center, null);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.Violet;
			DrawFlower(lightColor);
			lightColor.A = 0;
			DrawFlower(lightColor);
			return false;
		}
		private void DrawFlower(Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(GlowTexture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - MathHelper.PiOver4, new Vector2(0, texture.Height), Projectile.scale * MathHelper.Clamp((float)(Projectile.frame - 10) / 10f, 0f, 1f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver4, new Vector2(0, texture.Height), Projectile.scale * MathHelper.Clamp((float)(Projectile.frame - 30) / 10f, 0f, 1f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver4 * 3f, new Vector2(0, texture.Height), Projectile.scale * MathHelper.Clamp((float)(Projectile.frame - 50) / 10f, 0f, 1f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - MathHelper.PiOver4 * 3f, new Vector2(0, texture.Height), Projectile.scale * MathHelper.Clamp((float)(Projectile.frame - 70) / 10f, 0f, 1f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(0, texture.Height), Projectile.scale * MathHelper.Clamp((float)(Projectile.frame - 20) / 10f, 0f, 1f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver2, new Vector2(0, texture.Height), Projectile.scale * MathHelper.Clamp((float)(Projectile.frame - 40) / 10f, 0f, 1f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.Pi, new Vector2(0, texture.Height), Projectile.scale * MathHelper.Clamp((float)(Projectile.frame - 60) / 10f, 0f, 1f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation - MathHelper.PiOver2, new Vector2(0, texture.Height), Projectile.scale * MathHelper.Clamp((float)(Projectile.frame - 80) / 10f, 0f, 1f), SpriteEffects.None, 0);
			texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() * 0.5f, Projectile.scale * MathHelper.Clamp((float)Projectile.frame / 10f, 0f, 1f), SpriteEffects.None, 0);
		}
		public override bool ShouldUpdatePosition() => false;
	}
}