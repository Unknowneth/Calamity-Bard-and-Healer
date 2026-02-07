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
	public class Violince : BigInstrumentItemBase
	{
		public override BardInstrumentType InstrumentType => BardInstrumentType.String;
		public override void SafeSetStaticDefaults() {
			base.Item.ResearchUnlockCount = 1;
			this.Empowerments.AddInfo<Defense>(1, 0);
			this.Empowerments.AddInfo<InvincibilityFrames>(1, 0);
		}
		public override void SafeSetBardDefaults() {
			base.Item.width = 38;
			base.Item.height = 50;
			base.Item.useStyle = 14;
			base.Item.holdStyle = 6;
			base.Item.useTime = 20;
			base.Item.useAnimation = 20;
			base.Item.damage = 19;
			base.Item.crit = 6;
			base.Item.autoReuse = true;
			base.Item.knockBack = 1f;
			base.Item.value = Item.sellPrice(silver: 80);
			base.Item.rare = ItemRarityID.Orange;
			base.Item.noMelee = true;
			base.Item.shootSpeed = 4f;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.Violince>();
			base.Item.DamageType = BardDamage.Instance;
			base.Item.UseSound = new SoundStyle("CalamityBardHealer/Sounds/Violince") { SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest, MaxInstances = 5 };
		}
		public override void SafeBardShoot(int success, int level, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				int index = -1;
				float maxDistSQ = 1000000f;
				foreach(NPC npc in Main.ActiveNPCs) if(npc.CanBeChasedBy(null, false)) {
					float distSQ = player.DistanceSQ(npc.Center);
					if(distSQ < maxDistSQ && Collision.CanHit(player.Center, 1, 1, npc.Center, 1, 1)) {
						maxDistSQ = distSQ;
						index = npc.whoAmI;;
					}
				}
				if(index > -1) velocity = Vector2.Normalize(Main.npc[index].Center - position) * velocity.Length();
				int p = Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<Projectiles.ViolinceBow>(), 0, 0f, player.whoAmI, player.itemAnimationMax, player.itemAnimationMax);
				NetMessage.SendData(27, -1, -1, null, p);
				for(int i = 0; i < 3 + MathHelper.Min(4, success / 3); i++) {
					velocity += Main.rand.NextVector2CircularEdge(i, i) * velocity.Length() * 0.1f;
					p = Projectile.NewProjectile(source, position, velocity * (1f + (float)success * 0.05f), type, damage, knockback, player.whoAmI, (float)level * 2.5f);
					NetMessage.SendData(27, -1, -1, null, p);
				}
			}
		}
		public override void HoldItemFrame(Player player) => player.itemLocation += new Vector2(-8f, 10f) * player.Directions;
	}
}