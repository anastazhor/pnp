using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator anim;
    public float pushForce = 10f; // ���� ������������
    public float pushRadius = 5f; // ������ ��������

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("PunchTrigger");
            PushObjects(); // �������� ����� ������������
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Smert"))
        {
            anim.SetTrigger("HitTrigger");
        }
    }

    private void PushObjects()
    {
        // �������� ��� ������� � ������� ��������
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRadius);
        foreach (var hitCollider in hitColliders)
        {
            // ���������, ���� �� � ������� ��������� Rigidbody
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // ������������ ����������� �� ��������� � �������
                Vector3 direction = hitCollider.transform.position - transform.position;
                direction.Normalize(); // ����������� ������

                // ��������� ���� � �������
                rb.AddForce(direction * pushForce, ForceMode.Impulse);
            }
        }
    }
}
