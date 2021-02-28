using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RectListWrapper
{
    public List<Rect> viewportRects = new List<Rect>();
}

public class SplitScreenManager : MonoBehaviour
{
    public List<RectListWrapper> wrappedRects = new List<RectListWrapper>();
}
