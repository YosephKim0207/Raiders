using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200087A RID: 2170
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Activates/deactivates a Game Object. Use this to hide/show areas, or enable/disable many Behaviours at once.")]
	public class ActivateGameObject : FsmStateAction
	{
		// Token: 0x0600305D RID: 12381 RVA: 0x000FEA8C File Offset: 0x000FCC8C
		public override void Reset()
		{
			this.gameObject = null;
			this.activate = true;
			this.recursive = true;
			this.resetOnExit = false;
			this.everyFrame = false;
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000FEABC File Offset: 0x000FCCBC
		public override void OnEnter()
		{
			this.DoActivateGameObject();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000FEAD8 File Offset: 0x000FCCD8
		public override void OnUpdate()
		{
			this.DoActivateGameObject();
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000FEAE0 File Offset: 0x000FCCE0
		public override void OnExit()
		{
			if (this.activatedGameObject == null)
			{
				return;
			}
			if (this.resetOnExit)
			{
				if (this.recursive.Value)
				{
					this.SetActiveRecursively(this.activatedGameObject, !this.activate.Value);
				}
				else
				{
					this.activatedGameObject.SetActive(!this.activate.Value);
				}
			}
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000FEB54 File Offset: 0x000FCD54
		private void DoActivateGameObject()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			if (this.recursive.Value)
			{
				this.SetActiveRecursively(ownerDefaultTarget, this.activate.Value);
			}
			else
			{
				ownerDefaultTarget.SetActive(this.activate.Value);
			}
			this.activatedGameObject = ownerDefaultTarget;
		}

		// Token: 0x06003062 RID: 12386 RVA: 0x000FEBC0 File Offset: 0x000FCDC0
		public void SetActiveRecursively(GameObject go, bool state)
		{
			go.SetActive(state);
			IEnumerator enumerator = go.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					this.SetActiveRecursively(transform.gameObject, state);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = enumerator as IDisposable) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x040020FC RID: 8444
		[Tooltip("The GameObject to activate/deactivate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x040020FD RID: 8445
		[RequiredField]
		[Tooltip("Check to activate, uncheck to deactivate Game Object.")]
		public FsmBool activate;

		// Token: 0x040020FE RID: 8446
		[Tooltip("Recursively activate/deactivate all children.")]
		public FsmBool recursive;

		// Token: 0x040020FF RID: 8447
		[Tooltip("Reset the game objects when exiting this state. Useful if you want an object to be active only while this state is active.\nNote: Only applies to the last Game Object activated/deactivated (won't work if Game Object changes).")]
		public bool resetOnExit;

		// Token: 0x04002100 RID: 8448
		[Tooltip("Repeat this action every frame. Useful if Activate changes over time.")]
		public bool everyFrame;

		// Token: 0x04002101 RID: 8449
		private GameObject activatedGameObject;
	}
}
