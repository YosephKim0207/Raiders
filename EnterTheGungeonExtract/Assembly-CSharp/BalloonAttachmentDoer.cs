using System;
using UnityEngine;

// Token: 0x02001258 RID: 4696
public class BalloonAttachmentDoer : MonoBehaviour
{
	// Token: 0x0600694D RID: 26957 RVA: 0x00293320 File Offset: 0x00291520
	public void Initialize(GameActor target)
	{
		this.AttachTarget = target;
		this.m_currentOffset = new Vector2(1f, 2f);
		this.m_sprite = base.GetComponent<tk2dSprite>();
		this.m_mesh = new Mesh();
		this.m_vertices = new Vector3[20];
		this.m_mesh.vertices = this.m_vertices;
		int[] array = new int[54];
		Vector2[] array2 = new Vector2[20];
		int num = 0;
		for (int i = 0; i < 9; i++)
		{
			array[i * 6] = num;
			array[i * 6 + 1] = num + 2;
			array[i * 6 + 2] = num + 1;
			array[i * 6 + 3] = num + 2;
			array[i * 6 + 4] = num + 3;
			array[i * 6 + 5] = num + 1;
			num += 2;
		}
		this.m_mesh.triangles = array;
		this.m_mesh.uv = array2;
		GameObject gameObject = new GameObject("balloon string");
		this.m_stringFilter = gameObject.AddComponent<MeshFilter>();
		MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshRenderer.material = BraveResources.Load("Global VFX/WhiteMaterial", ".mat") as Material;
		this.m_stringFilter.mesh = this.m_mesh;
		base.transform.position = this.AttachTarget.transform.position + this.m_currentOffset.ToVector3ZisY(-3f);
	}

	// Token: 0x0600694E RID: 26958 RVA: 0x00293478 File Offset: 0x00291678
	private void LateUpdate()
	{
		if (this.AttachTarget != null)
		{
			if (this.AttachTarget is AIActor && (!this.AttachTarget || this.AttachTarget.healthHaver.IsDead))
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			this.m_currentOffset = new Vector2(Mathf.Lerp(0.5f, 1f, Mathf.PingPong(Time.realtimeSinceStartup, 3f) / 3f), Mathf.Lerp(1.33f, 2f, Mathf.PingPong(Time.realtimeSinceStartup / 1.75f, 3f) / 3f));
			Vector3 vector = this.AttachTarget.CenterPosition;
			if (this.AttachTarget is PlayerController)
			{
				PlayerHandController primaryHand = (this.AttachTarget as PlayerController).primaryHand;
				if (primaryHand.renderer.enabled)
				{
					vector = primaryHand.sprite.WorldCenter;
				}
			}
			Vector3 vector2 = this.AttachTarget.transform.position + this.m_currentOffset.ToVector3ZisY(-3f);
			float num = Vector3.Distance(base.transform.position, vector2);
			base.transform.position = Vector3.MoveTowards(base.transform.position, vector2, BraveMathCollege.UnboundedLerp(1f, 10f, num / 3f) * BraveTime.DeltaTime);
			this.BuildMeshAlongCurve(vector, vector, this.m_sprite.WorldCenter + new Vector2(0f, -2f), this.m_sprite.WorldCenter, 0.03125f);
			this.m_mesh.vertices = this.m_vertices;
			this.m_mesh.RecalculateBounds();
			this.m_mesh.RecalculateNormals();
		}
		if (!this.AttachTarget || this.AttachTarget.healthHaver.IsDead)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
	}

	// Token: 0x0600694F RID: 26959 RVA: 0x0029368C File Offset: 0x0029188C
	private void OnDestroy()
	{
		if (this.m_stringFilter)
		{
			UnityEngine.Object.Destroy(this.m_stringFilter.gameObject);
		}
	}

	// Token: 0x06006950 RID: 26960 RVA: 0x002936B0 File Offset: 0x002918B0
	private void BuildMeshAlongCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float meshWidth = 0.03125f)
	{
		Vector3[] vertices = this.m_vertices;
		Vector2? vector = null;
		for (int i = 0; i < 10; i++)
		{
			Vector2 vector2 = BraveMathCollege.CalculateBezierPoint((float)i / 9f, p0, p1, p2, p3);
			Vector2? vector3 = ((i != 9) ? new Vector2?(BraveMathCollege.CalculateBezierPoint((float)i / 9f, p0, p1, p2, p3)) : null);
			Vector2 vector4 = Vector2.zero;
			if (vector != null)
			{
				vector4 += (Quaternion.Euler(0f, 0f, 90f) * (vector2 - vector.Value)).XY().normalized;
			}
			if (vector3 != null)
			{
				vector4 += (Quaternion.Euler(0f, 0f, 90f) * (vector3.Value - vector2)).XY().normalized;
			}
			vector4 = vector4.normalized;
			vertices[i * 2] = (vector2 + vector4 * meshWidth).ToVector3ZisY(0f);
			vertices[i * 2 + 1] = (vector2 + -vector4 * meshWidth).ToVector3ZisY(0f);
			vector = new Vector2?(vector2);
		}
	}

	// Token: 0x040065A7 RID: 26023
	public GameActor AttachTarget;

	// Token: 0x040065A8 RID: 26024
	private Vector2 m_currentOffset;

	// Token: 0x040065A9 RID: 26025
	private Mesh m_mesh;

	// Token: 0x040065AA RID: 26026
	private Vector3[] m_vertices;

	// Token: 0x040065AB RID: 26027
	private tk2dSprite m_sprite;

	// Token: 0x040065AC RID: 26028
	private MeshFilter m_stringFilter;
}
