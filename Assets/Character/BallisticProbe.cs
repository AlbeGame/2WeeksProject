using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]
public class BallisticProbe : MonoBehaviour
{
    public EvaluationMode Evaluation; 
    public int MaxLenght = 20;
    int currentLenght;

    MainCharacter mainCh;

    ProbedTrajectory UnprobedParable;
    ProbedTrajectory ProbedParable;


    List<Box2DProbe> probes = new List<Box2DProbe>();


    LineRenderer lineRend;
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
        {
            UpdateTrajectory();
            UpdateTrajectoryCollision();
        }
        else
        {
            TurnOffProbes();
        }
    }

    private void Update()
    {
        if(IsRunning)
            RendLine();
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

        if (UnprobedParable.KinematicPoints.Length != currentLenght)
            UnprobedParable.SetKinematics(new  Vector2[currentLenght]);

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 velocity = mainCh.JumpSpeed;
        for (int i = 0; i < currentLenght; ++i)
        {
            UnprobedParable.KinematicPoints[i] = position;
            position += velocity * timeStep + 0.5f * gravity * timeStep * timeStep;
            velocity += gravity * timeStep;
        }
    }

    void UpdateTrajectoryCollision()
    {
        //Instanciate brand new set of Probes
        if(probes.Count == 0)
        {
            BoxCollider2D probeShape = mainCh.GetComponent<BoxCollider2D>();
            for (int i = 0; i < MaxLenght; i++)
            {
                probes.Add(new GameObject("Box2DProbe_" +i).AddComponent<Box2DProbe>());
                probes[i].Init(this, probeShape);
            }
        }

        //Check collision along the parabole
        for (int i = 0; i < UnprobedParable.KinematicPoints.Length; i++)
        {
            probes[i].ColliderActive = true;
            probes[i].transform.position = UnprobedParable.KinematicPoints[i];
        }
        //turnoff the unnecessary colliders
        for (int i = UnprobedParable.KinematicPoints.Length; i < probes.Count; i++)
        {
            probes[i].ColliderActive = false;
        }
    }

    void RendLine()
    {
        if (ProbedParable.KinematicPoints == null)
            return;

        if (lineRend.positionCount != ProbedParable.ClosestCollision)
            lineRend.positionCount = ProbedParable.ClosestCollision;

        for (int i = 0; i < ProbedParable.ClosestCollision; i++)
            lineRend.SetPosition(i, ProbedParable.KinematicPoints[i]);
    }

    void TurnOffProbes()
    {
        if (probes.Count <= 0)
            return;

        for (int i = 0; i < probes.Count; i++)
        {
            if (probes[i].ColliderActive == false)
                return;
            probes[i].ColliderActive = false;
        }
    }

    public void Init(MainCharacter _character)
    {
        mainCh = _character;

        UnprobedParable.SetKinematics(new Vector2[0]);
        ProbedParable.SetKinematics(new Vector2[0]);
        ProbedParable.TimeWhenRecorded = Time.time;
    }

    public void NotifyCollision(Box2DProbe other)
    {
        for (int i = 0; i < UnprobedParable.KinematicPoints.Length; i++)
        {
            if(probes[i] == other)
            {
                if(ProbedParable.TimeWhenRecorded < Time.time)
                {
                    ProbedParable.ClosestCollision = i;
                    ProbedParable.SetKinematics(UnprobedParable.KinematicPoints);
                    ProbedParable.TimeWhenRecorded = Time.time;
                }
                else if (i < ProbedParable.ClosestCollision)
                {
                    ProbedParable.ClosestCollision = i;
                    ProbedParable.SetKinematics(UnprobedParable.KinematicPoints);
                }
                return;
            }
        }
    }

    public struct ProbedTrajectory
    {
        public Vector2? InitialVelocity;
        public Vector2[] KinematicPoints;
        public float TimeWhenRecorded;
        public int ClosestCollision;

        public void SetKinematics(Vector2[] _newKinematics)
        {
            KinematicPoints = _newKinematics;
            ClosestCollision = _newKinematics.Length;
        }

        public void ResetClosestCollision()
        {
            ClosestCollision = KinematicPoints != null ? KinematicPoints.Length : 0;
        }
    }

    public enum EvaluationMode
    {
        ByTime,
        BySpace
    }
}
