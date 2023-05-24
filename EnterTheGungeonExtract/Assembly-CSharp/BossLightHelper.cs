using System;
using System.Collections;
using Dungeonator;
using UnityEngine;

// Token: 0x02001103 RID: 4355
[RequireComponent(typeof(SpotLightHelper))]
[RequireComponent(typeof(Light))]
public class BossLightHelper : TimeInvariantMonoBehaviour
{
	// Token: 0x06006011 RID: 24593 RVA: 0x0024FCB8 File Offset: 0x0024DEB8
	public void Start()
	{
		RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		this.m_bossHealth = roomFromPosition.GetActiveEnemies(RoomHandler.ActiveEnemyType.All)[0].healthHaver;
		this.m_light = base.GetComponent<Light>();
		this.m_lightHelper = base.GetComponent<SpotLightHelper>();
		this.m_startRotation = this.m_lightHelper.rotationSpeed;
		this.m_startIntensity = this.m_light.intensity;
	}

	// Token: 0x06006012 RID: 24594 RVA: 0x0024FD38 File Offset: 0x0024DF38
	protected override void InvariantUpdate(float realDeltaTime)
	{
		if (!this.m_isDead)
		{
			this.m_lightHelper.rotationSpeed = Mathf.Lerp(this.m_startRotation, this.MaxRotation, 1f - this.m_bossHealth.GetCurrentHealthPercentage());
		}
		if (this.m_bossHealth.IsDead && !this.m_isDead)
		{
			this.m_isDead = true;
			base.StartCoroutine(this.DeathEffects());
		}
		if (this.m_isDead || this.m_bossHealth.GetCurrentHealthPercentage() <= this.PulseThreshold)
		{
			this.m_pulseTimer += realDeltaTime;
			this.m_light.intensity = Mathf.Lerp(this.m_startIntensity, this.PulseMaxIntensity, Mathf.PingPong(this.m_pulseTimer, this.PulsePeriod) / this.PulsePeriod);
		}
	}

	// Token: 0x06006013 RID: 24595 RVA: 0x0024FE10 File Offset: 0x0024E010
	private IEnumerator DeathEffects()
	{
		float timer = 0f;
		float startMaxIntensity = this.PulseMaxIntensity;
		yield return null;
		for (;;)
		{
			timer += BraveTime.DeltaTime;
			this.m_lightHelper.rotationSpeed = Mathf.Lerp(this.MaxRotation, 0f, Mathf.Clamp01(timer / this.RotationStopTime));
			this.PulseMaxIntensity = Mathf.Lerp(startMaxIntensity, this.m_startIntensity, Mathf.Clamp01(timer / this.PulseStopTime));
			if (timer > this.RotationStopTime && timer > this.PulseStopTime)
			{
				break;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06006014 RID: 24596 RVA: 0x0024FE2C File Offset: 0x0024E02C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04005A93 RID: 23187
	public float MaxRotation = 360f;

	// Token: 0x04005A94 RID: 23188
	[Header("Intensity Pulse")]
	public float PulseThreshold = 0.2f;

	// Token: 0x04005A95 RID: 23189
	public float PulseMaxIntensity = 8f;

	// Token: 0x04005A96 RID: 23190
	public float PulsePeriod = 1f;

	// Token: 0x04005A97 RID: 23191
	[Header("On Death")]
	public float PulseStopTime = 5f;

	// Token: 0x04005A98 RID: 23192
	public float RotationStopTime = 10f;

	// Token: 0x04005A99 RID: 23193
	private HealthHaver m_bossHealth;

	// Token: 0x04005A9A RID: 23194
	private Light m_light;

	// Token: 0x04005A9B RID: 23195
	private SpotLightHelper m_lightHelper;

	// Token: 0x04005A9C RID: 23196
	private float m_startRotation;

	// Token: 0x04005A9D RID: 23197
	private float m_startIntensity;

	// Token: 0x04005A9E RID: 23198
	private float m_pulseTimer;

	// Token: 0x04005A9F RID: 23199
	private bool m_isDead;
}
