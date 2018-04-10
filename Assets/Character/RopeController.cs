using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeController : MonoBehaviour {

    LineRenderer lineRend;
	// Use this for initialization
	void Start () {
        lineRend = GetComponent<LineRenderer>();
	}

    public void DrowArrow(Vector2 _targetPosition)
    {

    }
}
