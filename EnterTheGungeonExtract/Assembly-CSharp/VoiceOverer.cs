using System;
using System.Collections;
using UnityEngine;

// Token: 0x020012BA RID: 4794
public class VoiceOverer : MonoBehaviour
{
	// Token: 0x06006B43 RID: 27459 RVA: 0x002A25D8 File Offset: 0x002A07D8
	private void Start()
	{
		this.m_bs = base.GetComponent<BehaviorSpeculator>();
		HealthHaver component = base.GetComponent<HealthHaver>();
		component.OnDamaged += this.HandleDamaged;
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnRealPlayerDeath = (Action<PlayerController>)Delegate.Combine(playerController.OnRealPlayerDeath, new Action<PlayerController>(this.HandlePlayerGameOver));
		}
	}

	// Token: 0x06006B44 RID: 27460 RVA: 0x002A2654 File Offset: 0x002A0854
	private void HandlePlayerGameOver(PlayerController player)
	{
		AIActor component = base.GetComponent<AIActor>();
		if (component.HasBeenEngaged && player.CurrentRoom == component.ParentRoom)
		{
			base.StartCoroutine(this.HandlePlayerLostVO());
		}
		this.DisconnectCallback();
	}

	// Token: 0x06006B45 RID: 27461 RVA: 0x002A2698 File Offset: 0x002A0898
	private void DisconnectCallback()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			playerController.OnRealPlayerDeath = (Action<PlayerController>)Delegate.Remove(playerController.OnRealPlayerDeath, new Action<PlayerController>(this.HandlePlayerGameOver));
		}
	}

	// Token: 0x06006B46 RID: 27462 RVA: 0x002A26F0 File Offset: 0x002A08F0
	private void OnDestroy()
	{
		this.DisconnectCallback();
	}

	// Token: 0x06006B47 RID: 27463 RVA: 0x002A26F8 File Offset: 0x002A08F8
	private void Update()
	{
		this.m_sinceBark += BraveTime.DeltaTime;
		this.m_sinceAttackBark += BraveTime.DeltaTime;
		this.m_sinceAnyBark += BraveTime.DeltaTime;
		bool flag = this.m_bs.ActiveContinuousAttackBehavior != null;
		if (flag && !this.m_lastFrameHadContinueAttack)
		{
			this.HandleAttackBark();
		}
		this.m_lastFrameHadContinueAttack = flag;
	}

	// Token: 0x06006B48 RID: 27464 RVA: 0x002A276C File Offset: 0x002A096C
	private void HandleDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		if (this.m_sinceBark > 10f && this.m_sinceAnyBark > 3f && resultValue > 0f)
		{
			this.HandleDamageBark();
			this.m_sinceBark = 0f;
			this.m_sinceAnyBark = 0f;
		}
	}

	// Token: 0x06006B49 RID: 27465 RVA: 0x002A27C0 File Offset: 0x002A09C0
	public IEnumerator HandleIntroVO()
	{
		int eventIndex = UnityEngine.Random.Range(0, this.IntroEvents.Length);
		AkSoundEngine.PostEvent(this.IntroEvents[eventIndex], base.gameObject);
		AIAnimator aiAnimator = base.GetComponent<AIAnimator>();
		aiAnimator.PlayUntilCancelled("intro_talk", false, null, -1f, false);
		yield return base.StartCoroutine(this.HandleTalk(this.IntroKeys[eventIndex], -1f));
		aiAnimator.EndAnimationIf("intro_talk");
		this.m_sinceAttackBark = 5f;
		yield break;
	}

	// Token: 0x06006B4A RID: 27466 RVA: 0x002A27DC File Offset: 0x002A09DC
	public IEnumerator HandlePlayerLostVO()
	{
		int eventIndex = UnityEngine.Random.Range(0, this.DefeatedPlayerEvents.Length);
		AkSoundEngine.PostEvent(this.DefeatedPlayerEvents[eventIndex], base.gameObject);
		yield return base.StartCoroutine(this.HandleTalk(this.DefeatedPlayerKeys[eventIndex], -1f));
		yield break;
	}

	// Token: 0x06006B4B RID: 27467 RVA: 0x002A27F8 File Offset: 0x002A09F8
	public IEnumerator HandlePlayerWonVO(float maxDuration)
	{
		int eventIndex = UnityEngine.Random.Range(0, this.KilledByPlayerEvents.Length);
		AkSoundEngine.PostEvent(this.KilledByPlayerEvents[eventIndex], base.gameObject);
		yield return base.StartCoroutine(this.HandleTalk(this.KilledByPlayerKeys[eventIndex], maxDuration));
		yield break;
	}

	// Token: 0x06006B4C RID: 27468 RVA: 0x002A281C File Offset: 0x002A0A1C
	public void HandleDamageBark()
	{
		int num = UnityEngine.Random.Range(0, this.DamageBarkEvents.Length);
		if (num == this.m_lastBark && this.m_lastBark == 2)
		{
			num = 0;
		}
		this.m_lastBark = num;
		AkSoundEngine.PostEvent(this.DamageBarkEvents[num], base.gameObject);
	}

	// Token: 0x06006B4D RID: 27469 RVA: 0x002A2870 File Offset: 0x002A0A70
	public void HandleAttackBark()
	{
		if (this.m_sinceAttackBark > 10f && this.m_sinceAnyBark > 3f)
		{
			this.m_sinceAttackBark = 0f;
			this.m_sinceAnyBark = 0f;
			int num;
			for (num = UnityEngine.Random.Range(0, this.AttackBarkEvents.Length); num == this.m_lastBark; num = UnityEngine.Random.Range(0, this.AttackBarkEvents.Length))
			{
			}
			AkSoundEngine.PostEvent(this.AttackBarkEvents[num], base.gameObject);
		}
	}

	// Token: 0x06006B4E RID: 27470 RVA: 0x002A28F8 File Offset: 0x002A0AF8
	public IEnumerator HandleTalk(string stringKey, float maxDuration = -1f)
	{
		base.GetComponent<GenericIntroDoer>().SuppressSkipping = true;
		if (string.IsNullOrEmpty(stringKey))
		{
			if (maxDuration > 0f)
			{
				yield return new WaitForSeconds(maxDuration / 2f);
			}
		}
		else
		{
			yield return base.StartCoroutine(this.TalkRaw(StringTableManager.GetString(stringKey), maxDuration));
		}
		yield return null;
		base.GetComponent<GenericIntroDoer>().SuppressSkipping = false;
		yield break;
	}

	// Token: 0x06006B4F RID: 27471 RVA: 0x002A2924 File Offset: 0x002A0B24
	private IEnumerator TalkRaw(string plaintext, float maxDuration = -1f)
	{
		TextBoxManager.ShowTextBox(base.transform.position + new Vector3(2.25f, 7.5f, 0f), base.transform, 5f, plaintext, string.Empty, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		float elapsed = 0f;
		bool advancedPressed = false;
		while (!advancedPressed)
		{
			advancedPressed = BraveInput.GetInstanceForPlayer(0).WasAdvanceDialoguePressed() || BraveInput.GetInstanceForPlayer(1).WasAdvanceDialoguePressed();
			if (maxDuration > 0f)
			{
				elapsed += GameManager.INVARIANT_DELTA_TIME;
				if (elapsed > maxDuration)
				{
					break;
				}
			}
			yield return null;
		}
		TextBoxManager.ClearTextBox(base.transform);
		yield break;
	}

	// Token: 0x04006834 RID: 26676
	public string[] IntroEvents;

	// Token: 0x04006835 RID: 26677
	public string[] IntroKeys;

	// Token: 0x04006836 RID: 26678
	public string[] DefeatedPlayerEvents;

	// Token: 0x04006837 RID: 26679
	public string[] DefeatedPlayerKeys;

	// Token: 0x04006838 RID: 26680
	public string[] KilledByPlayerEvents;

	// Token: 0x04006839 RID: 26681
	public string[] KilledByPlayerKeys;

	// Token: 0x0400683A RID: 26682
	public string[] DamageBarkEvents;

	// Token: 0x0400683B RID: 26683
	public string[] AttackBarkEvents;

	// Token: 0x0400683C RID: 26684
	private float m_sinceAnyBark = 100f;

	// Token: 0x0400683D RID: 26685
	private float m_sinceBark = 100f;

	// Token: 0x0400683E RID: 26686
	private int m_lastBark = -1;

	// Token: 0x0400683F RID: 26687
	private bool m_lastFrameHadContinueAttack;

	// Token: 0x04006840 RID: 26688
	private float m_sinceAttackBark = 100f;

	// Token: 0x04006841 RID: 26689
	private int m_lastAttackBark = -1;

	// Token: 0x04006842 RID: 26690
	private BehaviorSpeculator m_bs;
}
