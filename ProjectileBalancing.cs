using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Graphics.CameraModifiers;

namespace CalamityBardHealer
{
	public class ProjectileBalancing : GlobalProjectile
	{
		public override bool AppliesToEntity(Projectile projectile, bool lateInstatiation) => projectile.ModProjectile?.Mod.Name == "ThoriumMod" && ((!ModLoader.HasMod("ThoriumRework") && (projectile.ModProjectile?.Name == "DreadSpiritPro" || (projectile.ModProjectile?.GetType().Namespace.Equals("ThoriumMod.Projectiles.Boss") ?? false))) || (projectile.ModProjectile?.Name.Contains("RealitySlasher") ?? false) || projectile.ModProjectile?.Name == "BlackMIDIPro");
		public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers) {
			if(target.ModNPC?.Mod.Name == "CalamityMod") if(projectile.ModProjectile?.Name.Contains("RealitySlasher") ?? false) {
				if(target.ModNPC?.Name.StartsWith("Ares") ?? false) modifiers.FinalDamage *= 0.25f;
				else if(target.ModNPC?.Name.StartsWith("SupremeCa") ?? false) modifiers.FinalDamage *= 0.8f;
				else if(target.ModNPC?.Name.StartsWith("Thanatos") ?? false) modifiers.FinalDamage *= 0.1f;
			}
			else if(projectile.ModProjectile?.Name == "BlackMIDIPro") {
				if(target.ModNPC?.Name.StartsWith("Ares") ?? false) modifiers.FinalDamage *= 0.6f;
				else if(target.ModNPC?.Name.StartsWith("SupremeCa") ?? false) modifiers.FinalDamage *= 0.3f;
				else if(target.ModNPC?.Name.StartsWith("Thanatos") ?? false) modifiers.FinalDamage *= 0.85f;
			}
		}
		public override void SetDefaults(Projectile projectile) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && projectile.hostile) calamity.Call("SetDefenseDamageProjectile", projectile, true);
			string n = projectile.ModProjectile?.Name ?? "";
			if(n == "DreadSpiritPro") projectile.tileCollide = false;
			else if(n == "MainBeamCheese" || n == "MainBeamOuter") projectile.timeLeft = 0;
			else if(n == "MainBeam" || n == "BioCoreBeam" || n == "CryoCoreBeam" || n == "MoltenCoreBeam") projectile.MaxUpdates = projectile.timeLeft;
		}
		public override bool PreAI(Projectile projectile) {
			string n = projectile.ModProjectile?.Name ?? "";
			if(!ModLoader.HasMod("ThoriumRework") && n == "ZRealitySlasherSlash") return false;
			else if(n == "MainBeamCheese" || n == "MainBeamOuter") {
				projectile.Kill();
				return false;
			}
			else if(n == "MainBeam") {
				if(projectile.localAI[0] == 0f && projectile.localAI[1] == 0f) {
					projectile.localAI[0] = projectile.Center.X - projectile.velocity.X;
					projectile.localAI[1] = projectile.Center.Y - projectile.velocity.Y;
					Main.instance.CameraModifiers.Add(new PunchCameraModifier(Main.LocalPlayer.Center, Vector2.UnitY, 8f, 10, 60, 1600f, "Laser Screenshake"));
					SoundEngine.PlaySound(SoundID.NPCDeath56, projectile.Center);
				}
				if(projectile.ai[0] > 0f || (projectile.timeLeft <= 2 && projectile.ai[0] == 0f)) {
					projectile.extraUpdates = 0;
					projectile.timeLeft = 2;
					projectile.velocity = Vector2.Zero;
					if(projectile.ai[0] < 20f * projectile.MaxUpdates) projectile.ai[0]++;
					else projectile.active = false;
				}
				else if(projectile.ai[1] == 0) {
					bool spawnProj = false;
					Vector2 platformCheck = new Vector2((int)(projectile.position.X / 16f), (int)(projectile.position.Y / 16f));
					for(int i = 0; i < projectile.width / 16; i++) {
						platformCheck.X += i;
						for(int j = 0; j < projectile.height / 16; j++) {
							platformCheck.Y += j;
							if(Main.tile[(int)platformCheck.X, (int)platformCheck.Y].HasUnactuatedTile && Main.tileSolidTop[Main.tile[(int)platformCheck.X, (int)platformCheck.Y].TileType]) spawnProj = true;
							if(spawnProj) break;
						}
						if(spawnProj) break;
					}
					if(spawnProj && Main.netMode != 1) {
						Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.UnitX * 6f, ModLoader.GetMod("ThoriumMod").Find<ModProjectile>("VaporizePulse").Type, 16, 0f, Main.myPlayer);
						Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.UnitX * -6f, ModLoader.GetMod("ThoriumMod").Find<ModProjectile>("VaporizePulse").Type, 16, 0f, Main.myPlayer);
					}
					if(spawnProj) projectile.ai[1] = projectile.velocity.Length();
				}
				else projectile.ai[1]--;
				return false;
			}
			else if(n == "BioCoreBeam" || n == "CryoCoreBeam" || n == "MoltenCoreBeam") {
				if(projectile.localAI[0] == 0f && projectile.localAI[1] == 0f) {
					projectile.localAI[0] = projectile.Center.X - projectile.velocity.X;
					projectile.localAI[1] = projectile.Center.Y - projectile.velocity.Y;
					SoundEngine.PlaySound(SoundID.Item12, projectile.Center);
				}
				if(projectile.ai[0] > 0f || (projectile.timeLeft <= 2 && projectile.ai[0] == 0f)) {
					projectile.extraUpdates = 0;
					projectile.timeLeft = 2;
					projectile.velocity = Vector2.Zero;
					if(projectile.ai[0] < 20f * projectile.MaxUpdates) projectile.ai[0]++;
					else projectile.active = false;
				}
				return false;
			}
			return base.PreAI(projectile);
		}
		public override Color? GetAlpha(Projectile projectile, Color lightColor) {
			if((projectile.ModProjectile?.GetType().Namespace.Equals("ThoriumMod.Projectiles.Boss") ?? false) && !(projectile.ModProjectile?.Name.StartsWith("Viscount") ?? false)) return Color.White * projectile.Opacity;
			return base.GetAlpha(projectile, lightColor);
		}
		public override bool PreDraw(Projectile projectile, ref Color lightColor) {
			string n;
			if(projectile.ModProjectile is ModProjectile m) n = m.Name;
			else return base.PreDraw(projectile, ref lightColor);
			if(n == "MainBeam" || n == "BioCoreBeam" || n == "CryoCoreBeam" || n == "MoltenCoreBeam") for(int i = 0; i < 2; i++) {
				float laserSize = Vector2.UnitX.RotatedBy(projectile.ai[0] / (20f * projectile.MaxUpdates) * MathHelper.Pi).Y * projectile.scale;
				if(i > 0) lightColor = new Color(200, 200, 200, 0);
				else if(n == "MainBeam") lightColor = Color.Lerp(new Color(189, 48, 255, 255), new Color(227, 168, 255, 255), laserSize);
				else if(n == "BioCoreBeam") lightColor = Color.Lime;
				else if(n == "CryoCoreBeam") lightColor = Color.Cyan;
				else if(n == "MoltenCoreBeam") lightColor = Color.Orange;
				laserSize *= 0.075f * projectile.Size.Length();
				if(i > 0) laserSize *= 0.5f;
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
				Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, texture.Height / 2 - 1, texture.Width, 1), lightColor, (new Vector2(projectile.localAI[0], projectile.localAI[1]) - projectile.Center).ToRotation() - MathHelper.PiOver2, new Vector2(texture.Width * 0.5f, 0f), new Vector2(laserSize, Vector2.Distance(new Vector2(projectile.localAI[0], projectile.localAI[1]), projectile.Center)), SpriteEffects.None, 0);
				lightColor *= 0.5f;
				Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() * 0.5f, laserSize, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * -MathHelper.Pi, texture.Size() * 0.5f, laserSize, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * MathHelper.Pi - MathHelper.PiOver2, texture.Size() * 0.5f, laserSize, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * -MathHelper.Pi + MathHelper.PiOver2, texture.Size() * 0.5f, laserSize, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, new Vector2(projectile.localAI[0], projectile.localAI[1]) - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * MathHelper.Pi, texture.Size() * 0.5f, laserSize, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, new Vector2(projectile.localAI[0], projectile.localAI[1]) - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * -MathHelper.Pi, texture.Size() * 0.5f, laserSize, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, new Vector2(projectile.localAI[0], projectile.localAI[1]) - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * MathHelper.Pi - MathHelper.PiOver2, texture.Size() * 0.5f, laserSize, SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, new Vector2(projectile.localAI[0], projectile.localAI[1]) - Main.screenPosition, null, lightColor, Main.GlobalTimeWrappedHourly * -MathHelper.Pi + MathHelper.PiOver2, texture.Size() * 0.5f, laserSize, SpriteEffects.None, 0);
			}
			else if(!ModLoader.HasMod("ThoriumRework") && n == "ZRealitySlasherSlash") {
				if(projectile.timeLeft < 2) return false;
				Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("Terraria/Images/Extra_98");
				Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, null, new Color(189, 48, 255, 0) * (MathHelper.Clamp(projectile.timeLeft - 2, 0f, 5f) / 5f), projectile.velocity.ToRotation() + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(projectile.scale * 0.8f, MathHelper.Clamp(projectile.timeLeft - 2, 0f, 5f) / 5f), SpriteEffects.None, 0);
				Main.EntitySpriteDraw(texture, projectile.Center - Main.screenPosition, null, new Color(227, 168, 255, 55) * (MathHelper.Clamp(projectile.timeLeft - 2, 0f, 5f) / 5f), projectile.velocity.ToRotation() + MathHelper.PiOver2, texture.Size() * 0.5f, new Vector2(projectile.scale * 0.6f, MathHelper.Clamp(projectile.timeLeft - 2, 0f, 5f) / 5f), SpriteEffects.None, 0);
				return false;
			}
			return base.PreDraw(projectile, ref lightColor);
		}
		public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity) {
			string n;
			if(projectile.ModProjectile is ModProjectile m) n = m.Name;
			else return base.OnTileCollide(projectile, oldVelocity);
			if(n == "EdgeofImaginationPro" && Main.myPlayer == projectile.owner) {
				int who = 0;
				float maxRange = 800f;
				for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].CanBeChasedBy(projectile, false) && Collision.CanHitLine(projectile.position - oldVelocity, projectile.width, projectile.height, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height) && Main.npc[i].Distance(projectile.Center) < maxRange) {
					maxRange = Main.npc[i].Distance(projectile.Center);
					who = i + 1;
				}
				if(who > 0) {
					projectile.velocity = Vector2.Normalize(Main.npc[who - 1].Center + Main.npc[who - 1].velocity - projectile.Center) * oldVelocity.Length();
					NetMessage.SendData(27, -1, -1, null, projectile.whoAmI);
					return false;
				}
			}
			else if(n == "MainBeam" || n == "BioCoreBeam" || n == "CryoCoreBeam" || n == "MoltenCoreBeam") {
				if(n == "MainBeam" && projectile.velocity != Vector2.Zero && Main.netMode != 1) {
					Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.UnitX * 6f, ModLoader.GetMod("ThoriumMod").Find<ModProjectile>("VaporizePulse").Type, 16, 0f, Main.myPlayer);
					Projectile.NewProjectile(projectile.GetSource_FromAI(), projectile.Center, Vector2.UnitX * -6f, ModLoader.GetMod("ThoriumMod").Find<ModProjectile>("VaporizePulse").Type, 16, 0f, Main.myPlayer);
				}
				projectile.extraUpdates = 0;
				projectile.tileCollide = false;
				projectile.velocity = Vector2.Zero;
				return false;
			}
			return base.OnTileCollide(projectile, oldVelocity);
		}
	}
}