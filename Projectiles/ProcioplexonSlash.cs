using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class ProcioplexonSlash : ModProjectile
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
			Projectile.extraUpdates = 6;
			Projectile.timeLeft = 60;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = -1;
			Projectile.ArmorPenetration = 75;
			Projectile.DamageType = ModLoader.GetMod("ThoriumMod").Find<DamageClass>("HealerDamage");
		}
		public override void AI() => Projectile.rotation = Projectile.velocity.ToRotation();
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) target.AddBuff(entropy.Find<ModBuff>("LifeOppress").Type, 600);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			float point = 0f;
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center - Projectile.velocity * (Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)Projectile.timeLeft / 60f).Y + 0.1f), Projectile.Center + Projectile.velocity * (Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)Projectile.timeLeft / 60f).Y + 0.1f), Projectile.width * Projectile.scale, ref point);
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			lightColor = Color.SlateBlue;
			lightColor.A = 0;
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, base.Projectile.scale * new Vector2(1f, Projectile.velocity.Length() * Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)Projectile.timeLeft / 60f).Y + 0.1f), SpriteEffects.None, 0);
			Main.EntitySpriteDraw(texture, base.Projectile.Center - Main.screenPosition, null, new Color(125, 125, 125, 0), Projectile.rotation + MathHelper.PiOver2, texture.Size() * 0.5f, base.Projectile.scale * new Vector2(1f, Projectile.velocity.Length() * Vector2.UnitX.RotatedBy(MathHelper.Pi * (float)Projectile.timeLeft / 60f).Y + 0.1f) * 0.9f, SpriteEffects.None, 0);
			return false;
		}
		public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => overWiresUI.Add(index);
	}
}