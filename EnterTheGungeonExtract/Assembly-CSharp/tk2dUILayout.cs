using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C1B RID: 3099
[AddComponentMenu("2D Toolkit/UI/Core/tk2dUILayout")]
public class tk2dUILayout : MonoBehaviour
{
	// Token: 0x17000A15 RID: 2581
	// (get) Token: 0x0600429C RID: 17052 RVA: 0x001583CC File Offset: 0x001565CC
	public int ItemCount
	{
		get
		{
			return this.layoutItems.Count;
		}
	}

	// Token: 0x1400009D RID: 157
	// (add) Token: 0x0600429D RID: 17053 RVA: 0x001583DC File Offset: 0x001565DC
	// (remove) Token: 0x0600429E RID: 17054 RVA: 0x00158414 File Offset: 0x00156614
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Vector3, Vector3> OnReshape;

	// Token: 0x0600429F RID: 17055 RVA: 0x0015844C File Offset: 0x0015664C
	private void Reset()
	{
		if (base.GetComponent<Collider>() != null)
		{
			BoxCollider boxCollider = base.GetComponent<Collider>() as BoxCollider;
			if (boxCollider != null)
			{
				Bounds bounds = boxCollider.bounds;
				Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
				Vector3 position = base.transform.position;
				this.Reshape(worldToLocalMatrix.MultiplyPoint(bounds.min) - this.bMin, worldToLocalMatrix.MultiplyPoint(bounds.max) - this.bMax, true);
				Vector3 vector = worldToLocalMatrix.MultiplyVector(base.transform.position - position);
				Transform transform = base.transform;
				for (int i = 0; i < transform.childCount; i++)
				{
					Transform child = transform.GetChild(i);
					Vector3 vector2 = child.localPosition - vector;
					child.localPosition = vector2;
				}
				boxCollider.center -= vector;
				this.autoResizeCollider = true;
			}
		}
	}

	// Token: 0x060042A0 RID: 17056 RVA: 0x00158558 File Offset: 0x00156758
	public virtual void Reshape(Vector3 dMin, Vector3 dMax, bool updateChildren)
	{
		foreach (tk2dUILayoutItem tk2dUILayoutItem in this.layoutItems)
		{
			tk2dUILayoutItem.oldPos = tk2dUILayoutItem.gameObj.transform.position;
		}
		this.bMin += dMin;
		this.bMax += dMax;
		Vector3 vector = new Vector3(this.bMin.x, this.bMax.y);
		base.transform.position += base.transform.localToWorldMatrix.MultiplyVector(vector);
		this.bMin -= vector;
		this.bMax -= vector;
		if (this.autoResizeCollider)
		{
			BoxCollider component = base.GetComponent<BoxCollider>();
			if (component != null)
			{
				component.center += (dMin + dMax) / 2f - vector;
				component.size += dMax - dMin;
			}
		}
		foreach (tk2dUILayoutItem tk2dUILayoutItem2 in this.layoutItems)
		{
			Vector3 vector2 = base.transform.worldToLocalMatrix.MultiplyVector(tk2dUILayoutItem2.gameObj.transform.position - tk2dUILayoutItem2.oldPos);
			Vector3 vector3 = -vector2;
			Vector3 vector4 = -vector2;
			if (updateChildren)
			{
				vector3.x += ((!tk2dUILayoutItem2.snapLeft) ? ((!tk2dUILayoutItem2.snapRight) ? 0f : dMax.x) : dMin.x);
				vector3.y += ((!tk2dUILayoutItem2.snapBottom) ? ((!tk2dUILayoutItem2.snapTop) ? 0f : dMax.y) : dMin.y);
				vector4.x += ((!tk2dUILayoutItem2.snapRight) ? ((!tk2dUILayoutItem2.snapLeft) ? 0f : dMin.x) : dMax.x);
				vector4.y += ((!tk2dUILayoutItem2.snapTop) ? ((!tk2dUILayoutItem2.snapBottom) ? 0f : dMin.y) : dMax.y);
			}
			if (tk2dUILayoutItem2.sprite != null || tk2dUILayoutItem2.UIMask != null || tk2dUILayoutItem2.layout != null)
			{
				Matrix4x4 matrix4x = base.transform.localToWorldMatrix * tk2dUILayoutItem2.gameObj.transform.worldToLocalMatrix;
				vector3 = matrix4x.MultiplyVector(vector3);
				vector4 = matrix4x.MultiplyVector(vector4);
			}
			if (tk2dUILayoutItem2.sprite != null)
			{
				tk2dUILayoutItem2.sprite.ReshapeBounds(vector3, vector4);
			}
			else if (tk2dUILayoutItem2.UIMask != null)
			{
				tk2dUILayoutItem2.UIMask.ReshapeBounds(vector3, vector4);
			}
			else if (tk2dUILayoutItem2.layout != null)
			{
				tk2dUILayoutItem2.layout.Reshape(vector3, vector4, true);
			}
			else
			{
				Vector3 vector5 = vector3;
				if (tk2dUILayoutItem2.snapLeft && tk2dUILayoutItem2.snapRight)
				{
					vector5.x = 0.5f * (vector3.x + vector4.x);
				}
				if (tk2dUILayoutItem2.snapTop && tk2dUILayoutItem2.snapBottom)
				{
					vector5.y = 0.5f * (vector3.y + vector4.y);
				}
				tk2dUILayoutItem2.gameObj.transform.position += vector5;
			}
		}
		if (this.OnReshape != null)
		{
			this.OnReshape(dMin, dMax);
		}
	}

	// Token: 0x060042A1 RID: 17057 RVA: 0x001589F0 File Offset: 0x00156BF0
	public void SetBounds(Vector3 pMin, Vector3 pMax)
	{
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		this.Reshape(worldToLocalMatrix.MultiplyPoint(pMin) - this.bMin, worldToLocalMatrix.MultiplyPoint(pMax) - this.bMax, true);
	}

	// Token: 0x060042A2 RID: 17058 RVA: 0x00158A38 File Offset: 0x00156C38
	public Vector3 GetMinBounds()
	{
		return base.transform.localToWorldMatrix.MultiplyPoint(this.bMin);
	}

	// Token: 0x060042A3 RID: 17059 RVA: 0x00158A60 File Offset: 0x00156C60
	public Vector3 GetMaxBounds()
	{
		return base.transform.localToWorldMatrix.MultiplyPoint(this.bMax);
	}

	// Token: 0x060042A4 RID: 17060 RVA: 0x00158A88 File Offset: 0x00156C88
	public void Refresh()
	{
		this.Reshape(Vector3.zero, Vector3.zero, true);
	}

	// Token: 0x040034EA RID: 13546
	public Vector3 bMin = new Vector3(0f, -1f, 0f);

	// Token: 0x040034EB RID: 13547
	public Vector3 bMax = new Vector3(1f, 0f, 0f);

	// Token: 0x040034EC RID: 13548
	public List<tk2dUILayoutItem> layoutItems = new List<tk2dUILayoutItem>();

	// Token: 0x040034ED RID: 13549
	public bool autoResizeCollider;
}
