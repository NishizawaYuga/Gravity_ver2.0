using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GravityArrow : MonoBehaviour
{
    [SerializeField]
    [Tooltip("�N���O�̏d�ʕ����A��͑��Ɠ���")]
    private int startDirection;

    [SerializeField]
    [Tooltip("�N����̏d�ʕ����A��͑��Ɠ���")]
    private int endDirection;

    //�v���C���[�I�u�W�F�N�g
    [SerializeField]
    [Tooltip("�v���C���[�I�u�W�F�N�g")]
    private GameObject player;

    //�J����
    [SerializeField]
    [Tooltip("�J����")]
    private GameObject mainCamera;

    [SerializeField]
    [Tooltip("���̏d�͖��ɘA��������Ȃ�")]
    private GameObject other;

    public AudioSource seChangeGravity;

    //�X�N���v�g
    private ChangeGravity cg;           //�d�͕���
    private CaemeraFollowTarget cF;     //�J�����Ǐ]����
    private GravityArrow ga;            //�A���������
    //private CameraFollow cF;
    private Easing ease;                //�C�[�W���O
    private RotationDirection rotD; //��]����

    //�N���t���O
    bool isStartUp = false;
    //�����n�_�ƏI���_���i�[����ϐ�
    Vector3 startPos;
    Quaternion startRot;
    Vector3 endPos;
    Quaternion endRot;
    private float nowTime = 0f;            //�o�ߎ���
    private float maxTime = 0f;     //�ړ�����
    private float rotateTime = 0f;
    private bool isRotate = false;
    private float startObjectRotate = 0f;
    private bool alreadyStarted = false;

    private bool playSE = false;


    void Start()
    {
        //�X�N���v�g�o�^
        cg = player.GetComponent<ChangeGravity>();
        cF = mainCamera.GetComponent<CaemeraFollowTarget>();
        if (other != null)
        {
            ga = other.GetComponent<GravityArrow>();
        }
        //cF = mainCamera.GetComponent<CameraFollow>();
        ease = new Easing();
        rotD = new RotationDirection();
        maxTime = rotD.GetMax(startDirection, endDirection);
        rotD.RotationalCorrection(this.gameObject, startDirection);
        startObjectRotate = rotD.GetStart(startDirection);
        //RotationalCorrection(startDirection);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isStartUp && !alreadyStarted)
        {
            //if (cF.GetRotation() != endRot)
            if (nowTime < maxTime && !isRotate)
            {
                //cF.SetPos();
                //RotateObject();
                this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, ease.OutQuad(maxTime * rotD.BackReverce(startDirection, endDirection), startObjectRotate, maxTime, nowTime));
                nowTime += 8f;
            }
            else
            {
                player.GetComponent<PlayerController>().SendCanMoveFlag(false);
                isRotate = true;
                nowTime = 0;
                //isStartUp = false;
                //cF.IsFollow(false);
                cF.IsDefault(false);
                cg.GravityDirection(endDirection);
                //this.gameObject.transform.rotation = endRot;
            }
        }
        else if(other != null)
        {
            if (ga.GetOtherStart())
            {
                //MakeAvailableAgain();
            }
        }
        if (isRotate && !alreadyStarted)
        {
            if (rotateTime < maxTime + 1)
            {
                if (!playSE)
                {
                    seChangeGravity.Play();
                    playSE = true;
                    player.GetComponent<PlayerController>().ColorChange(endDirection);
                }
                cF.transform.rotation = Quaternion.Euler(0f, 0f, ease.OutQuad(maxTime * rotD.BackReverce(startDirection, endDirection), startObjectRotate, maxTime, rotateTime));
                rotateTime += 4f;
                if (rotateTime > maxTime)
                {
                    rotateTime = maxTime + 1;
                }
            }
            else
            {
                //cF.ChangeCameraRotation(endDirection);
                player.GetComponent<PlayerController>().SendCanMoveFlag(true);
                rotateTime = 0f;
                isRotate = false;
                alreadyStarted = true;
                //cF.IsFollow(true);
                //cF.SetPos(player.transform.position - cF.GetVector(endDirection) / 2);
            }
        }
    }


    public void ChangeGravity()
    {
        if (!isStartUp)
        {
            isStartUp = true;
            //cF.IsFollow(false);
            //cF.IsDefault(false);
            startPos = cF.GetVector();
            //startRot = cF.GetRotation();
            startRot = Quaternion.Euler(0, 0, 0);
            //endRot = cF.GetEndRotation(endDirection);
            endRot = Quaternion.Euler(0, 0, 180);
        }
    }
    private void MakeAvailableAgain()
    {
        if (nowTime < rotD.GetMax(endDirection,startDirection))
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, ease.OutQuad(rotD.GetMax(endDirection, startDirection) * rotD.BackReverce(endDirection, startDirection), rotD.GetStart(endDirection), rotD.GetMax(endDirection, startDirection), nowTime));
            nowTime += 4f;
        }
        else
        {
            nowTime = 0;
            alreadyStarted = false;
        }
    }
    public bool GetOtherStart()
    {
        return isStartUp;
    }

    public int GetStartDirection()
    {
        return startDirection;
    }
}
