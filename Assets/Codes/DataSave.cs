using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class DataSave : MonoBehaviour
{
    //�ő�l
    private const int maxWorld = 8;
    private const int maxCourse = 8;
    private const int maxCrystal = 3;
    //�R�[�X�̃N���A��
    private int[,] courseClear = new int[9, 9];
    //�R�[�X�̃N���X�^��������
    private int[,,] getCrystal = new int[9, 9, 4];
    //�������݂̃��[�h
    private bool isAppend = false;

    private int worldNum = 0;
    private int courseNum = 0;
    //���̃V�[���̃X�e�[�W�ԍ�
    public int thisWorld = 0;
    public int thisCourse = 0;
    void Start()
    {
        //�f�[�^�i�[�p�X
        string path = Application.persistentDataPath + "/savedata.gvsv";
        //������
        for (int i = 0; i < maxWorld; i++)
        {
            for (int j = 0; j < maxCourse; j++)
            {
                courseClear[i, j] = 0;
                for (int k = 0; k < maxCrystal; k++)
                {
                    getCrystal[i, j, k] = 0;
                }
            }
        }
        //�f�[�^�̗L���̃`�F�b�N
        if (System.IO.Directory.Exists(Application.persistentDataPath))
        {
            if (!System.IO.File.Exists(Application.persistentDataPath + "/savedata.gvsv"))
            {
                using (var fs = new StreamWriter(path, isAppend, System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    for (int i = 0; i < maxWorld; i++)
                    {
                        for (int j = 0; j < maxCourse; j++)
                        {
                            fs.Write(courseClear[i, j]);
                            fs.Write(getCrystal[i, j, 0]);
                            fs.Write(getCrystal[i, j, 1]);
                            fs.Write(getCrystal[i, j, 2] + "\n");
                        }
                    }
                }
            }
            else
            {
                //�f�[�^������Ȃ炻���ǂݍ���
                using (var fs = new StreamReader(path, System.Text.Encoding.GetEncoding("UTF-8")))
                {
                    while (fs.Peek() != -1)
                    {
                        string data = fs.ReadLine();
                        courseClear[worldNum, courseNum] = int.Parse(data[0].ToString());
                        getCrystal[worldNum, courseNum, 0] = int.Parse(data[1].ToString());
                        getCrystal[worldNum, courseNum, 1] = int.Parse(data[2].ToString());
                        getCrystal[worldNum, courseNum, 2] = int.Parse(data[3].ToString());
                        if (worldNum < maxWorld)
                        {
                            worldNum++;
                        }
                        if (courseNum < maxCourse)
                        {
                            courseNum++;
                        }
                    }
                }
            }
        }
    }

    private void Update()
    {
        //�f�o�b�O�p
        //if (Input.GetKey(KeyCode.A))
        //{
        //    Save(true, 0);
        //}
    }

    public void Save()
    {
        courseClear[thisWorld, thisCourse] = 1;
        //�f�[�^�X�V���I�������txt�Ƀf�[�^�ۑ�
        //�f�[�^�i�[�p�X
        string path = Application.persistentDataPath + "/savedata.gvsv";
        using (var fs = new StreamWriter(path, isAppend, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            for (int i = 0; i < maxWorld; i++)
            {
                for (int j = 0; j < maxCourse; j++)
                {
                    fs.Write(courseClear[i, j]);
                    fs.Write(getCrystal[i, j, 0]);
                    fs.Write(getCrystal[i, j, 1]);
                    fs.Write(getCrystal[i, j, 2] + "\n");
                }
            }
        }
    }

    public void DataDelete()
    {
        //�Z�[�u�f�[�^����
        string path = Application.persistentDataPath + "/savedata.gvsv";
        using (var fs = new StreamWriter(path, isAppend, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            for (int i = 0; i < maxWorld; i++)
            {
                for (int j = 0; j < maxCourse; j++)
                {
                    fs.Write(0);
                    fs.Write(0);
                    fs.Write(0);
                    fs.Write(0 + "\n");
                }
            }
        }
    }

    //�N���X�^���Q�b�g
    public void GetCrystal(int crystalNum)
    {
        getCrystal[thisWorld, thisCourse, crystalNum] = 1;
    }

    //�f�[�^�擾
    public bool GetCrystalData(int crystalNum)
    {
        //Debug.Log(getCrystal[thisWorld, thisCourse, 0]); 
        if (getCrystal[thisWorld, thisCourse, crystalNum] == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
