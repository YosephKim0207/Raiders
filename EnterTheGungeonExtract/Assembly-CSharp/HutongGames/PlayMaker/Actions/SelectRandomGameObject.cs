using System;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AB6 RID: 2742
	[Tooltip("Selects a Random Game Object from an array of Game Objects.")]
	[ActionCategory(ActionCategory.GameObject)]
	public class SelectRandomGameObject : FsmStateAction
	{
		// Token: 0x06003A31 RID: 14897 RVA: 0x00128464 File Offset: 0x00126664
		public override void Reset()
		{
			this.gameObjects = new FsmGameObject[3];
			this.weights = new FsmFloat[] { 1f, 1f, 1f };
			this.storeGameObject = null;
		}

		// Token: 0x06003A32 RID: 14898 RVA: 0x001284B8 File Offset: 0x001266B8
		public override void OnEnter()
		{
			this.DoSelectRandomGameObject();
			base.Finish();
		}

		// Token: 0x06003A33 RID: 14899 RVA: 0x001284C8 File Offset: 0x001266C8
		private void DoSelectRandomGameObject()
		{
			if (this.gameObjects == null)
			{
				return;
			}
			if (this.gameObjects.Length == 0)
			{
				return;
			}
			if (this.storeGameObject == null)
			{
				return;
			}
			int randomWeightedIndex = ActionHelpers.GetRandomWeightedIndex(this.weights);
			if (randomWeightedIndex != -1)
			{
				this.storeGameObject.Value = this.gameObjects[randomWeightedIndex].Value;
			}
		}

		// Token: 0x04002C63 RID: 11363
		[CompoundArray("Game Objects", "Game Object", "Weight")]
		public FsmGameObject[] gameObjects;

		// Token: 0x04002C64 RID: 11364
		[HasFloatSlider(0f, 1f)]
		public FsmFloat[] weights;

		// Token: 0x04002C65 RID: 11365
		[RequiredField]
		[UIHint(UIHint.Variable)]
		public FsmGameObject storeGameObject;
	}
}
