using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crystal : MonoBehaviour
{
    //‘Œ¸•Ï”
    float rotY = 0f;
    private const float rotX = 0f;

    GameObject crystalObj;

    [SerializeField]
    [Tooltip("•Ï‚í‚é‰ÓŠ‚Í‰ñ“]•ûŒü")]
    private int direction;

    public AudioSource secrystal;

    private bool isGet = false;

    void Start()
    {
        crystalObj = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
    }

    public bool IsGet()
    {
        return isGet;
    }
}
