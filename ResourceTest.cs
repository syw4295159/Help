using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class ResourceTest : MonoBehaviour
{
    public GameObject m_Perfab;
    // Start is called before the first frame update
    void Start()
    {
        //加载方式1：
        //GameObject obj = GameObject.Instantiate(m_Perfab);

        //加载方式2：resources文件下的资源储存有上限，所以项目一般不用
        //GameObject obj = GameObject.Instantiate(Resources.Load("Attack")) as GameObject;

        //加载方式3：AssetsBundle加载资源
        //AssetBundle asset = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/attack");
        //GameObject obj = GameObject.Instantiate(asset.LoadAsset<GameObject>("attack"));

        //加载方式4：UnityEditor编辑器代码的资源加载，路径为Assets文件夹的相对路径
        //GameObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameData/Prefabs/Attack.prefab");
        //GameObject.Instantiate(obj);

        //xml的序列化
        //SerilizeTest();

        //xml的反序列化
        //DeSerilizeTest();

        //二进制序列化
        //BinarySerTest();

        //二进制反序列化
        //BinaryDeSerilizeTest();

        //UnityScriptObject读取数据
        ReadTestAssets();
    }

    void SerilizeTest()
    {
        TestSerilize testSerilize = new TestSerilize();
        testSerilize.Id = 1;
        testSerilize.Name = "测试";
        testSerilize.List = new List<int>() { 0, 1, 2 };
        XmlSerilize(testSerilize);
    }

    void DeSerilizeTest()
    {
        TestSerilize testSerilize = XmlDeSerilize();
        string listStr = "";
        testSerilize.List.ForEach(x => listStr += x.ToString());
        Debug.LogFormat($"{testSerilize.Id},{testSerilize.Name},{listStr}");
    }

    void XmlSerilize(TestSerilize testSerilize)
    {
        //创建文件名
        FileStream fs = new FileStream(Application.dataPath + "/test.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        //创建写入流
        StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
        //实例化需要序列化的类型
        XmlSerializer xml = new XmlSerializer(testSerilize.GetType());
        //序列化：把testSerilize写入到sw里
        xml.Serialize(sw, testSerilize);
        sw.Close();//写入流关闭
        fs.Close();//文件流关闭
    }

    TestSerilize XmlDeSerilize()
    {
        FileStream fs = new FileStream(Application.dataPath + "/test.xml", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        XmlSerializer xs = new XmlSerializer(typeof(TestSerilize));
        TestSerilize testSerilize = xs.Deserialize(fs) as TestSerilize;
        fs.Close();
        return testSerilize;
    }

    void BinarySerTest()
    {
        TestSerilize testSerilize = new TestSerilize();
        testSerilize.Id = 5;
        testSerilize.Name = "二进制测试";
        testSerilize.List = new List<int>() { 0, 12, 23 };
        BinarySerilize(testSerilize);
    }

    void BinarySerilize(TestSerilize serilize)
    {
        FileStream fs = new FileStream(Application.dataPath + "/test.bytes", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, serilize);
        fs.Close();
    }
    

    void BinaryDeSerilizeTest()
    {
        TestSerilize testSerilize = BinaryDeserilize();
        string listStr = "";
        testSerilize.List.ForEach(x => listStr += x.ToString());
        Debug.LogFormat($"{testSerilize.Id},{testSerilize.Name},{listStr}");
    }
    TestSerilize BinaryDeserilize()
    {
        TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/test.bytes");
        MemoryStream ms = new MemoryStream(textAsset.bytes);
        BinaryFormatter bf = new BinaryFormatter();
        TestSerilize ts = bf.Deserialize(ms) as TestSerilize;
        ms.Close();
        return ts;
    }

    void ReadTestAssets()
    {
        AssetsSerilize assets = UnityEditor.AssetDatabase.LoadAssetAtPath<AssetsSerilize>("Assets/Scripts/TestAssets.asset");
        string listStr = "";
        assets.TestList.ForEach(x => listStr += x.ToString());
        Debug.LogFormat($"{assets.Id},{assets.Name},{listStr}");
    }

}
