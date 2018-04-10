using UnityEngine;

public class GameplayInputController : MonoBehaviour
{
    public float DeadZoneRadius = .5f;
    public GameObject Background;
    public GameObject Center;

    public bool IsVisible { get; private set; }

    // Use this for initialization
    void Start ()
    {
        ToggleGraphic(false);
	}

    Vector2? pointerPosition { get { return InputController.ICtrl.PointerPosition; } }

    private void Update()
    {
        if (!IsVisible)
        {
            if(pointerPosition != null)
                ShowController();
        }
        else
        {
            if (pointerPosition == null)
                HideController();
            else
                MoveController();
        }
    }

    void ShowController()
    {
        IsVisible = true;
        ToggleGraphic(true);

        transform.position = pointerPosition ?? default(Vector2);
    }

    void HideController()
    {
        IsVisible = false;

        Center.transform.localPosition = Vector2.zero;
        ToggleGraphic(false);
    }

    void MoveController()
    {
        Vector2 controllerPos = new Vector2(pointerPosition.Value.x - transform.position.x, pointerPosition.Value.y - transform.position.y);
        Center.transform.localPosition = controllerPos.normalized * DeadZoneRadius;
    }

    void ToggleGraphic(bool active)
    {
        if (Background.activeSelf != active)
            Background.SetActive(active);

        if (Center.activeSelf != active)
            Center.SetActive(active);
    }
}
