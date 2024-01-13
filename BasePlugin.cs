using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;

namespace BBGenFixes.Plugin
{
	[BepInPlugin(ModInfo.GUID, ModInfo.Name, ModInfo.Version)]
	public class BasePlugin : BaseUnityPlugin
	{
		void Awake()
		{
			Harmony harmony = new(ModInfo.GUID);
			harmony.PatchAll();

			EnableMysteryRoomFix = !Chainloader.PluginInfos.ContainsKey("pixelguy.pixelmodding.baldiplus.bbextracontent");
		}

		public static bool EnableMysteryRoomFix = true;

	}


	internal static class ModInfo
	{
		internal const string GUID = "pixelguy.pixelmodding.baldiplus.bbgenfixes";
		internal const string Name = "BB+ Gen Fixes";
		internal const string Version = "1.0.0";
	}


}
