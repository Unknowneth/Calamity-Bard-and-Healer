using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;
using CalamityMod.ExtraJumps;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class StatigelFoxMask : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 16;
			Item.height = 18;
			Item.rare = 4;
			Item.value = Item.sellPrice(gold: 2, silver: 40);
			Item.defense = 8;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.36f;
			player.GetDamage(DamageClass.Generic) -= 0.27f;
			player.GetAttackSpeed(HealerDamage.Instance) += 0.09f;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 4;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetModPlayer<CalamityPlayer>().statigelSet = true;
			player.GetJumpState<StatigelJump>().Enable();
			Player.jumpHeight += 5;
			player.jumpSpeedBoost += 0.6f;
			player.GetAttackSpeed(ThoriumDamageBase<HealerTool>.Instance) += 0.09f;
			player.GetDamage(HealerDamage.Instance) += 0.14f;
			player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.StatigelFoxMask.SetBonus") + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.PreHardmode.StatigelArmor.CommonSetBonus");
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("PurifiedGel").Type, 5).AddIngredient(calamity.Find<ModItem>("BlightedGel").Type, 5).AddTile(calamity.Find<ModTile>("StaticRefiner").Type).Register();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("StatigelArmor").Type && legs.type == calamity.Find<ModItem>("StatigelGreaves").Type : false;
	}
}