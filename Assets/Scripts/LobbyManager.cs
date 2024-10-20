using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField _playerName; // Поле для ввода имени игрока
    [SerializeField]
    private TMP_InputField _lobbyName; // Поле для ввода названия лобби

    [SerializeField]
    private Transform _lobbyListContent; // Контейнер для отображения списка лобби

    [SerializeField]
    private LobbyItem _lobbyPrefab; // Префаб лобби для отображения в списке

    /// <summary>
    /// Метод для нажатия кнопки создания лобби
    /// </summary>
    public void CreateLobbyButton_Click()
    {
        CreateLobby(_lobbyName.text, 4);
    }

    /// <summary>
    /// Метод для нажатия кнопки присоединения к лобби
    /// </summary>
    public void JoinLobbyButton_Click()
    {
        JoinLobby(_lobbyName.text);
    }

    /// <summary>
    /// Метод для обновления списка лобби
    /// </summary>
    /// <param name="lobbyList">Список доступных лобби</param>
    public void UpdateLobbyList(List<RoomInfo> lobbyList)
    {
        // Проверяем, не является ли список null или пустым
        if (lobbyList == null || lobbyList.Count == 0)
        {
            Logger.Log(Logger.LogLevel.Warning, "LobbyManager", "No lobbies to display!");
            return;
        }

        // Очищаем старые элементы списка
        foreach (Transform lobby in _lobbyListContent)
        {
            Destroy(lobby.gameObject); // Удаляем старые элементы списка
        }

        // Создаем элементы для каждого лобби в списке
        foreach (var lobby in lobbyList)
        {
            LobbyItem lobbyItem = Instantiate(_lobbyPrefab, _lobbyListContent);
            lobbyItem.Init(_lobbyName, lobby.Name, lobby.PlayerCount, lobby.MaxPlayers);
        }
    }

    /// <summary>
    /// Метод для создания нового лобби
    /// </summary>
    /// <param name="lobbyName">Название лобби</param>
    /// <param name="maxPlayers">Максимальное количество игроков в лобби</param>
    private void CreateLobby(string lobbyName, byte maxPlayers)
    {
        // Создание объекта RoomOptions для настройки параметров лобби
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers; // Максимальное количество игроков в лобби

        // Создание нового лобби с указанными параметрами
        PhotonNetwork.CreateRoom(lobbyName, roomOptions);
        // Логирование информации о создании лобби
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", $"Creating a new lobby: {lobbyName} with max players: {maxPlayers}.");
    }

    /// <summary>
    /// Метод для присоединения к существующему лобби
    /// </summary>
    /// <param name="lobbyName">Название лобби, к которому нужно присоединиться</param>
    private void JoinLobby(string lobbyName)
    {
        // Присоединение к лобби с указанным именем
        PhotonNetwork.JoinRoom(lobbyName);
        // Логирование информации о присоединении к лобби
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", $"Joining the lobby: {lobbyName}.");
    }

    /// <summary>
    /// Callback при успешном создании лобби
    /// </summary>
    public override void OnCreatedRoom()
    {
        // Логирование успешного создания лобби
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", "Successfully created a lobby.");
    }

    /// <summary>
    /// Callback при успешном присоединении к лобби
    /// </summary>
    public override void OnJoinedRoom()
    {
        // Логирование успешного присоединения к лобби
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", "Successfully joined the lobby.");

        // Сохраняем имя игрока
        Globals.PlayerName = _playerName.text;

        // Переход на сцену GameScene
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", "Loading GameScene...");
        PhotonNetwork.LoadLevel("GameScene"); // Загрузка сцены с именем "GameScene"
    }

    /// <summary>
    /// Callback при неудачном создании лобби
    /// </summary>
    /// <param name="returnCode">Код ошибки</param>
    /// <param name="message">Сообщение об ошибке</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // Логирование ошибки при создании лобби
        Logger.Log(Logger.LogLevel.Error, "LobbyManager", $"{returnCode}: Failed to create a lobby: {message}!");
    }

    /// <summary>
    /// Callback при неудачном присоединении к лобби
    /// </summary>
    /// <param name="returnCode">Код ошибки</param>
    /// <param name="message">Сообщение об ошибке</param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // Логирование ошибки при присоединении к лобби
        Logger.Log(Logger.LogLevel.Error, "LobbyManager", $"{returnCode}: Failed to join the lobby: {message}!");
    }
}
