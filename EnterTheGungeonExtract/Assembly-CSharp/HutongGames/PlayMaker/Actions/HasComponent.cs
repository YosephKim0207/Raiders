using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x020009EA RID: 2538
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Checks if an Object has a Component. Optionally remove the Component on exiting the state.")]
	public class HasComponent : FsmStateAction
	{
		// Token: 0x06003682 RID: 13954 RVA: 0x00116CF0 File Offset: 0x00114EF0
		public override void Reset()
		{
			this.aComponent = null;
			this.gameObject = null;
			this.trueEvent = null;
			this.falseEvent = null;
			this.component = null;
			this.store = null;
			this.everyFrame = false;
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x00116D24 File Offset: 0x00114F24
		public override void OnEnter()
		{
			this.DoHasComponent((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003684 RID: 13956 RVA: 0x00116D74 File Offset: 0x00114F74
		public override void OnUpdate()
		{
			this.DoHasComponent((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x00116DA8 File Offset: 0x00114FA8
		public override void OnExit()
		{
			if (this.removeOnExit.Value && this.aComponent != null)
			{
				UnityEngine.Object.Destroy(this.aComponent);
			}
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x00116DD8 File Offset: 0x00114FD8
		private void DoHasComponent(GameObject go)
		{
			if (go == null)
			{
				if (!this.store.IsNone)
				{
					this.store.Value = false;
				}
				base.Fsm.Event(this.falseEvent);
				return;
			}
			this.aComponent = go.GetComponent(ReflectionUtils.GetGlobalType(this.component.Value));
			if (!this.store.IsNone)
			{
				this.store.Value = this.aComponent != null;
			}
			base.Fsm.Event((!(this.aComponent != null)) ? this.falseEvent : this.trueEvent);
		}

		// Token: 0x040027DE RID: 10206
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040027DF RID: 10207
		[UIHint(UIHint.ScriptComponent)]
		[RequiredField]
		public FsmString component;

		// Token: 0x040027E0 RID: 10208
		public FsmBool removeOnExit;

		// Token: 0x040027E1 RID: 10209
		public FsmEvent trueEvent;

		// Token: 0x040027E2 RID: 10210
		public FsmEvent falseEvent;

		// Token: 0x040027E3 RID: 10211
		[UIHint(UIHint.Variable)]
		public FsmBool store;

		// Token: 0x040027E4 RID: 10212
		public bool everyFrame;

		// Token: 0x040027E5 RID: 10213
		private Component aComponent;
	}
}
