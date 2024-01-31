using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollContoller : MonoBehaviour
{
    public List<Text> texts;
    public List<string> strList;
    public float scrollSpeed = 0;

    private bool isEnd = false, isStop = false, isGoMain = false;
    private int topTextIndex = 0, strIndex = 0 , textEndIndex = 0;
    
    private void Start()
    {
        GameManager.instance.isAllStop = true;
        for (int i = 0; i < texts.Count; i++) 
        {
            texts[i].text = strList[strIndex];
            strIndex++;
        }
    }
    private void Update()
    {
        if (isGoMain && Input.anyKey)
            SceneManager.LoadScene(0);
        
        if (!isStop)
            for (int i = 0; i < texts.Count; i++) texts[i].transform.Translate(Vector2.up * scrollSpeed * Time.deltaTime);

        if (isEnd)
        {
            if (texts[textEndIndex].transform.position.y >= 0)
            { 
                isStop = true;
                isGoMain = true;
            }
        }

        if (texts[topTextIndex].transform.localPosition.y >= 175)
        {
            if (strList.Count > strIndex) texts[topTextIndex].text = strList[strIndex];
            else texts[topTextIndex].text = "";

            texts[topTextIndex].transform.localPosition = new Vector2(0, -175);

            strIndex++;
            if (strIndex == strList.Count)
            {
                texts[topTextIndex].fontSize = 50;
                textEndIndex = topTextIndex;
                isEnd = true;
            }
            if (topTextIndex <= texts.Count - 2) topTextIndex++;
            else topTextIndex = 0;
        }
    }
}
