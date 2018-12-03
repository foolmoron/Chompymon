using UnityEngine;
using System.Collections;

public class DemonSetter : Manager<DemonSetter> {

    public Material DemonMaterial;
    public TextMesh Name;
    public TextMesh Level;
    public TextMesh Size;
    public TextMesh Craving;

    void Start() {
        var demon = DemonManager.Inst.CurrentDemon;
        Name.text = demon.Name;
        Level.text = "Level " + demon.Multiplier;
        Size.text = (demon.Size / 100) + "." + demon.Size % 100 + "kg";
        Craving.text = "Evolves with a\n1000mb Image...";
    }
}
