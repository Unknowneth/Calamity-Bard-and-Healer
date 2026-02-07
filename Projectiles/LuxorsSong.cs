using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace CalamityBardHealer.Projectiles
{
	public class LuxorsSong : ModProjectile
	{
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 6, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = BardDamage.Instance;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}
		public override void AI() {
			if(Projectile.ai[0] < 0f) {
				if(Projectile.ai[0] == -4f) Projectile.timeLeft = 0;
				else if(Projectile.timeLeft > 15) Projectile.timeLeft = 15;
			}
			else if(Projectile.ai[0] == 5) Projectile.velocity += Vector2.Normalize(Projectile.velocity) * 0.25f;
			else if(Projectile.ai[0] == 1) {
				int nearest = -1;
				float maxRange = 160f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Projectile.Center, 0, 0, Main.npc[i].Center, 0, 0) && Projectile.Distance(Main.npc[i].Center) < maxRange) {
					maxRange = Main.npc[i].Distance(Projectile.Center);
					nearest = i;
				}
				if(nearest >= 0) Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(Projectile.velocity), Vector2.Normalize(Main.npc[nearest].Center - Projectile.Center), MathHelper.Min(++Projectile.ai[1] / 60f, 1f) * 0.25f)) * Projectile.velocity.Length();
			}
			Projectile.rotation = Projectile.velocity.ToRotation();
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 15;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Projectile.ai[0] != 2f) {
				if(Projectile.ai[0] > 0f) Projectile.ai[0] *= -1f;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
		}
		public override void OnKill(int timeLeft) {
			if(Projectile.ai[0] != 4f) return;
			Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item14, Projectile.position);
			Projectile.position = Projectile.Center;
			Projectile.Size *= 5f;
			Projectile.position -= Projectile.Size * 0.5f;
			for(int a = 0; a < 15; a++) {
				Dust d = Dust.NewDustPerfect(Projectile.Center, 278, Vector2.Zero, 100, new Color(255, 133, 112, 100), Main.rand.Next(2, 7) * 0.1f + 1f);
				d.position += Main.rand.NextVector2Circular(Projectile.width, Projectile.height) * 0.5f;
				d.noGravity = a % 2 == 0;
				d.velocity = (d.position - Projectile.Center) * 0.01f;
			}
			Projectile.Damage();
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(Projectile.ai[0] == 3f) if(++Projectile.ai[2] < 3 + Main.player[Projectile.owner].GetModPlayer<ThoriumPlayer>().bardBounceBonus) {
				if(Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = -oldVelocity.X;
				if(Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = -oldVelocity.Y;
				return false;
			}
			return true;
		}
		public override Color? GetAlpha(Color lightColor) => new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
	}
}