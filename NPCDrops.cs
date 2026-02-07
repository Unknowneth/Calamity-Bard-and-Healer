using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using CalamityMod;

namespace CalamityBardHealer
{
	public class NPCDrops : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation) => (npc.ModNPC?.Mod.Name == "CalamityMod" && (npc.ModNPC?.Name == "AstralProbe" || npc.ModNPC?.Name == "BelchingCoral" || npc.ModNPC?.Name == "GiantClam" || npc.ModNPC?.Name == "Clam" || npc.ModNPC?.Name == "AquaticScourgeHead" || npc.ModNPC?.Name == "DesertScourgeHead" || npc.ModNPC?.Name == "Crabulon" || npc.ModNPC?.Name == "HiveMind" || npc.ModNPC?.Name == "PerforatorHive" || npc.ModNPC?.Name == "SlimeGodCore" || npc.ModNPC?.Name == "Cryogen" || npc.ModNPC?.Name == "AstrumAureus" || npc.ModNPC?.Name == "AstrumDeusHead" || npc.ModNPC?.Name == "CalamitasClone" || npc.ModNPC?.Name == "Anahita" || npc.ModNPC?.Name == "Leviathan" || npc.ModNPC?.Name == "PlaguebringerGoliath" || npc.ModNPC?.Name == "RavagerBody" || npc.ModNPC?.Name == "Providence" || npc.ModNPC?.Name == "Polterghast" || npc.ModNPC?.Name == "OldDuke" || npc.ModNPC?.Name == "DevourerofGodsHead" || npc.ModNPC?.Name == "Yharon")) || (npc.ModNPC?.Mod.Name == "InfernumMode" && npc.ModNPC?.Name == "BereftVassal") || (npc.ModNPC?.Mod.Name == "CalamityHunt" && npc.ModNPC?.Name == "Goozma");
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot) {
			if(npc.ModNPC?.Name == "AstralProbe") {
				npcLoot.AddIf(() => DownedBossSystem.downedAstrumAureus, ModContent.ItemType<Items.StarCluster>(), 7, 1, 1, true, null);
				return;
			}
			if(npc.ModNPC?.Name == "BelchingCoral") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CommonDrop c && c.itemId == npc.ModNPC.Mod.Find<ModItem>("BelchingSaxophone").Type) c.itemId = ModContent.ItemType<Items.BelchingSaxophone>();
					return false;
				}, false);
				return;
			}
			if(npc.ModNPC?.Name.Contains("Clam") ?? false) {
				if(npc.ModNPC?.Name.Contains("Giant") ?? false) npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.ScreamingClam>(), 2));
				else npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.ScreamingClam>(), 7));
				return;
			}
			if(npc.ModNPC?.Name == "AstrumDeusHead") {
				if(ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) try {
					LeadingConditionRule lastWorm = npcLoot.DefineConditionalDropSet((DropAttemptInfo info) => !CalamityMod.NPCs.AstrumDeus.AstrumDeusHead.ShouldNotDropThings(info.npc));
					lastWorm.Add(DropHelper.NormalVsExpertQuantity(thorium.Find<ModItem>("CelestialFragment").Type, 1, 16, 24, 20, 32), false);
					lastWorm.Add(DropHelper.NormalVsExpertQuantity(thorium.Find<ModItem>("ShootingStarFragment").Type, 1, 16, 24, 20, 32), false);
					lastWorm.Add(DropHelper.NormalVsExpertQuantity(thorium.Find<ModItem>("WhiteDwarfFragment").Type, 1, 16, 24, 20, 32), false);
				}
				catch {
				}
				return;
			}
			if(npc.ModNPC?.Name == "AquaticScourgeHead") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.CottonMouth>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.CottonMouth>(), 4));
			}
			else if(npc.ModNPC?.Name == "DesertScourgeHead") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.DryMouth>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.DryMouth>(), 4));
			}
			else if(npc.ModNPC?.Name == "Crabulon") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.HyphaeBaton>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.InfestedCastanet>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.HyphaeBaton>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.InfestedCastanet>(), 4));
			}
			else if(npc.ModNPC?.Name == "HiveMind") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.FilthyFlute>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.FilthyFlute>(), 4));
			}
			else if(npc.ModNPC?.Name == "PerforatorHive") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.Violince>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Violince>(), 4));
			}
			else if(npc.ModNPC?.Name == "SlimeGodCore") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.ReturntoSludge>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.GelatinTherapy>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.ReturntoSludge>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.GelatinTherapy>(), 4));
			}
			else if(npc.ModNPC?.Name == "Cryogen") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.SchoolNurse>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.ArcticReinforcement>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.SchoolNurse>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.ArcticReinforcement>(), 4));
			}
			else if(npc.ModNPC?.Name == "AstrumAureus") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.Trinity>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.InterstellarShredder>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.InterstellarShredder>(), 4));
			}
			else if(npc.ModNPC?.Name == "Anahita" || npc.ModNPC?.Name == "Leviathan") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is LeadingConditionRule lead2 && lead2.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains2 in lead2.ChainedRules) if(chains2.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						c.stacks[2] = ModContent.ItemType<Items.AnahitasArpeggio>();
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.TidalForce>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.TidalForce>(), 4));
			}
			else if(npc.ModNPC?.Name == "CalamitasClone") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.FireHazard>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.FireHazard>(), 4));
			}
			else if(npc.ModNPC?.Name == "PlaguebringerGoliath") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 3;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 3] = ModContent.ItemType<Items.SARS>();
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.OmnicidesLaw>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.FlightoftheGoliath>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.SARS>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.OmnicidesLaw>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.FlightoftheGoliath>(), 4));
			}
			else if(npc.ModNPC?.Name == "RavagerBody") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.SoulSplicer>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.SoulSplicer>(), 4));
			}
			else if(npc.ModNPC?.Name == "BereftVassal") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.DustDevilDrums>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.DesertedDrugDeal>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.DustDevilDrums>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.DesertedDrugDeal>(), 4));
			}
			else if(npc.ModNPC?.Name == "Providence") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.Transfiguration>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.Transfiguration>(), 4));
			}
			else if(npc.ModNPC?.Name == "Polterghast") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 3;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 3] = ModContent.ItemType<Items.DeathAdder>();
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.FeralKeytar>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.PurgatoriumPandemonium>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.DeathAdder>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.FeralKeytar>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.PurgatoriumPandemonium>(), 4));
			}
			else if(npc.ModNPC?.Name == "OldDuke") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.IrradiatedKusarigama>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.OldDukesWisdom>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.IrradiatedKusarigama>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.OldDukesWisdom>(), 4));
			}
			else if(npc.ModNPC?.Name == "DevourerofGodsHead") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.MilkyWay>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.SavingGrace>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.MilkyWay>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.SavingGrace>(), 4));
			}
			else if(npc.ModNPC?.Name == "Yharon") {
				npcLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead && lead.condition is Conditions.NotExpert) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.PhoenicianBeak>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.DoomsdayCatharsis>();
						return false;
					}
					return false;
				}, false);
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.PhoenicianBeak>(), 4));
				//normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.DoomsdayCatharsis>(), 4));
			}
			else if(npc.ModNPC?.Name == "Goozma") {
				LeadingConditionRule normalOnly = new LeadingConditionRule(new Conditions.NotExpert());
				normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.HarmonyoftheOldGod>(), 4));
				normalOnly.OnSuccess(ItemDropRule.Common(ModContent.ItemType<Items.TimesOldRoman>(), 4));
				npcLoot.Add(normalOnly);
			}
		}
	}
}