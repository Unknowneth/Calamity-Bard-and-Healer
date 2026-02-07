using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;

namespace CalamityBardHealer.Projectiles
{
	public class SludgeBomb : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Boss/UnstableEbonianGlob";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 9, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.width = 24;
			base.Projectile.height = 24;
			base.Projectile.aiStyle = -1;
			base.Projectile.hostile = false;
			base.Projectile.friendly = true;
			base.Projectile.tileCollide = false;
			base.Projectile.timeLeft = 300;
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			if(base.Projectile.ai[0] >= 0f) {
				if(Main.myPlayer == base.Projectile.owner) {
					Player player = Main.player[base.Projectile.owner];
					if(player.channel && player.GetModPlayer<ThoriumPlayer>().bardResource > 0) base.Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * base.Projectile.velocity.Length();
					base.Projectile.Center = player.MountedCenter + Vector2.Normalize(base.Projectile.velocity) * 54f;
					if(player.HeldItem.type != ModContent.ItemType<Items.ReturntoSludge>() || ((!player.channel || player.GetModPlayer<ThoriumPlayer>().bardResource == 0) && player.itemTime == 0)) {
						int abyssal = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 173, 0f, 0f, 100, default(Color), 1.7f);
						Main.dust[abyssal].noGravity = true;
						Main.dust[abyssal].velocity *= 5f;
						Main.dust[abyssal].velocity += base.Projectile.velocity;
						abyssal = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 173, 0f, 0f, 100, default(Color), 1f);
						Main.dust[abyssal].velocity *= 2f;
						Main.dust[abyssal].velocity += base.Projectile.velocity;
						base.Projectile.velocity *= (base.Projectile.ai[0] * 0.2f + 1f);
						base.Projectile.ai[0] = -1f;
					}
					NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
				}
				base.Projectile.timeLeft++;
			}
			else {
				base.Projectile.tileCollide = true;
				base.Projectile.extraUpdates = 1;
				base.Projectile.velocity.Y += 0.21f;
				base.Projectile.velocity *= 0.99f;
				base.Projectile.rotation = base.Projectile.velocity.ToRotation();
			}
		}
		public override void OnKill(int timeLeft) {
			base.Projectile.position = base.Projectile.Center;
			base.Projectile.width = base.Projectile.height = (int)MathHelper.Lerp(80f, 160f, base.Projectile.ai[0] * 0.1f);
			base.Projectile.position = base.Projectile.position - base.Projectile.Size * 0.5f;
			base.Projectile.maxPenetrate = -1;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 10;
			base.Projectile.Damage();
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, base.Projectile.Center, null);
			for(int i = 0; i < 25; i++) {
				int abyssal = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 173, 0f, 0f, 100, default(Color), 1.2f);
				Main.dust[abyssal].velocity *= 3f;
				if(Main.rand.NextBool(2)) {
					Main.dust[abyssal].scale = 0.5f;
					Main.dust[abyssal].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
			}
			for(int j = 0; j < 25; j++) {
				int abyssal = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 173, 0f, 0f, 100, default(Color), 1.7f);
				Main.dust[abyssal].noGravity = true;
				Main.dust[abyssal].velocity *= 5f;
				abyssal = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 173, 0f, 0f, 100, default(Color), 1f);
				Main.dust[abyssal].velocity *= 2f;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			if(base.Projectile.ai[0] >= 0f) return false;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			for(int i = 1; i < base.Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + base.Projectile.Size * 0.5f - Main.screenPosition, null, new Color(100, 100, 100, 0) * MathHelper.Lerp(1f, 0f, (float)i / (float)base.Projectile.oldPos.Length), base.Projectile.oldRot[i], texture.Size() * 0.5f, new Vector2(1f + base.Projectile.velocity.Length() * 0.01f, 1f - base.Projectile.velocity.Length() * 0.01f) * base.Projectile.scale, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() * 0.5f, new Vector2(1f + base.Projectile.velocity.Length() * 0.01f, 1f - base.Projectile.velocity.Length() * 0.01f) * base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanDamage() => base.Projectile.ai[0] < 0f ? null : false;
		public override bool ShouldUpdatePosition() => base.Projectile.ai[0] < 0f;
	}
}