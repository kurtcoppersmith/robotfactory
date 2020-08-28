using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Cinemachine.Utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class Sides
{
    public float left;
    public float right;
    public float up;
    public float down;
    public float forward;
    public float back;
}

[Serializable]
public class BoundSides
{
    public Vector3 left;
    public Vector3 right;
    public Vector3 up;
    public Vector3 down;
    public Vector3 forward;
    public Vector3 back;
}

public static class HelperExtensions
{
    #region MonoBehaviour

    public static void WaitAndExecute(this MonoBehaviour self, Action action, float delay)
    {
        self.StartCoroutine(HelperUtilities.WaitAndExecute(action, delay));
    }

    public static void WaitForFrameAndExecute(this MonoBehaviour self, Action action)
    {
        self.StartCoroutine(HelperUtilities.WaitForFrameAndExecute(action));
    }

    #endregion

    #region GameObject

    public static Sequence DOFade(this GameObject obj, float endValue, float duration)
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.SetUpdate(true);

        List<Graphic> graphics = new List<Graphic>(obj.GetComponentsInChildren<Graphic>(true));
        foreach (var graphic in graphics)
        {
            float remappedEndValue = endValue;
            var graphicFadeHelper = graphic.GetComponent<GraphicFadeHelper>();
            if (graphicFadeHelper && graphicFadeHelper.enabled)
            {
                graphicFadeHelper.RefreshIfNeeded();
                remappedEndValue = HelperUtilities.Remap(endValue, 0, 1, graphicFadeHelper.minOpacity,
                    graphicFadeHelper.maxOpacity);
            }

            var graphicTween = graphic.DOFade(remappedEndValue, duration);
            mySequence.Insert(0, graphicTween);
            mySequence.PrependCallback(() => graphic.DOKill(true));
        }

        mySequence.SetTarget(obj);

