using SpaceWarp.API.Mods;
using UnityEngine;

namespace ColorPresets;

[MainMod]
public class ColorPresetsMod : Mod
{
    private static readonly GUIStyle ColorBoxStyle = new()
        { normal = new GUIStyleState { background = new Texture2D(20, 10) } };

    private const int WindowWidth = 350;
    private const int WindowHeight = 700;

    private Rect _windowRect = new(
        Screen.width * 0.85f - WindowWidth / 2f,
        Screen.height / 2 - WindowHeight / 2,
        0,
        0
    );

    private bool _drawUI;

    public override void OnInitialized()
    {
        Logger.Info("Color Presets is initialized");
    }

    private void OnGUI()
    {
        if (_drawUI)
        {
            _windowRect = GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                _windowRect,
                FillWindow,
                "Color Manager",
                GUILayout.Height(WindowHeight),
                GUILayout.Width(WindowWidth)
            );
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Semicolon))
            _drawUI = !_drawUI;
    }

    private void FillWindow(int windowID)
    {
        GUILayout.BeginVertical();

        GUILayout.Button("Add preset");

        DrawPreset(new ColorPreset(new Color(1, 0, 0, 1), new Color(0, 1, 0, 1)));
        DrawPreset(new ColorPreset(new Color(0, 0, 1, 1), new Color(1, 1, 0, 1)));

        GUILayout.EndVertical();
        GUI.DragWindow(new Rect(0, 0, 10000, 500));
    }

    private static void DrawPreset(ColorPreset preset)
    {
        GUILayout.BeginHorizontal();

        DrawColorBox(preset.Primary);
        DrawColorBox(preset.Secondary);
        GUILayout.Button("Use");
        GUILayout.Button("Delete");

        GUILayout.EndHorizontal();
    }

    private static void DrawColorBox(Color color)
    {
        var old = GUI.backgroundColor;
        GUI.backgroundColor = color;
        GUILayout.Box(GUIContent.none, ColorBoxStyle);
        GUI.backgroundColor = old;
    }
}

public class ColorPreset
{
    public Color Primary;
    public Color Secondary;

    public ColorPreset(Color primary, Color secondary)
    {
        Primary = primary;
        Secondary = secondary;
    }
}