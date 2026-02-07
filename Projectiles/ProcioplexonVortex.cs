using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using CalamityEntropy;
using CalamityEntropy.Content.Particles;

namespace CalamityBardHealer.Projectiles
{
	[ExtendsFromMod("CalamityEntropy")]
	[JITWhenModsEnabled("CalamityEntropy")]
	public class ProcioplexonVortex : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityEntropy");
		public override string Texture => "ThoriumMod/Projectiles/WhirlBlast";
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 360;
			Projectile.height = 360;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.ArmorPenetration = 100;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 40;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 300;
			Projectile.manualDirectionChange = true;
		}
		public override void AI() {
			if(300 - Projectile.timeLeft - Projectile.ai[0] < 0) {
				Player player = Main.player[Projectile.owner];
				Projectile.Center = player.Center;
			}
			else if(300 - Projectile.timeLeft - Projectile.ai[0] == 0 && Main.myPlayer == Projectile.owner) {
				Projectile.velocity = Vector2.Normalize(Main.MouseWorld - Projectile.Center).RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2, MathHelper.PiOver2)) * 16f;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			else if(300 - Projectile.timeLeft - Projectile.ai[0] > 0) Projectile.velocity *= 0.97f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			CalamityEntropy.CalamityEntropy.Instance.screenShakeAmp = 2f;
			EParticle.NewParticle(new AbyssalLine(), target.Center, Vector2.Zero, Color.White, 1f, 1f, true, BlendState.Additive, CEUtils.randomRot(), -1);
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) target.AddBuff(entropy.Find<ModBuff>("LifeOppress").Type, 600);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = (float)MathHelper.Min(15, Projectile.timeLeft) / 15f;
			float scale = Projectile.scale * MathHelper.Clamp(300 - Projectile.timeLeft - Projectile.ai[0], 0f, 15f) / 15f;
			for(int k = 0; k < 20; k++) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, new Color(220, 130, 250, 0) * fade * 0.1f, (Main.GlobalTimeWrappedHourly * MathHelper.TwoPi / (k / 20f + 1) * 7.5f - MathHelper.ToRadians(k * 36f)) * Projectile.spriteDirection, texture.Size() * 0.5f, scale * 0.5f * k * fade, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanHitNPC(NPC target) => Projectile.Distance(target.Center) <= (Projectile.width + Projectile.height) / 4 * MathHelper.Clamp(300 - Projectile.timeLeft - Projectile.ai[0], 0f, 15f) / 15f ? null : false;
	}
}