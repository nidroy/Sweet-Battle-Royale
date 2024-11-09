using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _loadingObject; // Объект, отображающий экран загрузки
    [SerializeField]
    private GameObject _lobbyObject; // Объект, отображающий меню лобби

    [SerializeField]
    private SettingsMenuManager _settingsMenuManager; // Ссылка на SettingsMenuManager
    [SerializeField]
    private LobbyManager _lobbyManager; // Ссылка на LobbyManager

    // Свойство для доступа к объекту загрузки
    public GameObject LoadingObject
    {
        get => _loadingObject;
        private set => _loadingObject = value;
    }

    // Свойство для доступа к объекту лобби
    public GameObject LobbyObject
    {
        get => _lobbyObject;
        private set => _lobbyObject = value;
    }

    private void Start()
    {
        _settingsMenuManager.Init(); // Инициализация настроек
        ShowScreen(LoadingObject, LobbyObject); // Показать экран загрузки при старте
        PhotonNetwork.ConnectUsingSettings(); // Подключение к мастер-серверу Photon
    }

    #region Публичные методы

    /// <summary>
    /// Обработчик нажатия кнопки для выхода из игры.
    /// </summary>
    public void OnQuitGameButtonClick()
    {
        LogInfo("Exiting game...");

        // Отключение от сервера Photon, если подключен
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            LogInfo("Disconnected from Photon server.");
        }
        else
        {
            LogInfo("Photon server was not connected.");
        }

        // Завершение приложения
        Application.Quit();

#if UNITY_EDITOR
        // Остановка игры в редакторе Unity
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    /// <summary>
    /// Метод для показа или скрытия экранов.
    /// </summary>
    /// <param name="screenToShow">Экран, который нужно показать</param>
    /// <param name="screenToHide">Экран, который нужно скрыть</param>
    public void ShowScreen(GameObject screenToShow = null, GameObject screenToHide = null)
    {
        if (screenToShow != null)
        {
            screenToShow.SetActive(true); // Активировать экран для показа
            LogInfo($"Showing screen: {screenToShow.name}.");
        }
        else
        {
            LogWarning("screenToShow is null, cannot show the screen!");
        }

        if (screenToHide != null)
        {
            screenToHide.SetActive(false); // Деактивировать экран для скрытия
            LogInfo($"Hiding screen: {screenToHide.name}.");
        }
        else
        {
            LogWarning("screenToHide is null, nothing to hide!");
        }
    }

    #endregion

    #region Приватные методы

    /// <summary>
    /// Метод для логирования предупреждений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(MainMenuManager), message);
    }

    /// <summary>
    /// Метод для логирования информационных сообщений
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(MainMenuManager), message);
    }

    #endregion

    #region Callbacks

    /// <summary>
    /// Callback при успешном подключении к мастер-серверу.
    /// </summary>
    public override void OnConnectedToMaster()
    {
        LogInfo("Successfully connected to the Photon Master Server.");
        PhotonNetwork.JoinLobby(); // Присоединение к лобби для получения списка доступных лобби
    }

    /// <summary>
    /// Callback при обновлении списка комнат.
    /// </summary>
    /// <param name="roomList">Список доступных комнат</param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Globals.UpdateLobbyList(roomList); // Обновляем список лобби
        LogInfo($"Found {Globals.LobbyList.Count} lobbies.");
        LogInfo("Lobby list updated successfully.");

        _lobbyManager.UpdateLobbyList(Globals.LobbyList); // Обновляем отображение списка лобби

        if (Globals.IsSceneLoading)
        {
            ShowScreen(LobbyObject, LoadingObject); // Показываем экран лобби
            Globals.IsSceneLoading = false; // Устанавливаем флаг загрузки в false
            LogInfo("Lobby screen shown, scene loading finished.");
        }
        else
        {
            LogInfo("Cannot show lobby screen, scene is already loaded.");
        }
    }

    #endregion
}