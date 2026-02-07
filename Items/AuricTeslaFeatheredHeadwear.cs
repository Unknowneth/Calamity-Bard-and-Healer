using Terraria;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod;
using CalamityMod.CalPlayer;
using CalamityMod.CalPlayer.Dashes;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class AuricTeslaFeatheredHeadwear : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
			Terraria.ID.ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 34;
			Item.height = 28;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else Item.rare = 10;
			Item.value = Item.sellPrice(gold: 30);
			Item.defense = 37;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(BardDamage.Instance) += 0.25f;
			player.GetCritChance(BardDamage.Instance) += 20;
			player.GetAttackSpeed(BardDamage.Instance) += 0.12f;
			player.GetModPlayer<ThoriumPlayer>().bardHomingSpeedBonus += 0.2f;
			player.GetModPlayer<ThoriumPlayer>().bardHomingRangeBonus += 0.2f;
			player.GetModPlayer<ThoriumPlayer>().bardBounceBonus += 3;
		}
		public override void UpdateArmorSet(Player player) {
			player.crimsonRegen = true;
			player.GetModPlayer<CalamityPlayer>().bloodflareSet = true;
			player.GetModPlayer<CalamityPlayer>().tarraSet = true;
			player.GetModPlayer<CalamityPlayer>().godSlayer = true;
			player.GetModPlayer<CalamityPlayer>().auricSet = true;
			if(Main.myPlayer == player.whoAmI && !player.HasBuff(ModContent.BuffType<Buffs.AlluringSong>()) && !player.HasBuff(ModContent.BuffType<Buffs.AlluringSongCD>()) && ThoriumHotkeySystem.ArmorKey.JustPressed) player.AddBuff(ModContent.BuffType<Buffs.AlluringSong>(), 300);
			if(player.GetModPlayer<CalamityPlayer>().godSlayerDashHotKeyPressed || (player.dashDelay != 0 && player.GetModPlayer<CalamityPlayer>().LastUsedDashID == GodslayerArmorDash.ID)) {
				player.GetModPlayer<CalamityPlayer>().DeferredDashID = GodslayerArmorDash.ID;
				player.dash = 0;
			}
			if(Main.myPlayer == player.whoAmI) if(++player.GetModPlayer<ThorlamityPlayer>().tarragonBeat > 60) {
				player.GetModPlayer<ThorlamityPlayer>().tarragonBeat = 0;
				int z = Projectile.NewProjectile(player.GetSource_Misc("Tarragon Chapeau"), player.MountedCenter + Main.rand.NextVector2Circular(320f, 320f), player.velocity, ModContent.ProjectileType<Projectiles.TarragonQuarterRest>(), player.statDefense * (1f + player.endurance) * 3, 10f, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, z);
				z = Projectile.NewProjectile(player.GetSource_Misc("Tarragon Chapeau"), Main.projectile[z].Center + Main.rand.NextVector2Circular(160f, 160f), player.velocity, ModContent.ProjectileType<Projectiles.TarragonQuarterRest>(), player.statDefense * (1f + player.endurance) * 3, 10f, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, z);
			}
			player.GetModPlayer<ThoriumPlayer>().bardBuffDuration += 300;
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.2f;
			player.GetModPlayer<ThoriumPlayer>().bardResourceDropBoost += 0.45f;
			player.thorns += 3f;
			player.ignoreWater = true;
			player.AddBuff(ModContent.BuffType<Buffs.CalamityBell>(), 2);
			player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.AuricTeslaFeatheredHeadwear.SetBonus");
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.TarragonChapeau>()).AddIngredient(ModContent.ItemType<Items.BloodflareSirenSkull>()).AddIngredient(ModContent.ItemType<Items.GodSlayerDeathsingerCowl>()).AddIngredient(calamity.Find<ModItem>("AuricBar").Type, 12).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlines = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("AuricTeslaBodyArmor").Type && legs.type == calamity.Find<ModItem>("AuricTeslaCuisses").Type : false;
	}
}