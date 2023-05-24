using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000C13 RID: 3091
[AddComponentMenu("2D Toolkit/UI/tk2dUITweenItem")]
public class tk2dUITweenItem : tk2dUIBaseItemControl
{
	// Token: 0x17000A09 RID: 2569
	// (get) Token: 0x06004239 RID: 16953 RVA: 0x00156BC0 File Offset: 0x00154DC0
	public bool UseOnReleaseInsteadOfOnUp
	{
		get
		{
			return this.useOnReleaseInsteadOfOnUp;
		}
	}

	// Token: 0x0600423A RID: 16954 RVA: 0x00156BC8 File Offset: 0x00154DC8
	private void Awake()
	{
		this.onUpScale = base.transform.localScale;
	}

	// Token: 0x0600423B RID: 16955 RVA: 0x00156BDC File Offset: 0x00154DDC
	private void OnEnable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown += this.ButtonDown;
			if (this.canButtonBeHeldDown)
			{
				if (this.useOnReleaseInsteadOfOnUp)
				{
					this.uiItem.OnRelease += this.ButtonUp;
				}
				else
				{
					this.uiItem.OnUp += this.ButtonUp;
				}
			}
		}
		this.internalTweenInProgress = false;
		this.tweenTimeElapsed = 0f;
		base.transform.localScale = this.onUpScale;
	}

	// Token: 0x0600423C RID: 16956 RVA: 0x00156C7C File Offset: 0x00154E7C
	private void OnDisable()
	{
		if (this.uiItem)
		{
			this.uiItem.OnDown -= this.ButtonDown;
			if (this.canButtonBeHeldDown)
			{
				if (this.useOnReleaseInsteadOfOnUp)
				{
					this.uiItem.OnRelease -= this.ButtonUp;
				}
				else
				{
					this.uiItem.OnUp -= this.ButtonUp;
				}
			}
		}
	}

	// Token: 0x0600423D RID: 16957 RVA: 0x00156CFC File Offset: 0x00154EFC
	private void ButtonDown()
	{
		if (this.tweenDuration <= 0f)
		{
			base.transform.localScale = this.onDownScale;
		}
		else
		{
			base.transform.localScale = this.onUpScale;
			this.tweenTargetScale = this.onDownScale;
			this.tweenStartingScale = base.transform.localScale;
			if (!this.internalTweenInProgress)
			{
				base.StartCoroutine(this.ScaleTween());
				this.internalTweenInProgress = true;
			}
		}
	}

	// Token: 0x0600423E RID: 16958 RVA: 0x00156D7C File Offset: 0x00154F7C
	private void ButtonUp()
	{
		if (this.tweenDuration <= 0f)
		{
			base.transform.localScale = this.onUpScale;
		}
		else
		{
			this.tweenTargetScale = this.onUpScale;
			this.tweenStartingScale = base.transform.localScale;
			if (!this.internalTweenInProgress)
			{
				base.StartCoroutine(this.ScaleTween());
				this.internalTweenInProgress = true;
			}
		}
	}

	// Token: 0x0600423F RID: 16959 RVA: 0x00156DEC File Offset: 0x00154FEC
	private IEnumerator ScaleTween()
	{
		this.tweenTimeElapsed = 0f;
		while (this.tweenTimeElapsed < this.tweenDuration)
		{
			base.transform.localScale = Vector3.Lerp(this.tweenStartingScale, this.tweenTargetScale, this.tweenTimeElapsed / this.tweenDuration);
			yield return null;
			this.tweenTimeElapsed += tk2dUITime.deltaTime;
		}
		base.transform.localScale = this.tweenTargetScale;
		this.internalTweenInProgress = false;
		if (!this.canButtonBeHeldDown)
		{
			if (this.tweenDuration <= 0f)
			{
				base.transform.localScale = this.onUpScale;
			}
			else
			{
				this.tweenTargetScale = this.onUpScale;
				this.tweenStartingScale = base.transform.localScale;
				base.StartCoroutine(this.ScaleTween());
				this.internalTweenInProgress = true;
			}
		}
		yield break;
	}

	// Token: 0x06004240 RID: 16960 RVA: 0x00156E08 File Offset: 0x00155008
	public void InternalSetUseOnReleaseInsteadOfOnUp(bool state)
	{
		this.useOnReleaseInsteadOfOnUp = state;
	}

	// Token: 0x040034A6 RID: 13478
	private Vector3 onUpScale;

	// Token: 0x040034A7 RID: 13479
	public Vector3 onDownScale = new Vector3(0.9f, 0.9f, 0.9f);

	// Token: 0x040034A8 RID: 13480
	public float tweenDuration = 0.1f;

	// Token: 0x040034A9 RID: 13481
	public bool canButtonBeHeldDown = true;

	// Token: 0x040034AA RID: 13482
	[SerializeField]
	private bool useOnReleaseInsteadOfOnUp;

	// Token: 0x040034AB RID: 13483
	private bool internalTweenInProgress;

	// Token: 0x040034AC RID: 13484
	private Vector3 tweenTargetScale = Vector3.one;

	// Token: 0x040034AD RID: 13485
	private Vector3 tweenStartingScale = Vector3.one;

	// Token: 0x040034AE RID: 13486
	private float tweenTimeElapsed;
}
