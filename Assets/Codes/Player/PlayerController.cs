using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Jobs;

public class PlayerController : MonoBehaviour
{
    public float power = 0.75f;
    private float oldPower = 0.75f;
    public new Rigidbody rigidbody;

    //ChangeGravity cG;

    //自機本体とカメラのオブジェクト
    GameObject player;
    GameObject mainCamera;

    //今現在の重力の方向 0から下右上左手前奥の順
    int underNum = 0;

    //動けるかどうかのフラグ
    bool canMove = true;

    //生きているかどうか
    bool isDead = false;

    //残機
    int life = 4;
    //復活までのタイマー
    int restartTimer = 250;
    const int maxReStartTimer = 250;
    //復活地点
    Vector3 restartPoint;
    //初期重力
    int startGravityNum = 0;
    //スクリプト
    private CaemeraFollowTarget cF;     //カメラ追従制御
    private ChangeGravity cG;   //重力

    //スピン
    int attackTimer = 0;
    const int maxAttak = 30;
    bool isAttack = false;
    private Easing ease;
    int coolTime = 0;
    bool coolCount = false;
    bool LR = false;
    [SerializeField]
    [Tooltip("スピンのエフェクト")]
    private GameObject spinEffect;

    public AudioSource seSpin;

    private float plTurn = 225;

    public enum GravityDirection
    {
        down,
        up,
        left,
        right,
        flont,
        back
    }

    // Start is called before the first frame update
    void Start()
    {
        //cG = GetComponent<ChangeGravity>();
        //オブジェクト指定
        player = this.gameObject;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        restartPoint = player.transform.position;
        cF = mainCamera.GetComponent<CaemeraFollowTarget>();
        cG = player.GetComponent<ChangeGravity>();
        startGravityNum = cG.GetNum();
        ease = new Easing();
        spinEffect.SetActive(false);
        ColorChange(underNum);
    }

    //重力方向取得
    public int GetNum()
    {
        return underNum;
    }
    //フラグ取得
    public bool GetCanMove()
    {
        return canMove;
    }
    //フラグ代入
    public void SendCanMoveFlag(bool flag)
    {
        canMove = flag;
    }
    public void ChangeActive(bool flag)
    {
        player.SetActive(flag);
    }

    //座標代入
    public void SetPosition(Vector3 pos)
    {
        player.transform.position.Set(pos.x, pos.y, pos.z);
    }

    //斜め減速処理
    private void DiagonalDeceleration(bool key1, bool key2, bool key3, bool key4)
    {
        if (key1 && key2 || key1 && key4 || key3 && key2 || key3 && key4)
        {
            power = oldPower * 0.65f;
        }
        else
        {
            power = oldPower;
        }
    }

