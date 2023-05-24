using System;
using System.Collections;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x0200096D RID: 2413
	[Tooltip("Finds the Child of a GameObject by Name and/or Tag. Use this to find attach points etc. NOTE: This action will search recursively through all children and return the first match; To find a specific child use Find Child.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class GetChild : FsmStateAction
	{
		// Token: 0x06003492 RID: 13458 RVA: 0x00110878 File Offset: 0x0010EA78
		public override void Reset()
		{
			this.gameObject = null;
			this.childName = string.Empty;
			this.withTag = "Untagged";
			this.storeResult = null;
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x001108A8 File Offset: 0x0010EAA8
		public override void OnEnter()
		{
			this.storeResult.Value = GetChild.DoGetChildByName(base.Fsm.GetOwnerDefaultTarget(this.gameObject), this.childName.Value, this.withTag.Value);
			base.Finish();
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x001108E8 File Offset: 0x0010EAE8
		private static GameObject DoGetChildByName(GameObject root, string name, string tag)
		{
			if (root == null)
			{
				return null;
			}
			IEnumerator enumerator = root.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (!string.IsNullOrEmpty(name))
					{
						if (transform.name == name)
						{
							if (string.IsNullOrEmpty(tag))
							{
								return transform.gameObject;
							}
							if (transform.tag.Equals(tag))
							{
								return transform.gameObject;
							}
						}
					}
					else if (!string.IsNullOrEmpty(tag) && transform.tag == tag)
					{
						return transform.gameObject;
					}
					GameObject gameObject = GetChild.DoGetChildByName(transform.gameObject, name, tag);
					if (gameObject != null)
					{
						return gameObject;
					}
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
			return null;
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x001109FC File Offset: 0x0010EBFC
		public override string ErrorCheck()
		{
			if (string.IsNullOrEmpty(this.childName.Value) && string.IsNullOrEmpty(this.withTag.Value))
			{
				return "Specify Child Name, Tag, or both.";
			}
			return null;
		}

		// Token: 0x040025C5 RID: 9669
		[RequiredField]
		[Tooltip("The GameObject to search.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x040025C6 RID: 9670
		[Tooltip("The name of the child to search for.")]
		public FsmString childName;

		// Token: 0x040025C7 RID: 9671
		[UIHint(UIHint.Tag)]
		[Tooltip("The Tag to search for. If Child Name is set, both name and Tag need to match.")]
		public FsmString withTag;

		// Token: 0x040025C8 RID: 9672
		[RequiredField]
		[UIHint(UIHint.Variable)]
		[Tooltip("Store the result in a GameObject variable.")]
		public FsmGameObject storeResult;
	}
}
