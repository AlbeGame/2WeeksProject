using UnityEngine;

public class MainCharacter : MonoBehaviour {

    RopeController ropeCtrl;

    Vector2? pointerPosition { get { return InputController.CtrlInstance.GetPointerPosition(); } }
    bool _isThrowing;
    bool IsThrowing
    {
        get { return _isThrowing; }
        set { _isThrowing = value; }
    }

	// Use this for initialization
	void Start () {
        ropeCtrl = GetComponentInChildren<RopeController>();
	}
	
	// Update is called once per frame
	void Update () {
        if(pointerPosition != null)
        {

        }
	}

    void DrawTranjectory()
    {

    }
}
