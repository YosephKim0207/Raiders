using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200177C RID: 6012
public class GameUIAmmoController : BraveBehaviour
{
	// Token: 0x06008C02 RID: 35842 RVA: 0x003A54F4 File Offset: 0x003A36F4
	private void Initialize()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		this.outlineSprites = new tk2dSprite[this.gunSprites.Length][];
		for (int i = 0; i < this.gunSprites.Length; i++)
		{
			tk2dClippedSprite tk2dClippedSprite = this.gunSprites[i];
			SpriteOutlineManager.AddOutlineToSprite(tk2dClippedSprite, Color.white, 2f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			this.outlineSprites[i] = SpriteOutlineManager.GetOutlineSprites(tk2dClippedSprite);
			for (int j = 0; j < this.outlineSprites[i].Length; j++)
			{
				if (this.outlineSprites[i].Length > 1)
				{
					float num = ((j != 1) ? 0f : 0.0625f);
					num = ((j != 3) ? num : (-0.0625f));
					float num2 = ((j != 0) ? 0f : 0.0625f);
					num2 = ((j != 2) ? num2 : (-0.0625f));
					this.outlineSprites[i][j].transform.localPosition = (new Vector3(num, num2, 0f) * Pixelator.Instance.CurrentTileScale * 16f * GameUIRoot.Instance.PixelsToUnits()).WithZ(this.UI_OUTLINE_DEPTH);
				}
				this.outlineSprites[i][j].gameObject.layer = tk2dClippedSprite.gameObject.layer;
			}
		}
		this.m_ClippedMaterial = new Material(ShaderCache.Acquire("Daikon Forge/Clipped UI Shader"));
		this.m_ClippedZWriteOffMaterial = new Material(ShaderCache.Acquire("Daikon Forge/Clipped UI Shader ZWriteOff"));
		this.topCapsForModules.Add(this.InitialTopCapSprite);
		this.bottomCapsForModules.Add(this.InitialBottomCapSprite);
		this.m_panel.SendToBack();
		this.m_initialized = true;
	}

