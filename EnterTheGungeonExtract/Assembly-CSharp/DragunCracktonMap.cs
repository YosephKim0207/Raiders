using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000E65 RID: 3685
public class DragunCracktonMap : MonoBehaviour
{
	// Token: 0x06004E66 RID: 20070 RVA: 0x001B173C File Offset: 0x001AF93C
	public void Start()
	{
		this.m_crackSpriteNames = new List<string>(this.crackSprites.Count);
		for (int i = 0; i < this.crackSprites.Count; i++)
		{
			if (this.crackSprites[i])
			{
				this.m_crackSpriteNames.Add(this.crackSprites[i].name);
			}
		}
		tk2dSprite[] componentsInChildren = base.GetComponentsInChildren<tk2dSprite>(true);
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].GenerateUV2 = true;
			componentsInChildren[j].usesOverrideMaterial = true;
			componentsInChildren[j].SpriteChanged += this.HandleCracktonChanged;
			componentsInChildren[j].ForceBuild();
			this.HandleCracktonChanged(componentsInChildren[j]);
		}
	}

	// Token: 0x06004E67 RID: 20071 RVA: 0x001B1800 File Offset: 0x001AFA00
	public void ConvertToCrackton()
	{
		base.StartCoroutine(this.HandleAmbient());
		base.StartCoroutine(this.HandleConversion());
	}

	// Token: 0x06004E68 RID: 20072 RVA: 0x001B181C File Offset: 0x001AFA1C
	private IEnumerator HandleAmbient()
	{
		float elapsed = 0f;
		float ambientChangeTime = 2f;
		Color startColor = RenderSettings.ambientLight;
		RoomHandler parentRoom = base.transform.position.GetAbsoluteRoom();
		while (elapsed < ambientChangeTime)
		{
			elapsed += BraveTime.DeltaTime;
			float t = elapsed / ambientChangeTime;
			parentRoom.area.runtimePrototypeData.usesCustomAmbient = true;
			parentRoom.area.runtimePrototypeData.customAmbient = new Color(0.92f, 0.92f, 0.92f);
			parentRoom.area.runtimePrototypeData.customAmbientLowQuality = new Color(0.95f, 0.95f, 0.95f);
			RenderSettings.ambientLight = Color.Lerp(startColor, new Color(0.92f, 0.92f, 0.92f), t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004E69 RID: 20073 RVA: 0x001B1838 File Offset: 0x001AFA38
	private IEnumerator HandleConversion()
	{
		float elapsed = 0f;
		float charTime = 1.7f;
		float crackTime = 0.7f;
		tk2dSprite[] childSprites = base.GetComponentsInChildren<tk2dSprite>(true);
		while (elapsed < charTime + crackTime)
		{
			elapsed += BraveTime.DeltaTime;
			float charT = Mathf.Clamp01(elapsed / charTime);
			float crackT = Mathf.Clamp01((elapsed - charTime) / crackTime);
			foreach (tk2dSprite tk2dSprite in childSprites)
			{
				if (tk2dSprite)
				{
					tk2dSprite.renderer.material.SetFloat("_CharAmount", charT);
					tk2dSprite.renderer.material.SetFloat("_CrackAmount", crackT);
				}
			}
			yield return null;
			if (!this)
			{
				yield break;
			}
		}
		yield break;
	}

	// Token: 0x06004E6A RID: 20074 RVA: 0x001B1854 File Offset: 0x001AFA54
	public void PreGold()
	{
		foreach (tk2dSprite tk2dSprite in base.GetComponentsInChildren<tk2dSprite>(true))
		{
			if (tk2dSprite)
			{
				tk2dSprite.renderer.material.SetFloat("_CharAmount", 1f);
				tk2dSprite.renderer.material.SetFloat("_CrackAmount", 1f);
			}
		}
	}

	// Token: 0x06004E6B RID: 20075 RVA: 0x001B18C8 File Offset: 0x001AFAC8
	public void ConvertToGold()
	{
		base.StartCoroutine(this.HandleGoldAmbient());
		base.StartCoroutine(this.HandleGoldConversion());
	}

	// Token: 0x06004E6C RID: 20076 RVA: 0x001B18E4 File Offset: 0x001AFAE4
	private IEnumerator HandleGoldAmbient()
	{
		float elapsed = 0f;
		float ambientChangeTime = 2f;
		Color startColor = RenderSettings.ambientLight;
		RoomHandler parentRoom = base.transform.position.GetAbsoluteRoom();
		while (elapsed < ambientChangeTime)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float t = elapsed / ambientChangeTime;
			parentRoom.area.runtimePrototypeData.usesCustomAmbient = true;
			parentRoom.area.runtimePrototypeData.customAmbient = new Color(1f, 0.78f, 0.6f);
			parentRoom.area.runtimePrototypeData.customAmbientLowQuality = new Color(1f, 0.82f, 0.64f);
			RenderSettings.ambientLight = Color.Lerp(startColor, new Color(1f, 0.78f, 0.6f), t);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06004E6D RID: 20077 RVA: 0x001B1900 File Offset: 0x001AFB00
	private IEnumerator HandleGoldConversion()
	{
		float elapsed = 0f;
		float crackTime = 3.83f;
		tk2dSprite[] childSprites = base.GetComponentsInChildren<tk2dSprite>(true);
		while (elapsed < crackTime)
		{
			elapsed += GameManager.INVARIANT_DELTA_TIME;
			float crackT = 1f - Mathf.Clamp01(elapsed / crackTime);
			foreach (tk2dSprite tk2dSprite in childSprites)
			{
				if (tk2dSprite)
				{
					tk2dSprite.renderer.material.SetFloat("_CrackAmount", crackT);
				}
			}
			yield return null;
			if (!this)
			{
				yield break;
			}
		}
		foreach (tk2dSprite tk2dSprite2 in childSprites)
		{
			if (tk2dSprite2)
			{
				tk2dSprite2.renderer.material.SetFloat("_CharAmount", 0f);
				tk2dSprite2.renderer.material.SetFloat("_CrackAmount", 0f);
			}
		}
		yield break;
	}

	// Token: 0x06004E6E RID: 20078 RVA: 0x001B191C File Offset: 0x001AFB1C
	private void HandleCracktonChanged(tk2dBaseSprite obj)
	{
		tk2dSpriteCollectionData collection = obj.Collection;
		int spriteId = obj.spriteId;
		Dictionary<int, Texture> dictionary;
		if (!this.m_cracktonMap.TryGetValue(collection, out dictionary))
		{
			dictionary = new Dictionary<int, Texture>();
			this.m_cracktonMap.Add(collection, dictionary);
		}
		Texture texture;
		if (dictionary.TryGetValue(spriteId, out texture))
		{
			if (texture != null)
			{
				obj.renderer.material.SetTexture("_CracksTex", texture);
			}
			return;
		}
		string text = obj.GetCurrentSpriteDef().name;
		string text2 = text.Insert(text.Length - 4, "_crackton");
		int num = this.m_crackSpriteNames.IndexOf(text2);
		if (num >= 0)
		{
			dictionary.Add(spriteId, this.crackSprites[num]);
			obj.renderer.material.SetTexture("_CracksTex", this.crackSprites[num]);
			return;
		}
		text2 = text.Substring(0, text.Length - 4) + "_crackton_001";
		num = this.m_crackSpriteNames.IndexOf(text2);
		if (num >= 0)
		{
			dictionary.Add(spriteId, this.crackSprites[num]);
			obj.renderer.material.SetTexture("_CracksTex", this.crackSprites[num]);
			return;
		}
		if (text.Length > 12)
		{
			text = obj.GetCurrentSpriteDef().name;
			text2 = text.Insert(11, "_crack");
			num = this.m_crackSpriteNames.IndexOf(text2);
			if (num >= 0)
			{
				dictionary.Add(spriteId, this.crackSprites[num]);
				obj.renderer.material.SetTexture("_CracksTex", this.crackSprites[num]);
				return;
			}
		}
		dictionary.Add(spriteId, null);
	}

	// Token: 0x040044C3 RID: 17603
	[SerializeField]
	public List<Texture> crackSprites;

	// Token: 0x040044C4 RID: 17604
	private List<string> m_crackSpriteNames;

	// Token: 0x040044C5 RID: 17605
	private Dictionary<tk2dSpriteCollectionData, Dictionary<int, Texture>> m_cracktonMap = new Dictionary<tk2dSpriteCollectionData, Dictionary<int, Texture>>();
}
