using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class MelterNote1 : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Magic/MelterNote1";
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.CloneDefaults(ModLoader.GetMod("CalamityMod").Find<ModProjectile>("MelterNote1").Type);
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.extraUpdates++;
		}
		public override void AI() {
			base.Projectile.velocity *= 0.985f;
			if(base.Projectile.localAI[0] == 0f) {
				base.Projectile.scale += 0.02f;
				if(base.Projectile.scale >= 1.25f) {
					base.Projectile.localAI[0] = 1f;
					return;
				}
			}
			else if(base.Projectile.localAI[0] == 1f) {
				base.Projectile.scale -= 0.02f;
				if(base.Projectile.scale <= 0.75f) base.Projectile.localAI[0] = 0f;
			}
		}
		public override void BardModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			if(target.lifeRegen < 0) modifiers.SourceDamage *= 2;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) target.AddBuff(thorium.Find<ModBuff>("Tuned").Type, 600);
			Main.player[base.Projectile.owner].GetModPlayer<ThoriumPlayer>().HealInspiration(1);
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}