using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001785 RID: 6021
public class GameUIItemController : MonoBehaviour
{
	// Token: 0x06008C6E RID: 35950 RVA: 0x003AC2F0 File Offset: 0x003AA4F0
	private void Update()
	{
		if (this.temporarilyPreventVisible && this.itemSprite && this.itemSprite.renderer.enabled)
		{
			this.ToggleRenderers(false);
		}
		if (!GameManager.Instance.IsLoadingLevel && Minimap.Instance.IsFullscreen && this.itemSprite && this.itemSprite.renderer && this.itemSprite.renderer.enabled)
		{
			this.itemSprite.renderer.enabled = false;
		}
	}

	// Token: 0x06008C6F RID: 35951 RVA: 0x003AC3A0 File Offset: 0x003AA5A0
	private void Initialize()
	{
		this.m_panel = base.GetComponent<dfPanel>();
		this.itemSprite.usesOverrideMaterial = true;
		this.itemSpriteMaterial = this.itemSprite.renderer.material;
		SpriteOutlineManager.AddOutlineToSprite(this.itemSprite, Color.white);
		this.outlineSprites = SpriteOutlineManager.GetOutlineSprites(this.itemSprite);
		for (int i = 0; i < this.outlineSprites.Length; i++)
		{
			if (this.outlineSprites.Length > 1)
			{
				float num = ((i != 1) ? 0f : 0.0625f);
				num = ((i != 3) ? num : (-0.0625f));
				float num2 = ((i != 0) ? 0f : 0.0625f);
				num2 = ((i != 2) ? num2 : (-0.0625f));
				this.outlineSprites[i].transform.localPosition = (new Vector3(num, num2, 0f) * Pixelator.Instance.CurrentTileScale * 16f * GameUIRoot.Instance.PixelsToUnits()).WithZ(this.UI_OUTLINE_DEPTH);
			}
			this.outlineSprites[i].gameObject.layer = this.itemSprite.gameObject.layer;
		}
		this.m_ClippedMaterial = new Material(ShaderCache.Acquire("Daikon Forge/Clipped UI Shader"));
		this.m_ClippedZWriteOffMaterial = new Material(ShaderCache.Acquire("Daikon Forge/Clipped UI Shader ZWriteOff"));
		this.m_initialized = true;
	}

	// Token: 0x06008C70 RID: 35952 RVA: 0x003AC51C File Offset: 0x003AA71C
	public void ToggleRenderersOld(bool value)
	{
		if (this.ItemBoxSprite)
		{
			this.ItemBoxSprite.IsVisible = value;
		}
		if (this.ItemCountLabel)
		{
			this.SetItemCountVisible(value);
		}
		if (this.itemSprite != null)
		{
			this.itemSprite.renderer.enabled = value;
			this.outlineSprites = SpriteOutlineManager.GetOutlineSprites(this.itemSprite);
			if (this.outlineSprites != null)
			{
				for (int i = 0; i < this.outlineSprites.Length; i++)
				{
					this.outlineSprites[i].renderer.enabled = value;
				}
			}
		}
	}

	// Token: 0x06008C71 RID: 35953 RVA: 0x003AC5C8 File Offset: 0x003AA7C8
	public void ToggleRenderers(bool value)
	{
		this.itemSprite.renderer.enabled = value;
		if (this.ItemBoxSprite != null && this.ItemBoxSprite.Parent != null)
		{
			this.ItemBoxSprite.IsVisible = value;
		}
		if (this.ItemBoxSprite != null)
		{
			this.ItemBoxSprite.IsVisible = value;
		}
		if (this.ItemCountLabel != null && !value)
		{
			this.SetItemCountVisible(value);
		}
		for (int i = 0; i < this.AdditionalItemBoxSprites.Count; i++)
		{
			this.AdditionalItemBoxSprites[i].IsVisible = value;
			this.AdditionalItemBoxSprites[i].IsVisible = value;
		}
		this.outlineSprites = SpriteOutlineManager.GetOutlineSprites(this.itemSprite);
		for (int j = 0; j < this.outlineSprites.Length; j++)
		{
			if (this.outlineSprites[j])
			{
				this.outlineSprites[j].renderer.enabled = value;
			}
		}
	}

	// Token: 0x06008C72 RID: 35954 RVA: 0x003AC6E4 File Offset: 0x003AA8E4
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

