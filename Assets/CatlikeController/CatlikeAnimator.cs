using Photon.Pun;
using UnityEngine;

public class CatlikeAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
	[SerializeField] private float networkMovementRange = 0.1f;
    private CatlikeController controller;
	private PhotonView view;
	private Transform thisTransform;
	private Vector3 previousPosition;

	private void Awake()
	{
		controller = GetComponent<CatlikeController>();
		thisTransform = transform;
		view = GetComponent<PhotonView>();
	}

	private void Start()
	{
		previousPosition = thisTransform.position;
	}

	private void Update()
	{
		if (!view.IsMine)
		{
			if(Vector3.Distance(thisTransform.position, previousPosition) < networkMovementRange)
			{
				animator.SetBool("IsMoving", false);
			}
			else
			{
				animator.SetBool("IsMoving", true);
			}
			previousPosition = thisTransform.position;
		}
		//else
		//{
		//	// we update our own animator (for debugging now, we can't see it anyways)
		//	float _yinput = InputBridgeBase.instance.MoveAxis;
		//	float _xinput = InputBridgeBase.instance.StrafeAxis;
		//	animator.SetBool("IsMoving", _yinput != 0 || _xinput != 0);
		//}
	}

	private void OnEnable()
	{
		if (!view.IsMine) return;
		controller.OnJump += OnJump;
		controller.OnLand += OnLand;
	}

	private void OnDisable()
	{
		if (!view.IsMine) return;
		controller.OnJump -= OnJump;
		controller.OnLand -= OnLand;
	}

	private void OnJump()
	{
		animator.SetTrigger("Jump");
	}

	private void OnLand()
	{
		animator.SetTrigger("Land");
	}
}