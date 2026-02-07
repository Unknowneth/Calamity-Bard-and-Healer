using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using ThoriumMod;
using ThoriumMod.Items;
using ThoriumMod.Items.HealerItems;
using System.Collections.Generic;
using System.Linq;

namespace CalamityBardHealer.Items
{
	public class PledgeofSelflessness : ThoriumItem
	{
		public override void SetDefaults() {
			this.healType = HealType.Ally;
			this.healAmount = 40;
			if(ModContent.GetInstance<BalanceConfig>().healing != 1) this.healAmount = (int)MathHelper.Max(ModContent.GetInstance<BalanceConfig>().healing * (float)this.healAmount, 1);
			this.healDisplay = true;
			this.isHealer = true;
			this.radiantLifeCost = 5;
			base.Item.DamageType = ThoriumDamageBase<HealerTool>.Instance;
			base.Item.mana = 15;
			base.Item.width = 36;
			base.Item.height = 21;
			base.Item.useTime = 9;
			base.Item.useAnimation = 9;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 1f;
			base.Item.channel = true;
			base.Item.value = Item.sellPrice(gold: 30);
			if(ModLoader.TryGetMod("CalamityMod", out Mod calamity)) base.Item.rare = calamity.Find<ModRarity>("CalamityRed").Type;
			else base.Item.rare = 10;
			base.Item.UseSound = SoundID.Item20;
			base.Item.autoReuse = true;
			base.Item.shoot = ModContent.ProjectileType<Projectiles.PledgeofSelflessness>();
			base.Item.shootSpeed = 6f;
		}
		public override void HoldItem(Player player) {
			if(player.lifeRegen > 0) player.lifeRegen = 0;
			player.statDefense *= 0;
			player.endurance *= 0f;
		}
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) {
			if(Main.myPlayer == player.whoAmI) {
				List<int> closest = new List<int>();
				Dictionary<int, float> players = new Dictionary<int, float>();
				for(int i = 0; i < Main.maxPlayers; i++) if(Main.player[i].active && !Main.player[i].dead && Main.player[i].statLife < Main.player[i].statLifeMax2 && Main.player[i].team == player.team && i != player.whoAmI) players.TryAdd(i, Main.player[i].Distance(Main.MouseWorld));
				for(int i = 0; i < 3; i++) if(players.Count > 0 && players is not null) {
					int who = (int)players.MinBy(kvp => kvp.Value).Key;
					closest.Add(who);
					int a = Projectile.NewProjectile(source, position, velocity, type, 0, 0f, player.whoAmI, who + 1);
					NetMessage.SendData(27, -1, -1, null, a);
					players.Remove(who);
				}
				else if(closest.Count > 0) {
					int a = Projectile.NewProjectile(source, position, velocity, type, 0, 0f, player.whoAmI, Main.rand.Next(closest.ToArray()) + 1);
					NetMessage.SendData(27, -1, -1, null, a);
					players.Clear();
				}
				closest.Clear();
			}
			return false;
		}
		public override Color? GetAlpha(Color lightColor) => Color.White;
	}
}