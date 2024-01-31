using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerBullet : MonoBehaviour
{
    private float bulletSpeed =  15;
    private void Update()
    {
        transform.Translate(bulletSpeed * Time.deltaTime, 0, 0);
        if (transform.position.x > 9)
        {
            GameManager.instance.PoolInObject(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            gameObject.SetActive(false);
            GameManager.instance.PoolInObject(gameObject);
        }
    }
}
