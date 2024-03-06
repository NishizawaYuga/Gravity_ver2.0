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

    //�I�u�W�F�N�g
    [SerializeField]
    [Tooltip("�v���C���[�I�u�W�F�N�g��u��")]
    private GameObject target;

    //�J����
    [SerializeField]
    [Tooltip("�J����")]
    private GameObject mainCamera;

    //�J����
    [SerializeField]
    [Tooltip("�ڒn����")]
    private GameObject groundCheck;

    //�X�N���v�g
    PlayerController script;
    UnderCollider underCollider;
    private CaemeraFollowTarget cF;     //�J�����Ǐ]����
    GroundCheck gc;

    //�W�����v�t���O
    bool isJump = false;
    //�W�����v���̐���
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

    //���G�\��
    //private Color32[,] DefaultColors = new Color32[7];
    private Color32 test;
    float testTimer;

    void Start()
    {
        TryGetComponent(out rb);
        //�X�N���v�g�o�^
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
            //�ړ�
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

    //�ړ�
    private void Move()
    {
        //�J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        if (script.GetNum() == 0 || script.GetNum() == 1)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 0).normalized);
            //�����L�[�̓��͒l���J�����̌�������ړ�����������
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂�
            //�d�͕����ɉ����ăW�����v�����̑��x�x�N�g���ʒu�ύX
            rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y + Vector3.up.y * Jump(), 0);
            //�L�[���͂ɂ��ړ����������܂��Ă���ꍇ�ɂ́A�L�����N�^�[�̌�����i�s�����ɍ��킹��
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else if (script.GetNum() == 2 || script.GetNum() == 3)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(0, 1, 0).normalized);
            //�����L�[�̓��͒l���J�����̌�������ړ�����������
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂�
            //�d�͕����ɉ����ăW�����v�����̑��x�x�N�g���ʒu�ύX
            rb.velocity = moveForward * moveSpeed + new Vector3(rb.velocity.x + Vector3.up.y * Jump(), 0, 0);
            //�L�[���͂ɂ��ړ����������܂��Ă���ꍇ�ɂ́A�L�����N�^�[�̌�����i�s�����ɍ��킹��
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
        else if (script.GetNum() == 4 || script.GetNum() == 5)
        {
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(0, 1, 0).normalized);
            //�����L�[�̓��͒l���J�����̌�������ړ�����������
            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizotal;
            //�ړ������ɃX�s�[�h���|����B�W�����v�◎��������ꍇ�́A�ʓrY�������̑��x�x�N�g���𑫂�
            //�d�͕����ɉ����ăW�����v�����̑��x�x�N�g���ʒu�ύX
            rb.velocity = moveForward * moveSpeed + new Vector3(0, 0, rb.velocity.z + Vector3.up.y * Jump());
            //�L�[���͂ɂ��ړ����������܂��Ă���ꍇ�ɂ́A�L�����N�^�[�̌�����i�s�����ɍ��킹��
            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
        }
    }

    private float Jump()
    {
        //�ڒn����
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
        //�������̏����ʒu�Ǝp��
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
        //�����̋���
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

    //�l�X�ȓ����蔻��
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
    //�v���C���[���N���ł����肷��L���͈�
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
