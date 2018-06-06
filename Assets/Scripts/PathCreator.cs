using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour {
    [HideInInspector]
    public Path path;

    public Color anchorColour = Color.red;
    public Color controlColor = Color.white;
    public Color segmentColor = Color.cyan;
    public Color seletedColour = Color.yellow;
    public float anchorDiam = .1f;
    public float controlDiam = .075f;
    public bool DisplayControlPoints = true;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    private void Reset()
    {
        CreatePath();
    }
}
