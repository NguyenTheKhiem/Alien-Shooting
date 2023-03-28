using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        //var enemy = col.GetComponent<Enemy>();
        //if (enemy)
        //{
        //    Time.timeScale = 1f;
        //}
        Destroy(col.gameObject);
    }
}
