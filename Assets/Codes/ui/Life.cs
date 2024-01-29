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
    void Start()
    {
        pm = player.GetComponent<PlayerMove>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        //var sr = this.gameObject.GetComponent<SpriteRenderer>();

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
}
