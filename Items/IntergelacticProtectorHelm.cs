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
	public class IntergelacticProtectorHelm : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CatalystMod");
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 24;
			Item.height = 26;
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) Item.rare = catalyst.Find<ModRarity>("SuperbossRarity").Type;
			else Item.rare = 8;
			Item.value = Item.sellPrice(gold: 30);
			Item.defense = 27;
			Item.maxStack = 1;
			Item.DamageType = HealerDamage.Instance;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.44f;
			player.GetDamage(DamageClass.Generic) -= 0.33f;
			player.GetAttackSpeed(HealerDamage.Instance) += 0.11f;
			player.GetCritChance(HealerDamage.Instance) += 11;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 4;
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
			player.setBonus += "\n" + Language.GetTextValue("Mods.CalamityBardHealer.Items.IntergelacticProtectorHelm.SetBonus", player.GetModPlayer<ThoriumPlayer>().healBonus);
			CatalystMod.Items.Armor.Intergelactic.IntergelacticHeadMelee.SetBonus(player, Item);
			player.GetDamage(HealerDamage.Instance) += 0.22f;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 6;
			player.GetModPlayer<ThorlamityPlayer>().intergelacticHealer = true;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CatalystMod", out Mod catalyst)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.StatigelFoxMask>(), 1).AddIngredient(catalyst.Find<ModItem>("MetanovaBar").Type, 6).AddTile(412).Register();
		}
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CatalystMod", out Mod catalyst) ? head.type == Type && body.type == catalyst.Find<ModItem>("IntergelacticBreastplate").Type && legs.type == catalyst.Find<ModItem>("IntergelacticGreaves").Type : true;
	}
}