using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSpinhit : MonoBehaviour
{
    [SerializeField]
    [Tooltip("発生させるエフェクト")]
    private ParticleSystem particle;

    public void ParticleStart()
    {
        Debug.Log("エフェクト出たよ");
        //パーティクルシステムのインスタンス生成
        ParticleSystem newParticle = Instantiate(particle);
        //パーティクルの発生場所を引数の場所に
        newParticle.transform.position = this.gameObject.transform.position;
        //パーティクルを発生させる
        newParticle.Play();
        //インスタンス化したパーティクルシステムのGameObjectの削除
        Destroy(newParticle.gameObject,0.2f);
    }
}
