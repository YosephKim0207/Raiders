using System;
using UnityEngine;

// Token: 0x020013AD RID: 5037
public class Encircler : MonoBehaviour
{
	// Token: 0x06007228 RID: 29224 RVA: 0x002D5D9C File Offset: 0x002D3F9C
	private void Start()
	{
		this.m_centerUVID = Shader.PropertyToID("_CenterUV");
		this.m_filter = base.GetComponent<MeshFilter>();
		this.m_renderer = base.GetComponent<Renderer>();
		this.m_actor = base.GetComponent<AIActor>();
		if (this.m_actor && this.m_actor.sprite)
		{
			SpriteOutlineManager.ToggleOutlineRenderers(this.m_actor.sprite, false);
		}
	}

	// Token: 0x06007229 RID: 29225 RVA: 0x002D5E14 File Offset: 0x002D4014
	private void LateUpdate()
	{
		Vector4 vector = Vector4.zero;
		Mesh sharedMesh = this.m_filter.sharedMesh;
		Vector2 vector2 = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 vector3 = new Vector2(float.MinValue, float.MinValue);
		for (int i = 0; i < sharedMesh.uv.Length; i++)
		{
			vector2 = Vector2.Min(sharedMesh.uv[i], vector2);
			vector3 = Vector2.Max(sharedMesh.uv[i], vector3);
			vector += new Vector4(sharedMesh.uv[i].x, sharedMesh.uv[i].y, 0f, 0f);
		}
		vector /= (float)sharedMesh.uv.Length;
		vector.z = Mathf.Min(vector3.x - vector2.x, vector3.y - vector2.y);
		vector.w = (float)this.m_renderer.sharedMaterial.mainTexture.width / (float)this.m_renderer.sharedMaterial.mainTexture.height;
		this.m_renderer.material.SetVector(this.m_centerUVID, vector);
	}

	// Token: 0x0600722A RID: 29226 RVA: 0x002D5F64 File Offset: 0x002D4164
	private void OnDestroy()
	{
	}

	// Token: 0x0400738F RID: 29583
	private MeshFilter m_filter;

	// Token: 0x04007390 RID: 29584
	private Renderer m_renderer;

	// Token: 0x04007391 RID: 29585
	private AIActor m_actor;

	// Token: 0x04007392 RID: 29586
	private int m_centerUVID;
}
