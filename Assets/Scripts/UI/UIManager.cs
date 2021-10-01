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

    public int levelMax = 3;
    public int curLevel = 0;

    public List<GameObject> levelList = new List<GameObject>();

    void Start()
    {
        curLevel = 0;
        levelMax--;
        levelsPanel.DOAnchorPos(Vector2.zero, 0);

        DOTween.KillAll();
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
        levelsPanel.DOKill(true);
        if (DOTween.TotalPlayingTweens() == 0)
        {
            curLevel++;
            levelsPanel.DOAnchorPos(new Vector2(levelsPanel.localPosition.x - offset, 0), smooth);
        }    
    }

    private void slideLeft()
    {
        levelsPanel.DOKill(true);
        if(DOTween.TotalPlayingTweens() == 0)
        {
            curLevel--;
            levelsPanel.DOAnchorPos(new Vector2(levelsPanel.localPosition.x + offset, 0), smooth);
        }
    }
}
