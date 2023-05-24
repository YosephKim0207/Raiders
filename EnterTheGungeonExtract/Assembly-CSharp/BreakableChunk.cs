using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001107 RID: 4359
public class BreakableChunk : BraveBehaviour
{
	// Token: 0x0600602A RID: 24618 RVA: 0x00250630 File Offset: 0x0024E830
	public void Awake()
	{
		if (this.subchunks == null)
		{
			this.subchunks = new List<GameObject>(base.transform.childCount);
		}
		if (this.subchunks.Count == 0)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				this.subchunks.Add(base.transform.GetChild(i).gameObject);
			}
		}
	}

	// Token: 0x0600602B RID: 24619 RVA: 0x002506A8 File Offset: 0x0024E8A8
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600602C RID: 24620 RVA: 0x002506B0 File Offset: 0x0024E8B0
	public void Trigger(bool destroyAfterTrigger = true, Vector3? directionalOrigin = null)
	{
		this.m_avgChunkPosition = Vector3.zero;
		foreach (GameObject gameObject in this.subchunks)
		{
			this.m_avgChunkPosition += gameObject.transform.position;
		}
		this.m_avgChunkPosition /= (float)this.subchunks.Count;
		int num = 0;
		while (num < this.randomDeletions && this.subchunks.Count > 1)
		{
			this.subchunks.RemoveAt(UnityEngine.Random.Range(0, this.subchunks.Count));
			num++;
		}
		if (this.puffCount > 0)
		{
			for (int i = 0; i < this.puffCount; i++)
			{
				if (this.puffSpawnDuration == 0f)
				{
					this.SpawnRandomizedPuff();
				}
				else
				{
					base.Invoke("SpawnRandomizedPuff", UnityEngine.Random.Range(0f, this.puffSpawnDuration));
				}
			}
		}
		foreach (GameObject gameObject2 in this.subchunks)
		{
			gameObject2.transform.parent = SpawnManager.Instance.VFX;
			gameObject2.SetActive(true);
			DebrisObject debrisObject = gameObject2.AddComponent<DebrisObject>();
			debrisObject.bounceCount = 0;
			debrisObject.angularVelocity = this.angularVelocity;
			debrisObject.GravityOverride = this.gravityOverride;
			Vector3 vector = gameObject2.transform.position - this.m_avgChunkPosition;
			if (this.useOverrideVelocityDir)
			{
				vector = this.overrideVelocityDir;
			}
			vector = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.angleVariance, this.angleVariance)) * vector;
			Vector3 vector2 = Vector3.zero;
			if (!this.slideMode)
			{
				vector2 = (vector.normalized * UnityEngine.Random.Range(this.minForce, this.maxForce)).WithZ(this.upwardForce);
				if (directionalOrigin != null)
				{
					vector = (gameObject2.transform.position - directionalOrigin.Value).WithZ(0f);
					vector = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.directionalAngleVariance, this.directionalAngleVariance)) * vector;
					vector2 += vector.normalized * UnityEngine.Random.Range(this.minDirectionalForce, this.maxDirectionalForce);
				}
			}
			debrisObject.Trigger(vector2, (this.startingHeight != 0f || this.slideMode) ? this.startingHeight : 0.01f, 1f);
			if (this.slideMode)
			{
				debrisObject.ApplyVelocity(vector.normalized * UnityEngine.Random.Range(this.minForce, this.maxForce));
			}
			BreakableChunk chunkScript = gameObject2.GetComponent<BreakableChunk>();
			if (chunkScript)
			{
				DebrisObject debrisObject2 = debrisObject;
				debrisObject2.OnGrounded = (Action<DebrisObject>)Delegate.Combine(debrisObject2.OnGrounded, new Action<DebrisObject>(delegate(DebrisObject d)
				{
					chunkScript.Trigger(true, null);
				}));
			}
		}
		if (destroyAfterTrigger)
		{
			foreach (Renderer renderer in base.GetComponents<Renderer>())
			{
				renderer.enabled = false;
			}
			UnityEngine.Object.Destroy(base.gameObject, this.puffSpawnDuration + 0.5f);
		}
		else
		{
			UnityEngine.Object.Destroy(this, this.puffSpawnDuration + 0.5f);
		}
	}

	// Token: 0x0600602D RID: 24621 RVA: 0x00250ABC File Offset: 0x0024ECBC
	private void SpawnRandomizedPuff()
	{
		this.puff.SpawnAtPosition(this.m_avgChunkPosition + new Vector3(UnityEngine.Random.Range(-this.puffAreaWidth / 2f, this.puffAreaWidth / 2f), UnityEngine.Random.Range(-this.puffAreaHeight / 2f, this.puffAreaHeight / 2f)), 0f, null, new Vector2?(Vector2.zero), new Vector2?(Vector2.zero), null, false, null, null, false);
	}

	// Token: 0x04005AB1 RID: 23217
	public List<GameObject> subchunks;

	// Token: 0x04005AB2 RID: 23218
	[Header("Puff Stuff")]
	public VFXPool puff;

	// Token: 0x04005AB3 RID: 23219
	public int puffCount;

	// Token: 0x04005AB4 RID: 23220
	public float puffAreaWidth;

	// Token: 0x04005AB5 RID: 23221
	public float puffAreaHeight;

	// Token: 0x04005AB6 RID: 23222
	public float puffSpawnDuration;

	// Token: 0x04005AB7 RID: 23223
	[Header("Debris Stuff")]
	public float startingHeight;

	// Token: 0x04005AB8 RID: 23224
	public float minForce;

	// Token: 0x04005AB9 RID: 23225
	public float maxForce = 1f;

	// Token: 0x04005ABA RID: 23226
	public float upwardForce;

	// Token: 0x04005ABB RID: 23227
	public float angleVariance = 30f;

	// Token: 0x04005ABC RID: 23228
	public float angularVelocity = 90f;

	// Token: 0x04005ABD RID: 23229
	public float gravityOverride;

	// Token: 0x04005ABE RID: 23230
	[Header("Minutiae")]
	public float minDirectionalForce;

	// Token: 0x04005ABF RID: 23231
	public float maxDirectionalForce;

	// Token: 0x04005AC0 RID: 23232
	public float directionalAngleVariance = 30f;

	// Token: 0x04005AC1 RID: 23233
	public int randomDeletions;

	// Token: 0x04005AC2 RID: 23234
	public bool slideMode;

	// Token: 0x04005AC3 RID: 23235
	public bool useOverrideVelocityDir;

	// Token: 0x04005AC4 RID: 23236
	[ShowInInspectorIf("useOverrideVelocityDir", false)]
	public Vector3 overrideVelocityDir;

	// Token: 0x04005AC5 RID: 23237
	private Vector3 m_avgChunkPosition;
}
