using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class AerospecBiretta : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 18;
			Item.rare = 3;
			Item.value = Item.sellPrice(silver: 80);
			Item.defense = 6;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.22f;
			player.GetDamage(DamageClass.Generic) -= 0.15f;
			player.GetAttackSpeed(HealerDamage.Instance) += 0.07f;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 2;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetModPlayer<CalamityPlayer>().aeroSet = true;
			if(player.velocity.Y != 0f) player.GetModPlayer<ThoriumPlayer>().healBonus += 3;
			player.noFallDmg = true;
			player.moveSpeed += 0.05f;
			player.GetDamage(HealerDamage.Instance) += 0.13f;
			player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.AerospecBiretta.SetBonus") + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.PreHardmode.AerospecBreastplate.CommonSetBonus");
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("AerialiteBar").Type, 5).AddIngredient(824, 3).AddIngredient(320, 1).AddTile(305).Register();
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadow = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("AerospecBreastplate").Type && legs.type == calamity.Find<ModItem>("AerospecLeggings").Type : false;
	}
}