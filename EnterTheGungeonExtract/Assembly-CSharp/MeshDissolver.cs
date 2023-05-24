using System;
using System.Collections;
using UnityEngine;

// Token: 0x020011A3 RID: 4515
public class MeshDissolver : MonoBehaviour
{
	// Token: 0x0600648A RID: 25738 RVA: 0x0026FB80 File Offset: 0x0026DD80
	public void DissolveMesh(Vector2 startPosition, float duration)
	{
		base.StartCoroutine(this.Dissolve(startPosition, duration));
	}

	// Token: 0x0600648B RID: 25739 RVA: 0x0026FB94 File Offset: 0x0026DD94
	private IEnumerator Dissolve(Vector2 startPosition, float duration)
	{
		Vector2 adjTestPosition = startPosition - base.transform.position.XY();
		MeshFilter mf = base.GetComponent<MeshFilter>();
		MeshRenderer mr = base.GetComponent<MeshRenderer>();
		for (int j = 0; j < mr.materials.Length; j++)
		{
			mr.materials[j].shader = Shader.Find("tk2d/CutoutVertexColor");
		}
		Mesh i = mf.mesh;
		float maxDistance = float.MinValue;
		float[] distances = new float[i.vertexCount];
		for (int k = 0; k < i.vertexCount; k++)
		{
			float num = Vector2.Distance(adjTestPosition, i.vertices[k].XY());
			distances[k] = num;
			maxDistance = Mathf.Max(num, maxDistance);
		}
		float elapsed = 0f;
		Color32[] colors = i.colors32;
		if (colors.Length != i.vertexCount)
		{
			colors = new Color32[i.vertexCount];
		}
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / duration;
			for (int l = 0; l < i.vertexCount; l++)
			{
				float num2 = distances[l] / maxDistance;
				float num3 = Mathf.Lerp(1f, 0f, t / num2);
				colors[l] = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte)(num3 * 255f));
			}
			i.colors32 = colors;
			yield return null;
		}
		yield return null;
		yield break;
	}
}
