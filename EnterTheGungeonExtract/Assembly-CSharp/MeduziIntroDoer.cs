using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FEB RID: 4075
[RequireComponent(typeof(GenericIntroDoer))]
public class MeduziIntroDoer : SpecificIntroDoer
{
	// Token: 0x060058EC RID: 22764 RVA: 0x0021F1F4 File Offset: 0x0021D3F4
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x060058ED RID: 22765 RVA: 0x0021F1FC File Offset: 0x0021D3FC
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x060058EE RID: 22766 RVA: 0x0021F20C File Offset: 0x0021D40C
	public IEnumerator DoIntro()
	{
		tk2dBaseSprite m_shadowSprite = base.aiActor.ShadowObject.GetComponent<tk2dBaseSprite>();
		float elapsed = 0f;
		float duration = 4f;
		while (elapsed < duration && !this.m_isFinished)
		{
			if (elapsed > 3.33f)
			{
				m_shadowSprite.color = m_shadowSprite.color.WithAlpha(Mathf.InverseLerp(3.33f, 3.75f, elapsed));
			}
			else if (elapsed > 2f)
			{
				m_shadowSprite.color = m_shadowSprite.color.WithAlpha(Mathf.InverseLerp(2.75f, 2f, elapsed));
			}
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		m_shadowSprite.color = m_shadowSprite.color.WithAlpha(1f);
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x17000CC3 RID: 3267
	// (get) Token: 0x060058EF RID: 22767 RVA: 0x0021F228 File Offset: 0x0021D428
	public override bool IsIntroFinished
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060058F0 RID: 22768 RVA: 0x0021F22C File Offset: 0x0021D42C
	public override void EndIntro()
	{
		this.m_isFinished = true;
	}

	// Token: 0x0400520E RID: 21006
	private bool m_isFinished;
}
