using System;
using System.Collections;
using UnityEngine;

// Token: 0x020011C6 RID: 4550
public class ParadoxPortalController : DungeonPlaceableBehaviour, IPlayerInteractable
{
	// Token: 0x0600657E RID: 25982 RVA: 0x00277858 File Offset: 0x00275A58
	public float GetDistanceToPoint(Vector2 point)
	{
		return Vector2.Distance(point, base.transform.position.XY()) / 1.5f;
	}

	// Token: 0x0600657F RID: 25983 RVA: 0x00277878 File Offset: 0x00275A78
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x06006580 RID: 25984 RVA: 0x00277880 File Offset: 0x00275A80
	public void OnEnteredRange(PlayerController interactor)
	{
	}

	// Token: 0x06006581 RID: 25985 RVA: 0x00277884 File Offset: 0x00275A84
	public void OnExitRange(PlayerController interactor)
	{
	}

	// Token: 0x06006582 RID: 25986 RVA: 0x00277888 File Offset: 0x00275A88
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x06006583 RID: 25987 RVA: 0x00277894 File Offset: 0x00275A94
	public void Interact(PlayerController interactor)
	{
		if (this.m_used || !interactor.IsPrimaryPlayer)
		{
			return;
		}
		this.m_used = true;
		interactor.portalEeveeTex = this.CosmicTex;
		interactor.IsTemporaryEeveeForUnlock = true;
		base.transform.position.GetAbsoluteRoom().DeregisterInteractable(this);
		base.StartCoroutine(this.HandleDestroy());
	}

	// Token: 0x06006584 RID: 25988 RVA: 0x002778F8 File Offset: 0x00275AF8
	private IEnumerator HandleDestroy()
	{
		float elapsed = 0f;
		float duration = 1f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			base.GetComponent<MeshRenderer>().material.SetFloat("_UVDistCutoff", Mathf.Lerp(0.2f, 0f, elapsed / duration));
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400613B RID: 24891
	public Texture2D CosmicTex;

	// Token: 0x0400613C RID: 24892
	private bool m_used;
}
