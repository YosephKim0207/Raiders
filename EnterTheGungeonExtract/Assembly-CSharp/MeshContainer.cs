using System;
using UnityEngine;

// Token: 0x020012C4 RID: 4804
public class MeshContainer
{
	// Token: 0x06006B7F RID: 27519 RVA: 0x002A3CDC File Offset: 0x002A1EDC
	public MeshContainer(Mesh m)
	{
		this.mesh = m;
		this.vertices = m.vertices;
		this.normals = m.normals;
	}

	// Token: 0x06006B80 RID: 27520 RVA: 0x002A3D04 File Offset: 0x002A1F04
	public void Update()
	{
		this.mesh.vertices = this.vertices;
		this.mesh.normals = this.normals;
	}

	// Token: 0x04006876 RID: 26742
	public Mesh mesh;

	// Token: 0x04006877 RID: 26743
	public Vector3[] vertices;

	// Token: 0x04006878 RID: 26744
	public Vector3[] normals;
}
