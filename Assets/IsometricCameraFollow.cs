using UnityEngine;

public class IsometricCameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;
    public Vector3 positionOffset = new Vector3(0, 0, 0);

    [Header("Follow Lag Settings")]
    public float positionLagTime = 0.3f; // Задержка в секундах
    public float maxLagDistance = 5f; // Максимальное отставание

    private Vector3 _currentVelocity;
    private Vector3 _targetPosition;

    void Start()
    {
        // Устанавливаем фиксированный поворот
        transform.rotation = Quaternion.Euler(30, -60, 0);

        // Инициализируем позицию камеры
        _targetPosition = target.position + positionOffset;
        transform.position = _targetPosition;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Рассчитываем целевую позицию с учетом смещения
        Vector3 desiredPosition = target.position + positionOffset;

        // Добавляем искусственную задержку
        _targetPosition = Vector3.SmoothDamp(_targetPosition, desiredPosition,
                                          ref _currentVelocity, positionLagTime);

        // Ограничиваем максимальное отставание
        if (Vector3.Distance(_targetPosition, desiredPosition) > maxLagDistance)
        {
            _targetPosition = desiredPosition - (_targetPosition - desiredPosition).normalized * maxLagDistance;
        }

        // Применяем позицию камеры
        transform.position = _targetPosition;
    }
}
