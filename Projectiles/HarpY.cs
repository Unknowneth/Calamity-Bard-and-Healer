using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class HarpY : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Magic/StickyFeather";
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 6, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 14;
			base.Projectile.height = 14;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = true;
			base.Projectile.timeLeft = 300;
			base.Projectile.extraUpdates = 1;
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(++base.Projectile.ai[2] < 2 + Main.player[base.Projectile.owner].GetModPlayer<ThoriumPlayer>().bardBounceBonus) {
				if(base.Projectile.velocity.X != oldVelocity.X) base.Projectile.velocity.X = -oldVelocity.X;
				if(base.Projectile.velocity.Y != oldVelocity.Y) base.Projectile.velocity.Y = -oldVelocity.Y;
				return false;
			}
			return true;
		}
		public override void AI() => base.Projectile.rotation = base.Projectile.velocity.ToRotation() + MathHelper.PiOver2;
	}
}