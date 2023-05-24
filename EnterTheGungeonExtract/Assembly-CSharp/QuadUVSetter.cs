using System;
using UnityEngine;

// Token: 0x02001728 RID: 5928
[ExecuteInEditMode]
public class QuadUVSetter : MonoBehaviour
{
	// Token: 0x060089AB RID: 35243 RVA: 0x00394368 File Offset: 0x00392568
	private void OnEnable()
	{
		Mesh sharedMesh = base.GetComponent<MeshFilter>().sharedMesh;
		Vector2[] uv = sharedMesh.uv;
		uv[0] = this.uv0;
		uv[1] = this.uv1;
		uv[2] = this.uv2;
		uv[3] = this.uv3;
		sharedMesh.uv = uv;
		sharedMesh.uv2 = uv;
	}

	// Token: 0x04009009 RID: 36873
	public Vector2 uv0;

	// Token: 0x0400900A RID: 36874
	public Vector2 uv1;

	// Token: 0x0400900B RID: 36875
	public Vector2 uv2;

	// Token: 0x0400900C RID: 36876
	public Vector2 uv3;
}
