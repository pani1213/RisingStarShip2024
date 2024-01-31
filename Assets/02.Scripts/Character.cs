using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Character : MonoBehaviour
{
    public CharacterData characterData;
    public int currentHp = 0;
}
public class Monster : Character
{
    protected WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);
    protected WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    public SpriteRenderer render;
    public int dieScoer = 0;

    protected IEnumerator HitAction()
    {
        render.color = Color.red;
        yield return waitForSeconds;
        render.color = Color.white;
    }
    protected virtual void DiedAction() { }
}