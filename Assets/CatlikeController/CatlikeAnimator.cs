using UnityEngine;

public class CatlikeAnimator : MonoBehaviour
{
    CatlikeController controller;
    Animator animator;

	void Awake()
	{
		animator = GetComponentInChildren<Animator>();
	}

	private void Update()
	{
		// if inputbridge has input we are moving
	}

	private void OnEnable()
	{
		controller.OnJump += OnJump;
	}

	private void OnDisable()
	{
		controller.OnJump -= OnJump;
	}

	private void OnJump()
	{
		animator.SetTrigger("Jump");
	}
}