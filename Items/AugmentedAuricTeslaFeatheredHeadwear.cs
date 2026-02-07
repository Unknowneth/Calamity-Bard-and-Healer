using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using ThoriumMod;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;
using CatalystMod;

namespace CalamityBardHealer.Items
{
	[ExtendsFromMod("CatalystMod")]
	[JITWhenModsEnabled("CatalystMod")]
	[AutoloadEquip(EquipType.Head)]
	public class AugmentedAuricTeslaFeatheredHeadwear : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 28;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else Item.rare = 10;
			Item.value = Item.sellPrice(gold: 30);
			Item.defense = 43;
			Item.maxStack = 1;
			Item.DamageType = BardDamage.Instance;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.25f;
			player.GetCritChance(BardDamage.Instance) += 20;
			player.GetAttackSpeed(BardDamage.Instance) += 0.12f;
			player.GetModPlayer<ThoriumPlayer>().bardHomingSpeedBonus += 0.2f;
			player.GetModPlayer<ThoriumPlayer>().bardHomingRangeBonus += 0.2f;
			player.GetModPlayer<ThoriumPlayer>().bardBounceBonus += 3;
		}
		public override void UpdateArmorSet(Player player) {
			player.crimsonRegen = true;
			player.GetModPlayer<CalamityPlayer>().bloodflareSet = true;
			player.GetModPlayer<CalamityPlayer>().godSlayer = true;
			player.GetModPlayer<CalamityPlayer>().auricSet = true;
			player.GetModPlayer<ThorlamityPlayer>().intergelacticBard = true;
			if(Main.myPlayer == player.whoAmI && !player.HasBuff(ModContent.BuffType<Buffs.AlluringSong>()) && !player.HasBuff(ModContent.BuffType<Buffs.AlluringSongCD>()) && ThoriumHotkeySystem.ArmorKey.JustPressed) player.AddBuff(ModContent.BuffType<Buffs.AlluringSong>(), 300);
			if(player.GetModPlayer<CalamityPlayer>().godSlayerDashHotKeyPressed || (player.dashDelay != 0 && player.GetModPlayer<CalamityPlayer>().LastUsedDashID == GodslayerArmorDash.ID)) {
				player.GetModPlayer<CalamityPlayer>().DeferredDashID = GodslayerArmorDash.ID;
				player.dash = 0;
			}
			try {
				using(System.Collections.Generic.List<string>.Enumerator enumerator = CatalystPlayer.AsteroidVisToggleKey.GetAssignedKeys(0).GetEnumerator()) {
					if(enumerator.MoveNext()) {
						string key = enumerator.Current;
						player.setBonus = Language.GetTextValue("Mods.CalamityBardHealer.Items.AugmentedAuricTeslaFeatheredHeadwear.SetBonus", Language.GetTextValue("Mods.CatalystMod.Common.BoundKey", key));
					}
					else player.setBonus = Language.GetTextValue("Mods.CalamityBardHealer.Items.AugmentedAuricTeslaFeatheredHeadwear.SetBonus", Language.GetTextValue("Mods.CatalystMod.Common.UnboundKey"));
				}
			}
			catch { }
			CatalystMod.Items.Armor.Intergelactic.IntergelacticHeadMelee.SetBonus(player, Item);
			player.GetModPlayer<ThoriumPlayer>().bardBuffDuration += 300;
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.2f;
			player.GetModPlayer<ThoriumPlayer>().bardResourceDropBoost += 0.45f;
			player.thorns += 3f;
			player.ignoreWater = true;
			player.AddBuff(ModContent.BuffType<Buffs.CalamityBell>(), 2);
		}
		public override void AddRecipes() {
			if(ModLoader.HasMod("CatalystMod")) if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.IntergelacticCloche>()).AddIngredient(ModContent.ItemType<Items.BloodflareSirenSkull>()).AddIngredient(ModContent.ItemType<Items.GodSlayerDeathsingerCowl>()).AddIngredient(calamity.Find<ModItem>("AuricBar").Type, 12).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
		public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips) {
			if(!Item.social) foreach(TooltipLine m in tooltips) if(m.Mod == "Terraria" && m.Name == "Defense") {
				m.Text = (Item.defense - 6).ToString() + "[c/70f4f4:(+6)]" + Lang.tip[25];
				break;
			}
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlines = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("AuricTeslaBodyArmor").Type && body.Catalyst().augmented && legs.type == calamity.Find<ModItem>("AuricTeslaCuisses").Type && legs.Catalyst().augmented : false;
	}
}