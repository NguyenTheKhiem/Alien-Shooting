using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : Singleton<GUI>
{
    public GameObject mainGame;
    public GameObject gamePlay;
    public Text bulletTExt;
    public Text levelText;
    public Image timerFilled;
    public Dialog gameoverDialog;
    public Dialog waveCOmpletedDialog;
    public override void Awake()
    {
        MakeSingleton(false);
    }
    public override void Start()
    {
        base.Start();
        UpdateTimeBar(1, 1);
    }
    public void ShowGameplay(bool isshow)
    {
        if (mainGame)
            mainGame.SetActive(!isshow);
        if (gamePlay)
            gamePlay.SetActive(isshow);
    }
    public void UpdateBuleetText(int bullet)
    {
        if (bulletTExt)
            bulletTExt.text = "x" + bullet;
    }
    public void UpdateLevelText( int level)
    {
        if (levelText)
            levelText.text = "LEVEL" + level;
    }
    public void UpdateTimeBar(float cur ,float total,bool isReverse = false)
    {
        if (!timerFilled) return;
        if (isReverse)
        {
            timerFilled.fillAmount = 1 - (cur / total);
        }
        else
        {
            timerFilled.fillAmount = cur / total;
        }
    }
}
