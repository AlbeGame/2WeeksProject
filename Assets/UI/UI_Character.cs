using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class UI_Character : MonoBehaviour {

    LineRenderer lineRend;

	// Use this for initialization
	void Start () {
        lineRend = GetComponent<LineRenderer>();
	}

    public void DrawParabable()
    {

    }
}
