using System;
using UnityEngine;

namespace InControl
{
	// Token: 0x0200076E RID: 1902
	[Serializable]
	public class TouchSprite
	{
		// Token: 0x06002A81 RID: 10881 RVA: 0x000C13A0 File Offset: 0x000BF5A0
		public TouchSprite()
		{
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000C1410 File Offset: 0x000BF610
		public TouchSprite(float size)
		{
			this.size = Vector2.one * size;
		}

		// Token: 0x1700081A RID: 2074
		// (get) Token: 0x06002A83 RID: 10883 RVA: 0x000C1490 File Offset: 0x000BF690
		// (set) Token: 0x06002A84 RID: 10884 RVA: 0x000C1498 File Offset: 0x000BF698
		public bool Dirty { get; set; }

		// Token: 0x1700081B RID: 2075
		// (get) Token: 0x06002A85 RID: 10885 RVA: 0x000C14A4 File Offset: 0x000BF6A4
		// (set) Token: 0x06002A86 RID: 10886 RVA: 0x000C14AC File Offset: 0x000BF6AC
		public bool Ready { get; set; }

		// Token: 0x06002A87 RID: 10887 RVA: 0x000C14B8 File Offset: 0x000BF6B8
		public void Create(string gameObjectName, Transform parentTransform, int sortingOrder)
		{
			this.spriteGameObject = this.CreateSpriteGameObject(gameObjectName, parentTransform);
			this.spriteRenderer = this.CreateSpriteRenderer(this.spriteGameObject, this.idleSprite, sortingOrder);
			this.spriteRenderer.color = this.idleColor;
			this.Ready = true;
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000C1504 File Offset: 0x000BF704
		public void Delete()
		{
			this.Ready = false;
			UnityEngine.Object.Destroy(this.spriteGameObject);
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000C1518 File Offset: 0x000BF718
		public void Update()
		{
			this.Update(false);
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000C1524 File Offset: 0x000BF724
		public void Update(bool forceUpdate)
		{
			if (this.Dirty || forceUpdate)
			{
				if (this.spriteRenderer != null)
				{
					this.spriteRenderer.sprite = ((!this.State) ? this.idleSprite : this.busySprite);
				}
				if (this.sizeUnitType == TouchUnitType.Pixels)
				{
					Vector2 vector = TouchUtility.RoundVector(this.size);
					this.ScaleSpriteInPixels(this.spriteGameObject, this.spriteRenderer, vector);
					this.worldSize = vector * TouchManager.PixelToWorld;
				}
				else
				{
					this.ScaleSpriteInPercent(this.spriteGameObject, this.spriteRenderer, this.size);
					if (this.lockAspectRatio)
					{
						this.worldSize = this.size * TouchManager.PercentToWorld;
					}
					else
					{
						this.worldSize = Vector2.Scale(this.size, TouchManager.ViewSize);
					}
				}
				this.Dirty = false;
			}
			if (this.spriteRenderer != null)
			{
				Color color = ((!this.State) ? this.idleColor : this.busyColor);
				if (this.spriteRenderer.color != color)
				{
					this.spriteRenderer.color = Utility.MoveColorTowards(this.spriteRenderer.color, color, 5f * Time.deltaTime);
				}
			}
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000C1688 File Offset: 0x000BF888
		private GameObject CreateSpriteGameObject(string name, Transform parentTransform)
		{
			return new GameObject(name)
			{
				transform = 
				{
					parent = parentTransform,
					localPosition = Vector3.zero,
					localScale = Vector3.one
				},
				layer = parentTransform.gameObject.layer
			};
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000C16DC File Offset: 0x000BF8DC
		private SpriteRenderer CreateSpriteRenderer(GameObject spriteGameObject, Sprite sprite, int sortingOrder)
		{
			SpriteRenderer spriteRenderer = spriteGameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = sprite;
			spriteRenderer.sortingOrder = sortingOrder;
			spriteRenderer.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
			spriteRenderer.sharedMaterial.SetFloat("PixelSnap", 1f);
			return spriteRenderer;
		}

		// Token: 0x06002A8D RID: 10893 RVA: 0x000C172C File Offset: 0x000BF92C
		private void ScaleSpriteInPixels(GameObject spriteGameObject, SpriteRenderer spriteRenderer, Vector2 size)
		{
			if (spriteGameObject == null || spriteRenderer == null || spriteRenderer.sprite == null)
			{
				return;
			}
			float num = spriteRenderer.sprite.rect.width / spriteRenderer.sprite.bounds.size.x;
			float num2 = TouchManager.PixelToWorld * num;
			float num3 = num2 * size.x / spriteRenderer.sprite.rect.width;
			float num4 = num2 * size.y / spriteRenderer.sprite.rect.height;
			spriteGameObject.transform.localScale = new Vector3(num3, num4);
		}

		// Token: 0x06002A8E RID: 10894 RVA: 0x000C17F4 File Offset: 0x000BF9F4
		private void ScaleSpriteInPercent(GameObject spriteGameObject, SpriteRenderer spriteRenderer, Vector2 size)
		{
			if (spriteGameObject == null || spriteRenderer == null || spriteRenderer.sprite == null)
			{
				return;
			}
			if (this.lockAspectRatio)
			{
				float num = Mathf.Min(TouchManager.ViewSize.x, TouchManager.ViewSize.y);
				float num2 = num * size.x / spriteRenderer.sprite.bounds.size.x;
				float num3 = num * size.y / spriteRenderer.sprite.bounds.size.y;
				spriteGameObject.transform.localScale = new Vector3(num2, num3);
			}
			else
			{
				float num4 = TouchManager.ViewSize.x * size.x / spriteRenderer.sprite.bounds.size.x;
				float num5 = TouchManager.ViewSize.y * size.y / spriteRenderer.sprite.bounds.size.y;
				spriteGameObject.transform.localScale = new Vector3(num4, num5);
			}
		}

		// Token: 0x06002A8F RID: 10895 RVA: 0x000C1940 File Offset: 0x000BFB40
		public bool Contains(Vector2 testWorldPoint)
		{
			if (this.shape == TouchSpriteShape.Oval)
			{
				float num = (testWorldPoint.x - this.Position.x) / this.worldSize.x;
				float num2 = (testWorldPoint.y - this.Position.y) / this.worldSize.y;
				return num * num + num2 * num2 < 0.25f;
			}
			float num3 = Utility.Abs(testWorldPoint.x - this.Position.x) * 2f;
			float num4 = Utility.Abs(testWorldPoint.y - this.Position.y) * 2f;
			return num3 <= this.worldSize.x && num4 <= this.worldSize.y;
		}

		// Token: 0x06002A90 RID: 10896 RVA: 0x000C1A1C File Offset: 0x000BFC1C
		public bool Contains(Touch touch)
		{
			return this.Contains(TouchManager.ScreenToWorldPoint(touch.position));
		}

		// Token: 0x06002A91 RID: 10897 RVA: 0x000C1A34 File Offset: 0x000BFC34
		public void DrawGizmos(Vector3 position, Color color)
		{
			if (this.shape == TouchSpriteShape.Oval)
			{
				Utility.DrawOvalGizmo(position, this.WorldSize, color);
			}
			else
			{
				Utility.DrawRectGizmo(position, this.WorldSize, color);
			}
		}

		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x06002A92 RID: 10898 RVA: 0x000C1A6C File Offset: 0x000BFC6C
		// (set) Token: 0x06002A93 RID: 10899 RVA: 0x000C1A74 File Offset: 0x000BFC74
		public bool State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state != value)
				{
					this.state = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06002A94 RID: 10900 RVA: 0x000C1A90 File Offset: 0x000BFC90
		// (set) Token: 0x06002A95 RID: 10901 RVA: 0x000C1A98 File Offset: 0x000BFC98
		public Sprite BusySprite
		{
			get
			{
				return this.busySprite;
			}
			set
			{
				if (this.busySprite != value)
				{
					this.busySprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06002A96 RID: 10902 RVA: 0x000C1ABC File Offset: 0x000BFCBC
		// (set) Token: 0x06002A97 RID: 10903 RVA: 0x000C1AC4 File Offset: 0x000BFCC4
		public Sprite IdleSprite
		{
			get
			{
				return this.idleSprite;
			}
			set
			{
				if (this.idleSprite != value)
				{
					this.idleSprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x1700081F RID: 2079
		// (set) Token: 0x06002A98 RID: 10904 RVA: 0x000C1AE8 File Offset: 0x000BFCE8
		public Sprite Sprite
		{
			set
			{
				if (this.idleSprite != value)
				{
					this.idleSprite = value;
					this.Dirty = true;
				}
				if (this.busySprite != value)
				{
					this.busySprite = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06002A99 RID: 10905 RVA: 0x000C1B28 File Offset: 0x000BFD28
		// (set) Token: 0x06002A9A RID: 10906 RVA: 0x000C1B30 File Offset: 0x000BFD30
		public Color BusyColor
		{
			get
			{
				return this.busyColor;
			}
			set
			{
				if (this.busyColor != value)
				{
					this.busyColor = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06002A9B RID: 10907 RVA: 0x000C1B54 File Offset: 0x000BFD54
		// (set) Token: 0x06002A9C RID: 10908 RVA: 0x000C1B5C File Offset: 0x000BFD5C
		public Color IdleColor
		{
			get
			{
				return this.idleColor;
			}
			set
			{
				if (this.idleColor != value)
				{
					this.idleColor = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06002A9D RID: 10909 RVA: 0x000C1B80 File Offset: 0x000BFD80
		// (set) Token: 0x06002A9E RID: 10910 RVA: 0x000C1B88 File Offset: 0x000BFD88
		public TouchSpriteShape Shape
		{
			get
			{
				return this.shape;
			}
			set
			{
				if (this.shape != value)
				{
					this.shape = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x06002A9F RID: 10911 RVA: 0x000C1BA4 File Offset: 0x000BFDA4
		// (set) Token: 0x06002AA0 RID: 10912 RVA: 0x000C1BAC File Offset: 0x000BFDAC
		public TouchUnitType SizeUnitType
		{
			get
			{
				return this.sizeUnitType;
			}
			set
			{
				if (this.sizeUnitType != value)
				{
					this.sizeUnitType = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000824 RID: 2084
		// (get) Token: 0x06002AA1 RID: 10913 RVA: 0x000C1BC8 File Offset: 0x000BFDC8
		// (set) Token: 0x06002AA2 RID: 10914 RVA: 0x000C1BD0 File Offset: 0x000BFDD0
		public Vector2 Size
		{
			get
			{
				return this.size;
			}
			set
			{
				if (this.size != value)
				{
					this.size = value;
					this.Dirty = true;
				}
			}
		}

		// Token: 0x17000825 RID: 2085
		// (get) Token: 0x06002AA3 RID: 10915 RVA: 0x000C1BF4 File Offset: 0x000BFDF4
		public Vector2 WorldSize
		{
			get
			{
				return this.worldSize;
			}
		}

		// Token: 0x17000826 RID: 2086
		// (get) Token: 0x06002AA4 RID: 10916 RVA: 0x000C1BFC File Offset: 0x000BFDFC
		// (set) Token: 0x06002AA5 RID: 10917 RVA: 0x000C1C28 File Offset: 0x000BFE28
		public Vector3 Position
		{
			get
			{
				return (!this.spriteGameObject) ? Vector3.zero : this.spriteGameObject.transform.position;
			}
			set
			{
				if (this.spriteGameObject)
				{
					this.spriteGameObject.transform.position = value;
				}
			}
		}

		// Token: 0x04001D6A RID: 7530
		[SerializeField]
		private Sprite idleSprite;

		// Token: 0x04001D6B RID: 7531
		[SerializeField]
		private Sprite busySprite;

		// Token: 0x04001D6C RID: 7532
		[SerializeField]
		private Color idleColor = new Color(1f, 1f, 1f, 0.5f);

		// Token: 0x04001D6D RID: 7533
		[SerializeField]
		private Color busyColor = new Color(1f, 1f, 1f, 1f);

		// Token: 0x04001D6E RID: 7534
		[SerializeField]
		private TouchSpriteShape shape;

		// Token: 0x04001D6F RID: 7535
		[SerializeField]
		private TouchUnitType sizeUnitType;

		// Token: 0x04001D70 RID: 7536
		[SerializeField]
		private Vector2 size = new Vector2(10f, 10f);

		// Token: 0x04001D71 RID: 7537
		[SerializeField]
		private bool lockAspectRatio = true;

		// Token: 0x04001D72 RID: 7538
		[SerializeField]
		[HideInInspector]
		private Vector2 worldSize;

		// Token: 0x04001D73 RID: 7539
		private Transform spriteParentTransform;

		// Token: 0x04001D74 RID: 7540
		private GameObject spriteGameObject;

		// Token: 0x04001D75 RID: 7541
		private SpriteRenderer spriteRenderer;

		// Token: 0x04001D76 RID: 7542
		private bool state;
	}
}