	// Token: 0x06008C73 RID: 35955 RVA: 0x003AC710 File Offset: 0x003AA910
	private void DoItemCardFlip(PlayerItem newItem, int change)
	{
		if (this.AdditionalItemBoxSprites.Count == 0)
		{
			return;
		}
		if (!this.m_isCurrentlyFlipping)
		{
			if (change > 0)
			{
				base.StartCoroutine(this.HandleItemCardFlipReverse(newItem));
			}
			else
			{
				base.StartCoroutine(this.HandleItemCardFlip(newItem));
			}
		}
		else if (!this.m_cardFlippedQueued)
		{
			base.StartCoroutine(this.WaitForCurrentItemFlipToEnd(newItem, change));
		}
	}

	// Token: 0x06008C74 RID: 35956 RVA: 0x003AC780 File Offset: 0x003AA980
	private IEnumerator WaitForCurrentItemFlipToEnd(PlayerItem newItem, int change)
	{
		this.m_cardFlippedQueued = true;
		while (this.m_isCurrentlyFlipping)
		{
			yield return null;
		}
		if (change > 0)
		{
			this.m_deferCurrentItemSwap = true;
		}
		this.m_isCurrentlyFlipping = true;
		yield return null;
		this.m_cardFlippedQueued = false;
		if (change > 0)
		{
			base.StartCoroutine(this.HandleItemCardFlipReverse(newItem));
		}
		else
		{
			base.StartCoroutine(this.HandleItemCardFlip(newItem));
		}
		yield break;
	}

