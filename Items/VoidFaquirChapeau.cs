using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Empowerments;
using CalamityMod.CalPlayer;
using CalamityEntropy;
using CalamityEntropy.Content.Items.Armor.VoidFaquir;

namespace CalamityBardHealer.Items
{
	[ExtendsFromMod("CalamityEntropy")]
	[JITWhenModsEnabled("CalamityEntropy")]
	[AutoloadEquip(EquipType.Head)]
	public class VoidFaquirChapeau : ModItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityEntropy");
		public override void SetStaticDefaults() => Item.ResearchUnlockCount = 1;
		public override void SetDefaults() {
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.buyPrice(0, 36, 0, 0);
			Item.defense = 32;
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) Item.rare = entropy.Find<ModRarity>("VoidPurple").Type;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.25f;
			player.GetAttackSpeed(BardDamage.Instance) += 0.15f;
			player.GetThoriumPlayer().inspirationRegenBonus += 0.15f;
		}
		public override void UpdateArmorSet(Player player) {
			player.GetArmorPenetration(DamageClass.Generic) += 20f;
			player.Entropy().VFSet = true;
			if(player.Entropy().VoidCharge >= 1f) return;
			//I wish there is a way to get ActiveEmpowerments list without System.Reflection because they cause crashes when Thorium updates
			//had to write this cursed ass code just to get this to work because EmpowermentTimer can only be assigned once or else shit breaks
			//I could just number e differently per instance but this is more convenient to my eyes
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<AquaticAbility>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<AttackSpeed>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<CriticalStrikeChance>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<Damage>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
				{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<DamageReduction>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<Defense>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<FlatDamage>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<FlightTime>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<InvincibilityFrames>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<JumpHeight>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<LifeRegeneration>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<MovementSpeed>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<ResourceConsumptionChance>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<ResourceGrabRange>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<ResourceMaximum>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			{
				EmpowermentTimer e = player.GetThoriumPlayer().GetEmpTimer<ResourceRegen>();
				if(e.timer > 0) player.Entropy().VoidCharge += e.level * 0.0001f;
			}
			if(player.Entropy().VoidCharge > 1f) player.Entropy().VoidCharge = 1f;
		}
		public override void ModifyTooltips(System.Collections.Generic.List<TooltipLine> tooltips) {
			foreach(TooltipLine m in tooltips) if(m.Mod == "Terraria" && m.Name.StartsWith("Tooltip") && m.Text.Equals("<armorBonus>")) {
				m.Text = Main.LocalPlayer.Entropy().VFSet ? Terraria.Localization.Language.GetTextValue("Mods.CalamityEntropy.vfb") + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.VoidFaquirChapeau.SetBonus") : "";
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