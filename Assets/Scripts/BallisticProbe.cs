using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BallisticProbe : MonoBehaviour
{
    public EvaluationMode Evaluation; 
    public int MaxLenght = 50;
    int currentLenght;

    MainCharacter mainCh;

    public ProbedTrajectory ProbedParable { get; private set; }

    Vector2 gravity;

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
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        gravity = Physics2D.gravity;

        mainCh = GetComponentInParent<MainCharacter>();

        IsRunning = false;
    }

    private void FixedUpdate()
    {
        if (IsRunning)
        {
            UpdateTrajectory();
            UpdateCollision();
        }
    }

    void UpdateTrajectory()
    {
        if(currentLenght < MaxLenght)
            currentLenght++;

        float timeStep;
        if (Evaluation == EvaluationMode.BySpace)
            timeStep = .5f / mainCh.JumpSpeed.magnitude;
        else
            timeStep = Time.fixedDeltaTime;

        ProbedParable.KinematicPoints.Clear();

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 velocity = mainCh.JumpSpeed;
        for (int i = 0; i < currentLenght; ++i)
        {
            ProbedParable.KinematicPoints.Add(position);
            position += velocity * timeStep + 0.5f * gravity * timeStep * timeStep;
            velocity += gravity * timeStep;
        }
    }

    void UpdateCollision()
    {
        RaycastHit2D hit;
        for (int i = 0; i < ProbedParable.KinematicPoints.Count; i++)
        {
            hit = Physics2D.CircleCast(ProbedParable.KinematicPoints[i], 0.5f, Vector2.zero);
            if (hit.collider == null)
                continue;

            if (hit.collider.tag == "Wall")
            {
                ProbedParable.KinematicPoints = ProbedParable.KinematicPoints.Take(i).ToList();
                ProbedParable.LastCollision = hit;
                break;
            }
        }
    }

    public void Init(MainCharacter _character)
    {
        ProbedParable = new ProbedTrajectory();
    }

    public class ProbedTrajectory
    {
        public Vector2? InitialVelocity;
        public List<Vector2> KinematicPoints = new List<Vector2>();
        public RaycastHit2D LastCollision = new RaycastHit2D();
    }

    public enum EvaluationMode
    {
        ByTime,
        BySpace
    }
}
