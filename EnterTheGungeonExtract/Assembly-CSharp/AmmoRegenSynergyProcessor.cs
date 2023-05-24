using System;
using UnityEngine;

// Token: 0x020016DA RID: 5850
public class AmmoRegenSynergyProcessor : MonoBehaviour
{
	// Token: 0x06008814 RID: 34836 RVA: 0x00386AB4 File Offset: 0x00384CB4
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
	}

	// Token: 0x06008815 RID: 34837 RVA: 0x00386AC4 File Offset: 0x00384CC4
	private void Update()
	{
		if (this.m_gun.CurrentOwner && this.m_gun.OwnerHasSynergy(this.RequiredSynergy) && (!this.PreventGainWhileFiring || !this.m_gun.IsFiring))
		{
			this.m_ammoCounter += BraveTime.DeltaTime * this.AmmoPerSecond;
			if (this.m_ammoCounter > 1f)
			{
				int num = Mathf.FloorToInt(this.m_ammoCounter);
				this.m_ammoCounter -= (float)num;
				this.m_gun.GainAmmo(num);
			}
		}
	}

	// Token: 0x06008816 RID: 34838 RVA: 0x00386B68 File Offset: 0x00384D68
	private void OnEnable()
	{
		if (this.m_gameTimeOnDisable > 0f)
		{
			this.m_ammoCounter += (Time.time - this.m_gameTimeOnDisable) * this.AmmoPerSecond;
			this.m_gameTimeOnDisable = 0f;
		}
	}

	// Token: 0x06008817 RID: 34839 RVA: 0x00386BA8 File Offset: 0x00384DA8
	private void OnDisable()
	{
		this.m_gameTimeOnDisable = Time.time;
	}

	// Token: 0x04008D51 RID: 36177
	[LongNumericEnum]
	public CustomSynergyType RequiredSynergy;

	// Token: 0x04008D52 RID: 36178
	public float AmmoPerSecond = 0.1f;

	// Token: 0x04008D53 RID: 36179
	public bool PreventGainWhileFiring = true;

	// Token: 0x04008D54 RID: 36180
	private Gun m_gun;

	// Token: 0x04008D55 RID: 36181
	private float m_ammoCounter;

	// Token: 0x04008D56 RID: 36182
	private float m_gameTimeOnDisable;
}
