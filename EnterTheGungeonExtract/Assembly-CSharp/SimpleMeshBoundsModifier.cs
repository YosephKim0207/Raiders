using System;
using UnityEngine;

// Token: 0x0200120E RID: 4622
public class SimpleMeshBoundsModifier : MonoBehaviour
{
	// Token: 0x0600676B RID: 26475 RVA: 0x00287CE8 File Offset: 0x00285EE8
	private void Start()
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		Bounds bounds = component.sharedMesh.bounds;
		bounds.Expand(this.expansionVector);
		component.mesh.bounds = bounds;
	}

	// Token: 0x0400634E RID: 25422
	public Vector3 expansionVector = new Vector3(0f, 20f, 0f);
}
