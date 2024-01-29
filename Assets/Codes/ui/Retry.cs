using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Retry : MonoBehaviour
{
    public GameObject gm;

    private int coolTimer = 30;
    //•`‰æŒn
    [SerializeField]
    float maxSize;

    [SerializeField]
    float minSize;

    [SerializeField] private Sprite keyboard;
    [SerializeField] private Sprite controller;

    Image image;

    public GameObject me;

    public AudioSource seRetry;

    private int maxTimer = 30;
    private int nowTimer = 0;
    Easing ease;
    private bool inStart = false;
    private bool inEnd = false;
    private bool selectMe = false;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        ease = new Easing();
        rectTransform.localScale = new Vector2(maxSize, minSize);
        me.SetActive(false);
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current == null)
        {
            image.sprite = keyboard;
        }
        else if (Gamepad.current != null)
        {
            image.sprite = controller;
        }

        if (!inStart)
        {
            InAnimation();
        }

        if (Input.GetKeyDown(KeyCode.R) && Gamepad.current==null)
        {
            selectMe = true;
            inEnd = true;
            seRetry.Play();
        }
        else if (Gamepad.current != null)
        {
            if (Gamepad.current.bButton.wasPressedThisFrame)
            {
                selectMe = true;
                inEnd = true;
                seRetry.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && Gamepad.current==null)
        {
            inEnd = true;
        }
        else if (Gamepad.current != null)
        {
            if (Gamepad.current.aButton.wasPressedThisFrame)
            {
                inEnd = true;
            }
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
            if (!selectMe)
            {
                rectTransform.localScale = new Vector2(maxSize, ease.InOutQuad(-maxSize, maxSize, maxTimer, nowTimer));
            }
            else if (selectMe)
            {
                if(nowTimer % 4 == 0 || nowTimer % 4 == 1)
                {
                    rectTransform.localScale = new Vector2(maxSize, 0f);
                }
                else
                {
                    rectTransform.localScale = new Vector2(maxSize, maxSize);
                }
            }
            nowTimer++;
        }
        else
        {
            //nowTimer = 0;
            if (selectMe)
            {
                rectTransform.localScale = new Vector2(maxSize, minSize);
                gm.GetComponent<GameManager>().ChangeScene("testStage3");
            }
            inStart = true;
        }
    }
}
