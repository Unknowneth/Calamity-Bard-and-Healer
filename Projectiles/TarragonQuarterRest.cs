using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace CalamityBardHealer.Projectiles
{
	public class TarragonQuarterRest : ModProjectile
	{
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 10, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 12;
			Projectile.height = 40;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = BardDamage.Instance;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.timeLeft = 180;
		}
		public override void AI() {
			if(Projectile.timeLeft < 15) {
				Projectile.alpha += 17;
				Projectile.velocity *= 0.93f;
				return;
			}
			else if(Projectile.alpha > 0) Projectile.alpha -= 15;
			int nearest = -1;
			float maxRange = 640f;
			for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(Projectile.Center, 0, 0, Main.npc[i].Center, 0, 0) && Projectile.Distance(Main.npc[i].Center) < maxRange) {
				maxRange = Main.npc[i].Distance(Projectile.Center);
				nearest = i;
			}
			if(nearest >= 0) Projectile.velocity += Vector2.Normalize(Main.npc[nearest].Center - Projectile.Center) * 0.03f;
			else Projectile.velocity -= Vector2.Normalize(Main.player[Projectile.owner].Center - Projectile.Center) * 0.03f;
			Projectile.velocity *= 0.97f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) => Main.player[Projectile.owner].GetModPlayer<ThoriumPlayer>().HealInspiration(1);
		public override Color? GetAlpha(Color lightColor) => new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
	}
}