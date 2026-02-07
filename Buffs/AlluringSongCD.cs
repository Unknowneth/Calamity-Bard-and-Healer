using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod;

namespace CalamityBardHealer.Buffs
{
	public class AlluringSongCD : ModBuff
	{
		public override void SetStaticDefaults() {
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Terraria.ID.BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
		}
	}
}