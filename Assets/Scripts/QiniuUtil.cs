using UnityEngine;
using System;
using System.IO;
using System.Text;
using Qiniu.Http;
using Qiniu.Storage;

public class QiniuUtil
{
    //上传本地文件
    public static void UploadFile(string key, string filePath, string uptoken, Action<string> callback)
    {
        //配置config
        Config config = new Config();
        config.Zone = Zone.ZONE_CN_South;
        config.UseHttps = false;
        config.UseCdnDomains = false;
        config.ChunkSize = ChunkUnit.U512K;

        //>表单上传
        UploadManager target = new UploadManager(config);
        HttpResult result = target.UploadFile(filePath, key, uptoken, null);
        if (callback != null)
            callback(result.Text);
    }

    //上传字节流
    public static void UploadData(string key, byte[] data, string uptoken, Action<string> callback)
    {
        //byte[] data = System.IO.File.ReadAllBytes(filePath);

        //配置config
        Config config = new Config();
        config.Zone = Zone.ZONE_CN_South;
        config.UseHttps = false;
        config.UseCdnDomains = false;
        config.ChunkSize = ChunkUnit.U512K;

        //>表单上传
        UploadManager target = new UploadManager(config);
        HttpResult result = target.UploadData(data, key, uptoken, null);
        if (callback != null)
            callback(result.Text);
    }

    //断点续传
    public static void UploadFileResum(string key, string filePath, string uptoken, Action<string> callback)
    {
        //配置config
        Config config = new Config();
        config.Zone = Zone.ZONE_CN_South;
        config.UseHttps = false;
        config.UseCdnDomains = false;
        config.ChunkSize = ChunkUnit.U512K;

        PutExtra put = new PutExtra();
        put.ProgressHandler = (long uploadedBytes, long totalBytes) =>
        {
            Debug.Log(uploadedBytes+"/"+ totalBytes);
        };

        ResumableUploader ru = new ResumableUploader(config);
        var result = ru.UploadFile(filePath, key, uptoken, put);
        if (callback != null)
            callback(result.Text);
    }

    //下载文件
    public static void DownloadFile(string url,string savePath, Action<int> callback)
    {
        HttpResult result = DownloadManager.Download(url, savePath);

        if (callback != null)
            callback(result.Code);
    }

    /// <summary>
    /// 计算文件的MD5值
    /// </summary>
    public static string md5file(string file)
    {
        try
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fs);
            fs.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("md5file() fail, error:" + ex.Message);
        }
    }

}
