using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class DataBridge : MonoBehaviour
{
    [SerializeField]
    [Tooltip("���[���h��")]
    int WorldNum;

    [SerializeField]
    [Tooltip("�R�[�X��")]
    int CourceNum;

    //�Z�[�u�f�[�^
    private DataSave dSave;
    //�X�e�[�W����p
    private void Start()
    {
        dSave = this.GetComponent<DataSave>();
    }

//    public bool CheckIsGetCrystal(int CrystalNum)
//    {
//        //return dSave.GetCrystalData(WorldNum, CourceNum, CrystalNum);
//    }
}
