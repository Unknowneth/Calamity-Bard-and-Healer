using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class OldDukesWisdom : BardProjectile
	{
		public override string Texture => "CalamityMod/NPCs/OldDuke/OldDukeToothBall";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetBardDefaults() {
			base.Projectile.width = 40;
			base.Projectile.height = 40;
			base.Projectile.aiStyle = -1;
			base.Projectile.alpha = 255;
			base.Projectile.timeLeft = 180;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.extraUpdates = 1;
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			width = height = 26;
			return true;
		}
		public override void AI() {
			if(base.Projectile.alpha > 0) base.Projectile.alpha -= 51;
			base.Projectile.velocity *= 0.9875f;
			base.Projectile.rotation += System.Math.Sign(base.Projectile.velocity.X) * 0.1f;
		}
		public override void OnKill(int timeLeft) {
			for(int i = 0; i < 6; i++) Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 75, 0f, 0f, 0, default(Color), 1f);
			if(Main.myPlayer == base.Projectile.owner) for(int i = 0; i < Main.rand.Next(2, 4); i++) NetMessage.SendData(27, -1, -1, null, Projectile.NewProjectile(Projectile.GetSource_FromAI(), base.Projectile.Center, Main.rand.NextVector2CircularEdge(12f, 12f) * Main.rand.Next(9, 12) * 0.1f, ModContent.ProjectileType<Projectiles.OldDukesWisdomTooth>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner));
			Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.NPCDeath1, base.Projectile.Center);
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(70, 120);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("SulphuricPoisoning").Type, 60);
		}
	}
}
