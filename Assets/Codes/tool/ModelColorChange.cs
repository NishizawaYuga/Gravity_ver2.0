using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelColorChange : MonoBehaviour
{
    //モデルのマテリアルの色の変更
    public void ColorChange(Color color)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }
}