	// Token: 0x06008C75 RID: 35957 RVA: 0x003AC7AC File Offset: 0x003AA9AC
	private IEnumerator HandleItemCardFlipReverse(PlayerItem newGun)
	{
		this.m_deferCurrentItemSwap = true;
		this.m_isCurrentlyFlipping = true;
		yield return null;
		float elapsed = 0f;
		float p2u = this.ItemBoxSprite.PixelsToUnits();
		Transform gbTransform = this.ItemBoxSprite.transform;
		GameObject placeholderCardObject = UnityEngine.Object.Instantiate<GameObject>(this.ExtraItemCardPrefab);
		dfControl placeholderCard = placeholderCardObject.GetComponent<dfControl>();
		Transform placeholderTransform = placeholderCardObject.transform;
		placeholderTransform.parent = this.m_panel.transform;
		this.m_panel.AddControl(placeholderCard);
		placeholderCard.RelativePosition = this.ItemBoxSprite.RelativePosition;
		this.m_cachedItemSpriteDefinition = this.m_cachedItem.sprite.Collection.spriteDefinitions[this.m_cachedItem.sprite.spriteId];
		this.m_currentItemSpriteZOffset = -2f;
		for (int i = 0; i < this.AdditionalItemBoxSprites.Count; i++)
		{
			(this.AdditionalItemBoxSprites[i] as dfTextureSprite).Material = this.m_ClippedMaterial;
			this.AdditionalItemBoxSprites[i].Invalidate();
		}
		Vector3 cachedPosition = Vector3.zero;
		Transform firstExtraGunCardTransform = gbTransform.GetChild(0);
		firstExtraGunCardTransform.parent = this.m_panel.transform;
		this.m_panel.AddControl(firstExtraGunCardTransform.GetComponent<dfControl>());
		cachedPosition = firstExtraGunCardTransform.position;
		(placeholderCard as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
		Transform leafExtraCardTransform = firstExtraGunCardTransform.GetFirstLeafChild();
		placeholderTransform.parent = leafExtraCardTransform;
		leafExtraCardTransform.GetComponent<dfControl>().AddControl(placeholderCard);
		this.ItemBoxSprite.IsVisible = false;
		tk2dClippedSprite newGunSprite = UnityEngine.Object.Instantiate<GameObject>(this.itemSprite.gameObject, this.itemSprite.transform.position, Quaternion.identity).GetComponent<tk2dClippedSprite>();
		newGunSprite.transform.parent = this.itemSprite.transform.parent;
		newGunSprite.transform.position = newGunSprite.transform.position.WithZ(5f);
		Vector3 startPosition = placeholderTransform.position + new Vector3(Pixelator.Instance.CurrentTileScale * (float)(-(float)this.AdditionalBoxOffsetPX) * (float)this.AdditionalItemBoxSprites.Count * p2u, 0f, 0f);
		tk2dBaseSprite weapSprite = newGun.sprite;
		this.UpdateItemSpriteScale();
		tk2dBaseSprite[] oldGunSpriteAndOutlines = newGunSprite.GetComponentsInChildren<tk2dBaseSprite>();
		for (int j = 0; j < oldGunSpriteAndOutlines.Length; j++)
		{
			oldGunSpriteAndOutlines[j].scale = newGunSprite.scale;
			oldGunSpriteAndOutlines[j].SetSprite(weapSprite.Collection, weapSprite.spriteId);
			SpriteOutlineManager.ForceUpdateOutlineMaterial(oldGunSpriteAndOutlines[j], weapSprite);
		}
		Bounds itemBounds = newGunSprite.GetUntrimmedBounds();
		bool hasDepthSwapped = false;
		float adjFlipTime = 0.15f * (float)((this.AdditionalItemBoxSprites.Count <= 20) ? 1 : (this.AdditionalItemBoxSprites.Count / 20));
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
				placeholderTransform.parent = this.m_panel.transform;
				this.m_panel.AddControl(placeholderCard);
				firstExtraGunCardTransform.parent = placeholderTransform;
				placeholderCard.AddControl(firstExtraGunCardTransform.GetComponent<dfControl>());
				(placeholderCard as dfTextureSprite).Material = this.m_ClippedMaterial;
				for (int k = 0; k < this.AdditionalItemBoxSprites.Count; k++)
				{
					(this.AdditionalItemBoxSprites[k] as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
				}
				this.m_currentItemSpriteZOffset = 5f;
			}
			float xOffset = BraveMathCollege.DoubleLerp(0f, (float)(this.AdditionalItemBoxSprites.Count * this.AdditionalBoxOffsetPX + this.AdditionalBoxOffsetPX * 2) * Pixelator.Instance.CurrentTileScale * -1f, (float)(this.AdditionalItemBoxSprites.Count * this.AdditionalBoxOffsetPX) * Pixelator.Instance.CurrentTileScale, t);
			float yOffset = BraveMathCollege.DoubleLerpSmooth(0f, 9f * Pixelator.Instance.CurrentTileScale, 0f, t);
			float zRotation = BraveMathCollege.DoubleLerp(0f, 20f, 0f, Mathf.Clamp01(t * 1.1f));
			placeholderTransform.position = startPosition + new Vector3(xOffset * p2u, yOffset * p2u, 0f);
			placeholderTransform.rotation = Quaternion.Euler(0f, 0f, zRotation);
			Vector3 center = placeholderCard.GetCenter();
			Vector3 vector = newGunSprite.transform.rotation * new Vector3(-itemBounds.extents.x, itemBounds.extents.y * -1f, itemBounds.extents.z);
			newGunSprite.transform.position = center.WithZ((!hasDepthSwapped) ? 5f : (center.z - 2f)) + vector;
			newGunSprite.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
			float extraCardXOffset = BraveMathCollege.SmoothLerp((float)this.AdditionalBoxOffsetPX * Pixelator.Instance.CurrentTileScale * p2u, 0f, t);
			this.m_currentItemSpriteXOffset = (float)(-(float)this.AdditionalBoxOffsetPX) * Pixelator.Instance.CurrentTileScale * p2u + extraCardXOffset;
			firstExtraGunCardTransform.position = cachedPosition + new Vector3(extraCardXOffset, 0f, 0f);
			firstExtraGunCardTransform.rotation = Quaternion.identity;
			yield return null;
			if (!newGunSprite.renderer.enabled)
			{
				for (int l = 0; l < oldGunSpriteAndOutlines.Length; l++)
				{
					oldGunSpriteAndOutlines[l].renderer.enabled = true;
				}
			}
		}
		this.m_cachedItemSpriteDefinition = null;
		this.m_deferCurrentItemSwap = false;
		yield return null;
		this.PostFlipReset(firstExtraGunCardTransform, gbTransform, placeholderCardObject, newGunSprite);
		this.m_isCurrentlyFlipping = false;
		yield break;
	}

