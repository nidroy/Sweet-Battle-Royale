using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Logger
{
    // Путь к файлу для хранения логов
    private static readonly string _logFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Sweet-Battle-Royale",
        "log.txt");

    // Максимальный размер файла логов в байтах (5 MB)
    private const int _maxLogFileSize = 5 * 1024 * 1024;

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

        // Выводим лог в консоль в зависимости от уровня логирования
#if UNITY_EDITOR
        switch (logLevel)
        {
            case LogLevel.Info:
                Debug.Log(logMessage);        // Логирование информационного сообщения
                break;
            case LogLevel.Warning:
                Debug.LogWarning(logMessage); // Логирование предупреждения
                break;
            case LogLevel.Error:
                Debug.LogError(logMessage);   // Логирование ошибки
                break;
        }
#endif
        // Асинхронно сохраняем сообщение в файл
        if (Settings.IsFileLogging)
        {
            _ = SaveLogToFileAsync(logMessage);
        }
    }

    /// <summary>
    /// Метод для удаления файла логов
    /// </summary>
    public static void DeleteLogFile()
    {
        try
        {
            if (File.Exists(_logFilePath))
            {
                File.Delete(_logFilePath);
#if UNITY_EDITOR
                Debug.Log("Log file deleted successfully.");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning("Log file does not exist!");
#endif
            }
        }
        catch (Exception ex)
        {
            // Логируем ошибку, если не удалось удалить файл
#if UNITY_EDITOR
            Debug.LogError($"Failed to delete log file: {ex.Message}!");
#endif
        }
    }

    /// <summary>
    /// Асинхронный метод для сохранения логов в файл
    /// </summary>
    /// <param name="logMessage">Сообщение, которое нужно сохранить</param>
    private static async Task SaveLogToFileAsync(string logMessage)
    {
        try
        {
            // Создаем директорию, если она не существует
            Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));

            // Проверяем размер файла логов; если превышен лимит, удаляем файл для очистки
            if (new FileInfo(_logFilePath).Length > _maxLogFileSize)
            {
                File.Delete(_logFilePath);
            }

            // Открываем файл в режиме добавления и записываем лог, используя UTF-8 для кодировки
            using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.UTF8))
            {
                await writer.WriteLineAsync(logMessage); // Асинхронная запись строки в файл
            }
        }
        catch (Exception ex)
        {
            // Логируем ошибку, если не удалось сохранить лог в файл
#if UNITY_EDITOR
            Debug.LogError($"Failed to write log to file: {ex.Message}!");
#endif
        }
    }
}
