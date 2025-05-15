using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startMenuPanel;  // Панель стартового меню
    public GameObject optionsPanel;     // Панель настроек

    [Header("Brightness Settings")]
    public Slider brightnessSlider;     // Слайдер яркости
    public Light directionalLight;     // Основной свет сцены

    private bool isMenuActive = true;

    private void Start()
    {
        // настройка сллайдера
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
            brightnessSlider.value = 1.0f;  // Стартовое значение (макс. яркость)
        }

        // Включаем стартовое меню и выключаем настройки
        SetMenuActive(true);
    }

    private void Update()
    {
        // Закрытие/открытие меню по Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        SetMenuActive(isMenuActive);
    }

    private void SetMenuActive(bool state)
    {
        // Если находимся в опциях - возвращаем в главное меню
        if (!state && optionsPanel.activeSelf)
        {
            BackToMenu();
            return;
        }

        startMenuPanel.SetActive(state);
        isMenuActive = state;

        // Пауза игры при открытом меню
        Time.timeScale = state ? 0 : 1;
    }

    // Метод для кнопки Start
    public void StartScene()
    {
        SetMenuActive(false);
        SceneManager.LoadScene("start_scene");
    }

    // Метод для кнопки Options
    public void OpenOptions()
    {
        startMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // Кнопка Back возврат в меню
    public void BackToMenu()
    {
        optionsPanel.SetActive(false);
        SetMenuActive(true);
    }

    // Метод для кнопки Quit
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void AdjustBrightness(float value)
    {
        if (directionalLight != null)
        {
            directionalLight.intensity = value;
        }
    }
}