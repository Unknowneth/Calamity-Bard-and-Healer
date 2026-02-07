using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Projectiles.Scythe;

namespace CalamityBardHealer.Projectiles
{
	public class IrradiatedKusarigama : ScythePro
	{
		public override void SafeSetDefaults() {
			base.Projectile.width = 1;
			base.Projectile.height = 1;
			base.Projectile.manualDirectionChange = true;
		}
		public override bool PreAI() {
			Player player = Main.player[Projectile.owner];
			player.heldProj = base.Projectile.whoAmI;
			player.compositeFrontArm.enabled = true;
			float attackTime = (float)player.itemTime / (float)player.itemTimeMax * 3;
			Vector2 posOff = Vector2.Zero;
			Projectile.timeLeft = player.itemTime - 1;
			bool emit = false;
			if(attackTime > 2f) {
				attackTime -= 2f;
				Projectile.direction = player.direction;
				player.compositeFrontArm.rotation = MathHelper.Pi * MathHelper.SmoothStep(-0.6f, -1.35f, attackTime) * player.direction;
				Projectile.rotation = MathHelper.Pi * MathHelper.SmoothStep(-0.7f, -1.5f, attackTime) * player.direction;
				if(Projectile.ai[0] == 0f) Projectile.Opacity = MathHelper.Min(1f, (1f - attackTime) * 3f);
				else attackTime += 2f;
			}
			else if(attackTime > 1f) {
				if(++Projectile.localAI[2] == 1f) Terraria.Audio.SoundEngine.PlaySound(player.HeldItem.UseSound, player.Center);
				attackTime -= 1f;
				player.direction = Projectile.direction;
				player.compositeFrontArm.rotation = MathHelper.Pi * MathHelper.SmoothStep(-0.5f, -0.6f, attackTime) * Projectile.direction;
				Projectile.rotation = MathHelper.Pi * MathHelper.SmoothStep(-0.6f, -0.7f, attackTime) * Projectile.direction;
				Projectile.rotation += new Vector2(Projectile.velocity.X * Projectile.direction, Projectile.velocity.Y).ToRotation() * MathHelper.Clamp(3f * (1f - attackTime), 0, 1) * Projectile.direction;
				player.compositeFrontArm.rotation += new Vector2(Projectile.velocity.X * player.direction, Projectile.velocity.Y).ToRotation()  * MathHelper.Clamp(3f * (1f - attackTime), 0, 1) * Projectile.direction;
				if(Projectile.ai[0] == 0f) posOff += Projectile.velocity.RotatedBy(0.1f * (float)System.Math.Sin(MathHelper.Pi * (1f - attackTime)) * -Projectile.direction) * (1f - attackTime) * 540f;
				else attackTime += 1f;
			}
			else {
				player.direction = Projectile.direction;
				player.compositeFrontArm.rotation = MathHelper.Pi * MathHelper.SmoothStep(-1.35f, -0.5f, attackTime) * Projectile.direction;
				Projectile.rotation = MathHelper.Pi * -0.6f * Projectile.direction + new Vector2(Projectile.velocity.X * Projectile.direction, Projectile.velocity.Y).ToRotation() * Projectile.direction;
				player.compositeFrontArm.rotation += new Vector2(Projectile.velocity.X * player.direction, Projectile.velocity.Y).ToRotation() * MathHelper.Clamp(3f * attackTime, 0, 1) * Projectile.direction;
				if(Projectile.ai[0] == 0f) posOff += Projectile.velocity * attackTime * 540f;
				emit = true;
			}
			if(attackTime <= 2f && Projectile.ai[0] == 1f) {
				posOff -= (Vector2.UnitX.RotatedBy(MathHelper.Pi * attackTime) * new Vector2(270, 54 * -player.direction)).RotatedBy(Projectile.velocity.ToRotation()) - Vector2.Normalize(Projectile.velocity) * 270;
				emit = true;
			}
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation += MathHelper.Pi * Projectile.spriteDirection;
			Projectile.Center = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation) + posOff;
			if(emit) if(Projectile.ai[0] == 0f && Main.myPlayer == player.whoAmI) NetMessage.SendData(27, -1, -1, null, Projectile.NewProjectile(player.GetSource_ItemUse(player.HeldItem), base.Projectile.Center, Main.rand.NextVector2CircularEdge(4f, 4f), ModContent.ProjectileType<Projectiles.IrradiatedVortex>(), Projectile.damage, Projectile.knockBack, Projectile.owner));
			else if(Projectile.ai[0] == 1f) Main.dust[Dust.NewDust(base.Projectile.Center, 0, 0, 75, 0f, 0f, 0, default(Color), Projectile.scale * 1.6f)].noGravity = true;
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			if(Projectile.ai[0] == 1f) return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Main.player[Projectile.owner].GetFrontHandPosition(Main.player[Projectile.owner].compositeFrontArm.stretch, Main.player[Projectile.owner].compositeFrontArm.rotation), base.Projectile.Center, 10 * base.Projectile.scale, ref point);
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), base.Projectile.Center, base.Projectile.Center - Vector2.UnitY.RotatedBy(base.Projectile.rotation) * 42 * base.Projectile.scale, 16 * base.Projectile.scale, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Player player = Main.player[Projectile.owner];
			Vector2 center = player.GetFrontHandPosition(player.compositeFrontArm.stretch, player.compositeFrontArm.rotation);
			Vector2 toUser = center - Projectile.Center;
			float chainRotation = toUser.ToRotation();
			bool otherWay = Projectile.ai[0] == 1f;
			if(otherWay) chainRotation += MathHelper.PiOver2;
			else chainRotation -= MathHelper.PiOver2;
			for(int i = 0; i < toUser.Length() / 6; i++) {
				Vector2 drawPos = Projectile.Center + Vector2.Normalize(toUser) * i * 6;
				if(i % 2 == 0) Main.EntitySpriteDraw(texture, drawPos - Main.screenPosition, new Rectangle(16, 44, 8, 8), Lighting.GetColor((int)(drawPos.X / 16), (int)(drawPos.Y / 16)), chainRotation, new Vector2(4), Projectile.scale, SpriteEffects.None, 0);
				else Main.EntitySpriteDraw(texture, drawPos - Main.screenPosition, new Rectangle(0, 44, 12, 8), Lighting.GetColor((int)(drawPos.X / 16), (int)(drawPos.Y / 16)), chainRotation, new Vector2(6, 4), Projectile.scale, SpriteEffects.None, 0);
			}
			if(otherWay) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 54, 18, 18), lightColor, Projectile.rotation, new Vector2(9), Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			else if(player.itemTime < player.itemTimeMax - 1) Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, 0, 34, 42), lightColor, Projectile.rotation, new Vector2(Projectile.spriteDirection < 0 ? texture.Width - 4 : 4, 42), Projectile.scale, Projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}
		public override void SafeOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f) NetMessage.SendData(27, -1, -1, null, Projectile.NewProjectile(Projectile.GetSource_OnHit(target), base.Projectile.Center, Main.rand.NextVector2CircularEdge(4f, 4f), ModContent.ProjectileType<Projectiles.IrradiatedVortex>(), Projectile.damage, Projectile.knockBack, Projectile.owner));
			target.AddBuff(70, 300);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("SulphuricPoisoning").Type, 240);
		}
		public override void OnFirstHit(NPC target, NPC.HitInfo hit, int damageDone) {
			if(Main.myPlayer == Projectile.owner && Projectile.ai[0] == 1f && Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<Projectiles.IrradiatedOasis>()] < 3) NetMessage.SendData(27, -1, -1, null, Projectile.NewProjectile(Projectile.GetSource_OnHit(target), base.Projectile.Center, Main.rand.NextVector2Circular(12f, 12f), ModContent.ProjectileType<Projectiles.IrradiatedOasis>(), 0, 0f, Projectile.owner));
		}
	}
}