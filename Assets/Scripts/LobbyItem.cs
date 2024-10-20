using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _lobbyName; // Текстовое поле для отображения имени лобби
    [SerializeField]
    private TMP_Text _playerCount; // Текстовое поле для отображения количества игроков
    [SerializeField]
    private Button _selectLobbyButton; // Кнопка для выбора лобби

    private TMP_InputField _menuLobbyName; // Поле для ввода названия лобби в меню
    private bool _isLobbySelected; // Флаг для проверки, можно ли выбрать это лобби

    /// <summary>
    /// Метод для инициализации элемента лобби
    /// </summary>
    /// <param name="menuLobbyName">Поле для ввода названия лобби в меню</param>
    /// <param name="lobbyName">Имя лобби</param>
    /// <param name="numPlayers">Текущее количество игроков в лобби</param>
    /// <param name="maxPlayers">Максимальное количество игроков в лобби</param>
    public void Init(TMP_InputField menuLobbyName, string lobbyName, int numPlayers, int maxPlayers)
    {
        // Устанавливаем имя лобби и количество игроков
        _lobbyName.text = lobbyName;
        _playerCount.text = $"{numPlayers}/{maxPlayers}";

        // Сохраняем ссылку на текстовое поле в меню для выбранного лобби
        _menuLobbyName = menuLobbyName;

        // Определяем, можно ли выбрать это лобби (если оно не заполнено)
        _isLobbySelected = numPlayers < maxPlayers;

        // Отписываемся от предыдущих событий, если они были подписаны
        _selectLobbyButton.onClick.RemoveAllListeners();
        // Подписываем обработчик события
        _selectLobbyButton.onClick.AddListener(OnSelectLobbyButtonClick);
    }

    /// <summary>
    /// Обработчик нажатия на кнопку выбора лобби
    /// </summary>
    private void OnSelectLobbyButtonClick()
    {
        // Если можно выбрать это лобби, заполняем текстовое поле в меню
        if (_isLobbySelected)
        {
            _menuLobbyName.text = _lobbyName.text;
        }
        else
        {
            // Логируем сообщение, если лобби заполнено
            Logger.Log(Logger.LogLevel.Info, "LobbyItem", "Cannot select this lobby, it is full.");
        }
    }
}