    //方向転換
    private void Turn()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxisRaw("Horizontal") > 0)
        {
            if (!isAttack)
            {
                plTurn = 225;
                LR = false;
            }
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxisRaw("Horizontal") < 0)
        {
            if (!isAttack)
            {
                plTurn = -45;
                LR = true;
            }
        }
    }

    public void ManualTurn(int gravityNum)
    {
        if (gravityNum == int.Parse(GravityDirection.down.ToString()))
        {
            player.transform.rotation = Quaternion.Euler(0, plTurn, 0);
        }
        else if (gravityNum == int.Parse(GravityDirection.up.ToString()))
        {
            player.transform.rotation = Quaternion.Euler(0, -plTurn, 180);
        }
        else if (gravityNum == int.Parse(GravityDirection.left.ToString()))
        {
            player.transform.rotation = Quaternion.Euler(plTurn, 0, -90);
        }
        else if (gravityNum == int.Parse(GravityDirection.right.ToString()))
        {
            player.transform.rotation = Quaternion.Euler(-plTurn, 0, 90);
        }
        else if (gravityNum == int.Parse(GravityDirection.flont.ToString()))
        {
            player.transform.rotation = Quaternion.Euler(-90, 0, -plTurn);
        }
        else if (gravityNum == int.Parse(GravityDirection.back.ToString()))
        {
            player.transform.rotation = Quaternion.Euler(90, 0, plTurn);
        }
    }

    private void Spin()
    {
        if (!isAttack && Input.GetMouseButtonDown(0) && canMove && coolTime < 1)
        {
            isAttack = true;
            attackTimer = 0;
            spinEffect.SetActive(true);
            seSpin.Play();
        }
        else if (Gamepad.current != null)
        {
            if(!isAttack && Gamepad.current.rightTrigger.wasPressedThisFrame && canMove && coolTime < 1)
            {
                isAttack = true;
                attackTimer = 0;
                spinEffect.SetActive(true);
                seSpin.Play();
            }
        }
        if (isAttack)
        {
            if (attackTimer < maxAttak)
            {
                attackTimer++;
                if (cG.GetGravity().y < 0)
                {
                    player.transform.rotation = Quaternion.Euler(0, ease.OutQuad(360, plTurn, maxAttak, attackTimer), 0);
                }
                else if (cG.GetGravity().y > 0)
                {
                    player.transform.rotation = Quaternion.Euler(0, -ease.OutQuad(360, plTurn, maxAttak, attackTimer), 180);
                }
                else if (cG.GetGravity().x < 0)
                {
                    player.transform.rotation = Quaternion.Euler(ease.OutQuad(360, plTurn, maxAttak, attackTimer), 0,-90);
                }

                else if (cG.GetGravity().x > 0)
                {
                    player.transform.rotation = Quaternion.Euler(ease.OutQuad(360, plTurn, maxAttak, attackTimer), 0, 90);
                }
            }
            else
            {
                isAttack = false;
                coolCount = true;
                coolTime = 60;
                spinEffect.SetActive(false);
            }
        }
        if (coolCount)
        {
            coolTime--;
            if (coolTime < 0)
            {
                coolCount = false;
            }
        }
    }

    public void ColorChange(int gravityNum)
    {
        if(gravityNum == 0)
        {
            transform.Find("triangle").gameObject.GetComponent<Renderer>().material.color = new Color32(50,150 , 255, 1);
        }
        else if (gravityNum == 1)
        {
            transform.Find("triangle").gameObject.GetComponent<Renderer>().material.color = new Color32(255, 50, 0, 1);
        }
        else if (gravityNum == 2)
        {
            transform.Find("triangle").gameObject.GetComponent<Renderer>().material.color = new Color32(0, 255, 50, 1);
        }
        else if (gravityNum == 3)
        {
            transform.Find("triangle").gameObject.GetComponent<Renderer>().material.color = new Color32(230, 10, 230, 1);
        }
        else
        {
            transform.Find("triangle").gameObject.GetComponent<Renderer>().material.color = new Color32(0, 0, 0, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        cG.Update();
        if (!isDead)
        {
            if (canMove)
            {
                Turn();
                Spin();
            }

            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0f);


        }
        if (!isAttack)
        {
            if (cG.GetGravity().y < 0)
            {
                underNum = 0;
                player.transform.rotation = Quaternion.Euler(0, plTurn, 0);
            }
            else if (cG.GetGravity().y > 0)
            {
                underNum = 1;
                player.transform.rotation = Quaternion.Euler(0, -plTurn, 180);
            }
            else if (cG.GetGravity().x < 0)
            {
                underNum = 2;
                player.transform.rotation = Quaternion.Euler(plTurn, 0, -90);
            }

            else if (cG.GetGravity().x > 0)
            {
                underNum = 3;
                player.transform.rotation = Quaternion.Euler(-plTurn, 0, 90);
            }
            else if (cG.GetGravity().z < 0)
            {
                underNum = 4;
                player.transform.rotation = Quaternion.Euler(-90, 0, -plTurn);
            }
            else if (cG.GetGravity().z > 0)
            {
                underNum = 5;
                player.transform.rotation = Quaternion.Euler(90, 0, plTurn);
            }
        }
        //斜め減速処理
        DiagonalDeceleration(Input.GetKey(KeyCode.W), Input.GetKey(KeyCode.A), Input.GetKey(KeyCode.S), Input.GetKey(KeyCode.D));
    }

    public void ChangeGravity(int direction)
    {
        cG.GravityDirection(direction);
    }

    public void ChangeDead(bool dead)
    {
        isDead = dead;
        life--;
    }
    public bool IsDead()
    {
        return isDead;
    }

    public bool IsAttack()
    {
        return isAttack;
    }
}
