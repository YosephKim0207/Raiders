using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x0200101E RID: 4126
[RequireComponent(typeof(GenericIntroDoer))]
public class DraGunIntroDoer : SpecificIntroDoer
{
	// Token: 0x06005A79 RID: 23161 RVA: 0x0022939C File Offset: 0x0022759C
	public void Start()
	{
		base.aiActor.IgnoreForRoomClear = true;
	}

	// Token: 0x06005A7A RID: 23162 RVA: 0x002293AC File Offset: 0x002275AC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06005A7B RID: 23163 RVA: 0x002293B4 File Offset: 0x002275B4
	public override IntVector2 OverrideExitBasePosition(DungeonData.Direction directionToWalk, IntVector2 exitBaseCenter)
	{
		return exitBaseCenter + new IntVector2(0, DraGunRoomPlaceable.HallHeight);
	}

	// Token: 0x17000D23 RID: 3363
	// (get) Token: 0x06005A7C RID: 23164 RVA: 0x002293C8 File Offset: 0x002275C8
	public override Vector2? OverrideIntroPosition
	{
		get
		{
			return new Vector2?(base.specRigidbody.UnitCenter + new Vector2(0f, 4f));
		}
	}

	// Token: 0x06005A7D RID: 23165 RVA: 0x002293F0 File Offset: 0x002275F0
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		this.m_introDummy = base.transform.Find("IntroDummy").GetComponent<tk2dSpriteAnimator>();
		this.m_transitionDummy = base.transform.Find("TransitionDummy").GetComponent<tk2dSpriteAnimator>();
		this.m_deathDummy = base.transform.Find("DeathDummy").GetComponent<tk2dSpriteAnimator>();
		this.m_introDummy.aiAnimator = base.aiAnimator;
		this.m_transitionDummy.aiAnimator = base.aiAnimator;
		this.m_deathDummy.aiAnimator = base.aiAnimator;
		this.m_neck = base.transform.Find("Neck").gameObject;
		this.m_wings = base.transform.Find("Wings").gameObject;
		this.m_leftArm = base.transform.Find("LeftArm").gameObject;
		this.m_rightArm = base.transform.Find("RightArm").gameObject;
		this.m_introDummy.gameObject.SetActive(true);
		this.m_transitionDummy.gameObject.SetActive(false);
		base.renderer.enabled = false;
		this.m_neck.SetActive(false);
		this.m_wings.SetActive(false);
		this.m_leftArm.SetActive(false);
		this.m_rightArm.SetActive(false);
		base.aiActor.IgnoreForRoomClear = false;
		base.aiActor.ParentRoom.SealRoom();
		base.StartCoroutine(this.RunEmbers());
	}

	// Token: 0x06005A7E RID: 23166 RVA: 0x00229574 File Offset: 0x00227774
	private IEnumerator RunEmbers()
	{
		DraGunRoomPlaceable emberDoer = base.aiActor.ParentRoom.GetComponentsAbsoluteInRoom<DraGunRoomPlaceable>()[0];
		emberDoer.UseInvariantTime = true;
		while (!this.m_isFinished)
		{
			yield return null;
		}
		emberDoer.UseInvariantTime = false;
		yield break;
	}

	// Token: 0x06005A7F RID: 23167 RVA: 0x00229590 File Offset: 0x00227790
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		animators.Add(this.m_introDummy);
		animators.Add(this.m_transitionDummy);
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x06005A80 RID: 23168 RVA: 0x002295B8 File Offset: 0x002277B8
	public IEnumerator DoIntro()
	{
		this.m_introDummy.Play("intro");
		while (this.m_introDummy.IsPlaying("intro"))
		{
			yield return null;
		}
		this.m_introDummy.gameObject.SetActive(false);
		this.m_transitionDummy.gameObject.SetActive(true);
		this.m_transitionDummy.Play("roar");
		while (this.m_transitionDummy.IsPlaying("roar"))
		{
			yield return null;
		}
		this.m_transitionDummy.Play("idle");
		base.GetComponent<DraGunController>().ModifyCamera(true);
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x17000D24 RID: 3364
	// (get) Token: 0x06005A81 RID: 23169 RVA: 0x002295D4 File Offset: 0x002277D4
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x06005A82 RID: 23170 RVA: 0x002295DC File Offset: 0x002277DC
	public override void EndIntro()
	{
		this.m_introDummy.gameObject.SetActive(false);
		this.m_transitionDummy.gameObject.SetActive(false);
		base.renderer.enabled = true;
		this.m_neck.SetActive(true);
		this.m_wings.SetActive(true);
		this.m_leftArm.SetActive(true);
		this.m_rightArm.SetActive(true);
		base.aiAnimator.EndAnimation();
		DraGunController component = base.GetComponent<DraGunController>();
		component.ModifyCamera(true);
		component.BlockPitTiles(true);
		component.HasDoneIntro = true;
		this.m_isFinished = true;
	}

	// Token: 0x040053E9 RID: 21481
	private bool m_isFinished;

	// Token: 0x040053EA RID: 21482
	private tk2dSpriteAnimator m_introDummy;

	// Token: 0x040053EB RID: 21483
	private tk2dSpriteAnimator m_transitionDummy;

	// Token: 0x040053EC RID: 21484
	private tk2dSpriteAnimator m_deathDummy;

	// Token: 0x040053ED RID: 21485
	private GameObject m_neck;

	// Token: 0x040053EE RID: 21486
	private GameObject m_wings;

	// Token: 0x040053EF RID: 21487
	private GameObject m_leftArm;

	// Token: 0x040053F0 RID: 21488
	private GameObject m_rightArm;
}
