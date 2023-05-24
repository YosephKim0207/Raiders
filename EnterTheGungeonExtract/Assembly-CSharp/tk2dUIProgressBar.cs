using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000C09 RID: 3081
[AddComponentMenu("2D Toolkit/UI/tk2dUIProgressBar")]
public class tk2dUIProgressBar : MonoBehaviour
{
	// Token: 0x1400008B RID: 139
	// (add) Token: 0x060041AA RID: 16810 RVA: 0x00153A18 File Offset: 0x00151C18
	// (remove) Token: 0x060041AB RID: 16811 RVA: 0x00153A50 File Offset: 0x00151C50
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnProgressComplete;

	// Token: 0x060041AC RID: 16812 RVA: 0x00153A88 File Offset: 0x00151C88
	private void Start()
	{
		this.InitializeSlicedSpriteDimensions();
		this.Value = this.percent;
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x060041AD RID: 16813 RVA: 0x00153A9C File Offset: 0x00151C9C
	// (set) Token: 0x060041AE RID: 16814 RVA: 0x00153AA4 File Offset: 0x00151CA4
	public float Value
	{
		get
		{
			return this.percent;
		}
		set
		{
			this.percent = Mathf.Clamp(value, 0f, 1f);
			if (Application.isPlaying)
			{
				if (this.clippedSpriteBar != null)
				{
					this.clippedSpriteBar.clipTopRight = new Vector2(this.Value, 1f);
				}
				else if (this.scalableBar != null)
				{
					this.scalableBar.localScale = new Vector3(this.Value, this.scalableBar.localScale.y, this.scalableBar.localScale.z);
				}
				else if (this.slicedSpriteBar != null)
				{
					this.InitializeSlicedSpriteDimensions();
					float num = Mathf.Lerp(this.emptySlicedSpriteDimensions.x, this.fullSlicedSpriteDimensions.x, this.Value);
					this.currentDimensions.Set(num, this.fullSlicedSpriteDimensions.y);
					this.slicedSpriteBar.dimensions = this.currentDimensions;
				}
				if (!this.isProgressComplete && this.Value == 1f)
				{
					this.isProgressComplete = true;
					if (this.OnProgressComplete != null)
					{
						this.OnProgressComplete();
					}
					if (this.sendMessageTarget != null && this.SendMessageOnProgressCompleteMethodName.Length > 0)
					{
						this.sendMessageTarget.SendMessage(this.SendMessageOnProgressCompleteMethodName, this, SendMessageOptions.RequireReceiver);
					}
				}
				else if (this.isProgressComplete && this.Value < 1f)
				{
					this.isProgressComplete = false;
				}
			}
		}
	}

	// Token: 0x060041AF RID: 16815 RVA: 0x00153C4C File Offset: 0x00151E4C
	private void InitializeSlicedSpriteDimensions()
	{
		if (!this.initializedSlicedSpriteDimensions)
		{
			if (this.slicedSpriteBar != null)
			{
				tk2dSpriteDefinition currentSprite = this.slicedSpriteBar.CurrentSprite;
				Vector3 boundsDataExtents = currentSprite.boundsDataExtents;
				this.fullSlicedSpriteDimensions = this.slicedSpriteBar.dimensions;
				this.emptySlicedSpriteDimensions.Set((this.slicedSpriteBar.borderLeft + this.slicedSpriteBar.borderRight) * boundsDataExtents.x / currentSprite.texelSize.x, this.fullSlicedSpriteDimensions.y);
			}
			this.initializedSlicedSpriteDimensions = true;
		}
	}

	// Token: 0x0400343C RID: 13372
	public Transform scalableBar;

	// Token: 0x0400343D RID: 13373
	public tk2dClippedSprite clippedSpriteBar;

	// Token: 0x0400343E RID: 13374
	public tk2dSlicedSprite slicedSpriteBar;

	// Token: 0x0400343F RID: 13375
	private bool initializedSlicedSpriteDimensions;

	// Token: 0x04003440 RID: 13376
	private Vector2 emptySlicedSpriteDimensions = Vector2.zero;

	// Token: 0x04003441 RID: 13377
	private Vector2 fullSlicedSpriteDimensions = Vector2.zero;

	// Token: 0x04003442 RID: 13378
	private Vector2 currentDimensions = Vector2.zero;

	// Token: 0x04003443 RID: 13379
	[SerializeField]
	private float percent;

	// Token: 0x04003444 RID: 13380
	private bool isProgressComplete;

	// Token: 0x04003445 RID: 13381
	public GameObject sendMessageTarget;

	// Token: 0x04003446 RID: 13382
	public string SendMessageOnProgressCompleteMethodName = string.Empty;
}
