using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Visibility : MonoBehaviour
{
    //���E
    GameObject visibility;
    //�����t���O
    bool search = false;

    // Start is called before the first frame update
    void Start()
    {
        visibility = this.gameObject;
    }

    //�Z�b�^�[�E�Q�b�^�[
    public void SetSearch(bool flag)
    {
        search = flag;
    }
    public bool GetSearch()
    {
        return search;
    }    
    //���G�L���`�F�b�N
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            search = true;
        }
        else
        {
            search = false;
        }
    }
}