	// Token: 0x06008C76 RID: 35958 RVA: 0x003AC7D0 File Offset: 0x003AA9D0
	private IEnumerator HandleItemCardFlip(PlayerItem newItem)
	{
		this.m_deferCurrentItemSwap = true;
		this.m_isCurrentlyFlipping = true;
		float elapsed = 0f;
		float p2u = this.ItemBoxSprite.PixelsToUnits();
		Transform gbTransform = this.ItemBoxSprite.transform;
		GameObject placeholderCardObject = UnityEngine.Object.Instantiate<GameObject>(this.ExtraItemCardPrefab);
		dfControl placeholderCard = placeholderCardObject.GetComponent<dfControl>();
		Transform placeholderTransform = placeholderCardObject.transform;
		placeholderTransform.parent = this.m_panel.transform;
		this.m_panel.AddControl(placeholderCard);
		placeholderCard.RelativePosition = this.ItemBoxSprite.RelativePosition;
		this.m_currentItemSpriteZOffset = 5f;
		for (int i = 0; i < this.AdditionalItemBoxSprites.Count; i++)
		{
			(this.AdditionalItemBoxSprites[i] as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
			this.AdditionalItemBoxSprites[i].Invalidate();
		}
		Vector3 cachedPosition = Vector3.zero;
		Transform newChild = gbTransform.GetChild(0);
		newChild.parent = placeholderTransform;
		(placeholderCard as dfTextureSprite).Material = this.m_ClippedMaterial;
		placeholderCard.AddControl(newChild.GetComponent<dfControl>());
		cachedPosition = newChild.position;
		this.ItemBoxSprite.IsVisible = false;
		tk2dClippedSprite previousItemSprite = UnityEngine.Object.Instantiate<GameObject>(this.itemSprite.gameObject, this.itemSprite.transform.position, Quaternion.identity).GetComponent<tk2dClippedSprite>();
		tk2dSpriteCollectionData previousItemSpriteCollection = previousItemSprite.Collection;
		previousItemSprite.transform.parent = this.itemSprite.transform.parent;
		previousItemSprite.transform.position = previousItemSprite.transform.position.WithZ(-2f);
		Vector3 startPosition = placeholderTransform.position;
		tk2dBaseSprite[] gunSpriteAndOutlines = this.itemSprite.GetComponentsInChildren<tk2dBaseSprite>();
		this.UpdateItemSpriteScale();
		for (int j = 0; j < gunSpriteAndOutlines.Length; j++)
		{
		}
		bool hasDepthSwapped = false;
		float adjFlipTime = 0.15f * (float)((this.AdditionalItemBoxSprites.Count <= 20) ? 1 : (this.AdditionalItemBoxSprites.Count / 20));
		while (elapsed < adjFlipTime)
		{
			if (!GameUIRoot.Instance.GunventoryFolded)
			{
				break;
			}
			if (!newChild || !this.m_panel)
			{
				break;
			}
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / adjFlipTime;
			if (t >= 0.5f && !hasDepthSwapped)
			{
				Debug.Log("doing depth swap");
				hasDepthSwapped = true;
				newChild.parent = this.m_panel.transform;
				this.m_panel.AddControl(newChild.GetComponent<dfControl>());
				(placeholderCard as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
				for (int k = 0; k < this.AdditionalItemBoxSprites.Count; k++)
				{
					(this.AdditionalItemBoxSprites[k] as dfTextureSprite).Material = this.m_ClippedMaterial;
				}
				this.m_currentItemSpriteZOffset = -2f;
				Transform firstLeafChild = newChild.GetFirstLeafChild();
				placeholderTransform.parent = firstLeafChild;
				firstLeafChild.GetComponent<dfControl>().AddControl(placeholderCard);
			}
			float xOffset = BraveMathCollege.DoubleLerp(0f, (float)(this.AdditionalItemBoxSprites.Count * this.AdditionalBoxOffsetPX + this.AdditionalBoxOffsetPX * 2) * Pixelator.Instance.CurrentTileScale * -1f, (float)(this.AdditionalItemBoxSprites.Count * this.AdditionalBoxOffsetPX) * Pixelator.Instance.CurrentTileScale * -1f, t);
			float yOffset = BraveMathCollege.DoubleLerpSmooth(0f, 9f * Pixelator.Instance.CurrentTileScale, 0f, t);
			float zRotation = BraveMathCollege.DoubleLerp(0f, 20f, 0f, t);
			if (placeholderTransform)
			{
				placeholderTransform.position = startPosition + new Vector3(xOffset * p2u, yOffset * p2u, 0f);
				placeholderTransform.rotation = Quaternion.Euler(0f, 0f, zRotation);
			}
			tk2dSprite[] array = SpriteOutlineManager.GetOutlineSprites<tk2dSprite>(previousItemSprite);
			if (array != null)
			{
				for (int l = 0; l < array.Length; l++)
				{
					if (array[l])
					{
						array[l].SetSprite(previousItemSpriteCollection, previousItemSprite.spriteId);
						array[l].ForceUpdateMaterial();
						SpriteOutlineManager.ForceRebuildMaterial(array[l], previousItemSprite, Color.white, 0f);
					}
				}
			}
			if (placeholderCard)
			{
				Vector3 center = placeholderCard.GetCenter();
				previousItemSprite.transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
				previousItemSprite.PlaceAtPositionByAnchor(center, tk2dBaseSprite.Anchor.MiddleCenter);
			}
			float extraCardXOffset = BraveMathCollege.SmoothLerp(0f, (float)this.AdditionalBoxOffsetPX * Pixelator.Instance.CurrentTileScale * p2u, t);
			this.m_currentItemSpriteXOffset = (float)(-(float)this.AdditionalBoxOffsetPX) * Pixelator.Instance.CurrentTileScale * p2u + extraCardXOffset;
			if (newChild)
			{
				newChild.position = cachedPosition + new Vector3(extraCardXOffset, 0f, 0f);
				newChild.rotation = Quaternion.identity;
			}
			yield return null;
			this.m_deferCurrentItemSwap = false;
			if (!this.itemSprite.renderer.enabled)
			{
				for (int m = 0; m < gunSpriteAndOutlines.Length; m++)
				{
					gunSpriteAndOutlines[m].renderer.enabled = true;
				}
			}
		}
		this.PostFlipReset(newChild, gbTransform, placeholderCardObject, previousItemSprite);
		this.m_isCurrentlyFlipping = false;
		yield break;
	}

	// Token: 0x06008C77 RID: 35959 RVA: 0x003AC7EC File Offset: 0x003AA9EC
	private void PostFlipReset(Transform newChild, Transform gbTransform, GameObject placeholderCardObject, tk2dClippedSprite oldGunSprite)
	{
		for (int i = 0; i < this.AdditionalItemBoxSprites.Count; i++)
		{
			(this.AdditionalItemBoxSprites[i] as dfTextureSprite).Material = this.m_ClippedZWriteOffMaterial;
		}
		if (newChild)
		{
			if (gbTransform)
			{
				newChild.parent = gbTransform;
			}
			this.ItemBoxSprite.AddControl(newChild.GetComponent<dfControl>());
			newChild.GetComponent<dfControl>().RelativePosition = new Vector3((float)(-(float)this.AdditionalBoxOffsetPX) * Pixelator.Instance.CurrentTileScale, 0f, 0f);
		}
		if (placeholderCardObject)
		{
			UnityEngine.Object.Destroy(placeholderCardObject);
		}
		if (oldGunSprite)
		{
			UnityEngine.Object.Destroy(oldGunSprite.gameObject);
		}
		this.m_currentItemSpriteXOffset = 0f;
		this.m_currentItemSpriteZOffset = 0f;
		this.ItemBoxSprite.IsVisible = true;
	}

	// Token: 0x06008C78 RID: 35960 RVA: 0x003AC8D8 File Offset: 0x003AAAD8
	public void UpdateScale()
	{
		this.ItemBoxSprite.Size = this.ItemBoxSprite.SpriteInfo.sizeInPixels * Pixelator.Instance.CurrentTileScale;
		this.ItemBoxFillSprite.Size = new Vector2(3f, 26f) * Pixelator.Instance.CurrentTileScale;
		this.ItemBoxFGSprite.Size = this.ItemBoxFGSprite.SpriteInfo.sizeInPixels * Pixelator.Instance.CurrentTileScale;
		if (!this.m_isCurrentlyFlipping)
		{
			this.ItemBoxFGSprite.RelativePosition = this.ItemBoxSprite.RelativePosition;
			this.ItemBoxFillSprite.RelativePosition = this.ItemBoxSprite.RelativePosition + new Vector3(123f, 3f, 0f);
		}
	}

	// Token: 0x06008C79 RID: 35961 RVA: 0x003AC9B4 File Offset: 0x003AABB4
	protected void RebuildExtraItemCards(PlayerItem current, List<PlayerItem> items)
	{
		float num = this.m_panel.PixelsToUnits();
		for (int i = 0; i < this.AdditionalItemBoxSprites.Count; i++)
		{
			UnityEngine.Object.Destroy(this.AdditionalItemBoxSprites[i].gameObject);
		}
		this.AdditionalItemBoxSprites.Clear();
		dfControl dfControl = this.ItemBoxSprite;
		Transform transform = this.ItemBoxSprite.transform;
		for (int j = 0; j < items.Count - 1; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.ExtraItemCardPrefab);
			gameObject.transform.parent = transform;
			dfControl component = gameObject.GetComponent<dfControl>();
			dfControl.AddControl(component);
			component.RelativePosition = new Vector3((float)(-(float)this.AdditionalBoxOffsetPX) * Pixelator.Instance.CurrentTileScale, 0f, 0f);
			dfControl = component;
			transform = gameObject.transform;
			this.AdditionalItemBoxSprites.Add(component);
		}
		float num2 = (float)(this.AdditionalBoxOffsetPX * this.AdditionalItemBoxSprites.Count) * Pixelator.Instance.CurrentTileScale * num;
		if (this.IsRightAligned)
		{
			this.ItemBoxSprite.transform.position = this.ItemBoxSprite.transform.position.WithX(this.m_panel.transform.position.x + -(this.ItemBoxSprite.Width * num) + num2);
		}
		else
		{
			this.ItemBoxSprite.transform.position = this.m_panel.transform.position + new Vector3(num2, 0f, 0f);
		}
		this.ItemBoxSprite.Invalidate();
	}

	// Token: 0x06008C7A RID: 35962 RVA: 0x003ACB6C File Offset: 0x003AAD6C
	public void DimItemSprite()
	{
		if (this.m_cachedItem == null)
		{
			return;
		}
		this.itemSprite.gameObject.SetActive(false);
	}

	// Token: 0x06008C7B RID: 35963 RVA: 0x003ACB94 File Offset: 0x003AAD94
	public void UndimItemSprite()
	{
		if (this.m_cachedItem == null)
		{
			return;
		}
		this.itemSprite.gameObject.SetActive(true);
	}

	// Token: 0x17001501 RID: 5377
	// (get) Token: 0x06008C7C RID: 35964 RVA: 0x003ACBBC File Offset: 0x003AADBC
	private int AdditionalBoxOffsetPX
	{
		get
		{
			return (!this.IsRightAligned) ? 2 : (-2);
		}
	}

	// Token: 0x06008C7D RID: 35965 RVA: 0x003ACBD4 File Offset: 0x003AADD4
	private void UpdateItemSpriteScale()
	{
		this.itemSprite.scale = GameUIUtility.GetCurrentTK2D_DFScale(this.m_panel.GetManager()) * Vector3.one;
		for (int i = 0; i < this.outlineSprites.Length; i++)
		{
			if (this.outlineSprites.Length > 1)
			{
				float num = ((i != 1) ? 0f : 0.0625f);
				num = ((i != 3) ? num : (-0.0625f));
				float num2 = ((i != 0) ? 0f : 0.0625f);
				num2 = ((i != 2) ? num2 : (-0.0625f));
				this.outlineSprites[i].transform.localPosition = (new Vector3(num, num2, 0f) * Pixelator.Instance.CurrentTileScale * 16f * GameUIRoot.Instance.PixelsToUnits()).WithZ(this.UI_OUTLINE_DEPTH);
			}
			this.outlineSprites[i].scale = this.itemSprite.scale;
		}
	}

	// Token: 0x06008C7E RID: 35966 RVA: 0x003ACCEC File Offset: 0x003AAEEC
	public Vector3 GetOffsetVectorForItem(PlayerItem newItem, bool isFlippingGun)
	{
		tk2dSpriteDefinition tk2dSpriteDefinition;
		if (this.m_cachedItemSpriteDefinition != null)
		{
			tk2dSpriteDefinition = this.m_cachedItemSpriteDefinition;
		}
		else
		{
			tk2dSpriteDefinition = newItem.sprite.Collection.spriteDefinitions[newItem.sprite.spriteId];
		}
		Vector3 vector = Vector3.Scale((-tk2dSpriteDefinition.GetBounds().min + -tk2dSpriteDefinition.GetBounds().extents).Quantize(0.0625f), this.itemSprite.scale);
		if (isFlippingGun)
		{
			vector += new Vector3(this.m_currentItemSpriteXOffset, 0f, this.m_currentItemSpriteZOffset);
		}
		return vector;
	}

	// Token: 0x06008C7F RID: 35967 RVA: 0x003ACD9C File Offset: 0x003AAF9C
	private void UpdateItemSprite(PlayerItem newItem, int itemShift)
	{
		tk2dSprite component = newItem.GetComponent<tk2dSprite>();
		if (newItem != this.m_cachedItem)
		{
			this.DoItemCardFlip(newItem, itemShift);
		}
		this.UpdateItemSpriteScale();
		if (!this.m_deferCurrentItemSwap)
		{
			if (!this.itemSprite.renderer.enabled)
			{
				this.ToggleRenderers(true);
			}
			if (this.itemSprite.spriteId != component.spriteId || this.itemSprite.Collection != component.Collection)
			{
				this.itemSprite.SetSprite(component.Collection, component.spriteId);
				for (int i = 0; i < this.outlineSprites.Length; i++)
				{
					this.outlineSprites[i].SetSprite(component.Collection, component.spriteId);
					SpriteOutlineManager.ForceUpdateOutlineMaterial(this.outlineSprites[i], component);
				}
			}
		}
		Vector3 center = this.ItemBoxSprite.GetCenter();
		this.itemSprite.transform.position = center + this.GetOffsetVectorForItem(newItem, this.m_isCurrentlyFlipping);
		this.itemSprite.transform.position = this.itemSprite.transform.position.Quantize(this.ItemBoxSprite.PixelsToUnits() * 3f);
		if (newItem.PreventCooldownBar || (!newItem.IsActive && !newItem.IsOnCooldown) || this.m_isCurrentlyFlipping)
		{
			this.ItemBoxFillSprite.IsVisible = false;
			this.ItemBoxFGSprite.IsVisible = false;
			this.ItemBoxSprite.SpriteName = "weapon_box_02";
		}
		else
		{
			this.ItemBoxFillSprite.IsVisible = true;
			this.ItemBoxFGSprite.IsVisible = true;
			this.ItemBoxSprite.SpriteName = "weapon_box_02_cd";
		}
		if (newItem.IsActive)
		{
			this.ItemBoxFillSprite.FillAmount = 1f - newItem.ActivePercentage;
		}
		else
		{
			this.ItemBoxFillSprite.FillAmount = 1f - newItem.CooldownPercentage;
		}
		PlayerController playerController = GameManager.Instance.PrimaryPlayer;
		if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && this.IsRightAligned)
		{
			playerController = GameManager.Instance.SecondaryPlayer;
		}
		if (newItem.IsOnCooldown || !newItem.CanBeUsed(playerController))
		{
			Color color = this.itemSpriteMaterial.GetColor("_OverrideColor");
			Color color2 = new Color(0f, 0f, 0f, 0.8f);
			if (color != color2)
			{
				this.itemSpriteMaterial.SetColor("_OverrideColor", color2);
				tk2dSprite[] array = SpriteOutlineManager.GetOutlineSprites(this.itemSprite);
				Color color3 = new Color(0.4f, 0.4f, 0.4f, 1f);
				for (int j = 0; j < array.Length; j++)
				{
					array[j].renderer.material.SetColor("_OverrideColor", color3);
				}
			}
		}
		else
		{
			Color color4 = this.itemSpriteMaterial.GetColor("_OverrideColor");
			Color color5 = new Color(0f, 0f, 0f, 0f);
			if (color4 != color5)
			{
				this.itemSpriteMaterial.SetColor("_OverrideColor", color5);
				tk2dSprite[] array2 = SpriteOutlineManager.GetOutlineSprites(this.itemSprite);
				Color white = Color.white;
				for (int k = 0; k < array2.Length; k++)
				{
					array2[k].renderer.material.SetColor("_OverrideColor", white);
				}
			}
		}
	}

