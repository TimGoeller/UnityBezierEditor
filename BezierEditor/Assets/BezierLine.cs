﻿using UnityEngine;

public class BezierLine
{
    private Vector3[] points = new Vector3[4];

    private BezierPoint start;
    private BezierPoint end;
    private Vector3 startHandle;
    private Vector3 endHandle;

    public Vector3 Start
    {
        get
        {
            return start;
        }
        set
        {
            if(start != value)
            {
                Vector3 movementDelta = value - start;
                start.position = PreprocessPoint(value);
                startHandle += movementDelta;
                if (previousLine != null)
                    previousLine.endHandle += movementDelta;
            }
        }
    }

    public Vector3 End
    {
        get
        {
            return end;
        }
        set
        {
            if(end != value)
            {
                Vector3 movementDelta = value - end;
                end.position = PreprocessPoint(value);
                endHandle += movementDelta;
                if (nextLine != null)
                    nextLine.startHandle += movementDelta;
            }
        }
    }

    public Vector3 StartHandle
    {
        get
        {
            return startHandle;
        }
        set
        {
            if(startHandle != value)
            {
                startHandle = PreprocessPoint(value);
                if (previousLine != null)
                    previousLine.endHandle = start + (start - startHandle);
            }
        }
    }

    public Vector3 EndHandle
    {
        get
        {
            return endHandle;
        }
        set
        {
            if(endHandle != value)
            {
                endHandle = PreprocessPoint(value);
                if (nextLine != null)
                    nextLine.startHandle = end + (end - endHandle);
            }
        }
    }

    private BezierLine previousLine;
    public BezierLine nextLine;

    public enum BezierPoints
    {
        StartPoint = 0, EndPoint = 1, StartHandle = 2, EndHandle = 3
    }

    public BezierLine(BezierLine previousLine, Vector3 endPoint)
    {
        SetPreviousLine(previousLine);
        end = new BezierPoint(endPoint);
        endHandle = end + Vector3.forward;
    }

    public BezierLine(Vector3 startPoint, Vector3 endPoint)
    {
        start = new BezierPoint(startPoint);
        end = new BezierPoint(endPoint);
        startHandle = start + Vector3.forward;
        endHandle = end + Vector3.forward;
    }

    public void SetPreviousLine(BezierLine previousLine)
    {
        this.previousLine = previousLine;
        previousLine.nextLine = this;

        start = previousLine.end;
        startHandle = start - (previousLine.endHandle - start);
    }

    public Vector3 PreprocessPoint(Vector3 point)
    {
        return point;
    }
}
