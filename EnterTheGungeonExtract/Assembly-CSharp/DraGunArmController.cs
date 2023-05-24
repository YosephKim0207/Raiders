using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001012 RID: 4114
public class DraGunArmController : BraveBehaviour
{
	// Token: 0x06005A07 RID: 23047 RVA: 0x00226760 File Offset: 0x00224960
	public void Start()
	{
		this.m_body = base.transform.parent.GetComponent<DraGunController>();
		this.m_body.specRigidbody.Initialize();
		float unitBottom = this.m_body.specRigidbody.PrimaryPixelCollider.UnitBottom;
		this.armSpriteClippers = new List<TileSpriteClipper>(this.balls.Count);
		for (int i = 0; i < this.balls.Count; i++)
		{
			tk2dBaseSprite componentInChildren = this.balls[i].GetComponentInChildren<tk2dBaseSprite>();
			TileSpriteClipper orAddComponent = componentInChildren.gameObject.GetOrAddComponent<TileSpriteClipper>();
			orAddComponent.doOptimize = true;
			orAddComponent.updateEveryFrame = true;
			orAddComponent.clipMode = TileSpriteClipper.ClipMode.ClipBelowY;
			orAddComponent.clipY = unitBottom;
			this.armSpriteClippers.Add(orAddComponent);
		}
		tk2dBaseSprite componentInChildren2 = this.hand.GetComponentInChildren<tk2dBaseSprite>();
		this.handSpriteClipper = componentInChildren2.gameObject.GetOrAddComponent<TileSpriteClipper>();
		this.handSpriteClipper.doOptimize = true;
		this.handSpriteClipper.updateEveryFrame = true;
		this.handSpriteClipper.clipMode = TileSpriteClipper.ClipMode.ClipBelowY;
		this.handSpriteClipper.clipY = unitBottom;
		this.handSpriteClipper.enabled = false;
		this.shoulderSprite = this.shoulder.GetComponentInChildren<tk2dBaseSprite>();
		this.m_body.sprite.SpriteChanged += this.BodySpriteChanged;
	}

	// Token: 0x06005A08 RID: 23048 RVA: 0x002268AC File Offset: 0x00224AAC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005A09 RID: 23049 RVA: 0x002268B4 File Offset: 0x00224AB4
	public void ClipArmSprites()
	{
		this.SetClipArmSprites(true);
	}

	// Token: 0x06005A0A RID: 23050 RVA: 0x002268C0 File Offset: 0x00224AC0
	public void UnclipArmSprites()
	{
		this.SetClipArmSprites(false);
	}

	// Token: 0x06005A0B RID: 23051 RVA: 0x002268CC File Offset: 0x00224ACC
	public void SetClipArmSprites(bool clip)
	{
		for (int i = 0; i < this.armSpriteClippers.Count; i++)
		{
			this.armSpriteClippers[i].enabled = clip;
		}
	}

	// Token: 0x06005A0C RID: 23052 RVA: 0x00226908 File Offset: 0x00224B08
	public void ClipHandSprite()
	{
		this.SetClipHandSprite(true);
	}

	// Token: 0x06005A0D RID: 23053 RVA: 0x00226914 File Offset: 0x00224B14
	public void UnclipHandSprite()
	{
		this.SetClipHandSprite(false);
	}

	// Token: 0x06005A0E RID: 23054 RVA: 0x00226920 File Offset: 0x00224B20
	public void SetClipHandSprite(bool clip)
	{
		this.handSpriteClipper.enabled = clip;
	}

	// Token: 0x06005A0F RID: 23055 RVA: 0x00226930 File Offset: 0x00224B30
	private void BodySpriteChanged(tk2dBaseSprite obj)
	{
		if (this.m_body.spriteAnimator.CurrentClip == null)
		{
			return;
		}
		float num = (float)this.m_body.spriteAnimator.CurrentFrame / (float)this.m_body.spriteAnimator.CurrentClip.frames.Length;
		int num2 = Mathf.Min(Mathf.FloorToInt(num * 6f), 5);
		float num3 = PhysicsEngine.PixelToUnit(this.offsets[num2]);
		this.shoulderSprite.transform.localPosition = this.shoulderSprite.transform.localPosition.WithY(num3);
		for (int i = 0; i < this.armSpriteClippers.Count; i++)
		{
			this.armSpriteClippers[i].transform.localPosition = this.shoulderSprite.transform.localPosition.WithY(Mathf.Lerp(num3, 0f, ((float)i + 1f) / ((float)this.armSpriteClippers.Count + 1f)));
		}
	}

	// Token: 0x04005373 RID: 21363
	public GameObject shoulder;

	// Token: 0x04005374 RID: 21364
	public List<GameObject> balls;

	// Token: 0x04005375 RID: 21365
	public GameObject hand;

	// Token: 0x04005376 RID: 21366
	private int[] offsets = new int[] { 0, -3, -5, -6, -5, -3 };

	// Token: 0x04005377 RID: 21367
	private DraGunController m_body;

	// Token: 0x04005378 RID: 21368
	private tk2dBaseSprite shoulderSprite;

	// Token: 0x04005379 RID: 21369
	private TileSpriteClipper handSpriteClipper;

	// Token: 0x0400537A RID: 21370
	private List<TileSpriteClipper> armSpriteClippers;
}
