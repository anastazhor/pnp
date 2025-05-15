using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 positionOffset = new Vector3(0, 0, 0);

    [Header("Follow Lag Settings")]
    public float positionLagTime = 0.3f; // �������� � ��������
    public float maxLagDistance = 5f; // ������������ ����������

    private Vector3 _currentVelocity;
    private Vector3 _targetPosition;

    void Start()
    {
        // ������������� ������������� �������
        transform.rotation = Quaternion.Euler(30, -60, 0);

        // �������������� ������� ������
        _targetPosition = target.position + positionOffset;
        transform.position = _targetPosition;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // ������������ ������� ������� � ������ ��������
        Vector3 desiredPosition = target.position + positionOffset;

        // ��������� ������������� ��������
        _targetPosition = Vector3.SmoothDamp(_targetPosition, desiredPosition,
                                          ref _currentVelocity, positionLagTime);

        // ������������ ������������ ����������
        if (Vector3.Distance(_targetPosition, desiredPosition) > maxLagDistance)
        {
            _targetPosition = desiredPosition - (_targetPosition - desiredPosition).normalized * maxLagDistance;
        }

        // ��������� ������� ������
        transform.position = _targetPosition;
    }
}
