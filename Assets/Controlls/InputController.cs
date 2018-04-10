using UnityEngine;

public class InputController : MonoBehaviour
{
    private static InputController _iCtrl;
    public static InputController ICtrl
    {
        get { return _iCtrl; }
        set { _iCtrl = value; }
    }

    private void Awake()
    {
        if (InputController.ICtrl != null)
            DestroyImmediate(this);
        else
            ICtrl = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Intercept TouchPosition
        DetectTouch();

        //Intercept MousePosition
        DetectMouse();

        InputDebug();
    }

    public bool TouchSensible = true;
    Vector2? touchPos = null;
    void DetectTouch()
    {
        if (TouchSensible)
        {
            if (Input.touchCount > 0)
                mousePos = Input.GetTouch(0).position;
            else
                touchPos = null;
        }
    }

    public bool MouseSensible = true;
    Vector2? mousePos = null;
    void DetectMouse()
    {
        if (MouseSensible)
        {
            if (Input.GetMouseButton(0))
                mousePos = Input.mousePosition;
            else
                mousePos = null;
        }
    }

    #region API
    /// <summary>
    /// Return last pointer position (Touch if possibile, than Mouse)
    /// Relative to Screen position
    /// Can return null
    /// </summary>
    /// <returns></returns>
    public Vector2? GetPointerPosition()
    {
        Vector2? onScreenPosition = null;

        if (mousePos != null)
            onScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.Value.x, mousePos.Value.y, 0));
        else if (touchPos != null)
            onScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.Value.x, touchPos.Value.y, 0));

        return onScreenPosition;
    }

    /// <summary>
    /// Return last pointer position (Touch if possibile, than Mouse)
    /// </summary>
    /// <returns></returns>
    public Vector2? GetPointerPositionRaw()
    {
        if (touchPos != null)
            return touchPos;

        if (mousePos != null)
            return mousePos;

        return null;
    }

    /// <summary>
    /// Last pointer position (Touch if possibile, than Mouse)
    /// Relative to Screen position
    /// Can be null
    /// </summary>
    public Vector2? PointerPosition { get { return GetPointerPosition(); } }
    #endregion

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

