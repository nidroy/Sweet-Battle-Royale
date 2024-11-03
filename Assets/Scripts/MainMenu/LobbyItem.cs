using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _lobbyNameText; // Текстовое поле для отображения имени лобби
    [SerializeField]
    private TMP_Text _playerCountText; // Текстовое поле для отображения количества игроков
    [SerializeField]
    private Button _selectLobbyButton; // Кнопка для выбора лобби

    private TMP_InputField _menuLobbyNameInputField; // Поле для ввода названия лобби в меню
    private bool _isLobbySelected; // Флаг для проверки, можно ли выбрать это лобби

    /// <summary>
    /// Метод для инициализации элемента лобби.
    /// </summary>
    /// <param name="menuLobbyNameInputField">Поле для ввода названия лобби в меню</param>
    /// <param name="lobbyName">Имя лобби</param>
    /// <param name="numPlayers">Текущее количество игроков в лобби</param>
    /// <param name="maxPlayers">Максимальное количество игроков в лобби</param>
    public void Init(TMP_InputField menuLobbyNameInputField, string lobbyName, int numPlayers, int maxPlayers)
    {
        _menuLobbyNameInputField = menuLobbyNameInputField; // Сохраняем ссылку на текстовое поле
        SetLobbyInfo(lobbyName, numPlayers, maxPlayers); // Устанавливаем информацию о лобби
        ConfigureSelectButton(); // Конфигурируем кнопку выбора лобби
    }

    /// <summary>
    /// Метод для установки имени лобби и количества игроков.
    /// </summary>
    /// <param name="lobbyName">Имя лобби</param>
    /// <param name="numPlayers">Текущее количество игроков</param>
    /// <param name="maxPlayers">Максимальное количество игроков</param>
    private void SetLobbyInfo(string lobbyName, int numPlayers, int maxPlayers)
    {
        _lobbyNameText.text = lobbyName;
        _playerCountText.text = $"{numPlayers}/{maxPlayers}";
        _isLobbySelected = CanSelectLobby(numPlayers, maxPlayers); // Определяем, можно ли выбрать это лобби
        UpdateSelectButtonState(); // Обновляем состояние кнопки выбора
    }

    /// <summary>
    /// Метод для проверки, можно ли выбрать лобби.
    /// </summary>
    /// <param name="numPlayers">Текущее количество игроков</param>
    /// <param name="maxPlayers">Максимальное количество игроков</param>
    /// <returns>True, если лобби можно выбрать; иначе false</returns>
    private bool CanSelectLobby(int numPlayers, int maxPlayers)
    {
        return numPlayers < maxPlayers;
    }

    /// <summary>
    /// Метод для конфигурирования кнопки выбора лобби.
    /// </summary>
    private void ConfigureSelectButton()
    {
        _selectLobbyButton.onClick.RemoveAllListeners(); // Отписываемся от предыдущих событий
        _selectLobbyButton.onClick.AddListener(OnSelectLobbyButtonClick); // Подписываем обработчик события
    }

    /// <summary>
    /// Метод для обновления состояния кнопки выбора.
    /// </summary>
    private void UpdateSelectButtonState()
    {
        _selectLobbyButton.interactable = _isLobbySelected; // Делаем кнопку активной, если лобби доступно
    }

    /// <summary>
    /// Обработчик нажатия на кнопку выбора лобби.
    /// </summary>
    private void OnSelectLobbyButtonClick()
    {
        if (_isLobbySelected)
        {
            _menuLobbyNameInputField.text = _lobbyNameText.text; // Заполняем текстовое поле в меню
        }
        else
        {
            LogLobbySelectionError(); // Логируем сообщение, если лобби заполнено
        }
    }

    /// <summary>
    /// Метод для логирования ошибки выбора лобби.
    /// </summary>
    private void LogLobbySelectionError()
    {
        Logger.Log(Logger.LogLevel.Info, nameof(LobbyItem), $"Cannot select the lobby '{_lobbyNameText.text}', it is full.");
    }
}
