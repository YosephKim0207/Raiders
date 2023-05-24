using System;
using System.Collections;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

// Token: 0x02001149 RID: 4425
public class DraGunRoomPlaceable : DungeonPlaceableBehaviour, IPlaceConfigurable
{
	// Token: 0x17000E71 RID: 3697
	// (get) Token: 0x0600620C RID: 25100 RVA: 0x0025FC18 File Offset: 0x0025DE18
	// (set) Token: 0x0600620D RID: 25101 RVA: 0x0025FC20 File Offset: 0x0025DE20
	public bool UseInvariantTime { get; set; }

	// Token: 0x17000E72 RID: 3698
	// (get) Token: 0x0600620E RID: 25102 RVA: 0x0025FC2C File Offset: 0x0025DE2C
	// (set) Token: 0x0600620F RID: 25103 RVA: 0x0025FC34 File Offset: 0x0025DE34
	public bool DraGunKilled { get; set; }

	// Token: 0x06006210 RID: 25104 RVA: 0x0025FC40 File Offset: 0x0025DE40
	public IEnumerator Start()
	{
		yield return null;
		this.m_dragunController = this.m_room.GetComponentsAbsoluteInRoom<DraGunController>()[0];
		this.m_deathBridge = base.GetComponentInChildren<MovingPlatform>();
		this.FindPitBounds();
		yield break;
	}

	// Token: 0x06006211 RID: 25105 RVA: 0x0025FC5C File Offset: 0x0025DE5C
	public void Update()
	{
		if (!this.m_dragunController && !this.DraGunKilled)
		{
			List<DraGunController> componentsAbsoluteInRoom = this.m_room.GetComponentsAbsoluteInRoom<DraGunController>();
			if (componentsAbsoluteInRoom.Count > 0)
			{
				this.m_dragunController = this.m_room.GetComponentsAbsoluteInRoom<DraGunController>()[0];
			}
		}
		if (this.m_dragunController && !this.m_dragunController.HasDoneIntro && GameManager.Instance.PrimaryPlayer.CurrentRoom == this.m_room)
		{
			float num = (GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter.y - this.m_roomMin.y) / (float)DraGunRoomPlaceable.HallHeight;
			GameManager.Instance.MainCameraController.OverrideZoomScale = Mathf.Lerp(1f, 0.75f, num);
		}
		if (!GameManager.Instance.IsLoadingLevel && GameManager.Instance.IsAnyPlayerInRoom(this.m_room) && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.LOW && GameManager.Options.ShaderQuality != GameOptions.GenericHighMedLowOption.VERY_LOW)
		{
			float num2 = ((!this.UseInvariantTime) ? BraveTime.DeltaTime : GameManager.INVARIANT_DELTA_TIME);
			float num3;
			if (this.m_dragunController && this.m_dragunController.healthHaver.IsAlive)
			{
				num3 = ((!this.m_dragunController.IsNearDeath) ? this.pitEmbers : this.nearDeathPitEmbers);
			}
			else
			{
				num3 = this.idlePitEmbers;
			}
			float num4 = 1f;
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.MEDIUM)
			{
				num4 = 0.25f;
			}
			GlobalSparksDoer.DoRandomParticleBurst((int)(num3 * num2 * num4), this.m_pitMin.ToVector3ZUp(100f), this.m_pitMax.ToVector3ZUp(100f), Vector3.up, 90f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
			if (this.m_dragunController && this.m_dragunController.healthHaver.IsAlive)
			{
				GlobalSparksDoer.DoRandomParticleBurst((int)(this.roomEmbers * num2 * num4), this.m_roomMin.ToVector3ZisY(0f), this.m_roomMax.ToVector3ZisY(0f), Vector3.up, 90f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
			}
			if (this.m_dragunController && this.m_dragunController.IsTransitioning)
			{
				GlobalSparksDoer.DoRandomParticleBurst((int)(this.transitioningEmbers * num2 * num4), this.m_bodyMin.ToVector3ZUp(this.m_bodyMin.y - 5f), this.m_bodyMax.ToVector3ZUp(this.m_bodyMin.y - 5f), Vector3.up * 1.5f, 180f, 0.5f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
			}
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH && GlobalSparksDoer.EmberParticles)
			{
				GlobalSparksDoer.EmberParticles.maxParticles = 10000;
			}
		}
	}

	// Token: 0x06006212 RID: 25106 RVA: 0x0025FFBC File Offset: 0x0025E1BC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06006213 RID: 25107 RVA: 0x0025FFC4 File Offset: 0x0025E1C4
	public void ExtendDeathBridge()
	{
		this.m_deathBridge.specRigidbody.enabled = true;
		this.m_deathBridge.specRigidbody.Initialize();
		this.m_deathBridge.spriteAnimator.Play();
		this.m_deathBridge.MarkCells();
	}

	// Token: 0x06006214 RID: 25108 RVA: 0x00260004 File Offset: 0x0025E204
	public void ConfigureOnPlacement(RoomHandler room)
	{
		this.m_room = room;
	}

	// Token: 0x06006215 RID: 25109 RVA: 0x00260010 File Offset: 0x0025E210
	private void FindPitBounds()
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		this.m_pitMin = intVector.ToVector2() + new Vector2(0f, 14f);
		this.m_pitMax = intVector.ToVector2() + new Vector2(36f, 29f);
		this.m_roomMin = this.m_room.area.UnitBottomLeft;
		this.m_roomMax = this.m_room.area.UnitTopRight;
		this.m_bodyMin = this.m_pitMin + new Vector2(15f, 0f);
		this.m_bodyMax = this.m_pitMin + new Vector2(21f, 15f);
	}

	// Token: 0x04005CF0 RID: 23792
	public static int HallHeight = 18;

	// Token: 0x04005CF1 RID: 23793
	public float roomEmbers = 100f;

	// Token: 0x04005CF2 RID: 23794
	public float pitEmbers = 300f;

	// Token: 0x04005CF3 RID: 23795
	public float nearDeathPitEmbers = 600f;

	// Token: 0x04005CF4 RID: 23796
	public float idlePitEmbers = 100f;

	// Token: 0x04005CF5 RID: 23797
	public float transitioningEmbers = 500f;

	// Token: 0x04005CF8 RID: 23800
	private RoomHandler m_room;

	// Token: 0x04005CF9 RID: 23801
	private DraGunController m_dragunController;

	// Token: 0x04005CFA RID: 23802
	private MovingPlatform m_deathBridge;

	// Token: 0x04005CFB RID: 23803
	private Vector2 m_pitMin;

	// Token: 0x04005CFC RID: 23804
	private Vector2 m_pitMax;

	// Token: 0x04005CFD RID: 23805
	private Vector2 m_roomMin;

	// Token: 0x04005CFE RID: 23806
	private Vector2 m_roomMax;

	// Token: 0x04005CFF RID: 23807
	private Vector2 m_bodyMin;

	// Token: 0x04005D00 RID: 23808
	private Vector2 m_bodyMax;
}
