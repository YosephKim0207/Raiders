using System;
using UnityEngine;

// Token: 0x020015F9 RID: 5625
public class PlayerHandController : BraveBehaviour
{
	// Token: 0x0600827D RID: 33405 RVA: 0x0035545C File Offset: 0x0035365C
	public void InitializeWithPlayer(PlayerController p, bool isPrimary)
	{
		this.m_ownerPlayer = p;
		this.IsPlayerPrimary = isPrimary;
	}

	// Token: 0x0600827E RID: 33406 RVA: 0x0035546C File Offset: 0x0035366C
	private void Start()
	{
		this.m_cachedStartPosition = base.transform.localPosition;
		base.sprite.HeightOffGround = this.handHeightFromGun;
		DepthLookupManager.ProcessRenderer(base.renderer);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black, this.OUTLINE_DEPTH, 0f, SpriteOutlineManager.OutlineType.NORMAL);
		this.m_cachedShader = base.sprite.renderer.material.shader;
	}

	// Token: 0x0600827F RID: 33407 RVA: 0x003554E0 File Offset: 0x003536E0
	private void ToggleRenderers(bool e)
	{
		if (this.outlineSprites == null || this.outlineSprites.Length == 0)
		{
			this.outlineSprites = SpriteOutlineManager.GetOutlineSprites(base.sprite);
		}
		base.renderer.enabled = e;
		for (int i = 0; i < this.outlineSprites.Length; i++)
		{
			this.outlineSprites[i].renderer.enabled = e;
		}
	}

	// Token: 0x06008280 RID: 33408 RVA: 0x00355550 File Offset: 0x00353750
	private void LateUpdate()
	{
		if (!this.attachPoint || !this.attachPoint.gameObject.activeSelf)
		{
			this.ToggleRenderers(false);
			base.transform.localPosition = this.m_cachedStartPosition;
		}
		else
		{
			this.ToggleRenderers(!this.ForceRenderersOff);
			base.transform.position = BraveUtility.QuantizeVector(this.attachPoint.position, 16f);
		}
		if (this.m_ownerPlayer && this.m_ownerPlayer.CurrentGun && this.m_ownerPlayer.CurrentGun.OnlyUsesIdleInWeaponBox)
		{
			float num = 0f;
			float currentAngle = this.m_ownerPlayer.CurrentGun.CurrentAngle;
			if (this.m_ownerPlayer.CurrentGun.IsFiring)
			{
				if (currentAngle <= 155f && currentAngle >= 25f)
				{
					num = 0f;
				}
				else
				{
					this.m_hasAlteredHeight = true;
					num = ((!this.IsPlayerPrimary) ? 1.5f : 0.5f);
				}
			}
			base.sprite.HeightOffGround = this.handHeightFromGun + num;
		}
		else if (this.m_hasAlteredHeight)
		{
			base.sprite.HeightOffGround = this.handHeightFromGun;
			this.m_hasAlteredHeight = false;
		}
		base.sprite.UpdateZDepth();
	}

	// Token: 0x06008281 RID: 33409 RVA: 0x003556C0 File Offset: 0x003538C0
	public Material SetOverrideShader(Shader overrideShader)
	{
		Debug.Log("overriding hand shader");
		base.sprite.renderer.material.shader = overrideShader;
		return base.sprite.renderer.material;
	}

	// Token: 0x06008282 RID: 33410 RVA: 0x003556F4 File Offset: 0x003538F4
	public void ClearOverrideShader()
	{
		base.sprite.renderer.material.shader = this.m_cachedShader;
	}

	// Token: 0x06008283 RID: 33411 RVA: 0x00355714 File Offset: 0x00353914
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400858C RID: 34188
	public bool ForceRenderersOff;

	// Token: 0x0400858D RID: 34189
	public Transform attachPoint;

	// Token: 0x0400858E RID: 34190
	public float handHeightFromGun = 0.05f;

	// Token: 0x0400858F RID: 34191
	protected float OUTLINE_DEPTH = 0.1f;

	// Token: 0x04008590 RID: 34192
	protected PlayerController m_ownerPlayer;

	// Token: 0x04008591 RID: 34193
	private bool IsPlayerPrimary;

	// Token: 0x04008592 RID: 34194
	protected Shader m_cachedShader;

	// Token: 0x04008593 RID: 34195
	private bool m_hasAlteredHeight;

	// Token: 0x04008594 RID: 34196
	private Vector3 m_cachedStartPosition;

	// Token: 0x04008595 RID: 34197
	private tk2dSprite[] outlineSprites;
}
