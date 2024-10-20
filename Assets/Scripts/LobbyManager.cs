using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField _playerName; // ���� ��� ����� ����� ������
    [SerializeField]
    private TMP_InputField _lobbyName; // ���� ��� ����� �������� �����

    [SerializeField]
    private Transform _lobbyListContent; // ��������� ��� ����������� ������ �����

    [SerializeField]
    private LobbyItem _lobbyPrefab; // ������ ����� ��� ����������� � ������

    /// <summary>
    /// ����� ��� ������� ������ �������� �����
    /// </summary>
    public void CreateLobbyButton_Click()
    {
        CreateLobby(_lobbyName.text, 4);
    }

    /// <summary>
    /// ����� ��� ������� ������ ������������� � �����
    /// </summary>
    public void JoinLobbyButton_Click()
    {
        JoinLobby(_lobbyName.text);
    }

    /// <summary>
    /// ����� ��� ���������� ������ �����
    /// </summary>
    /// <param name="lobbyList">������ ��������� �����</param>
    public void UpdateLobbyList(List<RoomInfo> lobbyList)
    {
        // ���������, �� �������� �� ������ null ��� ������
        if (lobbyList == null || lobbyList.Count == 0)
        {
            Logger.Log(Logger.LogLevel.Warning, "LobbyManager", "No lobbies to display!");
            return;
        }

        // ������� ������ �������� ������
        foreach (Transform lobby in _lobbyListContent)
        {
            Destroy(lobby.gameObject); // ������� ������ �������� ������
        }

        // ������� �������� ��� ������� ����� � ������
        foreach (var lobby in lobbyList)
        {
            LobbyItem lobbyItem = Instantiate(_lobbyPrefab, _lobbyListContent);
            lobbyItem.Init(_lobbyName, lobby.Name, lobby.PlayerCount, lobby.MaxPlayers);
        }
    }

    /// <summary>
    /// ����� ��� �������� ������ �����
    /// </summary>
    /// <param name="lobbyName">�������� �����</param>
    /// <param name="maxPlayers">������������ ���������� ������� � �����</param>
    private void CreateLobby(string lobbyName, byte maxPlayers)
    {
        // �������� ������� RoomOptions ��� ��������� ���������� �����
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers; // ������������ ���������� ������� � �����

        // �������� ������ ����� � ���������� �����������
        PhotonNetwork.CreateRoom(lobbyName, roomOptions);
        // ����������� ���������� � �������� �����
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", $"Creating a new lobby: {lobbyName} with max players: {maxPlayers}.");
    }

    /// <summary>
    /// ����� ��� ������������� � ������������� �����
    /// </summary>
    /// <param name="lobbyName">�������� �����, � �������� ����� ��������������</param>
    private void JoinLobby(string lobbyName)
    {
        // ������������� � ����� � ��������� ������
        PhotonNetwork.JoinRoom(lobbyName);
        // ����������� ���������� � ������������� � �����
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", $"Joining the lobby: {lobbyName}.");
    }

    /// <summary>
    /// Callback ��� �������� �������� �����
    /// </summary>
    public override void OnCreatedRoom()
    {
        // ����������� ��������� �������� �����
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", "Successfully created a lobby.");
    }

    /// <summary>
    /// Callback ��� �������� ������������� � �����
    /// </summary>
    public override void OnJoinedRoom()
    {
        // ����������� ��������� ������������� � �����
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", "Successfully joined the lobby.");

        // ��������� ��� ������
        Globals.PlayerName = _playerName.text;

        // ������� �� ����� GameScene
        Logger.Log(Logger.LogLevel.Info, "LobbyManager", "Loading GameScene...");
        PhotonNetwork.LoadLevel("GameScene"); // �������� ����� � ������ "GameScene"
    }

    /// <summary>
    /// Callback ��� ��������� �������� �����
    /// </summary>
    /// <param name="returnCode">��� ������</param>
    /// <param name="message">��������� �� ������</param>
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // ����������� ������ ��� �������� �����
        Logger.Log(Logger.LogLevel.Error, "LobbyManager", $"{returnCode}: Failed to create a lobby: {message}!");
    }

    /// <summary>
    /// Callback ��� ��������� ������������� � �����
    /// </summary>
    /// <param name="returnCode">��� ������</param>
    /// <param name="message">��������� �� ������</param>
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // ����������� ������ ��� ������������� � �����
        Logger.Log(Logger.LogLevel.Error, "LobbyManager", $"{returnCode}: Failed to join the lobby: {message}!");
    }
}
