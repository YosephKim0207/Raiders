using System;
using UnityEngine;

// Token: 0x020016C0 RID: 5824
public class ShellCasingSpawner : BraveBehaviour
{
	// Token: 0x0600876C RID: 34668 RVA: 0x003828F0 File Offset: 0x00380AF0
	public void Start()
	{
		this.m_shouldSpawn = true;
	}

	// Token: 0x0600876D RID: 34669 RVA: 0x003828FC File Offset: 0x00380AFC
	public void OnSpawned()
	{
		this.m_shouldSpawn = true;
	}

	// Token: 0x0600876E RID: 34670 RVA: 0x00382908 File Offset: 0x00380B08
	public void Update()
	{
		if (this.m_shouldSpawn)
		{
			this.SpawnShells();
			this.m_shouldSpawn = false;
		}
	}

	// Token: 0x0600876F RID: 34671 RVA: 0x00382924 File Offset: 0x00380B24
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06008770 RID: 34672 RVA: 0x0038292C File Offset: 0x00380B2C
	private void SpawnShells()
	{
		if (GameManager.Options.DebrisQuantity != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.DebrisQuantity != GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			for (int i = 0; i < this.shellsToLaunch; i++)
			{
				this.SpawnShellCasingAtPosition(base.transform.position);
			}
		}
	}

	// Token: 0x06008771 RID: 34673 RVA: 0x00382980 File Offset: 0x00380B80
	private void SpawnShellCasingAtPosition(Vector3 position)
	{
		if (this.shellCasing != null)
		{
			float num = ((!this.inheritRotationAsDirection) ? UnityEngine.Random.Range(-180f, 180f) : base.transform.eulerAngles.z);
			GameObject gameObject = SpawnManager.SpawnDebris(this.shellCasing, position, Quaternion.Euler(0f, 0f, num));
			ShellCasing component = gameObject.GetComponent<ShellCasing>();
			if (component != null)
			{
				component.Trigger();
			}
			DebrisObject component2 = gameObject.GetComponent<DebrisObject>();
			if (component2 != null)
			{
				Vector3 vector = BraveMathCollege.DegreesToVector(num + this.angleVariance * UnityEngine.Random.Range(-1f, 1f), Mathf.Lerp(this.minForce, this.maxForce, UnityEngine.Random.value)).ToVector3ZUp(UnityEngine.Random.value * 1.5f + 1f);
				float y = base.transform.position.y;
				float num2 = 0.2f;
				float num3 = component2.transform.position.y - y + UnityEngine.Random.value * 0.5f;
				component2.additionalHeightBoost = num2 - num3;
				if (num > 25f && num < 155f)
				{
					component2.additionalHeightBoost += -0.25f;
				}
				else
				{
					component2.additionalHeightBoost += 0.25f;
				}
				component2.Trigger(vector, num3, 1f);
			}
		}
	}

	// Token: 0x04008C93 RID: 35987
	public GameObject shellCasing;

	// Token: 0x04008C94 RID: 35988
	public bool inheritRotationAsDirection;

	// Token: 0x04008C95 RID: 35989
	public int shellsToLaunch;

	// Token: 0x04008C96 RID: 35990
	public float minForce = 1f;

	// Token: 0x04008C97 RID: 35991
	public float maxForce = 2.5f;

	// Token: 0x04008C98 RID: 35992
	public float angleVariance = 10f;

	// Token: 0x04008C99 RID: 35993
	private bool m_shouldSpawn;
}
