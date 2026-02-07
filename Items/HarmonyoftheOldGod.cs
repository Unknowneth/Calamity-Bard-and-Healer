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
	public class HarmonyoftheOldGod : BardItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("CalamityHunt");
		internal bool shootClient = false;
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<Damage>(4, 0);
			this.Empowerments.AddInfo<Defense>(4, 0);
			this.Empowerments.AddInfo<InvincibilityFrames>(4, 0);
			this.Empowerments.AddInfo<LifeRegeneration>(4, 0);
			this.Empowerments.AddInfo<MovementSpeed>(4, 0);
			this.Empowerments.AddInfo<FlightTime>(4, 0);
			this.Empowerments.AddInfo<JumpHeight>(4, 0);
			this.Empowerments.AddInfo<AquaticAbility>(4, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 52;
			base.Item.height = 106;
			base.Item.useStyle = 5;
			base.Item.holdStyle = 6;
			base.Item.useTime = 5;
			base.Item.useAnimation = 30;
			base.Item.damage = 1234;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.crit = 36;
			base.Item.autoReuse = true;
			base.Item.knockBack = 10f;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else base.Item.rare = 11;
			base.Item.noMelee = true;
			base.Item.noUseGraphic = true;
			base.Item.shootSpeed = 1f;
			if(ModLoader.HasMod("CalamityHunt")) base.Item.shoot = ModContent.ProjectileType<Projectiles.HarmonyoftheOldGod>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/Violince") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.InspirationCost = 1;
		}
		public override void BardHoldItem(Player player) {
			if(Main.myPlayer != player.whoAmI || player.ownedProjectileCounts[Item.shoot] > 0) return;
			int z = Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, Item.shoot, player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
			NetMessage.SendData(27, -1, -1, null, z);
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			shootClient = true;
			return false;
		}
	}
}