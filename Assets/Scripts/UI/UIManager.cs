using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    public RectTransform levelsPanel;

    public int offset = 500;
    public float smooth = 0.25f;

    private int levelMax = 3;
    public int curLevel = 0;

    public List<GameObject> levelList = new List<GameObject>();

    void Start()
    {
        curLevel = 0;
        levelsPanel.DOAnchorPos(Vector2.zero, 0);
    }

    public void nextLevelButton()
    {
        if (!(curLevel >= levelMax))
        {
            slideRight();
        }
        Debug.Log(curLevel);
    }

    public void prevLevelButton()
    {
        if(curLevel == 0)
        {
            levelsPanel.DOAnchorPos(Vector2.zero, smooth);
        }
        else if(!(curLevel < 0))
        {
            slideLeft();
        }
        Debug.Log(curLevel);
    }

    private void slideRight()
    {
        curLevel++;
        levelsPanel.DOAnchorPos(new Vector2(levelsPanel.localPosition.x - offset, 0), smooth);
    }

    private void slideLeft()
    {
        curLevel--;
        levelsPanel.DOAnchorPos(new Vector2(levelsPanel.localPosition.x + offset, 0), smooth);
    }
}
