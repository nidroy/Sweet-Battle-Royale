using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// ����� ��� ��������� ����� ��� ���������� ����������
    /// </summary>
    /// <param name="audioSource">�������������, ������� ����� �������������</param>
    public static void EnableSound(AudioSource audioSource)
    {
        // ���������, ������� �� ���������� ���������
        if (audioSource != null)
        {
            // ��������������� ��������� � ������������� ����
            SetVolume(audioSource, 50); // ���������� ������ ���������
            audioSource.Play(); // ���������, ��� ���� ���������������
            Logger.Log(Logger.LogLevel.Info, "AudioManager", "Sound enabled.");
        }
        else
        {
            // �������� ��������������, ���� ��������� �� �������
            Logger.Log(Logger.LogLevel.Warning, "AudioManager", "AudioSource is null!");
        }
    }

    /// <summary>
    /// ����� ��� ���������� ����� ��� ���������� ����������
    /// </summary>
    /// <param name="audioSource">�������������, ������� ����� ����������</param>
    public static void DisableSound(AudioSource audioSource)
    {
        // ���������, ������� �� ���������� ���������
        if (audioSource != null)
        {
            // ������������� ��������� �� 0
            SetVolume(audioSource, 0);
            Logger.Log(Logger.LogLevel.Info, "AudioManager", "Sound disabled.");
        }
        else
        {
            // �������� ��������������, ���� ��������� �� �������
            Logger.Log(Logger.LogLevel.Warning, "AudioManager", "AudioSource is null!");
        }
    }

    /// <summary>
    /// ����� ��� ��������� ��������� ��� ���������� ����������
    /// </summary>
    /// <param name="audioSource">�������������, ��������� �������� ����� ����������</param>
    /// <param name="volume">����� ��������� (�� 0 �� 100)</param>
    public static void SetVolume(AudioSource audioSource, float volume)
    {
        // ���������, ������� �� ���������� ���������
        if (audioSource != null)
        {
            // ������������ �������� ��������� �� 0 �� 100
            volume = Mathf.Clamp(volume, 0, 100);

            // ������������� ����� ���������
            audioSource.volume = volume / 100f;
            Logger.Log(Logger.LogLevel.Info, "AudioManager", $"Volume set to {volume}.");
        }
        else
        {
            // �������� ��������������, ���� ��������� �� �������
            Logger.Log(Logger.LogLevel.Warning, "AudioManager", "AudioSource is null!");
        }
    }
}
