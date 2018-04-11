using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BallisticProbe : MonoBehaviour
{
    public int MaxLenght = 20;
    int currentLenght;

    public MainCharacter mainCh;

    LineRenderer lineRend;
    Vector3 gravity;

    private bool _isRunning;
    public bool IsRunning
    {
        get { return _isRunning; }
        set
        {
            _isRunning = value;
            if (_isRunning == false)
            {
                currentLenght = 0;
                lineRend.positionCount = 0;
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        gravity = Physics2D.gravity;

        lineRend = GetComponent<LineRenderer>();

        mainCh = GetComponentInParent<MainCharacter>();

        IsRunning = false;
    }

    private void FixedUpdate()
    {
        if (IsRunning)
            UpdateTrajectory(mainCh.JumpSpeed);
    }

    void UpdateTrajectory(Vector3 initialVelocity)
    {
        if(currentLenght < MaxLenght)
            currentLenght++;

        float timeStep = .5f / initialVelocity.magnitude;

        if(lineRend.positionCount != currentLenght)
            lineRend.positionCount = currentLenght;

        Vector3 position = transform.position;
        Vector3 velocity = initialVelocity;
        for (int i = 0; i < currentLenght; ++i)
        {
            lineRend.SetPosition(i, position);

            position += velocity * timeStep + 0.5f * gravity * timeStep * timeStep;
            velocity += gravity * timeStep;
        }
    }
}
