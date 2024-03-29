﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHttp : MonoBehaviour
{
    private HttpInfo m_info;
    private WWW m_www;
    private float m_during = 0f;

    public static SimpleHttp newInstance
    {
        get
        {
            GameObject gameObject = new GameObject();
            SimpleHttp result = gameObject.AddComponent<SimpleHttp>();
            DontDestroyOnLoad(gameObject);
            return result;
        }
    }

    void Update()
    {
        if (m_info != null && m_www != null)
        {
            m_during += Time.deltaTime;
            if (m_during >= m_info.timeOut)
            {
                try
                {
                    m_www.Dispose();
                    if (m_info.callbackDel != null)
                    {
                        m_info.callbackDel(null);
                        m_info.callbackDel = null;
                        m_info = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("http timeout callback got exception " + ex.Message + "\n" + ex.StackTrace);
                }

                DestroyImmediate(gameObject);
            }
        }
    }

    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url">baseUrl+?+args</param>
    /// <param name="headData">表头</param>
    /// <param name="byteData">表单</param>
    /// <param name="callback">回调</param>
    /// <param name="timeOut">超时时间</param>
    public static void HttpGet(string url, Dictionary<string, string> headData, byte[] byteData, Action<WWW> callback, float timeOut = 10f)
    {
        HttpInfo httpInfo = new HttpInfo();
        httpInfo.callbackDel = callback;
        httpInfo.url = url;
        httpInfo.headData = headData;
        httpInfo.byteData = byteData;
        httpInfo.type = HTTP_TYPE.GET;
        httpInfo.timeOut = timeOut;
        SimpleHttp.newInstance.StartHttp(httpInfo);
    }

    /// <summary>
    /// Post请求
    /// </summary>
    /// <param name="url">baseUrl</param>
    /// <param name="headData">表头</param>
    /// <param name="byteData">表单</param>
    /// <param name="callback">回调</param>
    /// <param name="timeOut">超时时间</param>
    public static void HttpPost(string url, Dictionary<string, string> headData, byte[] byteData, Action<WWW> callback, float timeOut = 10f)
    {
        HttpInfo httpInfo = new HttpInfo();
        httpInfo.callbackDel = callback;
        httpInfo.url = url;
        httpInfo.headData = headData;
        httpInfo.byteData = byteData;
        httpInfo.type = HTTP_TYPE.POST;
        httpInfo.timeOut = timeOut;
        SimpleHttp.newInstance.StartHttp(httpInfo);
    }

    public void StartHttp(HttpInfo info)
    {
        if (info != null)
        {
            if (info.type == HTTP_TYPE.GET)
            {
                StartCoroutine(DoHttpGet(info));
            }

            if (info.type == HTTP_TYPE.POST)
            {
                StartCoroutine(DoHttpPost(info));
            }
        }
    }

    private IEnumerator DoHttpGet(HttpInfo info)
    {
        m_info = info;
        m_www = new WWW(m_info.url, m_info.byteData, m_info.headData);
        yield return m_www;

        Complete();
    }

    private IEnumerator DoHttpPost(HttpInfo info)
    {
        m_info = info;
        m_www = new WWW(m_info.url, m_info.byteData, m_info.headData);
        yield return m_www;

        Complete();
    }

    private void Complete()
    {
        try
        {
            if (m_info != null && m_info.callbackDel != null)
            {
                m_info.callbackDel(m_www);
                m_info.callbackDel = null;
            }
            m_info = null;
            m_www.Dispose();
        }
        catch (Exception ex)
        {
            Debug.Log("http complete callback got exception " + ex.Message + "\n" + ex.StackTrace);
        }

        DestroyImmediate(gameObject);
    }

}
