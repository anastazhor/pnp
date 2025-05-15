using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public Transform target;          // ���� (�����)
    public float stoppingDistance = 1.5f; // ��������� ��������� ����� �������
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance; // ������������� ��������� ���������
    }

    void Update()
    {
        if (target != null)
        {
            // ��������� ���� ������ ���� ����� ��� ���� ���������
            if (Vector3.Distance(transform.position, target.position) > stoppingDistance)
            {
                agent.SetDestination(target.position);
            }
            else
            {
                // ������������� ������, ���� �� ��� ������
                agent.ResetPath();
            }
        }
    }
}