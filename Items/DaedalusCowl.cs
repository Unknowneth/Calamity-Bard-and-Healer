using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class DaedalusCowl : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 22;
			Item.rare = 5;
			Item.value = Item.sellPrice(gold: 4, silver: 80);
			Item.defense = 17;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.35f;
			player.GetDamage(DamageClass.Generic) -= 0.25f;
			player.GetAttackSpeed(ThoriumDamageBase<HealerTool>.Instance) += 0.05f;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 3;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.25f;
			player.GetModPlayer<ThoriumPlayer>().healBonus++;
			ModKeybind armorKey = ThoriumHotkeySystem.ArmorKey;
			if(armorKey.JustPressed) {
				int z = Projectile.NewProjectile(player.GetSource_Misc("Daedulus Cowl"), player.Top, Vector2.Zero, ModContent.ProjectileType<Projectiles.IceCreamMachine>(), 0, 0f, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, z);
			}
			string assignedKeys = "";
			for(int i = 0; i < armorKey.GetAssignedKeys().Count; i++) if(i < armorKey.GetAssignedKeys().Count - 1) assignedKeys += armorKey.GetAssignedKeys()[i] + ", ";
			else assignedKeys += armorKey.GetAssignedKeys()[i];
			player.setBonus = Terraria.Localization.Language.GetText("Mods.CalamityBardHealer.Items.DaedalusCowl.SetBonus").Format(assignedKeys, player.GetModPlayer<ThoriumPlayer>().healBonus).ToString();
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