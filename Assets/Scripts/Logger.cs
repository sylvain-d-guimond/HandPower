using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Logger : Singleton<Logger>
{
    public StringArrEvent OnMessageLogged;
    public Text target;
    public int maxCharacters = 5000;
    public bool showStackTrace = false;

    public void LogMessageFromNetwork(string str)
    {
        LogMessage(new string[] { str }, true);
    }
    public void LogMessageFromNetwork(string[] strArr)
    {
        LogMessage(strArr, true);
    }

    public void LogMessage(string str)
    {
        LogMessage(new string[] { str }, false);
    }
    public void LogMessage(string[] strArr)
    {
        LogMessage(strArr, false);
    }

    private void LogMessage(string[] strArr, bool fromNetwork)
    {
        if (target)
        {
            string logResult = strArr[0] + (strArr.Length > 1 && showStackTrace ? strArr[1] : string.Empty) + "\n" + target.text;
            target.text = logResult.Substring(0, logResult.Length > maxCharacters ? maxCharacters : logResult.Length);
        }

        if (!fromNetwork)
            OnMessageLogged.Invoke(strArr);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += Application_logMessageReceived;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= Application_logMessageReceived;
    }

    private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
    {
        LogMessage(new string[] { condition, stackTrace }, false);
    } 
}