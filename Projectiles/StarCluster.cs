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
	public class StarCluster : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Typeless/AstralStar";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
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
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = -1;
		}
		public override void AI() {
			if(base.Projectile.ai[0] >= 0f) {
				if(Main.myPlayer == base.Projectile.owner) {
					Player player = Main.player[base.Projectile.owner];
					if(player.channel && player.GetModPlayer<ThoriumPlayer>().bardResource > 0) base.Projectile.velocity = Vector2.Normalize(Main.MouseWorld - player.MountedCenter) * base.Projectile.velocity.Length();
					base.Projectile.Center = player.MountedCenter + Vector2.Normalize(base.Projectile.velocity) * 58f;
					if(player.HeldItem.type != ModContent.ItemType<Items.StarCluster>() || ((!player.channel || player.GetModPlayer<ThoriumPlayer>().bardResource == 0) && player.itemTime == 0)) {
						base.Projectile.velocity += Main.rand.NextVector2Circular(2f, 2f);
						base.Projectile.ai[0] = -1f;
					}
					NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
				}
				base.Projectile.rotation += Projectile.velocity.X < 0 ? -0.2f : 0.2f;
				base.Projectile.timeLeft++;
			}
			else {
				if(Projectile.timeLeft > 5 && Collision.SolidCollision(Projectile.Center, 0, 0)) {
					Projectile.timeLeft = 5;
					Projectile.velocity *= 0f;
				}
				if(Projectile.timeLeft < 5) Projectile.alpha += 51;
				base.Projectile.extraUpdates = 2;
				if(Projectile.velocity.Length() > 0f) base.Projectile.rotation += Projectile.velocity.X < 0 ? -0.2f : 0.2f;
			}
		}
		public override bool PreDraw(ref Color lightColor) {
			if(base.Projectile.ai[0] >= 0f) return false;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.White * Projectile.Opacity;
			for(int i = 1; i < base.Projectile.oldPos.Length; i++) Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + base.Projectile.Size * 0.5f - Main.screenPosition, null, new Color(100, 100, 100, 0) * Projectile.Opacity * MathHelper.Lerp(1f, 0f, (float)i / (float)base.Projectile.oldPos.Length), base.Projectile.oldRot[i], texture.Size() * 0.5f, base.Projectile.scale * MathHelper.Lerp(1f, 0.5f, (float)i / (float)base.Projectile.oldPos.Length), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, base.Projectile.rotation, texture.Size() * 0.5f, base.Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanDamage() => base.Projectile.ai[0] < 0f ? null : false;
		public override bool ShouldUpdatePosition() => base.Projectile.ai[0] < 0f;
	}
}