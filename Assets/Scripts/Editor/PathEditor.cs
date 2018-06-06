using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor {

    PathCreator pathCreator;
    Path Path
    {
        get { return pathCreator.path; }
    }

    const float segmentSelectDistanceThreshold = .1f;
    int selectedSegmentIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create New"))
        {
            Undo.RecordObject(pathCreator, "Create New");
            pathCreator.CreatePath();
        }

        bool isClosed = GUILayout.Toggle(Path.IsClosed, "Closed"); ;
        if (isClosed != Path.IsClosed)
        {
            Undo.RecordObject(pathCreator, "Toggle Closed");
            Path.IsClosed = isClosed;
        }

        bool autoSetControlPoints = GUILayout.Toggle(Path.AutoSetControlPoints, "Auto Set Control Points");
        if (autoSetControlPoints != Path.AutoSetControlPoints)
        {
            Undo.RecordObject(pathCreator, "Toggle Auto Set Controls");
            Path.AutoSetControlPoints = autoSetControlPoints;
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
        for (int i = 0; i < Path.NumSegments; i++)
        {
            var points = Path.GetPointsInSegment(i);
            if (i % 3 == 0 || pathCreator.DisplayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
            }
            Color segmentColor = (i == selectedSegmentIndex && Event.current.shift) ? pathCreator.seletedColour : pathCreator.segmentColor;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentColor, null, 2);
        }
        
        for (int i = 0; i < Path.NumPoints; i++)
        {
            if (i % 3 == 0 || pathCreator.DisplayControlPoints)
            {
                Handles.color = i % 3 == 0 ? pathCreator.anchorColour : pathCreator.controlColor;
                float handleSize = i % 3 == 0 ? pathCreator.anchorDiam : pathCreator.controlDiam;
                Vector2 newPos = Handles.FreeMoveHandle(Path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
                if (Path[i] != newPos)
                {
                    Undo.RecordObject(pathCreator, "Move Point");
                    Path.MovePoint(i, newPos);
                }
            }
           
        }
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if (selectedSegmentIndex != -1)
            {
                Undo.RecordObject(pathCreator, "Split segment");
                Path.SplitSegment(mousePos, selectedSegmentIndex);
            }
            else if (!Path.IsClosed)
            {
                Undo.RecordObject(pathCreator, "Add segment");
                Path.AddSegment(mousePos);
            }
        }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDstToAnchor = pathCreator.anchorDiam * .5f;
            int closestAnchorIndex = -1;

            for (int i = 0; i < Path.NumPoints; i += 3)
            {
                float dst = Vector2.Distance(mousePos, Path[i]);
                if (dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }
            }

            if (closestAnchorIndex != -1)
            {
                Undo.RecordObject(pathCreator, "Delete segment");
                Path.DeleteSegment(closestAnchorIndex);
            }
        }

        if (guiEvent.type == EventType.MouseMove)
        {
            float minDstToSegment = segmentSelectDistanceThreshold;
            int newSelectedSegmentIndex = -1;

            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector2[] points = Path.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }
            }

            if (newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }
        }
    }

    void OnEnable()
    {
        pathCreator = (PathCreator)target;
        if (pathCreator.path == null)
        {
            pathCreator.CreatePath();
        }
    }
}
