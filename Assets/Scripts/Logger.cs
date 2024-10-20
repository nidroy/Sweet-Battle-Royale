using System;
using System.IO;
using UnityEngine;

// ����� ��� ����������� ���������
public class Logger
{
    // ���� � ����� ��� �������� �����
    private static readonly string logFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Sweet-Battle-Royale",
        "log.txt");

    // ������ �����������
    public enum LogLevel
    {
        Info,       // �������������� ���������
        Warning,    // ��������������
        Error       // ������
    }

    /// <summary>
    /// ����� ��� ����������� ��������� � ���������� �������� ��������
    /// </summary>
    /// <param name="logLevel">������� �����������</param>
    /// <param name="module">��� ������, �� �������� ���������� �����������</param>
    /// <param name="message">��������� ��� �����������</param>
    public static void Log(LogLevel logLevel, string module, string message)
    {
        // ����������� ���� � ����� ��� ����
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // ����������� ������ ��� �����������
        string logMessage = $"{timestamp} [{logLevel}] {module}: {message}";

        // �������� � ������� ��� �������������
        if (logLevel == LogLevel.Info)
        {
            Debug.Log(logMessage);
        }
        else if (logLevel == LogLevel.Warning)
        {
            Debug.LogWarning(logMessage);
        }
        else if (logLevel == LogLevel.Error)
        {
            Debug.LogError(logMessage);
        }

        // ��������� ��������� � ����
        SaveLogToFile(logMessage);
    }

    /// <summary>
    /// ����� ��� ���������� ����� � ����
    /// </summary>
    /// <param name="logMessage">���������, ������� ����� ���������</param>
    private static void SaveLogToFile(string logMessage)
    {
        try
        {
            // ������� ����������, ���� ��� �� ����������
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

            // ��������� ���� � ������ ����������
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(logMessage); // ���������� ��� � ����
            }
        }
        catch (Exception ex)
        {
            // �������� ������, ���� �� ������� ��������� ��� � ����
            Debug.LogError($"Failed to write log to file: {ex.Message}!");
        }
    }
}
