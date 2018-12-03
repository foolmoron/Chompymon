using UnityEngine;
using System.Collections;

public class DemonSetter : Manager<DemonSetter> {

    public Material DemonMaterial;
    public SpriteRenderer[] Eyes;
    public TextMesh Name;
    public TextMesh Level;
    public TextMesh Size;
    public TextMesh Craving;

    void Start() {
        var demon = DemonManager.Inst.CurrentDemon;
        DemonMaterial.SetColor("_Color1", Color.HSVToRGB(demon.Hue / 360f, 0.8f, 1));
        DemonMaterial.SetColor("_Color2", Color.HSVToRGB(((demon.Hue + (demon.HueMod == 1 ? 120 : -120) + 360) % 360) / 360f, 0.8f, 1));
        DemonMaterial.SetFloat("_Hue", 0);
        DemonMaterial.SetFloat("_FreqX", demon.FreqX);
        DemonMaterial.SetFloat("_FreqY", demon.FreqY);
        DemonMaterial.SetFloat("_ScrollX", demon.ScrollX);
        DemonMaterial.SetFloat("_ScrollY", demon.ScrollY);
        DemonMaterial.SetFloat("_Warp", demon.Warp);
        DemonMaterial.SetFloat("_Lerp10", demon.Lerp10);
        foreach (var eye in Eyes) {
            eye.color = Color.HSVToRGB(demon.EyeHue / 360f, 0.8f, 1);
        }
        UpdateWindow();
    }

    void Update() {
        var demon = DemonManager.Inst.CurrentDemon;
        if (demon == null) {
            return;
        }
        Name.text = demon.Name;
        Level.text = "Level " + demon.Multiplier;
        Size.text = (demon.Size / 100) + "." + demon.Size % 100 + "kg";
        var sizeString = demon.Craving == FileType.Chompymon
            ? "Level " + demon.Multiplier
            : (int) Mathf.Pow(2, demon.Multiplier) + "kb"
            ;
        Craving.text = $"Evolves with a\n{sizeString} {demon.Craving}...";
    }

    const int originalSize = 600;
    const int step = 40;
    public void UpdateWindow() {
        var size = originalSize + step * DemonManager.Inst.CurrentDemon.Multiplier;
        Screen.SetResolution(size, size, false);
    }
}
