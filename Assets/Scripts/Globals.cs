using Photon.Realtime;
using System.Collections.Generic;

// ����� ��� �������� ���������� ���������� � �������
public class Globals
{
    // �������� ��������� �������� �����
    private static bool _isSceneLoading = true;

    // �������� ��� ������� � ��������� �������� �����
    public static bool IsSceneLoading
    {
        get
        {
            // ���������� ������� ��������� �������� �����
            return _isSceneLoading;
        }
        set
        {
            // ���������, ���������� �� ��������� ��������
            if (_isSceneLoading != value)
            {
                // ������������� ����� �������� ��������� �������� �����
                _isSceneLoading = value;
            }
            else
            {
                // ����������� ������� ���������� ������� ��������
                Logger.Log(Logger.LogLevel.Warning, "LobbyManager", "Attempted to set IsSceneLoading to its current value!");
            }
        }
    }


    // �������� ��������� ������
    private static bool _isMusicMuted = true;

    // �������� ��� ������� � ��������� ������
    public static bool IsMusicMuted
    {
        get
        {
            // ���������� ������� ��������� ������
            return _isMusicMuted;
        }
        set
        {
            // ��������, ���������� �� ��������� ������
            if (_isMusicMuted != value)
            {
                // ������������� ����� �������� ��������� ������
                _isMusicMuted = value;
            }
            else
            {
                // ����������� ������� ���������� ������� ��������
                Logger.Log(Logger.LogLevel.Warning, "Globals", "Attempted to set IsMusicMuted to its current value!");
            }
        }
    }

    // ������ ������������ �����
    private static List<RoomInfo> _lobbyList = new List<RoomInfo>();

    // �������� ��� ������� � ������ �����
    public static List<RoomInfo> LobbyList
    {
        get
        {
            // ���������� ������� ������ �����
            return _lobbyList;
        }
        set
        {
            // ��������, ��� ���������� ������ �� ����� null
            if (value != null)
            {
                // ������� ������ ����� ����������� ������ ������ �����
                _lobbyList.Clear();

                // ��������� ��� �������� �� ����������� ������
                _lobbyList.AddRange(value);
            }
            else
            {
                // ����������� ������, ���� ������� null
                Logger.Log(Logger.LogLevel.Error, "Globals", "The provided lobby list is null!");
            }
        }
    }

    // �������� ����� ������
    private static string _playerName = "";

    // �������� ��� ������� � ����� ������
    public static string PlayerName
    {
        get
        {
            // ���������� ������� �������� ����� ������
            return _playerName;
        }
        set
        {
            // ��������, ��� ��� �� �������� null ��� ������ �������
            if (!string.IsNullOrEmpty(value))
            {
                // ������������� ����� �������� ��� ����� ������
                _playerName = value;
            }
            else
            {
                // ����������� ������, ���� ��� ������������
                Logger.Log(Logger.LogLevel.Error, "Globals", "Player name cannot be null or empty!");
            }
        }
    }
}
