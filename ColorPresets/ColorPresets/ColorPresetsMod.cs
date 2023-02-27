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
        // resize the window; TODO: make scrolling instead
        var rootTransform = (RectTransform) __instance.transform;
        rootTransform.sizeDelta += new Vector2(0, 150);
        
        // create a copy of the agency colors buttons to use for the preset
        var bodyTransform = __instance.transform.FindChildEx("UIPanel").FindChildEx("GRP-Body");
        var agencyColorsButtons = bodyTransform.FindChildEx("Agency Colors Buttons");
        var preset = UnityEngine.Object.Instantiate(agencyColorsButtons.gameObject, bodyTransform);
        preset.name = "Color presets";
        var presetTransform = (RectTransform) preset.transform;

        // move the preset
        presetTransform.SetSiblingIndex(agencyColorsButtons.GetSiblingIndex() + 1);
        presetTransform.anchoredPosition += new Vector2(0, -150);
        
        // change the label
        var label = presetTransform.FindChildEx("PartsManager-ELE-Property Name");
        label.name = "Label";
        label.GetComponent<TextMeshProUGUI>()
            .SetText("Extra color presets");
        label.SetSiblingIndex(0);
        
        // change the "set" button to "save new preset"
        var saveButton = presetTransform.FindChildEx("BTN-SetAgencyColors");
        saveButton.SetSiblingIndex(1);
        saveButton.GetComponentInChildren<TextMeshProUGUI>()
            .SetText("Save new preset");
        saveButton.name = "BTN-SaveNewPreset";
        
        // move the colors
        var presetContainer = presetTransform.FindChildEx("GRP-Agency Colors");
        presetContainer.SetSiblingIndex(2);
        presetContainer.name = "GRP-Preset";

        // move the "restore" button to the colors, change it to "use"
        var useButton = presetTransform.FindChildEx("BTN-RestoreAgencyColors");
        useButton.SetParent(presetContainer);
        useButton.SetSiblingIndex(3);
        useButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Use");
        useButton.name = "BTN-Use";

        var removeButton = UnityEngine.Object.Instantiate(useButton.gameObject, presetContainer);
        removeButton.GetComponentInChildren<TextMeshProUGUI>().SetText("Del");
        removeButton.name = "BTN-Delete";
    }
}

