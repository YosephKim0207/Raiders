using System;
using UnityEngine;
using UnityEngine.AI;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020008C4 RID: 2244
	[Tooltip("Synchronize a NavMesh Agent velocity and rotation with the animator process.")]
	[ActionCategory(ActionCategory.Animator)]
	public class NavMeshAgentAnimatorSynchronizer : FsmStateAction
	{
		// Token: 0x060031CB RID: 12747 RVA: 0x00107050 File Offset: 0x00105250
		public override void Reset()
		{
			this.gameObject = null;
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x0010705C File Offset: 0x0010525C
		public override void OnPreprocess()
		{
			base.Fsm.HandleAnimatorMove = true;
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x0010706C File Offset: 0x0010526C
		public override void OnEnter()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				base.Finish();
				return;
			}
			this._agent = ownerDefaultTarget.GetComponent<NavMeshAgent>();
			this._animator = ownerDefaultTarget.GetComponent<Animator>();
			if (this._animator == null)
			{
				base.Finish();
				return;
			}
			this._trans = ownerDefaultTarget.transform;
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x001070DC File Offset: 0x001052DC
		public override void DoAnimatorMove()
		{
			this._agent.velocity = this._animator.deltaPosition / Time.deltaTime;
			this._trans.rotation = this._animator.rootRotation;
		}

		// Token: 0x040022EF RID: 8943
		[CheckForComponent(typeof(Animator))]
		[CheckForComponent(typeof(NavMeshAgent))]
		[RequiredField]
		[Tooltip("The Agent target. An Animator component and a NavMeshAgent component are required")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040022F0 RID: 8944
		private Animator _animator;

		// Token: 0x040022F1 RID: 8945
		private NavMeshAgent _agent;

		// Token: 0x040022F2 RID: 8946
		private Transform _trans;
	}
}
