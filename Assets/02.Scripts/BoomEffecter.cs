using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomEffecter : MonoBehaviour
{
    private WaitForSeconds oneSecond = new WaitForSeconds(0.6f);
    private void OnEnable()
    {
        AudioManager.instance.PlayFxClip("Boom");
        StartCoroutine(OnDisAbleCoroutine());
    }
    IEnumerator OnDisAbleCoroutine()
    {
        yield return oneSecond;
        GameManager.instance.PoolInObject(gameObject);
        StopCoroutine(OnDisAbleCoroutine());
    }


}
