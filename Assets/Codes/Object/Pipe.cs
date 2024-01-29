using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�ڑ���I�u�W�F�N�g")]
    private GameObject destination;

    [SerializeField]
    [Tooltip("�^�C�v�ԍ�:0�͉����Ȃ��A1�Ńh�J���������s")]
    private int type;

    [SerializeField]
    [Tooltip("�h�J���̌����F�ԍ��͏d�͕����ԍ������A0����")]
    private int direction;

    [SerializeField]
    [Tooltip("���[�v���J�����ړ������邩")]
    private bool cameraMove = false;

    [SerializeField]
    [Tooltip("���[�v���J������]�����邩")]
    private bool cameraRotation = false;

    [SerializeField]
    [Tooltip("�G���A�ړ��p��")]
    private bool changeArea = false;

    //�v���C���[�I�u�W�F�N�g
    [SerializeField]
    [Tooltip("�v���C���[�I�u�W�F�N�g")]
    private GameObject player;

    //�J����
    [SerializeField]
    [Tooltip("�J����")]
    private GameObject mainCamera;

    public AudioSource sePipe;

    //�X�N���v�g
    PlayerController pCon;              //�v���C���[����
    private CaemeraFollowTarget cF;     //�J�����Ǐ]����
    private Pipe dPipe;                 //�ڑ���h�J��
    private ChangeGravity cg;           //�d�͕���
    private Easing ease;                //�C�[�W���O
    private RotationDirection rotD;     //��]����


    //�����l
    private int progressNum = 0;        //�i�s��
    private float nowTime = 0f;            //�o�ߎ���
    private const float maxTime = 1f;     //�ړ�����
    private Vector3 playerPos = new Vector3(0f, 0f, 0f);       //����p
    private Vector3 playerOldPos = new Vector3(0f, 0f, 0f);    //�v���C���[�̃X�^�[�g�n�_�i�[�p
    private int waitTimer = 0;          //�ҋ@����
    private float inoutPipeSpeed = 0.002f;    //�h�J�����o���肷��ۂ̑��x
    private int inoutTimer = 0; //�y�ǂ��o���肵�Ă��鎞��
    private float startObjectRotate = 0f;
    //�J�����C�[�W���O�i�ړ��j
    private Vector3 cameraPos = new Vector3(0f, 0f, 0f);
    private Vector3 oldCameraPos = new Vector3(0f, 0f, 0f);

    void Start()
    {
        //�X�N���v�g�o�^
        if (player != null)
        {
            pCon = player.GetComponent<PlayerController>();
            cF = mainCamera.GetComponent<CaemeraFollowTarget>();
            dPipe = destination.GetComponent<Pipe>();
            cg = player.GetComponent<ChangeGravity>();
        }
        ease = new Easing();
        rotD = new RotationDirection();
        //�h�J����������
        PipeDirection();

    }

    void FixedUpdate()
    {
        if (player != null)
        {
            if (type == 1)
            {
                if (progressNum == 1)
                {   //�y�ǂ̌����ɉ�����
                    if (direction <= 1)
                    {
                        //�d�́F�㉺
                        playerPos.x = ease.OutQuad(this.gameObject.transform.position.x - playerOldPos.x, playerOldPos.x, maxTime, nowTime);
                        playerPos.z = ease.OutQuad(this.gameObject.transform.position.z - playerOldPos.z, playerOldPos.z, maxTime, nowTime);
                    }
                    else if (direction >= 2 && direction <= 3)
                    {
                        //�d�́F���E
                        playerPos.y = ease.OutQuad(this.gameObject.transform.position.y - playerOldPos.y, playerOldPos.y, maxTime, nowTime);
                        playerPos.z = ease.OutQuad(this.gameObject.transform.position.z - playerOldPos.z, playerOldPos.z, maxTime, nowTime);
                    }
                    else if (direction >= 4)
                    {
                        //�d�́F�O��
                        playerPos.x = ease.OutQuad(this.gameObject.transform.position.x - playerOldPos.x, playerOldPos.x, maxTime, nowTime);
                        playerPos.y = ease.OutQuad(this.gameObject.transform.position.y - playerOldPos.y, playerOldPos.y, maxTime, nowTime);
                    }
                    player.transform.position = playerPos;
                    if (nowTime < maxTime)
                    {
                        nowTime += 0.02f;
                    }
                    else
                    {
                        sePipe.Play ();
                        if (direction <= 1)
                        {
                            player.transform.position = new Vector3(this.gameObject.transform.position.x, player.transform.position.y, this.gameObject.transform.position.z);
                        }
                        else if (direction >= 2 && direction <= 3)
                        {
                            player.transform.position = new Vector3(player.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
                        }
                        else if (direction >= 4)
                        {
                            player.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, player.transform.position.z);
                        }
                        nowTime = 0f;
                        progressNum++;
                        player.GetComponent<Collider>().enabled = false;
                        cg.GravityDirection(6);
                        if (cameraMove || cameraRotation)
                        {
                            cF.IsFollow(false);
                            cF.IsDefault(false);
                        }
                        else
                        {
                            cF.IsDefault(false);
                        }
                    }
                }
                else if (progressNum == 2)
                {
                    //�ҋ@����
                    if (waitTimer < 5 && inoutTimer < 50)
                    {
                        waitTimer++;
                    }
                    else
                    {
                        if (inoutTimer < 50)
                        {
                            inoutTimer++;
                            //�h�J���ɓ���
                            //�����ɉ����ē��������W��ς���
                            InWarp(direction, inoutPipeSpeed);
                        }
                        else
                        {
                            if (direction == 0)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(0f, -inoutPipeSpeed * inoutTimer, 0f);
                            }
                            else if (direction == 1)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(0f, +inoutPipeSpeed * inoutTimer, 0f);
                            }
                            else if (direction == 2)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(-inoutPipeSpeed * inoutTimer, 0f, 0f);
                            }
                            else if (direction == 3)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(+inoutPipeSpeed * inoutTimer, 0f, 0f);
                            }
                            else if (direction == 4)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, -inoutPipeSpeed * inoutTimer);
                            }
                            else if (direction == 5)
                            {
                                player.transform.position = this.gameObject.transform.position + new Vector3(0f, 0f, +inoutPipeSpeed * inoutTimer);
                            }
                            oldCameraPos = player.transform.position + cF.GetVector(direction);
                            oldCameraPos.y = cF.transform.position.y;
                            if (waitTimer > 0)
                            {
                                waitTimer--;
                            }
                            else
                            {
                                pCon.ManualTurn(dPipe.GetDirection());
                                progressNum = 3;
                                if (dPipe.GetDirection() == 0)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(0f, -inoutPipeSpeed * inoutTimer, 0f);
                                }
                                else if (dPipe.GetDirection() == 1)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(0f, +inoutPipeSpeed * inoutTimer, 0f);
                                }
                                else if (dPipe.GetDirection() == 2)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(-inoutPipeSpeed * inoutTimer, 0f, 0f);
                                }
                                else if (dPipe.GetDirection() == 3)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(+inoutPipeSpeed * inoutTimer, 0f, 0f);
                                }
                                else if (dPipe.GetDirection() == 4)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(0f, 0f, -inoutPipeSpeed * inoutTimer);
                                }
                                else if (dPipe.GetDirection() == 5)
                                {
                                    player.transform.position = destination.transform.position + new Vector3(0f, 0f, +inoutPipeSpeed * inoutTimer);
                                }
                                cameraPos = player.transform.position + cF.GetVector(dPipe.GetDirection());
                                cameraPos.y = cF.transform.position.y;
                                //�J������؂�ւ��邽�߈�u�����d�͕ύX
                                if (cameraRotation || changeArea)
                                {
                                    cF.ChangeCameraRotation(dPipe.GetDirection());
                                }
                                cF.SetPos();
                                inoutTimer = 0;
                                waitTimer = 0;
                                pCon.ColorChange(dPipe.GetDirection());
                            }
                        }
                    }
                }
                else if (progressNum == 3)
                {
                    if (waitTimer < rotD.GetMax(direction,dPipe.GetDirection()))
                    {
                        waitTimer += 4;
                        if (cameraRotation)
                        {
                            cF.transform.rotation = Quaternion.Euler(0f, 0f, ease.OutQuad(rotD.GetMax(direction, dPipe.GetDirection()) * rotD.BackReverce(direction, dPipe.GetDirection()), startObjectRotate, rotD.GetMax(direction, dPipe.GetDirection()), waitTimer));
                        }
                        if (cameraMove)
                        {
                            if (waitTimer > 0)
                            {
                                cF.transform.position = ease.OutQuadVec3(cameraPos - oldCameraPos, oldCameraPos, rotD.GetMax(direction, dPipe.GetDirection()), waitTimer) + cF.GetVector(dPipe.GetDirection());
                            }
                        }
                        sePipe.Play();
                    }
                    else
                    {
                        if (inoutTimer < 75)
                        {
                            inoutTimer++;
                            //�h�J���ɓ���
                            //�����ɉ����ē��������W��ς���
                            InWarp(dPipe.GetDirection(), -inoutPipeSpeed);
                            cF.SetPos();
                        }
                        else
                        {
                            if (cameraMove || cameraRotation || changeArea)
                            {
                                cF.IsFollow(true);
                            }
                            cg.GravityDirection(dPipe.GetDirection());
                            pCon.SendCanMoveFlag(true);
                            progressNum = 0;
                            player.GetComponent<Collider>().enabled = true;
                            waitTimer = 0;
                            inoutTimer = 0;
                        }
                    }
                }
            }
        }
    }
    //�y�ǂ̕�������
    private void PipeDirection()
    {
        //�y�ǂ̕�������
        if (direction == 0)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (direction == 1)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else if (direction == 2)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        else if (direction == 3)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (direction == 4)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else if (direction == 5)
        {
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        }
    }
    //���[�v��
    private void InWarp(int direction, float speed)
    {
        if (direction == 0)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - speed, player.transform.position.z);
        }
        else if (direction == 1)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + speed, player.transform.position.z);
        }
        else if (direction == 2)
        {
            player.transform.position = new Vector3(player.transform.position.x - speed, player.transform.position.y, player.transform.position.z);
        }
        else if (direction == 3)
        {
            player.transform.position = new Vector3(player.transform.position.x + speed, player.transform.position.y, player.transform.position.z);
        }
        else if (direction == 4)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - speed);
        }
        else if (direction == 5)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + speed);
        }
    }

    public void StartWarp()
    {
        if (player != null)
        {
            pCon.SendCanMoveFlag(false);
            progressNum++;
            playerPos = player.transform.position;
            playerOldPos = playerPos;
        }
    }

    //�ڑ���ɓn���ϐ�
    public Vector3 GetPos()
    {
        return this.transform.position;
    }
    public int GetDirection()
    {
        return direction;
    }

}
