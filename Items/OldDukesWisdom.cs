using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using ThoriumMod.Sounds;
using ThoriumMod.Utilities;

namespace CalamityBardHealer.Items
{
	public class OldDukesWisdom : BardItem
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Brass;
		public override void SetStaticDefaults() {
			this.Empowerments.AddInfo<AttackSpeed>(3, 0);
			this.Empowerments.AddInfo<Damage>(3, 0);
			this.Empowerments.AddInfo<FlatDamage>(4, 0);
			this.Empowerments.AddInfo<AquaticAbility>(4, 0);
			this.Empowerments.AddInfo<FlightTime>(3, 0);
		}
		public override bool PlayOnUse => false;
		public override void SetBardDefaults() {
			base.Item.damage = 210;
			base.InspirationCost = 1;
			base.Item.width = 30;
			base.Item.height = 30;
			base.Item.useTime = 8;
			base.Item.useAnimation = 30;
			base.Item.reuseDelay = 30;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.autoReuse = true;
			base.Item.noMelee = true;
			base.Item.knockBack = 8f;
			base.Item.value = Item.sellPrice(0, 35, 0, 0);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("PureGreen").Type;
			else base.Item.rare = 11;
			base.Item.UseSound = ThoriumSounds.Trumpet_Sound;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.OldDukesWisdom>();
			base.Item.shootSpeed = 12f;
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			Vector2 muzzleOffset = Vector2.Normalize(velocity);
			muzzleOffset = muzzleOffset * 30f + muzzleOffset.RotatedBy(MathHelper.PiOver2 * (float)player.direction) * 28f;
			if(Collision.CanHitLine(position, 0, 0, position + muzzleOffset, 0, 0)) position += muzzleOffset;
			if(Main.myPlayer == player.whoAmI) for(int i = 0; i < MathHelper.Clamp((float)player.itemAnimationMax / (float)player.itemAnimation / 2f, 1, 3); i++) NetMessage.SendData(27, -1, -1, null, Projectile.NewProjectile(source, position, velocity.RotatedByRandom((double)MathHelper.ToRadians(12f)) * (1f - Main.rand.NextFloat(0.3f)), type, damage, knockback, player.whoAmI));
			return false;
		}
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation += (new Vector2(ModLoader.HasMod("Look") ? -6 : -8, ModLoader.HasMod("Look") ? 12 : 10) * player.Directions).RotatedBy(player.itemRotation);
	}
}
