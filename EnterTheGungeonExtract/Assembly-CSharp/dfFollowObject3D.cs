using System;
using UnityEngine;

// Token: 0x02000464 RID: 1124
[ExecuteInEditMode]
[AddComponentMenu("Daikon Forge/Examples/3D/Follow Object (3D)")]
public class dfFollowObject3D : MonoBehaviour
{
	// Token: 0x06001A02 RID: 6658 RVA: 0x000796C8 File Offset: 0x000778C8
	public void OnEnable()
	{
		this.control = base.GetComponent<dfControl>();
		this.Update();
	}

	// Token: 0x06001A03 RID: 6659 RVA: 0x000796DC File Offset: 0x000778DC
	public void Update()
	{
		if (this.lastLiveUpdateValue != this.liveUpdate)
		{
			this.lastLiveUpdateValue = this.liveUpdate;
			if (!this.liveUpdate)
			{
				this.control.RelativePosition = this.designTimePosition;
				this.control.transform.localScale = Vector3.one;
				this.control.transform.localRotation = Quaternion.identity;
			}
			else
			{
				this.designTimePosition = this.control.RelativePosition;
			}
			this.control.Invalidate();
		}
		if (this.liveUpdate || Application.isPlaying)
		{
			this.updatePosition3D();
		}
	}

	// Token: 0x06001A04 RID: 6660 RVA: 0x00079788 File Offset: 0x00077988
	public void OnDrawGizmos()
	{
		if (this.control == null)
		{
			this.control = base.GetComponent<dfControl>();
		}
		Vector3 vector = this.control.Size * this.control.PixelsToUnits();
		Gizmos.matrix = Matrix4x4.TRS(this.attachedTo.position, this.attachedTo.rotation, this.attachedTo.localScale);
		Gizmos.color = Color.clear;
		Gizmos.DrawCube(Vector3.zero, vector);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(Vector3.zero, vector);
	}

	// Token: 0x06001A05 RID: 6661 RVA: 0x00079828 File Offset: 0x00077A28
	public void OnDrawGizmosSelected()
	{
		this.OnDrawGizmos();
	}

	// Token: 0x06001A06 RID: 6662 RVA: 0x00079830 File Offset: 0x00077A30
	private void updatePosition3D()
	{
		if (this.attachedTo == null)
		{
			return;
		}
		this.control.transform.position = this.attachedTo.position;
		this.control.transform.rotation = this.attachedTo.rotation;
		this.control.transform.localScale = this.attachedTo.localScale;
	}

	// Token: 0x04001467 RID: 5223
	public Transform attachedTo;

	// Token: 0x04001468 RID: 5224
	public bool liveUpdate;

	// Token: 0x04001469 RID: 5225
	[HideInInspector]
	[SerializeField]
	protected Vector3 designTimePosition;

	// Token: 0x0400146A RID: 5226
	private dfControl control;

	// Token: 0x0400146B RID: 5227
	private bool lastLiveUpdateValue;
}
