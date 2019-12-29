using System;
using System.Collections.Generic;
using System.Composition;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ArchiSteamFarm.Json;
using ArchiSteamFarm.Plugins;
using Newtonsoft.Json.Linq;
using SteamKit2;
namespace ArchiSteamFarm.Cobra.ClearNicknamesPlugin {
	[Export(typeof(IPlugin))]
	internal sealed class ClearNicknamesPlugin : IBotCommand {
		private static readonly Random Random = new Random();
		internal static int RandomNext(int min, int max) {
			lock (Random) {
				return Random.Next(min,max);
			}
		}
		public string Name => nameof(ClearNicknamesPlugin);
		public Version Version => typeof(ClearNicknamesPlugin).Assembly.GetName().Version;
		public async Task<string> OnBotCommand(Bot bot, ulong steamID, string message, string[] args) {
			switch (args[0].ToUpperInvariant()) {
				case "CLEARNICKNAMES" when bot.HasPermission(steamID, BotConfig.EPermission.Master):
					string request = "/profiles/" + bot.SteamID + "/ajaxclearaliashistory/";
					//1 field for the sessionid
					Dictionary<string, string> data = new Dictionary<string, string>(1) { };
					await bot.ArchiWebHandler.UrlPostToHtmlDocumentWithSession(ArchiWebHandler.SteamCommunityURL, request, data).ConfigureAwait(false);
					return bot.Commands.FormatBotResponse(ArchiSteamFarm.Localization.Strings.Done);
				default:
					return null;
			}
		}
		public void OnLoaded() {
			ASF.ArchiLogger.LogGenericInfo("ClearNicknamesPlugin by Cobra");
		}
	}
}
