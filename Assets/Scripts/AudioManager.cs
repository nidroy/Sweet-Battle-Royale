using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Метод для включения звука для указанного аудиофайла
    /// </summary>
    /// <param name="audioSource">Аудиоисточник, который нужно воспроизвести</param>
    public static void EnableSound(AudioSource audioSource)
    {
        // Проверяем, передан ли корректный аудиофайл
        if (audioSource != null)
        {
            // Восстанавливаем громкость и воспроизводим звук
            SetVolume(audioSource, 50); // Установите нужную громкость
            audioSource.Play(); // Убедитесь, что звук воспроизводится
            Logger.Log(Logger.LogLevel.Info, "AudioManager", "Sound enabled.");
        }
        else
        {
            // Логируем предупреждение, если аудиофайл не передан
            Logger.Log(Logger.LogLevel.Warning, "AudioManager", "AudioSource is null!");
        }
    }

    /// <summary>
    /// Метод для выключения звука для указанного аудиофайла
    /// </summary>
    /// <param name="audioSource">Аудиоисточник, который нужно остановить</param>
    public static void DisableSound(AudioSource audioSource)
    {
        // Проверяем, передан ли корректный аудиофайл
        if (audioSource != null)
        {
            // Устанавливаем громкость на 0
            SetVolume(audioSource, 0);
            Logger.Log(Logger.LogLevel.Info, "AudioManager", "Sound disabled.");
        }
        else
        {
            // Логируем предупреждение, если аудиофайл не передан
            Logger.Log(Logger.LogLevel.Warning, "AudioManager", "AudioSource is null!");
        }
    }

    /// <summary>
    /// Метод для установки громкости для указанного аудиофайла
    /// </summary>
    /// <param name="audioSource">Аудиоисточник, громкость которого нужно установить</param>
    /// <param name="volume">Новая громкость (от 0 до 100)</param>
    public static void SetVolume(AudioSource audioSource, float volume)
    {
        // Проверяем, передан ли корректный аудиофайл
        if (audioSource != null)
        {
            // Ограничиваем значение громкости от 0 до 100
            volume = Mathf.Clamp(volume, 0, 100);

            // Устанавливаем новую громкость
            audioSource.volume = volume / 100f;
            Logger.Log(Logger.LogLevel.Info, "AudioManager", $"Volume set to {volume}.");
        }
        else
        {
            // Логируем предупреждение, если аудиофайл не передан
            Logger.Log(Logger.LogLevel.Warning, "AudioManager", "AudioSource is null!");
        }
    }
}
