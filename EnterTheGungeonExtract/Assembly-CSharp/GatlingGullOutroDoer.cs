using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200102F RID: 4143
public class GatlingGullOutroDoer : BraveBehaviour
{
	// Token: 0x06005AD9 RID: 23257 RVA: 0x0022C10C File Offset: 0x0022A30C
	private void Start()
	{
		BossKillCam.hackGatlingGullOutroDoer = this;
	}

	// Token: 0x06005ADA RID: 23258 RVA: 0x0022C114 File Offset: 0x0022A314
	public void TriggerSequence()
	{
		AkSoundEngine.PostEvent("Play_ANM_Gull_Outro_01", base.gameObject);
		base.sprite.usesOverrideMaterial = true;
		base.sprite.IsPerpendicular = false;
		this.m_extantCrows = new List<GatlingGullCrowController>();
		int num = 100;
		for (int i = 0; i < num; i++)
		{
			float num2 = (float)UnityEngine.Random.Range(30, 40);
			if (UnityEngine.Random.value < 0.04f)
			{
				num2 = (float)UnityEngine.Random.Range(20, 30);
			}
			if (i == 0)
			{
				num2 = 10f;
			}
			Vector2 vector = UnityEngine.Random.insideUnitCircle.normalized * num2;
			Vector2 vector2 = base.transform.position.XY() + vector;
			Vector2 vector3 = base.transform.position.XY() + Vector2.Scale(UnityEngine.Random.insideUnitCircle, new Vector2(2.25f, 1.75f)) + new Vector2(3.1f, 2.1f);
			GameObject gameObject = SpawnManager.SpawnVFX(this.CrowVFX, vector2.ToVector3ZUp(vector2.y), Quaternion.identity, true);
			GatlingGullCrowController component = gameObject.GetComponent<GatlingGullCrowController>();
			component.CurrentTargetPosition = vector3;
			component.sprite.SortingOrder = 3;
			component.sprite.HeightOffGround = 20f;
			component.CurrentTargetHeight = 2f;
			component.useFacePoint = true;
			component.facePoint = base.transform.position.XY() + new Vector2(3.1f, 2.1f);
			this.m_extantCrows.Add(component);
		}
		base.StartCoroutine(this.HandleSequence());
	}

	// Token: 0x06005ADB RID: 23259 RVA: 0x0022C2B8 File Offset: 0x0022A4B8
	private IEnumerator HandleSequence()
	{
		float elapsed = 0f;
		tk2dSprite shadowSprite = null;
		for (int j = 0; j < base.transform.childCount; j++)
		{
			GameObject gameObject = base.transform.GetChild(j).gameObject;
			if (gameObject.name.Contains("shadow", true))
			{
				shadowSprite = gameObject.GetComponent<tk2dSprite>();
				if (shadowSprite)
				{
					break;
				}
			}
		}
		yield return new WaitForSeconds(8f);
		while (elapsed < 3f)
		{
			elapsed += BraveTime.DeltaTime;
			base.sprite.scale = Vector3.one * (0.5f - elapsed / 6f + 0.5f);
			if (shadowSprite)
			{
				shadowSprite.scale = Vector3.one * (0.5f - elapsed / 6f + 0.5f);
			}
			yield return null;
		}
		base.sprite.SetSprite("gatling_gill_die_var_011");
		base.sprite.renderer.enabled = false;
		base.sprite.scale = Vector3.one;
		if (shadowSprite)
		{
			UnityEngine.Object.Destroy(shadowSprite.gameObject);
		}
		base.sprite.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.NONE;
		base.sprite.ForceUpdateMaterial();
		for (int i = 0; i < this.m_extantCrows.Count; i++)
		{
			if (UnityEngine.Random.value < 0.2f)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.025f));
			}
			this.m_extantCrows[i].useFacePoint = false;
			Vector2 escapeDir = UnityEngine.Random.insideUnitCircle.normalized * (float)UnityEngine.Random.Range(50, 60);
			this.m_extantCrows[i].CurrentTargetHeight = 50f;
			this.m_extantCrows[i].CurrentTargetPosition = this.m_extantCrows[i].transform.position.XY() + escapeDir;
			this.m_extantCrows[i].destroyOnArrival = true;
		}
		yield break;
	}

	// Token: 0x06005ADC RID: 23260 RVA: 0x0022C2D4 File Offset: 0x0022A4D4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400545A RID: 21594
	public GameObject CrowVFX;

	// Token: 0x0400545B RID: 21595
	protected List<GatlingGullCrowController> m_extantCrows;
}
