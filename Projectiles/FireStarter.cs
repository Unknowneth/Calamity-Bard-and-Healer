using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class FireStarter : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.extraUpdates = 6;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.timeLeft = 120;
			Projectile.ArmorPenetration = 15;
		}
		public override void AI() {
			if(Projectile.ai[0] == 0f && Main.myPlayer == Projectile.owner && Projectile.timeLeft % 4 == 0) {
				Vector2 slashDir = Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver2) - MathHelper.PiOver4) / 4f;
				float slashRot = (Main.rand.NextFloat(MathHelper.PiOver2) - MathHelper.PiOver4) * 0.01f;
				if(slashRot == 0f) slashRot = Main.rand.NextBool(2) ? -0.0157f : 0.0157f;
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - slashDir.RotatedBy(slashRot * 0.5f) * 6f, slashDir.RotatedBy(slashRot), Type, Projectile.damage, Projectile.knockBack, Projectile.owner, -slashRot);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			else Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("BrimstoneFlames").Type, 120);
		}
		public override bool PreDraw(ref Color lightColor) {
			if(Projectile.ai[0] == 0f) return false;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)Projectile.alpha / 255f) * (MathHelper.Min(Projectile.timeLeft, 15) / 30);
			if(Projectile.velocity != Vector2.Zero) for(int i = 0; i < Projectile.oldPos.Length; i++) {
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 + Main.rand.NextVector2Circular(i, i) / (float)Projectile.oldPos.Length - Main.screenPosition, null, new Color(255, 0, 0, 0) * fade, Projectile.oldRot[i] + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(MathHelper.Lerp(Projectile.scale * 0.5f, 0.03f, (float)i / (float)Projectile.oldPos.Length)), SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 + Main.rand.NextVector2Circular(i, i) / (float)Projectile.oldPos.Length - Main.screenPosition, null, new Color(255, 140, 0, 0) * fade, Projectile.oldRot[i] + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(MathHelper.Lerp(Projectile.scale * 0.3f, 0.01f, (float)i / (float)Projectile.oldPos.Length)), SpriteEffects.None, 0);
			}
			return false;
		}
	}
}