using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.XR.OpenVR;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class monitor : MonoBehaviour
{
    //プレイヤーオブジェクト
    [SerializeField]
    [Tooltip("プレイヤーオブジェクト")]
    private GameObject player;

    public AudioSource seMonitor;

    [SerializeField] private Sprite keyboard;
    [SerializeField] private Sprite controller;

    private Easing ease;

    private bool viewMode = false;
    private int maxTimer = 30;
    private int timer = 0;

    private bool playSE= false;

    private bool viewStart = false;

    void Start()
    {
        ease = new Easing();
        this.gameObject.transform.localScale = new Vector3(0.5f, 0f, 0.5f);
    }

    void FixedUpdate()
    {
        var spriteRender = this.gameObject.GetComponent<SpriteRenderer>();

        if (Gamepad.current == null)
        {
            spriteRender.sprite = keyboard;
        }
        else if (Gamepad.current != null)
        {
            spriteRender.sprite = controller;
        }
        if (player.transform.position.x < this.gameObject.transform.position.x + 1.0f && player.transform.position.x > this.gameObject.transform.position.x - 1.0f)
        {
            if (!playSE)
            {
                seMonitor.Play();
                playSE = true;
                viewStart = true;
            }
        }
        if (viewStart)
        {
            if (!viewMode)
            {
                if (timer < maxTimer)
                {
                    this.gameObject.transform.localScale = new Vector3(0.5f, ease.OutQuad(0.5f, 0f, maxTimer, timer), 0.5f);
                    timer++;
                }
                else
                {
                    viewMode = true;
                    timer = 0;
                    this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }
        }
    }
}
