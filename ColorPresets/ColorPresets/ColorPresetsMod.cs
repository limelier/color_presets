using SpaceWarp.API.Mods;
using HarmonyLib;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

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
        var body = __instance.transform
            .FindChildEx("UIPanel")
            .FindChildEx("GRP-Body");

        SquishActiveColors(body);
        SquishAgencyButtons(body);
        CreatePresetsContainer(body);
    }

    private static void CreatePresetsContainer(Transform body)
    {
        var presetsContainer = (RectTransform)Object.Instantiate(
            body.FindChildEx("Agency Colors Buttons"),
            body
        );
        presetsContainer.anchoredPosition += new Vector2(0, -70);
        presetsContainer.name = "Extra Color Presets";

        var presetsLabel = presetsContainer.FindChildEx("PartsManager-ELE-Property Name");
        presetsLabel.name = "Label";
        presetsLabel.GetComponent<TextMeshProUGUI>().SetText("Extra color presets");

        var presetContainer = presetsContainer.FindChildEx("GRP-Agency Colors");
        presetContainer.name = "GRP-Preset";

        var useButton = presetContainer.FindChildEx("BTN-UseAgencyColors");
        useButton.GetComponentInChildren<TextMeshProUGUI>().SetText("USE");
        useButton.name = "BTN-Use";

        var removeButton = presetContainer.FindChildEx("BTN-SetAgencyColors");
        removeButton.GetComponentInChildren<TextMeshProUGUI>().SetText("DEL");
        removeButton.name = "BTN-Delete";

        var addButton = Object.Instantiate(removeButton, presetsContainer);
        addButton.name = "BTN-Add Preset";
        addButton.GetComponentInChildren<TextMeshProUGUI>().SetText("+");
    }

    private static void SquishAgencyButtons(Transform body)
    {
        var agencyColorContainer = body.FindChildEx("Agency Colors Buttons");

        // delete extra spacing
        Object.Destroy(agencyColorContainer.FindChildEx("Div"));

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

    private static void SquishActiveColors(Transform body)
    {
        var squishBy = new Vector2(0, 80);
        var activeColorContainer = body
            .FindChildEx("GRP-Selected Colors")
            .FindChildEx("GRP-ColorPreview-Hor");
        ((RectTransform)activeColorContainer).sizeDelta -= squishBy;

        // move everything else up to match; // TODO change body from LayoutElement to VerticalLayoutGroup?
        MoveBy(body.FindChildEx("GRP-HueSlider"), squishBy);
        MoveBy(body.FindChildEx("GRP-ColorPicker"), squishBy);
        MoveBy(body.FindChildEx("Agency Colors Buttons"), squishBy);
    }

    private static void MoveBy(Transform rect, Vector2 delta)
    {
        ((RectTransform)rect).anchoredPosition += delta;
    }
}