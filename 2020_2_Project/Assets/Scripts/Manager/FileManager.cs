using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;

// Use Manual
// 첫 앱 실행 시 파일을 받아오는 것은 Resources에서 담당
// 첫 앱 실행 후 종료하면 /storage/emulated/0/Android/data/번들이름/files 에 저장함
// 이후 앱 실행 시 위 경로로 저장된 파일을 불러옴.
// ReadData / WriteData 함수에 파일이름을 쓸 때는 확장자까지 작성해야 함.
// Awake에 모든 csv 파일을 로드하는 코드를 작성, OnApplicationQuit에 모든 csv 파일을 작성(저장)하는 코드를 작성.

public class FileManager : MonoBehaviour
{
    public static FileManager instance;

    public Dictionary<string, float> playerInfo = new Dictionary<string, float>();

    private string _csvPath; 

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        playerInfo = ReadData("DB_player.csv");
    }

    private void OnApplicationQuit()
    {
        WriteData("DB_player.csv", playerInfo);
    }

    public Dictionary<string, float> ReadData(string _filename)
    {
        Dictionary<string, float> tempDic = new Dictionary<string, float>();
        string source;
        string[] temp;

        string path = PathForDocumentsFile(_filename);

        if (File.Exists(path)) // 이 파일이 존재한다면
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            source = sr.ReadLine();
            while (source != null)
            {
                temp = source.Split(',');
                source = sr.ReadLine();
                tempDic.Add(temp[0].ToString(), float.Parse(temp[1]));
            }
            sr.Close();
            file.Close();
        }
        else
        {
            // LastIndexOf(char) : 뒤에서부터 검색하면서 첫 char 포함 뒤 문자열을 짤라준다.
            TextAsset data = Resources.Load("csvData/" + _filename.Substring(0, _filename.LastIndexOf('.')), typeof(TextAsset)) as TextAsset;
            if (data == null)
                return null;
            StringReader sr = new StringReader(data.text);

            source = sr.ReadLine();
            while (source != null)
            {
                temp = source.Split(',');
                source = sr.ReadLine();
                tempDic.Add(temp[0].ToString(), float.Parse(temp[1]));
            }
            sr.Close();
        }

        return tempDic;
    }

    public void WriteData(string _filename, Dictionary<string, float> _saveDic)
    {
        string path = PathForDocumentsFile(_filename);
        FileStream f = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter writer = new StreamWriter(f);

        foreach (KeyValuePair<string, float> items in _saveDic)
            writer.WriteLine(items.Key + "," + items.Value);
        writer.Close();
        f.Close();
    }

    private string PathForDocumentsFile(string _filename) // 플랫폼의 데이터 저장 경로에 파일이름을 추가해주는 함수
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer) // 안드로이드나 아이폰 플랫폼이면
        {
            _csvPath = "";
            _csvPath = Application.persistentDataPath + "/" + _filename; // persistentDataPath = /storage/emulated/0/Android/data/번들이름/files
            return _csvPath;
        }
        return null;
    }
}
