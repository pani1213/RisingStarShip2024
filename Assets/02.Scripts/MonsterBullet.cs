using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour 
{
    public float Speed;
    public Rigidbody2D myRigid;
    private Vector3 playerDir;
    private bool bossBullet = false;
    public void InIt( )
    {
        bossBullet = false;
        Speed = 10;
        GetPlayerDirection();
        playerDir.Normalize();
    }
    public void InIt(Vector2 _dir,Quaternion _quaternion)
    {
        bossBullet = true;
        Speed = 10;
        transform.rotation = _quaternion;
        myRigid .velocity = _dir;
        GetPlayerDirection();
        playerDir.Normalize();
    }
    private void Update()
    {
        if (!bossBullet)
            transform.Translate(playerDir * Speed * Time.deltaTime);
        if (transform.position.x < -9 || transform.position.y > 7 || transform.position.y < -7 || transform.position.x > 10)
            GameManager.instance.PoolInObject(gameObject);
    }
    private void GetPlayerDirection() => playerDir = GameManager.instance.playerController.transform.position - transform.position;
}


