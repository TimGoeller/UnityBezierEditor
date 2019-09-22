using UnityEngine;
using UnityEditor;
using System;
using static BezierLine.BezierPoints;

[CustomEditor(typeof(PolylineCreator))]
public class PolylineCreatorEditor : Editor
{
    PolylineCreator polylineCreator;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Close line"))
        {
            polylineCreator.CloseLine();
        }

        if (GUILayout.Button("New line"))
        {
            polylineCreator.CreateStart();
        }
    }

    void OnSceneGUI()
    {
        if(polylineCreator.start == null)
        {
            polylineCreator.CreateStart();
        }
        Input();
        Draw();
    }

    private void Draw()
    {
        //throw new NotImplementedException();

        BezierLine current = polylineCreator.start;
        do
        {
            Handles.DrawBezier(current.GetPoint(StartPoint), current.GetPoint(EndPoint),
                current.GetPoint(StartHandle), current.GetPoint(EndHandle), Color.blue, null, 5f);

            Handles.color = Color.red;
            current.MovePoint(StartPoint, Handles.FreeMoveHandle(current.GetPoint(StartPoint), Quaternion.identity, .1f, Vector2.zero, Handles.CubeHandleCap));

            if (!polylineCreator.closed && current.nextLine == null)
            {
                current.MovePoint(EndPoint, Handles.FreeMoveHandle(current.GetPoint(EndPoint), Quaternion.identity, .1f, Vector2.zero, Handles.CubeHandleCap));
            }

            Handles.color = Color.green;
            current.MovePoint(StartHandle, Handles.FreeMoveHandle(current.GetPoint(StartHandle), Quaternion.identity, .1f, Vector2.zero, Handles.CubeHandleCap));
            current.MovePoint(EndHandle, Handles.FreeMoveHandle(current.GetPoint(EndHandle), Quaternion.identity, .1f, Vector2.zero, Handles.CubeHandleCap));
            Handles.color = Color.white;
            Handles.DrawDottedLine(current.GetPoint(StartPoint), current.GetPoint(StartHandle), .5f);
            Handles.DrawDottedLine(current.GetPoint(EndPoint), current.GetPoint(EndHandle), .5f);

            current = current.nextLine;
        } while (current != polylineCreator.start && current != null);
    }

    static Plane XZPlane = new Plane(Vector3.up, Vector3.zero);

    private void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            // Undo.RecordObject(creator, "Add segment");
            float distance;
            Ray ray = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            if (XZPlane.Raycast(ray, out distance))
            {
                Vector3 hitPoint = ray.GetPoint(distance);
                //Just double check to ensure the y position is exactly zero
                hitPoint.y = 0;
                polylineCreator.AddLine(hitPoint);
            }
        }

        
    }

    void OnEnable()
    {
        polylineCreator = target as PolylineCreator;
    }
}