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
	public class InterstellarShredder : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(3, 3));
			this.Empowerments.AddInfo<Defense>(2, 0);
			this.Empowerments.AddInfo<DamageReduction>(2, 0);
			this.Empowerments.AddInfo<LifeRegeneration>(2, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 78;
			base.Item.height = 78;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
			base.Item.useTime = 12;
			base.Item.useAnimation = 12;
			base.Item.damage = 78;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 0.5f;
			base.Item.value = Item.sellPrice(gold: 9, silver: 60);
			base.Item.rare = 7;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 10f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.InterstellarShredder>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/InterstellarShredder") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.Item.scale = 0.8f;
			base.InspirationCost = 1;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			int j = Main.rand.Next(3);
			if(Main.myPlayer == player.whoAmI) for(int i = -1; i <= 1; i++) {
				int p = Projectile.NewProjectile(source, position, velocity.RotatedBy(i * MathHelper.PiOver4 / 9f), i != 0 ? ModContent.ProjectileType<Projectiles.InterstellarShredder2>() : type, damage, knockback, player.whoAmI);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-12f, 12f) * player.Directions;
	}
}