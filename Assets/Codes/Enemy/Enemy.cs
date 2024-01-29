using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //�������g
    GameObject enemy;
    //���E
    [SerializeField]
    [Tooltip("���E")]
    GameObject visibility;
    //�I�u�W�F�N�g
    [SerializeField]
    [Tooltip("�v���C���[�I�u�W�F�N�g��u��")]
    private GameObject target;
    //�X�N���v�g
    private Visibility vis;
    private ChangeGravity cG;   //�d��
    //�ǐՎ���
    int chaseTime = 60;
    int chaseMaxTime = 60;
    //�ēx���G�܂ł̃N�[���^�C��
    int coolSarchTime = 30;
    int coolSarchMaxTime = 30;
    //���
    int stateNum = 0;
    bool Damage = false;
    //�ړ����x
    float speed = 2f;

    void Start()
    {
        enemy = this.gameObject;
        vis = visibility.GetComponent<Visibility>();
        cG = enemy.GetComponent<ChangeGravity>();
    }
    void FixedUpdate()
    {
        switch (stateNum)
        {
            case 0:
                //���G�͈͂ɓ��������`�F�b�N
                CheckSearch();
                break;
            case 1:
                //�ǐ�
                Chase();
                break;
            case 2:
                //�x�e
                BreakTime();
                break;
        }
    }
    void CheckSearch()
    {
        if (vis.GetSearch())
        {
            //�ǐՊJ�n
            stateNum = 1;
        }
    }

    void Chase()
    {
        chaseTime--;
        Vector3 velocity = Vector3.zero;
        if (chaseTime > 0)
        {
            velocity = target.transform.position - enemy.transform.position;
            velocity = velocity.normalized;
            if (cG.GetNum() < 2)
            {
                //velocity.x *= speed;
                velocity.y = 0;
                //velocity.z *= speed;
                enemy.transform.position += velocity;
            }
            else if (cG.GetNum() > 1 && cG.GetNum() < 4)
            {
                velocity.x = 0;
                velocity.y *= speed;
                velocity.z *= speed;
                enemy.transform.position += velocity;
            }
            else if (cG.GetNum() > 3)
            {
                velocity.x *= speed;
                velocity.y *= speed;
                velocity.z = 0;
                enemy.transform.position += velocity;
            }
            //��ɓG�̒����_���v���C���[��
            enemy.transform.rotation.SetLookRotation(target.transform.position);
        }
        else
        {
            chaseTime = chaseMaxTime;
            stateNum = 2;
        }
    }
    void BreakTime()
    {
        if(coolSarchTime > 0)
        {
            coolSarchTime--;
        }
        else
        {
            coolSarchTime = coolSarchMaxTime;
            stateNum = 0;
        }
    }
}
