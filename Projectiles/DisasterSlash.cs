using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CalamityMod.Particles;

namespace CalamityBardHealer.Projectiles
{
	public class DisasterSlash : ModProjectile
	{
		public override void SetStaticDefaults() {
			Main.projFrames[Type] = 4;
			Terraria.ID.ProjectileID.Sets.TrailCacheLength[Type] = 3;
			Terraria.ID.ProjectileID.Sets.TrailingMode[Type] = 0;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 100;
			Projectile.height = 100;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.ArmorPenetration = 70;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60;
			Projectile.extraUpdates = 2;
		}
		public override void AI() {
			if(Projectile.timeLeft % 20 == 0) {
				Vector2 slashDir = Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver2) - MathHelper.PiOver4) / 4f;
				slashDir = slashDir.RotatedBy(Main.rand.NextBool(2) ? -MathHelper.PiOver2 : MathHelper.PiOver2);
				float slashRot = (Main.rand.NextFloat(MathHelper.PiOver2) - MathHelper.PiOver4) * 0.01f;
				if(slashRot == 0f) slashRot = Main.rand.NextBool(2) ? -0.0157f : 0.0157f;
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center + Projectile.velocity - slashDir.RotatedBy(slashRot * 0.5f) * 6f, slashDir.RotatedBy(slashRot), ModContent.ProjectileType<Projectiles.DisasterSlashes>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -slashRot);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			if(++Projectile.frameCounter >= 12) Projectile.frameCounter = 0;
			Projectile.frame = (int)(Projectile.frameCounter / 3f);
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("VulnerabilityHex").Type, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Vector2 origin = new Vector2(texture.Width, texture.Height / Main.projFrames[Type]);
			Vector2 offset = Projectile.Size * 0.5f - Main.screenPosition;
			for(int i = 0; i < Projectile.oldPos.Length; i++) {
				lightColor = Color.Lerp(Color.OrangeRed, Color.DarkRed, (float)i / (float)Projectile.oldPos.Length);
				lightColor.A = 0;
				int frames = Projectile.frame - i;
				while(frames < 0) frames += Main.projFrames[Type] - 1;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + offset, new Rectangle(0, (int)origin.Y * frames - 1, texture.Width, (int)origin.Y), lightColor, Projectile.velocity.ToRotation(), origin / 2, Projectile.scale * 2f, SpriteEffects.None, 0);
			}
			return false;
		}
	}
}