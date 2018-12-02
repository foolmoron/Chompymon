using UnityEngine;
using System.Collections;

public class StretchToScreenWidth : MonoBehaviour {

    public Camera TargetCamera;

    void Update() {
        transform.localScale = transform.localScale.withX(Screen.width * 2 / 100f);
        transform.position = transform.position.withX(TargetCamera.transform.position.x);
    }
}
