using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Utilities;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class Vivapollum : BardItem
	{
		private int bubbleTorrent = -1;
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityEntropy");
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<Damage>(4, 0);
			this.Empowerments.AddInfo<FlatDamage>(4, 0);
			this.Empowerments.AddInfo<CriticalStrikeChance>(4, 0);
			this.Empowerments.AddInfo<Defense>(4, 0);
			this.Empowerments.AddInfo<AquaticAbility>(4, 0);
			this.Empowerments.AddInfo<FlightTime>(4, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 78;
			base.Item.height = 42;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 5;
			base.Item.useAnimation = 25;
			base.Item.reuseDelay = 30;
			base.Item.damage = 3355;
			base.Item.crit = 16;
			base.Item.value = 12000;
			if(ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) base.Item.rare = entropy.Find<ModRarity>("AbyssalBlue").Type;
			else base.Item.rare = 11;
			base.Item.knockBack = 6.5f;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 8f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.Vivapollum>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/DoomsdayCatharsis") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.InspirationCost = 1;
		}
		public override void UpdateInventory(Player player) {
			if(bubbleTorrent > 0) {
				if(Main.myPlayer == player.whoAmI) NetMessage.SendData(27, -1, -1, null, Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center + new Vector2(Main.rand.Next(-1000, 1000), Main.rand.Next(560, 640)), -Vector2.UnitY, Item.shoot, Item.damage, base.Item.knockBack, player.whoAmI));
				bubbleTorrent--;
			}
			if(bubbleTorrent == 0 && player.GetThoriumPlayer().bardResource >= player.GetThoriumPlayer().bardResourceMax2) bubbleTorrent = -1;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("CalamityEntropy", out Mod entropy)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.OldDukesWisdom>()).AddIngredient(entropy.Find<ModItem>("WyrmTooth").Type, 10).AddIngredient(calamity.Find<ModItem>("ShadowspecBar").Type, 5).AddTile(entropy.Find<ModTile>("AbyssalAltarTile").Type).Register();
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 muzzleOffset = Vector2.Normalize(velocity);
			muzzleOffset = muzzleOffset * 48f + muzzleOffset.RotatedBy(MathHelper.PiOver2 * (float)player.direction) * 12f;
			if(Collision.CanHitLine(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
			if(player.GetThoriumPlayer().bardResource < GetInspirationCost(player) && bubbleTorrent == -1) bubbleTorrent = base.Item.reuseDelay;
			if(Main.myPlayer == player.whoAmI) for(int i = 0; i < 2; i++) NetMessage.SendData(27, -1, -1, null, Projectile.NewProjectile(source, position, velocity.RotatedByRandom((double)MathHelper.ToRadians(12f)) * (1f - Main.rand.NextFloat(0.3f)), type, damage, knockback, player.whoAmI));
			return false;
		}
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation -= (new Vector2(ModLoader.HasMod("Look") ? 4 : 6, ModLoader.HasMod("Look") ? -6 : -8) * player.Directions).RotatedBy(player.itemRotation) - (new Vector2(0f, 2f) * player.Directions).RotatedBy(player.fullRotation);
	}
}