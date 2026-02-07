using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class TarragonChapeau : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 24;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("Turquoise").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 24);
			Item.defense = 16;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.1f;
			player.GetCritChance(BardDamage.Instance) += 10;
			player.GetAttackSpeed(BardDamage.Instance) += 0.1f;
			player.endurance += 0.05f;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("UelibloomBar").Type, 7).AddIngredient(calamity.Find<ModItem>("DivineGeode").Type, 6).AddTile(412).Register();
		}
		public override void UpdateArmorSet(Player player) {
			if(Main.myPlayer == player.whoAmI) if(++player.GetModPlayer<ThorlamityPlayer>().tarragonBeat > 60) {
				player.GetModPlayer<ThorlamityPlayer>().tarragonBeat = 0;
				int z = Projectile.NewProjectile(player.GetSource_Misc("Tarragon Chapeau"), player.MountedCenter + Main.rand.NextVector2Circular(160f, 160f), player.velocity, ModContent.ProjectileType<Projectiles.TarragonQuarterRest>(), player.statDefense * (1f + player.endurance) * 2, 10f, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, z);
				z = Projectile.NewProjectile(player.GetSource_Misc("Tarragon Chapeau"), Main.projectile[z].Center + Main.rand.NextVector2Circular(160f, 160f), player.velocity, ModContent.ProjectileType<Projectiles.TarragonQuarterRest>(), player.statDefense * (1f + player.endurance) * 2, 10f, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, z);
			}
			player.GetModPlayer<CalamityPlayer>().tarraSet = true;
			player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.TarragonChapeau.SetBonus") + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.PostMoonLord.TarragonBreastplate.CommonSetBonus");
		}
		public override void ArmorSetShadows(Player player) {
			player.armorEffectDrawShadowSubtle = true;
			player.armorEffectDrawOutlines = true;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("TarragonBreastplate").Type && legs.type == calamity.Find<ModItem>("TarragonLeggings").Type : true;
	}
}