using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public List<GameObject> cards; // Список всех карточек
    public TextMeshProUGUI sumText; // Текстовый элемент для отображения суммы
    public TextMeshProUGUI scoreText; // Текстовый элемент для отображения очков
    public TextMeshProUGUI livesText; // Текстовый элемент для отображения жизней
    public TextMeshProUGUI gameOverText; // Текстовый элемент для отображения окончания игры
    public TextMeshProUGUI finalScoreText; // Текстовый элемент для отображения итогового счета
    public Button restartButton; // Кнопка для перезапуска игры
    public int targetSum; // Целевая сумма, которую нужно набрать
    public AudioSource audioSource; // Компонент AudioSource
    public AudioClip correctSound; // Звук для правильного ответа
    public AudioClip incorrectSound; // Звук для неправильного ответа
    public AudioClip gameOverSound; // Звук для окончания игры
    private List<int> cardValues = new List<int>(); // Список значений на карточках
    private bool cardsRevealed = false; // Переменная для отслеживания состояния карточек
    private int score = 0; // Переменная для хранения очков
    private int lives = 3; // Переменная для хранения жизней

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        // Генерация случайных значений для карточек
        for (int i = 0; i < cards.Count; i++)
        {
            int value = Random.Range(1, 10);
            cardValues.Add(value);
            cards[i].GetComponentInChildren<TextMeshProUGUI>().text = value.ToString();
        }

        // Выбор случайной целевой суммы
        targetSum = ChooseTargetSum();
        sumText.text = " " + targetSum;
        scoreText.text = "Skor: " + score;
        livesText.text = "Lives: " + lives;

        // Скрываем элементы окончания игры
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(false);

        // Откладываем переворачивание карточек
        Invoke("HideCards", 5f);
    }

    int ChooseTargetSum()
    {
        // Выбираем случайную сумму, которую нужно набрать
        int sum = 0;
        List<int> selectedValues = new List<int>();

        while (sum < 10) // Устанавливаем минимальную целевую сумму
        {
            selectedValues.Clear();
            sum = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                if (Random.value > 0.5f)
                {
                    selectedValues.Add(cardValues[i]);
                    sum += cardValues[i];
                }
            }
        }

        return sum;
    }

    void HideCards()
    {
        // Переворачиваем все карточки
        foreach (var card in cards)
        {
            card.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        cardsRevealed = true;
    }

    public void OnCardSelected(GameObject selectedCard)
    {
        if (!cardsRevealed)
            return;

        selectedCard.GetComponentInChildren<TextMeshProUGUI>().text = cardValues[cards.IndexOf(selectedCard)].ToString();

        List<int> selectedValues = new List<int>();
        foreach (var card in cards)
        {
            if (card.GetComponentInChildren<TextMeshProUGUI>().text != "")
            {
                selectedValues.Add(cardValues[cards.IndexOf(card)]);
            }
        }

        int selectedSum = 0;
        foreach (int value in selectedValues)
        {
            selectedSum += value;
        }

        if (selectedSum == targetSum)
        {
            score += 10; // Добавляем очки за правильный ответ
            scoreText.text = "Skor: " + score; // Обновляем отображение очков
            sumText.text = "Doğru!";
            audioSource.PlayOneShot(correctSound); // Воспроизводим звук правильного ответа
            Invoke("ResetGame", 2f); // Перезапускаем игру через 2 секунды
        }
        else if (selectedSum > targetSum)
        {
            lives--; // Уменьшаем количество жизней за неправильный ответ
            livesText.text = "Lives: " + lives; // Обновляем отображение жизней

            if (lives <= 0)
            {
                sumText.text = "Game over!";
                GameOver();
            }
            else
            {
                sumText.text = "Yanlış!";
                audioSource.PlayOneShot(incorrectSound); // Воспроизводим звук неправильного ответа
                Invoke("ResetGame", 2f); // Перезапускаем игру через 2 секунды
            }
        }
    }

    void ResetGame()
    {
        if (lives > 0)
        {
            // Сбрасываем игру, если у игрока остались жизни
            cardValues.Clear();
            InitializeGame();
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // Останавливаем игру и показываем элементы окончания игры
        gameOverText.gameObject.SetActive(true);
        finalScoreText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        // Отображаем итоговый счёт
        finalScoreText.text = "Son skor: " + score;
        audioSource.PlayOneShot(gameOverSound); // Воспроизводим звук окончания игры

        // Скрываем все остальные элементы UI
        sumText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);

        // Отключаем взаимодействие с карточками
        foreach (var card in cards)
        {
            card.GetComponent<Button>().interactable = false;
            card.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    public void RestartGame()
    {
        // Перезапускаем текущую сцену
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
