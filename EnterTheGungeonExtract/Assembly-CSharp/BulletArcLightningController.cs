using System;
using UnityEngine;

// Token: 0x02001080 RID: 4224
public class BulletArcLightningController : MonoBehaviour
{
	// Token: 0x06005CF4 RID: 23796 RVA: 0x00239628 File Offset: 0x00237828
	public void Initialize(Vector2 centerPoint, float velocity, string OwnerName, float startAngle = 0f, float endAngle = 360f, float startRadius = 0f)
	{
		this.m_ownerName = OwnerName;
		this.m_center = centerPoint;
		this.m_velocity = velocity;
		this.m_startAngle = startAngle;
		this.m_endAngle = endAngle;
		this.m_currentRadius = startRadius;
		this.m_line = base.gameObject.GetOrAddComponent<LineRenderer>();
		this.m_linePoints = new Vector3[16];
		this.m_line.SetVertexCount(this.m_linePoints.Length);
		this.m_line.SetPositions(this.m_linePoints);
		this.m_line.SetWidth(1f, 1f);
		this.m_line.material = BraveResources.Load("Global VFX/ArcLightningMaterial", ".mat") as Material;
		while (this.m_startAngle > this.m_endAngle)
		{
			this.m_endAngle += 360f;
		}
	}

	// Token: 0x06005CF5 RID: 23797 RVA: 0x00239700 File Offset: 0x00237900
	public void UpdateCenter(Vector2 newCenter)
	{
		this.m_center = newCenter;
	}

	// Token: 0x06005CF6 RID: 23798 RVA: 0x0023970C File Offset: 0x0023790C
	public void Update()
	{
		float num = this.m_velocity * BraveTime.DeltaTime;
		this.m_currentRadius += num;
		this.UpdateRendering();
		this.UpdateCollision();
	}

	// Token: 0x06005CF7 RID: 23799 RVA: 0x00239740 File Offset: 0x00237940
	public void OnDespawned()
	{
		if (this.m_line)
		{
			for (int i = 0; i < this.m_linePoints.Length; i++)
			{
				this.m_linePoints[i] = Vector3.zero;
			}
			this.m_line.SetPositions(this.m_linePoints);
			UnityEngine.Object.Destroy(this.m_line);
		}
		this.m_line = null;
		this.m_linePoints = null;
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x06005CF8 RID: 23800 RVA: 0x002397BC File Offset: 0x002379BC
	private void UpdateRendering()
	{
		float num = this.m_endAngle - this.m_startAngle;
		float num2 = num / (float)this.m_linePoints.Length;
		for (int i = 0; i < this.m_linePoints.Length; i++)
		{
			this.m_linePoints[i] = (this.m_center + BraveMathCollege.DegreesToVector(this.m_startAngle + (float)i * num2, this.m_currentRadius)).ToVector3ZisY(0f);
		}
		this.m_line.SetPositions(this.m_linePoints);
	}

	// Token: 0x06005CF9 RID: 23801 RVA: 0x0023984C File Offset: 0x00237A4C
	private bool IsBetweenAngles(Vector2 circleCenter, Vector2 point, float startAngle, float endAngle)
	{
		float num = ((point - circleCenter).ToAngle() + 360f) % 360f;
		return endAngle >= num && startAngle <= num;
	}

	// Token: 0x06005CFA RID: 23802 RVA: 0x00239884 File Offset: 0x00237A84
	public bool ArcIntersectsLine(Vector2 circleCenter, float radius, float startAngle, float endAngle, Vector2 point1, Vector2 point2)
	{
		Vector2 vector = point1 - circleCenter;
		Vector2 vector2 = point2 - circleCenter;
		Vector2 vector3 = vector2 - vector;
		float num = Vector2.Dot(vector3, vector3);
		float num2 = 2f * Vector2.Dot(vector, vector3);
		float num3 = Vector2.Dot(vector, vector) - radius * radius;
		float num4 = Mathf.Pow(num2, 2f) - 4f * num * num3;
		if (num4 > 0f)
		{
			float num5;
			if (num2 >= 0f)
			{
				num5 = (-num2 - Mathf.Sqrt(num4)) / 2f;
			}
			else
			{
				num5 = (-num2 + Mathf.Sqrt(num4)) / 2f;
			}
			float num6 = num5 / num;
			float num7 = num3 / num5;
			if (num7 < num6)
			{
				float num8 = num7;
				num7 = num6;
				num6 = num8;
			}
			if (0.0 <= (double)num6 && (double)num6 <= 1.0)
			{
				Vector2 vector4 = circleCenter + Vector2.Lerp(vector, vector2, num6);
				if (this.IsBetweenAngles(circleCenter, vector4, startAngle, endAngle))
				{
					return true;
				}
			}
			if (0.0 <= (double)num7 && (double)num7 <= 1.0)
			{
				Vector2 vector5 = circleCenter + Vector2.Lerp(vector, vector2, num7);
				if (this.IsBetweenAngles(circleCenter, vector5, startAngle, endAngle))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x06005CFB RID: 23803 RVA: 0x002399E0 File Offset: 0x00237BE0
	private bool ArcSliceIntersectsAABB(Vector2 centerPoint, float startAngle, float endAngle, float startRadius, float endRadius, Vector2 aabbBottomLeft, Vector2 aabbTopRight)
	{
		Vector2 vector = aabbBottomLeft;
		Vector2 vector2 = aabbTopRight;
		Vector2 vector3 = new Vector2(vector2.x, vector.y);
		Vector2 vector4 = new Vector2(vector.x, vector2.y);
		bool flag = this.ArcIntersectsLine(centerPoint, endRadius, startAngle, endAngle, vector, vector3);
		if (!flag)
		{
			flag = this.ArcIntersectsLine(centerPoint, endRadius, startAngle, endAngle, vector3, vector2);
		}
		if (!flag)
		{
			flag = this.ArcIntersectsLine(centerPoint, endRadius, startAngle, endAngle, vector4, vector2);
		}
		if (!flag)
		{
			flag = this.ArcIntersectsLine(centerPoint, endRadius, startAngle, endAngle, vector, vector4);
		}
		return flag;
	}

	// Token: 0x06005CFC RID: 23804 RVA: 0x00239A70 File Offset: 0x00237C70
	private void UpdateCollision()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (playerController && !playerController.IsGhost && playerController.healthHaver && playerController.healthHaver.IsAlive && playerController.healthHaver.IsVulnerable)
			{
				Vector2 zero = Vector2.zero;
				bool flag = this.ArcSliceIntersectsAABB(this.m_center, this.m_startAngle, this.m_endAngle, this.m_currentRadius, this.m_currentRadius, playerController.specRigidbody.HitboxPixelCollider.UnitBottomLeft, playerController.specRigidbody.HitboxPixelCollider.UnitTopRight);
				if (flag)
				{
					playerController.healthHaver.ApplyDamage(0.5f, Vector2.zero, this.m_ownerName, CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
				}
			}
		}
	}

	// Token: 0x040056B5 RID: 22197
	private Vector2 m_center;

	// Token: 0x040056B6 RID: 22198
	private float m_velocity;

	// Token: 0x040056B7 RID: 22199
	private float m_startAngle;

	// Token: 0x040056B8 RID: 22200
	private float m_endAngle;

	// Token: 0x040056B9 RID: 22201
	private float m_currentRadius;

	// Token: 0x040056BA RID: 22202
	private string m_ownerName;

	// Token: 0x040056BB RID: 22203
	private LineRenderer m_line;

	// Token: 0x040056BC RID: 22204
	private Vector3[] m_linePoints;
}
