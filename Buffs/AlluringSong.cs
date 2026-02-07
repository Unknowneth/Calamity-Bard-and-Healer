using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace CalamityBardHealer.Buffs
{
	public class AlluringSong : ModBuff
	{
		public override void SetStaticDefaults() => Main.buffNoSave[Type] = true;
		public override void Update(Player player, ref int buffIndex) {
			player.GetModPlayer<ThoriumPlayer>().inspirationRegenBonus += 0.5f;
			player.GetModPlayer<ThoriumPlayer>().bardRangeBoost += player.GetModPlayer<ThoriumPlayer>().bardRangeBoost / 2;
			player.GetModPlayer<ThoriumPlayer>().bardBuffDuration += 300;
			if(player.buffTime[buffIndex] == 0) player.AddBuff(ModContent.BuffType<Buffs.AlluringSongCD>(), 2700);
		}
	}
}