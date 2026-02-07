using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;

namespace CalamityBardHealer.Projectiles
{
	public class IceCube : ModProjectile
	{
		public override string Texture => "Terraria/Images/Item_664";
		public override void SetDefaults() {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.aiStyle = -1;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 300;
		}
		public override void AI() {
			for(int i = 0; i < Main.maxPlayers; i++) if(Projectile.Hitbox.Intersects(Main.player[i].Hitbox) && Main.player[i].active && !Main.player[i].dead && Main.player[i].statLife < Main.player[i].statLifeMax2 && i != Projectile.owner && Main.player[i].team == Main.player[Projectile.owner].team && HealerHelper.HealPlayer(Main.player[Projectile.owner], Main.player[i], 7, 60, true, delegate(Player p) { p.AddBuff(ModContent.BuffType<ThoriumMod.Buffs.Healer.Cured>(), 30, true, false); })) {
				Main.player[i].AddBuff(ModContent.BuffType<ThoriumMod.Buffs.Healer.Cured>(), 30, true, false);
				Projectile.Kill();
			}
			if(Projectile.velocity.Y != 0f) Projectile.rotation += Projectile.velocity.X / 15f;
			else Projectile.rotation = (float)(int)(Projectile.rotation / MathHelper.PiOver2) * MathHelper.PiOver2;
			Projectile.velocity.Y += 0.3f;
			Projectile.velocity.Y *= 0.97f;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) {
			fallThrough = Projectile.Center.Y <= Projectile.ai[0] || Projectile.velocity.Y < 0f;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity) {
			if(Projectile.velocity.Y != 0) Projectile.velocity.Y = 0;
			return false;
		}
	}
}