using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class DaedalusHat : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 26;
			Item.rare = 5;
			Item.value = Item.sellPrice(gold: 4, silver: 80);
			Item.defense = 11;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.13f;
			player.GetCritChance(BardDamage.Instance) += 7;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetAttackSpeed(BardDamage.Instance) += 0.05f;
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.1f;
			player.GetModPlayer<ThoriumPlayer>().bardBuffDuration += 180;
			player.GetModPlayer<ThorlamityPlayer>().daedalusBard = true;
			player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.DaedalusHat.SetBonus");
		}
		public override void ArmorSetShadows(Player player) {
			player.armorEffectDrawShadowSubtle = true;
			player.armorEffectDrawOutlines = true;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("CryonicBar").Type, 7).AddIngredient(calamity.Find<ModItem>("EssenceofEleum").Type, 1).AddTile(134).Register();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("DaedalusBreastplate").Type && legs.type == calamity.Find<ModItem>("DaedalusLeggings").Type : false;
	}
}