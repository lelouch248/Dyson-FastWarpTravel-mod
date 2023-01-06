using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DysonSphereProgram.Modding.FastTravelWithWarpers
{
  [BepInPlugin(GUID, NAME, VERSION)]
  public class Plugin : BaseUnityPlugin
  {
    public const string GUID = "dev.lelouch.dsp.FastTravelWithWarpers";
    public const string NAME = "FastTravelWithWarpers";
    public const string VERSION = "1.0.0";

    private Harmony _harmony;
    internal static ManualLogSource Log;

    private void Awake()
    {
      Plugin.Log = Logger;
      _harmony = new Harmony(GUID);
      _harmony.PatchAll(typeof(Patch));
      
      Logger.LogInfo("FastTravelWithWarpers Awake() called");
    }

    private void OnDestroy()
    {
      Logger.LogInfo("FastTravelWithWarpers OnDestroy() called");
      _harmony?.UnpatchSelf();
      Plugin.Log = null;
    }
  }
}
