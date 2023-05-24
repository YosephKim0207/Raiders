using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B3A RID: 2874
	[ActionCategory(ActionCategory.Transform)]
	[Tooltip("Translates a Game Object. Use a Vector3 variable and/or XYZ components. To leave any axis unchanged, set variable to 'None'.")]
	public class Translate : FsmStateAction
	{
		// Token: 0x06003C60 RID: 15456 RVA: 0x0012FEBC File Offset: 0x0012E0BC
		public override void Reset()
		{
			this.gameObject = null;
			this.vector = null;
			this.x = new FsmFloat
			{
				UseVariable = true
			};
			this.y = new FsmFloat
			{
				UseVariable = true
			};
			this.z = new FsmFloat
			{
				UseVariable = true
			};
			this.space = Space.Self;
			this.perSecond = true;
			this.everyFrame = true;
			this.lateUpdate = false;
			this.fixedUpdate = false;
		}

		// Token: 0x06003C61 RID: 15457 RVA: 0x0012FF38 File Offset: 0x0012E138
		public override void OnPreprocess()
		{
			base.Fsm.HandleFixedUpdate = true;
		}

		// Token: 0x06003C62 RID: 15458 RVA: 0x0012FF48 File Offset: 0x0012E148
		public override void OnEnter()
		{
			if (!this.everyFrame && !this.lateUpdate && !this.fixedUpdate)
			{
				this.DoTranslate();
				base.Finish();
			}
		}

		// Token: 0x06003C63 RID: 15459 RVA: 0x0012FF78 File Offset: 0x0012E178
		public override void OnUpdate()
		{
			if (!this.lateUpdate && !this.fixedUpdate)
			{
				this.DoTranslate();
			}
		}

		// Token: 0x06003C64 RID: 15460 RVA: 0x0012FF98 File Offset: 0x0012E198
		public override void OnLateUpdate()
		{
			if (this.lateUpdate)
			{
				this.DoTranslate();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x0012FFBC File Offset: 0x0012E1BC
		public override void OnFixedUpdate()
		{
			if (this.fixedUpdate)
			{
				this.DoTranslate();
			}
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x0012FFE0 File Offset: 0x0012E1E0
		private void DoTranslate()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			Vector3 vector = ((!this.vector.IsNone) ? this.vector.Value : new Vector3(this.x.Value, this.y.Value, this.z.Value));
			if (!this.x.IsNone)
			{
				vector.x = this.x.Value;
			}
			if (!this.y.IsNone)
			{
				vector.y = this.y.Value;
			}
			if (!this.z.IsNone)
			{
				vector.z = this.z.Value;
			}
			if (!this.perSecond)
			{
				ownerDefaultTarget.transform.Translate(vector, this.space);
			}
			else
			{
				ownerDefaultTarget.transform.Translate(vector * Time.deltaTime, this.space);
			}
		}

		// Token: 0x04002EB4 RID: 11956
		[Tooltip("The game object to translate.")]
		[RequiredField]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002EB5 RID: 11957
		[Tooltip("A translation vector. NOTE: You can override individual axis below.")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 vector;

		// Token: 0x04002EB6 RID: 11958
		[Tooltip("Translation along x axis.")]
		public FsmFloat x;

		// Token: 0x04002EB7 RID: 11959
		[Tooltip("Translation along y axis.")]
		public FsmFloat y;

		// Token: 0x04002EB8 RID: 11960
		[Tooltip("Translation along z axis.")]
		public FsmFloat z;

		// Token: 0x04002EB9 RID: 11961
		[Tooltip("Translate in local or world space.")]
		public Space space;

		// Token: 0x04002EBA RID: 11962
		[Tooltip("Translate over one second")]
		public bool perSecond;

		// Token: 0x04002EBB RID: 11963
		[Tooltip("Repeat every frame.")]
		public bool everyFrame;

		// Token: 0x04002EBC RID: 11964
		[Tooltip("Perform the translate in LateUpdate. This is useful if you want to override the position of objects that are animated or otherwise positioned in Update.")]
		public bool lateUpdate;

		// Token: 0x04002EBD RID: 11965
		[Tooltip("Perform the translate in FixedUpdate. This is useful when working with rigid bodies and physics.")]
		public bool fixedUpdate;
	}
}
