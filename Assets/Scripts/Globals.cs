using Photon.Realtime;
using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class Globals
{
    // Значение состояния загрузки сцены
    private static bool _isSceneLoading = true;

    // Свойство для доступа к состоянию загрузки сцены
    public static bool IsSceneLoading
    {
        get => _isSceneLoading;
        set => _isSceneLoading = value;
    }

    // Коллекция для хранения информации обо всех доступных лобби
    private static readonly List<RoomInfo> _lobbyList = new List<RoomInfo>();

    // Свойство для получения списка лобби в режиме только для чтения
    public static ReadOnlyCollection<RoomInfo> LobbyList => _lobbyList.AsReadOnly();

    /// <summary>
    /// Метод для обновления списка лобби.
    /// </summary>
    /// <param name="newLobbyList">Новый список лобби</param>
    public static void UpdateLobbyList(IEnumerable<RoomInfo> newLobbyList)
    {
        if (newLobbyList == null)
        {
            LogError("The provided lobby list is null!");
            return;
        }

        _lobbyList.Clear();
        _lobbyList.AddRange(newLobbyList);
    }

    // Имя игрока
    private static string _playerName = string.Empty;

    // Свойство для получения и установки имени игрока
    public static string PlayerName
    {
        get => _playerName;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                _playerName = value;
            }
            else
            {
                LogError("Player name cannot be null or empty!");
            }
        }
    }

    /// <summary>
    /// Метод для логирования ошибок.
    /// </summary>
    /// <param name="message">Сообщение для логирования</param>
    private static void LogError(string message)
    {
        Logger.Log(Logger.LogLevel.Error, nameof(Globals), message);
    }
}