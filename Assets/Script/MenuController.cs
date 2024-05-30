using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject rulesPanel;
    public AudioSource audioSource; // Компонент AudioSource
    public AudioClip buttonClickSound; // Звук для кнопки

    // Метод для загрузки игровой сцены
    public void StartGame()
    {
        audioSource.PlayOneShot(buttonClickSound);
        Invoke("LoadGameScene", buttonClickSound.length);
        // Убедитесь, что название сцены соответствует вашему новому имени
    }
    void LoadGameScene()
    {
        // Загрузка сцены с игрой (замените "GameScene" на имя вашей игровой сцены)
        SceneManager.LoadScene("SampleScene");
    }
    // Метод для показа панели правил
    public void ShowRules()
    {
        rulesPanel.SetActive(true);
    }

    // Метод для скрытия панели правил
    public void HideRules()
    {
        rulesPanel.SetActive(false);
    }
}
