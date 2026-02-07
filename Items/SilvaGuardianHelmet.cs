using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class SilvaGuardianHelmet : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 24;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("CosmicPurple").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 28);
			Item.defense = 28;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.42f;
			player.GetDamage(DamageClass.Generic) -= 0.3f;
			player.GetCritChance(HealerDamage.Instance) += 12;
			player.lifeRegen += 5;
			player.statLifeMax2 += 100;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 15;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("PlantyMush").Type, 6).AddIngredient(calamity.Find<ModItem>("EffulgentFeather").Type, 9).AddIngredient(calamity.Find<ModItem>("AscendantSpiritEssence").Type, 2).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
		public override void UpdateArmorSet(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.32f;
			player.GetAttackSpeed(ThoriumDamageBase<HealerTool>.Instance) += 0.11f;
			player.GetModPlayer<ThorlamityPlayer>().silvaHealer = true;
			player.GetModPlayer<CalamityPlayer>().silvaSet = true;
			player.setBonus = Language.GetTextValue("Mods.CalamityBardHealer.Items.SilvaGuardianHelmet.SetBonus") + "\n" + Language.GetTextValue("Mods.CalamityMod.Items.Armor.PostMoonLord.SilvaArmor.CommonSetBonus");
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("SilvaArmor").Type && legs.type == calamity.Find<ModItem>("SilvaLeggings").Type : true;
	}
}