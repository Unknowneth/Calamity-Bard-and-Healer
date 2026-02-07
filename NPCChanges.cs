using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using CalamityMod;
using ThoriumMod.NPCs.BossThePrimordials;
using System;
using System.Collections.Generic;

namespace CalamityBardHealer
{
	public class NPCChanges : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => !ModLoader.HasMod("ThoriumRework") && !ModLoader.HasMod("RagnarokMod") && (npc.boss || npc.ModNPC is ThoriumMod.NPCs.BossStarScouter.ScouterCoreBase || (npc.ModNPC?.Name.StartsWith("FallenChampion") ?? false) || npc.ModNPC?.Name == "EncroachingEnergy" || npc.ModNPC?.Name == "BiteyBaby" || npc.ModNPC?.Name == "Beholder") && npc.ModNPC?.Mod.Name == "ThoriumMod";
		public override void SetDefaults(NPC npc) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) calamity.Call("SetDefenseDamageNPC", npc, true);
			if(npc.ModNPC?.Name.Contains("TheGrandThunderBird") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToCold = true;
				npc.Calamity().VulnerableToSickness = true;
				npc.Calamity().VulnerableToElectricity = false;
			}
			else if(npc.ModNPC?.Name.Contains("QueenJellyfish") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToWater = false;
				npc.Calamity().VulnerableToCold = true;
				npc.Calamity().VulnerableToSickness = true;
				npc.Calamity().VulnerableToElectricity = false;
			}
			else if(npc.ModNPC?.Name.Contains("Viscount") ?? false) {
				npc.Calamity().VulnerableToHeat = true;
				npc.Calamity().VulnerableToSickness = true;
			}
			else if(npc.ModNPC?.Name.Contains("StarScouter") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToWater = true;
				npc.Calamity().VulnerableToCold = false;
				npc.Calamity().VulnerableToSickness = false;
				npc.Calamity().VulnerableToElectricity = false;
			}
			else if(npc.ModNPC?.Name.Contains("BuriedChampion") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToCold = false;
				npc.Calamity().VulnerableToSickness = false;
				npc.Calamity().VulnerableToElectricity = true;
			}
			else if(npc.ModNPC?.Name.Contains("GraniteEnergyStorm") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToWater = true;
				npc.Calamity().VulnerableToCold = false;
				npc.Calamity().VulnerableToSickness = false;
				npc.Calamity().VulnerableToElectricity = false;
			}
			else if(npc.ModNPC?.Name.Contains("FallenBeholder") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToWater = true;
				npc.Calamity().VulnerableToCold = true;
			}
			else if(npc.ModNPC?.Name.Contains("BoreanStrider") ?? false) {
				npc.Calamity().VulnerableToHeat = true;
				npc.Calamity().VulnerableToWater = false;
				npc.Calamity().VulnerableToCold = false;
				npc.Calamity().VulnerableToSickness = true;
			}
			else if(npc.ModNPC?.Name.Contains("Lich") ?? false) {
				npc.Calamity().VulnerableToSickness = false;
				npc.Calamity().VulnerableToElectricity = false;
			}
			else if(npc.ModNPC?.Name.Contains("ForgottenOne") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToWater = false;
				npc.Calamity().VulnerableToCold = false;
				npc.Calamity().VulnerableToSickness = true;
				npc.Calamity().VulnerableToElectricity = true;
			}
			else if(npc.ModNPC?.Name.Contains("Aquaius") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToWater = false;
				npc.Calamity().VulnerableToCold = false;
				npc.Calamity().VulnerableToSickness = true;
				npc.Calamity().VulnerableToElectricity = true;
			}
			else if(npc.ModNPC?.Name.Contains("Omnicide") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToWater = false;
				npc.Calamity().VulnerableToCold = false;
				npc.Calamity().VulnerableToSickness = false;
				npc.Calamity().VulnerableToElectricity = false;
			}
			else if(npc.ModNPC?.Name.Contains("SlagFury") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToWater = true;
				npc.Calamity().VulnerableToCold = true;
				npc.Calamity().VulnerableToSickness = false;
				npc.Calamity().VulnerableToElectricity = false;
			}
			else if(npc.ModNPC?.Name.Contains("DreamEater") ?? false) {
				npc.Calamity().VulnerableToHeat = false;
				npc.Calamity().VulnerableToWater = false;
				npc.Calamity().VulnerableToCold = false;
				npc.Calamity().VulnerableToSickness = false;
				npc.Calamity().VulnerableToElectricity = false;
			}
		}
		public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment) {
			if(ModLoader.TryGetMod("CalamityMod", out Mod Calamity) && Calamity.Call("GetDifficultyActive", "BossRush") is bool b && b) {
				if(npc.ModNPC?.Name.Contains("TheGrandThunderBird") ?? false) npc.lifeMax *= 125;
				else if(npc.ModNPC?.Name.Contains("QueenJellyfish") ?? false) npc.lifeMax *= 115;
				else if(npc.ModNPC?.Name.Contains("Viscount") ?? false) npc.lifeMax *= 110;
				else if(npc.ModNPC?.Name.Contains("StarScouter") ?? false) npc.lifeMax *= 105;
				else if(npc.ModNPC?.Name.Contains("BuriedChampion") ?? false) npc.lifeMax *= 75;
				else if(npc.ModNPC?.Name.Contains("GraniteEnergyStorm") ?? false) npc.lifeMax *= 75;
				else if(npc.ModNPC?.Name.Contains("FallenBeholder") ?? false) npc.lifeMax *= 65;
				else if(npc.ModNPC?.Name.Contains("Lich") ?? false) npc.lifeMax *= 30;
				else if(npc.ModNPC?.Name.Contains("ForgottenOne") ?? false) npc.lifeMax *= 15;
				else if(npc.ModNPC?.Name.Contains("SlagFury") ?? false) npc.lifeMax *= 2;
				else if(npc.ModNPC?.Name.Contains("Aquaius") ?? false) npc.lifeMax *= 2;
				else if(npc.ModNPC?.Name.Contains("Omnicide") ?? false) npc.lifeMax *= 2;
				else if(npc.ModNPC?.Name.Contains("DreamEater") ?? false) npc.lifeMax *= 2;
				if(ModLoader.HasMod("FargowiltasCrossmod")) if(npc.lifeMax >= 100000000) npc.lifeMax /= 100;
				else if(npc.lifeMax >= 10000000) npc.lifeMax /= 10;
			}
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			if(ModLoader.HasMod("FargowiltasCrossmod")) return;
			List<(int, int, int, int)> dropCache = new();
			npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
				if(rule is ItemDropWithConditionRule c && c.condition is Conditions.IsMasterMode) {
					dropCache.Add((c.itemId, c.chanceDenominator, c.amountDroppedMinimum, c.amountDroppedMaximum));
					return true;
				}
				return false;
			}, true);
			foreach(var i in dropCache) npcLoot.DefineConditionalDropSet(DropHelper.RevAndMaster).Add(i.Item1, i.Item2, i.Item3, i.Item4, false);
			dropCache.Clear();
		}
		public override void AI(NPC npc) {
			if(!ModLoader.TryGetMod("CalamityMod", out Mod calamity) || !ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) return;
			bool revengeance = (bool)calamity.Call("GetDifficultyActive", "Revengeance");
			bool death = (bool)calamity.Call("GetDifficultyActive", "Death");
			if(!revengeance) return;
			ref float[] ai = ref npc.GetGlobalNPC<CalamityMod.NPCs.CalamityGlobalNPC>().newAI;
			if(npc.ModNPC is SlagFury) {
				float attackLerp = npc.ai[1] / 45f;
				int atk = (int)attackLerp;
				attackLerp -= attackLerp * 45f;
				if(npc.ai[1] > 0f && npc.ai[1] < 240f && npc.velocity != Vector2.Zero) npc.position += npc.velocity.RotatedBy(atk % 2 == 0 ? -MathHelper.PiOver2 : MathHelper.PiOver2) * Vector2.UnitX.RotatedBy(attackLerp / 45f * MathHelper.Pi).Y;
			}
			else if(npc.ModNPC is Aquaius aqua && (aqua.phase2 || aqua.phase3 || aqua.phase4)) {
				float attackLerp = npc.ai[1] / (aqua.phase3 || aqua.phase4 ? 45f : 60f);
				int atk = (int)attackLerp;
				attackLerp -= attackLerp * (aqua.phase3 || aqua.phase4 ? 45f : 60f);
				if(npc.ai[1] > 0f && npc.ai[1] < 240f && npc.velocity != Vector2.Zero) npc.position += npc.velocity.RotatedBy(atk % 2 == 0 ? MathHelper.PiOver2 : -MathHelper.PiOver2) * Vector2.UnitX.RotatedBy(attackLerp / (aqua.phase3 || aqua.phase4 ? 45f : 60f) * MathHelper.Pi).Y;
			}
			else if(npc.ModNPC is Omnicide omni) {
				if(omni.afterImage == 20) npc.velocity = Vector2.Normalize(Main.player[npc.target].Center + Main.player[npc.target].velocity * 10f - npc.Center) * (death ? (omni.phase3 ? 20f : omni.phase2 ? 18f : 16f) : 12f);
				else if(npc.ai[1] > 20f && npc.ai[1] % 10 == 0 && !(npc.ai[1] % 60 == 0) && npc.ai[1] < 240f && omni.afterImage == 0 && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + npc.velocity, Vector2.Normalize(Main.player[npc.target].Center + Main.player[npc.target].velocity * 16f - npc.Center) * 16f, thorium.Find<ModProjectile>("DreadSpiritPro").Type, 35, 0f, Main.myPlayer);
			}
			else if(npc.ModNPC is DreamEater) {
				if(death && npc.Distance(Main.player[npc.target].Center) < 650f && npc.ai[1] % 30f == 0f && npc.ai[1] > 0f && npc.ai[1] < 240f && npc.velocity != Vector2.Zero) npc.velocity *= death ? 1.4f : 1.2f;
				int lookFor = -1;
				if(npc.ai[0] == 0f) lookFor = thorium.Find<ModNPC>("UnstableAnger").Type;
				else if(npc.ai[0] == 1f) lookFor = thorium.Find<ModNPC>("ImpendingDread").Type;
				else if(npc.ai[0] == 2f) lookFor = thorium.Find<ModNPC>("InnerDespair").Type;
				else lookFor = Main.rand.Next(new int[]{thorium.Find<ModNPC>("UnstableAnger").Type, thorium.Find<ModNPC>("ImpendingDread").Type, thorium.Find<ModNPC>("InnerDespair").Type});
				if(npc.ai[1] % 45f == 0 && npc.ai[1] > 0f && npc.ai[1] < 240f && lookFor > 0 && Main.netMode != 1) for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].active && Main.npc[i].type == lookFor) if(lookFor == thorium.Find<ModNPC>("UnstableAnger").Type) Projectile.NewProjectile(npc.GetSource_FromAI(), Main.npc[i].Center, Vector2.Normalize(Main.npc[i].Center - npc.Center) * 12f, thorium.Find<ModProjectile>("FlameNova").Type, 35, 0f, Main.myPlayer);
				else if(lookFor == thorium.Find<ModNPC>("ImpendingDread").Type) Projectile.NewProjectile(npc.GetSource_FromAI(), Main.npc[i].Center, Vector2.Normalize(Main.player[npc.target].Center - Main.npc[i].Center) * 16f, thorium.Find<ModProjectile>("DreadSpiritPro").Type, 35, 0f, Main.myPlayer);
				else if(lookFor == thorium.Find<ModNPC>("InnerDespair").Type) Projectile.NewProjectile(npc.GetSource_FromAI(), Main.npc[i].Center, Vector2.Normalize(npc.Center - Main.npc[i].Center) * 6f, thorium.Find<ModProjectile>("AquaSplash").Type, 35, 0f, Main.myPlayer);
				if(npc.ai[0] == 3f && npc.ai[1] == 0f && Main.netMode != 1) for(int i = 0; i < 25; i++) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitX.RotatedBy(i / 25f * MathHelper.TwoPi) * 6f, thorium.Find<ModProjectile>("LucidMiasma").Type, 50, 0f, Main.myPlayer);
			}
			else if(npc.noTileCollide) npc.position += npc.velocity * (death ? Main.masterMode ? 0.5f : 0.25f : 0.125f);
			if((Main.masterMode && death) || ++ai[3] > (death ? 2 : 4)) {
				if(npc.ModNPC?.PreAI() == true) npc.ModNPC?.AI();
				npc.ModNPC?.PostAI();
				ai[3] = 0;
			}
			else return;
			if(npc.ModNPC is SlagFury) {
				float attackLerp = npc.ai[1] / 45f;
				int atk = (int)attackLerp;
				attackLerp -= attackLerp * 45f;
				if(npc.ai[1] > 0f && npc.ai[1] < 240f && npc.velocity != Vector2.Zero) npc.position += npc.velocity.RotatedBy(atk % 2 == 0 ? -MathHelper.PiOver2 : MathHelper.PiOver2) * Vector2.UnitX.RotatedBy(attackLerp / 45f * MathHelper.Pi).Y;
			}
			else if(npc.ModNPC is Aquaius aqua) {
				if(!aqua.phase2 && !aqua.phase3 && !aqua.phase4) return;
				float attackLerp = npc.ai[1] / (aqua.phase3 || aqua.phase4 ? 45f : 60f);
				int atk = (int)attackLerp;
				attackLerp -= attackLerp * (aqua.phase3 || aqua.phase4 ? 45f : 60f);
				if(npc.ai[1] > 0f && npc.ai[1] < 240f && npc.velocity != Vector2.Zero) npc.position += npc.velocity.RotatedBy(atk % 2 == 0 ? MathHelper.PiOver2 : -MathHelper.PiOver2) * Vector2.UnitX.RotatedBy(attackLerp / (aqua.phase3 || aqua.phase4 ? 45f : 60f) * MathHelper.Pi).Y;
			}
			else if(npc.ModNPC is Omnicide omni) {
				if(omni.afterImage == 20) npc.velocity = Vector2.Normalize(Main.player[npc.target].Center + Main.player[npc.target].velocity * 10f - npc.Center) * (death ? (omni.phase3 ? 20f : omni.phase2 ? 18f : 16f) : 12f);
				else if(npc.ai[1] > 20f && npc.ai[1] % 10 == 0 && !(npc.ai[1] % 60 == 0) && npc.ai[1] < 240f && omni.afterImage == 0 && Main.netMode != 1) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center + npc.velocity, Vector2.Normalize(Main.player[npc.target].Center + Main.player[npc.target].velocity * 16f - npc.Center) * 16f, thorium.Find<ModProjectile>("DreadSpiritPro").Type, 35, 0f, Main.myPlayer);
			}
			else if(npc.ModNPC is DreamEater) {
				if(death && npc.Distance(Main.player[npc.target].Center) < 650f && npc.ai[1] % 30f == 0f && npc.ai[1] > 0f && npc.ai[1] < 240f && npc.velocity != Vector2.Zero) npc.velocity *= death ? 1.4f : 1.2f;
				int lookFor = -1;
				if(npc.ai[0] == 0f) lookFor = thorium.Find<ModNPC>("UnstableAnger").Type;
				else if(npc.ai[0] == 1f) lookFor = thorium.Find<ModNPC>("ImpendingDread").Type;
				else if(npc.ai[0] == 2f) lookFor = thorium.Find<ModNPC>("InnerDespair").Type;
				else lookFor = Main.rand.Next(new int[]{thorium.Find<ModNPC>("UnstableAnger").Type, thorium.Find<ModNPC>("ImpendingDread").Type, thorium.Find<ModNPC>("InnerDespair").Type});
				if(npc.ai[1] % 45f == 0 && npc.ai[1] > 0f && npc.ai[1] < 240f && lookFor > 0 && Main.netMode != 1) for(int i = 0; i < Main.maxNPCs; i++) if(Main.npc[i].active && Main.npc[i].type == lookFor) if(lookFor == thorium.Find<ModNPC>("UnstableAnger").Type) Projectile.NewProjectile(npc.GetSource_FromAI(), Main.npc[i].Center, Vector2.Normalize(Main.npc[i].Center - npc.Center) * 12f, thorium.Find<ModProjectile>("FlameNova").Type, 35, 0f, Main.myPlayer);
				else if(lookFor == thorium.Find<ModNPC>("ImpendingDread").Type) Projectile.NewProjectile(npc.GetSource_FromAI(), Main.npc[i].Center, Vector2.Normalize(Main.player[npc.target].Center - Main.npc[i].Center) * 16f, thorium.Find<ModProjectile>("DreadSpiritPro").Type, 35, 0f, Main.myPlayer);
				else if(lookFor == thorium.Find<ModNPC>("InnerDespair").Type) Projectile.NewProjectile(npc.GetSource_FromAI(), Main.npc[i].Center, Vector2.Normalize(npc.Center - Main.npc[i].Center) * 6f, thorium.Find<ModProjectile>("AquaSplash").Type, 35, 0f, Main.myPlayer);
				if(npc.ai[0] == 3f && npc.ai[1] == 0f && Main.netMode != 1) for(int i = 0; i < 25; i++) Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, Vector2.UnitX.RotatedBy(i / 25f * MathHelper.TwoPi) * 6f, thorium.Find<ModProjectile>("LucidMiasma").Type, 50, 0f, Main.myPlayer);
			}
		}
	}
}