using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class WhisperingTrunk : BardProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 65;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = 2;
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetBardDefaults() {
			base.Projectile.width = 16;
			base.Projectile.height = 16;
			base.Projectile.aiStyle = -1;
			base.Projectile.friendly = true;
			base.Projectile.penetrate = -1;
			base.Projectile.tileCollide = false;
			base.Projectile.DamageType = BardDamage.Instance;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 25;
			base.Projectile.timeLeft = 2;
			base.Projectile.hide = true;
		}
		public override void AI() {
			Projectile.timeLeft = (int)(--Projectile.ai[1]);
			if(base.Projectile.timeLeft % 20 == 0 && Main.myPlayer == base.Projectile.owner) {
				if(base.Projectile.ai[2] == 0f) {
					int p = Projectile.NewProjectile(base.Projectile.GetSource_FromAI(), base.Projectile.Center, base.Projectile.velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)), Type, base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, -base.Projectile.ai[0], base.Projectile.ai[1], 1f);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				base.Projectile.ai[0] = Main.rand.Next(10, 100) * (base.Projectile.ai[0] < 0f ? 0.06f : -0.06f);
				NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
			if(Main.rand.NextBool(2) && base.Projectile.timeLeft % 5 == 0 && Main.myPlayer == base.Projectile.owner) {
				Vector2 random = base.Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi));
				int p = Projectile.NewProjectile(base.Projectile.GetSource_FromAI(), base.Projectile.Center + random, random, ModContent.ProjectileType<Projectiles.WhisperingLeaf>(), base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			base.Projectile.velocity = base.Projectile.velocity.RotatedBy(MathHelper.ToRadians(base.Projectile.ai[0]));
			base.Projectile.rotation = base.Projectile.velocity.ToRotation();
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			bool trailHit = false;
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) trailHit |= targetHitbox.Intersects(new Rectangle((int)base.Projectile.oldPos[i].X, (int)base.Projectile.oldPos[i].Y, projHitbox.Width, projHitbox.Height));
			return projHitbox.Intersects(targetHitbox) || trailHit;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)base.Projectile.alpha / 255f) * (MathHelper.Min(base.Projectile.timeLeft, 15) / 15);
			if(base.Projectile.velocity != Vector2.Zero) for(int i = 0; i < base.Projectile.oldPos.Length; i++) {
				lightColor = Color.Lerp(new Color(137, 81, 48, 175), new Color(106, 59, 46, 175), (float)i / (float)base.Projectile.oldPos.Length) * fade;
				Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i] + new Vector2(base.Projectile.width, base.Projectile.height) / 2 - Main.screenPosition, new Rectangle(31, 0, 10, texture.Height), lightColor, base.Projectile.oldRot[i] + MathHelper.PiOver2, new Vector2(10, texture.Height) * 0.5f, 1f, SpriteEffects.None, 0);
			}
			return false;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindProjectiles.Add(index);
	}
}