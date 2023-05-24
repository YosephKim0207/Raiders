using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02000FB6 RID: 4022
[RequireComponent(typeof(GenericIntroDoer))]
public class AdvancedDraGunIntroDoer : SpecificIntroDoer
{
	// Token: 0x0600578D RID: 22413 RVA: 0x00216E9C File Offset: 0x0021509C
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0600578E RID: 22414 RVA: 0x00216EA4 File Offset: 0x002150A4
	public override IntVector2 OverrideExitBasePosition(DungeonData.Direction directionToWalk, IntVector2 exitBaseCenter)
	{
		return exitBaseCenter + new IntVector2(0, DraGunRoomPlaceable.HallHeight);
	}

	// Token: 0x17000C80 RID: 3200
	// (get) Token: 0x0600578F RID: 22415 RVA: 0x00216EB8 File Offset: 0x002150B8
	public override Vector2? OverrideIntroPosition
	{
		get
		{
			return new Vector2?(base.specRigidbody.UnitCenter + new Vector2(0f, 4f));
		}
	}

	// Token: 0x06005790 RID: 22416 RVA: 0x00216EE0 File Offset: 0x002150E0
	public override void PlayerWalkedIn(PlayerController player, List<tk2dSpriteAnimator> animators)
	{
		this.m_introDummy = base.transform.Find("IntroDummy").GetComponent<tk2dSpriteAnimator>();
		this.m_introBabyDummy = base.transform.Find("IntroDummy/baby").GetComponent<tk2dSpriteAnimator>();
		this.m_introVfxDummy = base.transform.Find("IntroDummy/vfx").GetComponent<tk2dSpriteAnimator>();
		this.m_introDummy.aiAnimator = base.aiAnimator;
		this.m_introBabyDummy.aiAnimator = base.aiAnimator;
		this.m_introVfxDummy.aiAnimator = base.aiAnimator;
		this.m_introVfxDummy.sprite.usesOverrideMaterial = false;
		this.m_neck = base.transform.Find("Neck").gameObject;
		this.m_wings = base.transform.Find("Wings").gameObject;
		this.m_leftArm = base.transform.Find("LeftArm").gameObject;
		this.m_rightArm = base.transform.Find("RightArm").gameObject;
		this.m_introDummy.gameObject.SetActive(true);
		this.m_introBabyDummy.gameObject.SetActive(true);
		this.m_introVfxDummy.gameObject.SetActive(true);
		base.renderer.enabled = false;
		this.m_neck.SetActive(false);
		this.m_wings.SetActive(false);
		this.m_leftArm.SetActive(false);
		this.m_rightArm.SetActive(false);
		base.StartCoroutine(this.RunEmbers());
	}

	// Token: 0x06005791 RID: 22417 RVA: 0x0021706C File Offset: 0x0021526C
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

	// Token: 0x06005792 RID: 22418 RVA: 0x00217088 File Offset: 0x00215288
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		animators.Add(this.m_introDummy);
		animators.Add(this.m_introBabyDummy);
		animators.Add(this.m_introVfxDummy);
		base.GetComponent<DragunCracktonMap>().ConvertToGold();
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x06005793 RID: 22419 RVA: 0x002170C8 File Offset: 0x002152C8
	public IEnumerator DoIntro()
	{
		this.m_introDummy.Play("intro");
		this.m_introBabyDummy.Play("intro_baby");
		this.m_introVfxDummy.Play("intro_vfx");
		this.m_introVfxDummy.sprite.usesOverrideMaterial = false;
		while (this.m_introDummy.IsPlaying("intro"))
		{
			yield return null;
		}
		this.m_introDummy.gameObject.SetActive(false);
		this.m_introBabyDummy.gameObject.SetActive(false);
		this.m_introVfxDummy.gameObject.SetActive(false);
		base.renderer.enabled = true;
		this.m_neck.SetActive(true);
		this.m_wings.SetActive(true);
		this.m_leftArm.SetActive(true);
		this.m_rightArm.SetActive(true);
		base.aiAnimator.EndAnimation();
		base.GetComponent<DraGunController>().ModifyCamera(true);
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x17000C81 RID: 3201
	// (get) Token: 0x06005794 RID: 22420 RVA: 0x002170E4 File Offset: 0x002152E4
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_isFinished;
		}
	}

	// Token: 0x06005795 RID: 22421 RVA: 0x002170EC File Offset: 0x002152EC
	public override void EndIntro()
	{
		this.m_introDummy.gameObject.SetActive(false);
		this.m_introBabyDummy.gameObject.SetActive(false);
		this.m_introVfxDummy.gameObject.SetActive(false);
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

	// Token: 0x0400509F RID: 20639
	private bool m_isFinished;

	// Token: 0x040050A0 RID: 20640
	private tk2dSpriteAnimator m_introDummy;

	// Token: 0x040050A1 RID: 20641
	private tk2dSpriteAnimator m_introBabyDummy;

	// Token: 0x040050A2 RID: 20642
	private tk2dSpriteAnimator m_introVfxDummy;

	// Token: 0x040050A3 RID: 20643
	private GameObject m_neck;

	// Token: 0x040050A4 RID: 20644
	private GameObject m_wings;

	// Token: 0x040050A5 RID: 20645
	private GameObject m_leftArm;

	// Token: 0x040050A6 RID: 20646
	private GameObject m_rightArm;
}