	// Token: 0x06008C03 RID: 35843 RVA: 0x003A56B8 File Offset: 0x003A38B8
	public dfSprite RegisterNewAdditionalSprite(string spriteName)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.InitialBottomCapSprite.gameObject);
		dfSprite component = gameObject.GetComponent<dfSprite>();
		this.InitialBottomCapSprite.Parent.AddControl(component);
		component.SpriteName = spriteName;
		component.Size = component.SpriteInfo.sizeInPixels * Pixelator.Instance.CurrentTileScale;
		this.m_additionalRegisteredSprites.Add(component);
		this.UpdateAdditionalSprites();
		return component;
	}

	// Token: 0x06008C04 RID: 35844 RVA: 0x003A5728 File Offset: 0x003A3928
	public void DeregisterAdditionalSprite(dfSprite sprite)
	{
		if (this.m_additionalRegisteredSprites.Contains(sprite))
		{
			this.m_additionalRegisteredSprites.Remove(sprite);
			UnityEngine.Object.Destroy(sprite.gameObject);
			this.UpdateAdditionalSprites();
		}
	}

	// Token: 0x06008C05 RID: 35845 RVA: 0x003A575C File Offset: 0x003A395C
	private void UpdateAdditionalSprites()
	{
		float currentTileScale = Pixelator.Instance.CurrentTileScale;
		Vector3 vector = this.GunAmmoCountLabel.Position;
		Vector3 vector2 = Vector3.zero;
		if (this.IsLeftAligned)
		{
			vector += new Vector3(0f, 4f * currentTileScale, 0f);
		}
		else
		{
			vector += new Vector3(this.GunAmmoCountLabel.Size.x, 4f * currentTileScale, 0f);
		}
		int num = ((!this.IsLeftAligned) ? (-1) : 1);
		for (int i = 0; i < this.m_additionalRegisteredSprites.Count; i++)
		{
			Vector2 size = this.m_additionalRegisteredSprites[i].Size;
			if (this.IsLeftAligned)
			{
				this.m_additionalRegisteredSprites[i].Position = vector + (float)num * vector2;
				vector2 += new Vector3(size.x + currentTileScale, 0f, 0f);
			}
			else
			{
				vector2 += new Vector3(size.x, 0f, 0f);
				this.m_additionalRegisteredSprites[i].Position = vector + (float)num * vector2;
				vector2 += new Vector3(currentTileScale, 0f, 0f);
			}
		}
	}

	// Token: 0x06008C06 RID: 35846 RVA: 0x003A58CC File Offset: 0x003A3ACC
	private void RepositionOutlines(dfControl arg1, Vector3 arg2, Vector3 arg3)
	{
		if (this.outlineSprites != null)
		{
			for (int i = 0; i < this.gunSprites.Length; i++)
			{
				for (int j = 0; j < this.outlineSprites.Length; j++)
				{
					this.outlineSprites[i][j].gameObject.layer = this.gunSprites[i].gameObject.layer;
				}
			}
		}
	}

	// Token: 0x06008C07 RID: 35847 RVA: 0x003A593C File Offset: 0x003A3B3C
	public void DimGunSprite()
	{
		for (int i = 0; i < this.gunSprites.Length; i++)
		{
			this.gunSprites[i].gameObject.SetActive(false);
		}
		if (this.m_extantNoAmmoIcon != null)
		{
			this.m_extantNoAmmoIcon.gameObject.SetActive(false);
		}
	}

	// Token: 0x06008C08 RID: 35848 RVA: 0x003A5998 File Offset: 0x003A3B98
	public void UndimGunSprite()
	{
		for (int i = 0; i < this.gunSprites.Length; i++)
		{
			this.gunSprites[i].gameObject.SetActive(true);
		}
		if (this.m_extantNoAmmoIcon != null)
		{
			this.m_extantNoAmmoIcon.gameObject.SetActive(true);
		}
	}

	// Token: 0x06008C09 RID: 35849 RVA: 0x003A59F4 File Offset: 0x003A3BF4
	private float ActualSign(float f)
	{
		if (f < 0f)
		{
			return -1f;
		}
		if (f > 0f)
		{
			return 1f;
		}
		return 0f;
	}

	// Token: 0x170014F0 RID: 5360
	// (get) Token: 0x06008C0A RID: 35850 RVA: 0x003A5A20 File Offset: 0x003A3C20
	public dfSprite DefaultAmmoFGSprite
	{
		get
		{
			if (this.fgSpritesForModules == null || this.fgSpritesForModules.Count == 0)
			{
				return null;
			}
			return this.fgSpritesForModules[0];
		}
	}

	// Token: 0x06008C0B RID: 35851 RVA: 0x003A5A4C File Offset: 0x003A3C4C
	public void UpdateScale()
	{
		float currentTileScale = Pixelator.Instance.CurrentTileScale;
		this.GunBoxSprite.Size = this.GunBoxSprite.SpriteInfo.sizeInPixels * currentTileScale;
		Vector2 vector = new Vector2(currentTileScale, currentTileScale);
		for (int i = 0; i < this.fgSpritesForModules.Count; i++)
		{
			if (this.fgSpritesForModules[i])
			{
				this.fgSpritesForModules[i].TileScale = vector;
				this.bgSpritesForModules[i].TileScale = vector;
			}
		}
		for (int j = 0; j < this.addlFgSpritesForModules.Count; j++)
		{
			List<dfTiledSprite> list = this.addlFgSpritesForModules[j];
			List<dfTiledSprite> list2 = this.addlBgSpritesForModules[j];
			for (int k = 0; k < list.Count; k++)
			{
				list[k].TileScale = vector;
				list2[k].TileScale = vector;
			}
		}
		for (int l = 0; l < this.topCapsForModules.Count; l++)
		{
			this.topCapsForModules[l].Size = this.topCapsForModules[l].SpriteInfo.sizeInPixels * currentTileScale;
			this.bottomCapsForModules[l].Size = this.bottomCapsForModules[l].SpriteInfo.sizeInPixels * currentTileScale;
		}
		if (this.GunClipCountLabel != null)
		{
			this.GunClipCountLabel.TextScale = currentTileScale;
		}
		if (this.GunAmmoCountLabel != null)
		{
			this.GunAmmoCountLabel.TextScale = currentTileScale;
		}
	}

	// Token: 0x06008C0C RID: 35852 RVA: 0x003A5C10 File Offset: 0x003A3E10
	public void SetAmmoCountLabelColor(Color targetcolor)
	{
		this.GunAmmoCountLabel.Color = targetcolor;
		this.GunAmmoCountLabel.BottomColor = targetcolor;
	}

	// Token: 0x06008C0D RID: 35853 RVA: 0x003A5C34 File Offset: 0x003A3E34
	public void ToggleRenderers(bool value)
	{
		if (!this.m_initialized)
		{
			return;
		}
		if (this.GunBoxSprite != null && this.GunBoxSprite.Parent != null)
		{
			this.GunBoxSprite.IsVisible = value;
		}
		if (this.GunBoxSprite != null)
		{
			this.GunBoxSprite.IsVisible = value;
		}
		if (this.GunQuickSwitchIcon != null && !value)
		{
			this.GunQuickSwitchIcon.IsVisible = value;
		}
		for (int i = 0; i < this.fgSpritesForModules.Count; i++)
		{
			if (this.fgSpritesForModules[i])
			{
				this.fgSpritesForModules[i].IsVisible = value;
				this.bgSpritesForModules[i].IsVisible = value;
			}
		}
		for (int j = 0; j < this.addlFgSpritesForModules.Count; j++)
		{
			List<dfTiledSprite> list = this.addlFgSpritesForModules[j];
			List<dfTiledSprite> list2 = this.addlBgSpritesForModules[j];
			for (int k = 0; k < list.Count; k++)
			{
				list[k].IsVisible = value;
				list2[k].IsVisible = value;
			}
		}
		if (this.m_extantNoAmmoIcon != null)
		{
			this.m_extantNoAmmoIcon.renderer.enabled = value;
		}
		if (this.GunAmmoCountLabel != null)
		{
			this.GunAmmoCountLabel.IsVisible = value;
		}
		for (int l = 0; l < this.topCapsForModules.Count; l++)
		{
			this.topCapsForModules[l].IsVisible = value;
			this.bottomCapsForModules[l].IsVisible = value;
		}
		if (this.GunClipCountLabel != null)
		{
			this.GunClipCountLabel.IsVisible = value;
		}
		for (int m = 0; m < this.gunSprites.Length; m++)
		{
			tk2dClippedSprite tk2dClippedSprite = this.gunSprites[m];
			if (tk2dClippedSprite.renderer.enabled != value)
			{
				tk2dClippedSprite.renderer.enabled = value;
				this.outlineSprites[m] = SpriteOutlineManager.GetOutlineSprites(tk2dClippedSprite);
				for (int n = 0; n < this.outlineSprites[m].Length; n++)
				{
					if (this.outlineSprites[m][n] && this.outlineSprites[m][n].renderer)
					{
						this.outlineSprites[m][n].renderer.enabled = value;
					}
				}
			}
		}
	}

	// Token: 0x170014F1 RID: 5361
	// (get) Token: 0x06008C0E RID: 35854 RVA: 0x003A5EEC File Offset: 0x003A40EC
	public bool IsFlipping
	{
		get
		{
			return this.m_isCurrentlyFlipping;
		}
	}

	// Token: 0x06008C0F RID: 35855 RVA: 0x003A5EF4 File Offset: 0x003A40F4
	private void DoGunCardFlip(Gun newGun, int change)
	{
		if (this.AdditionalGunBoxSprites.Count == 0)
		{
			return;
		}
		if (this.AdditionalGunBoxSprites.Count > 10)
		{
			return;
		}
		if (!this.m_isCurrentlyFlipping && GameUIRoot.Instance.GunventoryFolded)
		{
			if (change > 0)
			{
				base.StartCoroutine(this.HandleGunCardFlipReverse(newGun));
			}
			else
			{
				base.StartCoroutine(this.HandleGunCardFlip(newGun));
			}
		}
		else if (!this.m_cardFlippedQueued)
		{
		}
	}

	// Token: 0x06008C10 RID: 35856 RVA: 0x003A5F78 File Offset: 0x003A4178
	private Transform GetChildestOfTransforms(Transform parent)
	{
		Transform transform = parent;
		while (transform != null && transform.childCount > 0)
		{
			transform = this.GetFirstValidChild(transform);
		}
		return transform;
	}

	// Token: 0x06008C11 RID: 35857 RVA: 0x003A5FB0 File Offset: 0x003A41B0
	private IEnumerator WaitForCurrentGunFlipToEnd(Gun newGun, int change)
	{
		this.m_cardFlippedQueued = true;
		while (this.m_isCurrentlyFlipping)
		{
			yield return null;
		}
		if (change > 0)
		{
			this.m_deferCurrentGunSwap = true;
		}
		this.m_isCurrentlyFlipping = true;
		yield return null;
		this.m_cardFlippedQueued = false;
		if (change > 0)
		{
			base.StartCoroutine(this.HandleGunCardFlipReverse(newGun));
		}
		else
		{
			base.StartCoroutine(this.HandleGunCardFlip(newGun));
		}
		yield break;
	}

	// Token: 0x06008C12 RID: 35858 RVA: 0x003A5FDC File Offset: 0x003A41DC
	private IEnumerator HandleGunCardFlipReverse(Gun newGun)
	{
		this.m_deferCurrentGunSwap = true;
		this.m_isCurrentlyFlipping = true;
		this.m_currentFlipReverse = true;
		float elapsed = 0f;
		float p2u = this.GunBoxSprite.PixelsToUnits();
		Transform gbTransform = this.GunBoxSprite.transform;
		GameObject placeholderCardObject = UnityEngine.Object.Instantiate<GameObject>(this.ExtraGunCardPrefab);
		dfControl placeholderCard = placeholderCardObject.GetComponent<dfControl>();
		Transform placeholderTransform = placeholderCardObject.transform;
		placeholderCard.Pivot = ((!this.IsLeftAligned) ? dfPivotPoint.TopLeft : dfPivotPoint.TopRight);
		placeholderTransform.parent = this.m_panel.transform;
		this.m_panel.AddControl(placeholderCard);
		placeholderCard.RelativePosition = this.GunBoxSprite.RelativePosition;
		this.m_cachedGunSpriteDefinition = this.m_cachedGun.GetSprite().Collection.spriteDefinitions[this.m_cachedGun.DefaultSpriteID];
		this.m_currentGunSpriteZOffset = -2f;
		for (int i = 0; i < this.AdditionalGunBoxSprites.Count; i++)
		{
			(this.AdditionalGunBoxSprites[i] as dfTextureSprite).Material = this.m_ClippedMaterial;
			this.AdditionalGunBoxSprites[i].Invalidate();
		}
		Vector3 cachedPosition = Vector3.zero;
		Transform firstExtraGunCardTransform = this.GetFirstValidChild(gbTransform);
		firstExtraGunCardTransform.parent = this.m_panel.transform;
		this.m_panel.AddControl(firstExtraGunCardTransform.GetComponent<dfControl>());
		cachedPosition = firstExtraGunCardTransform.position;
		(placeholderCard as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
		Transform leafExtraCardTransform = this.GetChildestOfTransforms(firstExtraGunCardTransform);
		placeholderTransform.parent = leafExtraCardTransform;
		leafExtraCardTransform.GetComponent<dfControl>().AddControl(placeholderCard);
		this.GunBoxSprite.enabled = false;
		tk2dClippedSprite[] newGunSprites = new tk2dClippedSprite[this.gunSprites.Length];
		tk2dSprite[][] oldGunSpritesAndOutlines = new tk2dSprite[this.gunSprites.Length][];
		for (int j = 0; j < this.gunSprites.Length; j++)
		{
			newGunSprites[j] = UnityEngine.Object.Instantiate<GameObject>(this.gunSprites[j].gameObject, this.gunSprites[j].transform.position, Quaternion.identity).GetComponent<tk2dClippedSprite>();
		}
		Vector3 startPosition = placeholderTransform.position + new Vector3(Pixelator.Instance.CurrentTileScale * (float)this.AdditionalBoxOffsetPx * (float)this.AdditionalGunBoxSprites.Count * p2u, 0f, 0f);
		for (int k = 0; k < newGunSprites.Length; k++)
		{
			tk2dClippedSprite tk2dClippedSprite = newGunSprites[k];
			SpriteOutlineManager.RemoveOutlineFromSprite(tk2dClippedSprite, true);
			if (newGun.CurrentAmmo != 0)
			{
				SpriteOutlineManager.AddOutlineToSprite(tk2dClippedSprite, Color.white, 2f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			}
			tk2dClippedSprite.transform.parent = this.gunSprites[k].transform.parent;
			tk2dClippedSprite.transform.position = tk2dClippedSprite.transform.position.WithZ(5f);
			tk2dClippedSprite.renderer.material.SetFloat("_Saturation", (float)((newGun.CurrentAmmo != 0) ? 1 : 0));
			tk2dBaseSprite sprite = newGun.GetSprite();
			oldGunSpritesAndOutlines[k] = tk2dClippedSprite.GetComponentsInChildren<tk2dSprite>();
			for (int l = 0; l < oldGunSpritesAndOutlines[k].Length; l++)
			{
				oldGunSpritesAndOutlines[k][l].scale = tk2dClippedSprite.scale;
				oldGunSpritesAndOutlines[k][l].SetSprite(sprite.Collection, sprite.spriteId);
				SpriteOutlineManager.ForceUpdateOutlineMaterial(oldGunSpritesAndOutlines[k][l], sprite);
			}
		}
		bool hasDepthSwapped = false;
		float adjFlipTime = 0.15f * (float)((this.AdditionalGunBoxSprites.Count <= 20) ? 1 : (this.AdditionalGunBoxSprites.Count / 20));
		while (elapsed < adjFlipTime)
		{
			if (!GameUIRoot.Instance.GunventoryFolded)
			{
				break;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / adjFlipTime;
			if (t >= 0.5f && !hasDepthSwapped)
			{
				this.m_cachedGunSpriteDefinition = null;
				hasDepthSwapped = true;
				if (placeholderTransform)
				{
					placeholderTransform.parent = this.m_panel.transform;
					this.m_panel.AddControl(placeholderCard);
					firstExtraGunCardTransform.parent = placeholderTransform;
					placeholderCard.AddControl(firstExtraGunCardTransform.GetComponent<dfControl>());
					(placeholderCard as dfTextureSprite).Material = this.m_ClippedMaterial;
				}
				for (int m = 0; m < this.AdditionalGunBoxSprites.Count; m++)
				{
					(this.AdditionalGunBoxSprites[m] as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
				}
				this.m_currentGunSpriteZOffset = 5f;
			}
			float xOffset = BraveMathCollege.DoubleLerp(0f, (float)(this.AdditionalGunBoxSprites.Count * this.AdditionalBoxOffsetPx + this.AdditionalBoxOffsetPx * 2) * Pixelator.Instance.CurrentTileScale, (float)(this.AdditionalGunBoxSprites.Count * -(float)this.AdditionalBoxOffsetPx) * Pixelator.Instance.CurrentTileScale, t);
			float yOffset = BraveMathCollege.DoubleLerpSmooth(0f, 24f * Pixelator.Instance.CurrentTileScale, 0f, t);
			float zRotation = (float)((!this.IsLeftAligned) ? 1 : (-1)) * BraveMathCollege.DoubleLerp(0f, -20f, 0f, Mathf.Clamp01(t * 1.1f));
			if (placeholderTransform == null || !placeholderTransform)
			{
				break;
			}
			placeholderTransform.position = startPosition + new Vector3(xOffset * p2u, yOffset * p2u, 0f);
			placeholderTransform.rotation = Quaternion.Euler(0f, 0f, zRotation);
			for (int n = 0; n < newGunSprites.Length; n++)
			{
				Vector3 center = placeholderCard.GetCenter();
				tk2dClippedSprite tk2dClippedSprite2 = newGunSprites[n];
				tk2dClippedSprite2.SetSprite(newGun.GetSprite().Collection, newGun.DefaultSpriteID);
				tk2dBaseSprite[] array = SpriteOutlineManager.GetOutlineSprites<tk2dBaseSprite>(tk2dClippedSprite2);
				for (int num = 0; num < array.Length; num++)
				{
					SpriteOutlineManager.ForceUpdateOutlineMaterial(array[num], tk2dClippedSprite2);
				}
				Bounds untrimmedBounds = tk2dClippedSprite2.Collection.spriteDefinitions[newGun.DefaultSpriteID].GetUntrimmedBounds();
				Vector3 vector = Vector3.Scale(untrimmedBounds.min + untrimmedBounds.extents, tk2dClippedSprite2.scale);
				float z = (tk2dClippedSprite2.transform.rotation * new Vector3(-vector.x, vector.y * -1f, vector.z)).z;
				Vector3 vector2 = this.GetOffsetVectorForGun(newGun, true).WithZ(z);
				tk2dClippedSprite2.transform.position = center.WithZ((!hasDepthSwapped) ? 5f : (center.z - 2f)) + vector2;
				tk2dClippedSprite2.transform.position = tk2dClippedSprite2.transform.position.Quantize(this.GunBoxSprite.PixelsToUnits() * 3f);
				if (t >= 1f)
				{
					zRotation = 0f;
				}
				tk2dClippedSprite2.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
			}
			int hasAdditionalBGSprites = 0;
			if (this.addlBgSpritesForModules.Count > 0 && this.addlBgSpritesForModules[0].Count > 0)
			{
				hasAdditionalBGSprites = 1;
			}
			this.m_currentGunSpriteXOffset = (float)this.AdditionalBoxOffsetPx * Pixelator.Instance.CurrentTileScale * p2u * (float)hasAdditionalBGSprites * (float)((!this.IsLeftAligned) ? (-1) : 1);
			float extraCardXOffset = BraveMathCollege.SmoothLerp((float)(-(float)this.AdditionalBoxOffsetPx) * Pixelator.Instance.CurrentTileScale * p2u, 0f, t);
			if (firstExtraGunCardTransform)
			{
				firstExtraGunCardTransform.position = cachedPosition + new Vector3(extraCardXOffset, 0f, 0f);
				firstExtraGunCardTransform.rotation = Quaternion.identity;
			}
			if (elapsed > adjFlipTime)
			{
				this.m_currentGunSpriteXOffset = 0f;
				this.m_cachedGunSpriteDefinition = null;
			}
			yield return null;
			for (int num2 = 0; num2 < newGunSprites.Length; num2++)
			{
				tk2dClippedSprite tk2dClippedSprite3 = newGunSprites[num2];
				if (!tk2dClippedSprite3.renderer.enabled)
				{
					for (int num3 = 0; num3 < oldGunSpritesAndOutlines[num2].Length; num3++)
					{
						oldGunSpritesAndOutlines[num2][num3].renderer.enabled = true;
					}
				}
			}
		}
		this.m_currentGunSpriteXOffset = 0f;
		this.m_cachedGunSpriteDefinition = null;
		this.m_deferCurrentGunSwap = false;
		yield return null;
		this.m_cachedGunSpriteDefinition = null;
		this.PostFlipReset(firstExtraGunCardTransform, gbTransform, placeholderCardObject, newGunSprites, newGun);
		this.m_isCurrentlyFlipping = false;
		yield break;
	}

	// Token: 0x06008C13 RID: 35859 RVA: 0x003A6000 File Offset: 0x003A4200
	private Transform GetFirstValidChild(Transform source)
	{
		for (int i = 0; i < source.childCount; i++)
		{
			if (source.GetChild(i))
			{
				if (!this.GunQuickSwitchIcon || !(source.GetChild(i) == this.GunQuickSwitchIcon.transform))
				{
					return source.GetChild(i);
				}
			}
		}
		return null;
	}

	// Token: 0x06008C14 RID: 35860 RVA: 0x003A6070 File Offset: 0x003A4270
	public void UpdateNoAmmoIcon()
	{
		if (this.m_extantNoAmmoIcon != null)
		{
			this.m_extantNoAmmoIcon.scale = this.gunSprites[0].scale;
			this.m_extantNoAmmoIcon.transform.position = this.GunBoxSprite.GetCenter().Quantize(0.0625f * this.m_extantNoAmmoIcon.scale.x).WithZ(this.m_panel.transform.position.z - 3f);
		}
	}

	// Token: 0x06008C15 RID: 35861 RVA: 0x003A6104 File Offset: 0x003A4304
	public void AddNoAmmoIcon()
	{
		if (this.m_extantNoAmmoIcon == null)
		{
			this.gunSprites[0].renderer.material.SetFloat("_Saturation", 0f);
			SpriteOutlineManager.ToggleOutlineRenderers(this.gunSprites[0], false);
			tk2dSprite component = ((GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("Global Prefabs/NoAmmoIcon", ".prefab"))).GetComponent<tk2dSprite>();
			component.transform.parent = this.m_panel.transform;
			component.HeightOffGround = 5f;
			component.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
			component.scale = this.gunSprites[0].scale;
			component.transform.position = this.GunBoxSprite.GetCenter().Quantize(0.0625f * component.scale.x);
			this.m_extantNoAmmoIcon = component;
		}
	}

	// Token: 0x06008C16 RID: 35862 RVA: 0x003A61E4 File Offset: 0x003A43E4
	public void ClearNoAmmoIcon()
	{
		if (this.m_extantNoAmmoIcon != null)
		{
			if (this.m_isCurrentlyFlipping && this.m_currentFlipReverse)
			{
				this.m_extantNoAmmoIcon.renderer.enabled = false;
			}
			else
			{
				SpriteOutlineManager.ToggleOutlineRenderers(this.gunSprites[0], true);
				this.gunSprites[0].renderer.material.SetFloat("_Saturation", 1f);
				UnityEngine.Object.Destroy(this.m_extantNoAmmoIcon.gameObject);
				this.m_extantNoAmmoIcon = null;
			}
		}
	}

	// Token: 0x06008C17 RID: 35863 RVA: 0x003A6274 File Offset: 0x003A4474
	private IEnumerator HandleGunCardFlip(Gun newGun)
	{
		this.m_deferCurrentGunSwap = true;
		this.m_isCurrentlyFlipping = true;
		this.m_currentFlipReverse = false;
		float elapsed = 0f;
		float p2u = this.GunBoxSprite.PixelsToUnits();
		Transform gbTransform = this.GunBoxSprite.transform;
		GameObject placeholderCardObject = UnityEngine.Object.Instantiate<GameObject>(this.ExtraGunCardPrefab);
		dfControl placeholderCard = placeholderCardObject.GetComponent<dfControl>();
		Transform placeholderTransform = placeholderCardObject.transform;
		placeholderCard.Pivot = ((!this.IsLeftAligned) ? dfPivotPoint.TopLeft : dfPivotPoint.TopRight);
		placeholderTransform.parent = this.m_panel.transform;
		this.m_panel.AddControl(placeholderCard);
		placeholderCard.RelativePosition = this.GunBoxSprite.RelativePosition;
		this.m_currentGunSpriteZOffset = 5f;
		for (int i = 0; i < this.AdditionalGunBoxSprites.Count; i++)
		{
			(this.AdditionalGunBoxSprites[i] as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
			this.AdditionalGunBoxSprites[i].Invalidate();
		}
		Vector3 cachedPosition = Vector3.zero;
		Transform newChild = this.GetFirstValidChild(gbTransform);
		newChild.parent = placeholderTransform;
		(placeholderCard as dfTextureSprite).Material = this.m_ClippedMaterial;
		placeholderCard.AddControl(newChild.GetComponent<dfControl>());
		cachedPosition = newChild.position;
		this.GunBoxSprite.enabled = false;
		Vector3 startPosition = placeholderTransform.position;
		tk2dClippedSprite[] oldGunSprites = new tk2dClippedSprite[this.gunSprites.Length];
		tk2dBaseSprite[][] gunSpritesAndOutlines = new tk2dBaseSprite[this.gunSprites.Length][];
		tk2dSpriteDefinition[] oldGunSpriteDefinitions = new tk2dSpriteDefinition[this.gunSprites.Length];
		tk2dSpriteCollectionData[] oldGunSpriteCollections = new tk2dSpriteCollectionData[this.gunSprites.Length];
		for (int j = 0; j < this.gunSprites.Length; j++)
		{
			tk2dClippedSprite tk2dClippedSprite = this.gunSprites[j];
			oldGunSprites[j] = UnityEngine.Object.Instantiate<GameObject>(tk2dClippedSprite.gameObject, tk2dClippedSprite.transform.position, Quaternion.identity).GetComponent<tk2dClippedSprite>();
			oldGunSpriteDefinitions[j] = this.m_cachedGun.GetSprite().Collection.spriteDefinitions[this.m_cachedGun.DefaultSpriteID];
			oldGunSpriteCollections[j] = this.m_cachedGun.GetSprite().Collection;
			oldGunSprites[j].transform.parent = tk2dClippedSprite.transform.parent;
			oldGunSprites[j].transform.position = oldGunSprites[j].transform.position.WithZ(-2f);
			oldGunSprites[j].renderer.material.SetFloat("_Saturation", (float)((this.m_cachedGun.CurrentAmmo != 0) ? 1 : 0));
			SpriteOutlineManager.RemoveOutlineFromSprite(oldGunSprites[j], true);
			if (this.m_cachedGun.CurrentAmmo != 0)
			{
				SpriteOutlineManager.AddOutlineToSprite(oldGunSprites[j], Color.white, 2f, 0f, SpriteOutlineManager.OutlineType.NORMAL);
			}
			gunSpritesAndOutlines[j] = tk2dClippedSprite.GetComponentsInChildren<tk2dBaseSprite>();
		}
		bool hasDepthSwapped = false;
		float adjFlipTime = 0.15f * (float)((this.AdditionalGunBoxSprites.Count <= 20) ? 1 : (this.AdditionalGunBoxSprites.Count / 20));
		while (elapsed < adjFlipTime)
		{
			if (!GameUIRoot.Instance.GunventoryFolded)
			{
				break;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / adjFlipTime;
			if (t >= 0.5f && !hasDepthSwapped)
			{
				hasDepthSwapped = true;
				newChild.parent = this.m_panel.transform;
				this.m_panel.AddControl(newChild.GetComponent<dfControl>());
				(placeholderCard as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
				for (int k = 0; k < this.AdditionalGunBoxSprites.Count; k++)
				{
					(this.AdditionalGunBoxSprites[k] as dfTextureSprite).Material = this.m_ClippedMaterial;
				}
				this.m_currentGunSpriteZOffset = -2f;
				Transform childestOfTransforms = this.GetChildestOfTransforms(newChild);
				if (placeholderTransform)
				{
					placeholderTransform.parent = childestOfTransforms;
				}
				childestOfTransforms.GetComponent<dfControl>().AddControl(placeholderCard);
			}
			float xOffset = BraveMathCollege.DoubleLerp(0f, (float)(this.AdditionalGunBoxSprites.Count * this.AdditionalBoxOffsetPx + this.AdditionalBoxOffsetPx * 2) * Pixelator.Instance.CurrentTileScale, (float)(this.AdditionalGunBoxSprites.Count * this.AdditionalBoxOffsetPx) * Pixelator.Instance.CurrentTileScale, t);
			float yOffset = BraveMathCollege.DoubleLerpSmooth(0f, 24f * Pixelator.Instance.CurrentTileScale, 0f, t);
			float zRotation = (float)((!this.IsLeftAligned) ? 1 : (-1)) * BraveMathCollege.DoubleLerp(0f, -20f, 0f, t);
			if (placeholderTransform)
			{
				placeholderTransform.position = startPosition + new Vector3(xOffset * p2u, yOffset * p2u, 0f);
				placeholderTransform.rotation = Quaternion.Euler(0f, 0f, zRotation);
			}
			for (int l = 0; l < this.gunSprites.Length; l++)
			{
				tk2dClippedSprite tk2dClippedSprite2 = oldGunSprites[l];
				tk2dSprite[] array = SpriteOutlineManager.GetOutlineSprites<tk2dSprite>(tk2dClippedSprite2);
				if (array != null)
				{
					for (int m = 0; m < array.Length; m++)
					{
						if (array[m])
						{
							array[m].SetSprite(oldGunSpriteCollections[l], tk2dClippedSprite2.spriteId);
							array[m].ForceUpdateMaterial();
							SpriteOutlineManager.ForceRebuildMaterial(array[m], tk2dClippedSprite2, Color.white, 0f);
						}
					}
				}
				if (placeholderCard)
				{
					Vector3 center = placeholderCard.GetCenter();
					Vector3 vector = Vector3.Scale(oldGunSpriteDefinitions[l].GetUntrimmedBounds().extents, tk2dClippedSprite2.scale);
					Vector3 vector2 = tk2dClippedSprite2.transform.rotation * new Vector3(-vector.x, vector.y * -1f, vector.z);
					tk2dClippedSprite2.transform.position = center.WithZ((!hasDepthSwapped) ? (center.z - 2f) : 5f) + vector2;
					tk2dClippedSprite2.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
				}
			}
			float extraCardXOffset = BraveMathCollege.SmoothLerp(0f, (float)(-(float)this.AdditionalBoxOffsetPx) * Pixelator.Instance.CurrentTileScale * p2u, t);
			this.m_currentGunSpriteXOffset = (float)this.AdditionalBoxOffsetPx * Pixelator.Instance.CurrentTileScale * p2u + extraCardXOffset;
			if (newChild)
			{
				newChild.position = cachedPosition + new Vector3(extraCardXOffset, 0f, 0f);
				newChild.rotation = Quaternion.identity;
			}
			yield return null;
			this.m_deferCurrentGunSwap = false;
			for (int n = 0; n < this.gunSprites.Length; n++)
			{
				if (!this.gunSprites[n].renderer.enabled)
				{
					for (int num = 0; num < gunSpritesAndOutlines[n].Length; num++)
					{
						gunSpritesAndOutlines[n][num].renderer.enabled = true;
					}
				}
			}
		}
		this.PostFlipReset(newChild, gbTransform, placeholderCardObject, oldGunSprites, newGun);
		this.m_isCurrentlyFlipping = false;
		yield break;
	}

	// Token: 0x06008C18 RID: 35864 RVA: 0x003A6298 File Offset: 0x003A4498
	private void PostFlipReset(Transform newChild, Transform gbTransform, GameObject placeholderCardObject, tk2dClippedSprite[] oldGunSprites, Gun newGun)
	{
		for (int i = 0; i < this.AdditionalGunBoxSprites.Count; i++)
		{
			(this.AdditionalGunBoxSprites[i] as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
		}
		if (newChild)
		{
			newChild.parent = gbTransform;
			this.GunBoxSprite.AddControl(newChild.GetComponent<dfControl>());
			newChild.GetComponent<dfControl>().RelativePosition = new Vector3((float)this.AdditionalBoxOffsetPx * Pixelator.Instance.CurrentTileScale, 0f, 0f);
		}
		UnityEngine.Object.Destroy(placeholderCardObject);
		for (int j = 0; j < oldGunSprites.Length; j++)
		{
			UnityEngine.Object.Destroy(oldGunSprites[j].gameObject);
		}
		this.m_currentGunSpriteXOffset = 0f;
		this.m_currentGunSpriteZOffset = 0f;
		this.GunBoxSprite.enabled = true;
		this.UpdateGunSprite(newGun, 0, null);
	}

	// Token: 0x06008C19 RID: 35865 RVA: 0x003A6384 File Offset: 0x003A4584
	private void UpdateGunSprite(Gun newGun, int change, Gun secondaryGun = null)
	{
		if (newGun != this.m_cachedGun && !this.SuppressNextGunFlip && this.m_cachedGun != null)
		{
			this.DoGunCardFlip(newGun, change);
		}
		this.SuppressNextGunFlip = false;
		if (newGun.CurrentAmmo == 0)
		{
			this.AddNoAmmoIcon();
			this.UpdateNoAmmoIcon();
		}
		else
		{
			this.ClearNoAmmoIcon();
		}
		for (int i = 0; i < this.gunSprites.Length; i++)
		{
			Gun gun = ((i <= 0 || !secondaryGun) ? newGun : secondaryGun);
			tk2dBaseSprite sprite = gun.GetSprite();
			int num = sprite.spriteId;
			tk2dSpriteCollectionData collection = sprite.Collection;
			if (gun.OnlyUsesIdleInWeaponBox)
			{
				num = gun.DefaultSpriteID;
			}
			else if (gun.weaponPanelSpriteOverride)
			{
				num = gun.weaponPanelSpriteOverride.GetMatch(num);
			}
			tk2dClippedSprite tk2dClippedSprite = this.gunSprites[i];
			tk2dClippedSprite.scale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_panel.GetManager()) * Vector3.one;
			for (int j = 0; j < this.outlineSprites[i].Length; j++)
			{
				if (this.outlineSprites[i].Length > 1)
				{
					float num2 = ((j != 1) ? 0f : 0.0625f);
					num2 = ((j != 3) ? num2 : (-0.0625f));
					float num3 = ((j != 0) ? 0f : 0.0625f);
					num3 = ((j != 2) ? num3 : (-0.0625f));
					this.outlineSprites[i][j].transform.localPosition = (new Vector3(num2, num3, 0f) * Pixelator.Instance.CurrentTileScale * 16f * GameUIRoot.Instance.PixelsToUnits()).WithZ(this.UI_OUTLINE_DEPTH);
				}
				this.outlineSprites[i][j].scale = tk2dClippedSprite.scale;
			}
			if (!this.m_deferCurrentGunSwap)
			{
				if (!tk2dClippedSprite.renderer.enabled)
				{
					this.ToggleRenderers(true);
				}
				if (tk2dClippedSprite.spriteId != num || tk2dClippedSprite.Collection != collection)
				{
					tk2dClippedSprite.SetSprite(collection, num);
					if (tk2dClippedSprite.OverrideMaterialMode != tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE)
					{
						tk2dClippedSprite.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_SIMPLE;
						if (tk2dClippedSprite.renderer && tk2dClippedSprite.renderer.material.shader.name.Contains("Gonner"))
						{
							tk2dClippedSprite.renderer.material.shader = Shader.Find("tk2d/CutoutVertexColorTilted");
						}
					}
					tk2dClippedSprite.renderer.material.EnableKeyword("SATURATION_ON");
					for (int k = 0; k < this.outlineSprites[i].Length; k++)
					{
						this.outlineSprites[i][k].SetSprite(collection, num);
						SpriteOutlineManager.ForceUpdateOutlineMaterial(this.outlineSprites[i][k], sprite);
					}
				}
			}
			Vector3 vector = this.GunBoxSprite.GetCenter();
			if (secondaryGun)
			{
				vector += this.CalculateLocalOffsetForGunInDualWieldMode(newGun, secondaryGun, i);
			}
			tk2dClippedSprite.transform.position = vector + this.GetOffsetVectorForGun((i <= 0 || !secondaryGun) ? newGun : secondaryGun, this.m_isCurrentlyFlipping);
			tk2dClippedSprite.transform.position = tk2dClippedSprite.transform.position.Quantize(this.GunBoxSprite.PixelsToUnits() * 3f);
		}
		if (!newGun.UsesRechargeLikeActiveItem && !newGun.IsUndertaleGun)
		{
			this.GunCooldownFillSprite.IsVisible = false;
			this.GunCooldownForegroundSprite.IsVisible = false;
		}
		else
		{
			this.GunCooldownForegroundSprite.RelativePosition = this.GunBoxSprite.RelativePosition;
			this.GunCooldownFillSprite.RelativePosition = this.GunBoxSprite.RelativePosition + new Vector3(123f, 3f, 0f);
			this.GunCooldownFillSprite.ZOrder = this.GunBoxSprite.ZOrder + 1;
			this.GunCooldownForegroundSprite.ZOrder = this.GunCooldownFillSprite.ZOrder + 1;
			this.GunCooldownFillSprite.IsVisible = true;
			this.GunCooldownForegroundSprite.IsVisible = true;
		}
		if (newGun.UsesRechargeLikeActiveItem || newGun.IsUndertaleGun)
		{
			this.GunCooldownFillSprite.FillAmount = newGun.CurrentActiveItemChargeAmount;
		}
	}

	// Token: 0x06008C1A RID: 35866 RVA: 0x003A6814 File Offset: 0x003A4A14
	private Vector3 CalculateLocalOffsetForGunInDualWieldMode(Gun primary, Gun secondary, int currentIndex)
	{
		float num = this.GunBoxSprite.PixelsToUnits();
		Vector2 vector = this.GunBoxSprite.Size * 0.5f * num;
		Bounds bounds = primary.GetSprite().GetBounds();
		Bounds bounds2 = secondary.GetSprite().GetBounds();
		Bounds bounds3 = ((currentIndex != 0) ? bounds2 : bounds);
		Vector3 vector2 = vector + new Vector2(-8f * num, -8f * num) - Vector2.Scale(bounds3.extents.XY(), this.gunSprites[0].scale.XY());
		if (currentIndex == 0)
		{
			return new Vector3(vector2.x, -vector2.y, 0f);
		}
		return new Vector3(-vector2.x, vector2.y, 0f);
	}

	// Token: 0x06008C1B RID: 35867 RVA: 0x003A68F4 File Offset: 0x003A4AF4
	public Vector2 GetOffsetVectorForSpecificSprite(tk2dBaseSprite targetSprite, bool isFlippingGun)
	{
		tk2dSpriteDefinition currentSpriteDef = targetSprite.GetCurrentSpriteDef();
		Vector3 vector = Vector3.Scale(-currentSpriteDef.GetBounds().min + -currentSpriteDef.GetBounds().extents, this.gunSprites[0].scale);
		if (isFlippingGun)
		{
			vector += new Vector3(this.m_currentGunSpriteXOffset, 0f, this.m_currentGunSpriteZOffset);
		}
		return vector;
	}

	// Token: 0x06008C1C RID: 35868 RVA: 0x003A6970 File Offset: 0x003A4B70
	public Vector3 GetOffsetVectorForGun(Gun newGun, bool isFlippingGun)
	{
		tk2dSpriteDefinition tk2dSpriteDefinition;
		if (this.m_cachedGunSpriteDefinition != null && !isFlippingGun)
		{
			tk2dSpriteDefinition = this.m_cachedGunSpriteDefinition;
		}
		else
		{
			tk2dSpriteDefinition = newGun.GetSprite().Collection.spriteDefinitions[newGun.DefaultSpriteID];
		}
		Vector3 vector = Vector3.Scale(-tk2dSpriteDefinition.GetBounds().min + -tk2dSpriteDefinition.GetBounds().extents, this.gunSprites[0].scale);
		if (isFlippingGun)
		{
			vector += new Vector3(this.m_currentGunSpriteXOffset, 0f, this.m_currentGunSpriteZOffset);
		}
		return vector;
	}

	// Token: 0x170014F2 RID: 5362
	// (get) Token: 0x06008C1D RID: 35869 RVA: 0x003A6A18 File Offset: 0x003A4C18
	private int GUN_BOX_EXTRA_PX_OFFSET
	{
		get
		{
			return (!this.IsLeftAligned) ? 9 : (-9);
		}
	}

	// Token: 0x170014F3 RID: 5363
	// (get) Token: 0x06008C1E RID: 35870 RVA: 0x003A6A30 File Offset: 0x003A4C30
	private int AdditionalBoxOffsetPx
	{
		get
		{
			return (!this.IsLeftAligned) ? 2 : (-2);
		}
	}

	// Token: 0x06008C1F RID: 35871 RVA: 0x003A6A48 File Offset: 0x003A4C48
	protected void RebuildExtraGunCards(GunInventory guns)
	{
		Debug.Log("REBUILDING EXTRA GUN CARDS");
		float num = this.m_panel.PixelsToUnits();
		for (int i = 0; i < this.AdditionalGunBoxSprites.Count; i++)
		{
			this.AdditionalGunBoxSprites[i].transform.parent = null;
			UnityEngine.Object.Destroy(this.AdditionalGunBoxSprites[i].gameObject);
		}
		this.AdditionalGunBoxSprites.Clear();
		dfControl dfControl = this.GunBoxSprite;
		Transform transform = this.GunBoxSprite.transform;
		int num2 = Mathf.Min(guns.AllGuns.Count - 1, GameUIAmmoController.NumberOfAdditionalGunCards);
		for (int j = 0; j < num2; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ExtraGunCardPrefab);
			gameObject.transform.parent = transform;
			dfControl component = gameObject.GetComponent<dfControl>();
			dfControl.AddControl(component);
			component.RelativePosition = new Vector3((float)this.AdditionalBoxOffsetPx * Pixelator.Instance.CurrentTileScale, 0f, 0f);
			dfControl = component;
			transform = gameObject.transform;
			this.AdditionalGunBoxSprites.Add(component);
		}
		float num3 = (float)((!this.IsLeftAligned) ? 1 : (-1)) * Pixelator.Instance.CurrentTileScale * (float)(this.m_cachedNumberModules - 1) * -10f * num;
		float num4 = (float)(-(float)this.AdditionalBoxOffsetPx * this.AdditionalGunBoxSprites.Count + -(float)this.GUN_BOX_EXTRA_PX_OFFSET) * Pixelator.Instance.CurrentTileScale * num;
		if (this.IsLeftAligned)
		{
			this.GunBoxSprite.transform.position = this.GunBoxSprite.transform.position.WithX(this.m_panel.transform.position.x - this.m_panel.Width * num + num4 + num3);
		}
		else
		{
			this.GunBoxSprite.transform.position = this.m_panel.transform.position + new Vector3(num4 + num3, 0f, 0f);
		}
		this.GunBoxSprite.Invalidate();
	}

	// Token: 0x06008C20 RID: 35872 RVA: 0x003A6C70 File Offset: 0x003A4E70
	private GameUIAmmoType GetUIAmmoType(GameUIAmmoType.AmmoType sourceType, string customType)
	{
		GameUIAmmoType[] array = this.ammoTypes;
		if (this.IsLeftAligned)
		{
			for (int i = 0; i < GameUIRoot.Instance.ammoControllers.Count; i++)
			{
				if (!GameUIRoot.Instance.ammoControllers[i].IsLeftAligned)
				{
					array = GameUIRoot.Instance.ammoControllers[i].ammoTypes;
					break;
				}
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (sourceType == GameUIAmmoType.AmmoType.CUSTOM)
			{
				if (array[j].ammoType == GameUIAmmoType.AmmoType.CUSTOM && array[j].customAmmoType == customType)
				{
					return array[j];
				}
			}
			else if (array[j].ammoType == sourceType)
			{
				return array[j];
			}
		}
		return array[0];
	}

	// Token: 0x06008C21 RID: 35873 RVA: 0x003A6D40 File Offset: 0x003A4F40
	public void TriggerUIDisabled()
	{
		if (GameUIRoot.Instance.ForceHideGunPanel)
		{
			this.ToggleRenderers(false);
		}
	}

	// Token: 0x06008C22 RID: 35874 RVA: 0x003A6D58 File Offset: 0x003A4F58
	private void CleanupLists(int numberModules)
	{
		for (int i = this.fgSpritesForModules.Count - 1; i >= numberModules; i--)
		{
			if (this.fgSpritesForModules[i])
			{
				UnityEngine.Object.Destroy(this.fgSpritesForModules[i].gameObject);
				this.fgSpritesForModules.RemoveAt(i);
			}
			if (this.bgSpritesForModules[i])
			{
				UnityEngine.Object.Destroy(this.bgSpritesForModules[i].gameObject);
				this.bgSpritesForModules.RemoveAt(i);
			}
			this.cachedAmmoTypesForModules.RemoveAt(i);
			this.cachedCustomAmmoTypesForModules.RemoveAt(i);
		}
		for (int j = this.addlFgSpritesForModules.Count - 1; j >= numberModules; j--)
		{
			if (this.addlFgSpritesForModules[j] != null)
			{
				for (int k = this.addlFgSpritesForModules[j].Count - 1; k >= 0; k--)
				{
					UnityEngine.Object.Destroy(this.addlFgSpritesForModules[j][k].gameObject);
					UnityEngine.Object.Destroy(this.addlBgSpritesForModules[j][k].gameObject);
				}
				this.addlFgSpritesForModules.RemoveAt(j);
				this.addlBgSpritesForModules.RemoveAt(j);
			}
		}
		for (int l = this.topCapsForModules.Count - 1; l >= numberModules; l--)
		{
			if (this.topCapsForModules[l])
			{
				UnityEngine.Object.Destroy(this.topCapsForModules[l].gameObject);
				this.topCapsForModules.RemoveAt(l);
			}
			if (this.bottomCapsForModules[l])
			{
				UnityEngine.Object.Destroy(this.bottomCapsForModules[l].gameObject);
				this.bottomCapsForModules.RemoveAt(l);
			}
		}
		for (int m = this.m_cachedModuleShotsRemaining.Count - 1; m >= numberModules; m--)
		{
			this.m_cachedModuleShotsRemaining.RemoveAt(m);
		}
	}

	// Token: 0x06008C23 RID: 35875 RVA: 0x003A6F6C File Offset: 0x003A516C
	private void EnsureInitialization(int usedModuleIndex)
	{
		if (usedModuleIndex >= this.fgSpritesForModules.Count)
		{
			this.fgSpritesForModules.Add(null);
		}
		if (usedModuleIndex >= this.bgSpritesForModules.Count)
		{
			this.bgSpritesForModules.Add(null);
		}
		if (usedModuleIndex >= this.addlFgSpritesForModules.Count)
		{
			this.addlFgSpritesForModules.Add(new List<dfTiledSprite>());
		}
		if (usedModuleIndex >= this.addlBgSpritesForModules.Count)
		{
			this.addlBgSpritesForModules.Add(new List<dfTiledSprite>());
		}
		if (usedModuleIndex >= this.cachedAmmoTypesForModules.Count)
		{
			this.cachedAmmoTypesForModules.Add(GameUIAmmoType.AmmoType.SMALL_BULLET);
		}
		if (usedModuleIndex >= this.cachedCustomAmmoTypesForModules.Count)
		{
			this.cachedCustomAmmoTypesForModules.Add(string.Empty);
		}
		if (usedModuleIndex >= this.topCapsForModules.Count)
		{
			dfSprite dfSprite = this.topCapsForModules[0].Parent.AddPrefab(this.topCapsForModules[0].gameObject) as dfSprite;
			dfSprite dfSprite2 = this.bottomCapsForModules[0].Parent.AddPrefab(this.bottomCapsForModules[0].gameObject) as dfSprite;
			this.topCapsForModules.Add(dfSprite);
			this.bottomCapsForModules.Add(dfSprite2);
		}
		if (usedModuleIndex >= this.m_cachedModuleShotsRemaining.Count)
		{
			this.m_cachedModuleShotsRemaining.Add(0);
		}
	}

	// Token: 0x06008C24 RID: 35876 RVA: 0x003A70D4 File Offset: 0x003A52D4
	public void UpdateUIGun(GunInventory guns, int inventoryShift)
	{
		if (!this.m_initialized)
		{
			this.Initialize();
		}
		if (guns.AllGuns.Count != 0 && guns.AllGuns.Count - 1 != this.AdditionalGunBoxSprites.Count && GameUIRoot.Instance.GunventoryFolded && !this.m_isCurrentlyFlipping && (guns.AllGuns.Count - 1 < this.AdditionalGunBoxSprites.Count || this.AdditionalGunBoxSprites.Count < GameUIAmmoController.NumberOfAdditionalGunCards))
		{
			this.RebuildExtraGunCards(guns);
		}
		Gun currentGun = guns.CurrentGun;
		Gun currentSecondaryGun = guns.CurrentSecondaryGun;
		if (currentGun == null || GameUIRoot.Instance.ForceHideGunPanel || this.temporarilyPreventVisible || this.forceInvisiblePermanent)
		{
			this.ToggleRenderers(false);
			return;
		}
		this.GunQuickSwitchIcon.IsVisible = false;
		int num = 0;
		for (int i = 0; i < currentGun.Volley.projectiles.Count; i++)
		{
			ProjectileModule projectileModule = currentGun.Volley.projectiles[i];
			if (projectileModule == currentGun.DefaultModule || (projectileModule.IsDuctTapeModule && projectileModule.ammoCost > 0))
			{
				num++;
			}
		}
		if (currentSecondaryGun)
		{
			for (int j = 0; j < currentSecondaryGun.Volley.projectiles.Count; j++)
			{
				ProjectileModule projectileModule2 = currentSecondaryGun.Volley.projectiles[j];
				if (projectileModule2 == currentSecondaryGun.DefaultModule || (projectileModule2.IsDuctTapeModule && projectileModule2.ammoCost > 0))
				{
					num++;
				}
			}
		}
		bool flag = currentGun != this.m_cachedGun || currentGun.DidTransformGunThisFrame;
		currentGun.DidTransformGunThisFrame = false;
		this.UpdateGunSprite(currentGun, inventoryShift, currentSecondaryGun);
		if (num != this.m_cachedNumberModules)
		{
			int num2 = num - this.m_cachedNumberModules;
			float num3 = (float)((!this.IsLeftAligned) ? 1 : (-1)) * Pixelator.Instance.CurrentTileScale * (float)num2 * -10f;
			this.GunAmmoCountLabel.RelativePosition += new Vector3(num3, 0f, 0f);
			this.GunBoxSprite.RelativePosition += new Vector3(num3, 0f, 0f);
		}
		if (this.m_cachedTotalAmmo != currentGun.CurrentAmmo || this.m_cachedMaxAmmo != currentGun.AdjustedMaxAmmo || this.m_cachedUndertaleness != currentGun.IsUndertaleGun)
		{
			if (currentGun.IsUndertaleGun)
			{
				if (!this.IsLeftAligned && this.m_cachedMaxAmmo == 2147483647)
				{
					this.GunAmmoCountLabel.RelativePosition = this.GunAmmoCountLabel.RelativePosition + new Vector3(3f, 0f, 0f);
				}
				this.GunAmmoCountLabel.Text = "0/0";
			}
			else if (currentGun.InfiniteAmmo)
			{
				if (!this.IsLeftAligned && (!this.m_cachedGun || !this.m_cachedGun.InfiniteAmmo))
				{
					this.GunAmmoCountLabel.RelativePosition = this.GunAmmoCountLabel.RelativePosition + new Vector3(-3f, 0f, 0f);
				}
				this.GunAmmoCountLabel.ProcessMarkup = true;
				this.GunAmmoCountLabel.ColorizeSymbols = false;
				this.GunAmmoCountLabel.Text = "[sprite \"infinite-big\"]";
			}
			else if (currentGun.AdjustedMaxAmmo > 0)
			{
				if (!this.IsLeftAligned && this.m_cachedMaxAmmo == 2147483647)
				{
					this.GunAmmoCountLabel.RelativePosition = this.GunAmmoCountLabel.RelativePosition + new Vector3(3f, 0f, 0f);
				}
				this.GunAmmoCountLabel.Text = currentGun.CurrentAmmo.ToString() + "/" + currentGun.AdjustedMaxAmmo.ToString();
			}
			else
			{
				if (!this.IsLeftAligned && this.m_cachedMaxAmmo == 2147483647)
				{
					this.GunAmmoCountLabel.RelativePosition = this.GunAmmoCountLabel.RelativePosition + new Vector3(3f, 0f, 0f);
				}
				this.GunAmmoCountLabel.Text = currentGun.CurrentAmmo.ToString();
			}
		}
		this.CleanupLists(num);
		int num4 = 0;
		int num5 = currentGun.Volley.projectiles.Count;
		if (currentSecondaryGun)
		{
			num5 += currentSecondaryGun.Volley.projectiles.Count;
		}
		for (int k = 0; k < num5; k++)
		{
			Gun gun = ((k < currentGun.Volley.projectiles.Count) ? currentGun : currentSecondaryGun);
			int num6 = ((!(gun == currentGun)) ? (k - currentGun.Volley.projectiles.Count) : k);
			ProjectileModule projectileModule3 = gun.Volley.projectiles[num6];
			bool flag2 = projectileModule3 == gun.DefaultModule || (projectileModule3.IsDuctTapeModule && projectileModule3.ammoCost > 0);
			if (flag2)
			{
				this.EnsureInitialization(num4);
				dfTiledSprite dfTiledSprite = this.fgSpritesForModules[num4];
				dfTiledSprite dfTiledSprite2 = this.bgSpritesForModules[num4];
				List<dfTiledSprite> list = this.addlFgSpritesForModules[num4];
				List<dfTiledSprite> list2 = this.addlBgSpritesForModules[num4];
				dfSprite dfSprite = this.topCapsForModules[num4];
				dfSprite dfSprite2 = this.bottomCapsForModules[num4];
				GameUIAmmoType.AmmoType ammoType = this.cachedAmmoTypesForModules[num4];
				string text = this.cachedCustomAmmoTypesForModules[num4];
				int num7 = this.m_cachedModuleShotsRemaining[num4];
				this.UpdateAmmoUIForModule(ref dfTiledSprite, ref dfTiledSprite2, list, list2, dfSprite, dfSprite2, projectileModule3, gun, ref ammoType, ref text, ref num7, flag, num - (num4 + 1));
				this.fgSpritesForModules[num4] = dfTiledSprite;
				this.bgSpritesForModules[num4] = dfTiledSprite2;
				this.cachedAmmoTypesForModules[num4] = ammoType;
				this.cachedCustomAmmoTypesForModules[num4] = text;
				this.m_cachedModuleShotsRemaining[num4] = num7;
				num4++;
			}
		}
		if (currentGun.IsHeroSword)
		{
			for (int l = 0; l < this.bgSpritesForModules.Count; l++)
			{
				this.fgSpritesForModules[l].IsVisible = false;
				this.bgSpritesForModules[l].IsVisible = false;
			}
			for (int m = 0; m < this.topCapsForModules.Count; m++)
			{
				this.topCapsForModules[m].IsVisible = false;
				this.bottomCapsForModules[m].IsVisible = false;
			}
		}
		else if (!this.bottomCapsForModules[0].IsVisible)
		{
			for (int n = 0; n < this.bgSpritesForModules.Count; n++)
			{
				this.fgSpritesForModules[n].IsVisible = true;
				this.bgSpritesForModules[n].IsVisible = true;
			}
			for (int num8 = 0; num8 < this.topCapsForModules.Count; num8++)
			{
				this.topCapsForModules[num8].IsVisible = true;
				this.bottomCapsForModules[num8].IsVisible = true;
			}
		}
		this.GunClipCountLabel.IsVisible = false;
		this.m_cachedGun = currentGun;
		this.m_cachedNumberModules = num;
		this.m_cachedTotalAmmo = currentGun.CurrentAmmo;
		this.m_cachedMaxAmmo = currentGun.AdjustedMaxAmmo;
		this.m_cachedUndertaleness = currentGun.IsUndertaleGun;
		this.UpdateAdditionalSprites();
	}

	// Token: 0x06008C25 RID: 35877 RVA: 0x003A78F4 File Offset: 0x003A5AF4
	private void UpdateAmmoUIForModule(ref dfTiledSprite currentAmmoFGSprite, ref dfTiledSprite currentAmmoBGSprite, List<dfTiledSprite> AddlModuleFGSprites, List<dfTiledSprite> AddlModuleBGSprites, dfSprite ModuleTopCap, dfSprite ModuleBottomCap, ProjectileModule module, Gun currentGun, ref GameUIAmmoType.AmmoType cachedAmmoTypeForModule, ref string cachedCustomAmmoTypeForModule, ref int cachedShotsInClip, bool didChangeGun, int numberRemaining)
	{
		int num = ((module.GetModNumberOfShotsInClip(currentGun.CurrentOwner) > 0) ? (module.GetModNumberOfShotsInClip(currentGun.CurrentOwner) - currentGun.RuntimeModuleData[module].numberShotsFired) : currentGun.ammo);
		if (num > currentGun.ammo)
		{
			num = currentGun.ammo;
		}
		int num2 = ((module.GetModNumberOfShotsInClip(currentGun.CurrentOwner) > 0) ? module.GetModNumberOfShotsInClip(currentGun.CurrentOwner) : currentGun.AdjustedMaxAmmo);
		if (currentGun.RequiresFundsToShoot)
		{
			num = Mathf.FloorToInt((float)(currentGun.CurrentOwner as PlayerController).carriedConsumables.Currency / (float)currentGun.CurrencyCostPerShot);
			num2 = Mathf.FloorToInt((float)(currentGun.CurrentOwner as PlayerController).carriedConsumables.Currency / (float)currentGun.CurrencyCostPerShot);
		}
		if (currentAmmoFGSprite == null || didChangeGun || module.ammoType != cachedAmmoTypeForModule || module.customAmmoType != cachedCustomAmmoTypeForModule)
		{
			this.m_additionalAmmoTypeDefinitions.Clear();
			if (currentAmmoFGSprite != null)
			{
				UnityEngine.Object.Destroy(currentAmmoFGSprite.gameObject);
			}
			if (currentAmmoBGSprite != null)
			{
				UnityEngine.Object.Destroy(currentAmmoBGSprite.gameObject);
			}
			for (int i = 0; i < AddlModuleBGSprites.Count; i++)
			{
				UnityEngine.Object.Destroy(AddlModuleBGSprites[i].gameObject);
				UnityEngine.Object.Destroy(AddlModuleFGSprites[i].gameObject);
			}
			AddlModuleBGSprites.Clear();
			AddlModuleFGSprites.Clear();
			GameUIAmmoType uiammoType = this.GetUIAmmoType(module.ammoType, module.customAmmoType);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(uiammoType.ammoBarFG.gameObject);
			GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(uiammoType.ammoBarBG.gameObject);
			gameObject.transform.parent = this.GunBoxSprite.transform.parent;
			gameObject2.transform.parent = this.GunBoxSprite.transform.parent;
			gameObject.name = uiammoType.ammoBarFG.name;
			gameObject2.name = uiammoType.ammoBarBG.name;
			currentAmmoFGSprite = gameObject.GetComponent<dfTiledSprite>();
			currentAmmoBGSprite = gameObject2.GetComponent<dfTiledSprite>();
			this.m_panel.AddControl(currentAmmoFGSprite);
			this.m_panel.AddControl(currentAmmoBGSprite);
			currentAmmoFGSprite.EnableBlackLineFix = module.shootStyle == ProjectileModule.ShootStyle.Beam;
			currentAmmoBGSprite.EnableBlackLineFix = currentAmmoFGSprite.EnableBlackLineFix;
			if (module.usesOptionalFinalProjectile)
			{
				GameUIAmmoType uiammoType2 = this.GetUIAmmoType(module.finalAmmoType, module.finalCustomAmmoType);
				this.m_additionalAmmoTypeDefinitions.Add(uiammoType2);
				gameObject = UnityEngine.Object.Instantiate<GameObject>(uiammoType2.ammoBarFG.gameObject);
				gameObject2 = UnityEngine.Object.Instantiate<GameObject>(uiammoType2.ammoBarBG.gameObject);
				gameObject.transform.parent = this.GunBoxSprite.transform.parent;
				gameObject2.transform.parent = this.GunBoxSprite.transform.parent;
				gameObject.name = uiammoType2.ammoBarFG.name;
				gameObject2.name = uiammoType2.ammoBarBG.name;
				AddlModuleFGSprites.Add(gameObject.GetComponent<dfTiledSprite>());
				AddlModuleBGSprites.Add(gameObject2.GetComponent<dfTiledSprite>());
				this.m_panel.AddControl(AddlModuleFGSprites[0]);
				this.m_panel.AddControl(AddlModuleBGSprites[0]);
			}
		}
		float currentTileScale = Pixelator.Instance.CurrentTileScale;
		int num3 = ((!module.usesOptionalFinalProjectile) ? 0 : module.GetModifiedNumberOfFinalProjectiles(currentGun.CurrentOwner));
		int num4 = num2 - num3;
		int num5 = Mathf.Max(0, num - num3);
		int num6 = Mathf.Min(num3, num);
		int num7 = 125;
		if (module.shootStyle == ProjectileModule.ShootStyle.Beam)
		{
			num7 = 500;
			num3 = Mathf.CeilToInt((float)num3 / 2f);
			num4 = Mathf.CeilToInt((float)num4 / 2f);
			num5 = Mathf.CeilToInt((float)num5 / 2f);
			num6 = Mathf.CeilToInt((float)num6 / 2f);
		}
		num4 = Mathf.Min(num7, num4);
		num5 = Mathf.Min(num7, num5);
		currentAmmoBGSprite.Size = new Vector2(currentAmmoBGSprite.SpriteInfo.sizeInPixels.x * currentTileScale, currentAmmoBGSprite.SpriteInfo.sizeInPixels.y * currentTileScale * (float)num4);
		currentAmmoFGSprite.Size = new Vector2(currentAmmoFGSprite.SpriteInfo.sizeInPixels.x * currentTileScale, currentAmmoFGSprite.SpriteInfo.sizeInPixels.y * currentTileScale * (float)num5);
		for (int j = 0; j < AddlModuleBGSprites.Count; j++)
		{
			AddlModuleBGSprites[j].Size = new Vector2(AddlModuleBGSprites[j].SpriteInfo.sizeInPixels.x * currentTileScale, AddlModuleBGSprites[j].SpriteInfo.sizeInPixels.y * currentTileScale * (float)num3);
			AddlModuleFGSprites[j].Size = new Vector2(AddlModuleFGSprites[j].SpriteInfo.sizeInPixels.x * currentTileScale, AddlModuleFGSprites[j].SpriteInfo.sizeInPixels.y * currentTileScale * (float)num6);
		}
		if (!didChangeGun && this.AmmoBurstVFX != null && cachedShotsInClip > num && !currentGun.IsReloading)
		{
			int num8 = cachedShotsInClip - num;
			for (int k = 0; k < num8; k++)
			{
				GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.AmmoBurstVFX.gameObject);
				dfSprite component = gameObject3.GetComponent<dfSprite>();
				dfSpriteAnimation component2 = gameObject3.GetComponent<dfSpriteAnimation>();
				component.ZOrder = currentAmmoFGSprite.ZOrder + 1;
				float num9 = component.Size.y / 2f;
				currentAmmoFGSprite.AddControl(component);
				component.transform.position = currentAmmoFGSprite.GetCenter();
				component.RelativePosition = component.RelativePosition.WithY((float)k * currentAmmoFGSprite.SpriteInfo.sizeInPixels.y * currentTileScale - num9);
				if (num5 == 0 && num3 > 0)
				{
					component.RelativePosition += new Vector3(0f, AddlModuleFGSprites[0].SpriteInfo.sizeInPixels.y * currentTileScale * Mathf.Max(0f, (float)(num3 - num6) - 0.5f), 0f);
				}
				component2.Play();
			}
		}
		float num10 = currentTileScale * (float)numberRemaining * -10f;
		float num11 = -Pixelator.Instance.CurrentTileScale + num10;
		float num12 = 0f;
		float num13 = ((AddlModuleBGSprites.Count <= 0) ? 0f : AddlModuleBGSprites[0].Size.y);
		if (this.IsLeftAligned)
		{
			ModuleBottomCap.RelativePosition = this.m_panel.Size.WithX(0f).ToVector3ZUp(0f) - ModuleBottomCap.Size.WithX(0f).ToVector3ZUp(0f) + new Vector3(-num11, num12, 0f);
		}
		else
		{
			ModuleBottomCap.RelativePosition = this.m_panel.Size.ToVector3ZUp(0f) - ModuleBottomCap.Size.ToVector3ZUp(0f) + new Vector3(num11, -num12, 0f);
		}
		ModuleTopCap.RelativePosition = ModuleBottomCap.RelativePosition + new Vector3(0f, -currentAmmoBGSprite.Size.y + -num13 + -ModuleTopCap.Size.y, 0f);
		float num14 = ModuleTopCap.Size.x / 2f;
		float num15 = BraveMathCollege.QuantizeFloat(currentAmmoBGSprite.Size.x / 2f - num14, currentTileScale);
		float num16 = currentAmmoFGSprite.Size.x / 2f - num14;
		currentAmmoBGSprite.RelativePosition = ModuleTopCap.RelativePosition + new Vector3(-num15, ModuleTopCap.Size.y, 0f);
		currentAmmoFGSprite.RelativePosition = ModuleTopCap.RelativePosition + new Vector3(-num16, ModuleTopCap.Size.y + currentAmmoFGSprite.SpriteInfo.sizeInPixels.y * (float)(num4 - num5) * currentTileScale, 0f);
		currentAmmoFGSprite.ZOrder = currentAmmoBGSprite.ZOrder + 1;
		if (AddlModuleBGSprites.Count > 0)
		{
			num15 = BraveMathCollege.QuantizeFloat(AddlModuleBGSprites[0].Size.x / 2f - num14, currentTileScale);
			AddlModuleBGSprites[0].RelativePosition = ModuleTopCap.RelativePosition + new Vector3(-num15, ModuleTopCap.Size.y + currentAmmoBGSprite.Size.y, 0f);
			num16 = AddlModuleFGSprites[0].Size.x / 2f - num14;
			AddlModuleFGSprites[0].RelativePosition = ModuleTopCap.RelativePosition + new Vector3(-num16, ModuleTopCap.Size.y + currentAmmoBGSprite.Size.y + AddlModuleFGSprites[0].SpriteInfo.sizeInPixels.y * (float)(num3 - num6) * currentTileScale, 0f);
		}
		cachedAmmoTypeForModule = module.ammoType;
		cachedCustomAmmoTypeForModule = module.customAmmoType;
		cachedShotsInClip = num;
	}

	// Token: 0x06008C26 RID: 35878 RVA: 0x003A82DC File Offset: 0x003A64DC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400931A RID: 37658
	[SerializeField]
	[BetterList]
	public GameUIAmmoType[] ammoTypes;

	// Token: 0x0400931B RID: 37659
	[FormerlySerializedAs("GunAmmoBottomCapSprite")]
	public dfSprite InitialBottomCapSprite;

	// Token: 0x0400931C RID: 37660
	[FormerlySerializedAs("GunAmmoTopCapSprite")]
	public dfSprite InitialTopCapSprite;

	// Token: 0x0400931D RID: 37661
	public dfSprite GunBoxSprite;

	// Token: 0x0400931E RID: 37662
	public dfSprite GunQuickSwitchIcon;

	// Token: 0x0400931F RID: 37663
	public GameObject ExtraGunCardPrefab;

	// Token: 0x04009320 RID: 37664
	public List<dfControl> AdditionalGunBoxSprites = new List<dfControl>();

	// Token: 0x04009321 RID: 37665
	public dfLabel GunClipCountLabel;

	// Token: 0x04009322 RID: 37666
	public dfLabel GunAmmoCountLabel;

	// Token: 0x04009323 RID: 37667
	public dfSprite GunCooldownForegroundSprite;

	// Token: 0x04009324 RID: 37668
	public dfSprite GunCooldownFillSprite;

	// Token: 0x04009325 RID: 37669
	public dfSpriteAnimation AmmoBurstVFX;

	// Token: 0x04009326 RID: 37670
	[NonSerialized]
	public bool temporarilyPreventVisible;

	// Token: 0x04009327 RID: 37671
	[NonSerialized]
	public bool forceInvisiblePermanent;

	// Token: 0x04009328 RID: 37672
	private List<dfTiledSprite> fgSpritesForModules = new List<dfTiledSprite>();

	// Token: 0x04009329 RID: 37673
	private List<dfTiledSprite> bgSpritesForModules = new List<dfTiledSprite>();

	// Token: 0x0400932A RID: 37674
	private List<List<dfTiledSprite>> addlFgSpritesForModules = new List<List<dfTiledSprite>>();

	// Token: 0x0400932B RID: 37675
	private List<List<dfTiledSprite>> addlBgSpritesForModules = new List<List<dfTiledSprite>>();

	// Token: 0x0400932C RID: 37676
	private List<dfSprite> topCapsForModules = new List<dfSprite>();

	// Token: 0x0400932D RID: 37677
	private List<dfSprite> bottomCapsForModules = new List<dfSprite>();

	// Token: 0x0400932E RID: 37678
	private List<GameUIAmmoType.AmmoType> cachedAmmoTypesForModules = new List<GameUIAmmoType.AmmoType>();

	// Token: 0x0400932F RID: 37679
	private List<string> cachedCustomAmmoTypesForModules = new List<string>();

	// Token: 0x04009330 RID: 37680
	private dfPanel m_panel;

	// Token: 0x04009331 RID: 37681
	private List<GameUIAmmoType> m_additionalAmmoTypeDefinitions = new List<GameUIAmmoType>();

	// Token: 0x04009332 RID: 37682
	public tk2dClippedSprite[] gunSprites;

	// Token: 0x04009333 RID: 37683
	public bool IsLeftAligned;

	// Token: 0x04009334 RID: 37684
	private Gun m_cachedGun;

	// Token: 0x04009335 RID: 37685
	private List<int> m_cachedModuleShotsRemaining = new List<int>();

	// Token: 0x04009336 RID: 37686
	private int m_cachedMaxAmmo;

	// Token: 0x04009337 RID: 37687
	private int m_cachedTotalAmmo;

	// Token: 0x04009338 RID: 37688
	private int m_cachedNumberModules = 1;

	// Token: 0x04009339 RID: 37689
	private bool m_cachedUndertaleness;

	// Token: 0x0400933A RID: 37690
	private tk2dSprite[][] outlineSprites;

	// Token: 0x0400933B RID: 37691
	private bool m_initialized;

	// Token: 0x0400933C RID: 37692
	private Material m_ClippedMaterial;

	// Token: 0x0400933D RID: 37693
	private Material m_ClippedZWriteOffMaterial;

	// Token: 0x0400933E RID: 37694
	private float UI_OUTLINE_DEPTH = 1f;

	// Token: 0x0400933F RID: 37695
	private static int NumberOfAdditionalGunCards = 3;

	// Token: 0x04009340 RID: 37696
	private List<dfSprite> m_additionalRegisteredSprites = new List<dfSprite>();

	// Token: 0x04009341 RID: 37697
	private tk2dSpriteDefinition m_cachedGunSpriteDefinition;

	// Token: 0x04009342 RID: 37698
	private bool m_isCurrentlyFlipping;

	// Token: 0x04009343 RID: 37699
	private bool m_currentFlipReverse;

	// Token: 0x04009344 RID: 37700
	private float m_currentGunSpriteXOffset;

	// Token: 0x04009345 RID: 37701
	private float m_currentGunSpriteZOffset;

	// Token: 0x04009346 RID: 37702
	private bool m_deferCurrentGunSwap;

	// Token: 0x04009347 RID: 37703
	private bool m_cardFlippedQueued;

	// Token: 0x04009348 RID: 37704
	private const float FLIP_TIME = 0.15f;

	// Token: 0x04009349 RID: 37705
	private tk2dSprite m_extantNoAmmoIcon;

	// Token: 0x0400934A RID: 37706
	public bool SuppressNextGunFlip;

	// Token: 0x0400934B RID: 37707
	private const int NUM_PIXELS_PER_MODULE = -10;
}
