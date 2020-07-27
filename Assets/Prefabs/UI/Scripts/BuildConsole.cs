using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// in-game console for debugging
/// </summary>

public class BuildConsole : MonoBehaviour
{
    //[SerializeField] GameObject panel = default;
    [SerializeField] Text text = default;
    [SerializeField] bool onlyLogs = true;

    struct Log
    {
        public string message;
        public string stackTrace;
        public LogType type;
    }

    Queue<Log> logQueue = new Queue<Log>();

    static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
    {
        { LogType.Assert, Color.black },
        { LogType.Error, Color.red },
        { LogType.Exception, Color.red },
        { LogType.Log, Color.black },
        { LogType.Warning, Color.yellow },
    };

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    // change the text when a new log is received
    void UpdateConsole()
    {
        string logstring = "";
        var qlist = logQueue.ToArray();

        for (int i = qlist.Length - 1; i >= 0; i--)
        {
            logstring += $"${qlist[i].stackTrace}: ${qlist[i].message}\n";
        }

        text.text = logstring;
    }

    // update the log queue with new logs
    void HandleLog(string message, string stackTrace, LogType type)
    {
        if (logQueue.Count > 10)
        {
            logQueue.Dequeue();
        }

        if (onlyLogs)
        {
            if (type == LogType.Log)
            {
                logQueue.Enqueue(new Log()
                {
                    message = message,
                    stackTrace = stackTrace,
                    type = type,
                });

                UpdateConsole();
            }
        }
        else
        {
            if (type == LogType.Error || type == LogType.Warning || type == LogType.Exception)
            {
                logQueue.Enqueue(new Log()
                {
                    message = message,
                    stackTrace = stackTrace,
                    type = type,
                });

                UpdateConsole();
            }
        }
    }
}
