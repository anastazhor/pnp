using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startMenuPanel;  // ������ ���������� ����
    public GameObject optionsPanel;     // ������ ��������

    [Header("Brightness Settings")]
    public Slider brightnessSlider;     // ������� �������
    public Light directionalLight;     // �������� ���� �����

    private bool isMenuActive = true;

    private void Start()
    {
        // ��������� ���������
        if (brightnessSlider != null)
        {
            brightnessSlider.onValueChanged.AddListener(AdjustBrightness);
            brightnessSlider.value = 1.0f;  // ��������� �������� (����. �������)
        }

        // �������� ��������� ���� � ��������� ���������
        SetMenuActive(true);
    }

    private void Update()
    {
        // ��������/�������� ���� �� Esc
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
        // ���� ��������� � ������ - ���������� � ������� ����
        if (!state && optionsPanel.activeSelf)
        {
            BackToMenu();
            return;
        }

        startMenuPanel.SetActive(state);
        isMenuActive = state;

        // ����� ���� ��� �������� ����
        Time.timeScale = state ? 0 : 1;
    }

    // ����� ��� ������ Start
    public void StartScene()
    {
        SetMenuActive(false);
        SceneManager.LoadScene("start_scene");
    }

    // ����� ��� ������ Options
    public void OpenOptions()
    {
        startMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // ������ Back ������� � ����
    public void BackToMenu()
    {
        optionsPanel.SetActive(false);
        SetMenuActive(true);
    }

    // ����� ��� ������ Quit
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