using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001817 RID: 6167
public class UnearthController : BraveBehaviour
{
	// Token: 0x06009170 RID: 37232 RVA: 0x003D8ACC File Offset: 0x003D6CCC
	private void Update()
	{
		if (this.m_state == UnearthController.UnearthState.Idle)
		{
			if (base.aiAnimator.IsPlaying(this.triggerAnim))
			{
				this.m_state = UnearthController.UnearthState.Unearth;
				base.StartCoroutine(this.DirtCR());
				base.StartCoroutine(this.PuffCR());
			}
		}
		else if (this.m_state == UnearthController.UnearthState.Unearth && !base.aiAnimator.IsPlaying(this.triggerAnim))
		{
			this.m_state = UnearthController.UnearthState.Finished;
		}
	}

	// Token: 0x06009171 RID: 37233 RVA: 0x003D8B4C File Offset: 0x003D6D4C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06009172 RID: 37234 RVA: 0x003D8B54 File Offset: 0x003D6D54
	private IEnumerator DirtCR()
	{
		List<GameObject> dirtObjs = new List<GameObject>();
		Vector2 minPos = base.specRigidbody.UnitBottomLeft;
		Vector2 maxPos = base.specRigidbody.UnitBottomRight;
		for (int i = 0; i < this.dirtCount; i++)
		{
			GameObject gameObject = BraveUtility.RandomElement<GameObject>(this.dirtVfx);
			Vector2 vector = BraveUtility.RandomVector2(minPos, maxPos, new Vector2(0.125f, 0f));
			GameObject gameObject2 = SpawnManager.SpawnVFX(gameObject, vector, Quaternion.identity);
			dirtObjs.Add(gameObject2);
			tk2dBaseSprite component = gameObject2.GetComponent<tk2dBaseSprite>();
			if (component)
			{
				base.sprite.AttachRenderer(component);
				component.HeightOffGround = 0.1f;
				component.UpdateZDepth();
			}
		}
		while (this.m_state == UnearthController.UnearthState.Unearth)
		{
			yield return null;
		}
		for (int j = 0; j < dirtObjs.Count; j++)
		{
			SpawnManager.Despawn(dirtObjs[j]);
		}
		yield break;
	}

	// Token: 0x06009173 RID: 37235 RVA: 0x003D8B70 File Offset: 0x003D6D70
	private IEnumerator PuffCR()
	{
		Vector2 minPos = base.specRigidbody.UnitBottomLeft + this.dustOffset;
		Vector2 maxPos = base.specRigidbody.UnitBottomLeft + this.dustOffset + this.dustDimensions;
		float intraTimer = 0f;
		while (this.m_state == UnearthController.UnearthState.Unearth)
		{
			while (intraTimer <= 0f)
			{
				GameObject gameObject = BraveUtility.RandomElement<GameObject>(this.dustVfx);
				Vector2 vector = BraveUtility.RandomVector2(minPos, maxPos);
				GameObject gameObject2 = SpawnManager.SpawnVFX(gameObject, vector, Quaternion.identity);
				tk2dBaseSprite component = gameObject2.GetComponent<tk2dBaseSprite>();
				if (component)
				{
					base.sprite.AttachRenderer(component);
					component.HeightOffGround = 0.1f;
					component.UpdateZDepth();
				}
				intraTimer += this.dustMidDelay;
			}
			yield return null;
			intraTimer -= BraveTime.DeltaTime;
		}
		yield break;
	}

	// Token: 0x040099B3 RID: 39347
	public string triggerAnim;

	// Token: 0x040099B4 RID: 39348
	public List<GameObject> dirtVfx;

	// Token: 0x040099B5 RID: 39349
	public int dirtCount;

	// Token: 0x040099B6 RID: 39350
	public List<GameObject> dustVfx;

	// Token: 0x040099B7 RID: 39351
	public float dustMidDelay = 0.05f;

	// Token: 0x040099B8 RID: 39352
	public Vector2 dustOffset;

	// Token: 0x040099B9 RID: 39353
	public Vector2 dustDimensions;

	// Token: 0x040099BA RID: 39354
	private UnearthController.UnearthState m_state;

	// Token: 0x02001818 RID: 6168
	private enum UnearthState
	{
		// Token: 0x040099BC RID: 39356
		Idle,
		// Token: 0x040099BD RID: 39357
		Unearth,
		// Token: 0x040099BE RID: 39358
		Finished
	}
}
