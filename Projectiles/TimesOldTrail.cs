using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using System.Collections.Generic;

namespace CalamityBardHealer.Projectiles
{
	public class TimesOldTrail : ModProjectile
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityHunt");
		private Vector2 lastInkPoint = new(0f, 0f);
		private List<Vector2> inkPoints = new();
		private List<Color> colorPoints = new();
		public override void SetStaticDefaults() {
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 1, base.Projectile.type);
			mor.Call("addElementProj", 8, base.Projectile.type);
			mor.Call("addElementProj", 14, base.Projectile.type);
		}
		public override void SetDefaults() {
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
			Projectile.DamageType = ThoriumMod.HealerDamage.Instance;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 8;
			Projectile.penetrate = -1;
		}
		public override void AI() {
			if(lastInkPoint == Vector2.Zero) return;
			inkPoints.Add(lastInkPoint);
			lastInkPoint = Vector2.Zero;
			colorPoints.Add(Main.DiscoColor);
		}
		public override bool PreDraw(ref Color lightColor) {
			if(inkPoints is null || inkPoints.Count == 0) return false;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Vector2 offset = Projectile.Size * 0.5f - Main.screenPosition;
			float fadeOut = MathHelper.Min(1f, (float)Projectile.timeLeft / 10f);
			for(int i = 0; i < inkPoints.Count; i++) Main.EntitySpriteDraw(texture, inkPoints[i] + offset, null, colorPoints[i], 0f, texture.Size() * 0.5f, Projectile.scale * fadeOut, SpriteEffects.None, 0);
			for(int i = 1; i < inkPoints.Count; i++) if(!inkPoints[i].HasNaNs() && inkPoints[i] != Vector2.Zero) Main.EntitySpriteDraw(texture, inkPoints[i] + offset, new Rectangle(texture.Width / 2, 0, 1, texture.Height), Color.Lerp(colorPoints[i], colorPoints[i - 1], 0.5f), (inkPoints[i] - inkPoints[i - 1]).ToRotation(), new Vector2(2, texture.Height) * 0.5f, new Vector2((inkPoints[i - 1] - inkPoints[i]).Length(), Projectile.scale * fadeOut), SpriteEffects.None, 0);
			return false;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if(inkPoints is null || inkPoints.Count == 0) return null;
			bool trailHit = false;
			for(int i = 0; i < inkPoints.Count; i++) trailHit |= targetHitbox.Intersects(new Rectangle((int)inkPoints[i].X, (int)inkPoints[i].Y, projHitbox.Width, projHitbox.Height));
			return trailHit;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			Player player = Main.player[Projectile.owner];
			if(player.HeldItem.ModItem is ThoriumMod.Items.HealerItems.ScytheItem s) {
				player.AddBuff(ModContent.BuffType<ThoriumMod.Buffs.Healer.SoulEssence>(), 1800, true, false);
				CombatText.NewText(target.Hitbox, new Color(100, 255, 200), s.scytheSoulCharge, false, true);
				player.GetModPlayer<ThoriumMod.ThoriumPlayer>().soulEssence += s.scytheSoulCharge;
			}
		}
		public override void SendExtraAI(BinaryWriter writer) {
			if(lastInkPoint == Vector2.Zero) return;
			writer.Write(lastInkPoint.X);
			writer.Write(lastInkPoint.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader) {
			if(lastInkPoint == Vector2.Zero) return;
			lastInkPoint.X = reader.ReadSingle();
			lastInkPoint.Y = reader.ReadSingle();
		}
		public void AddPoint(Vector2 inkPoint) {
			if(Main.myPlayer == Projectile.owner) {
				lastInkPoint = inkPoint;
				NetMessage.SendData(27, -1, -1, null, Projectile.whoAmI);
			}
			AI();
		}
	}
}