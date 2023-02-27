using SpaceWarp.API.Mods;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace ColorPresets;

[MainMod]
public class ColorPresetsMod : Mod
{
    public override void OnInitialized()
    {
        Logger.Info("Color Presets is initialized");
    }
}

[HarmonyPatch(typeof(KSP.OAB.ObjectAssemblyColorPicker))]
[HarmonyPatch("Start")]
class ColorPickerPatcher
{
    public static void Postfix(KSP.OAB.ObjectAssemblyColorPicker __instance)
    {
        var agencyColorContainer = __instance.transform
            .FindChildEx("UIPanel")
            .FindChildEx("GRP-Body")
            .FindChildEx("Agency Colors Buttons");
        
        var horizontalContainer = agencyColorContainer.FindChildEx("GRP-Agency Colors");
        
        // move and rename restore button (use)
        var useButton = agencyColorContainer.FindChildEx("BTN-RestoreAgencyColors");
        useButton.SetParent(horizontalContainer);
        useButton.GetComponentInChildren<TextMeshProUGUI>().SetText("USE");
        useButton.name = "BTN-UseAgencyColors";
        
        // move and rename set button
        var setButton = agencyColorContainer.FindChildEx("BTN-SetAgencyColors");
        setButton.SetParent(horizontalContainer);
        setButton.GetComponentInChildren<TextMeshProUGUI>().SetText("SET");
    }
}

