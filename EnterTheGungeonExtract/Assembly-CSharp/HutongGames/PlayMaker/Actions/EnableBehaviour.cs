using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200093E RID: 2366
	[ActionCategory(ActionCategory.ScriptControl)]
	[Tooltip("Enables/Disables a Behaviour on a GameObject. Optionally reset the Behaviour on exit - useful if you want the Behaviour to be active only while this state is active.")]
	public class EnableBehaviour : FsmStateAction
	{
		// Token: 0x060033C7 RID: 13255 RVA: 0x0010E230 File Offset: 0x0010C430
		public override void Reset()
		{
			this.gameObject = null;
			this.behaviour = null;
			this.component = null;
			this.enable = true;
			this.resetOnExit = true;
		}

		// Token: 0x060033C8 RID: 13256 RVA: 0x0010E260 File Offset: 0x0010C460
		public override void OnEnter()
		{
			this.DoEnableBehaviour(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			base.Finish();
		}

		// Token: 0x060033C9 RID: 13257 RVA: 0x0010E280 File Offset: 0x0010C480
		private void DoEnableBehaviour(GameObject go)
		{
			if (go == null)
			{
				return;
			}
			if (this.component != null)
			{
				this.componentTarget = this.component as Behaviour;
			}
			else
			{
				this.componentTarget = go.GetComponent(ReflectionUtils.GetGlobalType(this.behaviour.Value)) as Behaviour;
			}
			if (this.componentTarget == null)
			{
				base.LogWarning(" " + go.name + " missing behaviour: " + this.behaviour.Value);
				return;
			}
			this.componentTarget.enabled = this.enable.Value;
		}

		// Token: 0x060033CA RID: 13258 RVA: 0x0010E330 File Offset: 0x0010C530
		public override void OnExit()
		{
			if (this.componentTarget == null)
			{
				return;
			}
			if (this.resetOnExit.Value)
			{
				this.componentTarget.enabled = !this.enable.Value;
			}
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x0010E370 File Offset: 0x0010C570
		public override string ErrorCheck()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null || this.component != null || this.behaviour.IsNone || string.IsNullOrEmpty(this.behaviour.Value))
			{
				return null;
			}
			Behaviour behaviour = ownerDefaultTarget.GetComponent(ReflectionUtils.GetGlobalType(this.behaviour.Value)) as Behaviour;
			return (!(behaviour != null)) ? "Behaviour missing" : null;
		}

		// Token: 0x040024EB RID: 9451
		[RequiredField]
		[Tooltip("The GameObject that owns the Behaviour.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040024EC RID: 9452
		[Tooltip("The name of the Behaviour to enable/disable.")]
		[UIHint(UIHint.Behaviour)]
		public FsmString behaviour;

		// Token: 0x040024ED RID: 9453
		[Tooltip("Optionally drag a component directly into this field (behavior name will be ignored).")]
		public Component component;

		// Token: 0x040024EE RID: 9454
		[Tooltip("Set to True to enable, False to disable.")]
		[RequiredField]
		public FsmBool enable;

		// Token: 0x040024EF RID: 9455
		public FsmBool resetOnExit;

		// Token: 0x040024F0 RID: 9456
		private Behaviour componentTarget;
	}
}
