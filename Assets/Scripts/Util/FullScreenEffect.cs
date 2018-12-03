using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FullScreenEffect : MonoBehaviour {

    public Material Effect;

    void OnRenderImage(RenderTexture src, RenderTexture dest) {
        Graphics.Blit(src, dest, Effect);
    }
}