	// Token: 0x06008C80 RID: 35968 RVA: 0x003AD130 File Offset: 0x003AB330
	public void TriggerUIDisabled()
	{
		if (GameUIRoot.Instance.ForceHideItemPanel)
		{
			this.itemSprite.renderer.enabled = false;
			for (int i = 0; i < this.outlineSprites.Length; i++)
			{
				this.outlineSprites[i].renderer.enabled = false;
			}
			this.ItemBoxSprite.IsVisible = false;
			this.ItemBoxFillSprite.IsVisible = false;
			this.ItemBoxFGSprite.IsVisible = false;
		}
	}

	// Token: 0x06008C81 RID: 35969 RVA: 0x003AD1B0 File Offset: 0x003AB3B0
	private void SetItemCountVisible(bool val)
	{
		this.ItemCountLabel.IsVisible = val;
	}

	// Token: 0x06008C82 RID: 35970 RVA: 0x003AD1C0 File Offset: 0x003AB3C0
	public void UpdateItem(PlayerItem current, List<PlayerItem> items)
	{
		if (!this.m_initialized)
		{
			this.Initialize();
		}
		if (GameUIRoot.Instance.ForceHideItemPanel || this.temporarilyPreventVisible)
		{
			this.ToggleRenderers(false);
			return;
		}
		if (items.Count != 0 && items.Count - 1 != this.AdditionalItemBoxSprites.Count && GameUIRoot.Instance.GunventoryFolded)
		{
			this.RebuildExtraItemCards(current, items);
		}
		if (current == null || GameUIRoot.Instance.ForceHideItemPanel || this.temporarilyPreventVisible)
		{
			if (this.ItemBoxSprite.IsVisible)
			{
				this.itemSprite.renderer.enabled = false;
				for (int i = 0; i < this.outlineSprites.Length; i++)
				{
					this.outlineSprites[i].renderer.enabled = false;
				}
				this.ItemBoxSprite.IsVisible = false;
				this.ItemBoxFillSprite.IsVisible = false;
				this.ItemBoxFGSprite.IsVisible = false;
			}
			this.SetItemCountVisible(false);
		}
		else
		{
			if ((!this.ItemBoxSprite.IsVisible || !this.itemSprite.renderer.enabled) && !this.m_isCurrentlyFlipping && !this.m_deferCurrentItemSwap)
			{
				this.itemSprite.renderer.enabled = true;
				for (int j = 0; j < this.outlineSprites.Length; j++)
				{
					this.outlineSprites[j].renderer.enabled = true;
				}
				this.ItemBoxSprite.IsVisible = true;
			}
			bool flag = (current.canStack && current.numberOfUses > 1 && current.consumable) || (current.numberOfUses > 1 && current.UsesNumberOfUsesBeforeCooldown && !current.IsOnCooldown);
			if (flag)
			{
				this.SetItemCountVisible(true);
				this.ItemCountLabel.Text = current.numberOfUses.ToString();
			}
			else if (current is EstusFlaskItem)
			{
				EstusFlaskItem estusFlaskItem = current as EstusFlaskItem;
				this.SetItemCountVisible(true);
				this.ItemCountLabel.Text = estusFlaskItem.RemainingDrinks.ToString();
			}
			else if (current is RatPackItem && !current.IsOnCooldown)
			{
				RatPackItem ratPackItem = current as RatPackItem;
				this.SetItemCountVisible(true);
				this.ItemCountLabel.Text = ratPackItem.ContainedBullets.ToString();
			}
			else
			{
				this.SetItemCountVisible(false);
			}
			int num = 0;
			if (current != this.m_cachedItem && items.Contains(this.m_cachedItem))
			{
				int num2 = items.IndexOf(this.m_cachedItem);
				int num3 = items.IndexOf(current);
				if (items.Count == 2)
				{
					num = -1;
				}
				else if (num3 == 0 && num2 == items.Count - 1)
				{
					num = 1;
				}
				else if (num3 == items.Count - 1 && num2 == 0)
				{
					num = -1;
				}
				else
				{
					num = num3 - num2;
				}
			}
			else if (current != this.m_cachedItem)
			{
				num = -1;
			}
			this.UpdateItemSprite(current, num);
		}
		if (this.itemSprite.renderer.enabled && !this.ItemBoxSprite.IsVisible)
		{
			this.ToggleRenderers(true);
		}
		this.m_cachedItem = current;
	}

