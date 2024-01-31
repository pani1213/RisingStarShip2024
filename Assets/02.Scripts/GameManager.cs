using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : Singleton<GameManager>
{
    // objectpooler
    public Queue<GameObject> boomEffects = new Queue<GameObject>();
    public Queue<GameObject> playerBullets = new Queue<GameObject>();
    public List<GameObject> playerBulletList = new List<GameObject>();
    public Queue<GameObject> monsters = new Queue<GameObject>();
    public Queue<GameObject> monsterBullets = new Queue<GameObject>();

    public List<SpawnPostion> spawnPositions = new List<SpawnPostion>();
    public List<SpawnPostion> useSpawnPositions = new List<SpawnPostion>();
    public BossController bossContainer;

    public GameObject titleObj, bossEffectObj, boomEffectObj, bulletObj, monsterObj, monsterBulletObj, backgroundImage, endingObj,gameOverObj,playerHPBar, playerHpObj;
    public Text scoerText, bossName;
    public PlayerController playerController;
    public Image bossHP;

    public int PlayerColor = 0, scoer;
    public float coolTime = 1;
    public bool isAllStop = false;

    private int bossScoer = 300;
    private float timer = 0;
    private bool isSpawnMonster = false, isStart = true;
    
    public void InIt()
    {
        isSpawnMonster = false;
        isStart = true;
        isAllStop = false;
        gameOverObj.SetActive(false);
        titleObj.SetActive(true);
        scoerText.gameObject.SetActive(false);
        endingObj.SetActive(false);
        playerController.InIt();
        backgroundImage.transform.localPosition = new Vector2(640,0);
        AudioManager.instance.PlayBgmClip("GameBGM");
    }
    private void Start()
    {
        Screen.SetResolution(1920, 1080, false);
        Application.targetFrameRate = 60;
        AudioManager.instance.InIt();
        InItSpawnPostionList();
        InIt();
    }
    public void Update()
    {
        StartGame();
        if (scoer >= bossScoer)
            StartCoroutine(OnBossEffect());

        if (isSpawnMonster)
            SpawnMonster();
        if (backgroundImage.transform.localPosition.x > -640)
            backgroundImage.transform.Translate(Vector2.left * 0.1f * Time.deltaTime);
    }
    public void SpawnMonster()
    {
        timer += Time.deltaTime;
        if (timer > coolTime)
        {
            if (spawnPositions.Count <= 0)
            {
                Debug.Log("spawn X");
                return;
            }
            int randomNum = Random.Range(0, spawnPositions.Count);
            useSpawnPositions.Add(spawnPositions[randomNum]);
            GetMonster(transform).InIt(spawnPositions[randomNum]);
            spawnPositions.RemoveAt(randomNum);
            timer = 0;
        }
    }
    public void GetBullet(Transform _transform)
    {
        GameObject _obj;
        if (GetActiveFalseObjectInList(playerBulletList) == null)
        {
            _obj = Instantiate(bulletObj);
            _obj.name = bulletObj.name;
        }
        else
            _obj = GetActiveFalseObjectInList(playerBulletList);

        _obj.transform.localScale = Vector3.one;
        _obj.transform.localPosition = _transform.position;
        _obj.SetActive(true);
    }

    public void GetBoomEffect(Transform _transform)
    {
        GameObject _obj;
        if (boomEffects.Count <= 0)
        {
            _obj = Instantiate(boomEffectObj);
            _obj.name = boomEffectObj.name;
        }
        else
            _obj = boomEffects.Dequeue();
        //_obj.transform.localScale = Vector3.one;
        _obj.transform.localPosition = _transform.position;
        _obj.SetActive(true);
    }
    public MonsterBullet GetMonsterBullet(Transform _transform)
    {
        GameObject _obj;
        if (monsterBullets.Count <= 0)
        {
            _obj = Instantiate(monsterBulletObj);
            _obj.name = monsterBulletObj.name;
        }
        else
            _obj = monsterBullets.Dequeue();

        _obj.transform.localScale = Vector3.one;
        _obj.transform.localPosition = _transform.position;
        _obj.SetActive(true);
        return _obj.GetComponent<MonsterBullet>();

    }
    public MonsterController GetMonster(Transform _transform)
    {
        GameObject _obj;
        if (monsters.Count <= 0)
        {
            _obj = Instantiate(monsterObj);
            _obj.name = monsterObj.name;
        }
        else
            _obj = monsters.Dequeue().gameObject;
        _obj.transform.localScale = Vector3.one;
        _obj.SetActive(true);
        return _obj.GetComponent<MonsterController>();
    }
    public void PoolInObject(GameObject _obj)
    {
        if (_obj.name == boomEffectObj.name)
            boomEffects.Enqueue(_obj);
        else if (_obj.name == bulletObj.name)
            playerBulletList.Add(_obj);
        else if (_obj.name == monsterObj.name)
            monsters.Enqueue(_obj);
        else if (_obj.name == monsterBulletObj.name)
            monsterBullets.Enqueue(_obj);
        _obj.transform.parent = transform;
        _obj.SetActive(false);
    }
    public GameObject GetActiveFalseObjectInList(List<GameObject> _list)
    {
        for (int i = 0; i < _list.Count; i++) if (!_list[i].activeSelf) return _list[i];
        return null;
    }
    public void StartGame()
    {
        if (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1) && isStart)
        {
            isSpawnMonster = true;
            titleObj.SetActive(false);
            scoerText.gameObject.SetActive(true);
            playerHPBar.gameObject.SetActive(true);
            isStart = false;
        }
    }
    public void SetColorNumBtn(int _num)
    {
        PlayerColor = _num;
        playerController.ChangePlayerSprite(PlayerColor);
    }
    IEnumerator OnBossEffect()
    {
        bossScoer = 1500;
        isSpawnMonster = false;
        AudioManager.instance.PlayBgmClip("WarningBGM");
        bossEffectObj.SetActive(true);
        yield return new WaitForSeconds(7);
        AudioManager.instance.PlayBgmClip("BossBGM");
        OnandOffBossState(true);
        bossName.text = bossContainer.characterData.Character_name;
        bossContainer.InIt();
        bossEffectObj.SetActive(false);
    }
    private void InItSpawnPostionList()
    {
        float x = 3.5f, y = 3f;
        for (int i = 0; i < 20; i++)
        {
            spawnPositions.Add(new SpawnPostion(false, new Vector2(x, y)));
            if (x < 8) x += 1.5f;
            else
            {
                x = 3.5f;
                y -= 1.5f;
            }
        }
    }
    public void PoolinMonster(SpawnPostion _pos)
    {
        useSpawnPositions.Remove(_pos);
        _pos.isSpawn = false;
        spawnPositions.Add(_pos);
    }
    public void SetScoer(int _count)
    {
        scoer += _count;
        scoerText.text = $"{scoer:D8}";
    }
    public void SetBossHP(float _Maxvalue, float _value)
    {
        bossHP.fillAmount = _value / _Maxvalue;
    }
    public void OnandOffBossState(bool _onOff)
    {
        bossName.gameObject.SetActive(_onOff);
        bossHP.gameObject.SetActive(_onOff);
    }
    public void EndingEvent()
    {
        StartCoroutine(ending());
    }
    IEnumerator ending()
    {
        AudioManager.instance.StopBgmClip();
        yield return new WaitForSeconds(1);
        endingObj.SetActive(true);
        AudioManager.instance.PlayBgmClip("EndingBGM");
    }
}
public class SpawnPostion
{
    public SpawnPostion(bool _isSpawn , Vector2 _pos)
    {
        isSpawn = _isSpawn;
        pos = _pos;
    }
    public bool isSpawn = false;
    public Vector2 pos;
}
