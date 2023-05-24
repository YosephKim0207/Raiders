using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x020013DB RID: 5083
[Serializable]
public class RadialSlowInterface
{
	// Token: 0x06007359 RID: 29529 RVA: 0x002DE4D8 File Offset: 0x002DC6D8
	public void DoRadialSlow(Vector2 centerPoint, RoomHandler targetRoom)
	{
		List<AIActor> activeEnemies = targetRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
		if (!string.IsNullOrEmpty(this.audioEvent))
		{
			AkSoundEngine.PostEvent(this.audioEvent, GameManager.Instance.BestActivePlayer.gameObject);
		}
		if (activeEnemies != null && activeEnemies.Count > 0)
		{
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				AIActor aiactor = activeEnemies[i];
				if (aiactor && aiactor.IsNormalEnemy && aiactor.healthHaver && !aiactor.IsGone)
				{
					aiactor.StartCoroutine(this.ProcessSlow(centerPoint, aiactor, 0f));
				}
			}
		}
		if (this.DoesSepia)
		{
			Pixelator.Instance.StartCoroutine(this.ProcessEnsepia());
		}
		if (this.DoesCirclePass)
		{
			Pixelator.Instance.StartCoroutine(this.ProcessCirclePass(centerPoint));
		}
		if (this.UpdatesForNewEnemies)
		{
			GameManager.Instance.Dungeon.StartCoroutine(this.HandleNewEnemies(centerPoint, targetRoom));
		}
	}

	// Token: 0x0600735A RID: 29530 RVA: 0x002DE5EC File Offset: 0x002DC7EC
	private IEnumerator HandleNewEnemies(Vector2 centerPoint, RoomHandler targetRoom)
	{
		float totalDuration = this.RadialSlowHoldTime + this.RadialSlowInTime + this.RadialSlowOutTime;
		float elapsed = 0f;
		Action<AIActor> enemyAdded = delegate(AIActor a)
		{
			if (a && a.IsNormalEnemy && a.healthHaver && !a.IsGone)
			{
				a.StartCoroutine(this.ProcessSlow(centerPoint, a, elapsed));
			}
		};
		targetRoom.OnEnemyRegistered = (Action<AIActor>)Delegate.Combine(targetRoom.OnEnemyRegistered, enemyAdded);
		while (elapsed < totalDuration)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		targetRoom.OnEnemyRegistered = (Action<AIActor>)Delegate.Remove(targetRoom.OnEnemyRegistered, enemyAdded);
		yield break;
	}

	// Token: 0x0600735B RID: 29531 RVA: 0x002DE618 File Offset: 0x002DC818
	private IEnumerator ProcessCirclePass(Vector2 centerPoint)
	{
		Material newPass = new Material(Shader.Find("Brave/Effects/PartialDesaturationEffect"));
		newPass.SetVector("_WorldCenter", new Vector4(centerPoint.x, centerPoint.y, 0f, 0f));
		Pixelator.Instance.RegisterAdditionalRenderPass(newPass);
		float elapsed = 0f;
		while (elapsed < this.RadialSlowInTime)
		{
			elapsed += BraveTime.DeltaTime;
			newPass.SetFloat("_Radius", Mathf.Lerp(0f, this.EffectRadius, elapsed / this.RadialSlowInTime));
			yield return null;
		}
		elapsed = 0f;
		newPass.SetFloat("_Radius", this.EffectRadius);
		while (elapsed < this.RadialSlowHoldTime)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		elapsed = 0f;
		newPass.SetFloat("_Radius", this.EffectRadius);
		while (elapsed < this.RadialSlowOutTime)
		{
			elapsed += BraveTime.DeltaTime;
			newPass.SetFloat("_Radius", Mathf.Lerp(this.EffectRadius, 0f, elapsed / this.RadialSlowOutTime));
			yield return null;
		}
		Pixelator.Instance.DeregisterAdditionalRenderPass(newPass);
		yield break;
	}

	// Token: 0x0600735C RID: 29532 RVA: 0x002DE63C File Offset: 0x002DC83C
	private IEnumerator ProcessEnsepia()
	{
		float elapsed = 0f;
		while (elapsed < this.RadialSlowInTime)
		{
			elapsed += BraveTime.DeltaTime;
			Pixelator.Instance.SetFreezeFramePower(elapsed / this.RadialSlowInTime, false);
			yield return null;
		}
		elapsed = 0f;
		Pixelator.Instance.SetFreezeFramePower(1f, false);
		while (elapsed < this.RadialSlowHoldTime)
		{
			elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		elapsed = 0f;
		while (elapsed < this.RadialSlowOutTime)
		{
			elapsed += BraveTime.DeltaTime;
			Pixelator.Instance.SetFreezeFramePower(1f - elapsed / this.RadialSlowOutTime, false);
			yield return null;
		}
		Pixelator.Instance.SetFreezeFramePower(0f, false);
		yield break;
	}

	// Token: 0x0600735D RID: 29533 RVA: 0x002DE658 File Offset: 0x002DC858
	private IEnumerator ProcessSlow(Vector2 centerPoint, AIActor target, float startTime)
	{
		float elapsed = startTime;
		float sqrRadius = this.EffectRadius * this.EffectRadius;
		if (this.RadialSlowInTime > 0f)
		{
			while (elapsed < this.RadialSlowInTime)
			{
				if (!target || target.healthHaver.IsDead)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				float t = elapsed / this.RadialSlowInTime;
				if ((target.CenterPosition - centerPoint).sqrMagnitude > sqrRadius)
				{
					t = 0f;
				}
				target.LocalTimeScale = Mathf.Lerp(1f, this.RadialSlowTimeModifier, t);
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.RadialSlowHoldTime > 0f)
		{
			while (elapsed < this.RadialSlowHoldTime)
			{
				if (!target || target.healthHaver.IsDead)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				float timeTarget = (((target.CenterPosition - centerPoint).sqrMagnitude <= sqrRadius) ? this.RadialSlowTimeModifier : 1f);
				target.LocalTimeScale = timeTarget;
				yield return null;
			}
		}
		elapsed = 0f;
		if (this.RadialSlowOutTime > 0f)
		{
			while (elapsed < this.RadialSlowOutTime)
			{
				if (!target || target.healthHaver.IsDead)
				{
					break;
				}
				elapsed += BraveTime.DeltaTime;
				float t2 = elapsed / this.RadialSlowOutTime;
				if ((target.CenterPosition - centerPoint).sqrMagnitude > sqrRadius)
				{
					t2 = 1f;
				}
				target.LocalTimeScale = Mathf.Lerp(this.RadialSlowTimeModifier, 1f, t2);
				yield return null;
			}
		}
		if (target)
		{
			target.LocalTimeScale = 1f;
		}
		yield break;
	}

	// Token: 0x040074EB RID: 29931
	public float EffectRadius = 100f;

	// Token: 0x040074EC RID: 29932
	public float RadialSlowInTime;

	// Token: 0x040074ED RID: 29933
	public float RadialSlowHoldTime = 1f;

	// Token: 0x040074EE RID: 29934
	public float RadialSlowOutTime = 0.5f;

	// Token: 0x040074EF RID: 29935
	public float RadialSlowTimeModifier = 0.25f;

	// Token: 0x040074F0 RID: 29936
	public string audioEvent;

	// Token: 0x040074F1 RID: 29937
	public bool DoesSepia;

	// Token: 0x040074F2 RID: 29938
	public bool DoesCirclePass;

	// Token: 0x040074F3 RID: 29939
	public bool UpdatesForNewEnemies = true;
}
