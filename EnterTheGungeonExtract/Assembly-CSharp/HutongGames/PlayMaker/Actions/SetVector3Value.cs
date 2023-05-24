using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000B1A RID: 2842
	[Tooltip("Sets the value of a Vector3 Variable.")]
	[ActionCategory(ActionCategory.Vector3)]
	public class SetVector3Value : FsmStateAction
	{
		// Token: 0x06003BE4 RID: 15332 RVA: 0x0012D9FC File Offset: 0x0012BBFC
		public override void Reset()
		{
			this.vector3Variable = null;
			this.vector3Value = null;
			this.everyFrame = false;
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x0012DA14 File Offset: 0x0012BC14
		public override void OnEnter()
		{
			this.vector3Variable.Value = this.vector3Value.Value;
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x0012DA40 File Offset: 0x0012BC40
		public override void OnUpdate()
		{
			this.vector3Variable.Value = this.vector3Value.Value;
		}

		// Token: 0x04002E02 RID: 11778
		[UIHint(UIHint.Variable)]
		[RequiredField]
		public FsmVector3 vector3Variable;

		// Token: 0x04002E03 RID: 11779
		[RequiredField]
		public FsmVector3 vector3Value;

		// Token: 0x04002E04 RID: 11780
		public bool everyFrame;
	}
}
