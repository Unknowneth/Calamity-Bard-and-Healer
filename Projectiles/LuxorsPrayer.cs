using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;

namespace CalamityBardHealer.Projectiles
{
	public class LuxorsPrayer : ModProjectile
	{
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 1, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.DamageType = HealerDamage.Instance;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
			Projectile.timeLeft = 180;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
		}
		public override void AI() {
			if(Projectile.timeLeft < 15) Projectile.alpha += 17;
			else if(Projectile.alpha > 0) Projectile.alpha -= 15;
			if(Projectile.ai[0] > 0f) {
				for(int i = 0; i < Main.maxPlayers; i++) if(Main.player[i].Hitbox.Intersects(Projectile.Hitbox) && Main.player[i].active && !Main.player[i].dead && Main.player[i].statLife < Main.player[i].statLifeMax2 && Main.player[i].team == Main.player[Projectile.owner].team && HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.player[i], (int)MathHelper.Max(1f, Projectile.ai[0] / 10f), 60)) Projectile.Kill();
				Projectile.rotation += Projectile.direction / 5f;
				Projectile.velocity *= 0.96f;
				Projectile.velocity.Y += 0.14f;
			}
			else Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Projectile.ai[0] < 1f && Main.myPlayer == Projectile.owner) {
				Projectile.ai[0] = damageDone;
				Projectile.velocity.X = -Projectile.oldVelocity.X / 2f;
				Projectile.velocity.Y = -Projectile.oldVelocity.Y / 2f;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
		}
		public override bool? CanDamage() => Projectile.ai[0] <= 0f;
		public override Color? GetAlpha(Color lightColor) => new Color(255 - Projectile.alpha, 255 - Projectile.alpha, 255 - Projectile.alpha, 0);
	}
}