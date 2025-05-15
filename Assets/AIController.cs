using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform target;          // Цель (игрок)
    public float stoppingDistance = 1.5f; // Дистанция остановки перед игроком
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance; // Устанавливаем дистанцию остановки
    }

    void Update()
    {
        if (target != null)
        {
            // Обновляем путь только если игрок вне зоны остановки
            if (Vector3.Distance(transform.position, target.position) > stoppingDistance)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                // Останавливаем агента, если он уже близко
                agent.ResetPath();
            }
        }
    }
}