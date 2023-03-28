using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public GameObject viewFinderPrefabs;
    int m_bullet;
    Camera m_cam;
    GameObject m_viewFinderClone;

    public int Bullet { get => m_bullet; set => m_bullet = value; }
    public override void Awake()
    {
        MakeSingleton(false);
    }
    public override void Start()
    {
        if (!GameManager.Ins) return;
        m_cam = GameManager.Ins.m_cam;
        if (GameManager.Ins.is_OnMobile || !viewFinderPrefabs) return;
        m_viewFinderClone = Instantiate(viewFinderPrefabs, new Vector3(1000, 1000, 0), Quaternion.identity);
    }
    private void Update()
    {
        if (!m_cam) return;
        Vector3 mousePos = m_cam.ScreenToWorldPoint(Input.mousePosition);
        m_viewFinderClone.transform.position = new Vector3(mousePos.x,mousePos.y,0f);
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(mousePos);
        }
    }
    public void Shoot(Vector3 mousePos)
    {
        if(m_bullet<=0) return;
        m_bullet--;
        Vector3 shootingDir = m_cam.transform.position - mousePos;

        shootingDir.Normalize();
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, shootingDir,0.1f);
        if (hits == null || hits.Length <= 0) return;
        for (int i = 0; i < hits.Length; i++)
        {
            var hited = hits[i];
            if (!hited.collider) continue;
           // float distBetweenMouseHited = Vector2.Distance((Vector2)hited.collider.transform.position, (Vector2)mousePos);
          //  if(distBetweenMouseHited <= 0.4f)
           // {
                var enemy = hited.collider.GetComponent<Enemy>();
                if (enemy)
                {
                    enemy.Dead();
                }
            // }
            if (GUI.Ins)
            {
                GUI.Ins.UpdateBuleetText(m_bullet);
            }
            if (AudioController.Ins)
            {
                AudioController.Ins.PlaySound(AudioController.Ins.shootingSound);
            }
            if (CinemachineController.Ins)
            {
                CinemachineController.Ins.ShakeTrigger();
            }
        }
    }
}
