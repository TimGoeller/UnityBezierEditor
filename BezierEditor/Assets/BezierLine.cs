using UnityEngine;

public class BezierLine
{
    private Vector3[] points = new Vector3[4];

    public BezierLine previousLine;
    public BezierLine nextLine;

    public enum BezierPoints
    {
        StartPoint = 0, EndPoint = 1, StartHandle = 2, EndHandle = 3
    }

    public BezierLine(Vector3 startPoint, Vector3 endPoint, Vector3 startHandle, Vector3 endHandle)
    {
        startPoint.y = 0;
        endPoint.y = 0;
        startHandle.y = 0;
        endHandle.y = 0;

        points[(int)BezierPoints.StartPoint] = startPoint;
        points[(int)BezierPoints.EndPoint] = endPoint;
        points[(int)BezierPoints.StartHandle] = startHandle;
        points[(int)BezierPoints.EndHandle] = endHandle;
    }

    public void SetPreviousLine(BezierLine previousLine)
    {
        this.previousLine = previousLine;
        previousLine.nextLine = this;

        points[(int)BezierPoints.StartPoint] = previousLine.GetPoint(BezierPoints.EndPoint);
        points[(int)BezierPoints.StartHandle] = points[(int)BezierPoints.StartPoint] - (previousLine.GetPoint(BezierPoints.EndHandle) - points[(int)BezierPoints.StartPoint]);
    }

    public void MovePoint(BezierPoints index, Vector3 newPosition)
    {
        newPosition.y = 0;
        if (newPosition != points[(int)index])
        {
            switch (index)
            {
                case BezierPoints.StartPoint: //endPoint
                    points[(int)BezierPoints.StartHandle] += newPosition - points[(int)BezierPoints.StartPoint];

                    if (previousLine != null)
                    {
                        previousLine.SetPoint(BezierPoints.EndPoint, points[(int)BezierPoints.StartPoint]);
                        previousLine.SetPoint(BezierPoints.EndHandle, newPosition - (points[(int)BezierPoints.StartHandle] - newPosition));
                    }

                    points[(int)BezierPoints.StartPoint] = newPosition;
                    break;
                case BezierPoints.EndPoint: //endPoint
                    points[(int)BezierPoints.EndHandle] += newPosition - points[(int)BezierPoints.EndPoint];

                    if (nextLine != null)
                    {
                        nextLine.SetPoint(BezierPoints.StartPoint, points[(int)BezierPoints.EndPoint]);
                        nextLine.SetPoint(BezierPoints.StartHandle, newPosition - (points[(int)BezierPoints.EndHandle] - newPosition));
                    }

                    points[(int)BezierPoints.EndPoint] = newPosition;
                    break;
                case BezierPoints.StartHandle: //endPoint
                    points[(int)BezierPoints.StartHandle] = newPosition;
                    if (previousLine != null)
                    {
                        previousLine.SetPoint(BezierPoints.EndHandle, points[(int)BezierPoints.StartPoint] - (newPosition - points[(int)BezierPoints.StartPoint]));
                    }
                    break;
                case BezierPoints.EndHandle: //endPoint
                    points[(int)BezierPoints.EndHandle] = newPosition;
                    if (nextLine != null)
                    {
                        nextLine.SetPoint(BezierPoints.StartHandle, points[(int)BezierPoints.EndPoint] - (newPosition - points[(int)BezierPoints.EndPoint]));
                    }
                    break;
            }
        }
    }

    public Vector3 GetPoint(BezierPoints index)
    {
        return points[(int)index];
    }

    public void SetPoint(BezierPoints index, Vector3 newPos)
    {
        points[(int)index] = newPos;
    }
}
