using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class TarragonParagonCrown : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 24;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("Turquoise").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 24);
			Item.defense = 26;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.4f;
			player.GetDamage(DamageClass.Generic) -= 0.3f;
			player.GetCritChance(HealerDamage.Instance) += 5;
			player.statLifeMax2 += 20;
			player.lifeRegen += 5;
			player.endurance += 0.05f;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 8;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("UelibloomBar").Type, 7).AddIngredient(calamity.Find<ModItem>("DivineGeode").Type, 6).AddTile(412).Register();
		}
		public override void UpdateArmorSet(Player player) {
			if(Main.myPlayer == player.whoAmI) if(++player.GetModPlayer<ThorlamityPlayer>().tarragonHeartbeat > 240) {
				player.GetModPlayer<ThorlamityPlayer>().tarragonHeartbeat = 0;
				int z = Projectile.NewProjectile(player.GetSource_Misc("Tarragon Paragon Crown"), player.MountedCenter, player.velocity, ModContent.ProjectileType<Projectiles.TarragonHeartbeat>(), 0, 0f, player.whoAmI, 7f);
				NetMessage.SendData(27, -1, -1, null, z);
			}
			player.GetDamage(HealerDamage.Instance) += 0.3f;
			player.GetModPlayer<CalamityPlayer>().tarraSet = true;
			player.setBonus = Terraria.Localization.Language.GetText("Mods.CalamityBardHealer.Items.TarragonParagonCrown.SetBonus").Format(ModLoader.GetMod("ThoriumMod").Call("GetHealerHealBonus", player)).ToString() + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.PostMoonLord.TarragonBreastplate.CommonSetBonus");
		}
		public override void ArmorSetShadows(Player player) {
			player.armorEffectDrawShadowSubtle = true;
			player.armorEffectDrawOutlines = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("TarragonBreastplate").Type && legs.type == calamity.Find<ModItem>("TarragonLeggings").Type : true;
	}
}