using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200092F RID: 2351
	[Tooltip("Destroys a Component of an Object.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class DestroyComponent : FsmStateAction
	{
		// Token: 0x06003393 RID: 13203 RVA: 0x0010D920 File Offset: 0x0010BB20
		public override void Reset()
		{
			this.aComponent = null;
			this.gameObject = null;
			this.component = null;
		}

		// Token: 0x06003394 RID: 13204 RVA: 0x0010D938 File Offset: 0x0010BB38
		public override void OnEnter()
		{
			this.DoDestroyComponent((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			base.Finish();
		}

		// Token: 0x06003395 RID: 13205 RVA: 0x0010D974 File Offset: 0x0010BB74
		private void DoDestroyComponent(GameObject go)
		{
			this.aComponent = go.GetComponent(ReflectionUtils.GetGlobalType(this.component.Value));
			if (this.aComponent == null)
			{
				base.LogError("No such component: " + this.component.Value);
			}
			else
			{
				UnityEngine.Object.Destroy(this.aComponent);
			}
		}

		// Token: 0x040024BF RID: 9407
		[Tooltip("The GameObject that owns the Component.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040024C0 RID: 9408
		[Tooltip("The name of the Component to destroy.")]
		[RequiredField]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString component;

		// Token: 0x040024C1 RID: 9409
		private Component aComponent;
	}
}
