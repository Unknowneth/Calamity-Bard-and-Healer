using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace CalamityBardHealer
{
	public class TreasureBags : GlobalItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityMod");
		public override bool AppliesToEntity(Item item, bool lateInstatiation) => (item.ModItem?.Mod.Name == "CalamityMod" && (item.ModItem?.Name == "AstralCrate" || item.ModItem?.Name == "AquaticScourgeBag" || item.ModItem?.Name == "DesertScourgeBag" || item.ModItem?.Name == "CrabulonBag" || item.ModItem?.Name == "HiveMindBag" || item.ModItem?.Name == "PerforatorBag" || item.ModItem?.Name == "SlimeGodBag" || item.ModItem?.Name == "CryogenBag" || item.ModItem?.Name == "AstrumAureusBag" || item.ModItem?.Name == "LeviathanBag" || item.ModItem?.Name == "CalamitasCloneBag" || item.ModItem?.Name == "PlaguebringerGoliathBag" || item.ModItem?.Name == "RavagerBag" || item.ModItem?.Name == "ProvidenceBag" || item.ModItem?.Name == "PolterghastBag" || item.ModItem?.Name == "OldDukeBag" || item.ModItem?.Name == "DevourerofGodsBag" || item.ModItem?.Name == "YharonBag")) || (item.ModItem?.Mod.Name == "InfernumMode" && item.ModItem?.Name == "BereftVassalBossBag") || (item.ModItem?.Mod.Name == "CalamityHunt" && (item.ModItem?.Name.StartsWith("Treasure") ?? false));
		public override void ModifyItemLoot(Item item, ItemLoot itemLoot) {
			if(item.ModItem?.Name == "AstralCrate") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is LeadingConditionRule lead) foreach(IItemDropRuleChainAttempt chains in lead.ChainedRules) if(chains.RuleToChain is OneFromOptionsDropRule c) {
						int newMaxSize = c.dropIds.Length + 1;
						System.Array.Resize(ref c.dropIds, newMaxSize);
						c.dropIds[newMaxSize - 1] = ModContent.ItemType<Items.StarCluster>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.StarCluster>(), 10));
			}
			else if(item.ModItem?.Name == "AquaticScourgeBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.CottonMouth>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.CottonMouth>(), 3));
			}
			else if(item.ModItem?.Name == "DesertScourgeBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.DryMouth>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.DryMouth>(), 3));
			}
			else if(item.ModItem?.Name == "CrabulonBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.HyphaeBaton>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.InfestedCastanet>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.HyphaeBaton>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.InfestedCastanet>(), 3));
			}
			else if(item.ModItem?.Name == "HiveMindBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.FilthyFlute>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.FilthyFlute>(), 3));
			}
			else if(item.ModItem?.Name == "PerforatorBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.Violince>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Violince>(), 3));
			}
			else if(item.ModItem?.Name == "SlimeGodBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.ReturntoSludge>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.GelatinTherapy>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.ReturntoSludge>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.GelatinTherapy>(), 3));
			}
			else if(item.ModItem?.Name == "CryogenBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.SchoolNurse>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.ArcticReinforcement>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.SchoolNurse>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.ArcticReinforcement>(), 3));
			}
			else if(item.ModItem?.Name == "AstrumAureusBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.Trinity>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.InterstellarShredder>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.InterstellarShredder>(), 3));
			}
			else if(item.ModItem?.Name == "LeviathanBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						c.stacks[2] = ModContent.ItemType<Items.AnahitasArpeggio>();
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.TidalForce>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.TidalForce>(), 3));
			}
			else if(item.ModItem?.Name == "CalamitasCloneBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.FireHazard>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.FireHazard>(), 3));
			}
			else if(item.ModItem?.Name == "PlaguebringerGoliathBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 3;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 3] = ModContent.ItemType<Items.SARS>();
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.OmnicidesLaw>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.FlightoftheGoliath>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.SARS>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.OmnicidesLaw>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.FlightoftheGoliath>(), 3));
			}
			else if(item.ModItem?.Name == "RavagerBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.SoulSplicer>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.SoulSplicer>(), 3));
			}
			else if(item.ModItem?.Name == "BereftVassalBossBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.DustDevilDrums>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.DesertedDrugDeal>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.DustDevilDrums>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.DesertedDrugDeal>(), 3));
			}
			else if(item.ModItem?.Name == "ProvidenceBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 1;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.Transfiguration>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.Transfiguration>(), 3));
			}
			else if(item.ModItem?.Name == "PolterghastBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 3;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 3] = ModContent.ItemType<Items.DeathAdder>();
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.FeralKeytar>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.PurgatoriumPandemonium>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.DeathAdder>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.FeralKeytar>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.PurgatoriumPandemonium>(), 3));
			}
			else if(item.ModItem?.Name == "OldDukeBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.IrradiatedKusarigama>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.OldDukesWisdom>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.IrradiatedKusarigama>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.OldDukesWisdom>(), 3));
			}
			else if(item.ModItem?.Name == "DevourerofGodsBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.MilkyWay>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.SavingGrace>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.MilkyWay>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.SavingGrace>(), 3));
			}
			else if(item.ModItem?.Name == "YharonBag") {
				itemLoot.RemoveWhere(delegate(IItemDropRule rule) {
					if(rule is CalamityMod.DropHelper.AllOptionsAtOnceWithPityDropRule c) {
						int newMaxSize = c.stacks.Length + 2;
						System.Array.Resize(ref c.stacks, newMaxSize);
						c.stacks[newMaxSize - 2] = ModContent.ItemType<Items.PhoenicianBeak>();
						c.stacks[newMaxSize - 1] = ModContent.ItemType<Items.DoomsdayCatharsis>();
					}
					return false;
				}, false);
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.PhoenicianBeak>(), 3));
				//itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.DoomsdayCatharsis>(), 3));
			}
			else if(item.ModItem?.Name == "TreasureBucket" || item.ModItem?.Name == "TreasureTrunk") {
				//Only exception as Goozma uses vanilla loot drop conditions
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.HarmonyoftheOldGod>(), 3));
				itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<Items.TimesOldRoman>(), 3));
			}
		}
	}
}