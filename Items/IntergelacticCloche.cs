using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using ThoriumMod;
using CatalystMod;

namespace CalamityBardHealer.Items
{
	[ExtendsFromMod("CatalystMod")]
	[JITWhenModsEnabled("CatalystMod")]
	[AutoloadEquip(EquipType.Head)]
	public class IntergelacticCloche : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 28;
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) Item.rare = catalyst.Find<ModRarity>("SuperbossRarity").Type;
			else Item.rare = 8;
			Item.value = Item.sellPrice(gold: 30);
			Item.defense = 17;
			Item.maxStack = 1;
			Item.DamageType = BardDamage.Instance;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.18f;
			player.GetAttackSpeed(BardDamage.Instance) += 0.18f;
			player.GetCritChance(BardDamage.Instance) += 18;
			player.GetModPlayer<ThoriumPlayer>().bardHomingSpeedBonus += 0.12f;
			player.GetModPlayer<ThoriumPlayer>().bardHomingRangeBonus += 0.12f;
			player.GetModPlayer<ThoriumPlayer>().bardBounceBonus += 3;
		}
		public override void UpdateArmorSet(Player player) {
			try {
				using(System.Collections.Generic.List<string>.Enumerator enumerator = CatalystPlayer.AsteroidVisToggleKey.GetAssignedKeys(0).GetEnumerator()) {
					if(enumerator.MoveNext()) {
						string key = enumerator.Current;
						player.setBonus = Language.GetTextValue("Mods.CatalystMod.ArmorSetBonus.IntergelacticAll", Language.GetTextValue("Mods.CatalystMod.Common.BoundKey", key));
					}
					else player.setBonus = Language.GetTextValue("Mods.CatalystMod.ArmorSetBonus.IntergelacticAll", Language.GetTextValue("Mods.CatalystMod.Common.UnboundKey"));
				}
			}
			catch { }
			player.setBonus += "\n" + Language.GetTextValue("Mods.CalamityBardHealer.Items.IntergelacticCloche.SetBonus");
			CatalystMod.Items.Armor.Intergelactic.IntergelacticHeadMelee.SetBonus(player, Item);
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.17f;
			player.GetModPlayer<ThoriumPlayer>().bardResourceMax2 += 7;
			player.GetModPlayer<ThoriumPlayer>().bardBuffDuration += 120;
			player.GetModPlayer<ThorlamityPlayer>().intergelacticBard = true;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.StatigelEarrings>(), 1).AddIngredient(catalyst.Find<ModItem>("MetanovaBar").Type, 6).AddTile(412).Register();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CatalystMod", out Mod catalyst) ? head.type == Type && body.type == catalyst.Find<ModItem>("IntergelacticBreastplate").Type && legs.type == catalyst.Find<ModItem>("IntergelacticGreaves").Type : true;
	}
}