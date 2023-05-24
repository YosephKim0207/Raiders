using System;
using UnityEngine;

// Token: 0x020013D6 RID: 5078
public class ProjectileSpawnedTrailModifier : MonoBehaviour
{
	// Token: 0x0600733F RID: 29503 RVA: 0x002DDB54 File Offset: 0x002DBD54
	private void Start()
	{
		this.m_projectile = base.GetComponent<Projectile>();
		this.m_srb = base.GetComponent<SpeculativeRigidbody>();
	}

	// Token: 0x06007340 RID: 29504 RVA: 0x002DDB70 File Offset: 0x002DBD70
	private void Update()
	{
		this.m_elapsed += BraveTime.DeltaTime;
		if (this.m_elapsed > this.SpawnPeriod)
		{
			if (this.InFlightSpawnTransform)
			{
				this.m_elapsed -= this.SpawnPeriod;
				SpawnManager.SpawnVFX(this.TrailPrefab, this.InFlightSpawnTransform.position + this.WorldSpaceSpawnOffset, Quaternion.identity);
			}
			else
			{
				this.m_elapsed -= this.SpawnPeriod;
				SpawnManager.SpawnVFX(this.TrailPrefab, this.m_srb.UnitCenter.ToVector3ZisY(0f) + this.WorldSpaceSpawnOffset, Quaternion.identity);
			}
			if (!string.IsNullOrEmpty(this.spawnAudioEvent))
			{
				AkSoundEngine.PostEvent(this.spawnAudioEvent, base.gameObject);
			}
		}
	}

	// Token: 0x040074C2 RID: 29890
	public GameObject TrailPrefab;

	// Token: 0x040074C3 RID: 29891
	public string spawnAudioEvent;

	// Token: 0x040074C4 RID: 29892
	public Transform InFlightSpawnTransform;

	// Token: 0x040074C5 RID: 29893
	public Vector3 WorldSpaceSpawnOffset;

	// Token: 0x040074C6 RID: 29894
	public float SpawnPeriod;

	// Token: 0x040074C7 RID: 29895
	private float m_elapsed;

	// Token: 0x040074C8 RID: 29896
	private Projectile m_projectile;

	// Token: 0x040074C9 RID: 29897
	private SpeculativeRigidbody m_srb;
}
