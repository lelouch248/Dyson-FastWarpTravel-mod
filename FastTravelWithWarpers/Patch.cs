using HarmonyLib;
using Steamworks;
using UnityEngine;

namespace DysonSphereProgram.Modding.FastTravelWithWarpers;

public class Patch
{
    public static int WarpersRequiredForFastTravel = 5;
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(
        UIStarmap),nameof(UIStarmap.UpdateCursorView))]
    public static void pathFastTravelVisibility(UIStarmap __instance)
    {
        if (GameMain.sandboxToolsEnabled)
        {
            return;
        }

        if (__instance.focusPlanet == null )
        {
            return;
        }

        if (GameMain.mainPlayer.planetId==__instance.focusPlanet.planet.id)
        {
            return;
        }

        if (GameMain.mainPlayer.mecha.thrusterLevel< 3)
        {
         return;   
        }
        __instance.fastTravelButton.gameObject.SetActive(true);
        __instance.fastTravelButton.button.interactable =
            GameMain.mainPlayer.mecha.warpStorage.GetItemCount(ItemProto.kWarperId) >= WarpersRequiredForFastTravel;

    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(
        UIStarmap),nameof(UIStarmap.OnFastTravelButtonClick))]
    public static void BypassFastTravelRestriction(UIStarmap __instance)
    {
        Plugin.Log.LogInfo("BeingClicked");
        if (GameMain.sandboxToolsEnabled)
        {
            return;
        }

        if (__instance.focusPlanet == null)
        {
            return;
        }

        int itemId = ItemProto.kWarperId;
        int count = WarpersRequiredForFastTravel;
        GameMain.mainPlayer.mecha.warpStorage.TakeTailItems(ref itemId, ref count, out _);
        Plugin.Log.LogInfo(count);
        if (count< WarpersRequiredForFastTravel)
        {
            return;
        }
        __instance.StartFastTravelToPlanet(__instance.focusPlanet.planet);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(
        UIStarmap), nameof(UIStarmap.StartFastTravelToUPosition))]
    public static void ByPassFastTravelRestriction2(UIStarmap __instance, VectorLF3 uPos)
    {
        if (GameMain.sandboxToolsEnabled || GameMain.mainPlayer.warping)
        {
            return;
        }

        Player mainplayer = GameMain.mainPlayer;
        __instance.PrepareForFastTravel();
        mainplayer.controller.SetDesireUPosition(uPos);
        __instance.fastTravelling = true;
    }
}