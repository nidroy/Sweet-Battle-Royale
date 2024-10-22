using Photon.Realtime;
using System.Collections.Generic;

// Класс для хранения глобальных переменных и свойств
public class Globals
{
    // Значение состояния загрузки сцены
    private static bool _isSceneLoading = true;

    // Свойство для доступа к состоянию загрузки сцены
    public static bool IsSceneLoading
    {
        get
        {
            // Возвращаем текущее состояние загрузки сцены
            return _isSceneLoading;
        }
        set
        {
            // Проверяем, изменилось ли состояние загрузки
            if (_isSceneLoading != value)
            {
                // Устанавливаем новое значение состояния загрузки сцены
                _isSceneLoading = value;
            }
            else
            {
                // Логирование попытки установить текущее значение
                Logger.Log(Logger.LogLevel.Warning, "LobbyManager", "Attempted to set IsSceneLoading to its current value!");
            }
        }
    }


    // Значение состояния музыки
    private static bool _isMusicMuted = true;

    // Свойство для доступа к состоянию музыки
    public static bool IsMusicMuted
    {
        get
        {
            // Возвращаем текущее состояние музыки
            return _isMusicMuted;
        }
        set
        {
            // Проверка, изменилось ли состояние музыки
            if (_isMusicMuted != value)
            {
                // Устанавливаем новое значение состояния музыки
                _isMusicMuted = value;
            }
            else
            {
                // Логирование попытки установить текущее значение
                Logger.Log(Logger.LogLevel.Warning, "Globals", "Attempted to set IsMusicMuted to its current value!");
            }
        }
    }

    // Список существующих лобби
    private static List<RoomInfo> _lobbyList = new List<RoomInfo>();

    // Свойство для доступа к списку лобби
    public static List<RoomInfo> LobbyList
    {
        get
        {
            // Возвращаем текущий список лобби
            return _lobbyList;
        }
        set
        {
            // Проверка, что переданный список не равен null
            if (value != null)
            {
                // Очищаем список перед добавлением нового списка лобби
                _lobbyList.Clear();

                // Добавляем все элементы из переданного списка
                _lobbyList.AddRange(value);
            }
            else
            {
                // Логирование ошибки, если передан null
                Logger.Log(Logger.LogLevel.Error, "Globals", "The provided lobby list is null!");
            }
        }
    }

    // Значение имени игрока
    private static string _playerName = "";

    // Свойство для доступа к имени игрока
    public static string PlayerName
    {
        get
        {
            // Возвращаем текущее значение имени игрока
            return _playerName;
        }
        set
        {
            // Проверка, что имя не является null или пустой строкой
            if (!string.IsNullOrEmpty(value))
            {
                // Устанавливаем новое значение для имени игрока
                _playerName = value;
            }
            else
            {
                // Логирование ошибки, если имя некорректное
                Logger.Log(Logger.LogLevel.Error, "Globals", "Player name cannot be null or empty!");
            }
        }
    }
}
