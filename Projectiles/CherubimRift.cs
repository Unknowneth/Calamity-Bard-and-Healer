using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using CalamityMod.Particles;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class CherubimRift : ModProjectile
	{
		public override string Texture => "Terraria/Images/Extra_98";
		public override void SetStaticDefaults() {
			Terraria.ID.ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1000;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 8, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.hide = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 60;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.ArmorPenetration = 75;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
		}
		public override void AI() {
			if(Projectile.localAI[0] == 0f && Projectile.localAI[1] == 0f) {
				Projectile.localAI[0] = Projectile.Center.X;
				Projectile.localAI[1] = Projectile.Center.Y;
			}
			if(Main.myPlayer == Projectile.owner && Projectile.timeLeft == 30) {
				Projectile.ai[0] = Vector2.Distance(new Vector2(Projectile.localAI[0], Projectile.localAI[1]), Projectile.Center);
				Projectile.Center = Vector2.Lerp(new Vector2(Projectile.localAI[0], Projectile.localAI[1]), Projectile.Center, 0.5f);
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			if(Projectile.timeLeft < 30) {
				if(Main.myPlayer == Projectile.owner && Projectile.timeLeft % 6 == 0) {
					int p = Projectile.NewProjectile(Main.player[Projectile.owner].GetSource_ItemUse(Main.player[Projectile.owner].HeldItem), base.Projectile.Center + Projectile.velocity * Main.rand.Next(-65, 66) * 0.1f, Projectile.velocity.RotatedBy(Main.rand.NextFloat(MathHelper.PiOver2) - MathHelper.PiOver4 + MathHelper.PiOver2) * (Main.rand.NextBool(2) ? 0.3f : -0.3f), ModContent.ProjectileType<Projectiles.CherubimBeam>(), base.Projectile.damage, base.Projectile.knockBack, Projectile.owner);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				float rotation = MathHelper.Pi * (float)Projectile.timeLeft / 30f - base.Projectile.rotation + Main.GlobalTimeWrappedHourly * MathHelper.PiOver4;
				while(rotation > MathHelper.Pi) rotation -= MathHelper.TwoPi;
				while(rotation < -MathHelper.Pi) rotation += MathHelper.TwoPi;
				GeneralParticleHandler.SpawnParticle(new HeavySmokeParticle(base.Projectile.Center + Projectile.velocity * Main.rand.Next(-30, 31) * 0.1f, Main.rand.NextVector2Circular(Projectile.velocity.Length(), Projectile.velocity.Length()),  Main.hslToRgb(System.Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * 0.7f, 15, Main.rand.NextFloat(0.4f, 0.7f) * base.Projectile.scale * 0.8f, 0.8f, Main.rand.NextFloat(-0.03f, 0.03f), true, 0.01f, true));
			}
			else if(Projectile.timeLeft == 30) Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item45, Projectile.position);
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("MiracleBlight").Type, 240);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Vector2.Normalize(Projectile.velocity) * Projectile.ai[0] * 0.4f, Projectile.Center + Vector2.Normalize(Projectile.velocity) * Projectile.ai[0] * 0.4f, Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)Projectile.timeLeft / 30f).Y * Projectile.width * Projectile.scale, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			if(Projectile.timeLeft > 30) return false;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			float rotation = MathHelper.Pi * (float)Projectile.timeLeft / 30f - base.Projectile.rotation + Main.GlobalTimeWrappedHourly * MathHelper.PiOver4;
			while(rotation > MathHelper.Pi) rotation -= MathHelper.TwoPi;
			while(rotation < -MathHelper.Pi) rotation += MathHelper.TwoPi;
			lightColor = Main.hslToRgb(System.Math.Abs(rotation / MathHelper.TwoPi) % 1f, 1f, 0.66f) * 0.7f;
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, base.Projectile.scale * new Vector2(2f * Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)Projectile.timeLeft / 30f).Y, Projectile.ai[0] / texture.Height + 4f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, base.Projectile.scale * new Vector2(1.75f * Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)Projectile.timeLeft / 30f).Y, Projectile.ai[0] / texture.Height + 4f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, new Color(100, 100, 100, 0), Projectile.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, base.Projectile.scale * new Vector2(1.5f * Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)Projectile.timeLeft / 30f).Y, Projectile.ai[0] / texture.Height + 4f) * 0.9f, SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, Color.Black, Projectile.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, base.Projectile.scale * new Vector2(1.25f * Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)Projectile.timeLeft / 30f).Y, Projectile.ai[0] / texture.Height + 4f) * 0.7f, SpriteEffects.None, 0);
			return false;
		}
		public override bool? CanDamage() => Projectile.ai[0] > 0f ? null : false;
		public override bool ShouldUpdatePosition() => Projectile.ai[0] == 0f;
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => overWiresUI.Add(index);
	}
}