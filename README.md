# 七牛云 SDK demo

## 上传步骤
1.获取token
2.上传文件

## 下载步骤
1.获得文件url
2.下载文件


## 上传本地文件
 ```cs
    public static void UploadFile(string key, string filePath, string token, Action<string> callback)
    {
        //配置config
        Config config = new Config();
        config.Zone = Zone.ZONE_CN_South;
        config.UseHttps = false;
        config.UseCdnDomains = false;
        config.ChunkSize = ChunkUnit.U512K;
        //>表单上传
        UploadManager target = new UploadManager(config);
        HttpResult result = target.UploadFile(filePath, key, token, null);
        if (callback != null)
            callback(result.Text);
    }
```

## 上传字节流

```cs
    public static void UploadData(string key, byte[] data, string token, Action<string> callback)
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
        HttpResult result = target.UploadData(data, key, token, null);
        if (callback != null)
            callback(result.Text);
    }
```

## 断点续传
```cs
    public static void UploadFileResum(string key, string filePath, string token, Action<string> callback)
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
        var result = ru.UploadFile(filePath, key, token, put);
        if (callback != null)
            callback(result.Text);
    }
```
## 下载文件
```cs
    public static void DownloadFile(string url,string savePath, Action<int> callback)
    {
        HttpResult result = DownloadManager.Download(url, savePath);

        if (callback != null)
            callback(result.Code);
    }
```