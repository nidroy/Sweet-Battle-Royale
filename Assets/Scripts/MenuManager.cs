using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private LobbyManager _lobbyManager; // Ссылка на компонент LobbyManager

    [SerializeField]
    private GameObject _loading; // Объект, отображающий экран загрузки
    [SerializeField]
    private GameObject _lobby; // Объект, отображающий меню лобби

    [SerializeField]
    private Button _musicButton; // Кнопка для управления музыкой

    [SerializeField]
    private Sprite _musicEnabled; // Изображение для состояния, когда музыка включена
    [SerializeField]
    private Sprite _musicDisabled; // Изображение для состояния, когда музыка включена

    [SerializeField]
    private AudioSource _musicSource; // Аудиоисточник для музыки

    private void Start()
    {
        ShowLoadingScreen(); // Показать экран загрузки при старте
        UpdateMusicButtonImage(Globals.IsMusicMuted); // Обновить изображение кнопки в зависимости от состояния музыки
        ToggleMusic(Globals.IsMusicMuted); // Включить или выключить музыку в зависимости от состояния музыки

        PhotonNetwork.ConnectUsingSettings(); // Подключение к мастер-серверу Photon
    }

    /// <summary>
    /// Метод для нажатия кнопки управления музыкой
    /// </summary>
    public void MusicButton_Click()
    {
        // Переключаем состояние музыки
        Globals.IsMusicMuted = !Globals.IsMusicMuted;
        // Обновляем изображение кнопки в зависимости от состояния музыки
        UpdateMusicButtonImage(Globals.IsMusicMuted);
        // Включаем или выключаем музыку в зависимости от состояния музыки
        ToggleMusic(Globals.IsMusicMuted);
    }

    /// <summary>
    /// Callback при успешном подключении к мастер-серверу
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // Логирование успешного подключения
        Logger.Log(Logger.LogLevel.Info, "MenuManager", "Successfully connected to the Photon Master Server.");

        // Получение списка существующих лобби
        PhotonNetwork.JoinLobby(); // Присоединение к лобби для получения списка доступных лобби
    }

    /// <summary>
    /// Callback при обновлении списка комнат
    /// </summary>
    /// <param name="roomList">Список доступных комнат</param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // Добавление нового списка лобби
        Globals.LobbyList = roomList;
        // Логирование количества найденных лобби
        Logger.Log(Logger.LogLevel.Info, "MenuManager", $"Found {Globals.LobbyList.Count} lobbies.");

        // Проверяем, что _lobbyManager назначен и список лобби в Globals не пустой
        if (_lobbyManager != null)
        {
            if (Globals.LobbyList != null)
            {
                // Oбновляем список лобби
                _lobbyManager.UpdateLobbyList(Globals.LobbyList);
                Logger.Log(Logger.LogLevel.Info, "MenuManager", "Lobby list updated successfully.");
                // Показать экран лобби, когда подключение успешно и список лобби успешно получен
                ShowLobbyScreen();
            }
            else
            {
                Logger.Log(Logger.LogLevel.Warning, "MenuManager", "No lobbies available in Globals.LobbyList!");
            }
        }
        else
        {
            Logger.Log(Logger.LogLevel.Error, "MenuManager", "LobbyManager is not assigned!");
        }
    }

    /// <summary>
    /// Метод для обновления изображения кнопки в зависимости от состояния музыки
    /// </summary>
    /// <param name="isMusicEnabled">Состояние музыки</param>
    private void UpdateMusicButtonImage(bool isMusicEnabled)
    {
        // Проверяем, включена ли музыка
        if (isMusicEnabled)
        {
            // Если музыка включена, показываем изображение для включенной музыки
            _musicButton.image.sprite = _musicEnabled;
        }
        else
        {
            // Если музыка выключена, показываем изображение для выключенной музыки
            _musicButton.image.sprite = _musicDisabled;
        }
    }

    /// <summary>
    /// Метод для включения или выключения музыки
    /// </summary>
    /// <param name="isMusicMuted">Состояние музыки</param>
    private void ToggleMusic(bool isMusicMuted)
    {
        // Проверяем, что источник музыки назначен
        if (_musicSource != null)
        {
            if (isMusicMuted)
            {
                // Включение музыки через AudioManager
                AudioManager.EnableSound(_musicSource); // Воспроизводим музыку через аудиоисточник
                // Логирование успешного включения музыки
                Logger.Log(Logger.LogLevel.Info, "MenuManager", "Music enabled.");
            }
            else
            {
                // Выключение музыки через AudioManager
                AudioManager.DisableSound(_musicSource); // Останавливаем воспроизведение музыки
                // Логирование успешного выключения музыки
                Logger.Log(Logger.LogLevel.Info, "MenuManager", "Music disabled.");
            }
        }
        else
        {
            // Логирование ошибки, если источник музыки не назначен
            Logger.Log(Logger.LogLevel.Error, "MenuManager", "MusicSource is not assigned!");
        }
    }

    /// <summary>
    /// Метод для показа экрана загрузки
    /// </summary>
    private void ShowLoadingScreen()
    {
        // Проверка, что объекты загружены, перед их активацией
        if (_loading != null && _lobby != null)
        {
            _loading.SetActive(true); // Активировать экран загрузки
            _lobby.SetActive(false); // Деактивировать экран лобби
        }
        else
        {
            // Сообщение об ошибке
            Logger.Log(Logger.LogLevel.Error, "MenuManager", "Loading or Lobby GameObject is not assigned in the inspector!");
        }
    }

    /// <summary>
    /// Метод для показа экрана лобби
    /// </summary>
    private void ShowLobbyScreen()
    {
        // Проверка, что объекты загружены, перед их активацией
        if (_loading != null && _lobby != null)
        {
            _loading.SetActive(false); // Деактивировать экран загрузки
            _lobby.SetActive(true); // Активировать экран лобби
        }
        else
        {
            // Сообщение об ошибке
            Logger.Log(Logger.LogLevel.Error, "MenuManager", "Loading or Lobby GameObject is not assigned in the inspector!");
        }
    }
}
