using System;
using UnityEngine;

// Token: 0x0200121A RID: 4634
public class SpotLightHelper : TimeInvariantMonoBehaviour
{
	// Token: 0x060067A8 RID: 26536 RVA: 0x00288CB0 File Offset: 0x00286EB0
	private void Start()
	{
		this.m_transform = base.transform;
		this.m_transform.position = this.m_transform.position.WithZ(this.m_transform.position.z + this.zHeightOffset);
		this.m_light = base.GetComponent<Light>();
		if (this.randomStartingRotation)
		{
			this.m_transform.rotation = Quaternion.Euler(this.m_transform.rotation.x, UnityEngine.Random.Range(this.rotationMin, this.rotationMax), this.m_transform.rotation.z);
		}
		if (this.pointDirectlyAtFloor)
		{
			this.magicNumberAngle = 45f;
			this.m_transform.rotation = Quaternion.Euler(this.magicNumberAngle, this.m_transform.rotation.y, this.m_transform.rotation.z);
		}
		if (this.randomStartingCookieAngle)
		{
			this.m_light.spotAngle = UnityEngine.Random.Range(this.cookieAngleMin, this.cookieAngleMax);
		}
		if (this.randomIntensity)
		{
			this.m_light.intensity = UnityEngine.Random.Range(this.intensityMin, this.intensityMax);
		}
	}

	// Token: 0x060067A9 RID: 26537 RVA: 0x00288E00 File Offset: 0x00287000
	protected override void InvariantUpdate(float realDeltaTime)
	{
		if (this.rotationSpeed != 0f)
		{
			this.m_transform.Rotate(0f, 0f, this.rotationSpeed * realDeltaTime);
		}
		if (this.swayRotation)
		{
			Quaternion quaternion = Quaternion.Euler(this.magicNumberAngle, this.rotationMin, this.m_transform.rotation.z);
			Quaternion quaternion2 = Quaternion.Euler(this.magicNumberAngle, this.rotationMax, this.m_transform.rotation.z);
			float num = 0.5f * (1f + Mathf.Sin(3.1415927f * Time.realtimeSinceStartup * (this.swaySpeed / 10f)));
			this.m_transform.rotation = Quaternion.Lerp(quaternion, quaternion2, num);
		}
		if (this.pulseCookieAngle)
		{
			this.m_light.spotAngle = Mathf.SmoothStep(this.cookieAngleMin, this.cookieAngleMax, Mathf.PingPong(Time.time / this.pulseCookieAngleSpeed4Real, this.pulseCookieAngleHang));
		}
		if (this.pulseIntensity)
		{
			this.m_light.intensity = Mathf.SmoothStep(this.intensityMin, this.intensityMax, Mathf.PingPong(Time.time / this.pulseIntensitySpeed4Real, this.pulseIntensityHang));
		}
		if (this.doPingPong)
		{
			RenderSettings.ambientLight = Color.Lerp(this.startColor, this.endColor, Mathf.PingPong(Time.time * this.otherNumber, this.pingPongTime));
		}
	}

	// Token: 0x060067AA RID: 26538 RVA: 0x00288F84 File Offset: 0x00287184
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400638B RID: 25483
	[Header("Inital Position and Intensity")]
	public bool pointDirectlyAtFloor;

	// Token: 0x0400638C RID: 25484
	public float zHeightOffset = -30f;

	// Token: 0x0400638D RID: 25485
	[Header("Cookie Rotation")]
	public bool randomStartingRotation;

	// Token: 0x0400638E RID: 25486
	public bool swayRotation;

	// Token: 0x0400638F RID: 25487
	public float swaySpeed = 0.18f;

	// Token: 0x04006390 RID: 25488
	public float rotationMin;

	// Token: 0x04006391 RID: 25489
	public float rotationMax;

	// Token: 0x04006392 RID: 25490
	[Header("Constant Rotation")]
	public float rotationSpeed;

	// Token: 0x04006393 RID: 25491
	[Header("Inital Spot/Cookie Angle")]
	public bool randomStartingCookieAngle;

	// Token: 0x04006394 RID: 25492
	public bool pulseCookieAngle;

	// Token: 0x04006395 RID: 25493
	public float pulseCookieAngleHang = 1f;

	// Token: 0x04006396 RID: 25494
	public float pulseCookieAngleSpeed4Real = 1f;

	// Token: 0x04006397 RID: 25495
	public float cookieAngleMin;

	// Token: 0x04006398 RID: 25496
	public float cookieAngleMax;

	// Token: 0x04006399 RID: 25497
	[Header("Light Intensity")]
	public bool randomIntensity;

	// Token: 0x0400639A RID: 25498
	public bool pulseIntensity;

	// Token: 0x0400639B RID: 25499
	public float pulseIntensityHang;

	// Token: 0x0400639C RID: 25500
	public float pulseIntensitySpeed4Real = 10f;

	// Token: 0x0400639D RID: 25501
	public float intensityMin;

	// Token: 0x0400639E RID: 25502
	public float intensityMax;

	// Token: 0x0400639F RID: 25503
	[Header("Ambient Light Ping Pong")]
	public bool doPingPong;

	// Token: 0x040063A0 RID: 25504
	public Color startColor = Color.blue;

	// Token: 0x040063A1 RID: 25505
	public Color endColor = Color.red;

	// Token: 0x040063A2 RID: 25506
	public float pingPongTime = 2f;

	// Token: 0x040063A3 RID: 25507
	public float otherNumber = 1.5f;

	// Token: 0x040063A4 RID: 25508
	protected Transform m_transform;

	// Token: 0x040063A5 RID: 25509
	protected float magicNumberAngle;

	// Token: 0x040063A6 RID: 25510
	protected Light m_light;
}
