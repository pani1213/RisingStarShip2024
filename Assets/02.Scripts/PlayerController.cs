using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : Character
{
    public Sprite[] starshipAni;
    public SpriteRenderer playerSprite;
    public GameObject hitBox;
    public float speed;
    public Rigidbody2D rigidBody2D;
    public BoxCollider2D playerCollider;

    const float moveSpaceX = 8.5f;
    const float moveSpaceY = 4.8f;
    private Vector2  dir;

    WaitForSeconds bulletDelay = new WaitForSeconds(0.1f);
    WaitForSeconds waitForhitAction = new WaitForSeconds(0.005f);
    private bool isShotReady = true, isMighty = false;
    private int inputCount = 0;
    float timer = 0;
    
    public void InIt()
    {
       
        playerSprite.color = Color.white;
        gameObject.SetActive(true);
        currentHp = characterData.Max_HP;
        gameObject.transform.position = new Vector2(-7.5f, 0);
        SetPlayerHp();
    }

    void Update()
    {
        dir = new Vector2 (Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        dir = dir * speed;
        MoveSpace();
        StarshipAni();
        if (Input.GetKeyDown(KeyCode.LeftControl) && inputCount <= 0 && !GameManager.instance.isAllStop)
            inputCount++;
        if (inputCount > 0 && isShotReady)
        {
            StartCoroutine(ShotDelay());
            inputCount--;
        }
    }
    IEnumerator ShotDelay()
    {
        isShotReady = false;
        AudioManager.instance.PlayFxClip("PlayerLaser");
        GameManager.instance.GetBullet(transform);
        yield return bulletDelay;
        AudioManager.instance.PlayFxClip("PlayerLaser");
        GameManager.instance.GetBullet(transform);
        yield return bulletDelay;
        AudioManager.instance.PlayFxClip("PlayerLaser");
        GameManager.instance.GetBullet(transform);
        yield return bulletDelay;
        isShotReady = true;
        StopCoroutine(ShotDelay());
    }
    private void FixedUpdate()
    {
        rigidBody2D.velocity = dir * Time.deltaTime;
    }
    public void StarshipAni()
    {
        if (0 > Input.GetAxisRaw("Vertical")) ChangePlayerSprite(1 + GameManager.instance.PlayerColor);
        else if (0 < Input.GetAxisRaw("Vertical")) ChangePlayerSprite(2 + GameManager.instance.PlayerColor);
        else ChangePlayerSprite(0 + GameManager.instance.PlayerColor);
    }
    public void ChangePlayerSprite(int _num)
    {
        playerSprite.sprite = starshipAni[_num];
    }
    private void MoveSpace()
    {
        if (GameManager.instance.isAllStop)
            return;
        if (transform.position.x > moveSpaceX)
            transform.position = new Vector2(moveSpaceX, transform.position.y);
        else if (transform.position.x < -moveSpaceX)
            transform.position = new Vector2(-moveSpaceX, transform.position.y);
        if (transform.position.y > moveSpaceY)
            transform.position = new Vector2(transform.position.x, moveSpaceY);
        else if (transform.position.y < -moveSpaceY)
            transform.position = new Vector2(transform.position.x, -moveSpaceY);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            if (!isMighty && !GameManager.instance.isAllStop)
            {
                currentHp--;
                DeleteHp();
                StartCoroutine(OnOffMighty());
                if (currentHp <= 0)
                    StartCoroutine(DiedAction());
            }
        }
    }
    private void DeleteHp() => Destroy(GameManager.instance.playerHPBar.transform.GetChild(0).gameObject);
    
    private void SetPlayerHp()
    {
        GameObject obj;
        for (int i = 0; i < characterData.Max_HP; i++)
        {
            obj = Instantiate(GameManager.instance.playerHpObj, GameManager.instance.playerHPBar.transform);
            obj.SetActive(true);
            obj.transform.localScale = Vector3.one;
        }

    }
    IEnumerator DiedAction()
    {
        GameManager.instance.GetBoomEffect(transform);
        GameManager.instance.gameOverObj.SetActive(true);
        GameManager.instance.isAllStop = true;

        playerSprite.gameObject.SetActive(false);
        hitBox.SetActive(false);
        while (true) 
        {
            if (Input.anyKey) SceneManager.LoadScene(0);
            yield return waitForhitAction;
        }
    }
    IEnumerator OnOffMighty()
    {
        isMighty = true;
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(1);
        playerSprite.color = Color.white;
        isMighty = false;
    }

}
