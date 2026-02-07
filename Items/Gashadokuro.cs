using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class Gashadokuro : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			Main.RegisterItemAnimation(base.Item.type, new DrawAnimationVertical(3, 3));
			this.Empowerments.AddInfo<AttackSpeed>(4, 0);
			this.Empowerments.AddInfo<Defense>(4, 0);
			this.Empowerments.AddInfo<LifeRegeneration>(4, 0);
			this.Empowerments.AddInfo<MovementSpeed>(4, 0);
			this.Empowerments.AddInfo<FlightTime>(4, 0);
			this.Empowerments.AddInfo<JumpHeight>(4, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 46;
			base.Item.height = 56;
			base.Item.useStyle = 14;
			base.Item.holdStyle = 6;
			base.Item.useTime = 60;
			base.Item.useAnimation = 60;
			base.Item.damage = 888;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.crit = 66;
			base.Item.autoReuse = true;
			base.Item.knockBack = 10f;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("CalamityRed").Type;
			else base.Item.rare = 10;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 1f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.Gashadokuro>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new Terraria.Audio.SoundStyle("CalamityBardHealer/Sounds/CalamityBell");
			base.InspirationCost = 2;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}