using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class ExoSound : BardProjectile
	{
		public override string Texture => "CalamityMod/Particles/SemiCircularSmear";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 6, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 90;
			base.Projectile.height = 90;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.ArmorPenetration = 50;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 20;
			base.Projectile.extraUpdates = 1;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft = 180;
			base.Projectile.alpha = 255;
		}
		public override void AI() {
			if(base.Projectile.timeLeft < 15) base.Projectile.alpha += 17;
			else if(base.Projectile.alpha > 0) base.Projectile.alpha -= 51;
			if(base.Projectile.ai[1] == 0f && Main.myPlayer == base.Projectile.owner) {
				float maxRange = 1600f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Main.npc[i].Distance(Main.MouseWorld) < maxRange) {
					maxRange = Main.npc[i].Distance(Main.MouseWorld);
					base.Projectile.ai[1] = i + 1;
				}
				if(base.Projectile.ai[1] > 0f) NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
			else if(base.Projectile.ai[1] > 0f) if(!Main.npc[(int)base.Projectile.ai[1] - 1].CanBeChasedBy(this, false)) base.Projectile.ai[0] = base.Projectile.ai[1] = 0f;
			else base.Projectile.velocity = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(base.Projectile.velocity), Vector2.Normalize(Main.npc[(int)base.Projectile.ai[1] - 1].Center - base.Projectile.Center), MathHelper.Min(++base.Projectile.ai[0] / 120f, 1f) * 0.25f)) * base.Projectile.velocity.Length();
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Main.player[Projectile.owner].statLife += damageDone / 20;
			Main.player[Projectile.owner].HealEffect(damageDone / 20);
			if(Main.rand.NextBool(3)) Main.player[Projectile.owner].GetModPlayer<ThoriumPlayer>().HealInspiration(1);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("MiracleBlight").Type, 240);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) {
				float rotation = MathHelper.ToRadians(20f * (base.Projectile.timeLeft) + i * 9f);
				if(rotation < 0f) rotation += MathHelper.TwoPi;
				lightColor = Main.hslToRgb(System.Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.75f) * MathHelper.Lerp(MathHelper.Lerp(0.5f, 0f, (float)i / (float)base.Projectile.oldPos.Length), 0f, (float)base.Projectile.alpha / 255f);
				lightColor.A = 0;
				Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + base.Projectile.Size * 0.5f - Main.screenPosition, null, lightColor, base.Projectile.oldRot[i], texture.Size() / 2, base.Projectile.scale * 0.5f, SpriteEffects.FlipHorizontally, 0);
			}
			return false;
		}
	}
}