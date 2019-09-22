using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolylineCreator : MonoBehaviour
{
    public BezierLine start;
    public bool closed;

    public void AddLine(Vector3 endPoint)
    {
        if (closed)
        {
            BezierLine current = start;
            float lowestMangitude = float.MaxValue;
            BezierLine currentClosest = current;

            do
            {
                float magnitude = ((endPoint - current.Start) + (endPoint - current.End)).magnitude;

                if (magnitude < lowestMangitude)
                {
                    lowestMangitude = magnitude;
                    currentClosest = current;
                }
                current = current.nextLine;
            }
            while (current.nextLine != start);

            BezierLine currentClosestNext = currentClosest.nextLine;
            BezierLine newLine = new BezierLine(currentClosest, endPoint);
            currentClosestNext.SetPreviousLine(newLine);
        }
        else
        {
            BezierLine currentLast = GetLastLine();
            BezierLine newLine = new BezierLine(currentLast, endPoint);
            newLine.SetPreviousLine(currentLast);
        }

    }

    public void CreateStart()
    {
        start = new BezierLine(transform.position, transform.position + Vector3.right);
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
        BezierLine fillerLine = new BezierLine(lastLine.End, start.Start);
        fillerLine.SetPreviousLine(lastLine);
        start.SetPreviousLine(fillerLine);
        closed = true;
    }

}

