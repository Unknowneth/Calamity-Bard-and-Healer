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
	public class GashadokuroCracks : BardProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[base.Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[base.Projectile.type] = -1;
			ProjectileID.Sets.DrawScreenCheckFluff[base.Projectile.type] = 1000;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 2, base.Projectile.type);
			mor.Call("addElementProj", 12, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetBardDefaults() {
			base.Projectile.width = 16;
			base.Projectile.height = 16;
			base.Projectile.aiStyle = -1;
			base.Projectile.friendly = true;
			base.Projectile.penetrate = -1;
			base.Projectile.tileCollide = false;
			base.Projectile.extraUpdates = 8;
			base.Projectile.timeLeft = 120;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = -1;
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			if(base.Projectile.timeLeft % 10 == 0 && base.Projectile.velocity != Vector2.Zero) {
				for(int i = base.Projectile.oldPos.Length - 1; i > 0; i--) base.Projectile.oldPos[i] = base.Projectile.oldPos[i - 1];
				base.Projectile.oldPos[0] = base.Projectile.Center;
				if(Main.myPlayer == base.Projectile.owner) {
					if(--base.Projectile.ai[0] % 10 == 0f && base.Projectile.timeLeft > 30) {
						int p = Projectile.NewProjectile(base.Projectile.GetSource_FromAI(), base.Projectile.Center, base.Projectile.velocity.RotatedBy(Main.rand.NextBool(2) ? MathHelper.PiOver4 : -MathHelper.PiOver4) * 0.5f, Type, base.Projectile.damage, base.Projectile.knockBack, base.Projectile.owner, 0f, base.Projectile.ai[1]);
						Main.projectile[p].timeLeft = base.Projectile.timeLeft;
						NetMessage.SendData(27, -1, -1, null, p);
					}
					base.Projectile.velocity = base.Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver4) * (base.Projectile.timeLeft % 20 == 0 ? 1 : -1) * 0.5f);
					NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
				}
				if(base.Projectile.ai[1] < base.Projectile.oldPos.Length - 1) base.Projectile.ai[1]++;
				else base.Projectile.velocity *= 0f;
			}
			if(base.Projectile.timeLeft < 85) base.Projectile.alpha += 3;
		}
		public override bool PreDraw(ref Color lightColor) {
			float size = base.Projectile.scale * MathHelper.Lerp(1f, 0f, (float)base.Projectile.alpha / 255f);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.EntitySpriteDraw(texture, base.Projectile.oldPos[0] - Main.screenPosition, new Rectangle(31, texture.Height / 2, 10, texture.Height / 2), new Color(255, 55, 0, 0), (base.Projectile.oldPos[0] - base.Projectile.oldPos[1]).ToRotation() - MathHelper.PiOver2, new Vector2(5f, 0f), new Vector2(size, 1f), SpriteEffects.None, 0);
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) if(i > 0 && base.Projectile.oldPos[i] != base.Projectile.oldPos[i - 1] && base.Projectile.oldPos[i] != Vector2.Zero) Main.EntitySpriteDraw(texture, base.Projectile.oldPos[i - 1] - Main.screenPosition, new Rectangle(31, texture.Height / 2 - 1, 10, 1), new Color(255, (int)MathHelper.Lerp(55f, 0f, (float)i / (float)base.Projectile.oldPos.Length), 0, 0), (base.Projectile.oldPos[i] - base.Projectile.oldPos[i - 1]).ToRotation() - MathHelper.PiOver2, new Vector2(5f, 0f), new Vector2(size, Vector2.Distance(base.Projectile.oldPos[i], base.Projectile.oldPos[i - 1])), SpriteEffects.None, 0);
			return false;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => overWiresUI.Add(index);
	}
}