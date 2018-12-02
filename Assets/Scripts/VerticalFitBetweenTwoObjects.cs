using UnityEngine;
using System.Collections;

public class VerticalFitBetweenTwoObjects : MonoBehaviour {

    public Transform First;
    public Transform Second;

    void Update() {
        var centerY = (First.transform.position.y + Second.transform.position.y) / 2;
        var dist = Mathf.Abs(First.transform.position.y - Second.transform.position.y);
        transform.localScale = transform.localScale.withY(dist);
        transform.position = transform.position.withY(centerY);
    }
}
