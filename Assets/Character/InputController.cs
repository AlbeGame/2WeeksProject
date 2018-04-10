using UnityEngine;

public class InputController : MonoBehaviour
{
    private static InputController _iCtrl;
    public static InputController CtrlInstance
    {
        get { return _iCtrl; }
        set { _iCtrl = value; }
    }

    private void Awake()
    {
        if (InputController.CtrlInstance != null)
            DestroyImmediate(this);
        else
            CtrlInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Intercept TouchPosition
        if (TouchSensible)
        {
            if (Input.touchCount > 0)
                mousePos = Input.GetTouch(0).position;
            else
                touchPos = null;
        }

        //Intercept MousePosition
        if (MouseSensible)
        {
            if (Input.GetMouseButton(0))
                mousePos = Input.mousePosition;
            else
                mousePos = null;
        }

        InputDebug();
    }

    public bool TouchSensible = true;
    public bool MouseSensible = true;

    Vector2? touchPos = null;
    Vector2? mousePos = null;
    //Return last pointer position (Touch if possibile, than Mouse)
    public Vector2? GetPointerPosition()
    {
        Vector2? onScreenPosition = null;

        if (mousePos != null)
            onScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.Value.x, mousePos.Value.y, 0));
        else if (touchPos != null)
            onScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.Value.x, touchPos.Value.y, 0));

        return onScreenPosition;
    }
    public Vector2? PointerPosition { get { return GetPointerPosition(); } }
    #region Debug
    public GameObject DebugTool;
    void InputDebug()
    {
        if (PointerPosition != null)
        {
            if(!DebugTool.activeSelf)
                DebugTool.SetActive(true);
            DebugTool.transform.position = PointerPosition ?? default(Vector2);
        }
        else
        {
            if (DebugTool.activeSelf)
                DebugTool.SetActive(false);
        }
    }
    #endregion
}

