using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_only_text : MonoBehaviour
{

    //•`‰æŒn
    [SerializeField]
    float maxSize;

    [SerializeField]
    float minSize;

    public GameObject me;

    private int maxTimer = 30;
    private int nowTimer = 0;
    Easing ease;
    private bool inStart = false;
    private bool inEnd = false;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        ease = new Easing();
        rectTransform.localScale = new Vector2(maxSize, minSize);
        me.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!inStart)
        {
            InAnimation();
        }

        if (inEnd)
        {
            EndAnimation();
        }
    }

    void InAnimation()
    {
        if (nowTimer < maxTimer)
        {
            rectTransform.localScale = new Vector2(maxSize, ease.InOutQuad(maxSize, minSize, maxTimer, nowTimer));
            nowTimer++;
        }
        else
        {
            nowTimer = 0;
            rectTransform.localScale = new Vector2(maxSize, maxSize);
            inStart = true;
        }
    }

    void EndAnimation()
    {
        if (nowTimer < maxTimer)
        {
            rectTransform.localScale = new Vector2(maxSize, ease.InOutQuad(-maxSize, maxSize, maxTimer, nowTimer));
            nowTimer++;
        }
        else
        {
            nowTimer = 0;
            rectTransform.localScale = new Vector2(maxSize, minSize);
            inStart = true;
        }
    }
}
