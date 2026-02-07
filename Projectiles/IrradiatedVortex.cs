using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalamityBardHealer.Projectiles
{
	public class IrradiatedVortex : ModProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Boss/OldDukeVortex";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 3, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.timeLeft = 300;
			Projectile.ArmorPenetration = 15;
		}
		public override void AI() {
			if(Projectile.timeLeft < 240) {
				float maxRange = 800f;
				int target = -1;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(this, false) && Main.npc[i].Distance(Projectile.Center) < maxRange) {
					maxRange = Main.npc[i].Distance(Projectile.Center);
					target = i;
				}
				if(target >= 0) Projectile.velocity += Vector2.Normalize(Main.npc[target].Center - Projectile.Center) * 0.54f;
			}
			Projectile.velocity *= 0.96f;
			if(Projectile.ai[0] == 1f && Projectile.timeLeft > 15) Projectile.timeLeft = 15;
			Projectile.rotation += 0.1f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			target.AddBuff(70, 300);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("SulphuricPoisoning").Type, 240);
			Projectile.ai[0] = 1f;
			NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)Projectile.alpha / 255f) * (MathHelper.Min(Projectile.timeLeft, 15) / 30);
			for(int i = 0; i < Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + Projectile.Size / 2 - Main.screenPosition, null, new Color(255, 255, 255, 0) * (i == 0 ? 0.2f * (1f - (float)i / (float)Projectile.oldPos.Length) : 1f) * fade, Projectile.oldRot[i], texture.Size() * 0.5f, Projectile.scale / 408f * 16f, SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanDamage() => Projectile.timeLeft < 240;
	}
}