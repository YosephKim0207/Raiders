using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200087C RID: 2172
	[Tooltip("Adds a Component to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Component on exiting the state.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class AddComponent : FsmStateAction
	{
		// Token: 0x06003068 RID: 12392 RVA: 0x000FED60 File Offset: 0x000FCF60
		public override void Reset()
		{
			this.gameObject = null;
			this.component = null;
			this.storeComponent = null;
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000FED78 File Offset: 0x000FCF78
		public override void OnEnter()
		{
			this.DoAddComponent();
			base.Finish();
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x000FED88 File Offset: 0x000FCF88
		public override void OnExit()
		{
			if (this.removeOnExit.Value && this.addedComponent != null)
			{
				UnityEngine.Object.Destroy(this.addedComponent);
			}
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000FEDB8 File Offset: 0x000FCFB8
		private void DoAddComponent()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			this.addedComponent = ownerDefaultTarget.AddComponent(ReflectionUtils.GetGlobalType(this.component.Value));
			this.storeComponent.Value = this.addedComponent;
			if (this.addedComponent == null)
			{
				base.LogError("Can't add component: " + this.component.Value);
			}
		}

		// Token: 0x04002108 RID: 8456
		[Tooltip("The GameObject to add the Component to.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002109 RID: 8457
		[Title("Component Type")]
		[Tooltip("The type of Component to add to the Game Object.")]
		[RequiredField]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString component;

		// Token: 0x0400210A RID: 8458
		[UIHint(UIHint.Variable)]
		[ObjectType(typeof(Component))]
		[Tooltip("Store the component in an Object variable. E.g., to use with Set Property.")]
		public FsmObject storeComponent;

		// Token: 0x0400210B RID: 8459
		[Tooltip("Remove the Component when this State is exited.")]
		public FsmBool removeOnExit;

		// Token: 0x0400210C RID: 8460
		private Component addedComponent;
	}
}
