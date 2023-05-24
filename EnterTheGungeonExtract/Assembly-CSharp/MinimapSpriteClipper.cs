using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015A4 RID: 5540
public class MinimapSpriteClipper : MonoBehaviour
{
	// Token: 0x06007F24 RID: 32548 RVA: 0x0033546C File Offset: 0x0033366C
	public void ForceUpdate()
	{
		this.ClipToTileBounds();
	}

	// Token: 0x06007F25 RID: 32549 RVA: 0x00335474 File Offset: 0x00333674
	private void ClipToTileBounds()
	{
		Transform transform = base.transform;
		if (this.m_baseSprite == null)
		{
			this.m_baseSprite = base.GetComponent<tk2dBaseSprite>();
		}
		Bounds bounds = this.m_baseSprite.GetBounds();
		Vector2 vector = transform.position.XY() + bounds.min.XY();
		Vector2 vector2 = transform.position.XY() + bounds.max.XY();
		IntVector2 intVector = ((vector - Minimap.Instance.transform.position.XY()) * 8f).ToIntVector2(VectorConversions.Floor);
		IntVector2 intVector2 = ((vector2 - Minimap.Instance.transform.position.XY()) * 8f).ToIntVector2(VectorConversions.Floor);
		tk2dSpriteDefinition tk2dSpriteDefinition = this.m_baseSprite.Collection.spriteDefinitions[this.m_baseSprite.spriteId];
		Vector2 vector3 = new Vector2(float.MaxValue, float.MaxValue);
		Vector2 vector4 = new Vector2(float.MinValue, float.MinValue);
		for (int i = 0; i < tk2dSpriteDefinition.uvs.Length; i++)
		{
			vector3 = Vector2.Min(vector3, tk2dSpriteDefinition.uvs[i]);
			vector4 = Vector2.Max(vector4, tk2dSpriteDefinition.uvs[i]);
		}
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Vector2> list3 = new List<Vector2>();
		for (int j = intVector.x; j <= intVector2.x; j++)
		{
			for (int k = intVector.y; k <= intVector2.y; k++)
			{
				if (Minimap.Instance[j, k])
				{
					int count = list.Count;
					float num = (float)j / 8f + Minimap.Instance.transform.position.x;
					float num2 = (float)k / 8f + Minimap.Instance.transform.position.y;
					float num3 = Mathf.Max(num, vector.x) - transform.position.x;
					float num4 = Mathf.Min(num + 0.125f, vector2.x) - transform.position.x;
					float num5 = Mathf.Max(num2, vector.y) - transform.position.y;
					float num6 = Mathf.Min(num2 + 0.125f, vector2.y) - transform.position.y;
					list.Add(new Vector3(num3, num5, 0f));
					list.Add(new Vector3(num4, num5, 0f));
					list.Add(new Vector3(num3, num6, 0f));
					list.Add(new Vector3(num4, num6, 0f));
					list2.Add(count);
					list2.Add(count + 2);
					list2.Add(count + 1);
					list2.Add(count + 2);
					list2.Add(count + 3);
					list2.Add(count + 1);
					float num7 = (num3 + transform.position.x - vector.x) / (vector2.x - vector.x);
					float num8 = (num4 + transform.position.x - vector.x) / (vector2.x - vector.x);
					float num9 = (num5 + transform.position.y - vector.y) / (vector2.y - vector.y);
					float num10 = (num6 + transform.position.y - vector.y) / (vector2.y - vector.y);
					float num11 = Mathf.Lerp(vector3.x, vector4.x, num7);
					float num12 = Mathf.Lerp(vector3.x, vector4.x, num8);
					float num13 = Mathf.Lerp(vector3.y, vector4.y, num9);
					float num14 = Mathf.Lerp(vector3.y, vector4.y, num10);
					list3.Add(new Vector2(num11, num13));
					list3.Add(new Vector2(num12, num13));
					list3.Add(new Vector2(num11, num14));
					list3.Add(new Vector2(num12, num14));
				}
			}
		}
		MeshFilter component = base.GetComponent<MeshFilter>();
		Mesh mesh = new Mesh();
		mesh.vertices = list.ToArray();
		mesh.triangles = list2.ToArray();
		mesh.uv = list3.ToArray();
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();
		component.mesh = mesh;
	}

	// Token: 0x040081DD RID: 33245
	private tk2dBaseSprite m_baseSprite;
}
