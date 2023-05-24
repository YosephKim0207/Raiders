using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000A87 RID: 2695
	[ActionCategory("Substance")]
	[Tooltip("Rebuilds all dirty textures. By default the rebuild is spread over multiple frames so it won't halt the game. Check Immediately to rebuild all textures in a single frame.")]
	public class RebuildTextures : FsmStateAction
	{
		// Token: 0x06003932 RID: 14642 RVA: 0x001252D8 File Offset: 0x001234D8
		public override void Reset()
		{
			this.substanceMaterial = null;
			this.immediately = false;
			this.everyFrame = false;
		}

		// Token: 0x06003933 RID: 14643 RVA: 0x001252F4 File Offset: 0x001234F4
		public override void OnEnter()
		{
			this.DoRebuildTextures();
			if (!this.everyFrame)
			{
				base.Finish();
			}
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x00125310 File Offset: 0x00123510
		public override void OnUpdate()
		{
			this.DoRebuildTextures();
		}

		// Token: 0x06003935 RID: 14645 RVA: 0x00125318 File Offset: 0x00123518
		private void DoRebuildTextures()
		{
			ProceduralMaterial proceduralMaterial = this.substanceMaterial.Value as ProceduralMaterial;
			if (proceduralMaterial == null)
			{
				base.LogError("Not a substance material!");
				return;
			}
			if (!this.immediately.Value)
			{
				proceduralMaterial.RebuildTextures();
			}
			else
			{
				proceduralMaterial.RebuildTexturesImmediately();
			}
		}

		// Token: 0x04002B84 RID: 11140
		[RequiredField]
		public FsmMaterial substanceMaterial;

		// Token: 0x04002B85 RID: 11141
		[RequiredField]
		public FsmBool immediately;

		// Token: 0x04002B86 RID: 11142
		public bool everyFrame;
	}
}
