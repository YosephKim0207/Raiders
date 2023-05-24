using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020012B3 RID: 4787
public class ScarfAttachmentDoer : MonoBehaviour
{
	// Token: 0x06006B1A RID: 27418 RVA: 0x002A0E34 File Offset: 0x0029F034
	public void Initialize(GameActor target)
	{
		this.m_targetLength = this.ScarfLength;
		this.AttachTarget = target;
		this.m_currentOffset = new Vector2(1f, 2f);
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
		this.m_mr = gameObject.AddComponent<MeshRenderer>();
		this.m_mr.material = this.ScarfMaterial;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		this.m_stringFilter.mesh = this.m_mesh;
		base.transform.position = this.AttachTarget.transform.position + this.m_currentOffset.ToVector3ZisY(-3f);
	}

	// Token: 0x06006B1B RID: 27419 RVA: 0x002A0F98 File Offset: 0x0029F198
	private void LateUpdate()
	{
		if (GameManager.Instance.IsLoadingLevel || Dungeon.IsGenerating)
		{
			return;
		}
		if (this.AttachTarget != null)
		{
			if (this.AttachTarget is AIActor && (!this.AttachTarget || this.AttachTarget.healthHaver.IsDead))
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			this.m_targetLength = this.ScarfLength;
			bool flag = false;
			if (this.AttachTarget is PlayerController)
			{
				PlayerController playerController = this.AttachTarget as PlayerController;
				this.m_mr.enabled = playerController.IsVisible && playerController.sprite.renderer.enabled;
				this.m_mr.gameObject.layer = playerController.gameObject.layer;
				if (playerController.FacingDirection <= 155f && playerController.FacingDirection >= 25f)
				{
					flag = true;
				}
				if (playerController.IsFalling)
				{
					this.m_targetLength = 0.05f;
				}
			}
			this.m_currentLength = Mathf.MoveTowards(this.m_currentLength, this.m_targetLength, BraveTime.DeltaTime * 2.5f);
			if (this.m_currentLength < 0.1f)
			{
				this.m_mr.enabled = false;
			}
			Vector2 lastCommandedDirection = (this.AttachTarget as PlayerController).LastCommandedDirection;
			if (lastCommandedDirection.magnitude < 0.125f)
			{
				this.m_isLerpingBack = true;
			}
			else
			{
				this.m_isLerpingBack = false;
			}
			float num = this.m_lastVelAngle;
			if (this.m_isLerpingBack)
			{
				float num2 = Mathf.DeltaAngle(this.m_lastVelAngle, -45f);
				float num3 = Mathf.DeltaAngle(this.m_lastVelAngle, 135f);
				float num4 = (float)((num2 <= num3) ? 0 : 180);
				num = num4;
			}
			else
			{
				num = BraveMathCollege.Atan2Degrees(lastCommandedDirection);
			}
			this.m_lastVelAngle = Mathf.LerpAngle(this.m_lastVelAngle, num, BraveTime.DeltaTime * this.AngleLerpSpeed * Mathf.Lerp(1f, 2f, Mathf.DeltaAngle(this.m_lastVelAngle, num) / 180f));
			float num5 = this.m_currentLength * Mathf.Lerp(2f, 1f, Vector2.Distance(base.transform.position.XY(), this.AttachTarget.sprite.WorldCenter) / 3f);
			this.m_currentOffset = (Quaternion.Euler(0f, 0f, this.m_lastVelAngle) * Vector2.left * num5).XY();
			Vector2 vector = Vector2.Lerp(this.MinOffset, this.MaxOffset, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(Time.realtimeSinceStartup * this.AnimationSpeed, 3f) / 3f));
			this.m_currentOffset += vector;
			Vector3 vector2 = this.AttachTarget.sprite.WorldCenter + new Vector2(0f, -0.3125f);
			Vector3 vector3 = vector2 + this.m_currentOffset.ToVector3ZisY(-3f);
			float num6 = Vector3.Distance(base.transform.position, vector3);
			if (num6 > 10f)
			{
				base.transform.position = vector3;
			}
			else
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, vector3, BraveMathCollege.UnboundedLerp(1f, 10f, num6 / this.CatchUpScale) * BraveTime.DeltaTime);
			}
			Vector2 vector4 = vector3.XY() - base.transform.position.XY();
			this.m_additionalOffsetTime += UnityEngine.Random.Range(0f, BraveTime.DeltaTime);
			this.BuildMeshAlongCurve(vector2, vector2.XY() + new Vector2(0f, 0.1f), base.transform.position.XY() + vector4, base.transform.position.XY(), (!flag) ? this.ForwardZOffset : this.BackwardZOffset);
			this.m_mesh.vertices = this.m_vertices;
			this.m_mesh.RecalculateBounds();
			this.m_mesh.RecalculateNormals();
		}
	}

	// Token: 0x06006B1C RID: 27420 RVA: 0x002A1408 File Offset: 0x0029F608
	private void OnDestroy()
	{
		if (this.m_stringFilter)
		{
			UnityEngine.Object.Destroy(this.m_stringFilter.gameObject);
		}
	}

	// Token: 0x06006B1D RID: 27421 RVA: 0x002A142C File Offset: 0x0029F62C
	private void BuildMeshAlongCurve(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float zOffset)
	{
		Vector3[] vertices = this.m_vertices;
		Vector2? vector = null;
		Vector2 vector2 = p3 - p0;
		Vector2 vector3 = (Quaternion.Euler(0f, 0f, 90f) * vector2).XY();
		for (int i = 0; i < 10; i++)
		{
			Vector2 vector4 = BraveMathCollege.CalculateBezierPoint((float)i / 9f, p0, p1, p2, p3);
			Vector2? vector5 = ((i != 9) ? new Vector2?(BraveMathCollege.CalculateBezierPoint((float)i / 9f, p0, p1, p2, p3)) : null);
			Vector2 vector6 = Vector2.zero;
			if (vector != null)
			{
				vector6 += (Quaternion.Euler(0f, 0f, 90f) * (vector4 - vector.Value)).XY().normalized;
			}
			if (vector5 != null)
			{
				vector6 += (Quaternion.Euler(0f, 0f, 90f) * (vector5.Value - vector4)).XY().normalized;
			}
			vector6 = vector6.normalized;
			float num = Mathf.Lerp(this.StartWidth, this.EndWidth, (float)i / 9f);
			vector4 += vector3.normalized * Mathf.Sin(Time.realtimeSinceStartup * this.SinSpeed + (float)i * this.WavelengthMod) * this.AmplitudeMod * ((float)i / 9f);
			vertices[i * 2] = (vector4 + vector6 * num).ToVector3ZisY(zOffset);
			vertices[i * 2 + 1] = (vector4 + -vector6 * num).ToVector3ZisY(zOffset);
			vector = new Vector2?(vector4);
		}
	}

	// Token: 0x040067EE RID: 26606
	public float StartWidth = 0.0625f;

	// Token: 0x040067EF RID: 26607
	public float EndWidth = 0.125f;

	// Token: 0x040067F0 RID: 26608
	public float AnimationSpeed = 1f;

	// Token: 0x040067F1 RID: 26609
	public float ScarfLength = 1.5f;

	// Token: 0x040067F2 RID: 26610
	public float AngleLerpSpeed = 15f;

	// Token: 0x040067F3 RID: 26611
	public float ForwardZOffset;

	// Token: 0x040067F4 RID: 26612
	public float BackwardZOffset = -0.2f;

	// Token: 0x040067F5 RID: 26613
	public Vector2 MinOffset;

	// Token: 0x040067F6 RID: 26614
	public Vector2 MaxOffset;

	// Token: 0x040067F7 RID: 26615
	public float CatchUpScale = 2f;

	// Token: 0x040067F8 RID: 26616
	public GameActor AttachTarget;

	// Token: 0x040067F9 RID: 26617
	public Material ScarfMaterial;

	// Token: 0x040067FA RID: 26618
	private float m_additionalOffsetTime;

	// Token: 0x040067FB RID: 26619
	private Vector2 m_currentOffset;

	// Token: 0x040067FC RID: 26620
	private Mesh m_mesh;

	// Token: 0x040067FD RID: 26621
	private Vector3[] m_vertices;

	// Token: 0x040067FE RID: 26622
	private MeshFilter m_stringFilter;

	// Token: 0x040067FF RID: 26623
	private MeshRenderer m_mr;

	// Token: 0x04006800 RID: 26624
	private float m_lastVelAngle;

	// Token: 0x04006801 RID: 26625
	private bool m_isLerpingBack;

	// Token: 0x04006802 RID: 26626
	private float m_targetLength;

	// Token: 0x04006803 RID: 26627
	private float m_currentLength = 0.05f;

	// Token: 0x04006804 RID: 26628
	public float SinSpeed = 1f;

	// Token: 0x04006805 RID: 26629
	public float AmplitudeMod = 0.25f;

	// Token: 0x04006806 RID: 26630
	public float WavelengthMod = 1f;
}
