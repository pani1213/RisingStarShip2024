using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Monster
{
    private float movePower;
    private Vector3 startPos,targetPos;
    private bool isMove = true;
    private int bossPartternCount=0;

    public void InIt()
    {
        gameObject.SetActive(true);
        currentHp = characterData.Max_HP;
        render.color = Color.white;
        StartCoroutine(StartMove());
    }
    IEnumerator StartMove()
    {
        movePower = 1f;
        while (transform.position.x > 6)
        {
            transform.Translate(Vector2.left * movePower * Time.deltaTime);
            yield return waitForEndOfFrame;
        }
        while (true)
        {
            if (isMove)
            {
                if (bossPartternCount <= 1)
                    StartCoroutine(DefaltMove());
                else
                {
                     if (Random.Range(0, 2) == 0)
                         StartCoroutine(TriangleMove());
                     else
                         StartCoroutine(spiralAttact());
                }
            }
            yield return waitForEndOfFrame;
        }
    }
    IEnumerator DefaltMove()
    {
        isMove = false;
        startPos = transform.position;
        targetPos = new Vector2(Random.Range(1.5f, 7.5f), Random.Range(-3f, 3f));

        movePower = 0;
        while (transform.position != targetPos)
        {
            movePower += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, targetPos, movePower);
            yield return waitForEndOfFrame;
        }
        StartCoroutine(FanPattern(10,5,10,3));
        yield return new WaitForSeconds(3);
        isMove = true;
        bossPartternCount++;
    }
    IEnumerator TriangleMove()
    {
        isMove = false;
        int count = 0;
        Vector2 pos1,pos2,pos3;
        if (Random.Range(0, 2) == 0)
        {
            pos1 = new Vector2(1.5f, -3);
            pos2 = new Vector2(7, 0);
            pos3 = new Vector2(1.5f, 3);
        }
        else
        {
            pos1 = new Vector2(1.5f, 3);
            pos2 = new Vector2(7, 0);
            pos3 = new Vector2(1.5f, -3);
        }

        startPos = transform.position;
        targetPos = pos1;
        movePower = 0;
        while (count < 4)
        {
            if (transform.position == (Vector3)pos1 && count == 0)
            {
                StartCoroutine(FanPattern(30, 5, 10, 1));
                count++;
                movePower = 0;
                startPos = pos1;
                targetPos = pos2;
            }
            else if (transform.position == (Vector3)pos2)
            {
                StartCoroutine(FanPattern(30, 5, 10, 1));
                count++;
                movePower = 0;
                startPos = pos2;
                targetPos = pos3;
            }
            else if (transform.position == (Vector3)pos3)
            {
                StartCoroutine(FanPattern(350, 30, 3, 4));
                count++;
                movePower = 0;
                startPos = pos3;
                targetPos = pos1;
            }
            else if (transform.position == (Vector3)pos1 && count == 3)
                count++;
            
            if (count != 3) movePower += Time.deltaTime;
            else movePower += Time.deltaTime * 0.3f;

            transform.position = Vector2.Lerp(startPos, targetPos, movePower);
            yield return waitForEndOfFrame;
        }

        bossPartternCount = 0;
        isMove = true;
    }
    IEnumerator spiralAttact()
    {
        isMove = false;
        startPos = transform.position;
        movePower = 0;
        while (transform.position != Vector3.zero) 
        {
            movePower += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos,Vector2.zero,movePower);
            yield return waitForEndOfFrame;
        }
        StartCoroutine(FanPattern(340, 50, 8, 3, true));
        yield return new WaitForSeconds(8f);
        bossPartternCount =0;
        isMove = true;
    }
    IEnumerator FanPattern(float _angle, int _bulletCount,float _bulletSpeed,int patternCount, bool isdelay = false)
    {
        float startAngle = -_angle / 2f;
        float angleStep = _angle / (_bulletCount - 1);
        int count =1;
        while (count <= patternCount)
        {
            count++;
            Vector2 directionToTarget = (GameManager.instance.playerController.transform.position - transform.position).normalized;
         
            for (int i = _bulletCount; i >= 0; i--)
            {
                //각도계산
                Quaternion bulletRotation = Quaternion.Euler(0f, 0f, startAngle + i * angleStep);
                Vector2 bulletDirection = Quaternion.Euler(0f, 0f, bulletRotation.eulerAngles.z) * directionToTarget;

                MonsterBullet bullet = GameManager.instance.GetMonsterBullet(transform);
                bullet.InIt(bulletDirection * _bulletSpeed, bulletRotation);
                if(isdelay) yield return waitForSeconds;
            }
            if (!isdelay) yield return new WaitForSeconds(1);
            else yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player_Bullet"))
        {
            currentHp--;
            AudioManager.instance.PlayFxClip("Hit");
            GameManager.instance.SetBossHP(characterData.Max_HP, currentHp);
            StartCoroutine(HitAction());
            if (currentHp <= 0)
            {
                GameManager.instance.GetBoomEffect(transform);
                DiedAction();
            }
        }
    }
    protected override void DiedAction()
    {
        GameManager.instance.EndingEvent();
        gameObject.SetActive(false);
        GameManager.instance.OnandOffBossState(false);
    }

 
}
