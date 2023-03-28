using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameoverDialog : Dialog
{
    public Text totalScore;
    public Text bestScore;
    public override void Show(bool isShow)
    {
        base.Show(isShow);
        if (totalScore)
            totalScore.text = GameManager.Ins.Score.ToString();
        if (bestScore)
            bestScore.text = Prefs.bestScore.ToString();
        if (AudioController.Ins)
        {
            AudioController.Ins.PlaySound(AudioController.Ins.loseSound);

        }
    }
    public void BackToMenu()
    {
        if (SceneController.Ins)
            SceneController.Ins.LoadCurrentScene();
    }
    public void Replay()
    {
        Close();
        if (GameManager.Ins)
            GameManager.Ins.StartGame();
    }
}
