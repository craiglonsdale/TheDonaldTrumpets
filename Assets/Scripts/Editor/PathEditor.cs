using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor {

    PathCreator pathCreator;
    Path path;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create New"))
        {
            Undo.RecordObject(pathCreator, "Create New");
            pathCreator.CreatePath();
            path = pathCreator.path;
        }
        if (GUILayout.Button("Toggle Closed"))
        {
            Undo.RecordObject(pathCreator, "Toggle Closed");
            path.ToggleClosed();
        }
        bool autoSetControlPoints = GUILayout.Toggle(path.AutoSetControlPoints, "Auto Set Control Points");
        if (autoSetControlPoints != path.AutoSetControlPoints)
        {
            Undo.RecordObject(pathCreator, "Toggle Auto Set Controls");
            path.AutoSetControlPoints = autoSetControlPoints;
        }

        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }
    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Draw()
    {
        for (int i = 0; i < path.NumSegments; i++)
        {
            var points = path.GetPointsInSegment(i);
            Handles.color = Color.black;
            Handles.DrawLine(points[1], points[0]);
            Handles.DrawLine(points[2], points[3]);
            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.cyan, null, 2);
        }
        Handles.color = Color.red;
        for (int i = 0; i < path.NumPoints; i++)
        {
            Vector2 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, 0.1f, Vector2.zero, Handles.CylinderHandleCap);
            if (path[i] != newPos)
            {
                Undo.RecordObject(pathCreator, "Move Point");
                path.MovePoint(i, newPos);
            }
        }
    }

    void Input()
    {
        var guiEvent = Event.current;
        var mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            Undo.RecordObject(pathCreator, "Add Segment");
            path.AddSegment(mousePos);
        }
    }

    void OnEnable()
    {
        pathCreator = (PathCreator)target;
        if (pathCreator.path == null)
        {
            pathCreator.CreatePath();
        }
        path = pathCreator.path;
    }
}
