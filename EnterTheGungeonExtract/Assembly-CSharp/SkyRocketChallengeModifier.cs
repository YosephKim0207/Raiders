using System;
using UnityEngine;

// Token: 0x02001287 RID: 4743
public class SkyRocketChallengeModifier : ChallengeModifier
{
	// Token: 0x06006A2A RID: 27178 RVA: 0x00299F00 File Offset: 0x00298100
	private void Update()
	{
		this.m_elapsedSinceRocket += BraveTime.DeltaTime;
		if (this.m_elapsedSinceRocket > this.TimeBetweenRockets)
		{
			this.m_elapsedSinceRocket = 0f;
			this.FireRocket();
		}
		if (this.m_spawnedRockets > 0 && (BossKillCam.BossDeathCamRunning || GameManager.Instance.PreventPausing))
		{
			this.Cleanup();
		}
	}

	// Token: 0x06006A2B RID: 27179 RVA: 0x00299F6C File Offset: 0x0029816C
	private void OnDestroy()
	{
		this.Cleanup();
	}

	// Token: 0x06006A2C RID: 27180 RVA: 0x00299F74 File Offset: 0x00298174
	private void FireRocket()
	{
		if (BossKillCam.BossDeathCamRunning)
		{
			return;
		}
		if (GameManager.Instance.PreventPausing)
		{
			return;
		}
		PlayerController randomActivePlayer = GameManager.Instance.GetRandomActivePlayer();
		SkyRocket component = SpawnManager.SpawnProjectile(this.Rocket, Vector3.zero, Quaternion.identity, true).GetComponent<SkyRocket>();
		component.Target = randomActivePlayer.specRigidbody;
		tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
		component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
		this.m_spawnedRockets++;
	}

	// Token: 0x06006A2D RID: 27181 RVA: 0x0029A028 File Offset: 0x00298228
	public void Cleanup()
	{
		this.m_spawnedRockets = 0;
		SkyRocket[] array = UnityEngine.Object.FindObjectsOfType<SkyRocket>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i])
			{
				array[i].DieInAir();
			}
		}
	}

	// Token: 0x0400669F RID: 26271
	public GameObject Rocket;

	// Token: 0x040066A0 RID: 26272
	public float TimeBetweenRockets = 3f;

	// Token: 0x040066A1 RID: 26273
	private float m_elapsedSinceRocket;

	// Token: 0x040066A2 RID: 26274
	private int m_spawnedRockets;
}
