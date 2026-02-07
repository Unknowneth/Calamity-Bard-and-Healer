using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class BloodflareRitualistMask : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 28;
			Item.height = 24;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("PureGreen").Type;
			else Item.rare = 11;
			Item.value = Item.sellPrice(gold: 26);
			Item.defense = 37;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.4f;
			player.GetDamage(DamageClass.Generic) -= 0.25f;
			player.GetCritChance(HealerDamage.Instance) += 15;
			player.statLifeMax2 += 80;
			player.lifeRegen += 10;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 12;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(calamity.Find<ModItem>("BloodstoneCore").Type, 11).AddIngredient(calamity.Find<ModItem>("RuinousSoul").Type, 2).AddTile(412).Register();
		}
		public override void UpdateArmorSet(Player player) {
			ModKeybind armorKey = ThoriumHotkeySystem.ArmorKey;
			if(Main.myPlayer == player.whoAmI && armorKey.JustPressed && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.BloodflareRitual>()] == 0) {
				int z = Projectile.NewProjectile(player.GetSource_Misc("Bloodflare Ritualist Mask"), player.MountedCenter, player.velocity, ModContent.ProjectileType<Projectiles.BloodflareRitual>(), 0, 0f, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, z);
				PlayerDeathReason damageSource = PlayerDeathReason.ByOther(13);
				if(Main.rand.NextBool(2)) damageSource = PlayerDeathReason.ByOther(player.Male ? 14 : 15);
				player.Hurt(new Player.HurtInfo() {Damage = player.statLifeMax2 / 2, DamageSource = damageSource, HitDirection = player.direction, Dodgeable = false});
				if(player.statLife <= 0) player.KillMe(damageSource, 1.0, 0);
				player.lifeRegenCount = 0;
				player.lifeRegenTime = 0;
				NetMessage.SendPlayerHurt(player.whoAmI, new Player.HurtInfo() {Damage = player.statLifeMax2 / 2, DamageSource = damageSource, HitDirection = player.direction, Dodgeable = false});
			}
			player.GetDamage(HealerDamage.Instance) += 0.27f;
			player.GetAttackSpeed(HealerDamage.Instance) += 0.11f;
			player.crimsonRegen = true;
			player.GetModPlayer<CalamityPlayer>().bloodflareSet = true;
			string assignedKeys = "";
			for(int i = 0; i < armorKey.GetAssignedKeys().Count; i++) if(i < armorKey.GetAssignedKeys().Count - 1) assignedKeys += armorKey.GetAssignedKeys()[i] + ", ";
			else assignedKeys += armorKey.GetAssignedKeys()[i];
			player.setBonus = Terraria.Localization.Language.GetText("Mods.CalamityBardHealer.Items.BloodflareRitualistMask.SetBonus").Format(new object[]{assignedKeys, player.GetModPlayer<ThoriumPlayer>().healBonus}).ToString() + "\n" + Terraria.Localization.Language.GetTextValue("Mods.CalamityMod.Items.Armor.PostMoonLord.BloodflareBodyArmor.CommonSetBonus");
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawShadowSubtle = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("BloodflareBodyArmor").Type && legs.type == calamity.Find<ModItem>("BloodflareCuisses").Type : true;
	}
}