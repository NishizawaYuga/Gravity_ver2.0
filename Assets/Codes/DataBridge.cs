using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public static class DataBridge
{
    private const int maxWorld = 5;
    private const int maxCourse = 5;
    //�R�[�X�̃N���A��
    public static bool[,] courseClear = new bool[5, 5];
    //�R�[�X�̒��ԏ�
    public static int[,] middle = new int[5, 5];
    private static void Start()
    {
        for (int i = 0; i < maxWorld; i++)
        {
            for (int j = 0; j < maxCourse; j++)
            {
                courseClear[i, j] = false;
                middle[i, j] = 0;
            }
        }
    }
}
