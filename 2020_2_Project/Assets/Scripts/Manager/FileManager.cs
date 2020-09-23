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

    public static Dictionary<string, float> playerInfo = new Dictionary<string, float>();
    public static Dictionary<string, bool> soundSetting = new Dictionary<string, bool>();

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        ReadData("DB_player.csv", playerInfo);
        ReadData("DB_bool.csv", soundSetting);
    }

    private void OnApplicationQuit()
    {
        WriteData("DB_player.csv", playerInfo);
    }

    public void ReadData(string _filename, Dictionary<string,float> _readDic)
    {
        string filepath = PathForDocumentsFile(_filename);

        if (File.Exists(filepath)) // 이 파일이 존재한다면
        {
            List<string> readList = ReadData_oldFile(filepath);

            for (int i = 0; i < readList.Count; i += 2)
                _readDic.Add(readList[i].ToString(), float.Parse(readList[i + 1]));
        }
        else // 파일이 존재하지 않다면(앱 설치 후 첫 실행 시에만 작동)
        {
            List<string> readList = ReadData_newFile(_filename);

            for (int i = 0; i < readList.Count; i += 2)
                _readDic.Add(readList[i].ToString(), float.Parse(readList[i + 1]));
        }
    }

    public void ReadData(string _filename, Dictionary<string, bool> _readDic)
    {
        string filepath = PathForDocumentsFile(_filename);

        if (File.Exists(filepath)) // 이 파일이 존재한다면
        {
            List<string> readList = ReadData_oldFile(filepath);

            for (int i = 0; i < readList.Count; i += 2)
                _readDic.Add(readList[i].ToString(), bool.Parse(readList[i + 1]));
        }
        else // 파일이 존재하지 않다면(앱 설치 후 첫 실행 시에만 작동)
        {
            List<string> readList = ReadData_newFile(_filename);

            for (int i = 0; i < readList.Count; i += 2)
                _readDic.Add(readList[i].ToString(), bool.Parse(readList[i + 1]));
        }
    }

    public void ReadData(string _filename, Dictionary<string, int> _readDic)
    {
        string filepath = PathForDocumentsFile(_filename);

        if (File.Exists(filepath)) // 이 파일이 존재한다면
        {
            List<string> readList = ReadData_oldFile(filepath);

            for (int i = 0; i < readList.Count; i += 2)
                _readDic.Add(readList[i].ToString(), int.Parse(readList[i + 1]));
        }
        else // 파일이 존재하지 않다면(앱 설치 후 첫 실행 시에만 작동)
        {
            List<string> readList = ReadData_newFile(_filename);

            for (int i = 0; i < readList.Count; i += 2)
                _readDic.Add(readList[i].ToString(), int.Parse(readList[i + 1]));
        }
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

    public static void WriteData(string _filename, Dictionary<string, bool> _saveDic)
    {
        string path = PathForDocumentsFile(_filename);
        FileStream f = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter writer = new StreamWriter(f);

        foreach (KeyValuePair<string, bool> items in _saveDic)
            writer.WriteLine(items.Key + "," + items.Value);
        writer.Close();
        f.Close();
    }

    public void WriteData(string _filename, Dictionary<string, int> _saveDic)
    {
        string path = PathForDocumentsFile(_filename);
        FileStream f = new FileStream(path, FileMode.Create, FileAccess.Write);

        StreamWriter writer = new StreamWriter(f);

        foreach (KeyValuePair<string, int> items in _saveDic)
            writer.WriteLine(items.Key + "," + items.Value);
        writer.Close();
        f.Close();
    }

    private static string PathForDocumentsFile(string _filename) // 플랫폼의 데이터 저장 경로에 파일이름을 추가해주는 함수
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer) // 안드로이드나 아이폰 플랫폼이면
            return Application.persistentDataPath + "/" + _filename; // persistentDataPath = /storage/emulated/0/Android/data/번들이름/files
        return null;
    }

    private List<string> ReadData_oldFile(string _filePath)
    {
        FileStream fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);

        string source = "";
        string[] divsource;
        List<string> divList = new List<string>();

        source = streamReader.ReadLine();

        while (source != null)
        {
            divsource = source.Split(',');
            divList.Add(divsource[0]);
            divList.Add(divsource[1]);
            source = streamReader.ReadLine();
        }

        streamReader.Close();
        fileStream.Close();

        return divList;
    }

    private List<string> ReadData_newFile(string _filename)
    {
        // LastIndexOf(char) : 뒤에서부터 검색하면서 첫 char 포함 뒤 문자열을 짤라준다.
        // SubString(index1, index2) : index1부터 index2의 직전 텍스트까지만 잘라서 반환한다.
        TextAsset data = Resources.Load("csvData/" + _filename.Substring(0, _filename.LastIndexOf('.')), typeof(TextAsset)) as TextAsset;
        if (data == null)
            return null;
        StringReader stringReader = new StringReader(data.text);

        string source = "";
        string[] divsource;
        List<string> divList = new List<string>();

        source = stringReader.ReadLine();

        while (source != null)
        {
            divsource = source.Split(',');
            divList.Add(divsource[0]);
            divList.Add(divsource[1]);
            source = stringReader.ReadLine();
        }

        stringReader.Close();

        return divList;
    }
}
