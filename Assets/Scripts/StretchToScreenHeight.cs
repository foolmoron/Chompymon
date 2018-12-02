using UnityEngine;
using System.Collections;

public class StretchToScreenHeight : MonoBehaviour {

    public Camera TargetCamera;

    void Update() {
        transform.localScale = transform.localScale.withY(Screen.height * 2 / 100f);
        transform.position = transform.position.withY(TargetCamera.transform.position.y);
    }
}
