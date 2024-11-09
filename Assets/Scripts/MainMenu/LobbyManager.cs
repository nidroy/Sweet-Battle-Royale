using Photon.Pun;
using Photon.Realtime;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField _playerNameInputField; // Поле для ввода имени игрока
    [SerializeField]
    private TMP_InputField _lobbyNameInputField; // Поле для ввода названия лобби
    [SerializeField]
    private Transform _lobbyListContent; // Контейнер для отображения списка лобби
    [SerializeField]
    private LobbyItem _lobbyItemPrefab; // Префаб лобби для отображения в списке

    [SerializeField]
    private MainMenuManager _mainMenuManager; // Ссылка на MainMenuManager

    #region Публичные методы

    /// <summary>
    /// Обработчик нажатия кнопки создания лобби.
    /// </summary>
    public void OnCreateLobbyButtonClick()
    {
        if (IsLobbyNameValid())
        {
            CreateLobby(_lobbyNameInputField.text, 4);
            LoadGameScene();
        }
    }

    /// <summary>
    /// Обработчик нажатия кнопки присоединения к лобби.
    /// </summary>
    public void OnJoinLobbyButtonClick()
    {
        if (IsLobbyNameValid())
        {
            JoinLobby(_lobbyNameInputField.text);
            LoadGameScene();
        }
    }

    /// <summary>
    /// Метод для обновления списка лобби в UI.
    /// </summary>
    /// <param name="lobbyList">Список доступных лобби</param>
    public void UpdateLobbyList(ReadOnlyCollection<RoomInfo> lobbyList)
    {
        // Проверка на наличие лобби в списке. Если их нет, выводим предупреждение и выходим из метода.
        if (lobbyList == null || lobbyList.Count == 0)
        {
            LogWarning("No lobbies to display!");
            return;
        }

        // Удаляем старые элементы лобби, чтобы предотвратить дублирование в UI
        foreach (Transform lobby in _lobbyListContent)
        {
            Destroy(lobby.gameObject);
        }

        // Создаем новые элементы лобби для каждого доступного лобби в списке
        foreach (var lobby in lobbyList)
        {
            // Проверка на актуальность данных о лобби перед добавлением (например, если лобби закрыто, пропускаем его)
            if (lobby.RemovedFromList) continue;

            // Создаем новый элемент лобби и инициализируем его с актуальными данными
            LobbyItem lobbyItem = Instantiate(_lobbyItemPrefab, _lobbyListContent);
            lobbyItem.Init(_lobbyNameInputField, lobby.Name, lobby.PlayerCount, lobby.MaxPlayers);
        }
    }

    #endregion

    #region Приватные методы

    /// <summary>
    /// Метод для проверки корректности имени лобби.
    /// </summary>
    /// <returns>True, если имя лобби валидно; иначе false</returns>
    private bool IsLobbyNameValid()
    {
        if (string.IsNullOrWhiteSpace(_lobbyNameInputField.text))
        {
            LogWarning("Lobby name cannot be empty!");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Метод для создания нового лобби.
    /// </summary>
    /// <param name="lobbyName">Название лобби</param>
    /// <param name="maxPlayers">Максимальное количество игроков в лобби</param>
    private void CreateLobby(string lobbyName, byte maxPlayers)
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = maxPlayers }; // Создание объекта RoomOptions

        PhotonNetwork.CreateRoom(lobbyName, roomOptions); // Создание нового лобби
        LogInfo($"Creating a new lobby: {lobbyName} with max players: {maxPlayers}.");
    }

    /// <summary>
    /// Метод для присоединения к существующему лобби.
    /// </summary>
    /// <param name="lobbyName">Название лобби, к которому нужно присоединиться</param>
    private void JoinLobby(string lobbyName)
    {
        PhotonNetwork.JoinRoom(lobbyName); // Присоединение к лобби с указанным именем
        LogInfo($"Joining the lobby: {lobbyName}.");
    }

    /// <summary>
    /// Метод для загрузки игровой сцены.
    /// </summary>
    private void LoadGameScene()
    {
        if (!Globals.IsSceneLoading)
        {
            Globals.IsSceneLoading = true;
            // Показываем экран загрузки через MainMenuManager
            _mainMenuManager.ShowScreen(_mainMenuManager.LoadingObject);
            LogInfo("Loading GameScene...");
        }
    }

    /// <summary>
    /// Метод для логирования ошибок.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(LobbyManager), message);
    }

    /// <summary>
    /// Метод для логирования предупреждений.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogWarning(string message)
    {
        Logger.Log(Logger.LogLevel.Warning, nameof(LobbyManager), message);
    }

    /// <summary>
    /// Метод для логирования информационных сообщений.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private void LogInfo(string message)
    {
        Logger.Log(Logger.LogLevel.Info, nameof(LobbyManager), message);
    }

    #endregion

    #region Callbacks

    /// <summary>
    /// Callback при успешном создании лобби.
    /// </summary>
    public override void OnCreatedRoom()
    {
        LogInfo("Successfully created a lobby.");
    }

    /// <summary>
    /// Callback при успешном присоединении к лобби.
    /// </summary>
    public override void OnJoinedRoom()
    {
        LogInfo("Successfully joined the lobby.");
        Globals.PlayerName = _playerNameInputField.text; // Сохраняем имя игрока
        PhotonNetwork.LoadLevel("GameScene"); // Загрузка сцены с именем "GameScene"
        LogInfo("GameScene is loaded.");
    }

    /// <summary>
    /// Callback при неудачном создании лобби.
    /// </summary>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        LogError($"{returnCode}: Failed to create a lobby: {message}!");
    }

    /// <summary>
    /// Callback при неудачном присоединении к лобби.
    /// </summary>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        LogError($"{returnCode}: Failed to join the lobby: {message}!");
    }

    #endregion
}