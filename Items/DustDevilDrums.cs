using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Sounds;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class DustDevilDrums : BardItem
	{
		public override bool IsLoadingEnabled(Mod mod) => ModLoader.HasMod("InfernumMode");
		public override BardInstrumentType InstrumentType => BardInstrumentType.Percussion;
		public override void SetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<AquaticAbility>(3, 0);
			this.Empowerments.AddInfo<MovementSpeed>(2, 0);
			this.Empowerments.AddInfo<FlightTime>(3, 0);
			this.Empowerments.AddInfo<JumpHeight>(2, 0);
		}
		public override void SetBardDefaults() {
			base.Item.width = 56;
			base.Item.height = 26;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
			base.Item.useTime = 9;
			base.Item.useAnimation = 27;
			base.Item.damage = 92;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 2.75f;
			base.Item.value = Item.sellPrice(gold: 16);
			if(ModLoader.TryGetMod("InfernumMode", out Mod infernum)) {
				Item.rare = infernum.Find<ModRarity>("InfernumVassalRarity").Type;
				Item.shoot = ModContent.ProjectileType<Projectiles.DustDevil>();
			}
			else base.Item.rare = 9;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 16f;
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle?(ThoriumSounds.Bongo);
			base.InspirationCost = 1;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == Main.myPlayer) {
				int p = player.itemAnimation / player.itemTime - 1;
				velocity = velocity.RotatedBy(p * MathHelper.TwoPi / GetInspirationCost(player) * player.direction) * -0.5f;
				p = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, -1f, velocity.Length(), Main.rand.NextBool(4) ? 1f : 0f);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			return false;
		}
		public override void UseItemFrame(Player player) {
			float attackTime = (float)player.itemAnimation / (float)player.itemAnimationMax;
			if(attackTime < 0.3f && attackTime > 0.1f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Quarter;
			else if(attackTime < 0.8f && attackTime > 0.5f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Full;
			else player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
			player.compositeBackArm.rotation -= Vector2.UnitY.RotatedBy(attackTime * MathHelper.Pi).X * MathHelper.PiOver4 / 3f * player.direction;
			this.HoldItemFrame(player);
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-10f, 6f) * player.Directions;
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}