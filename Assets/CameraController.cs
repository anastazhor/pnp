using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform Controller; // ����� ��� ������, ������� ����� ���������
    public Camera firstPersonCamera; // ������ �� ������� ����
    public Camera thirdPersonCamera; // ������ �� �������� ����
    private bool isThirdPerson = false; // ������������� ����
    float xRotation = 0f;
    float yRotation = 0f; // ��� �������� �� ��� Y

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SwitchToFirstPerson(); // �������� � ���� �� ������� ����
    }

    void Update()
    {
        // ������������ ����
        if (Input.GetKeyDown(KeyCode.V))
        {
            isThirdPerson = !isThirdPerson; // ����������� ���
            if (isThirdPerson)
            {
                SwitchToThirdPerson();
            }
            else
            {
                SwitchToFirstPerson();
            }
        }

        // ��������� ����
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
            // �������� ��������� ��� ������� ����
            yRotation += mouseX;
            Controller.rotation = Quaternion.Euler(0f, yRotation, 0f);
            // ������ ����� �������� �� ������
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

        // ��������� ������� ������� ������
        thirdPersonCamera.transform.position = Controller.position - Controller.forward * 3 + Vector3.up * 1; // ������ ��������� ������
        thirdPersonCamera.transform.LookAt(Controller.position + Vector3.up); // ������� �� ������
    }
}