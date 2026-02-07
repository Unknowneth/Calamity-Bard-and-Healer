using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class ChristmasCarol : BardProjectile
	{
		public override string Texture => "CalamityBardHealer/Items/ChristmasCarol";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 4, base.Projectile.type);
			mor.Call("addElementProj", 8, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 46;
			base.Projectile.height = 46;
			base.Projectile.aiStyle = -1;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = false;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.extraUpdates = 2;
			base.Projectile.penetrate = -1;
			base.Projectile.timeLeft = 180;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 25;
		}
		public override void AI() {
			base.Projectile.rotation += base.Projectile.direction;
			base.Projectile.spriteDirection = base.Projectile.velocity.X > 0 ? 1 : -1;
			if(base.Projectile.ai[1] > 0f) {
				if(base.Projectile.ai[0] > 1f) {
					if(base.Projectile.timeLeft <= 2) base.Projectile.ai[0] = 0f;
					else base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(Main.npc[(int)base.Projectile.ai[1] - 1].Center - base.Projectile.Center), ++base.Projectile.ai[2] / 30f)) * base.Projectile.velocity.Length();
				}
				else {
					base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(Main.player[base.Projectile.owner].Center - base.Projectile.Center), ++base.Projectile.ai[2] / 60f)) * base.Projectile.velocity.Length();
					if(base.Projectile.Hitbox.Intersects(Main.player[base.Projectile.owner].Hitbox)) base.Projectile.Kill();
					else base.Projectile.timeLeft = 2;
				}
				return;
			}
			int who = 0;
			float maxRange = 800f;
			for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Collision.CanHitLine(base.Projectile.Center, 0, 0, Main.npc[i].Center, 0, 0) && Main.npc[i].Distance(base.Projectile.Center) < maxRange) {
				maxRange = Main.npc[i].Distance(base.Projectile.Center);
				who = i + 1;
			}
			if(who > 0) base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(Main.npc[who - 1].Center - base.Projectile.Center), ++base.Projectile.ai[2] / 60f * 0.5f)) * base.Projectile.velocity.Length();
			else if(base.Projectile.Distance(base.Projectile.Center) > 640f || base.Projectile.timeLeft <= 2) {
				base.Projectile.ai[1] = 1f;
				base.Projectile.ai[2] = 0f;
			}
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == base.Projectile.owner) {
				base.Projectile.ai[1] = target.whoAmI + 1;
				base.Projectile.ai[2] = 0f;
				if(base.Projectile.ai[0] < 2f) base.Projectile.velocity *= -1f;
				NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
		}
	}
}