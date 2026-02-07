using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class Gashadokuro : BardProjectile
	{
		public override string Texture => "CalamityMod/Particles/HighResHollowCircleHardEdge";
		public override void SetStaticDefaults() {
			Terraria.ID.ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2048;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetBardDefaults() {
			base.Projectile.width = 1;
			base.Projectile.height = 1;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.tileCollide = false;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = -1;
			base.Projectile.timeLeft = 20;
		}
		public override bool PreAI() {
			if(base.Projectile.timeLeft < 20) return true;
			Vector2 shootDir = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2();
			for(int i = 0; i < 3; i++) {
				int p = Projectile.NewProjectile(base.Projectile.GetSource_FromAI(), base.Projectile.Center, shootDir.RotatedBy(MathHelper.ToRadians(120f) * i) * 4f, ModContent.ProjectileType<Projectiles.GashadokuroCracks>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, Main.rand.Next(3) + 1);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			Main.instance.CameraModifiers.Add(new Terraria.Graphics.CameraModifiers.PunchCameraModifier(base.Projectile.Center, Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2(), 16f, 8, 20, 80f * base.Projectile.ai[0], "Calamity Bell Rings"));
			return false;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Vector2 shootDir = Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2();
			for(int i = 0; i < 3; i++) {
				int p = Projectile.NewProjectile(base.Projectile.GetSource_FromAI(), target.Center, shootDir.RotatedBy(MathHelper.ToRadians(120f) * i) * 4f, ModContent.ProjectileType<Projectiles.GashadokuroCracks>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, Main.rand.Next(3) + 1);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("VulnerabilityHex").Type, 600);
		}
		public override bool PreDraw(ref Color lightColor) {
			lightColor = Color.OrangeRed;
			lightColor.A = 0;
			lightColor *= base.Projectile.timeLeft * 0.05f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, Main.rand.NextFloat(MathHelper.TwoPi), texture.Size() / 2, MathHelper.SmoothStep(1f, 0f, base.Projectile.timeLeft * 0.05f), SpriteEffects.None, 0);
			return false;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) {
			float size = MathHelper.SmoothStep(1024f, 0f, base.Projectile.timeLeft * 0.05f);
			hitbox = new Rectangle((int)(base.Projectile.Center.X - size), (int)(base.Projectile.Center.Y - size), (int)(size * 2f), (int)(size * 2f));
		}
		public override bool? CanHitNPC(NPC target) => target.Distance(base.Projectile.Center) > MathHelper.SmoothStep(1024f, 0f, base.Projectile.timeLeft * 0.05f) ? false : null;
		public override bool ShouldUpdatePosition() => false;
	}
}