using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGravity : MonoBehaviour
{
    [SerializeField] private Vector3 localGravity;
    private Rigidbody rBody;

    [SerializeField]
    [Tooltip("�d�͕���")]
    public int gravitySwitch;
    //�؂�ւ��p�ϐ�
    //int gravitySwitch = 2;

    // Use this for initialization
    private void Start()
    {
        rBody = this.GetComponent<Rigidbody>();
        rBody.useGravity = false; //�ŏ���rigidBody�̏d�͂��g��Ȃ�����
    }

    private void FixedUpdate()
    {
        SetLocalGravity(); //�d�͂�AddForce�ł����郁�\�b�h���ĂԁBFixedUpdate���D�܂����B
    }

    private void SetLocalGravity()
    {
        rBody.AddForce(localGravity, ForceMode.Acceleration);
    }

    public Vector3 GetGravity()
    {
        return localGravity;
    }

    public int GetNum()
    {
        return gravitySwitch;
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        {
            //gravitySwitch++;
            //if (gravitySwitch > 11)
            //{
            //    gravitySwitch = 0;
            //}
        }

        //01�㉺�F23:���E�F45�O��
        if (gravitySwitch == 0)
        {
            localGravity.x = 0;
            localGravity.y = -9.81f;
            localGravity.z = 0;
        }
        else if (gravitySwitch == 1)
        {
            localGravity.x = 0;
            localGravity.y = 9.81f;
            localGravity.z = 0;
        }
        else if (gravitySwitch == 2)
        {
            localGravity.x = -9.81f;
            localGravity.y = 0;
            localGravity.z = 0;
        }
        
        else if (gravitySwitch == 3)
        {
            localGravity.x = 9.81f;
            localGravity.y = 0;
            localGravity.z = 0;
        }
        else if (gravitySwitch == 4)
        {
            localGravity.x = 0;
            localGravity.y = 0;
            localGravity.z = -9.81f;
        }
        else if (gravitySwitch == 5)
        {
            localGravity.x = 0;
            localGravity.y = 0;
            localGravity.z = 9.81f;
        }
        else if(gravitySwitch == 6)
        {
            //���d��
            localGravity.x = 0f;
            localGravity.y = 0f;
            localGravity.z = 0f;
        }
    }

    public void GravityDirection(int gravityNum)
    {
        gravitySwitch = gravityNum;
    }
}
