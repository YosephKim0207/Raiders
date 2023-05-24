using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000E98 RID: 3736
public class FoyerGungeonDoor : BraveBehaviour
{
	// Token: 0x06004F35 RID: 20277 RVA: 0x001B7700 File Offset: 0x001B5900
	private void Start()
	{
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggered));
	}

	// Token: 0x06004F36 RID: 20278 RVA: 0x001B772C File Offset: 0x001B592C
	private void OnTriggered(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.LoadsCustomLevel && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
		{
			return;
		}
		PlayerController component = specRigidbody.GetComponent<PlayerController>();
		if (this.ReturnToFoyerFromTutorial)
		{
			if (!this.m_triggered && component != null && component == GameManager.Instance.PrimaryPlayer)
			{
				this.m_triggered = true;
				base.StartCoroutine(this.HandleLoading(component));
				return;
			}
		}
		else if (!this.m_triggered && component != null && component == GameManager.Instance.PrimaryPlayer)
		{
			this.m_triggered = true;
			base.StartCoroutine(this.HandleLoading(component));
		}
		else if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && component != null && component == GameManager.Instance.SecondaryPlayer && this.m_triggered && !this.m_coopTriggered)
		{
			this.m_coopTriggered = true;
			base.StartCoroutine(this.HandleCoopAnimation(component));
		}
	}

	// Token: 0x06004F37 RID: 20279 RVA: 0x001B7850 File Offset: 0x001B5A50
	private IEnumerator HandleCoopAnimation(PlayerController p)
	{
		p.specRigidbody.Velocity = Vector2.zero;
		p.CurrentInputState = PlayerInputState.NoInput;
		p.ToggleShadowVisiblity(false);
		if (!this.southernDoor)
		{
			p.QueueSpecificAnimation("doorway");
			float elapsed = 0f;
			float duration = 0.5f;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				p.specRigidbody.Velocity = Vector2.up * 2f;
				yield return null;
			}
		}
		else
		{
			p.spriteAnimator.Stop();
		}
		yield break;
	}

	// Token: 0x06004F38 RID: 20280 RVA: 0x001B7874 File Offset: 0x001B5A74
	private IEnumerator HandleLoading(PlayerController p)
	{
		GameUIRoot.Instance.HideCoreUI(string.Empty);
		GameUIRoot.Instance.PauseMenuPanel.GetComponent<PauseMenuController>().ForceHideMetaCurrencyPanel();
		p.specRigidbody.Velocity = Vector2.zero;
		p.CurrentInputState = PlayerInputState.NoInput;
		p.ToggleShadowVisiblity(false);
		if (this.ReturnToFoyerFromTutorial)
		{
			p.specRigidbody.Velocity = Vector2.down * p.stats.MovementSpeed;
		}
		else if (!this.southernDoor)
		{
			p.QueueSpecificAnimation("doorway");
			float elapsed = 0f;
			float duration = 0.5f;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				p.specRigidbody.Velocity = Vector2.up * 2f;
				yield return null;
			}
		}
		else
		{
			p.spriteAnimator.Stop();
		}
		Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
		if (this.ReturnToFoyerFromTutorial)
		{
			Foyer.DoIntroSequence = false;
			Foyer.DoMainMenu = false;
			GameManager.Instance.DelayedReturnToFoyer(0.75f);
		}
		else if (this.LoadsCharacterSelect)
		{
			GameManager.Instance.DelayedLoadMainMenu(0.75f);
		}
		else if (this.LoadsCustomLevel)
		{
			GameManager.Instance.DelayedLoadCustomLevel(0.75f, this.LevelNameToLoad);
		}
		else
		{
			GameManager.Instance.DelayedLoadNextLevel(0.75f);
		}
		yield return new WaitForSeconds(0.5f);
		if (this.ReturnToFoyerFromTutorial)
		{
			yield break;
		}
		Foyer.Instance.OnDepartedFoyer();
		yield break;
	}

	// Token: 0x06004F39 RID: 20281 RVA: 0x001B7898 File Offset: 0x001B5A98
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04004672 RID: 18034
	public bool LoadsCustomLevel;

	// Token: 0x04004673 RID: 18035
	[ShowInInspectorIf("LoadsCustomLevel", false)]
	public string LevelNameToLoad = string.Empty;

	// Token: 0x04004674 RID: 18036
	public bool LoadsCharacterSelect;

	// Token: 0x04004675 RID: 18037
	public bool ReturnToFoyerFromTutorial;

	// Token: 0x04004676 RID: 18038
	public bool southernDoor;

	// Token: 0x04004677 RID: 18039
	private bool m_triggered;

	// Token: 0x04004678 RID: 18040
	private bool m_coopTriggered;
}
