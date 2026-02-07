using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;
using CalamityEntropy;
using CalamityEntropy.Content.Items.Armor.VoidFaquir;

namespace CalamityBardHealer.Items
{
	[ExtendsFromMod("CalamityEntropy")]
	[JITWhenModsEnabled("CalamityEntropy")]
	[AutoloadEquip(EquipType.Head)]
	public class VoidFaquirBiretta : ModItem
	{
		private float HPS = 0f;
		private int soulEssence = 0;
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityEntropy");
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 36, 0, 0);
			Item.defense = 44;
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) Item.rare = entropy.Find<ModRarity>("VoidPurple").Type;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.65f;
			player.GetDamage(DamageClass.Generic) -= 0.35f;
			player.GetAttackSpeed(HealerDamage.Instance) += 0.15f;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 8;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetArmorPenetration(DamageClass.Generic) += 20f;
			player.Entropy().VFSet = true;
			if(player.Entropy().VoidCharge >= 1f) return;
			float hps = player.GetModPlayer<ThoriumPlayer>().healsPerSecond;
			if(hps != HPS) {
				if(hps > HPS) player.Entropy().VoidCharge += System.Math.Abs(hps - HPS) * 0.001f;
				HPS = hps;
			}
			if(player.GetModPlayer<ThoriumPlayer>().soulEssence != soulEssence) {
				if(player.GetModPlayer<ThoriumPlayer>().soulEssence == 0 && soulEssence > 0) player.Entropy().VoidCharge += soulEssence * 0.01f;
				soulEssence = player.GetModPlayer<ThoriumPlayer>().soulEssence;
			}
			if(player.Entropy().VoidCharge > 1f) player.Entropy().VoidCharge = 1f;
		}
		public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips) {
			foreach(TooltipLine m in tooltips) if(m.Mod == "Terraria" && m.Name.StartsWith("Tooltip") && m.Text.Equals("<armorBonus>")) {
				m.Text = Main.LocalPlayer.Entropy().VFSet ? Terraria.Localization.Language.GetTextValue("Mods.CalamityEntropy.vfb") + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.VoidFaquirBiretta.SetBonus") : "";
				break;
			}
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) CreateRecipe().AddIngredient(entropy.Find<ModItem>("VoidBar").Type, 14).AddIngredient(calamity.Find<ModItem>("RuinousSoul").Type, 6).AddIngredient(calamity.Find<ModItem>("TwistingNether").Type, 8).AddTile(412).Register();
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlines = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => head.type == Type && body.ModItem is VoidFaquirBodyArmor && legs.ModItem is VoidFaquirCuises;
	}
}