using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class IconSizer : MonoBehaviour
{
    public RectTransform image;
    private Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
    private float smooth = 0.6f;

    void Start()
    {
        image.DOScale(scale, smooth);
    }
}
