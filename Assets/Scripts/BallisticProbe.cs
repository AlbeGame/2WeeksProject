using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BallisticProbe : MonoBehaviour
{
    public bool Gizmos_Kinematics;

    public EvaluationMode Evaluation; 
    public int MaxLenght = 50;
    int currentLenght;

    MainCharacter mainCh;
    Rigidbody2D mainRB;
    CircleCollider2D mainCollider;

    MovementDirections currentDir;
    public ProbedTrajectory LastProbedTrajectory { get; private set; }

    Vector2 gravity;

    bool isRunning;
    private void FixedUpdate()
    {
        if (isRunning)
        {
            UpdateTrajectory();
            UpdateCollision();
        }
    }

    void UpdateTrajectory()
    {
        if(currentLenght < MaxLenght)
            currentLenght++;

        LastProbedTrajectory.KinematicPoints.Clear();
        LastProbedTrajectory.KinematicPoints.TrimExcess();

        float timeStep;
        if (Evaluation == EvaluationMode.BySpace)
            timeStep = .5f / mainCh.JumpSpeed.magnitude;
        else
            timeStep = Time.fixedDeltaTime;

        Vector2 velocity = mainCh.JumpSpeed;
        if (velocity == Vector2.zero)
            return;

        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        for (int i = 0; i < currentLenght; ++i)
        {
            LastProbedTrajectory.KinematicPoints.Add(position);
            position += velocity * timeStep;
            if(currentDir == MovementDirections.Jumping)
            {
                position += 0.5f * gravity * timeStep * timeStep;
                velocity += gravity * timeStep;
            }
            if (currentDir != MovementDirections.Jumping && mainCollider.sharedMaterial != null)
            {
                float staticFriction = mainCollider.sharedMaterial.friction * 9.8f * timeStep;
                velocity += staticFriction < velocity.magnitude ? -velocity.normalized * staticFriction : -velocity;
            }
        }
    }

    void UpdateCollision()
    {
        RaycastHit2D hit;

        for (int i = 0; i < LastProbedTrajectory.KinematicPoints.Count; i++)
        {
            hit = Physics2D.CircleCast(LastProbedTrajectory.KinematicPoints[i], 0.5f, Vector2.zero);

            if (hit.collider == null)
                continue;

            if (hit.collider.tag == "Wall")
            {
                LastProbedTrajectory.KinematicPoints = LastProbedTrajectory.KinematicPoints.Take(i).ToList();
                LastProbedTrajectory.LastCollision = hit;
                break;
            }
        }
    }

    public void Init(MainCharacter _character)
    {
        mainCh = _character;
        mainRB = mainCh.GetComponent<Rigidbody2D>();
        mainCollider = mainCh.GetComponent<CircleCollider2D>();

        gravity = Physics2D.gravity;

        isRunning = false;
        LastProbedTrajectory = new ProbedTrajectory();
    }

    public void StartPrediction(MovementDirections _dir)
    {
        if (!isRunning)
        {
            isRunning = true;
            currentLenght = 0;
            LastProbedTrajectory = new ProbedTrajectory();
        }

        if(currentDir != _dir)
            currentDir = _dir;
    }

    public void StopPrediction()
    {
        isRunning = false;
    }

    private void OnDrawGizmos()
    {
        if (!Gizmos_Kinematics)
            return;

        Gizmos.color = Color.red;
        if (isRunning)
        {
            foreach (Vector2 point in LastProbedTrajectory.KinematicPoints)
            {
                Gizmos.DrawWireSphere(point, .5f);
            }
        }
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
