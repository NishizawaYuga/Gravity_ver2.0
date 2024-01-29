using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    //�f���{�̂Ǝ��@
    private GameObject planet;
    private GameObject player;

    //�d�͕ϓ����������邽�߂̃X�N���v�g
    ChangeGravity script;

    //�d�͔����͈�
    const float gravityArea = 3.0f;
    //���S���瑫��܂ł̋���
    const float distance = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        //�I�u�W�F�N�g�ƃX�N���v�g�o�^
        planet = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        script = player.GetComponent<ChangeGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        //�f���̍��W�𒆐S�Ɉ�苗���ȓ���������d�͂𔭐�������
        //������
        if (planet.transform.position.y - player.transform.position.y <= -distance)
        {
            script.GravityDirection(0);
        }
        //�E����
        else if (planet.transform.position.x - player.transform.position.x >= distance)
        {
            script.GravityDirection(1);
        }
        //�����
        else if (planet.transform.position.y - player.transform.position.y >= distance)
        {
            script.GravityDirection(2);
        }
        //������
        else if (planet.transform.position.x - player.transform.position.x <= -distance)
        {
            script.GravityDirection(3);
        }
        //��O����
        else if (planet.transform.position.z - player.transform.position.z >= distance)
        {
            script.GravityDirection(4);
        }
        //������
        else if (planet.transform.position.z - player.transform.position.z <= -distance)
        {
            script.GravityDirection(5);
        }
    }
}
