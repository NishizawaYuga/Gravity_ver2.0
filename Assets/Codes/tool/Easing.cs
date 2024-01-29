using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easing
{
    public float InQuad(float move, float start, float maxTime, float nowTime)
    { //move���ړ��ʁAstart���ŏ��̒n�_�AmaxTime���ړ����ԁAnowTime���o�ߎ���
        nowTime /= maxTime;
        return move * nowTime * nowTime + start;
    }
    public float OutQuad(float move, float start, float maxTime, float nowTime)
    { //move���ړ��ʁAstart���ŏ��̒n�_�AmaxTime���ړ����ԁAnowTime���o�ߎ���
        nowTime /= maxTime;
        return -move * nowTime * (nowTime - 2) + start;
    }
    public float InOutQuad(float move, float start, float maxTime, float nowTime)
    { //move���ړ��ʁAstart���ŏ��̒n�_�AmaxTime���ړ����ԁAnowTime���o�ߎ���
        nowTime /= maxTime / 2;
        if (nowTime < 1) return move / 2 * nowTime * nowTime + start;
        return -move / 2 * ((--nowTime) * (nowTime - 2) - 1) + start;
    }
    public Vector3 InQuadVec3(Vector3 move, Vector3 start, float maxTime, float nowTime)
    { //move���ړ��ʁAstart���ŏ��̒n�_�AmaxTime���ړ����ԁAnowTime���o�ߎ���
        nowTime /= maxTime;
        return move * nowTime * nowTime + start;
    }
    public Vector3 OutQuadVec3(Vector3 move, Vector3 start, float maxTime, float nowTime)
    { //move���ړ��ʁAstart���ŏ��̒n�_�AmaxTime���ړ����ԁAnowTime���o�ߎ���
        nowTime /= maxTime;
        return -move * nowTime * (nowTime - 2) + start;
    }
    public Vector3 InOutQuadVec3(Vector3 move, Vector3 start, float maxTime, float nowTime)
    { //move���ړ��ʁAstart���ŏ��̒n�_�AmaxTime���ړ����ԁAnowTime���o�ߎ���
        nowTime /= maxTime / 2;
        if (nowTime < 1) return move / 2 * nowTime * nowTime + start;
        return -move / 2 * ((--nowTime) * (nowTime - 2) - 1) + start;
    }
}
