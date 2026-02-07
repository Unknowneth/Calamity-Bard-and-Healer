using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using ThoriumMod.Utilities;
using ThoriumMod.UI.ResourceBars;

namespace CalamityBardHealer.UI
{
	internal class CosmicSong : InterfaceResource
	{
		public CosmicSong() : base("CalamityBardHealer: Song of the Cosmos", (InterfaceScaleType)1) {
		}

		public Asset<Texture2D> SheetAsset {
			get {
				Asset<Texture2D> result;
				if((result = this.sheetAsset) == null) result = (this.sheetAsset = ModContent.Request<Texture2D>("CalamityBardHealer/UI/CosmicSong"));
				return result;
			}
		}

		public static int FadeTime { get; internal set; }

		public override void Update(GameTime gameTime) {
			if(Main.LocalPlayer.dead) {
				CosmicSong.FadeTime = 0;
				return;
			}
			if(Main.LocalPlayer.HeldItem.ModItem is Items.SongoftheCosmos sotc && sotc.charge > 0) {
				CosmicSong.FadeTime = 40;
				return;
			}
			if(CosmicSong.FadeTime > 0) CosmicSong.FadeTime--;
		}

		protected override bool DrawSelf() {
			Player player = Main.LocalPlayer;
			if(player.dead || player.ghost || CosmicSong.FadeTime == 0 || Main.ingameOptionsWindow || Main.InGameUI.IsVisible) return true;
			int charge = 0;
			if(player.HeldItem.ModItem is Items.SongoftheCosmos sotc) charge = sotc.charge;
			else return true;
			if(charge > 1000) charge = 1000;
			float alphaMult = Math.Min((float)CosmicSong.FadeTime / 35f, 1f);
			Texture2D texture = this.SheetAsset.Value;
			Rectangle frame = Utils.Frame(texture, 1, 4, 0, 0, 0, 0);
			Vector2 origin = Utils.Size(frame) / 2f;
			Vector2 position = Utils.Floor(player.Bottom + new Vector2(0f, (float)(14 + InterfaceResource.ThoriumGaugeOffset) + player.gfxOffY));
			Color color = Color.White * alphaMult;
			InterfaceResource.IncreaseThoriumGaugeOffset(26);
			if(Main.playerInventory && Main.screenHeight < 1000 && (player.breath < player.breathMax || player.lavaTime < player.lavaMax)) position.Y += (float)(20 + 20 * ((player.breathMax - 1) / 200 + 1));
			position = Vector2.Transform(position - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix) / Main.UIScale;
			Main.spriteBatch.Draw(texture, position, frame, color, 0f, origin, 1f, 0, 0f);
			float fill = (float)charge / 1000f;
			frame.Width = (int)((float)(frame.Width - 8) * fill + 8f);
			frame.Y = frame.Height * (charge >= 1000 ? 1 : 2);
			Main.spriteBatch.Draw(texture, position, frame, color, 0f, origin, 1f, 0, 0f);
			frame.Y = frame.Height * 3;
			frame.Width = texture.Width;
			Main.spriteBatch.Draw(texture, position - new Vector2(6, 0), frame, color, 0f, origin, 1f, 0, 0f);
			if(Main.ingameOptionsWindow || Main.InGameUI.IsVisible || Main.mouseText) return true;
			frame = new Rectangle((int)position.X - frame.Width / 2, (int)position.Y - frame.Height / 2, frame.Width, frame.Height);
			if(!frame.Contains(Main.mouseX, Main.mouseY)) return true;
			player.cursorItemIconEnabled = false;
			string text = charge + " / 1000";
			Main.instance.MouseTextHackZoom(text, -12, 0, null);
			Main.mouseText = true;
			return true;
		}

		public override int GetInsertIndex(List<GameInterfaceLayer> layers) => layers.FindIndex((GameInterfaceLayer layer) => layer.Active && layer.Name.Equals("Vanilla: Ingame Options"));
		private Asset<Texture2D> sheetAsset;

		public const int MAX_FADE_TIME = 35;

		public const int FADE_DELAY = 5;
	}
}