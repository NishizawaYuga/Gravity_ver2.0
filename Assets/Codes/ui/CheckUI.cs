using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUI : MonoBehaviour
{
    public GameObject player;
    public GameObject Goal;
    public GameObject gameover;
    public GameObject retry;
    public GameObject title;
    public GameObject gameclear;
    void Start()
    {
        gameover.SetActive(false);
        retry.SetActive(false);
        title.SetActive(false);
        gameclear.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().IsDead())
        {
            gameover.SetActive (true);
            retry.SetActive (true);
            title.SetActive (true);
        }
        if(Goal.GetComponent<Goal>().isGoal()) {
        gameclear.SetActive (true);
        }
    }
}
