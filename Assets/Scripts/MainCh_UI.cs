using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class MainCh_UI : MonoBehaviour
{
    MainCharacter mainCh;
    LineRenderer lineRend;

    bool _displayTrajectory;
    public bool DisplayTrajectory {
        get { return _displayTrajectory; }
        set
        {
            _displayTrajectory = value;
            if (_displayTrajectory)
            {
                lineRend.enabled = true;
                lineRend.positionCount = 0;
            }
            else
                lineRend.enabled = false;
        }
    }

    public void Init(MainCharacter _mainCh)
    {
        mainCh = _mainCh;
        lineRend = GetComponent<LineRenderer>();
        lineRend.startColor = Color.gray;
        DisplayTrajectory = false;
    }


    private void LateUpdate()
    {
        if(DisplayTrajectory)
            RendLine();
    }

    void RendLine()
    {
        List<Vector2> kinPositions = mainCh.Trajectory.KinematicPoints;
        if (kinPositions.Count <= 0)
            return;

        if (lineRend.positionCount != kinPositions.Count)
            lineRend.positionCount = kinPositions.Count;

        for (int i = 0; i < lineRend.positionCount; i++)
            lineRend.SetPosition(i, kinPositions[i]);

        if (mainCh.Trajectory.LastCollision.collider != null && mainCh.Trajectory.LastCollision.collider.GetComponent<Surface_Breakable>() != null)
            lineRend.endColor = Color.red;
        else
            lineRend.endColor = Color.black;
    }
}
