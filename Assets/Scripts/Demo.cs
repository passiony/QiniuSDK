using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;

public class Demo : MonoBehaviour
{
    public static string url = "http://192.168.4.134:8888/tripartite/policy/1";

    void Start()
    {
        TestUpload();
    }

    public static void TestUpload()
    {
        //1.请求Token，添加header验证
        Dictionary<string, string> form = new Dictionary<string, string>();
        form.Add("Authorization", "Bearer 9701d8d0-f932-4648-8d08-1fc9fff8b895");
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
                //2.获得uptoken
                string uptoken = Convert.ToString(root["data"]["token"]);
                string key = string.Format("u3d.{0}.target",root["timestamp"]);
                string filePath = Application.streamingAssetsPath + "/9924317EB4DC8E3E1A604108133F837D.target";
                string persistPath = Application.persistentDataPath + "/9924317EB4DC8E3E1A604108133F837D.target";

                File.Copy(filePath, persistPath, true);

                QiniuUtil.UploadFile(key, persistPath, uptoken, (string text) => {
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