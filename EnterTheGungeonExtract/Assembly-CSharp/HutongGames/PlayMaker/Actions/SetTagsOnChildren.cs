using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B17 RID: 2839
	[Tooltip("Set the Tag on all children of a GameObject. Optionally filter by component.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class SetTagsOnChildren : FsmStateAction
	{
		// Token: 0x06003BD5 RID: 15317 RVA: 0x0012D534 File Offset: 0x0012B734
		public override void Reset()
		{
			this.gameObject = null;
			this.tag = null;
			this.filterByComponent = null;
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x0012D54C File Offset: 0x0012B74C
		public override void OnEnter()
		{
			this.SetTag(base.Fsm.GetOwnerDefaultTarget(this.gameObject));
			base.Finish();
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0012D56C File Offset: 0x0012B76C
		private void SetTag(GameObject parent)
		{
			if (parent == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.filterByComponent.Value))
			{
				IEnumerator enumerator = parent.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Transform transform = (Transform)obj;
						transform.gameObject.tag = this.tag.Value;
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
			else
			{
				this.UpdateComponentFilter();
				if (this.componentFilter != null)
				{
					Component[] componentsInChildren = parent.GetComponentsInChildren(this.componentFilter);
					foreach (Component component in componentsInChildren)
					{
						component.gameObject.tag = this.tag.Value;
					}
				}
			}
			base.Finish();
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0012D668 File Offset: 0x0012B868
		private void UpdateComponentFilter()
		{
			this.componentFilter = ReflectionUtils.GetGlobalType(this.filterByComponent.Value);
			if (this.componentFilter == null)
			{
				this.componentFilter = ReflectionUtils.GetGlobalType("UnityEngine." + this.filterByComponent.Value);
			}
			if (this.componentFilter == null)
			{
				Debug.LogWarning("Couldn't get type: " + this.filterByComponent.Value);
			}
		}

		// Token: 0x04002DF2 RID: 11762
		[Tooltip("GameObject Parent")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002DF3 RID: 11763
		[UIHint(UIHint.Tag)]
		[Tooltip("Set Tag To...")]
		[RequiredField]
		public FsmString tag;

		// Token: 0x04002DF4 RID: 11764
		[Tooltip("Only set the Tag on children with this component.")]
		[UIHint(UIHint.ScriptComponent)]
		public FsmString filterByComponent;

		// Token: 0x04002DF5 RID: 11765
		private Type componentFilter;
	}
}
