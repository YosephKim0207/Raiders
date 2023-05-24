using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000946 RID: 2374
	[ActionCategory(ActionCategory.GameObject)]
	[Tooltip("Finds the closest object to the specified Game Object.\nOptionally filter by Tag and Visibility.")]
	public class FindClosest : FsmStateAction
	{
		// Token: 0x060033EE RID: 13294 RVA: 0x0010E9B0 File Offset: 0x0010CBB0
		public override void Reset()
		{
			this.gameObject = null;
			this.withTag = "Untagged";
			this.ignoreOwner = true;
			this.mustBeVisible = false;
			this.storeObject = null;
			this.storeDistance = null;
			this.everyFrame = false;
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x0010EA04 File Offset: 0x0010CC04
		public override void OnEnter()
		{
			this.DoFindClosest();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x0010EA20 File Offset: 0x0010CC20
		public override void OnUpdate()
		{
			this.DoFindClosest();
		}

		// Token: 0x060033F1 RID: 13297 RVA: 0x0010EA28 File Offset: 0x0010CC28
		private void DoFindClosest()
		{
			GameObject gameObject = ((this.gameObject.OwnerOption != OwnerDefaultOption.UseOwner) ? this.gameObject.GameObject.Value : base.Owner);
			GameObject[] array;
			if (string.IsNullOrEmpty(this.withTag.Value) || this.withTag.Value == "Untagged")
			{
				array = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
			}
			else
			{
				array = GameObject.FindGameObjectsWithTag(this.withTag.Value);
			}
			GameObject gameObject2 = null;
			float num = float.PositiveInfinity;
			foreach (GameObject gameObject3 in array)
			{
				if (!this.ignoreOwner.Value || !(gameObject3 == base.Owner))
				{
					if (!this.mustBeVisible.Value || ActionHelpers.IsVisible(gameObject3))
					{
						float sqrMagnitude = (gameObject.transform.position - gameObject3.transform.position).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							gameObject2 = gameObject3;
						}
					}
				}
			}
			this.storeObject.Value = gameObject2;
			if (!this.storeDistance.IsNone)
			{
				this.storeDistance.Value = Mathf.Sqrt(num);
			}
		}

		// Token: 0x0400250F RID: 9487
		[RequiredField]
		[Tooltip("The GameObject to measure from.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002510 RID: 9488
		[Tooltip("Only consider objects with this Tag. NOTE: It's generally a lot quicker to find objects with a Tag!")]
		[UIHint(UIHint.Tag)]
		[RequiredField]
		public FsmString withTag;

		// Token: 0x04002511 RID: 9489
		[Tooltip("If checked, ignores the object that owns this FSM.")]
		public FsmBool ignoreOwner;

		// Token: 0x04002512 RID: 9490
		[Tooltip("Only consider objects visible to the camera.")]
		public FsmBool mustBeVisible;

		// Token: 0x04002513 RID: 9491
		[Tooltip("Store the closest object.")]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeObject;

		// Token: 0x04002514 RID: 9492
		[Tooltip("Store the distance to the closest object.")]
		[UIHint(UIHint.Variable)]
		public FsmFloat storeDistance;

		// Token: 0x04002515 RID: 9493
		[Tooltip("Repeat every frame")]
		public bool everyFrame;
	}
}
