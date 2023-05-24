using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001147 RID: 4423
public class DraGunBoulderController : BraveBehaviour
{
	// Token: 0x060061FF RID: 25087 RVA: 0x0025F7A0 File Offset: 0x0025D9A0
	public void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleTriggerEntered));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnExitTrigger = (SpeculativeRigidbody.OnTriggerExitDelegate)Delegate.Combine(specRigidbody2.OnExitTrigger, new SpeculativeRigidbody.OnTriggerExitDelegate(this.HandleTriggerExited));
		if (this.CircleSprite)
		{
			tk2dSpriteDefinition currentSpriteDef = this.CircleSprite.GetCurrentSpriteDef();
			Vector2 vector = new Vector2(float.MaxValue, float.MaxValue);
			Vector2 vector2 = new Vector2(float.MinValue, float.MinValue);
			for (int i = 0; i < currentSpriteDef.uvs.Length; i++)
			{
				vector = Vector2.Min(vector, currentSpriteDef.uvs[i]);
				vector2 = Vector2.Max(vector2, currentSpriteDef.uvs[i]);
			}
			Vector2 vector3 = (vector + vector2) / 2f;
			this.CircleSprite.renderer.material.SetVector("_WorldCenter", new Vector4(vector3.x, vector3.y, vector3.x - vector.x, vector3.y - vector.y));
		}
		this.m_lifeTime = 0f;
	}

	// Token: 0x06006200 RID: 25088 RVA: 0x0025F8EC File Offset: 0x0025DAEC
	private void Update()
	{
		this.m_lifeTime += BraveTime.DeltaTime;
		if (this.m_lifeTime >= this.LifeTime)
		{
			this.m_lifeTime = 0f;
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleBreakCR());
		}
		for (int i = 0; i < this.m_cursedPlayers.Count; i++)
		{
			this.DoCurse(this.m_cursedPlayers[i]);
		}
	}

	// Token: 0x06006201 RID: 25089 RVA: 0x0025F96C File Offset: 0x0025DB6C
	private IEnumerator HandleBreakCR()
	{
		float elapsed = 0f;
		float duration = 0.3f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
			if (this.CircleSprite)
			{
				this.CircleSprite.scale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
			}
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06006202 RID: 25090 RVA: 0x0025F988 File Offset: 0x0025DB88
	private void DoCurse(PlayerController targetPlayer)
	{
		if (targetPlayer.IsGhost)
		{
			return;
		}
		targetPlayer.CurrentStoneGunTimer = Mathf.Max(targetPlayer.CurrentStoneGunTimer, 0.3f);
	}

	// Token: 0x06006203 RID: 25091 RVA: 0x0025F9AC File Offset: 0x0025DBAC
	private void HandleTriggerExited(SpeculativeRigidbody exitRigidbody, SpeculativeRigidbody sourceSpecRigidbody)
	{
		if (exitRigidbody && exitRigidbody.gameActor && exitRigidbody.gameActor is PlayerController && this.m_cursedPlayers.Contains(exitRigidbody.gameActor as PlayerController))
		{
			this.m_cursedPlayers.Remove(exitRigidbody.gameActor as PlayerController);
		}
	}

	// Token: 0x06006204 RID: 25092 RVA: 0x0025FA18 File Offset: 0x0025DC18
	private void HandleTriggerEntered(SpeculativeRigidbody enteredRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round)) != GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(enteredRigidbody.UnitCenter.ToIntVector2(VectorConversions.Round)))
		{
			return;
		}
		if (enteredRigidbody.gameActor != null && enteredRigidbody.gameActor is PlayerController)
		{
			this.m_cursedPlayers.Add(enteredRigidbody.gameActor as PlayerController);
		}
	}

	// Token: 0x04005CE5 RID: 23781
	public float LifeTime = 1f;

	// Token: 0x04005CE6 RID: 23782
	public tk2dSprite CircleSprite;

	// Token: 0x04005CE7 RID: 23783
	private float m_lifeTime;

	// Token: 0x04005CE8 RID: 23784
	private List<PlayerController> m_cursedPlayers = new List<PlayerController>();
}
