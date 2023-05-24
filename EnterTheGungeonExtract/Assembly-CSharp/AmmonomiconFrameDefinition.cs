using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200172C RID: 5932
[Serializable]
public class AmmonomiconFrameDefinition
{
	// Token: 0x04009022 RID: 36898
	[FormerlySerializedAs("AmmonomiconTopLayerImage")]
	public Texture2D AmmonomiconTopLayerTexture;

	// Token: 0x04009023 RID: 36899
	[FormerlySerializedAs("AmmonomiconBottomLayerImage")]
	public Texture2D AmmonomiconBottomLayerTexture;

	// Token: 0x04009024 RID: 36900
	public Texture2D AmmonomiconToppestLayerTexture;

	// Token: 0x04009025 RID: 36901
	public float frameTime;

	// Token: 0x04009026 RID: 36902
	public bool CurrentLeftVisible = true;

	// Token: 0x04009027 RID: 36903
	public Vector3 CurrentLeftOffset;

	// Token: 0x04009028 RID: 36904
	public Matrix4x4 CurrentLeftMatrix;

	// Token: 0x04009029 RID: 36905
	public bool CurrentRightVisible = true;

	// Token: 0x0400902A RID: 36906
	public Vector3 CurrentRightOffset;

	// Token: 0x0400902B RID: 36907
	public Matrix4x4 CurrentRightMatrix;

	// Token: 0x0400902C RID: 36908
	public bool ImpendingLeftVisible = true;

	// Token: 0x0400902D RID: 36909
	public Vector3 ImpendingLeftOffset;

	// Token: 0x0400902E RID: 36910
	public Matrix4x4 ImpendingLeftMatrix;

	// Token: 0x0400902F RID: 36911
	public bool ImpendingRightVisible = true;

	// Token: 0x04009030 RID: 36912
	public Vector3 ImpendingRightOffset;

	// Token: 0x04009031 RID: 36913
	public Matrix4x4 ImpendingRightMatrix;
}
