using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    //増減変数
    float rotY = 0f;
    private const float rotX = -90f;
    //クリアフラグ
    bool isClear = false;

    //クリア後処理
    private int waitTimer = 180;
    private const int maxWaitTimer = 300;

    GameObject goal;

    [SerializeField]
    [Tooltip("変わる箇所は回転方向")]
    private int direction;

    [SerializeField]
    [Tooltip("ゲームマネージャー")]
    GameObject gameManager;

    public AudioSource seGoal;

    void Start()
    {
        goal = this.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotY += 1.6f;
        if (direction == 0)
        {
            goal.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
        else if (direction == 1)
        {
            goal.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
        else if (direction == 2)
        {
            goal.transform.rotation = Quaternion.Euler(0, rotX, rotY);
        }
        else if (direction == 3)
        {
            goal.transform.rotation = Quaternion.Euler(0, rotX, rotY);
        }
        else if (direction == 4)
        {
            goal.transform.rotation = Quaternion.Euler(rotY, 90, 0);
        }
        else if (direction == 5)
        {
            goal.transform.rotation = Quaternion.Euler(rotY, -90, 0);
        }

        if (rotY > 180f)
        {
            rotY = -180f;
        }

        GoalScene();
    }
    public void GetStar()
    {
        //goal.SetActive(false);
        goal.transform.position = new Vector3(0f, 0f, 0f);
        isClear = true;
        seGoal.Play ();
        gameManager.GetComponent<DataSave>().Save();
    }
    private void GoalScene()
    {
        if (isClear)
        {
            if (waitTimer > 0)
            {
                waitTimer--;
            }
            else
            {
                waitTimer = maxWaitTimer;
                isClear = false;
                SceneManager.LoadScene("title");
            }
        }
    }

    public bool isGoal()
    {
        return isClear;
    }
}
