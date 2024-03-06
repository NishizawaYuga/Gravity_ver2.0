using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    private Image image;
    [SerializeField]
    private Sprite life6;
    [SerializeField]
    private Sprite life5;
    [SerializeField] private Sprite life4;
    [SerializeField] private Sprite life3;
    [SerializeField] private Sprite life2;
    [SerializeField] private Sprite life1;
    [SerializeField] private Sprite life0;

    [SerializeField]
    private GameObject player;
    private PlayerMove pm;
    //スプライトの座標
    private RectTransform spritePos;
    //デフォルトの座標
    private float defPosX = 153.6f;
    private float defPosY = 571.9f;
    //シェイク関連
    private bool doShake = false;
    private int shakeLevel = 51;
    private int shakeTime = 0;
    private int shakeX;
    private int shakeY;
    //比較用ライフ
    private int oldLife = 0;
    void Start()
    {
        pm = player.GetComponent<PlayerMove>();
        image = GetComponent<Image>();
        spritePos = this.GetComponent<RectTransform>();
        oldLife = pm.GetLife();
    }

    // Update is called once per frame
    void Update()
    {
        if (oldLife > pm.GetLife())
        {
            doShake = true;
        }
        oldLife = pm.GetLife();
        Shake();
        if(pm.GetLife() == 6)
        {
            image.sprite = life6;
        }
        else if(pm.GetLife() == 5)
        {
            image.sprite = life5;
        }
        else if (pm.GetLife() == 4)
        {
            image.sprite = life4;
        }
        else if (pm.GetLife() == 3)
        {
            image.sprite = life3;
        }
        else if (pm.GetLife() == 2)
        {
            image.sprite = life2;
        }
        else if (pm.GetLife() == 1)
        {
            image.sprite = life1;
        }
        else if (pm.GetLife() == 0)
        {
            image.sprite = life0;
        }
    }

    private void Shake()
    {
        if (doShake)
        {
            image.color = Color.red;
            if(shakeLevel > 0)
            {
                shakeX = Random.Range(0, 100) % shakeLevel - (shakeLevel / 2);
                shakeY = Random.Range(0, 100) % shakeLevel - (shakeLevel / 2);
                shakeTime++;
                if(shakeTime < 5)
                {
                    shakeLevel -= 5;
                }
                else if (shakeLevel < 10 && shakeLevel > 5)
                {
                    shakeLevel -= 3;
                }
                else
                {
                    shakeLevel--;
                }
            }
            else
            {
                doShake = false;
                shakeLevel = 51;
                shakeTime = 0;
            }
            spritePos.position = new Vector2(defPosX + shakeX, defPosY + shakeY);
        }
        else
        {
            image.color = Color.white;
        }
    }
}
