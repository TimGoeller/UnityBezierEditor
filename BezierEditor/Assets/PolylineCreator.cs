using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolylineCreator : MonoBehaviour
{
    public BezierLine start;
    public bool closed;

    public void AddLine(Vector3 endPoint)
    {
        if(closed)
        {
            BezierLine current = start;
            float lowestMangitude = float.MaxValue;
            BezierLine currentClosest = current;
            while (current.nextLine != null)
            {
                float magnitude = ((endPoint - current.GetPoint(BezierLine.BezierPoints.StartPoint)) + (endPoint - current.GetPoint(BezierLine.BezierPoints.EndPoint))).magnitude;

                if(magnitude < lowestMangitude)
                {
                    lowestMangitude = magnitude;
                    currentClosest = current;
                }
                current = current.nextLine;
            }
            BezierLine newLine = new BezierLine(Vector3.zero, endPoint, Vector3.zero, endPoint + Vector3.forward);
            newLine.SetPreviousLine(currentClosest);
        }
        else
        {
            BezierLine currentLast = GetLastLine();
            BezierLine newLine = new BezierLine(Vector3.zero, endPoint, Vector3.zero, endPoint + Vector3.forward);
            newLine.SetPreviousLine(currentLast);
        }
        
    }

    public void CreateStart()
    {
        start = new BezierLine(transform.position, transform.position + Vector3.right, transform.position + Vector3.forward, transform.position + Vector3.right + Vector3.back);
        closed = false;
    }

    public BezierLine GetLastLine()
    {
        BezierLine current = start;
        while (current.nextLine != null)
        {
            current = current.nextLine;
        }

        return current;
    }


    public void CloseLine()
    {
        BezierLine lastLine = GetLastLine();
        BezierLine fillerLine = new BezierLine(lastLine.GetPoint(BezierLine.BezierPoints.EndPoint), start.GetPoint(BezierLine.BezierPoints.StartPoint), Vector3.zero, Vector3.zero);
        fillerLine.SetPreviousLine(lastLine);
        start.SetPreviousLine(fillerLine);
        closed = true;
    }

}

