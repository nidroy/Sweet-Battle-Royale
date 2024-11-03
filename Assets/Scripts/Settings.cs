using System.IO;
using System;
using UnityEngine;

public static class Settings
{
    // Путь к файлу настроек
    private static readonly string _settingsFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "Sweet-Battle-Royale",
        "settings.json"
    );

    /// <summary>
    /// Класс для хранения данных настроек.
    /// </summary>
    [Serializable]
    private class SettingsData
    {
        public string ScreenResolution = "1920x1080";   // Разрешение экрана, по умолчанию 1920x1080
        public bool IsFullScreen = true;                // Значение состояния полноэкранного режима, по умолчанию включен
        public float MusicVolume = 100f;                // Громкость музыки от 0 до 100, по умолчанию 100
    }

    // Объект для хранения текущих значений настроек
    private static SettingsData _currentSettings = new SettingsData();

    // Свойство для получения и установки разрешения экрана
    public static string ScreenResolution
    {
        get => _currentSettings.ScreenResolution;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                _currentSettings.ScreenResolution = value;
            else
                LogError("Screen resolution cannot be null or empty!");
        }
    }

    // Свойство для доступа к состоянию полноэкранного режима
    public static bool IsFullScreen
    {
        get => _currentSettings.IsFullScreen;
        set => _currentSettings.IsFullScreen = value;
    }

    // Свойство для получения и установки громкости музыки (от 0 до 100)
    public static float MusicVolume
    {
        get => _currentSettings.MusicVolume;
        set => _currentSettings.MusicVolume = Mathf.Clamp(value, 0, 100);
    }

    /// <summary>
    /// Метод для сохранения текущих настроек в файл JSON.
    /// </summary>
    public static void SaveSettings()
    {
        try
        {
            // Создание директории, если она не существует
            Directory.CreateDirectory(Path.GetDirectoryName(_settingsFilePath));
            // Сериализация объекта настроек в строку JSON
            string json = JsonUtility.ToJson(_currentSettings);
            // Запись JSON-строки в файл настроек
            File.WriteAllText(_settingsFilePath, json);
            LogInfo("Settings saved successfully.");
        }
        catch (Exception ex)
        {
            LogError($"Failed to save settings: {ex.Message}");
        }
    }

    /// <summary>
    /// Метод для загрузки настроек из файла JSON.
    /// </summary>
    public static void LoadSettings()
    {
        try
        {
            if (File.Exists(_settingsFilePath))
            {
                // Чтение содержимого файла настроек в формате JSON
                string json = File.ReadAllText(_settingsFilePath);
                // Десериализация строки JSON в объект настроек
                _currentSettings = JsonUtility.FromJson<SettingsData>(json) ?? new SettingsData();
                LogInfo("Settings loaded successfully.");
            }
            else
            {
                // Установка значений по умолчанию и создание файла, если он отсутствует
                SetDefaultSettings();
                SaveSettings();
                LogWarning("Settings file not found. Created a new one with default values.");
            }
        }
        catch (Exception ex)
        {
            LogError($"Failed to load settings: {ex.Message}");
        }
    }

    /// <summary>
    /// Метод для установки настроек по умолчанию.
    /// </summary>
    public static void SetDefaultSettings()
    {
        // Создание нового экземпляра настроек со значениями по умолчанию
        _currentSettings = new SettingsData();
        LogInfo("Default settings applied.");
    }

    /// <summary>
    /// Метод для логирования ошибок.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(Settings), message);
    }

    /// <summary>
    /// Метод для логирования предупреждений.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(Settings), message);
    }

    /// <summary>
    /// Метод для логирования информационных сообщений.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(Settings), message);
    }
}
