using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer
{
	public class ThorlamityPlayer : ModPlayer
	{
		public bool yharimsJam = false;
		public bool ataxiaBard = false;
		public bool fleshWings = false;
		public bool dreadWings = false;
		public bool dragonWings = false;
		public bool silvaHealer = false;
		public bool omniSpeaker = false;
		public bool ataxiaHealer = false;
		public bool daedalusBard = false;
		public bool screamingClam = false;
		public bool elementalBloom = false;
		public bool saintessStatue = false;
		public bool demonBloodWings = false;
		public bool intergelacticBard = false;
		public bool intergelacticHealer = false;
		public bool noisebringerGoliath = false;
		public int tarragonBeat = 0;
		public int iceCreamCoolDown = 0;
		public int tarragonHeartbeat = 0;
		public override void ResetEffects() {
			yharimsJam = false;
			ataxiaBard = false;
			fleshWings = false;
			dreadWings = false;
			dragonWings = false;
			silvaHealer = false;
			omniSpeaker = false;
			ataxiaHealer = false;
			daedalusBard = false;
			screamingClam = false;
			elementalBloom = false;
			saintessStatue = false;
			demonBloodWings = false;
			intergelacticBard = false;
			intergelacticHealer = false;
			noisebringerGoliath = false;
			if(iceCreamCoolDown > 0) iceCreamCoolDown--;
			else iceCreamCoolDown = 0;
		}
		public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo) {
			int s = drawInfo.drawPlayer.armor[10].IsAir ? 0 : 10;
			if(drawInfo.drawPlayer.armor[s].ModItem is ModItem m && m.Mod.Name == "CalamityBardHealer") if(m.Name == "VictideAmmoniteHat" || m.Name == "AugmentedAuricTeslaFeatheredHeadwear") drawInfo.helmetOffset -= Vector2.UnitY.RotatedBy(drawInfo.drawPlayer.headRotation) * drawInfo.drawPlayer.gravDir * 6f;
			else if(m.Name == "VoidFaquirBiretta") drawInfo.helmetOffset -= Vector2.UnitY.RotatedBy(drawInfo.drawPlayer.headRotation) * drawInfo.drawPlayer.gravDir * 4f;
			else if(m.Name == "VoidFaquirChapeau") drawInfo.helmetOffset -= Vector2.UnitY.RotatedBy(drawInfo.drawPlayer.headRotation) * drawInfo.drawPlayer.gravDir * 2f;
		}
		public override void RefreshInfoAccessoriesFromTeamPlayers(Player otherPlayer) {
			if(otherPlayer.GetModPlayer<ThorlamityPlayer>().omniSpeaker) this.omniSpeaker = true;
			if(otherPlayer.GetModPlayer<ThorlamityPlayer>().daedalusBard) this.daedalusBard = true;
		}
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource) {
			if(CalamityPlayer.areThereAnyDamnBosses) if(Player.GetModPlayer<ThoriumPlayer>().rebirthStatuette) Player.respawnTimer /= 2;
			else if(Player.GetModPlayer<ThoriumPlayer>().accSpiritsGrace) Player.respawnTimer = (int)(Player.respawnTimer * 2f / 3f);
			else foreach(Projectile p in Main.ActiveProjectiles) if(p.owner != Player.whoAmI && p.type == ModContent.ProjectileType<ThoriumMod.Projectiles.Healer.PhoenixStaffPro>() && p.DistanceSQ(Player.Center) < 1000000f) Player.respawnTimer -= (int)(Player.respawnTimer * p.ai[1]);
		}
		public override void OnHurt(Player.HurtInfo info) {
			if(silvaHealer && info.Damage >= Player.statLifeMax2 / 3) {
				int lowest = 0;
				int minLife = int.MaxValue;
				for(int i = 0; i < Main.maxPlayers; i++) if(Main.player[i].active && !Main.player[i].dead && i != Player.whoAmI && Main.player[i].statLife < minLife) {
					minLife = Main.player[i].statLife;
					lowest = i + 1;
				}
				if(lowest > 0) Main.player[lowest - 1].AddBuff(ModLoader.GetMod("ThoriumMod").Find<ModBuff>("SpiritualistBuff").Type, 1800);
			}
			if(ataxiaHealer && info.Damage > 75 && Player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.HydrothermalOasis>()] == 0 && Main.myPlayer == Player.whoAmI) {
				int p = Projectile.NewProjectile(Player.GetSource_OnHurt(Player), Player.MountedCenter, Vector2.Zero, ModContent.ProjectileType<Projectiles.HydrothermalOasis>(), 0, 0f, Player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			if(Player.HasBuff(ModContent.BuffType<Buffs.AlluringSong>())) {
				Player.statLife += 200;
				if(Player.statLife > Player.statLifeMax2) Player.statLife = Player.statLifeMax2;
				Player.HealEffect(250);
				Player.ClearBuff(ModContent.BuffType<Buffs.AlluringSong>());
				NetMessage.SendData(16, -1, -1, null, Player.whoAmI);
			}
		}
		public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Player.GetModPlayer<CalamityPlayer>().luxorsGift && (Player.HeldItem.CountsAsClass(BardDamage.Instance) || Player.HeldItem.CountsAsClass(HealerDamage.Instance)) && !Player.HeldItem.channel) {
				Projectile projectile = new Projectile();
				projectile.SetDefaults(Player.HeldItem.shoot);
				int p = -1;
				Vector2 shoot = Vector2.Normalize(Main.MouseWorld - Player.MountedCenter) * velocity.Length() * projectile.MaxUpdates;
				if(shoot.Length() <= 1f) shoot = Vector2.Normalize(Main.MouseWorld - Player.MountedCenter) * 16f;
				if(Player.HeldItem.CountsAsClass(HealerDamage.Instance)) p = Projectile.NewProjectile(source, Player.MountedCenter, shoot, ModContent.ProjectileType<Projectiles.LuxorsPrayer>(), damage / 4, knockback, Player.whoAmI);
				else if(Player.HeldItem.CountsAsClass(BardDamage.Instance) && Player.HeldItem.ModItem is ThoriumMod.Items.BardItem bardItem) p = Projectile.NewProjectile(source, Player.MountedCenter, shoot, ModContent.ProjectileType<Projectiles.LuxorsSong>(), damage / 2, knockback, Player.whoAmI, (float)(int)bardItem.InstrumentType);
				if(p >= 0) NetMessage.SendData(27, -1, -1, null, p);
			}
			return true;
		}
		public override void OnHitNPCWithProj(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone) {
			if(projectile.ModProjectile?.Mod.Name == "CatalystMod" && projectile.ModProjectile?.Name == "AstralRocksProj") {
				if(Main.rand.NextBool(5) && intergelacticHealer && Player.statLife < Player.statLifeMax2) {
					Player.statLife += Player.GetModPlayer<ThoriumPlayer>().healBonus;
					if(Player.statLife > Player.statLifeMax2) Player.statLife = Player.statLifeMax2;
					Player.HealEffect(Player.GetModPlayer<ThoriumPlayer>().healBonus);
				}
				if(Main.rand.NextBool(3) && intergelacticBard) Player.GetModPlayer<ThoriumPlayer>().HealInspiration(3);
			}
			if(projectile.DamageType == BardDamage.Instance) {
				if(ataxiaBard && projectile.type != ModContent.ProjectileType<Projectiles.HydrothermalEruption>() && hit.Crit && Player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.HydrothermalEruption>()] == 0 && Main.myPlayer == Player.whoAmI) {
					int p = Projectile.NewProjectile(Player.GetSource_OnHit(target), target.Center, projectile.velocity != Vector2.Zero ? Vector2.Normalize(projectile.velocity) : -Vector2.UnitY, ModContent.ProjectileType<Projectiles.HydrothermalEruption>(), 10 + (int)(Main.masterMode ? target.defense : Main.expertMode ? (float)target.defense * 0.75f : (float)target.defense * 0.5f), 0f, Player.whoAmI, target.whoAmI);
					NetMessage.SendData(27, -1, -1, null, p);
				}
				if(daedalusBard) {
					target.AddBuff(44, 240);
					target.AddBuff(324, 240);
				}
				if(omniSpeaker && !Main.rand.NextBool(3)) target.AddBuff(ModLoader.GetMod("CalamityMod").Find<ModBuff>("GodSlayerInferno").Type, 300);
			}
			else {
				if(daedalusBard && Main.rand.NextBool(3)) target.AddBuff(324, 240);
				if(omniSpeaker && Main.rand.NextBool(3)) target.AddBuff(ModLoader.GetMod("CalamityMod").Find<ModBuff>("GodSlayerInferno").Type, 180);
			}
			if(projectile.DamageType == HealerDamage.Instance) {
				if(saintessStatue) target.AddBuff(189, 300);
				if(elementalBloom) target.AddBuff(ModLoader.GetMod("CalamityMod").Find<ModBuff>("ElementalMix").Type, 300);
			}
			if(fleshWings && Main.rand.NextBool(5)) {
				int healing = damageDone / 10;
				if(healing < 5) healing = 5;
				Player.HealEffect(healing);
				Player.statLife += healing;
				if(Player.statLife > Player.statLifeMax2) Player.statLife = Player.statLifeMax2;
			}
			if(dragonWings) target.AddBuff(39, 300);
		}
		public override void PostUpdateEquips() {
			if(Player.GetModPlayer<CalamityPlayer>().rogueStealthMax > 0f) {
				if(Player.GetModPlayer<ThoriumPlayer>().GetEmpTimer<ResourceRegen>().timer > 0) Player.GetModPlayer<CalamityPlayer>().accStealthGenBoost += (float)Player.GetModPlayer<ThoriumPlayer>().GetEmpTimer<ResourceRegen>().level * 0.05f;
				if(Player.GetModPlayer<ThoriumPlayer>().GetEmpTimer<ResourceConsumptionChance>().timer > 0) Player.GetModPlayer<CalamityPlayer>().flatStealthLossReduction += Player.GetModPlayer<ThoriumPlayer>().GetEmpTimer<ResourceConsumptionChance>().level * 2;
				if(Player.GetModPlayer<ThoriumPlayer>().GetEmpTimer<ResourceMaximum>().timer > 0) Player.GetModPlayer<CalamityPlayer>().rogueStealthMax += Player.GetModPlayer<CalamityPlayer>().rogueStealthMax * (float)Player.GetModPlayer<ThoriumPlayer>().GetEmpTimer<ResourceMaximum>().level * 0.05f;
			}
			if(!ModContent.GetInstance<BalanceConfig>().rogueThrowerMerge) {
				if(Player.GetModPlayer<CalamityPlayer>().omegaBlueSet) {
					Player.GetModPlayer<ThoriumPlayer>().bardResourceMax2 += 12;
					Player.GetModPlayer<ThoriumPlayer>().healBonus += 7;
					Player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.ItemDescriptions.OmegaBlueSetNoThrower") + "\n" + Player.setBonus;
				}
				return;
			}
			if(Player.GetModPlayer<CalamityPlayer>().omegaBlueSet) {
				Player.GetModPlayer<ThoriumPlayer>().techPointsMax++;
				Player.GetModPlayer<ThoriumPlayer>().bardResourceMax2 += 12;
				Player.GetModPlayer<ThoriumPlayer>().healBonus += 7;
				Player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.ItemDescriptions.OmegaBlueSet") + "\n" + Player.setBonus;
			}
			if(Player.GetModPlayer<CalamityPlayer>().godSlayerThrowing || Player.GetModPlayer<CalamityPlayer>().dsSetBonus) Player.GetModPlayer<ThoriumPlayer>().techPointsMax++;
			if(Player.GetModPlayer<CalamityPlayer>().wearingRogueArmor) Player.GetModPlayer<ThoriumPlayer>().techPointsMax++;
		}
		public override void PostUpdateBuffs() {
			if(Player.GetModPlayer<CalamityPlayer>().rageModeActive) {
				if(dreadWings) Player.GetDamage(DamageClass.Generic) += 0.5f;
				if(demonBloodWings && Player.statLife < Player.statLifeMax2) Player.statLife++;
				Player.GetModPlayer<ThoriumPlayer>().bardResource++;
			}
			if(Player.GetModPlayer<CalamityPlayer>().adrenalineModeActive) Player.GetModPlayer<ThoriumPlayer>().healBonus += (int)(Player.GetModPlayer<ThoriumPlayer>().healBonus * 2);
		}
	}
}