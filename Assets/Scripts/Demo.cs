using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class Demo : MonoBehaviour
{
    public static string url = "http://192.168.4.134:8888/tripartite/policy/1";

    void Start()
    {
        TestDownload();
    }

    public static void TestUpload()
    {
        //1.请求Token，添加header验证
        Dictionary<string, string> form = new Dictionary<string, string>();
        form.Add("Authorization", "Bearer ffadb808-8c3d-4e82-a1f5-4dd02bc2477a");
        SimpleHttp.HttpPost(url, form, null, (WWW wwwInfo) =>
        {
            Debug.Log(wwwInfo.text);
            /**{
                "code": 200, 
                "message": "success", 
                "data": {
                    "token": "xxxxxxxxxxxx"
                }, 
                "timestamp": "1573436429382"
            }
            */
            JObject root = JObject.Parse(wwwInfo.text);
            int code = Convert.ToInt32(root["code"]);
            if (code == 200)
            {
                //2.获得token
                string token = Convert.ToString(root["data"]["token"]);
                string key = "TestImage";
                string filePath = "D:/TestImage.jpg";

                QiniuUtil.UploadFile(key, filePath, token, (string text) => {
                    Debug.Log(text);
                });
            }
        });
    }

    public static void TestDownload()
    {
        string url = "http://static.ingoing.company.cn/TestImage";
        QiniuUtil.DownloadFile(url, "D:/download.png", (int code) => {
            Debug.Log(code);
        });
    }
}