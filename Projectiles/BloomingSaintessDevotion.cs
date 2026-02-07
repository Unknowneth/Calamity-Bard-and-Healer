using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class BloomingSaintessDevotion : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 70;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 10, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 25;
			Projectile.timeLeft = 2;
			Projectile.extraUpdates = 1;
			Projectile.hide = true;
		}
		public override void AI() {
			Projectile.timeLeft = (int)(--Projectile.ai[1]);
			if(Projectile.timeLeft % 10 == 0 && Main.myPlayer == Projectile.owner) {
				if(Projectile.ai[2] < 2f) {
					int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4)), Type, Projectile.damage, Projectile.knockBack, Projectile.owner, -Projectile.ai[0], Projectile.ai[1], Projectile.ai[2] + 1f);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				Projectile.ai[0] = Main.rand.Next(10, 100) * (Projectile.ai[0] < 0f ? 0.06f : -0.06f);
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(Main.rand.NextBool(4) && Projectile.timeLeft % 5 == 0 && Main.myPlayer == Projectile.owner) {
				int p = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)), ModContent.ProjectileType<Projectiles.BloomingSaintessDevotion2>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			if(target.type == ModLoader.GetMod("CalamityMod").Find<ModNPC>("DevourerofGodsBody").Type) modifiers.SourceDamage *= 4;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			bool trailHit = false;
			for(int i = 0; i < base.Projectile.oldPos.Length; i++) trailHit |= targetHitbox.Intersects(new Rectangle((int)base.Projectile.oldPos[i].X, (int)base.Projectile.oldPos[i].Y, projHitbox.Width, projHitbox.Height));
			return projHitbox.Intersects(targetHitbox) || trailHit;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float fade = MathHelper.Lerp(1f, 0f, (int)Projectile.alpha / 255f) * (MathHelper.Min(Projectile.timeLeft, 15) / 15);
			if(Projectile.velocity != Vector2.Zero) for(int i = 0; i < Projectile.oldPos.Length; i++) {
				lightColor = Color.Lerp(Color.Lime, Color.DarkGreen, (float)i / (float)Projectile.oldPos.Length);
				lightColor.A = 200;
				lightColor *= fade;
				Main.EntitySpriteDraw(texture, Projectile.oldPos[i] + new Vector2(Projectile.width, Projectile.height) / 2 - Main.screenPosition, new Rectangle(31, 0, 10, texture.Height), lightColor, Projectile.oldRot[i] + MathHelper.PiOver2, new Vector2(10, texture.Height) * 0.5f, 0.75f, SpriteEffects.None, 0);
			}
			return false;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindProjectiles.Add(index);
	}
}