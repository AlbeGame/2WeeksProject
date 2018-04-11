using UnityEngine;

public class GameplayInputController : MonoBehaviour
{
    public float DragElasticSensibility = .5f;
    public float DeadZoneRadius = .5f;
    public GameObject Background;
    public GameObject Center;

    public bool IsPointerHolding { get; private set; }
    public MainCharacter mainCh;

    // Use this for initialization
    void Start ()
    {
        ToggleGraphic(false);

        if (mainCh == null)
            mainCh = FindObjectOfType<MainCharacter>();
	}

    Vector2? pointerPosition { get { return InputController.ICtrl.PointerPosition; } }
    Vector2? pointerDirection = null;
    float distanceFromCenter;

    private void Update()
    {
        if (!IsPointerHolding)
        {
            if (pointerPosition != null)
                OnPointerDown();
        }
        else
        {
            if (pointerPosition == null)
                OnPointerUp();
            else
                OnPointerDrag();
        }
    }

    void OnPointerDown()
    {
        ShowController();

        mainCh.ChanrgeJump(pointerDirection ?? default(Vector2));
    }

    void OnPointerDrag()
    {
        Vector2 controllerPos = new Vector2(pointerPosition.Value.x - transform.position.x, pointerPosition.Value.y - transform.position.y) * DragElasticSensibility;
        distanceFromCenter = controllerPos.magnitude;

        pointerDirection = -controllerPos;

        mainCh.ChanrgeJump(pointerDirection ?? default(Vector2));

        MoveControllerUI(distanceFromCenter < DeadZoneRadius ? controllerPos : controllerPos.normalized * DeadZoneRadius);
    }

    void OnPointerUp()
    {
        HideController();

        mainCh.Jump(pointerDirection ?? default(Vector2));
    }

    #region UI
    void ShowController()
    {
        IsPointerHolding = true;
        ToggleGraphic(true);

        transform.position = pointerPosition ?? default(Vector2);
    }

    void HideController()
    {
        IsPointerHolding = false;

        Center.transform.localPosition = Vector2.zero;
        ToggleGraphic(false);
    }

    void MoveControllerUI(Vector3 _direction)
    {
        Center.transform.localPosition = _direction;
    }

    void ToggleGraphic(bool active)
    {
        if (Background.activeSelf != active)
            Background.SetActive(active);

        if (Center.activeSelf != active)
            Center.SetActive(active);
    }
    #endregion
}
