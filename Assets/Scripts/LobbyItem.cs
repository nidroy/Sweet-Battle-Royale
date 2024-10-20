using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _lobbyName; // ��������� ���� ��� ����������� ����� �����
    [SerializeField]
    private TMP_Text _playerCount; // ��������� ���� ��� ����������� ���������� �������
    [SerializeField]
    private Button _selectLobbyButton; // ������ ��� ������ �����

    private TMP_InputField _menuLobbyName; // ���� ��� ����� �������� ����� � ����
    private bool _isLobbySelected; // ���� ��� ��������, ����� �� ������� ��� �����

    /// <summary>
    /// ����� ��� ������������� �������� �����
    /// </summary>
    /// <param name="menuLobbyName">���� ��� ����� �������� ����� � ����</param>
    /// <param name="lobbyName">��� �����</param>
    /// <param name="numPlayers">������� ���������� ������� � �����</param>
    /// <param name="maxPlayers">������������ ���������� ������� � �����</param>
    public void Init(TMP_InputField menuLobbyName, string lobbyName, int numPlayers, int maxPlayers)
    {
        // ������������� ��� ����� � ���������� �������
        _lobbyName.text = lobbyName;
        _playerCount.text = $"{numPlayers}/{maxPlayers}";

        // ��������� ������ �� ��������� ���� � ���� ��� ���������� �����
        _menuLobbyName = menuLobbyName;

        // ����������, ����� �� ������� ��� ����� (���� ��� �� ���������)
        _isLobbySelected = numPlayers < maxPlayers;

        // ������������ �� ���������� �������, ���� ��� ���� ���������
        _selectLobbyButton.onClick.RemoveAllListeners();
        // ����������� ���������� �������
        _selectLobbyButton.onClick.AddListener(OnSelectLobbyButtonClick);
    }

    /// <summary>
    /// ���������� ������� �� ������ ������ �����
    /// </summary>
    private void OnSelectLobbyButtonClick()
    {
        // ���� ����� ������� ��� �����, ��������� ��������� ���� � ����
        if (_isLobbySelected)
        {
            _menuLobbyName.text = _lobbyName.text;
        }
        else
        {
            // �������� ���������, ���� ����� ���������
            Logger.Log(Logger.LogLevel.Info, "LobbyItem", "Cannot select this lobby, it is full.");
        }
    }
}
