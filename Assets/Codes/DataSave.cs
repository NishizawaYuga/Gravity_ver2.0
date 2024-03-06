using UnityEngine;
using System.IO;
using UnityEngine.UIElements;

public class DataSave : MonoBehaviour
{
    //最大値
    private const int maxWorld = 8;
    private const int maxCourse = 8;
    private const int maxCrystal = 3;
    //コースのクリア状況
    private int[,] courseClear = new int[9, 9];
    //コースのクリスタル所持率
    private int[,,] getCrystal = new int[9, 9, 4];
    //書き込みのモード
    private bool isAppend = false;

    private int worldNum = 0;
    private int courseNum = 0;
    //そのシーンのステージ番号
    public int thisWorld = 0;
    public int thisCourse = 0;
    void Start()
    {
        //データ格納パス
        string path = Application.persistentDataPath + "/savedata.gvsv";
        //初期化
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
        //データの有無のチェック
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
                //データがあるならそれを読み込む
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
        //デバッグ用
        //if (Input.GetKey(KeyCode.A))
        //{
        //    Save(true, 0);
        //}
    }

    public void Save()
    {
        courseClear[thisWorld, thisCourse] = 1;
        //データ更新が終わったらtxtにデータ保存
        //データ格納パス
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
        //セーブデータ消去
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

    //クリスタルゲット
    public void GetCrystal(int crystalNum)
    {
        getCrystal[thisWorld, thisCourse, crystalNum] = 1;
    }

    //データ取得
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
