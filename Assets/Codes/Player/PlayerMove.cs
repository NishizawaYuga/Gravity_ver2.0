using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private float inputHorizotal;
    private float inputVertical;
    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed;

    //オブジェクト
    [SerializeField]
    [Tooltip("プレイヤーオブジェクトを置く")]
    private GameObject target;

    //カメラ
    [SerializeField]
    [Tooltip("カメラ")]
    private GameObject mainCamera;

    //カメラ
    [SerializeField]
    [Tooltip("接地判定")]
    private GameObject groundCheck;

    //スクリプト
    PlayerController script;
    UnderCollider underCollider;
    private CaemeraFollowTarget cF;     //カメラ追従制御
    GroundCheck gc;

    //ジャンプフラグ
    bool isJump = false;
    //ジャンプ時の勢い
    float jumpPower = 3f;

    bool push = false;

    public AudioSource seJump;
    public AudioSource seOnArrow;
    public AudioSource seDamage;
    public AudioSource seDeath;

    private float knockBack = 2f;

    private int life = 6;
    bool invincible = false;
    int invincibleTimer = 30;

    bool isGoal = false;

    private StartSpinhit ssh;

    private ModelColorChange mcc;

    //無敵表示
    //private Color32[,] DefaultColors = new Color32[7];
    private Color32 test;
    float testTimer;

    void Start()
    {
        TryGetComponent(out rb);
        //スクリプト登録
        script = target.GetComponent<PlayerController>();
        underCollider = target.GetComponent<UnderCollider>();
        cF = mainCamera.GetComponent<CaemeraFollowTarget>();
        gc = groundCheck.GetComponent<GroundCheck>();
        ssh = target.GetComponent<StartSpinhit>();
        mcc = target.GetComponent<ModelColorChange>();
    }

    // Update is called once per frame
    void Update()
    {
        inputHorizotal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        //transform.Find("eye").gameObject.GetComponent<Renderer>().material.color = new Color32(255, 0, 0, 0);
        //testTimer+=0.01f;
        //isJump = gc.IsGround();
        if (isGoal)
        {
            GoalRotation(target.GetComponent<PlayerController>().GetNum());
        }

        if (script.GetCanMove() && !script.IsDead())
        {
            //移動
            Move();

            if (invincible)
            {
                if (invincibleTimer > 0)
                {
                    invincibleTimer--;
                }
                else
                {
                    invincible = false;
                    invincibleTimer = 30;
                }
            }

            if (!Input.GetKey(KeyCode.Space) && Gamepad.current == null)
            {
                if (push)
                {
                    push = false;
                }
            }
            else if(!Gamepad.current.bButton.isPressed && !Input.GetKey(KeyCode.Space) && Gamepad.current != null)
            {
                if (push)
                {
                    push = false;
                }
            }
        }
        else if (!script.GetCanMove() && !script.IsDead())
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
        else if (!script.GetCanMove() && script.IsDead())
        {

        }
    }

    //移動
    private void Move()
    {
        //カメラの方向から、X-Z平面の単位ベクトルを取得
        if (script.GetNum() == 0 || script.GetNum() == 1)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 0).normalized);
            //方向キーの入力値をカメラの向きから移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す
            //重力方向に応じてジャンプ方向の速度ベクトル位置変更
            rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y + Vector3.up.y * Jump(), 0);
            //キー入力により移動方向が決まっている場合には、キャラクターの向きを進行方向に合わせる
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else if (script.GetNum() == 2 || script.GetNum() == 3)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(0, 1, 0).normalized);
            //方向キーの入力値をカメラの向きから移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す
            //重力方向に応じてジャンプ方向の速度ベクトル位置変更
            rb.velocity = moveForward * moveSpeed + new Vector3(rb.velocity.x + Vector3.up.y * Jump(), 0, 0);
            //キー入力により移動方向が決まっている場合には、キャラクターの向きを進行方向に合わせる
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else if (script.GetNum() == 4 || script.GetNum() == 5)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(0, 1, 0).normalized);
            //方向キーの入力値をカメラの向きから移動方向を決定
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す
            //重力方向に応じてジャンプ方向の速度ベクトル位置変更
            rb.velocity = moveForward * moveSpeed + new Vector3(0, 0, rb.velocity.z + Vector3.up.y * Jump());
            //キー入力により移動方向が決まっている場合には、キャラクターの向きを進行方向に合わせる
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
    }

    private float Jump()
    {
        //接地判定
        isJump = CheckGrounded();

        if (Input.GetKey(KeyCode.Space))
        {
            if (isJump && !push)
            {
                push = true;
                seJump.Play();
                if (script.GetNum() % 2 == 0)
                {
                    return jumpPower;
                    //rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
                }
                else if (script.GetNum() % 2 == 1)
                {
                    return -jumpPower;
                }
            }
        }
        else if (Gamepad.current != null)
        {
            if (Gamepad.current.bButton.isPressed)
            {
                if (isJump && !push)
                {
                    push = true;
                    seJump.Play();
                    if (script.GetNum() % 2 == 0)
                    {
                        return jumpPower;
                    }
                    else if (script.GetNum() % 2 == 1)
                    {
                        return -jumpPower;
                    }
                }
            }
        }
        return 0.0f;
    }

    bool CheckGrounded()
    {
        Vector3 direction = new Vector3(0, 0, 0);
        //放つ光線の初期位置と姿勢
        if (script.GetNum() == 0)
        {
            direction = Vector3.down;
        }
        else if (script.GetNum() == 1)
        {
            direction = Vector3.up;
        }
        else if (script.GetNum() == 2)
        {
            direction = Vector3.left;
        }
        else if (script.GetNum() == 3)
        {
            direction = Vector3.right;
        }
        else if (script.GetNum() == 4)
        {
            direction = Vector3.back;
        }
        else if (script.GetNum() == 5)
        {
            direction = Vector3.forward;
        }
        var ray = new Ray(transform.position, direction);
        //光線の距離
        var distance = 0.06f;

        Debug.DrawRay(ray.origin, new Vector3(0f, distance, 0f), Color.red, 0.05f);

        return Physics.Raycast(ray, distance);
    }

    void GoalRotation(int gravityNum)
    {
        if (gravityNum == 0)
        {
            target.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
        }
        else if (gravityNum == 1)
        {
            target.transform.rotation = Quaternion.Euler(0f, 90f, 180f);
        }
        else if (gravityNum == 2)
        {
            target.transform.rotation = Quaternion.Euler(-90f, 0f, -95f);
        }
        else if (gravityNum == 3)
        {
            target.transform.rotation = Quaternion.Euler(90f, 0f, 95f);
        }
    }
    void GoalLeftArmPos(int gravityNum)
    {
        if (gravityNum == 0)
        {
            transform.Find("left_arm").gameObject.transform.position += new Vector3(0f,0.1f, 0f);
        }
        else if (gravityNum == 1)
        {
            transform.Find("left_arm").gameObject.transform.position += new Vector3(0f,-0.1f, 0f);
        }
        else if (gravityNum == 2)
        {
            transform.Find("left_arm").gameObject.transform.position += new Vector3(0.1f, 0f, 0f);
        }
        else if (gravityNum == 3)
        {
            transform.Find("left_arm").gameObject.transform.position += new Vector3(-0.1f, 0f, 0f);
        }
    }

    //様々な当たり判定
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "pipehole")
        {
            if (!script.IsAttack())
            {
                other.gameObject.GetComponent<Pipe>().StartWarp();
            }
        }
        if (other.gameObject.tag == "MissArea")
        {
            if (!script.IsAttack())
            {
                script.SendCanMoveFlag(false);
                //target.SetActive(false);
                cF.IsFollow(false);
                script.ChangeDead(true);
                seDeath.Play();
                life = 0;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Goal")
        {
            script.SendCanMoveFlag(false);
            collision.gameObject.GetComponent<Goal>().GetStar();
            script.ChangeGravity(6);
            GoalRotation(target.GetComponent<PlayerController>().GetNum());
            GoalLeftArmPos(target.GetComponent<PlayerController>().GetNum());
            isGoal = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (!script.IsAttack() && !collision.gameObject.GetComponent<Enemy_GreenVirus>().IsDeath())
            {
                rb.velocity = Vector3.zero;

                Vector3 distination = (target.transform.position - collision.transform.position).normalized;

                if (life > 0 && !invincible)
                {
                    seDamage.Play();
                    life--;
                    ssh.ParticleStart();

                    if (life <= 0 && !invincible && !script.IsDead())
                    {
                        script.ChangeDead(true);
                        seDeath.Play();
                        target.transform.position = new Vector3(100f, 100f, 100f);
                    }
                    invincible = true;
                }

                rb.AddForce(distination * knockBack, ForceMode.VelocityChange);

            }
        }
    }
    //プレイヤーが起動できたりする有効範囲
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "GravityArrow")
        {
            if (target.GetComponent<ChangeGravity>().GetNum() == other.gameObject.GetComponent<GravityArrow>().GetStartDirection())
            {
                if (this.gameObject.transform.position.y > other.gameObject.transform.position.y - 0.3f && this.gameObject.transform.position.y < other.gameObject.transform.position.y + 0.3f)
                {
                    if (this.gameObject.transform.position.x > other.gameObject.transform.position.x - 0.3f && this.gameObject.transform.position.x < other.gameObject.transform.position.x + 0.3f)
                    {
                        if (Input.GetMouseButton(1))
                        {
                            other.gameObject.GetComponent<GravityArrow>().ChangeGravity();
                            seOnArrow.Play();
                        }
                        else if (Gamepad.current != null)
                        {
                            if (Gamepad.current.yButton.IsPressed())
                            {
                                other.gameObject.GetComponent<GravityArrow>().ChangeGravity();
                                seOnArrow.Play();
                            }
                        }
                    }
                }
            }
        }
        if (other.gameObject.tag == "Crystal")
        {
            other.gameObject.GetComponent<crystal>().GetCrystal();
        }
    }

    public int GetLife()
    {
        return life;
    }
}
