using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000979 RID: 2425
	[Tooltip("Gets info on the last event that caused a state change. See also Set Event Data action.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class GetEventInfo : FsmStateAction
	{
		// Token: 0x060034C5 RID: 13509 RVA: 0x001113C0 File Offset: 0x0010F5C0
		public override void Reset()
		{
			this.sentByGameObject = null;
			this.fsmName = null;
			this.getBoolData = null;
			this.getIntData = null;
			this.getFloatData = null;
			this.getVector2Data = null;
			this.getVector3Data = null;
			this.getStringData = null;
			this.getGameObjectData = null;
			this.getRectData = null;
			this.getQuaternionData = null;
			this.getMaterialData = null;
			this.getTextureData = null;
			this.getColorData = null;
			this.getObjectData = null;
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x00111438 File Offset: 0x0010F638
		public override void OnEnter()
		{
			if (Fsm.EventData.SentByFsm != null)
			{
				this.sentByGameObject.Value = Fsm.EventData.SentByFsm.GameObject;
				this.fsmName.Value = Fsm.EventData.SentByFsm.Name;
			}
			else
			{
				this.sentByGameObject.Value = null;
				this.fsmName.Value = string.Empty;
			}
			this.getBoolData.Value = Fsm.EventData.BoolData;
			this.getIntData.Value = Fsm.EventData.IntData;
			this.getFloatData.Value = Fsm.EventData.FloatData;
			this.getVector2Data.Value = Fsm.EventData.Vector2Data;
			this.getVector3Data.Value = Fsm.EventData.Vector3Data;
			this.getStringData.Value = Fsm.EventData.StringData;
			this.getGameObjectData.Value = Fsm.EventData.GameObjectData;
			this.getRectData.Value = Fsm.EventData.RectData;
			this.getQuaternionData.Value = Fsm.EventData.QuaternionData;
			this.getMaterialData.Value = Fsm.EventData.MaterialData;
			this.getTextureData.Value = Fsm.EventData.TextureData;
			this.getColorData.Value = Fsm.EventData.ColorData;
			this.getObjectData.Value = Fsm.EventData.ObjectData;
			base.Finish();
		}

		// Token: 0x040025FF RID: 9727
		[UIHint(UIHint.Variable)]
		public FsmGameObject sentByGameObject;

		// Token: 0x04002600 RID: 9728
		[UIHint(UIHint.Variable)]
		public FsmString fsmName;

		// Token: 0x04002601 RID: 9729
		[UIHint(UIHint.Variable)]
		public FsmBool getBoolData;

		// Token: 0x04002602 RID: 9730
		[UIHint(UIHint.Variable)]
		public FsmInt getIntData;

		// Token: 0x04002603 RID: 9731
		[UIHint(UIHint.Variable)]
		public FsmFloat getFloatData;

		// Token: 0x04002604 RID: 9732
		[UIHint(UIHint.Variable)]
		public FsmVector2 getVector2Data;

		// Token: 0x04002605 RID: 9733
		[UIHint(UIHint.Variable)]
		public FsmVector3 getVector3Data;

		// Token: 0x04002606 RID: 9734
		[UIHint(UIHint.Variable)]
		public FsmString getStringData;

		// Token: 0x04002607 RID: 9735
		[UIHint(UIHint.Variable)]
		public FsmGameObject getGameObjectData;

		// Token: 0x04002608 RID: 9736
		[UIHint(UIHint.Variable)]
		public FsmRect getRectData;

		// Token: 0x04002609 RID: 9737
		[UIHint(UIHint.Variable)]
		public FsmQuaternion getQuaternionData;

		// Token: 0x0400260A RID: 9738
		[UIHint(UIHint.Variable)]
		public FsmMaterial getMaterialData;

		// Token: 0x0400260B RID: 9739
		[UIHint(UIHint.Variable)]
		public FsmTexture getTextureData;

		// Token: 0x0400260C RID: 9740
		[UIHint(UIHint.Variable)]
		public FsmColor getColorData;

		// Token: 0x0400260D RID: 9741
		[UIHint(UIHint.Variable)]
		public FsmObject getObjectData;
	}
}
