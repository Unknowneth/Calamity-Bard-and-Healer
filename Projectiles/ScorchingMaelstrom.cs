using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class ScorchingMaelstrom : ModProjectile
	{
		public override string Texture => "Terraria/Images/Projectile_657";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = Projectile.height = 240;
			Projectile.friendly = true;
			Projectile.hostile = true;
			Projectile.aiStyle = -1;
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.ownerHitCheck = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.ArmorPenetration = 70;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			if(Projectile.timeLeft % 6 == 0) {
				Vector2 slashDir = Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver2) - MathHelper.PiOver4);
				slashDir = slashDir.RotatedBy(Main.rand.NextBool(2) ? -MathHelper.PiOver2 : MathHelper.PiOver2);
				float slashRot = (Main.rand.NextFloat(MathHelper.PiOver2) - MathHelper.PiOver4) * 0.03f;
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, slashDir.RotatedBy(slashRot) * 4f, ModContent.ProjectileType<Projectiles.DisasterSlashes>(), Projectile.damage, Projectile.knockBack, Projectile.owner, -slashRot);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			if(Projectile.velocity.X != 0) Projectile.spriteDirection = (int)Projectile.ai[0];
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 17;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(0.5f, 0f, (float)Projectile.alpha / 255f);
			for(int k = 0; k < 25; k++) {
				lightColor = Color.Lerp(Color.Red, Color.OrangeRed, (float)k / 25f) * fade;
				lightColor.A = 0;
				Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * Projectile.spriteDirection * MathHelper.TwoPi / (k / 25f + 1) * 3f + MathHelper.ToRadians(k * 30f * Projectile.spriteDirection), texture.Size() * 0.5f, Projectile.scale * 0.3f * k, Projectile.spriteDirection > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			}
			return false;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("VulnerabilityHex").Type, 120);
		}
		public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers) {
			modifiers.SourceDamage *= 0.1f;
			modifiers.FinalDamage *= 0.25f;
		}
		public override bool CanHitPlayer(Player target) => Projectile.timeLeft < 120 && target.Distance(Projectile.Center) < 120f;
	}
}