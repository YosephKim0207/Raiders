using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000972 RID: 2418
	[ActionCategory(ActionCategory.UnityObject)]
	[Tooltip("Gets a Component attached to a GameObject and stores it in an Object variable. NOTE: Set the Object variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera.")]
	public class GetComponent : FsmStateAction
	{
		// Token: 0x060034A8 RID: 13480 RVA: 0x00110D7C File Offset: 0x0010EF7C
		public override void Reset()
		{
			this.gameObject = null;
			this.storeComponent = null;
			this.everyFrame = false;
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x00110D94 File Offset: 0x0010EF94
		public override void OnEnter()
		{
			this.DoGetComponent();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x00110DB0 File Offset: 0x0010EFB0
		public override void OnUpdate()
		{
			this.DoGetComponent();
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00110DB8 File Offset: 0x0010EFB8
		private void DoGetComponent()
		{
			if (this.storeComponent == null)
			{
				return;
			}
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (this.storeComponent.IsNone)
			{
				return;
			}
			this.storeComponent.Value = ownerDefaultTarget.GetComponent(this.storeComponent.ObjectType);
		}

		// Token: 0x040025DA RID: 9690
		[Tooltip("The GameObject that owns the component.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040025DB RID: 9691
		[Tooltip("Store the component in an Object variable.\nNOTE: Set theObject variable's Object Type to get a component of that type. E.g., set Object Type to UnityEngine.AudioListener to get the AudioListener component on the camera.")]
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmObject storeComponent;

		// Token: 0x040025DC RID: 9692
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;
	}
}
