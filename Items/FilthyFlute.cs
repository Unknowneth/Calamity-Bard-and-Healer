using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Empowerments;
using ThoriumMod.Items;
using ThoriumMod.Items.BardItems;

namespace CalamityBardHealer.Items
{
	public class FilthyFlute : BigInstrumentItemBase
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.Wind;
		public override void SafeSetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<ResourceMaximum>(1, 0);
			this.Empowerments.AddInfo<ResourceRegen>(1, 0);
			ItemID.Sets.ItemsThatAllowRepeatedRightClick[base.Item.type] = true;
		}
		public override void SafeSetBardDefaults() {
			base.Item.width = 62;
			base.Item.height = 18;
			base.Item.useStyle = 5;
			if(!ModLoader.HasMod("Look")) base.Item.holdStyle = 3;
			base.Item.useTime = 18;
			base.Item.useAnimation = 18;
			base.Item.damage = 26;
			base.Item.crit = 6;
			base.Item.autoReuse = true;
			base.Item.knockBack = 3f;
			base.Item.value = Item.sellPrice(silver: 80);
			base.Item.rare = ItemRarityID.Orange;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 4f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.FilthyFlute>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/FilthyFlute") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
		}
		public override void SafeBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				position += Vector2.Normalize(velocity) * 58f;
				int index = -1;
				float maxDistSQ = 1000000f;
				foreach(NPC npc in Main.ActiveNPCs) if(npc.CanBeChasedBy(null, false)) {
					float distSQ = player.DistanceSQ(npc.Center);
					if(distSQ < maxDistSQ && Collision.CanHit(player.Center, 1, 1, npc.Center, 1, 1)) {
						maxDistSQ = distSQ;
						index = npc.whoAmI;
					}
				}
				if(index > -1) velocity = Vector2.Normalize(Main.npc[index].Center - position) * velocity.Length();
				int p = Projectile.NewProjectile(source, position, velocity * (1f + (float)success * 0.05f), type, damage, knockback, player.whoAmI, (float)level * 2.5f);
				NetMessage.SendData(27, -1, -1, null, p);
				type = ModContent.ProjectileType<Projectiles.FilthyCreeper>();
				if(success > 4 && player.ownedProjectileCounts[type] < 1) {
					p = Projectile.NewProjectile(source, position, Main.rand.NextVector2CircularEdge(base.Item.shootSpeed, base.Item.shootSpeed) * 3f, type, damage, knockback, player.whoAmI, velocity.Length() * (1f + (float)success * 0.05f), (float)level * 2.5f);
					NetMessage.SendData(27, -1, -1, null, p);
				}
			}
		}
		public override void UseItemFrame(Player player) => this.HoldItemFrame(player);
		public override void HoldItemFrame(Player player) => player.itemLocation += (new Vector2(ModLoader.HasMod("Look") ? -1 : -3) * player.Directions).RotatedBy(player.itemRotation);
		public override bool AltFunctionUse(Player player) => true;
	}
}