using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crystal_ui : MonoBehaviour
{
    public GameObject crystal;
    private Image image;
    [SerializeField]
    private Sprite crystal_none;
    [SerializeField]
    private Sprite crystal_get;
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (crystal.GetComponent<crystal>().IsGet())
        {
            image.sprite = crystal_get;
        }
    }
}
