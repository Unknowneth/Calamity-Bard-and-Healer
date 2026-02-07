using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class SongoftheCosmos : BardItem
	{
		internal int charge;
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<ResourceMaximum>(4, 0);
			this.Empowerments.AddInfo<ResourceRegen>(4, 0);
			this.Empowerments.AddInfo<AttackSpeed>(4, 0);
			this.Empowerments.AddInfo<CriticalStrikeChance>(4, 0);
			this.Empowerments.AddInfo<Defense>(4, 0);
			this.Empowerments.AddInfo<LifeRegeneration>(4, 0);
			this.Empowerments.AddInfo<MovementSpeed>(4, 0);
			this.Empowerments.AddInfo<FlightTime>(4, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 74;
			base.Item.height = 30;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 36;
			base.Item.useAnimation = 36;
			base.Item.damage = 200;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.crit = 16;
			base.Item.autoReuse = true;
			base.Item.knockBack = 4f;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else base.Item.rare = 11;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 5f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.CosmicSong>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/FilthyFlute") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.InspirationCost = 2;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(player.altFunctionUse == 2 && charge >= 1000) {
				int p = Projectile.NewProjectile(source, Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<Projectiles.CosmicEncore>(), damage * 2, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
				charge = 0;
				return false;
			}
			int random = Main.rand.Next(91);
			if(Main.myPlayer == player.whoAmI) for(int i = 0; i < 5; i++) {
				int p = Projectile.NewProjectile(source, position + Vector2.Normalize(velocity) * 84f, velocity, type, damage, knockback, player.whoAmI, velocity.Length(), i * 18f + random, player.direction * player.gravDir);
				if(Main.projectile[p].ai[1] > 90f) Main.projectile[p].ai[1] -= 90f;
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.SongoftheElements>()).AddIngredient(thorium.Find<ModItem>("Holophonor").Type).AddIngredient(calamity.Find<ModItem>("AuricBar").Type, 5).AddTile(calamity.Find<ModTile>("CosmicAnvil").Type).Register();
		}
		public override void ModifyInspirationCost(Player player, ref int cost) {
			if(player.altFunctionUse == 2) cost = 0;
			else if(player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.CosmicEncore>()] > 0) cost /= 2;
		}
		public override bool AltFunctionUse(Player player) => charge >= 1000;
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation -= (new Vector2(ModLoader.HasMod("Look") ? 0 : 2, ModLoader.HasMod("Look") ? 4 : 6) * player.Directions).RotatedBy(player.itemRotation);
	}
}