	// Token: 0x040093C2 RID: 37826
	public dfSprite ItemBoxSprite;

	// Token: 0x040093C3 RID: 37827
	public dfSprite ItemBoxFillSprite;

	// Token: 0x040093C4 RID: 37828
	public dfSprite ItemBoxFGSprite;

	// Token: 0x040093C5 RID: 37829
	public tk2dClippedSprite itemSprite;

	// Token: 0x040093C6 RID: 37830
	public GameObject ExtraItemCardPrefab;

	// Token: 0x040093C7 RID: 37831
	public List<dfControl> AdditionalItemBoxSprites = new List<dfControl>();

	// Token: 0x040093C8 RID: 37832
	public dfLabel ItemCountLabel;

	// Token: 0x040093C9 RID: 37833
	[NonSerialized]
	public bool temporarilyPreventVisible;

	// Token: 0x040093CA RID: 37834
	public bool IsRightAligned;

	// Token: 0x040093CB RID: 37835
	private Material itemSpriteMaterial;

	// Token: 0x040093CC RID: 37836
	private tk2dSprite[] outlineSprites;

	// Token: 0x040093CD RID: 37837
	private PlayerItem m_cachedItem;

	// Token: 0x040093CE RID: 37838
	private bool m_initialized;

	// Token: 0x040093CF RID: 37839
	private Material m_ClippedMaterial;

	// Token: 0x040093D0 RID: 37840
	private Material m_ClippedZWriteOffMaterial;

	// Token: 0x040093D1 RID: 37841
	private dfPanel m_panel;

	// Token: 0x040093D2 RID: 37842
	private float UI_OUTLINE_DEPTH = 1f;

	// Token: 0x040093D3 RID: 37843
	private tk2dSpriteDefinition m_cachedItemSpriteDefinition;

	// Token: 0x040093D4 RID: 37844
	private bool m_isCurrentlyFlipping;

	// Token: 0x040093D5 RID: 37845
	private float m_currentItemSpriteXOffset;

	// Token: 0x040093D6 RID: 37846
	private float m_currentItemSpriteZOffset;

	// Token: 0x040093D7 RID: 37847
	private bool m_deferCurrentItemSwap;

	// Token: 0x040093D8 RID: 37848
	private bool m_cardFlippedQueued;

	// Token: 0x040093D9 RID: 37849
	private const float FLIP_TIME = 0.15f;
}
