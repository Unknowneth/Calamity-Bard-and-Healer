using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class AerospecHeadphones : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 20;
			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 80);
			Item.defense = 3;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.08f;
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.08f;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetModPlayer<ThoriumPlayer>().bardHomingSpeedBonus += 0.1f;
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.05f;
			player.GetModPlayer<CalamityPlayer>().aeroSet = true;
			player.noFallDmg = true;
			player.moveSpeed += 0.05f;
			player.GetModPlayer<ThoriumPlayer>().accWindHoming = true;
			player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.AerospecHeadphones.SetBonus") + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.PreHardmode.AerospecBreastplate.CommonSetBonus");
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("AerialiteBar").Type, 5).AddIngredient(824, 3).AddIngredient(320, 1).AddTile(305).Register();
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("AerospecBreastplate").Type && legs.type == calamity.Find<ModItem>("AerospecLeggings").Type : false;
	}
}