        return mySequence;
    }

    public static List<Selectable> GetInteractiveSelectables(this GameObject obj)
    {
        return obj.GetComponentsInChildren<Selectable>(true).Where((selectable, i) => selectable.interactable).ToList();
    }

    public static void UpdateInteractables(this GameObject obj, bool interactable)
    {
        var selectables = obj.GetComponentsInChildren<Selectable>(true);
        foreach (var selectable in selectables)
        {
            selectable.interactable = interactable;
        }
    }

    #endregion

    #region Transform

    public static Bounds TransformBounds(this Transform self, Bounds bounds)
    {
        var center = self.TransformPoint(bounds.center);
        var points = bounds.GetCorners();

        var result = new Bounds(center, Vector3.zero);
        foreach (var point in points)
            result.Encapsulate(self.TransformPoint(point));
        return result;
    }

    public static Bounds InverseTransformBounds(this Transform self, Bounds bounds)
    {
        var center = self.InverseTransformPoint(bounds.center);
        var points = bounds.GetCorners();

        var result = new Bounds(center, Vector3.zero);
        foreach (var point in points)
            result.Encapsulate(self.InverseTransformPoint(point));
        return result;
    }

    public static Bounds GetBoundsFromRenderers(this Transform self)
    {
        List<Renderer> renderers = new List<Renderer>();

        var selfRenderer = self.GetComponent<Renderer>();
        if (selfRenderer != null)
        {
            renderers.Add(selfRenderer);
        }

        renderers.AddRange(self.GetComponentsInChildren<Renderer>());

        Bounds bounds = new Bounds();
        if (renderers.Count > 0)
        {
            bounds = renderers[0].bounds;
            foreach (var r in renderers) bounds.Encapsulate(r.bounds);
        }

        return bounds;
    }

    public static void DestroyAllChildren(this Transform self, bool immediate = false)
    {
        while (self.childCount > 0)
        {
            for (int i = 0; i < self.childCount; i++)
            {
                var child = self.GetChild(i);

                if (immediate)
                {
                    GameObject.DestroyImmediate(child.gameObject);
                }
                else
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }

    #endregion

    #region float

    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return HelperUtilities.Remap(value, from1, to1, from2, to2);
    }

    #endregion

    #region Dictionary

    public static TN GetOrDefault<TK, TV, TN>(this Dictionary<TK, TV> self, TK key, TN defaultValue) where TN: TV
    {
        if (self.ContainsKey(key))
        {
            return (TN) self[key];
        }

        return defaultValue;
    }

    #endregion

    #region List

    public static T GetRandom<T>(this List<T> self)
    {
        return self[Random.Range(0, self.Count)];
    }

    #endregion

    #region Bounds

    public static BoundSides GetSides(this Bounds obj, bool includePosition = true)
    {
        return new BoundSides()
        {
            left = (includePosition ? obj.center : Vector3.zero) + (Vector3.left * obj.extents.x),
            right = (includePosition ? obj.center : Vector3.zero) + (Vector3.right * obj.extents.x),
            up = (includePosition ? obj.center : Vector3.zero) + (Vector3.up * obj.extents.y),
            down = (includePosition ? obj.center : Vector3.zero) + (Vector3.down * obj.extents.y),
            forward = (includePosition ? obj.center : Vector3.zero) + (Vector3.forward * obj.extents.z),
            back = (includePosition ? obj.center : Vector3.zero) + (Vector3.back * obj.extents.z),
        };
    }

    public static List<Vector3> GetCorners(this Bounds obj, bool includePosition = true)
    {
        var result = new List<Vector3>();
        for (int x = -1; x <= 1; x += 2)
        for (int y = -1; y <= 1; y += 2)
        for (int z = -1; z <= 1; z += 2)
            result.Add((includePosition ? obj.center : Vector3.zero) + obj.extents.Times(new Vector3(x, y, z)));
        return result;
    }

    public static float GetVolume(this Bounds obj)
    {
        return obj.size.x * obj.size.y * obj.size.z;
    }

    public static Vector3 DisplacementToFitInside(this Bounds obj, Bounds other)
    {
        if (obj.GetVolume() > other.GetVolume())
        {
            return Vector3.zero;
        }

        var objSides = obj.GetSides();
        var otherSides = other.GetSides();

        Vector3 displacement = Vector3.zero;

        if (objSides.left.x < otherSides.left.x)
        {
            displacement.x = otherSides.left.x - objSides.left.x;
        }
        else if (objSides.right.x > otherSides.right.x)
        {
            displacement.x = otherSides.right.x - objSides.right.x;
        }

        if (objSides.up.y > otherSides.up.y)
        {
            displacement.y = otherSides.up.y - objSides.up.y;
        }
        else if (objSides.down.y < otherSides.down.y)
        {
            displacement.y = otherSides.down.y - objSides.down.y;
        }

        if (objSides.forward.z > otherSides.forward.z)
        {
            displacement.z = otherSides.forward.z - objSides.forward.z;
        }
        else if (objSides.back.z < otherSides.back.z)
        {
            displacement.z = otherSides.back.z - objSides.back.z;
        }

        return displacement;
    }

    public static Vector3 GetRandomPointInBounds(this Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    #endregion

    #region Vector3

    public static Vector3 Times(this Vector3 self, Vector3 other)
    {
        return new Vector3(self.x * other.x, self.y * other.y, self.z * other.z);
    }

    public static Vector3 GetYLess(this Vector3 self)
    {
        var res = self;
        res.y = 0;
        return res;
    }

    #endregion

    #region Camera

    public class CamBounds
    {
        public Vector3 topLeft;
        public Vector3 bottomRight;
    }

    public static Bounds OrthographicBounds(this Camera camera)
    {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

    public static CamBounds GetCameraBounds(this Camera camera)
    {
        var cam = camera;

        return new CamBounds
        {
            topLeft = cam.ViewportToWorldPoint(new Vector3(0, 1)),
            bottomRight = cam.ViewportToWorldPoint(new Vector3(1, 0))
        };
    }

    #endregion

    #region CinemachinePOV

    public static void ResetRotation(this CinemachinePOV self, Quaternion targetRot)
    {
        Vector3 up = self.VcamState.ReferenceUp;
        Vector3 fwd = Vector3.forward;
        Transform parent = self.VirtualCamera.transform.parent;
        if (parent != null)
            fwd = parent.rotation * fwd;

        self.m_HorizontalAxis.Value = 0;
        self.m_HorizontalAxis.Reset();
        Vector3 targetFwd = targetRot * Vector3.forward;
        Vector3 a = fwd.ProjectOntoPlane(up);
        Vector3 b = targetFwd.ProjectOntoPlane(up);
        if (!a.AlmostZero() && !b.AlmostZero())
            self.m_HorizontalAxis.Value = Vector3.SignedAngle(a, b, up);

        self.m_VerticalAxis.Value = 0;
        self.m_VerticalAxis.Reset();
        fwd = Quaternion.AngleAxis(self.m_HorizontalAxis.Value, up) * fwd;
        Vector3 right = Vector3.Cross(up, fwd);
        if (!right.AlmostZero())
            self.m_VerticalAxis.Value = Vector3.SignedAngle(fwd, targetFwd, right);
    }

    #endregion

    #region Volume

    public static Tween DOWeight(this Volume self, float endValue, float duration)
    {
        return DOTween.To(() => self.weight, value => self.weight = value, endValue, duration).SetTarget(self);
    }

    #endregion
}