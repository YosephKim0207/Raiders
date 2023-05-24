using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000947 RID: 2375
	[Tooltip("Finds a Game Object by Name and/or Tag.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class FindGameObject : FsmStateAction
	{
		// Token: 0x060033F3 RID: 13299 RVA: 0x0010EB98 File Offset: 0x0010CD98
		public override void Reset()
		{
			this.objectName = string.Empty;
			this.withTag = "Untagged";
			this.store = null;
		}

		// Token: 0x060033F4 RID: 13300 RVA: 0x0010EBC4 File Offset: 0x0010CDC4
		public override void OnEnter()
		{
			this.Find();
			base.Finish();
		}

		// Token: 0x060033F5 RID: 13301 RVA: 0x0010EBD4 File Offset: 0x0010CDD4
		private void Find()
		{
			if (!(this.withTag.Value != "Untagged"))
			{
				this.store.Value = GameObject.Find(this.objectName.Value);
				return;
			}
			if (!string.IsNullOrEmpty(this.objectName.Value))
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag(this.withTag.Value);
				foreach (GameObject gameObject in array)
				{
					if (gameObject.name == this.objectName.Value)
					{
						this.store.Value = gameObject;
						return;
					}
				}
				this.store.Value = null;
				return;
			}
			this.store.Value = GameObject.FindGameObjectWithTag(this.withTag.Value);
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x0010ECA8 File Offset: 0x0010CEA8
		public override string ErrorCheck()
		{
			if (string.IsNullOrEmpty(this.objectName.Value) && string.IsNullOrEmpty(this.withTag.Value))
			{
				return "Specify Name, Tag, or both.";
			}
			return null;
		}

		// Token: 0x04002516 RID: 9494
		[Tooltip("The name of the GameObject to find. You can leave this empty if you specify a Tag.")]
		public FsmString objectName;

		// Token: 0x04002517 RID: 9495
		[Tooltip("Find a GameObject with this tag. If Object Name is specified then both name and Tag must match.")]
		[UIHint(UIHint.Tag)]
		public FsmString withTag;

		// Token: 0x04002518 RID: 9496
		[Tooltip("Store the result in a GameObject variable.")]
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmGameObject store;
	}
}
