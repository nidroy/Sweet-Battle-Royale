using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private LobbyManager _lobbyManager; // ������ �� ��������� LobbyManager

    [SerializeField]
    private GameObject _loading; // ������, ������������ ����� ��������
    [SerializeField]
    private GameObject _lobby; // ������, ������������ ���� �����

    [SerializeField]
    private Button _musicButton; // ������ ��� ���������� �������

    [SerializeField]
    private Sprite _musicEnabled; // ����������� ��� ���������, ����� ������ ��������
    [SerializeField]
    private Sprite _musicDisabled; // ����������� ��� ���������, ����� ������ ��������

    [SerializeField]
    private AudioSource _musicSource; // ������������� ��� ������

    private void Start()
    {
        ShowLoadingScreen(); // �������� ����� �������� ��� ������
        UpdateMusicButtonImage(Globals.IsMusicMuted); // �������� ����������� ������ � ����������� �� ��������� ������
        ToggleMusic(Globals.IsMusicMuted); // �������� ��� ��������� ������ � ����������� �� ��������� ������

        PhotonNetwork.ConnectUsingSettings(); // ����������� � ������-������� Photon
    }

    /// <summary>
    /// ����� ��� ������� ������ ���������� �������
    /// </summary>
    public void MusicButton_Click()
    {
        // ����������� ��������� ������
        Globals.IsMusicMuted = !Globals.IsMusicMuted;
        // ��������� ����������� ������ � ����������� �� ��������� ������
        UpdateMusicButtonImage(Globals.IsMusicMuted);
        // �������� ��� ��������� ������ � ����������� �� ��������� ������
        ToggleMusic(Globals.IsMusicMuted);
    }

    /// <summary>
    /// Callback ��� �������� ����������� � ������-�������
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // ����������� ��������� �����������
        Logger.Log(Logger.LogLevel.Info, "MenuManager", "Successfully connected to the Photon Master Server.");

        // ��������� ������ ������������ �����
        PhotonNetwork.JoinLobby(); // ������������� � ����� ��� ��������� ������ ��������� �����
    }

    /// <summary>
    /// Callback ��� ���������� ������ ������
    /// </summary>
    /// <param name="roomList">������ ��������� ������</param>
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ���������� ������ ������ �����
        Globals.LobbyList = roomList;
        // ����������� ���������� ��������� �����
        Logger.Log(Logger.LogLevel.Info, "MenuManager", $"Found {Globals.LobbyList.Count} lobbies.");

        // ���������, ��� _lobbyManager �������� � ������ ����� � Globals �� ������
        if (_lobbyManager != null)
        {
            if (Globals.LobbyList != null)
            {
                // O�������� ������ �����
                _lobbyManager.UpdateLobbyList(Globals.LobbyList);
                Logger.Log(Logger.LogLevel.Info, "MenuManager", "Lobby list updated successfully.");
                // �������� ����� �����, ����� ����������� ������� � ������ ����� ������� �������
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
    /// ����� ��� ���������� ����������� ������ � ����������� �� ��������� ������
    /// </summary>
    /// <param name="isMusicEnabled">��������� ������</param>
    private void UpdateMusicButtonImage(bool isMusicEnabled)
    {
        // ���������, �������� �� ������
        if (isMusicEnabled)
        {
            // ���� ������ ��������, ���������� ����������� ��� ���������� ������
            _musicButton.image.sprite = _musicEnabled;
        }
        else
        {
            // ���� ������ ���������, ���������� ����������� ��� ����������� ������
            _musicButton.image.sprite = _musicDisabled;
        }
    }

    /// <summary>
    /// ����� ��� ��������� ��� ���������� ������
    /// </summary>
    /// <param name="isMusicMuted">��������� ������</param>
    private void ToggleMusic(bool isMusicMuted)
    {
        // ���������, ��� �������� ������ ��������
        if (_musicSource != null)
        {
            if (isMusicMuted)
            {
                // ��������� ������ ����� AudioManager
                AudioManager.EnableSound(_musicSource); // ������������� ������ ����� �������������
                // ����������� ��������� ��������� ������
                Logger.Log(Logger.LogLevel.Info, "MenuManager", "Music enabled.");
            }
            else
            {
                // ���������� ������ ����� AudioManager
                AudioManager.DisableSound(_musicSource); // ������������� ��������������� ������
                // ����������� ��������� ���������� ������
                Logger.Log(Logger.LogLevel.Info, "MenuManager", "Music disabled.");
            }
        }
        else
        {
            // ����������� ������, ���� �������� ������ �� ��������
            Logger.Log(Logger.LogLevel.Error, "MenuManager", "MusicSource is not assigned!");
        }
    }

    /// <summary>
    /// ����� ��� ������ ������ ��������
    /// </summary>
    private void ShowLoadingScreen()
    {
        // ��������, ��� ������� ���������, ����� �� ����������
        if (_loading != null && _lobby != null)
        {
            _loading.SetActive(true); // ������������ ����� ��������
            _lobby.SetActive(false); // �������������� ����� �����
        }
        else
        {
            // ��������� �� ������
            Logger.Log(Logger.LogLevel.Error, "MenuManager", "Loading or Lobby GameObject is not assigned in the inspector!");
        }
    }

    /// <summary>
    /// ����� ��� ������ ������ �����
    /// </summary>
    private void ShowLobbyScreen()
    {
        // ��������, ��� ������� ���������, ����� �� ����������
        if (_loading != null && _lobby != null)
        {
            _loading.SetActive(false); // �������������� ����� ��������
            _lobby.SetActive(true); // ������������ ����� �����
        }
        else
        {
            // ��������� �� ������
            Logger.Log(Logger.LogLevel.Error, "MenuManager", "Loading or Lobby GameObject is not assigned in the inspector!");
        }
    }
}
