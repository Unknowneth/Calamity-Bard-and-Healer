using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using CalamityMod.CalPlayer;

namespace CalamityBardHealer.Items
{
	[AutoloadEquip(EquipType.Head)]
	public class AuricTeslaValkyrieVisage : ModItem
	{
		public override void SetStaticDefaults() {
			Item.ResearchUnlockCount = 1;
			Terraria.ID.ArmorIDs.Head.Sets.PreventBeardDraw[Item.headSlot] = true;
		}
		public override void SetDefaults() {
			Item.width = 30;
			Item.height = 24;
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else Item.rare = 10;
			Item.value = Item.sellPrice(gold: 30);
			Item.defense = 45;
			Item.maxStack = 1;
		}
		public override void UpdateEquip(Player player) {
			player.GetDamage(HealerDamage.Instance) += 0.5f;
			player.GetDamage(DamageClass.Generic) -= 0.35f;
			player.GetCritChance(HealerDamage.Instance) += 10;
			player.manaCost -= 0.45f;
			player.lifeRegen += 15;
			player.manaRegenBonus += 6;
			player.manaRegenDelayBonus += 4f;
			player.statManaMax2 += 100;
			player.statLifeMax2 += 200;
			player.GetModPlayer<ThoriumPlayer>().healBonus += 18;
		}
		public override void UpdateArmorSet(Player player) {
			player.crimsonRegen = true;
			player.GetModPlayer<CalamityPlayer>().bloodflareSet = true;
			player.GetModPlayer<CalamityPlayer>().tarraSet = true;
			player.GetModPlayer<CalamityPlayer>().silvaSet = true;
			player.GetModPlayer<CalamityPlayer>().auricSet = true;
			player.GetModPlayer<ThorlamityPlayer>().silvaHealer = true;
			if(Main.myPlayer == player.whoAmI) {
				if(++player.GetModPlayer<ThorlamityPlayer>().tarragonHeartbeat > 120) {
					player.GetModPlayer<ThorlamityPlayer>().tarragonHeartbeat = 0;
					int z = Projectile.NewProjectile(player.GetSource_Misc("Tarragon Paragon Crown"), player.MountedCenter, player.velocity, ModContent.ProjectileType<Projectiles.TarragonHeartbeat>(), 0, 0f, player.whoAmI, 7f);
					NetMessage.SendData(27, -1, -1, null, z);
				}
				if(ThoriumHotkeySystem.ArmorKey.JustPressed && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.BloodflareRitual>()] == 0) {
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
			}
			player.GetDamage(HealerDamage.Instance) += 0.4f;
			player.GetCritChance(HealerDamage.Instance) += 20;
			player.GetAttackSpeed(HealerDamage.Instance) += 0.16f;
			player.GetAttackSpeed(ThoriumDamageBase<HealerTool>.Instance) += 0.16f;
			player.thorns += 3f;
			player.ignoreWater = true;
			player.setBonus = Terraria.Localization.Language.GetTextValue("Mods.CalamityBardHealer.Items.AuricTeslaValkyrieVisage.SetBonus");
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.TarragonParagonCrown>()).AddIngredient(ModContent.ItemType<Items.BloodflareRitualistMask>()).AddIngredient(ModContent.ItemType<Items.SilvaGuardianHelmet>()).AddIngredient(calamity.Find<ModItem>("AuricBar").Type, 12).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
		public override void ArmorSetShadows(Player player) => player.armorEffectDrawOutlines = true;
		public override bool IsArmorSet(Item head, Item body, Item legs) => ModLoader.TryGetMod("CalamityMod", out Mod calamity) ? head.type == Type && body.type == calamity.Find<ModItem>("AuricTeslaBodyArmor").Type && legs.type == calamity.Find<ModItem>("AuricTeslaCuisses").Type : false;
	}
}