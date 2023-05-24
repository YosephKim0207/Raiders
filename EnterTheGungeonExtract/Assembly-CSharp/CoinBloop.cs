using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000E51 RID: 3665
public class CoinBloop : BraveBehaviour
{
	// Token: 0x06004E0F RID: 19983 RVA: 0x001AEA4C File Offset: 0x001ACC4C
	private void Start()
	{
		if (this.m_bloopIndex == -1)
		{
			base.gameObject.layer = LayerMask.NameToLayer("Unpixelated");
			base.transform.localPosition = BraveUtility.QuantizeVector(base.transform.localPosition, 16f);
			this.m_cachedLocalPosition = base.transform.localPosition;
			base.renderer.enabled = false;
		}
	}

	// Token: 0x06004E10 RID: 19984 RVA: 0x001AEAB8 File Offset: 0x001ACCB8
	private void Update()
	{
		if (this.m_bloopIndex > -1)
		{
			if (CoinBloop.bloopCounter > this.m_cachedBloopCounter)
			{
				this.m_cachedBloopCounter = CoinBloop.bloopCounter;
				if (this.elapsed / this.bloopWait > 0.75f)
				{
					this.elapsed = this.bloopWait;
				}
			}
			if (this.m_sprBounds == null)
			{
				this.m_sprBounds = new Bounds?(base.sprite.GetBounds());
			}
			float num = Mathf.Max(0.625f, this.m_sprBounds.Value.extents.y * 2f + 0.0625f);
			float num2 = (float)(CoinBloop.bloopCounter - this.m_bloopIndex) * num;
			float num3 = 0f;
			if (GameUIRoot.Instance && this.m_player && GameUIRoot.Instance.GetReloadBarForPlayer(this.m_player) && GameUIRoot.Instance.GetReloadBarForPlayer(this.m_player).ReloadIsActive)
			{
				num3 = 0.5f;
			}
			base.transform.parent.localPosition = this.m_cachedParentLocalPosition + new Vector3(0f, num2 + num3, 0f);
		}
	}

	// Token: 0x06004E11 RID: 19985 RVA: 0x001AEC08 File Offset: 0x001ACE08
	protected void DoBloopInternal(tk2dBaseSprite targetSprite, string overrideSprite, Color tintColor, bool addOutline = false)
	{
		this.m_bloopIndex = CoinBloop.bloopCounter;
		this.m_cachedBloopCounter = CoinBloop.bloopCounter;
		if (string.IsNullOrEmpty(overrideSprite))
		{
			base.sprite.SetSprite(targetSprite.Collection, targetSprite.spriteId);
		}
		else
		{
			base.sprite.SetSprite(targetSprite.Collection, overrideSprite);
		}
		base.sprite.color = tintColor;
		if (addOutline)
		{
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
		}
		Bounds bounds = base.sprite.GetBounds();
		float num = bounds.min.x + bounds.extents.x;
		base.transform.parent.position = base.transform.parent.position.WithX(BraveMathCollege.QuantizeFloat(base.transform.parent.parent.GetComponent<PlayerController>().LockedApproximateSpriteCenter.x - num, 0.0625f));
		base.transform.parent.localPosition = base.transform.parent.localPosition.WithZ(-5f);
		this.m_cachedParentLocalPosition = base.transform.parent.localPosition;
		base.StartCoroutine(this.Bloop());
	}

	// Token: 0x06004E12 RID: 19986 RVA: 0x001AED58 File Offset: 0x001ACF58
	public void DoBloop(tk2dBaseSprite targetSprite, string overrideSprite, Color tintColor, bool addOutline = false)
	{
		CoinBloop.bloopCounter++;
		if (this.m_player == null)
		{
			this.m_player = base.GetComponentInParent<PlayerController>();
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.transform.parent.gameObject);
		gameObject.transform.parent = base.transform.parent.parent;
		gameObject.transform.position = base.transform.parent.position;
		CoinBloop componentInParent = gameObject.transform.GetChild(0).GetComponentInParent<CoinBloop>();
		componentInParent.m_player = this.m_player;
		componentInParent.DoBloopInternal(targetSprite, overrideSprite, tintColor, addOutline);
	}

	// Token: 0x06004E13 RID: 19987 RVA: 0x001AEE04 File Offset: 0x001AD004
	private IEnumerator Bloop()
	{
		Vector3 localPosition = base.transform.localPosition;
		base.GetComponent<Animation>().Play();
		base.renderer.enabled = true;
		this.elapsed = 0f;
		while (this.elapsed < this.bloopWait)
		{
			float yOffset = 0f;
			if (GameUIRoot.Instance && GameUIRoot.Instance.GetReloadBarForPlayer(this.m_player).ReloadIsActive)
			{
				yOffset = 0.5f;
			}
			base.transform.localPosition = BraveUtility.QuantizeVector(localPosition + new Vector3(0f, yOffset, 0f), 16f);
			if (base.sprite)
			{
				base.sprite.UpdateZDepth();
			}
			this.elapsed += BraveTime.DeltaTime;
			yield return null;
		}
		base.GetComponent<Animation>().Stop();
		base.renderer.enabled = false;
		base.transform.localPosition = this.m_cachedLocalPosition;
		UnityEngine.Object.Destroy(base.transform.parent.gameObject);
		yield break;
	}

	// Token: 0x06004E14 RID: 19988 RVA: 0x001AEE20 File Offset: 0x001AD020
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400445A RID: 17498
	private static int bloopCounter;

	// Token: 0x0400445B RID: 17499
	public float bloopWait = 0.7f;

	// Token: 0x0400445C RID: 17500
	private int m_bloopIndex = -1;

	// Token: 0x0400445D RID: 17501
	private Vector3 m_cachedLocalPosition;

	// Token: 0x0400445E RID: 17502
	private Vector3 m_cachedParentLocalPosition;

	// Token: 0x0400445F RID: 17503
	private int m_cachedBloopCounter = -1;

	// Token: 0x04004460 RID: 17504
	private float elapsed;

	// Token: 0x04004461 RID: 17505
	private PlayerController m_player;

	// Token: 0x04004462 RID: 17506
	private Bounds? m_sprBounds;
}
