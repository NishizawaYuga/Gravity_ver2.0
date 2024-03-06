using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class DataBridge : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ワールド数")]
    int WorldNum;

    [SerializeField]
    [Tooltip("コース数")]
    int CourceNum;

    //セーブデータ
    private DataSave dSave;
    //ステージ側専用
    private void Start()
    {
        dSave = this.GetComponent<DataSave>();
    }

//    public bool CheckIsGetCrystal(int CrystalNum)
//    {
//        //return dSave.GetCrystalData(WorldNum, CourceNum, CrystalNum);
//    }
}
