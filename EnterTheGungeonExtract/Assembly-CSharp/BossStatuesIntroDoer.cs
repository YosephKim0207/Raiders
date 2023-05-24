using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000FF9 RID: 4089
[RequireComponent(typeof(GenericIntroDoer))]
public class BossStatuesIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005967 RID: 22887 RVA: 0x00221CFC File Offset: 0x0021FEFC
	private void Start()
	{
		this.m_statuesController = base.GetComponent<BossStatuesController>();
		for (int i = 0; i < this.m_statuesController.allStatues.Count; i++)
		{
			BossStatueController bossStatueController = this.m_statuesController.allStatues[i];
			bossStatueController.specRigidbody.CollideWithOthers = false;
			bossStatueController.aiActor.IsGone = true;
		}
	}

	// Token: 0x06005968 RID: 22888 RVA: 0x00221D60 File Offset: 0x0021FF60
	private void Update()
	{
	}

	// Token: 0x06005969 RID: 22889 RVA: 0x00221D64 File Offset: 0x0021FF64
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x17000CE9 RID: 3305
	// (get) Token: 0x0600596A RID: 22890 RVA: 0x00221D6C File Offset: 0x0021FF6C
	public Vector2? BossCenter
	{
		get
		{
			Vector2 vector = base.transform.position.XY() + new Vector2((float)base.dungeonPlaceable.placeableWidth / 2f, (float)base.dungeonPlaceable.placeableHeight / 2f);
			vector += new Vector2(0f, 2f);
			return new Vector2?(vector);
		}
	}

	// Token: 0x0600596B RID: 22891 RVA: 0x00221DD4 File Offset: 0x0021FFD4
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		this.m_animators = animators;
		for (int i = 0; i < this.dustAnimators.Count; i++)
		{
			this.m_animators.Add(this.dustAnimators[i]);
		}
		for (int j = 0; j < this.ghostAnimators.Count; j++)
		{
			this.m_animators.Add(this.ghostAnimators[j]);
			this.ghostAnimators[j].renderer.enabled = false;
		}
		this.m_allStatues = this.m_statuesController.allStatues;
		this.m_startingPositions = new Vector3[this.m_allStatues.Count];
		this.m_startingShadowPositions = new Vector3[this.m_allStatues.Count];
		for (int k = 0; k < this.m_allStatues.Count; k++)
		{
			this.m_animators.Add(this.m_allStatues[k].landVfx);
			this.m_startingPositions[k] = this.m_allStatues[k].transform.position;
			this.m_startingShadowPositions[k] = this.m_allStatues[k].shadowSprite.transform.position;
		}
		base.StartCoroutine(this.PlayIntro());
	}

	// Token: 0x0600596C RID: 22892 RVA: 0x00221F3C File Offset: 0x0022013C
	public override void EndIntro()
	{
		base.StopAllCoroutines();
		GameUIRoot.Instance.bossController.SetBossName(StringTableManager.GetEnemiesString(base.GetComponent<GenericIntroDoer>().portraitSlideSettings.bossNameString, -1));
		for (int i = 0; i < this.m_statuesController.allStatues.Count; i++)
		{
			BossStatueController bossStatueController = this.m_statuesController.allStatues[i];
			bossStatueController.aiActor.SkipOnEngaged();
			GameUIRoot.Instance.bossController.RegisterBossHealthHaver(bossStatueController.healthHaver, null);
			bossStatueController.specRigidbody.CollideWithOthers = true;
			bossStatueController.aiActor.IsGone = false;
			bossStatueController.aiActor.State = AIActor.ActorState.Normal;
		}
		for (int j = 0; j < this.ghostAnimators.Count; j++)
		{
			if (this.ghostAnimators[j] != null)
			{
				this.ghostAnimators[j].renderer.enabled = false;
			}
		}
		for (int k = 0; k < this.dustAnimators.Count; k++)
		{
			this.dustAnimators[k].renderer.enabled = false;
		}
		if (this.m_allStatues != null)
		{
			for (int l = 0; l < this.m_allStatues.Count; l++)
			{
				this.m_allStatues[l].transform.position = this.m_startingPositions[l];
				this.m_allStatues[l].shadowSprite.transform.position = this.m_startingShadowPositions[l];
			}
		}
		this.eyeVfx.DestroyAll();
	}

	// Token: 0x17000CEA RID: 3306
	// (get) Token: 0x0600596D RID: 22893 RVA: 0x002220F8 File Offset: 0x002202F8
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_state == BossStatuesIntroDoer.State.Finished;
		}
	}

	// Token: 0x0600596E RID: 22894 RVA: 0x00222104 File Offset: 0x00220304
	public override void OnCameraIntro()
	{
		for (int i = 0; i < this.ghostAnimators.Count; i++)
		{
			this.ghostAnimators[i].renderer.enabled = false;
		}
	}

	// Token: 0x0600596F RID: 22895 RVA: 0x00222144 File Offset: 0x00220344
	public override void OnBossCard()
	{
		this.eyeVfx.DestroyAll();
	}

	// Token: 0x06005970 RID: 22896 RVA: 0x00222154 File Offset: 0x00220354
	public override void OnCleanup()
	{
		base.behaviorSpeculator.enabled = true;
	}

	// Token: 0x06005971 RID: 22897 RVA: 0x00222164 File Offset: 0x00220364
	private IEnumerator PlayIntro()
	{
		this.m_state = BossStatuesIntroDoer.State.Playing;
		BraveUtility.RandomizeList<tk2dSpriteAnimator>(this.ghostAnimators, 0, -1);
		yield return base.StartCoroutine(this.WaitForSecondsInvariant(this.ghostDelay));
		AkSoundEngine.PostEvent("Play_ENM_statue_intro_01", base.gameObject);
		for (int i = 0; i < this.ghostAnimators.Count; i++)
		{
			this.ghostAnimators[i].renderer.enabled = true;
			this.ghostAnimators[i].Play();
			if (i < this.ghostMidDelay.Length)
			{
				yield return base.StartCoroutine(this.WaitForSecondsInvariant(this.ghostMidDelay[i]));
			}
		}
		bool done = false;
		while (!done)
		{
			done = true;
			for (int j = 0; j < this.ghostAnimators.Count; j++)
			{
				if (!(this.ghostAnimators[j] == null))
				{
					if (this.ghostAnimators[j].IsPlaying(this.ghostAnimators[j].DefaultClip))
					{
						done = false;
					}
					else
					{
						BossStatueController component = this.ghostAnimators[j].transform.parent.GetComponent<BossStatueController>();
						this.eyeVfx.SpawnAtLocalPosition(Vector3.zero, 0f, component.transformPoints[0], null, null, true, null, false);
						this.eyeVfx.SpawnAtLocalPosition(Vector3.zero, 0f, component.transformPoints[1], null, null, true, null, false);
						this.eyeVfx.ForEach(delegate(GameObject go)
						{
							foreach (tk2dSpriteAnimator tk2dSpriteAnimator in go.GetComponentsInChildren<tk2dSpriteAnimator>())
							{
								if (!this.m_animators.Contains(tk2dSpriteAnimator))
								{
									this.m_animators.Add(tk2dSpriteAnimator);
								}
							}
						});
						this.ghostAnimators[j].renderer.enabled = false;
						this.ghostAnimators[j] = null;
					}
				}
			}
			yield return null;
		}
		yield return base.StartCoroutine(this.WaitForSecondsInvariant(this.dustDelay));
		for (int k = 0; k < this.dustAnimators.Count; k++)
		{
			this.dustAnimators[k].transform.parent = SpawnManager.Instance.VFX;
			this.dustAnimators[k].Play();
			this.dustAnimators[k].GetComponent<SpriteAnimatorKiller>().enabled = true;
		}
		yield return base.StartCoroutine(this.WaitForSecondsInvariant(this.floatDelay));
		GameManager.Instance.MainCameraController.DoScreenShake(this.floatScreenShake, null, false);
		float elapsed = 0f;
		float duration = this.floatTime;
		while (elapsed < duration)
		{
			float height = elapsed / duration * this.m_statuesController.attackHopHeight;
			for (int l = 0; l < this.m_allStatues.Count; l++)
			{
				this.m_allStatues[l].transform.position = this.m_startingPositions[l] + new Vector3(0f, height, 0f);
				this.m_allStatues[l].shadowSprite.transform.position = this.m_startingShadowPositions[l];
				int num = Mathf.RoundToInt((float)(this.m_allStatues[l].shadowSprite.spriteAnimator.DefaultClip.frames.Length - 1) * Mathf.Clamp01(height / this.m_statuesController.attackHopHeight));
				this.m_allStatues[l].shadowSprite.spriteAnimator.SetFrame(num);
			}
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		yield return base.StartCoroutine(this.WaitForSecondsInvariant(this.hangTime));
		float gravity = -(2f * (this.m_statuesController.attackHopHeight / (0.5f * this.m_statuesController.attackHopTime))) / (0.5f * this.m_statuesController.attackHopTime);
		elapsed = 0f;
		duration = this.m_statuesController.attackHopTime / 2f;
		while (elapsed < duration)
		{
			float height2 = this.m_statuesController.attackHopHeight + 0f * elapsed + 0.5f * gravity * elapsed * elapsed;
			for (int m = 0; m < this.m_allStatues.Count; m++)
			{
				this.m_allStatues[m].transform.position = this.m_startingPositions[m] + new Vector3(0f, height2, 0f);
				this.m_allStatues[m].shadowSprite.transform.position = this.m_startingShadowPositions[m];
				int num2 = Mathf.RoundToInt((float)(this.m_allStatues[m].shadowSprite.spriteAnimator.DefaultClip.frames.Length - 1) * Mathf.Clamp01(height2 / this.m_statuesController.attackHopHeight));
				this.m_allStatues[m].shadowSprite.spriteAnimator.SetFrame(num2);
			}
			yield return null;
			elapsed += GameManager.INVARIANT_DELTA_TIME;
		}
		for (int n = 0; n < this.m_allStatues.Count; n++)
		{
			this.m_allStatues[n].transform.position = this.m_startingPositions[n];
			this.m_allStatues[n].shadowSprite.transform.position = this.m_startingShadowPositions[n];
		}
		for (int num3 = 0; num3 < this.m_allStatues.Count; num3++)
		{
			this.m_allStatues[num3].landVfx.gameObject.SetActive(true);
			this.m_allStatues[num3].landVfx.GetComponent<SpriteAnimatorKiller>().Restart();
		}
		GameManager.Instance.MainCameraController.DoScreenShake(this.slamScreenShake, null, false);
		yield return base.StartCoroutine(this.WaitForSecondsInvariant(this.slamTime));
		this.m_state = BossStatuesIntroDoer.State.Finished;
		yield break;
	}

	// Token: 0x06005972 RID: 22898 RVA: 0x00222180 File Offset: 0x00220380
	private IEnumerator WaitForSecondsInvariant(float time)
	{
		for (float elapsed = 0f; elapsed < time; elapsed += GameManager.INVARIANT_DELTA_TIME)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x040052B1 RID: 21169
	public float ghostDelay;

	// Token: 0x040052B2 RID: 21170
	public float[] ghostMidDelay;

	// Token: 0x040052B3 RID: 21171
	public List<tk2dSpriteAnimator> ghostAnimators;

	// Token: 0x040052B4 RID: 21172
	public VFXPool eyeVfx;

	// Token: 0x040052B5 RID: 21173
	public float dustDelay;

	// Token: 0x040052B6 RID: 21174
	public List<tk2dSpriteAnimator> dustAnimators;

	// Token: 0x040052B7 RID: 21175
	public float floatDelay = 0.3f;

	// Token: 0x040052B8 RID: 21176
	public float floatTime = 2f;

	// Token: 0x040052B9 RID: 21177
	public float hangTime = 0.5f;

	// Token: 0x040052BA RID: 21178
	public float slamTime = 1f;

	// Token: 0x040052BB RID: 21179
	public ScreenShakeSettings floatScreenShake;

	// Token: 0x040052BC RID: 21180
	public ScreenShakeSettings slamScreenShake;

	// Token: 0x040052BD RID: 21181
	private BossStatuesIntroDoer.State m_state;

	// Token: 0x040052BE RID: 21182
	private BossStatuesController m_statuesController;

	// Token: 0x040052BF RID: 21183
	private List<BossStatueController> m_allStatues;

	// Token: 0x040052C0 RID: 21184
	private List<tk2dSpriteAnimator> m_animators;

	// Token: 0x040052C1 RID: 21185
	private Vector3[] m_startingPositions;

	// Token: 0x040052C2 RID: 21186
	private Vector3[] m_startingShadowPositions;

	// Token: 0x02000FFA RID: 4090
	private enum State
	{
		// Token: 0x040052C4 RID: 21188
		Idle,
		// Token: 0x040052C5 RID: 21189
		Playing,
		// Token: 0x040052C6 RID: 21190
		Finished
	}
}
