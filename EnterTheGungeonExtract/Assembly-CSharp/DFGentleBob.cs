using System;
using UnityEngine;

// Token: 0x0200175D RID: 5981
public class DFGentleBob : MonoBehaviour
{
	// Token: 0x170014C4 RID: 5316
	// (get) Token: 0x06008B37 RID: 35639 RVA: 0x0039F624 File Offset: 0x0039D824
	// (set) Token: 0x06008B38 RID: 35640 RVA: 0x0039F62C File Offset: 0x0039D82C
	public Vector3 AbsoluteStartPosition
	{
		get
		{
			return this.m_startAbsolutePosition;
		}
		set
		{
			this.m_startAbsolutePosition = value;
		}
	}

	// Token: 0x06008B39 RID: 35641 RVA: 0x0039F638 File Offset: 0x0039D838
	private void Start()
	{
		this.m_transform = base.transform;
		this.m_control = base.GetComponent<dfControl>();
		this.m_rigidbody = base.GetComponent<SpeculativeRigidbody>();
		this.m_startAbsolutePosition = this.m_transform.position;
		if (this.m_control != null)
		{
			this.m_startRelativePosition = this.m_control.RelativePosition;
		}
		this.t = UnityEngine.Random.value;
	}

	// Token: 0x06008B3A RID: 35642 RVA: 0x0039F6A8 File Offset: 0x0039D8A8
	private void Update()
	{
		if (this.t == 0f)
		{
			this.t = UnityEngine.Random.value;
		}
		float num = ((!this.BobDuringBossIntros || !GameManager.IsBossIntro) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
		this.t += num * this.bounceSpeed;
		if (this.m_control != null)
		{
			this.m_control.RelativePosition = this.m_startRelativePosition + new Vector3(0f, Mathf.Lerp((float)this.upPixels, (float)this.downPixels, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, Mathf.PingPong(this.t, 1f)))), 0f);
		}
		else if (this.m_rigidbody != null)
		{
			Vector3 vector = this.m_startAbsolutePosition + new Vector3(0f, 0.0625f * Mathf.Lerp((float)this.upPixels, (float)(-(float)this.downPixels), Mathf.SmoothStep(0f, 1f, Mathf.PingPong(this.t, 1f))), 0f).Quantize(0.0625f);
			Vector2 vector2 = vector.XY() - base.transform.position.XY();
			this.m_rigidbody.Velocity = vector2 / num;
		}
		else
		{
			this.m_transform.position = this.m_startAbsolutePosition + new Vector3(0f, 0.0625f * Mathf.Lerp((float)this.upPixels, (float)(-(float)this.downPixels), Mathf.SmoothStep(0f, 1f, Mathf.PingPong(this.t, 1f))), 0f);
			if (this.Quantized)
			{
				this.m_transform.position = this.m_transform.position.Quantize(0.0625f);
			}
		}
	}

	// Token: 0x06008B3B RID: 35643 RVA: 0x0039F8B4 File Offset: 0x0039DAB4
	private void OnDisable()
	{
		if (this.m_rigidbody)
		{
			this.m_rigidbody.Velocity = Vector2.zero;
		}
	}

	// Token: 0x04009204 RID: 37380
	public int upPixels = 6;

	// Token: 0x04009205 RID: 37381
	public int downPixels = 6;

	// Token: 0x04009206 RID: 37382
	public float bounceSpeed = 1f;

	// Token: 0x04009207 RID: 37383
	public bool Quantized;

	// Token: 0x04009208 RID: 37384
	private dfControl m_control;

	// Token: 0x04009209 RID: 37385
	private SpeculativeRigidbody m_rigidbody;

	// Token: 0x0400920A RID: 37386
	private Transform m_transform;

	// Token: 0x0400920B RID: 37387
	public bool BobDuringBossIntros;

	// Token: 0x0400920C RID: 37388
	private Vector3 m_startAbsolutePosition;

	// Token: 0x0400920D RID: 37389
	private Vector3 m_startRelativePosition;

	// Token: 0x0400920E RID: 37390
	private float t;
}
