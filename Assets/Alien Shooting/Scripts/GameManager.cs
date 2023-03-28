using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameState state;
    public Camera m_cam;
    public bool is_OnMobile;
    public Enemy[] enemiesPrefab;
    public int wavePerLevel;//so wave moi level
    public int enemyPerLevel;// so enemy moi level
    public int enemyUpPerLevel;//so enemy tang len moi level
    public int bulletExtra; // dan cong them
    public float timeDownPerLevel; //thoi gian giam moi level

    List<Enemy> m_enemySpawneds;
    int m_killed;
    int m_level =1;
    int m_waveCounting  = 1;
    int m_score;

    bool m_isSlowed;
    bool m_isBeginSlow;

    public int Killed { get => m_killed; set => m_killed = value; }
    public int Score { get => m_score;  }
    public bool IsSlowed { get => m_isSlowed; set => m_isSlowed = value; }

    public override void Awake()
    {
        MakeSingleton(false);
        m_enemySpawneds = new List<Enemy>();
    }
    public override void Start()
    {
        ///
        base.Start();
       state= GameState.Starting;
    }
   
    private void Update()
    {
        if (state != GameState.Playing) return;
        if(CanLow() && !m_isBeginSlow)
        {
            m_isBeginSlow = true;
            float delay = Random.Range(0.01f, 0.05f);
            Timer.Schedule(this, delay, () =>
            {
                SlowController.Ins.DoSlowmotion();
            },true);
        }
        if(Time.timeScale<1 && m_killed>= m_enemySpawneds.Count && state != GameState.WaveCompleted)
        {
            if(m_waveCounting % wavePerLevel == 0)
            {
                state = GameState.WaveCompleted;
                enemyPerLevel += enemyUpPerLevel;
                m_level++;
                if (GUI.Ins)
                {
                    GUI.Ins.UpdateLevelText(m_level);
                    GUI.Ins.waveCOmpletedDialog.Show(true);
                }
                Debug.Log("you win");
            }
            else
            {
                ResetData();
                Spawn();
                m_waveCounting++;
                state = GameState.Playing;
            }
        }
        if(m_isSlowed &&  Time.timeScale>= 0.9f &&!CanLow() && m_killed < m_enemySpawneds.Count && state!= GameState.Gameover)
        {
            state = GameState.Gameover;
            GUI.Ins.gameoverDialog.Show(true);
            

            Debug.Log("gameover");
            m_score = 0;
        }
    }
    public void NextLevel()
    {
        if(state == GameState.WaveCompleted)
        {
            Timer.Schedule(this, 1f, () =>
            {
                m_waveCounting = 1;
                ResetData();
                Spawn();
                state = GameState.Playing;
            });
        }
        if (AudioController.Ins)
        {
            AudioController.Ins.PlayBackgroundMusic();

        }
    }
    public void StartGame()
    {
        ResetData();
        Timer.Schedule(this, 1f, () =>
        {
            if (AudioController.Ins)
            {
            AudioController.Ins.PlayBackgroundMusic();

            }
            Spawn();
            state = GameState.Playing;
        });
        if (GUI.Ins)
        {
            GUI.Ins.UpdateBuleetText(Player.Ins.Bullet);
            GUI.Ins.UpdateLevelText(m_level);
            GUI.Ins.ShowGameplay(true);
        }
    }
    public void AddScore()
    {
        m_score++;
        Prefs.bestScore = m_score;
    }
    public void Spawn()
    {
        if (!SlowController.Ins || !Player.Ins) return;
        SlowController.Ins.slowdownLength = (enemyPerLevel / 2 + 1.5f) - timeDownPerLevel* m_waveCounting;
        Player.Ins.Bullet = enemyPerLevel + bulletExtra;
        if (enemiesPrefab == null || enemiesPrefab.Length <= 0) return;
        for (int i = 0; i < enemyPerLevel; i++)
        {
            int randIdx = Random.Range(0, enemiesPrefab.Length);
            var enemyPb = enemiesPrefab[randIdx];
            float spawnPosX = Random.Range(-8f, 8f);
            float spawnPosY = Random.Range(7.5f, 8f);
            Vector3 spawnPos = new Vector3(spawnPosX, spawnPosY, 0f);
            if (enemyPb)
            {
            var enemyClone = Instantiate(enemyPb, spawnPos, Quaternion.identity);
                m_enemySpawneds.Add(enemyClone);
            }
        }
        if(GUI.Ins)
            GUI.Ins.UpdateBuleetText(Player.Ins.Bullet);

    }
    public void ResetData()
    {
        m_isBeginSlow = false;
        m_isSlowed = false;
        m_killed = 0;
       // m_enemySpawneds.Clear();
        if(state == GameState.Gameover)
        {
            m_waveCounting = 1;
            m_level = 1;
        }
        state = GameState.Starting;
        if (m_enemySpawneds == null || m_enemySpawneds.Count <= 0) return;
        for (int i = 0; i < m_enemySpawneds.Count; i++)
        {
            var enemy = m_enemySpawneds[i];
            if (enemy)
            {
                Destroy(enemy.gameObject);
            }
        }
        m_enemySpawneds.Clear();
    }
    public bool CanLow()
    {
        if (m_enemySpawneds == null || m_enemySpawneds.Count <= 0) return false;
        int check = 0;
        for (int i = 0; i < m_enemySpawneds.Count; i++)
        {
            var enemy = m_enemySpawneds[i];
            if(enemy && enemy.CanSlow)
            {
                check++;
            }
        }
        if(check == m_enemySpawneds.Count)
        {
            return true;
        }
        return false;
    }
}
