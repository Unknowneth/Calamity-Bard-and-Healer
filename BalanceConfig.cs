using System.ComponentModel;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace CalamityBardHealer
{
	[Label("Balance Settings")]
	public class BalanceConfig : ModConfig
	{
		public override ConfigScope Mode => ConfigScope.ServerSide;

		[ReloadRequired]
		[DefaultValue(true)]
		public bool expensiveDraedonsForge;

		[DefaultValue(true)]
		public bool rogueThrowerMerge;

		[Range(0f, 10f)]
		[DefaultValue(1f)]
		public float bard;

		[Range(0f, 10f)]
		[DefaultValue(1f)]
		public float radiant;

		[Range(0, 10)]
		[DefaultValue(1)]
		[Slider]
		public int soulEssence;

		[Range(0f, 10f)]
		[DefaultValue(1f)]
		public float healing;
	}
}