using System;
using System.IO;
using UnityEngine;

// Класс для логирования сообщений
public class Logger
{
    // Путь к файлу для хранения логов
    private static readonly string logFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Sweet-Battle-Royale",
        "log.txt");

    // Уровни логирования
    public enum LogLevel
    {
        Info,       // Информационные сообщения
        Warning,    // Предупреждения
        Error       // Ошибки
    }

    /// <summary>
    /// Метод для логирования сообщений с различными уровнями важности
    /// </summary>
    /// <param name="logLevel">Уровень логирования</param>
    /// <param name="module">Имя модуля, из которого происходит логирование</param>
    /// <param name="message">Сообщение для логирования</param>
    public static void Log(LogLevel logLevel, string module, string message)
    {
        // Форматируем дату и время для лога
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Форматируем строку для логирования
        string logMessage = $"{timestamp} [{logLevel}] {module}: {message}";

        // Логируем в консоль для разработчиков
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

        // Сохраняем сообщение в файл
        SaveLogToFile(logMessage);
    }

    /// <summary>
    /// Метод для сохранения логов в файл
    /// </summary>
    /// <param name="logMessage">Сообщение, которое нужно сохранить</param>
    private static void SaveLogToFile(string logMessage)
    {
        try
        {
            // Создаем директорию, если она не существует
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

            // Открываем файл в режиме добавления
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(logMessage); // Записываем лог в файл
            }
        }
        catch (Exception ex)
        {
            // Логируем ошибку, если не удалось сохранить лог в файл
            Debug.LogError($"Failed to write log to file: {ex.Message}!");
        }
    }
}
