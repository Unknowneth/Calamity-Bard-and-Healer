using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class OldDukesWisdomTooth : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Boss/OldDukeToothBallSpike";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetBardDefaults() {
			base.Projectile.width = 10;
			base.Projectile.height = 10;
			base.Projectile.aiStyle = -1;
			base.Projectile.alpha = 255;
			base.Projectile.timeLeft = 180;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			if(base.Projectile.alpha > 0) base.Projectile.alpha -= 51;
			base.Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
		}
		public override void OnKill(int timeLeft) {
			for(int i = 0; i < 3; i++) Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 75, base.Projectile.velocity.X, base.Projectile.velocity.Y, 0, default(Color), 0.8f);
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(70, 120);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("SulphuricPoisoning").Type, 60);
		}
	}
}
