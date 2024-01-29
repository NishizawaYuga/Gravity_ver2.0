using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    [SerializeField]
    private GameObject gm;

    [SerializeField] private Sprite keyboard;
    [SerializeField] private Sprite controller;

    Image image;

    //•`‰æŒn
    private const float maxSize = 0.25f;
    private const float minSize = 0f;
    private int maxTimer = 30;
    private int nowTimer = 0;
    Easing ease;
    private bool inStart = false;
    private bool inEnd = false;

    private RectTransform rectTransform;

    public AudioSource button;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        ease = new Easing();
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
        if (Input.GetKeyDown(KeyCode.Space) && Gamepad.current==null)
        {
            if (inStart && !inEnd)
            {
                inEnd = true;
                button.Play();
            }
        }
        else if (Gamepad.current != null)
        {
            if (Gamepad.current.bButton.wasPressedThisFrame)
            {
                if (inStart && !inEnd)
                {
                    inEnd = true;
                    button.Play();
                }
            }
        }
        if(inEnd)
        {
            EndAnimation();
        }
    }

    void InAnimation()
    {
        if (nowTimer < maxTimer)
        {
            rectTransform.localScale = new Vector2(0.25f,ease.InOutQuad(maxSize, minSize, maxTimer, nowTimer));
            nowTimer++;
        }
        else
        {
            nowTimer = 0;
            rectTransform.localScale = new Vector2(0.25f, maxSize);
            inStart = true;
        }
    }
    void EndAnimation()
    {
        if (nowTimer < maxTimer)
        {
            rectTransform.localScale = new Vector2(0.25f, ease.InOutQuad(-maxSize, maxSize, maxTimer, nowTimer));
            nowTimer++;
        }
        else
        {
            nowTimer = 0;
            rectTransform.localScale = new Vector2(0.25f, minSize);
            inStart = true;
            gm.GetComponent<GameManager>().ChangeScene("testStage3");
        }
    }
}
