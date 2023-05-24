using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AD1 RID: 2769
	[Tooltip("Sets Event Data before sending an event. Get the Event Data, along with sender information, using Get Event Info action.")]
	[ActionCategory(ActionCategory.StateMachine)]
	public class SetEventData : FsmStateAction
	{
		// Token: 0x06003AA4 RID: 15012 RVA: 0x00129A6C File Offset: 0x00127C6C
		public override void Reset()
		{
			this.setGameObjectData = new FsmGameObject
			{
				UseVariable = true
			};
			this.setIntData = new FsmInt
			{
				UseVariable = true
			};
			this.setFloatData = new FsmFloat
			{
				UseVariable = true
			};
			this.setStringData = new FsmString
			{
				UseVariable = true
			};
			this.setBoolData = new FsmBool
			{
				UseVariable = true
			};
			this.setVector2Data = new FsmVector2
			{
				UseVariable = true
			};
			this.setVector3Data = new FsmVector3
			{
				UseVariable = true
			};
			this.setRectData = new FsmRect
			{
				UseVariable = true
			};
			this.setQuaternionData = new FsmQuaternion
			{
				UseVariable = true
			};
			this.setColorData = new FsmColor
			{
				UseVariable = true
			};
			this.setMaterialData = new FsmMaterial
			{
				UseVariable = true
			};
			this.setTextureData = new FsmTexture
			{
				UseVariable = true
			};
			this.setObjectData = new FsmObject
			{
				UseVariable = true
			};
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x00129B98 File Offset: 0x00127D98
		public override void OnEnter()
		{
			Fsm.EventData.BoolData = this.setBoolData.Value;
			Fsm.EventData.IntData = this.setIntData.Value;
			Fsm.EventData.FloatData = this.setFloatData.Value;
			Fsm.EventData.Vector2Data = this.setVector2Data.Value;
			Fsm.EventData.Vector3Data = this.setVector3Data.Value;
			Fsm.EventData.StringData = this.setStringData.Value;
			Fsm.EventData.GameObjectData = this.setGameObjectData.Value;
			Fsm.EventData.RectData = this.setRectData.Value;
			Fsm.EventData.QuaternionData = this.setQuaternionData.Value;
			Fsm.EventData.ColorData = this.setColorData.Value;
			Fsm.EventData.MaterialData = this.setMaterialData.Value;
			Fsm.EventData.TextureData = this.setTextureData.Value;
			Fsm.EventData.ObjectData = this.setObjectData.Value;
			base.Finish();
		}

		// Token: 0x04002CC4 RID: 11460
		public FsmGameObject setGameObjectData;

		// Token: 0x04002CC5 RID: 11461
		public FsmInt setIntData;

		// Token: 0x04002CC6 RID: 11462
		public FsmFloat setFloatData;

		// Token: 0x04002CC7 RID: 11463
		public FsmString setStringData;

		// Token: 0x04002CC8 RID: 11464
		public FsmBool setBoolData;

		// Token: 0x04002CC9 RID: 11465
		public FsmVector2 setVector2Data;

		// Token: 0x04002CCA RID: 11466
		public FsmVector3 setVector3Data;

		// Token: 0x04002CCB RID: 11467
		public FsmRect setRectData;

		// Token: 0x04002CCC RID: 11468
		public FsmQuaternion setQuaternionData;

		// Token: 0x04002CCD RID: 11469
		public FsmColor setColorData;

		// Token: 0x04002CCE RID: 11470
		public FsmMaterial setMaterialData;

		// Token: 0x04002CCF RID: 11471
		public FsmTexture setTextureData;

		// Token: 0x04002CD0 RID: 11472
		public FsmObject setObjectData;
	}
}
