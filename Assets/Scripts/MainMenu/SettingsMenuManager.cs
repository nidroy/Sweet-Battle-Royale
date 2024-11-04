using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _settingsMenuObject; // Объект, отображающий меню настроек
    [SerializeField]
    private TMP_Dropdown _screenResolutionDropdown; // Поле для выбора разрешения экрана
    [SerializeField]
    private Toggle _fullScreenToggle; // Чекбокс для переключения полноэкранного режима
    [SerializeField]
    private Slider _musicVolumeSlider; // Слайдер для управления громкостью музыки
    [SerializeField]
    private TMP_Text _musicVolumeText; // Текстовое поле для отображения значения громкости музыки
    [SerializeField]
    private AudioSource _musicSource; // Источник звука для музыки

    // Структура для хранения разрешения экрана
    private struct ScreenResolution
    {
        public int Width; // Ширина разрешения
        public int Height; // Высота разрешения
        public string DisplayString => $"{Width}x{Height}"; // Строковое представление разрешения

        public ScreenResolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }

    // Массив доступных разрешений экрана формата 16:9
    private readonly ScreenResolution[] _screenResolutions = new ScreenResolution[]
    {
        new ScreenResolution(1920, 1080), // Full HD
        new ScreenResolution(1600, 900),  // HD+
        new ScreenResolution(1366, 768),  // HD+
        new ScreenResolution(1280, 720),  // HD
        new ScreenResolution(2560, 1440), // QHD
        new ScreenResolution(3840, 2160), // 4K
        new ScreenResolution(640, 360)    // SD
    };

    #region Публичные методы

    /// <summary>
    /// Метод для инициализации настроек
    /// </summary>
    public void Init()
    {
        LoadSettings(); // Загрузка настроек из файла
        ApplySettings(); // Применение текущих настроек
        LogInfo("Settings initialized successfully.");
    }

    /// <summary>
    /// Обработчик нажатия кнопки для открытия меню настроек и загрузки текущих настроек из файла.
    /// </summary>
    public void OnOpenSettingsMenuButtonClick()
    {
        LoadSettings(); // Загрузка настроек из файла
        LoadScreeResolutionsToDropdown(); // Загрузка доступных разрешений экрана в выпадающий список
        UpdateFullScreenToggle(); // Обновление состояния чекбокса
        UpdateMusicVolumeSlider(); // Обновление значения слайдера громкости музыки
        ApplySettings(); // Применение текущих настроек
        _settingsMenuObject.SetActive(true); // Отображение меню настроек
        LogInfo("Settings menu opened.");
    }

    /// <summary>
    /// Обработчик нажатия кнопки для сохранения текущих настроек и закрытия меню.
    /// </summary>
    public void OnSaveSettingsButtonClick()
    {
        SaveSettings(); // Сохранение настроек в файл
        _settingsMenuObject.SetActive(false); // Закрытие меню настроек
        LogInfo("Settings saved and menu closed.");
    }

    /// <summary>
    /// Обработчик нажатия кнопки для закрытия меню настроек без сохранения.
    /// </summary>
    public void OnCloseSettingsMenuButtonClick()
    {
        LoadSettings(); // Загрузка настроек из файла
        ApplySettings(); // Применение текущих настроек
        _settingsMenuObject.SetActive(false); // Закрытие меню настроек
        LogInfo("Settings menu closed without saving.");
    }

    /// <summary>
    /// Обработчик изменения состояния чекбокса для переключения полноэкранного режима.
    /// </summary>
    public void OnFullScreenToggleChanged()
    {
        Settings.IsFullScreen = _fullScreenToggle.isOn; // Сохраняем текущее состояние полноэкранного режима в настройках
        Settings.ApplyFullScreen(); // Применяем состояние полноэкранного режима
        LogInfo($"Full screen mode set to: {Settings.IsFullScreen}.");
    }

    /// <summary>
    /// Обработчик изменения громкости музыки слайдером.
    /// </summary>
    public void OnMusicVolumeSliderChanged()
    {
        Settings.MusicVolume = _musicVolumeSlider.value; // Сохраняем текущее значение громкости в настройках
        Settings.ApplyMusicVolume(_musicSource); // Применяем выбранную громкость музыки
        UpdateMusicVolumeText(); // Обновляем текстовое поле громкости
        LogInfo($"Music volume set to: {Settings.MusicVolume}.");
    }

    #endregion

    #region Приватные методы

    /// <summary>
    /// Метод для загрузки настроек из файла.
    /// </summary>
    private void LoadSettings()
    {
        Settings.Load(); // Загрузка настроек
        LogInfo("Settings loaded successfully.");
    }

    /// <summary>
    /// Метод для сохранения текущих настроек в файл.
    /// </summary>
    private void SaveSettings()
    {
        Settings.Save(); // Сохранение настроек
        LogInfo("Settings saved to file.");
    }

    /// <summary>
    /// Метод для применения текущих настроек.
    /// </summary>
    private void ApplySettings()
    {
        Settings.ApplyScreenResolution(); // Применение разрешения экрана из настроек
        Settings.ApplyFullScreen(); // Применение состояния полноэкранного режима
        Settings.ApplyMusicVolume(_musicSource); // Применение громкости музыки
        LogInfo("Settings applied successfully.");
    }

    /// <summary>
    /// Метод для загрузки доступных разрешений экрана в выпадающий список.
    /// </summary>
    private void LoadScreeResolutionsToDropdown()
    {
        _screenResolutionDropdown.ClearOptions(); // Очистка текущих опций

        List<string> options = new List<string>();
        foreach (var res in _screenResolutions)
        {
            options.Add(res.DisplayString); // Добавление нового разрешения экрана
        }
        _screenResolutionDropdown.AddOptions(options); // Добавление новых опций

        int currentIndex = GetScreeResolutionIndex(Settings.ScreenResolution); // Получаем текущий индекс разрешения экрана
        _screenResolutionDropdown.value = currentIndex; // Устанавливаем выбранное значение
        _screenResolutionDropdown.RefreshShownValue(); // Обновление отображаемого значения

        // Удаление предыдущих обработчиков событий и добавление нового
        _screenResolutionDropdown.onValueChanged.RemoveAllListeners();
        _screenResolutionDropdown.onValueChanged.AddListener(OnChangeScreenResolution);
    }

    /// <summary>
    /// Обработчик изменения разрешения экрана из выпадающего списка.
    /// </summary>
    /// <param name="index">Индекс выбранного разрешения экрана</param>
    private void OnChangeScreenResolution(int index)
    {
        Settings.ScreenResolution = _screenResolutions[index].DisplayString; // Сохраняем текущее разрешение экрана в настройках
        Settings.ApplyScreenResolution(); // Применяем выбранное разрешение экрана
        LogInfo($"Screen resolution changed to: {Settings.ScreenResolution}.");
    }

    /// <summary>
    /// Метод для получения индекса указанного разрешения экрана в списке доступных разрешений.
    /// </summary>
    /// <param name="resolution">Строка указанного разрешения экрана</param>
    /// <returns>Индекс указанного разрешения экрана</returns>
    private int GetScreeResolutionIndex(string resolution)
    {
        for (int i = 0; i < _screenResolutions.Length; i++)
        {
            if (_screenResolutions[i].DisplayString == resolution)
                return i; // Возвращаем индекс, если совпадение найдено
        }
        return 0; // По умолчанию первый индекс
    }

    /// <summary>
    /// Метод для обновления состояния чекбокса полноэкранного режима.
    /// </summary>
    private void UpdateFullScreenToggle()
    {
        _fullScreenToggle.isOn = Settings.IsFullScreen; // Установка состояния чекбокса
    }

    /// <summary>
    /// Метод для обновления текущего значения слайдера громкости музыки.
    /// </summary>
    private void UpdateMusicVolumeSlider()
    {
        _musicVolumeSlider.value = Settings.MusicVolume; // Установка значения слайдера громкости музыки
        UpdateMusicVolumeText(); // Обновление текста с громкостью
    }

    /// <summary>
    /// Метод для обновления текстового поля с текущей громкостью музыки.
    /// </summary>
    private void UpdateMusicVolumeText()
    {
        _musicVolumeText.text = $"{Settings.MusicVolume.ToString("F0")}"; // Обновляем текст, показывая громкость в процентах
    }

    /// <summary>
    /// Метод для логирования ошибок.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(SettingsMenuManager), message);
    }

    /// <summary>
    /// Метод для логирования предупреждений.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(SettingsMenuManager), message);
    }

    /// <summary>
    /// Метод для логирования информационных сообщений.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(SettingsMenuManager), message);
    }

    #endregion
}
