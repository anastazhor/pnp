using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private Animator anim;
    public float pushForce = 10f; // Сила отталкивания
    public float pushRadius = 5f; // Радиус действия

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            anim.SetTrigger("PunchTrigger");
            PushObjects(); // Вызываем метод отталкивания
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
        // Получаем все объекты в радиусе действия
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pushRadius);
        foreach (var hitCollider in hitColliders)
        {
            // Проверяем, есть ли у объекта компонент Rigidbody
            Rigidbody rb = hitCollider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Рассчитываем направление от персонажа к объекту
                Vector3 direction = hitCollider.transform.position - transform.position;
                direction.Normalize(); // Нормализуем вектор

                // Применяем силу к объекту
                rb.AddForce(direction * pushForce, ForceMode.Impulse);
            }
        }
    }
}
