using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;
using System.Collections.Generic;
using System.IO;

namespace CalamityBardHealer.Items
{
	public class SymphonicFabrications : BardItem
	{
		private int comboCounter = -1;
		public override BardInstrumentType InstrumentType => BardInstrumentType.Electronic;
		public override void SetStaticDefaults() => base.Item.ResearchUnlockCount = 1;
		public override void SetBardDefaults() {
			base.Item.width = 108;
			base.Item.height = 66;
			base.Item.holdStyle = 5;
			base.Item.useStyle = 12;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.damage = 211;
			if(ModContent.GetInstance<BalanceConfig>().bard != 1f) base.Item.damage = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().bard * (float)base.Item.damage, 1);
			base.Item.autoReuse = true;
			base.Item.knockBack = 3.5f;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("BurnishedAuric").Type;
			else base.Item.rare = 11;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 6f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.SymphonicExoMissile>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/FlightoftheGoliath") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
			base.InspirationCost = 2;
		}
		public override void BardHoldItem(Player player) {
			int p = ModContent.ProjectileType<Projectiles.ExoSurroundSound>();
			int z = player.ownedProjectileCounts[p];
			if(z >= 2 || Main.myPlayer != player.whoAmI) return;
			z = Projectile.NewProjectile(player.GetSource_ItemUse(base.Item), player.MountedCenter, player.velocity, p, player.GetWeaponDamage(base.Item), base.Item.knockBack, player.whoAmI, z);
			NetMessage.SendData(27, -1, -1, null, z);
		}
		public override bool BardShoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(++comboCounter > 10) comboCounter = 0;
			if(Main.myPlayer == player.whoAmI) for(int i = -1; i <= 1; i++) {
				int c = comboCounter - i;
				if(c < 0) c += 10;
				if(c > 10) c -= 10;
				position += new Vector2(Main.rand.Next(8, 36) * player.direction, Main.rand.Next(-2, 13));
				int p = Projectile.NewProjectile(source, position, Vector2.UnitX.RotatedBy(player.fullRotation + MathHelper.ToRadians(i * 15f) - Main.rand.NextFloat(MathHelper.PiOver4) * 0.2f) * player.direction * velocity.Length(), type, damage, knockback, player.whoAmI, 0f, 0f, c);
				NetMessage.SendData(27, -1, -1, null, p);
			}
			for(int i = 0; i < Main.maxPlayers; i++) if(Main.player[i].active && Main.player[i].Distance(player.Center) < player.GetModPlayer<ThoriumPlayer>().bardRangeBoost + 500) base.NetApplyEmpowerments(Main.player[i], (byte)comboCounter);
			return false;
		}
		public override void ModifyEmpowermentPool(Player player, Player target, EmpowermentPool empPool) {
			byte[] empList = new byte[] {
				EmpowermentLoader.EmpowermentType<Damage>(),
				EmpowermentLoader.EmpowermentType<AttackSpeed>(),
				EmpowermentLoader.EmpowermentType<CriticalStrikeChance>(),
				EmpowermentLoader.EmpowermentType<JumpHeight>(),
				EmpowermentLoader.EmpowermentType<MovementSpeed>(),
				EmpowermentLoader.EmpowermentType<InvincibilityFrames>(),
				EmpowermentLoader.EmpowermentType<DamageReduction>(),
				EmpowermentLoader.EmpowermentType<Defense>(),
				EmpowermentLoader.EmpowermentType<ResourceRegen>(),
				EmpowermentLoader.EmpowermentType<ResourceConsumptionChance>(),
				EmpowermentLoader.EmpowermentType<ResourceMaximum>()
			};
			empPool.Add(empList[(int)this.NetInfo], 4);
		}
		public override void BardModifyTooltips(List<TooltipLine> list) {
			int index = -1;
			for(int i = 0; i < list.Count; i++) if(list[i].Name.Contains("Tooltip")) index = i;
			if(index == -1) return;
			list.Insert(index + 1, new TooltipLine(base.Mod, "transformationText", "Variety IV"));
			list.Insert(index + 1, new TooltipLine(base.Mod, "transformationText", "Playing empowers players with bonus:") {OverrideColor = new Color?(new Color(140, 250, 180))});
		}
		public override void AddRecipes() {
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity) && ModLoader.TryGetMod("ThoriumMod", out Mod thorium)) CreateRecipe().AddIngredient(ModContent.ItemType<Items.WulfrumMegaphone>()).AddIngredient(thorium.Find<ModItem>("TerrariumAutoharp").Type).AddIngredient(thorium.Find<ModItem>("BlackMIDI").Type).AddIngredient(ModContent.ItemType<Items.FeralKeytar>()).AddIngredient(ModContent.ItemType<Items.FlightoftheGoliath>()).AddIngredient(ModContent.ItemType<Items.NoisebringerGoliath>()).AddIngredient(calamity.Find<ModItem>("MiracleMatter").Type).AddTile(calamity.Find<ModTile>("DraedonsForge").Type).Register();
		}
		public override void UseItemFrame(Player player) {
			float attackTime = (float)player.itemAnimation / (float)player.itemAnimationMax;
			if(attackTime < 0.3f && attackTime > 0.1f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Quarter;
			else if(attackTime < 0.8f && attackTime > 0.5f) player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.Full;
			else player.compositeBackArm.stretch = Player.CompositeArmStretchAmount.ThreeQuarters;
			player.compositeBackArm.rotation -= Vector2.UnitY.RotatedBy(attackTime * MathHelper.Pi).X * MathHelper.PiOver4 / 3f * player.direction;
			this.HoldItemFrame(player);
		}
		public override void NetSend(BinaryWriter writer) => writer.Write(comboCounter);
		public override void NetReceive(BinaryReader reader) => comboCounter = reader.ReadInt32();
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-24f, 32f) * player.Directions;
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}