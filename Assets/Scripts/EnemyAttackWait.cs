using UnityEngine;

public class EnemyAttackWait : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Enemy componentInParent = animator.GetComponentInParent<Enemy>();
		if ((bool)componentInParent)
		{
			componentInParent.AttackWaitState = true;
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		Enemy componentInParent = animator.GetComponentInParent<Enemy>();
		if ((bool)componentInParent)
		{
			componentInParent.AttackWaitState = false;
		}
	}
}
