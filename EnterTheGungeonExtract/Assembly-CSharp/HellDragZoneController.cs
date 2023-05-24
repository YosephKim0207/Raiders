using System;
using System.Collections;
using HutongGames.PlayMaker;
using UnityEngine;

// Token: 0x0200118D RID: 4493
public class HellDragZoneController : BraveBehaviour
{
	// Token: 0x060063E2 RID: 25570 RVA: 0x0026C1F8 File Offset: 0x0026A3F8
	private void Start()
	{
		this.HoleObject.SetActive(false);
		this.HoleMaterial = this.HoleObject.GetComponent<MeshRenderer>().material;
		bool flag = GameManager.Instance.PrimaryPlayer.characterIdentity == PlayableCharacters.Eevee && GameStatsManager.Instance.AllCorePastsBeaten() && !GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED);
		if ((GameManager.Instance.PrimaryPlayer.characterIdentity != PlayableCharacters.Gunslinger || GameStatsManager.Instance.GetFlag(GungeonFlags.GUNSLINGER_UNLOCKED)) && (!GameManager.Instance.PrimaryPlayer.CharacterUsesRandomGuns || GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_LICH)) && GameStatsManager.Instance.AllCorePastsBeaten())
		{
			if (GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_BULLET_HELL) == 0f || flag)
			{
				SpeculativeRigidbody specRigidbody = base.specRigidbody;
				specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.ProcessTriggerEntered));
			}
			else
			{
				this.HoleObject.SetActive(true);
				this.SetHoleSize(0.25f);
				this.m_holeIsActive = true;
				SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
				specRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.ProcessTriggerFrame));
			}
		}
		if (this.m_holeIsActive && GameStatsManager.Instance.GetPlayerStatValue(TrackedStats.TIMES_REACHED_BULLET_HELL) >= 1f)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.CryoButtonPrefab);
			gameObject.transform.parent = base.transform;
			gameObject.transform.localPosition = new Vector3(-0.5625f, -0.875f, 0f);
			gameObject.GetComponent<SpeculativeRigidbody>().Reinitialize();
			TalkDoerLite componentInChildren = gameObject.GetComponentInChildren<TalkDoerLite>();
			componentInChildren.GetAbsoluteParentRoom().RegisterInteractable(componentInChildren);
			TalkDoerLite talkDoerLite = componentInChildren;
			talkDoerLite.OnGenericFSMActionA = (Action)Delegate.Combine(talkDoerLite.OnGenericFSMActionA, new Action(this.SwitchToCryoElevator));
			TalkDoerLite talkDoerLite2 = componentInChildren;
			talkDoerLite2.OnGenericFSMActionB = (Action)Delegate.Combine(talkDoerLite2.OnGenericFSMActionB, new Action(this.RescindCryoElevator));
			this.m_cryoBool = componentInChildren.playmakerFsm.FsmVariables.GetFsmBool("IS_CRYO");
			this.m_normalBool = componentInChildren.playmakerFsm.FsmVariables.GetFsmBool("IS_NORMAL");
			this.m_cryoBool.Value = false;
			this.m_normalBool.Value = true;
			Transform transform = gameObject.transform.Find(this.cryoAnimatorName);
			if (transform)
			{
				this.cryoAnimator = transform.GetComponent<tk2dSpriteAnimator>();
			}
		}
	}

	// Token: 0x060063E3 RID: 25571 RVA: 0x0026C494 File Offset: 0x0026A694
	private void RescindCryoElevator()
	{
		this.m_cryoBool.Value = false;
		this.m_normalBool.Value = true;
		if (this.cryoAnimator && !string.IsNullOrEmpty(this.cyroDepartAnimation))
		{
			this.cryoAnimator.Play(this.cyroDepartAnimation);
		}
	}

	// Token: 0x060063E4 RID: 25572 RVA: 0x0026C4EC File Offset: 0x0026A6EC
	private void SwitchToCryoElevator()
	{
		this.m_cryoBool.Value = true;
		this.m_normalBool.Value = false;
		if (this.cryoAnimator && !string.IsNullOrEmpty(this.cryoArriveAnimation))
		{
			this.cryoAnimator.Play(this.cryoArriveAnimation);
		}
	}

	// Token: 0x060063E5 RID: 25573 RVA: 0x0026C544 File Offset: 0x0026A744
	private void ProcessTriggerFrame(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		if (this.m_holeIsActive)
		{
			PlayerController component = specRigidbody.GetComponent<PlayerController>();
			if (component && Vector2.Distance(component.CenterPosition, this.HoleObject.transform.PositionVector2()) < 2.5f)
			{
				this.GrabPlayer(component);
				this.m_holeIsActive = false;
			}
		}
	}

	// Token: 0x060063E6 RID: 25574 RVA: 0x0026C5A4 File Offset: 0x0026A7A4
	private void SetHoleSize(float size)
	{
		this.HoleMaterial.SetFloat("_UVDistCutoff", size);
	}

	// Token: 0x060063E7 RID: 25575 RVA: 0x0026C5B8 File Offset: 0x0026A7B8
	private IEnumerator LerpHoleSize(float startSize, float endSize, float duration, PlayerController targetPlayer)
	{
		float ela = 0f;
		while (ela < duration)
		{
			this.HoleObject.transform.position = targetPlayer.SpriteBottomCenter.XY().ToVector3ZisY(0f);
			ela += BraveTime.DeltaTime;
			this.SetHoleSize(Mathf.Lerp(startSize, endSize, ela / duration));
			yield return null;
		}
		yield break;
	}

	// Token: 0x060063E8 RID: 25576 RVA: 0x0026C5F0 File Offset: 0x0026A7F0
	private IEnumerator HandleGrabbyGrab(PlayerController grabbedPlayer)
	{
		grabbedPlayer.specRigidbody.Velocity = Vector2.zero;
		grabbedPlayer.specRigidbody.CapVelocity = true;
		grabbedPlayer.specRigidbody.MaxVelocity = Vector2.zero;
		yield return new WaitForSeconds(0.2f);
		grabbedPlayer.IsVisible = false;
		yield return new WaitForSeconds(2.3f);
		grabbedPlayer.specRigidbody.CapVelocity = false;
		Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
		if (this.m_cryoBool != null && this.m_cryoBool.Value)
		{
			AkSoundEngine.PostEvent("Stop_MUS_All", base.gameObject);
			GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.HELLGEON);
			float num = 0.6f;
			GameManager.Instance.DelayedLoadCharacterSelect(num, true, true);
		}
		else
		{
			GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.HELLGEON);
			GameManager.Instance.DelayedLoadNextLevel(0.5f);
		}
		yield break;
	}

	// Token: 0x060063E9 RID: 25577 RVA: 0x0026C614 File Offset: 0x0026A814
	private void GrabPlayer(PlayerController enteredPlayer)
	{
		enteredPlayer.CurrentInputState = PlayerInputState.NoInput;
		GameObject gameObject = enteredPlayer.PlayEffectOnActor(this.HellDragVFX, Vector3.zero, true, false, false);
		tk2dBaseSprite component = gameObject.GetComponent<tk2dBaseSprite>();
		component.UpdateZDepth();
		component.attachParent = null;
		component.IsPerpendicular = false;
		component.HeightOffGround = 1f;
		component.UpdateZDepth();
		component.transform.position = component.transform.position.WithX(component.transform.position.x + 0.25f);
		component.transform.position = component.transform.position.WithY((float)enteredPlayer.CurrentRoom.area.basePosition.y + 55f);
		component.usesOverrideMaterial = true;
		component.renderer.material.shader = ShaderCache.Acquire("Brave/Effects/StencilMasked");
		base.StartCoroutine(this.HandleGrabbyGrab(enteredPlayer));
	}

	// Token: 0x060063EA RID: 25578 RVA: 0x0026C704 File Offset: 0x0026A904
	private void ProcessTriggerEntered(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		Debug.Log("Hell Hole entered!");
		PlayerController component = specRigidbody.GetComponent<PlayerController>();
		this.HoleObject.SetActive(true);
		if (component)
		{
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
				specRigidbody2.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Remove(specRigidbody2.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.ProcessTriggerEntered));
			}
			this.GrabPlayer(component);
			base.StartCoroutine(this.LerpHoleSize(0f, 0.15f, 0.3f, component));
		}
	}

	// Token: 0x04005F74 RID: 24436
	public GameObject HoleObject;

	// Token: 0x04005F75 RID: 24437
	public GameObject HellDragVFX;

	// Token: 0x04005F76 RID: 24438
	public GameObject CryoButtonPrefab;

	// Token: 0x04005F77 RID: 24439
	private Material HoleMaterial;

	// Token: 0x04005F78 RID: 24440
	private bool m_holeIsActive;

	// Token: 0x04005F79 RID: 24441
	public string cryoAnimatorName;

	// Token: 0x04005F7A RID: 24442
	public string cryoArriveAnimation;

	// Token: 0x04005F7B RID: 24443
	public string cyroDepartAnimation;

	// Token: 0x04005F7C RID: 24444
	private tk2dSpriteAnimator cryoAnimator;

	// Token: 0x04005F7D RID: 24445
	private FsmBool m_cryoBool;

	// Token: 0x04005F7E RID: 24446
	private FsmBool m_normalBool;
}
