using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class BloodflareSirenSkull : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 26;
			Item.height = 28;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("PureGreen").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 26);
			Item.defense = 26;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.10f;
			player.GetCritChance(BardDamage.Instance) += 10;
			player.GetAttackSpeed(BardDamage.Instance) += 0.12f;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("BloodstoneCore").Type, 11).AddIngredient(calamity.Find<ModItem>("RuinousSoul").Type, 2).AddTile(412).Register();
		}

		public override void UpdateArmorSet(Player player) {
			ModKeybind armorKey = ThoriumHotkeySystem.ArmorKey;
			if(Main.myPlayer == player.whoAmI && !player.HasBuff(ModContent.BuffType<Buffs.AlluringSong>()) && !player.HasBuff(ModContent.BuffType<Buffs.AlluringSongCD>()) && armorKey.JustPressed) player.AddBuff(ModContent.BuffType<Buffs.AlluringSong>(), 300);
			player.crimsonRegen = true;
			player.GetModPlayer<CalamityPlayer>().bloodflareSet = true;
			player.GetModPlayer<ThoriumPlayer>().bardBuffDuration += 240;
			player.GetDamage(BardDamage.Instance) += 0.08f;
			player.GetCritChance(BardDamage.Instance) += 5;
			player.GetAttackSpeed(BardDamage.Instance) += 0.05f;
			player.GetModPlayer<ThoriumPlayer>().bardResourceMax2 += 4;
			string assignedKeys = "";
			for(int i = 0; i < armorKey.GetAssignedKeys().Count; i++) if(i < armorKey.GetAssignedKeys().Count - 1) assignedKeys += armorKey.GetAssignedKeys()[i] + ", ";
			else assignedKeys += armorKey.GetAssignedKeys()[i];
			player.setBonus = Terraria.Localization.Language.GetText("Mods.CalamityBardHealer.Items.BloodflareSirenSkull.SetBonus").Format(new object[]{assignedKeys}).ToString() + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.PostMoonLord.BloodflareBodyArmor.CommonSetBonus");
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadowSubtle = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("BloodflareBodyArmor").Type && legs.type == calamity.Find<ModItem>("BloodflareCuisses").Type : true;
	}
}