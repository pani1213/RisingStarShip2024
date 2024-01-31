using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
public class MonsterController : Monster
{
    public int spawnNum = 0;
    public SpawnPostion spawnPostion;
    List<Vector2> movePos;

    public Rigidbody2D myRigid;
    private WaitForSeconds waitShotDelay = new WaitForSeconds(0.5f);

    private float movePower;
    private const int screenOut_x = -10;
    private float timer = 0;
    private bool isShot = true;
    public void InIt(SpawnPostion _spawnPostion)
    {
        currentHp = characterData.Max_HP;
        timer = 0;
        isShot = true;
        spawnPostion = _spawnPostion;
        spawnPostion.isSpawn = true;
        render.color = Color.white;
        StartCoroutine(StartMove());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player_Bullet"))
        {
            currentHp--;
            AudioManager.instance.PlayFxClip("Hit");
            StartCoroutine(HitAction());
            if (currentHp <= 0)
            {
                GameManager.instance.SetScoer(dieScoer);
                GameManager.instance.GetBoomEffect(transform);
                DiedAction();
            }
        }
    }
    IEnumerator StartMove()
    {
        movePos = GetBezierVectorList();
        movePower = 0.1f;
        while (timer < 2)
        {
            movePower += 0.5f * Time.deltaTime;
            timer += Time.deltaTime;
            transform.position = GetBezierPoint(movePower, movePos);
           yield return waitForEndOfFrame;
        }
        timer = 0;
        StartCoroutine(Shot());
        yield return new WaitForSeconds(10f);
        StartCoroutine(LastMove());
    }
    IEnumerator Shot()
    {
        while (isShot)
        {
            GameManager.instance.GetMonsterBullet(transform).InIt();
            yield return waitShotDelay;
        }
    }
   IEnumerator LastMove()
   {
       isShot = false;
       movePower = 15;
       while (transform.position.x > screenOut_x)
       {
           myRigid.AddForce(Vector2.left * movePower * Time.deltaTime, ForceMode2D.Impulse);
           yield return waitForEndOfFrame;
       }
        DiedAction();
       yield return null;
   }
    private List<Vector2> GetBezierVectorList()
    {
        List<Vector2> _answer = new List<Vector2>();
        int randomNum = UnityEngine.Random.Range(-6, 6);
        _answer.Add(new Vector2(15, randomNum));
        _answer.Add(new Vector2(-2, 0));
        _answer.Add(spawnPostion.pos);
        return _answer;
    }
    Vector2 GetBezierPoint(float _value, List<Vector2> points)
    {
        if (points.Count == 1)
            return points[0];
        List<Vector2> reducedPoints = new List<Vector2>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            reducedPoints.Add(Vector3.Lerp(points[i], points[i + 1], _value));
        }
        return GetBezierPoint(_value, reducedPoints);
    }
    protected override void DiedAction()
    {
        StopAllCoroutines();
        GameManager.instance.PoolinMonster(spawnPostion);
        GameManager.instance.PoolInObject(gameObject);
        gameObject.SetActive(false);
    }
}
