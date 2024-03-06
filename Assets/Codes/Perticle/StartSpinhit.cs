using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpinhit : MonoBehaviour
{
    [SerializeField]
    [Tooltip("����������G�t�F�N�g")]
    private ParticleSystem particle;

    public void ParticleStart()
    {
        Debug.Log("�G�t�F�N�g�o����");
        //�p�[�e�B�N���V�X�e���̃C���X�^���X����
        ParticleSystem newParticle = Instantiate(particle);
        //�p�[�e�B�N���̔����ꏊ�������̏ꏊ��
        newParticle.transform.position = this.gameObject.transform.position;
        //�p�[�e�B�N���𔭐�������
        newParticle.Play();
        //�C���X�^���X�������p�[�e�B�N���V�X�e����GameObject�̍폜
        Destroy(newParticle.gameObject,0.2f);
    }
}
