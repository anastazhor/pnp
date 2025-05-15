using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _characterAnimator;

    private void Awake()
    {
        _characterAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
            _characterAnimator.SetTrigger("Jump");
    }
}