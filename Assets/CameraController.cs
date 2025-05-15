using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform Controller; // Игрок или объект, который будет вращаться
    public Camera firstPersonCamera; // Камера от первого лица
    public Camera thirdPersonCamera; // Камера от третьего лица
    private bool isThirdPerson = false; // Переключатель вида
    float xRotation = 0f;
    float yRotation = 0f; // Для вращения по оси Y

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SwitchToFirstPerson(); // Начинаем с вида от первого лица
    }

    void Update()
    {
        // Переключение вида
        if (Input.GetKeyDown(KeyCode.V))
        {
            isThirdPerson = !isThirdPerson; // Переключаем вид
            if (isThirdPerson)
            {
                SwitchToThirdPerson();
            }
            else
            {
                SwitchToFirstPerson();
            }
        }

        // Обработка мыши
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        if (!isThirdPerson)
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            firstPersonCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            Controller.Rotate(Vector3.up * mouseX);
        }
        else
        {
            // Вращение персонажа при третьем лице
            yRotation += mouseX;
            Controller.rotation = Quaternion.Euler(0f, yRotation, 0f);
            // Камера будет смотреть на игрока
            thirdPersonCamera.transform.LookAt(Controller.position + Vector3.up);
        }
    }

    void SwitchToFirstPerson()
    {
        firstPersonCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);
    }

    void SwitchToThirdPerson()
    {
        firstPersonCamera.gameObject.SetActive(false);
        thirdPersonCamera.gameObject.SetActive(true);

        // Настройка позиции третьей камеры
        thirdPersonCamera.transform.position = Controller.position - Controller.forward * 3 + Vector3.up * 1; // Заднее положение камеры
        thirdPersonCamera.transform.LookAt(Controller.position + Vector3.up); // Смотрим на игрока
    }
}