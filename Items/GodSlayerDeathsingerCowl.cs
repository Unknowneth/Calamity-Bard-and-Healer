using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class GodSlayerDeathsingerCowl : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 28;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("CosmicPurple").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 28);
			Item.defense = 40;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.14f;
			player.GetCritChance(BardDamage.Instance) += 14;
			player.GetAttackSpeed(BardDamage.Instance) += 0.09f;
			player.GetModPlayer<ThoriumPlayer>().accWindHoming = true;
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.09f;
			player.GetModPlayer<ThoriumPlayer>().bardHomingSpeedBonus += 0.15f;
			player.GetModPlayer<ThoriumPlayer>().bardBounceBonus += 2;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("CosmiliteBar").Type, 7).AddIngredient(calamity.Find<ModItem>("AscendantSpiritEssence").Type, 2).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
		public override void UpdateArmorSet(Player player) {
			player.GetModPlayer<CalamityPlayer>().godSlayer = true;
			if(player.GetModPlayer<CalamityPlayer>().godSlayerDashHotKeyPressed || (player.dashDelay != 0 && player.GetModPlayer<CalamityPlayer>().LastUsedDashID == GodslayerArmorDash.ID)) {
				player.GetModPlayer<CalamityPlayer>().DeferredDashID = GodslayerArmorDash.ID;
				player.dash = 0;
			}
			player.GetModPlayer<ThoriumPlayer>().bardPercussionCritDamage += 0.2f;
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.18f;
			player.GetModPlayer<ThoriumPlayer>().bardRangeBoost += 250;
			player.AddBuff(ModContent.BuffType<Buffs.CalamityBell>(), 2);
			player.setBonus = Language.GetTextValue("Mods.CalamityBardHealer.Items.GodSlayerDeathsingerCowl.SetBonus") + "\n" + Language.GetText("Mods.CalamityMod.Items.Armor.PostMoonLord.GodSlayerChestplate.CommonSetBonus").Format(new object[]{ CalamityKeybinds.GodSlayerDashHotKey.TooltipHotkeyString(), CalamityMod.Items.Armor.GodSlayer.GodSlayerChestplate.DashCooldown}).ToString();
			player.setBonus = player.setBonus.Replace("ff00ff", Utils.Hex3(CalamityMod.NPCs.DevourerofGods.DevourerofGodsHead.SpecialMoveColor));
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("GodSlayerChestplate").Type && legs.type == calamity.Find<ModItem>("GodSlayerLeggings").Type : true;
	}
}