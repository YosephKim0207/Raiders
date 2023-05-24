using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000880 RID: 2176
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Adds a Script to a Game Object. Use this to change the behaviour of objects on the fly. Optionally remove the Script on exiting the state.")]
	public class AddScript : FsmStateAction
	{
		// Token: 0x0600307D RID: 12413 RVA: 0x000FF208 File Offset: 0x000FD408
		public override void Reset()
		{
			this.gameObject = null;
			this.script = null;
		}

		// Token: 0x0600307E RID: 12414 RVA: 0x000FF218 File Offset: 0x000FD418
		public override void OnEnter()
		{
			this.DoAddComponent((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			base.Finish();
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x000FF254 File Offset: 0x000FD454
		public override void OnExit()
		{
			if (this.removeOnExit.Value && this.addedComponent != null)
			{
				UnityEngine.Object.Destroy(this.addedComponent);
			}
		}

		// Token: 0x06003080 RID: 12416 RVA: 0x000FF284 File Offset: 0x000FD484
		private void DoAddComponent(GameObject go)
		{
			this.addedComponent = go.AddComponent(ReflectionUtils.GetGlobalType(this.script.Value));
			if (this.addedComponent == null)
			{
				base.LogError("Can't add script: " + this.script.Value);
			}
		}

		// Token: 0x04002121 RID: 8481
		[Tooltip("The GameObject to add the script to.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002122 RID: 8482
		[RequiredField]
		[Tooltip("The Script to add to the GameObject.")]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString script;

		// Token: 0x04002123 RID: 8483
		[Tooltip("Remove the script from the GameObject when this State is exited.")]
		public FsmBool removeOnExit;

		// Token: 0x04002124 RID: 8484
		private Component addedComponent;
	}
}
