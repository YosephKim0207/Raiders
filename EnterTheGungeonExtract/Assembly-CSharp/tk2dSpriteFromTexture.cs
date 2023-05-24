using System;
using tk2dRuntime;
using UnityEngine;

// Token: 0x02000BDE RID: 3038
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteFromTexture")]
public class tk2dSpriteFromTexture : MonoBehaviour
{
	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x06004045 RID: 16453 RVA: 0x001465E4 File Offset: 0x001447E4
	private tk2dBaseSprite Sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<tk2dBaseSprite>();
				if (this._sprite == null)
				{
					Debug.Log("tk2dSpriteFromTexture - Missing sprite object. Creating.");
					this._sprite = base.gameObject.AddComponent<tk2dSprite>();
				}
			}
			return this._sprite;
		}
	}

	// Token: 0x06004046 RID: 16454 RVA: 0x00146640 File Offset: 0x00144840
	private void Awake()
	{
		this.Create(this.spriteCollectionSize, this.texture, this.anchor);
	}

	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x06004047 RID: 16455 RVA: 0x0014665C File Offset: 0x0014485C
	public bool HasSpriteCollection
	{
		get
		{
			return this.spriteCollection != null;
		}
	}

	// Token: 0x06004048 RID: 16456 RVA: 0x0014666C File Offset: 0x0014486C
	private void OnDestroy()
	{
		this.DestroyInternal();
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().material = null;
		}
	}

	// Token: 0x06004049 RID: 16457 RVA: 0x00146694 File Offset: 0x00144894
	public void Create(tk2dSpriteCollectionSize spriteCollectionSize, Texture texture, tk2dBaseSprite.Anchor anchor)
	{
		this.DestroyInternal();
		if (texture != null)
		{
			this.spriteCollectionSize.CopyFrom(spriteCollectionSize);
			this.texture = texture;
			this.anchor = anchor;
			GameObject gameObject = new GameObject("tk2dSpriteFromTexture - " + texture.name);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject.hideFlags = HideFlags.DontSave;
			Vector2 anchorOffset = tk2dSpriteGeomGen.GetAnchorOffset(anchor, (float)texture.width, (float)texture.height);
			this.spriteCollection = SpriteCollectionGenerator.CreateFromTexture(gameObject, texture, spriteCollectionSize, new Vector2((float)texture.width, (float)texture.height), new string[] { "unnamed" }, new Rect[]
			{
				new Rect(0f, 0f, (float)texture.width, (float)texture.height)
			}, null, new Vector2[] { anchorOffset }, new bool[1], this.CustomShaderResource);
			string text = "SpriteFromTexture " + texture.name;
			this.spriteCollection.spriteCollectionName = text;
			this.spriteCollection.spriteDefinitions[0].material.name = text;
			this.spriteCollection.spriteDefinitions[0].material.hideFlags = HideFlags.HideInInspector | HideFlags.DontSaveInEditor | HideFlags.DontSaveInBuild | HideFlags.DontUnloadUnusedAsset;
			this.Sprite.SetSprite(this.spriteCollection, 0);
		}
	}

	// Token: 0x0600404A RID: 16458 RVA: 0x00146810 File Offset: 0x00144A10
	public void Clear()
	{
		this.DestroyInternal();
	}

	// Token: 0x0600404B RID: 16459 RVA: 0x00146818 File Offset: 0x00144A18
	public void ForceBuild()
	{
		this.DestroyInternal();
		this.Create(this.spriteCollectionSize, this.texture, this.anchor);
	}

	// Token: 0x0600404C RID: 16460 RVA: 0x00146838 File Offset: 0x00144A38
	private void DestroyInternal()
	{
		if (this.spriteCollection != null)
		{
			if (this.spriteCollection.spriteDefinitions[0].material != null)
			{
				UnityEngine.Object.DestroyImmediate(this.spriteCollection.spriteDefinitions[0].material);
			}
			UnityEngine.Object.DestroyImmediate(this.spriteCollection.gameObject);
			this.spriteCollection = null;
		}
	}

	// Token: 0x04003342 RID: 13122
	public Texture texture;

	// Token: 0x04003343 RID: 13123
	public tk2dSpriteCollectionSize spriteCollectionSize = new tk2dSpriteCollectionSize();

	// Token: 0x04003344 RID: 13124
	public tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.MiddleCenter;

	// Token: 0x04003345 RID: 13125
	public string CustomShaderResource;

	// Token: 0x04003346 RID: 13126
	private tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04003347 RID: 13127
	private tk2dBaseSprite _sprite;
}
