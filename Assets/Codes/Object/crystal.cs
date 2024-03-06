using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystal : MonoBehaviour
{
    //増減変数
    float rotY = 0f;
    private const float rotX = 0f;

    GameObject crystalObj;

    [SerializeField]
    [Tooltip("変わる箇所は回転方向")]
    private int direction;

    [SerializeField]
    [Tooltip("番号 0～2")]
    int crystalNum;

    [SerializeField]
    [Tooltip("ゲームマネージャー")]
    GameObject gameManager;

    public AudioSource secrystal;

    private bool isGet = false;

    void Start()
    {
        crystalObj = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameManager.GetComponent<DataSave>().GetCrystalData(crystalNum) && !isGet)
        {
            crystalObj.transform.position = new Vector3(0f, 0f, 0f);
            isGet = true;
        }
        rotY += 2f;
        if (direction == 0)
        {
            crystalObj.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
        else if (direction == 1)
        {
            crystalObj.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
        else if (direction == 2)
        {
            crystalObj.transform.rotation = Quaternion.Euler(0, rotX, rotY);
        }
        else if (direction == 3)
        {
            crystalObj.transform.rotation = Quaternion.Euler(0, rotX, rotY);
        }
        else if (direction == 4)
        {
            crystalObj.transform.rotation = Quaternion.Euler(rotY, 90, 0);
        }
        else if (direction == 5)
        {
            crystalObj.transform.rotation = Quaternion.Euler(rotY, -90, 0);
        }

        if (rotY > 180f)
        {
            rotY = -180f;
        }
    }

    public void GetCrystal()
    {
        isGet = true;
        crystalObj.transform.position = new Vector3(0f, 0f, 0f);
        secrystal.Play();
        gameManager.GetComponent<DataSave>().GetCrystal(crystalNum);
    }

    public bool IsGet()
    {
        return isGet;
    }
}
