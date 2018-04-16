using UnityEngine;

public class GameplayInputController : MonoBehaviour
{
    public float DragElasticSensibility = .5f;
    public float ForceMultiplier = 10f;
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
    Vector2 pointerDirection;
    MovementDirections diretion = MovementDirections.Right;
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

        mainCh.ChanrgeJump(Vector2.zero, MovementDirections.Jumping);
    }

    void OnPointerDrag()
    {
        Vector2 controllerPos = new Vector2(pointerPosition.Value.x - transform.position.x, pointerPosition.Value.y - transform.position.y) * DragElasticSensibility;
        distanceFromCenter = controllerPos.magnitude;

        pointerDirection = distanceFromCenter < DeadZoneRadius ? controllerPos : controllerPos.normalized * DeadZoneRadius;
        pointerDirection *= ForceMultiplier;

        diretion = EvaluateDirection(controllerPos);
        switch (diretion)
        {
            case MovementDirections.Right:
                {
                    pointerDirection = Vector2.right * pointerDirection.magnitude;
                    mainCh.ChargeDash(pointerDirection, diretion);
                }
                break;
            case MovementDirections.Left:
                {
                    pointerDirection = -Vector2.right * pointerDirection.magnitude;
                    mainCh.ChargeDash(pointerDirection, diretion);
                }
                break;
            case MovementDirections.Jumping:
                {
                    pointerDirection *= -1;
                    mainCh.ChanrgeJump(pointerDirection, diretion);
                }
                break;
            default:
                break;
        }

        MoveControllerUI(pointerDirection);
    }

    void OnPointerUp()
    {
        HideController();

        switch (diretion)
        {
            case MovementDirections.Right:
                mainCh.Dash(pointerDirection);
                break;
            case MovementDirections.Left:
                mainCh.Dash(pointerDirection);
                break;
            case MovementDirections.Jumping:
                mainCh.Jump(pointerDirection);
                break;
            default:
                break;
        }
    }

    MovementDirections EvaluateDirection(Vector2 _dir)
    {
        float angle = Vector2.SignedAngle(mainCh.transform.up, _dir);

        if (angle >= 120 || angle <= -120)
            return MovementDirections.Jumping;
        else if (angle < 0)
            return MovementDirections.Left;
        else
            return MovementDirections.Right;
    }

    #region UI
    void ShowController()
    {
        IsPointerHolding = true;

        transform.position = pointerPosition ?? default(Vector2);
    }

    void HideController()
    {
        IsPointerHolding = false;

        Center.transform.localPosition = Vector2.zero;
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
