using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path {
    [SerializeField, HideInInspector]
    List<Vector2> points;
    [SerializeField, HideInInspector]
    bool isClosed;
    [SerializeField, HideInInspector]
    bool autoSetControlPoints;

    public Path(Vector2 centre)
    {
        points = new List<Vector2>
        {
            centre + Vector2.left,
            centre + (Vector2.left + Vector2.up) * 0.5f,
            centre + (Vector2.right + Vector2.down) * 0.5f,
            centre + Vector2.right
        };
    }

    public void AddSegment(Vector2 anchorPosition)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPosition) * 0.5f);
        points.Add(anchorPosition);

        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(points.Count - 1);
        }
    }

    public void SplitSegment(Vector2 anchorPos, int segmentIndex)
    {
        points.InsertRange(segmentIndex * 3 + 2, new Vector2[] { Vector2.zero, anchorPos, Vector2.zero });
        if (autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(segmentIndex * 3 + 3);
        }
        else
        {
            AutoSetAnchorControlPoints(segmentIndex * 3 + 3);
        }
    }

    public void DeleteSegment(int anchorIndex)
    {
        if (NumSegments > 2 || !isClosed && NumSegments > 1)
        {
            if (anchorIndex == 0)
            {
                if (isClosed)
                {
                    points[points.Count - 1] = points[2];
                }
                points.RemoveRange(0, 3);
            }
            else if (anchorIndex == points.Count - 1 && !isClosed)
            {
                points.RemoveRange(anchorIndex - 2, 3);
            }
            else
            {
                points.RemoveRange(anchorIndex - 1, 3);
            }
        }
    }

    public bool AutoSetControlPoints
    {
        get { return autoSetControlPoints; }
        set
        {
            if (autoSetControlPoints != value)
            {
                autoSetControlPoints = value;
                if (autoSetControlPoints)
                {
                    AutoSetAllControlPoints();
                }
            }
        }
    }
    public int NumSegments
    {
        get
        {
            return points.Count / 3;
        }
    }

    public int NumPoints
    {
        get { return points.Count;  }
    }

    public bool IsClosed
    {
        get { return isClosed; }
        set
        {
            if (isClosed != value)
            {
                isClosed = value;

                if (isClosed)
                {
                    points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                    points.Add(points[0] * 2 - points[1]);
                    if (autoSetControlPoints)
                    {
                        AutoSetAnchorControlPoints(0);
                        AutoSetAnchorControlPoints(points.Count - 3);
                    }
                }
                else
                {
                    points.RemoveRange(points.Count - 2, 2);
                    if (autoSetControlPoints)
                    {
                        AutoSetStartAndEndControls();
                    }
                }
            }
        }
    }
    public Vector2 this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public Vector2[] GetPointsInSegment(int segmentIndex)
    {
        return new Vector2[]
        {
            points[segmentIndex * 3],
            points[segmentIndex * 3 + 1],
            points[segmentIndex * 3 + 2],
            points[LoopIndex(segmentIndex * 3 + 3)]
        };
    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];
        
        // If moving Anchor
        if (i % 3 == 0 || !autoSetControlPoints)
        {
            points[i] = pos;
            if (autoSetControlPoints)
            {
                AutoSetAllAffectedControlPoints(i);
            }
            else
            {
                if (i % 3 == 0)
                {
                    if (i + 1 < points.Count || isClosed)
                    {
                        points[LoopIndex(i + 1)] += deltaMove;
                    }

                    if (i - 1 >= 0 || isClosed)
                    {
                        points[LoopIndex(i - 1)] += deltaMove;
                    }
                }
                // If moving non anchor point
                else
                {
                    bool nextPointIsAnchor = (i + 1) % 3 == 0;
                    int correspondingControlIndex = nextPointIsAnchor ? i + 2 : i - 2;
                    int anchorIndex = nextPointIsAnchor ? i + 1 : i - 1;

                    if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                    {
                        float distance = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                        Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                        points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * distance;
                    }
                }
            }
        }
    }

    public Vector2[] CalculateSpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector2> evenlySpacedPoints = new List<Vector2>();
        evenlySpacedPoints.Add(points[0]);
        Vector2 prevPoint = points[0];
        float distTravelled = 0;

        for (int segIndex = 0; segIndex < NumSegments; segIndex++)
        {
            Vector2[] p = GetPointsInSegment(segIndex);
            float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
            float estimatedCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength * .5f;
            int divisions = Mathf.CeilToInt(estimatedCurveLength * resolution * 10);
            float t = 0;
            while (t <= 1)
            {
                t += 1f/divisions;
                Vector2 pointOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                distTravelled += Vector2.Distance(prevPoint, pointOnCurve);

                while (distTravelled >= spacing)
                {
                    float overshoot = distTravelled - spacing;
                    Vector2 newEvenlySpacedPoint = pointOnCurve + (prevPoint - pointOnCurve).normalized * overshoot;
                    evenlySpacedPoints.Add(newEvenlySpacedPoint);
                    distTravelled = overshoot;
                    prevPoint = newEvenlySpacedPoint;
                }
                prevPoint = pointOnCurve;
            }
        }

        return evenlySpacedPoints.ToArray();
    }

    void AutoSetAnchorControlPoints(int anchorIndex)
    {
        Vector2 anchorPos = points[anchorIndex];
        Vector2 dir = Vector2.zero;
        float[] neighbourDistances = new float[2];

        if (anchorIndex - 3 >= 0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex - 3)] - anchorPos;
            dir += offset.normalized;
            neighbourDistances[0] = offset.magnitude;
        }
        if (anchorIndex + 3 >= 0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex + 3)] - anchorPos;
            dir -= offset.normalized;
            neighbourDistances[1] = -offset.magnitude;
        }

        dir.Normalize();

        for (int i = 0; i < 2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if (controlIndex >= 0 && controlIndex < points.Count || isClosed)
            {
                points[LoopIndex(controlIndex)] = anchorPos + dir * neighbourDistances[i] * .5f;
            }
        }
    }

    void AutoSetStartAndEndControls()
    {
        if (!isClosed)
        {
            points[1] = (points[0] + points[2]) * .5f;
            points[points.Count - 2] = (points[points.Count - 1]) + points[points.Count - 3] * .5f;
        }
    }

    void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex - 3; i <= updatedAnchorIndex + 3 ; i+=3)
        {
            if (i >= 0 && i < points.Count || isClosed)
            {
                AutoSetAnchorControlPoints(LoopIndex(i));
            }
        }
        AutoSetStartAndEndControls();
    }

    void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i+=3)
        {
            AutoSetAnchorControlPoints(i);
        }
        AutoSetStartAndEndControls();
    }

    int LoopIndex(int i)
    {
        return (i + points.Count) % points.Count;
    }
}
