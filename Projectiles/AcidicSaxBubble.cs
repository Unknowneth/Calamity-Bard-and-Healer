using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Projectiles;
using ThoriumMod.Projectiles.Bard;
using CalamityMod;

namespace CalamityBardHealer.Projectiles
{
	public class AcidicSaxBubble : BardProjectile
	{
		public override string Texture => "CalamityMod/Projectiles/Magic/AcidicSaxBubble";
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			Main.projFrames[base.Projectile.type] = 7;
			if(!ModLoader.TryGetMod("Redemption", out Mod mor)) return;
			mor.Call("addElementProj", 11, base.Projectile.type);
		}
		public override void SetBardDefaults() {
			base.Projectile.CloneDefaults(ModLoader.GetMod("CalamityMod").Find<ModProjectile>("AcidicSaxBubble").Type);
			base.Projectile.DamageType = BardDamage.Instance;
		}
		public override void AI() {
			if(++base.Projectile.frameCounter > 6) {
				if(++base.Projectile.frame > Main.projFrames[base.Projectile.type] - 1) base.Projectile.frame = 0;
				base.Projectile.frameCounter = 0;
			}
			if(base.Projectile.owner == Main.myPlayer) if(this.counter >= 120f) {
				this.counter = 0f;
				Vector2 mistRandDirection = Vector2.Normalize(Main.rand.NextVector2Circular(100f, 100f)) * (float)Main.rand.Next(50, 401) * 0.01f;
				int p = Projectile.NewProjectile(base.Projectile.GetSource_FromThis(), base.Projectile.Center, mistRandDirection, ModContent.ProjectileType<Projectiles.AcidicSaxMist>(), (int)Main.player[base.Projectile.owner].GetTotalDamage(BardDamage.Instance).ApplyTo(32f), 1f, base.Projectile.owner, 0f, 0f, 0f);
				NetMessage.SendData(27, -1, -1, null, base.Projectile.whoAmI);
			}
			else this.counter++;
			if(base.Projectile.ai[0] == 0f) {
				base.Projectile.ai[1]++;
				if(base.Projectile.ai[1] >= 6f) {
					if(base.Projectile.alpha > 0) base.Projectile.alpha -= 20;
					if(base.Projectile.alpha < 80) base.Projectile.alpha = 80;
				}
				if(base.Projectile.ai[1] >= 45f) {
					base.Projectile.ai[1] = 45f;
					if(this.counter2 < 1f) {
						this.counter2 += 0.002f;
						base.Projectile.scale += 0.002f;
						base.Projectile.width = (int)(30f * base.Projectile.scale);
						base.Projectile.height = (int)(30f * base.Projectile.scale);
					}
					else {
						base.Projectile.width = 60;
						base.Projectile.height = 60;
					}
					if(base.Projectile.wet) {
						if(base.Projectile.velocity.Y > 0f) base.Projectile.velocity.Y = base.Projectile.velocity.Y * 0.98f;
						if(base.Projectile.velocity.Y > -1f) base.Projectile.velocity.Y = base.Projectile.velocity.Y - 0.2f;
					}
					else if(base.Projectile.velocity.Y > -2f) base.Projectile.velocity.Y = base.Projectile.velocity.Y - 0.05f;
				}
				this.killCounter++;
				if(this.killCounter >= 200) base.Projectile.Kill();
			}
			if(Projectile.ai[0] == 1f) {
				int timeLeft = 15;
				bool killProj = false;
				bool spawnDust = false;
				Projectile.tileCollide = false;
				Projectile.localAI[0] += 1f;
				if (Projectile.localAI[0] % 30f == 0f) spawnDust = true;
				int npcIndex = (int)Projectile.ai[1];
				NPC npc = Main.npc[npcIndex];
				if (Projectile.localAI[0] >= (float)(60 * timeLeft)) killProj = true;
				else if(!npcIndex.WithinBounds(Main.maxNPCs)) killProj = true;
				else if(npc.active && !npc.dontTakeDamage) {
					Projectile.Center = npc.Center - Projectile.velocity * 2f;
					Projectile.gfxOffY = npc.gfxOffY;
					if (spawnDust) npc.HitEffect(0, 1.0, null);
				}
				else killProj = true;
				if(killProj) Projectile.Kill();
			}
		}
		public override void BardModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers) {
			Player player = Main.player[Projectile.owner];
			Rectangle myRect = Projectile.Hitbox;
			int maxStick = 3;
			if(Projectile.owner == Main.myPlayer) for(int npcIndex = 0; npcIndex < Main.maxNPCs; npcIndex++) {
				NPC npc = Main.npc[npcIndex];
				if (npc.active && !npc.dontTakeDamage && ((Projectile.friendly && (!npc.friendly || (npc.type == 22 && Projectile.owner < 255 && player.killGuide) || (npc.type == 54 && Projectile.owner < 255 && player.killClothier))) || (Projectile.hostile && npc.friendly && !npc.dontTakeDamageFromHostiles)) && (Projectile.owner < 0 || npc.immune[Projectile.owner] == 0 || Projectile.maxPenetrate == 1) && (npc.noTileCollide || !Projectile.ownerHitCheck)) {
					bool stickingToNPC;
					if (npc.type == 414) {
						Rectangle rect = npc.Hitbox;
						int crawltipedeHitboxMod = 8;
						rect.X -= crawltipedeHitboxMod;
						rect.Y -= crawltipedeHitboxMod;
						rect.Width += crawltipedeHitboxMod * 2;
						rect.Height += crawltipedeHitboxMod * 2;
						stickingToNPC = Projectile.Colliding(myRect, rect);
					}
					else stickingToNPC = Projectile.Colliding(myRect, npc.Hitbox);
					if(stickingToNPC) {
						if(npc.reflectsProjectiles && Projectile.CanBeReflected()) {
							npc.ReflectProjectile(Projectile);
							return;
						}
						Projectile.ai[0] = 1f;
						Projectile.ai[1] = (float)npcIndex;
						Projectile.velocity = npc.Center - Projectile.Center;
						Projectile.netUpdate = true;
						Point[] array2 = new Point[maxStick];
						int projCount = 0;
						for (int projIndex = 0; projIndex < Main.maxProjectiles; projIndex++) {
							Projectile proj = Main.projectile[projIndex];
							if (projIndex != Projectile.whoAmI && proj.active && proj.owner == Main.myPlayer && proj.type == Projectile.type && proj.ai[0] == 1f && proj.ai[1] == (float)npcIndex) {
								array2[projCount++] = new Point(projIndex, proj.timeLeft);
								if (projCount >= array2.Length) break;
							}
						}
						if(projCount >= array2.Length) {
							int stuckProjAmt = 0;
							for(int i = 1; i < array2.Length; i++) {
								if (array2[i].Y < array2[stuckProjAmt].Y) stuckProjAmt = i;
								Main.projectile[array2[stuckProjAmt].X].Kill();
							}
						}
					}
				}
			}
		}
		public override bool? CanDamage() => base.Projectile.ai[0] != 1f ? base.CanDamage() : new bool?(false);
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) {
			if(targetHitbox.Width > 8 && targetHitbox.Height > 8) targetHitbox.Inflate(-targetHitbox.Width / 8, -targetHitbox.Height / 8);
			return null;
		}
		public override bool PreDraw(ref Color lightColor) {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.spriteBatch.Draw(texture, base.Projectile.Center - Main.screenPosition + new Vector2(0f, base.Projectile.gfxOffY), new Rectangle(0, texture.Height / Main.projFrames[base.Projectile.type] * base.Projectile.frame, texture.Width, texture.Height / Main.projFrames[base.Projectile.type]), base.Projectile.GetAlpha(lightColor), base.Projectile.rotation, new Vector2(texture.Width, texture.Height / Main.projFrames[base.Projectile.type]) / 2f, base.Projectile.scale, 0, 0f);
			return false;
		}
		public override void BardOnHitNPC(NPC target, NPC.HitInfo hit, int damageDone) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) target.AddBuff(calamity.Find<ModBuff>("Irradiated").Type, 180);
		}
		public override void OnKill(int timeLeft) {
			base.Projectile.position = base.Projectile.Center;
			base.Projectile.width = (base.Projectile.height = 64);
			base.Projectile.position.X = base.Projectile.position.X - (float)(base.Projectile.width / 2);
			base.Projectile.position.Y = base.Projectile.position.Y - (float)(base.Projectile.height / 2);
			SoundEngine.PlaySound(Terraria.ID.SoundID.Item54, base.Projectile.Center);
			for(int i = 0; i < 25; i++) {
				int toxicDust = Dust.NewDust(base.Projectile.position, base.Projectile.width, base.Projectile.height, 75, 0f, 0f, 0, default(Color), 1f);
				Main.dust[toxicDust].position = (Main.dust[toxicDust].position + base.Projectile.position) / 2f;
				Main.dust[toxicDust].velocity = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				Main.dust[toxicDust].velocity.Normalize();
				Main.dust[toxicDust].velocity *= (float)Main.rand.Next(1, 30) * 0.1f;
				Main.dust[toxicDust].alpha = base.Projectile.alpha;
			}
			base.Projectile.maxPenetrate = -1;
			base.Projectile.penetrate = -1;
			base.Projectile.usesLocalNPCImmunity = true;
			base.Projectile.localNPCHitCooldown = 10;
			base.Projectile.Damage();
		}
		public float counter;
		public float counter2;
		public int killCounter;
	}
}