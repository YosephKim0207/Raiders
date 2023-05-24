using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020016C4 RID: 5828
public class SkyRocket : BraveBehaviour
{
	// Token: 0x0600877A RID: 34682 RVA: 0x00382CD0 File Offset: 0x00380ED0
	public void Start()
	{
		base.sprite = base.GetComponentInChildren<tk2dSprite>();
		this.m_timer = this.AscentTime;
		this.m_startPosition = base.transform.position;
		this.m_startHeight = base.sprite.HeightOffGround;
		base.sprite = base.GetComponentInChildren<tk2dSprite>();
		base.spriteAnimator = base.GetComponentInChildren<tk2dSpriteAnimator>();
		if (this.AscentTime == 0f)
		{
			base.transform.position = this.m_startPosition + new Vector3(0f, this.MaxHeight, 0f);
		}
	}

	// Token: 0x0600877B RID: 34683 RVA: 0x00382D6C File Offset: 0x00380F6C
	public void Update()
	{
		this.m_timer -= BraveTime.DeltaTime;
		if (this.m_state == SkyRocket.SkyRocketState.Ascend)
		{
			float num = this.AscentCurve.Evaluate(1f - Mathf.Clamp01(this.m_timer / this.AscentTime));
			float num2 = num * this.MaxHeight;
			base.transform.position = this.m_startPosition + new Vector3(0f, num2, 0f);
			base.sprite.HeightOffGround = this.m_startHeight + num * this.MaxSpriteHeight;
			if (this.m_timer <= 0f)
			{
				this.m_timer = this.HangTime;
				this.m_state = SkyRocket.SkyRocketState.Hang;
				if (base.sprite.attachParent)
				{
					base.sprite.attachParent.DetachRenderer(base.sprite);
				}
				if (this.TargetVector2 != Vector2.zero)
				{
					this.m_targetLandPosition = this.TargetVector2;
				}
				else
				{
					Vector2 vector = new Vector2(UnityEngine.Random.Range(-this.Variance, this.Variance), UnityEngine.Random.Range(-this.Variance, this.Variance));
					bool flag = UnityEngine.Random.value < this.LeadPercentage;
					this.m_targetLandPosition = this.Target.UnitCenter + vector;
					if (this.Target)
					{
						PlayerController playerController = this.Target.gameActor as PlayerController;
						if (flag && playerController)
						{
							Vector2 vector2 = ((!playerController) ? this.Target.Velocity : playerController.AverageVelocity);
							this.m_targetLandPosition += vector2 * (this.HangTime + this.DescentTime);
						}
					}
					IntVector2 intVector = this.Target.UnitCenter.ToIntVector2(VectorConversions.Floor);
					RoomHandler roomFromPosition = GameManager.Instance.Dungeon.data.GetRoomFromPosition(intVector);
					if (roomFromPosition != null)
					{
						this.m_targetLandPosition = Vector2Extensions.Clamp(this.m_targetLandPosition, (roomFromPosition.area.basePosition + IntVector2.One).ToVector2(), (roomFromPosition.area.basePosition + roomFromPosition.area.dimensions - IntVector2.One).ToVector2());
					}
				}
				this.m_landingTarget = SpawnManager.SpawnVFX(this.LandingTargetSprite, this.m_targetLandPosition, Quaternion.identity);
				this.m_landingTarget.GetComponentInChildren<tk2dSprite>().UpdateZDepth();
				tk2dSpriteAnimator componentInChildren = this.m_landingTarget.GetComponentInChildren<tk2dSpriteAnimator>();
				componentInChildren.Play(componentInChildren.DefaultClip, 0f, (float)componentInChildren.DefaultClip.frames.Length / (this.HangTime + this.DescentTime), false);
			}
		}
		else if (this.m_state == SkyRocket.SkyRocketState.Hang)
		{
			base.transform.position = this.m_targetLandPosition + new Vector3(0f, this.MaxHeight, 0f);
			if (this.m_timer <= 0f)
			{
				this.m_timer = this.DescentTime;
				this.m_state = SkyRocket.SkyRocketState.Descend;
				base.transform.localEulerAngles = base.transform.localEulerAngles + new Vector3(0f, 0f, 180f);
				if (!string.IsNullOrEmpty(this.DownSprite))
				{
					base.sprite.SetSprite(this.DownSprite);
				}
			}
		}
		else if (this.m_state == SkyRocket.SkyRocketState.Descend)
		{
			float num3 = 1f - this.DescentCurve.Evaluate(1f - Mathf.Clamp01(this.m_timer / this.DescentTime));
			float num4 = this.MaxHeight - num3 * this.MaxHeight;
			base.transform.position = this.m_targetLandPosition + new Vector3(0f, num4, 0f);
			base.sprite.HeightOffGround = this.m_startHeight + (this.MaxSpriteHeight - num3 * this.MaxSpriteHeight);
			if (this.m_timer <= 0f)
			{
				base.transform.position = this.m_targetLandPosition;
				if (this.DoExplosion)
				{
					Vector3 targetLandPosition = this.m_targetLandPosition;
					ExplosionData explosionData = this.ExplosionData;
					Vector2 zero = Vector2.zero;
					bool ignoreExplosionQueues = this.IgnoreExplosionQueues;
					Exploder.Explode(targetLandPosition, explosionData, zero, null, ignoreExplosionQueues, CoreDamageTypes.None, false);
				}
				this.SpawnVfx.SpawnAtPosition(base.transform.position, 0f, null, null, null, null, false, null, null, false);
				if (this.SpawnObject)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.SpawnObject, base.transform.position, Quaternion.identity);
				}
				SpawnManager.Despawn(this.m_landingTarget);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		base.sprite.HeightOffGround = 2f;
		base.sprite.UpdateZDepth();
	}

	// Token: 0x0600877C RID: 34684 RVA: 0x00383298 File Offset: 0x00381498
	public void DieInAir()
	{
		if (this.m_state == SkyRocket.SkyRocketState.Descend && this.m_timer < 0.5f)
		{
			return;
		}
		if (this.m_landingTarget)
		{
			SpawnManager.Despawn(this.m_landingTarget);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600877D RID: 34685 RVA: 0x003832EC File Offset: 0x003814EC
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x04008CA4 RID: 36004
	public float AscentTime = 1f;

	// Token: 0x04008CA5 RID: 36005
	public AnimationCurve AscentCurve;

	// Token: 0x04008CA6 RID: 36006
	public float HangTime = 1f;

	// Token: 0x04008CA7 RID: 36007
	public float DescentTime = 1f;

	// Token: 0x04008CA8 RID: 36008
	public AnimationCurve DescentCurve;

	// Token: 0x04008CA9 RID: 36009
	public float MaxHeight = 30f;

	// Token: 0x04008CAA RID: 36010
	public string DownSprite = "rocket_white_red_down_001";

	// Token: 0x04008CAB RID: 36011
	public float Variance = 0.25f;

	// Token: 0x04008CAC RID: 36012
	public float LeadPercentage = 0.66f;

	// Token: 0x04008CAD RID: 36013
	public GameObject LandingTargetSprite;

	// Token: 0x04008CAE RID: 36014
	public bool DoExplosion = true;

	// Token: 0x04008CAF RID: 36015
	public ExplosionData ExplosionData;

	// Token: 0x04008CB0 RID: 36016
	public bool IgnoreExplosionQueues;

	// Token: 0x04008CB1 RID: 36017
	public VFXPool SpawnVfx;

	// Token: 0x04008CB2 RID: 36018
	public GameObject SpawnObject;

	// Token: 0x04008CB3 RID: 36019
	private float MaxSpriteHeight = 10f;

	// Token: 0x04008CB4 RID: 36020
	[NonSerialized]
	public SpeculativeRigidbody Target;

	// Token: 0x04008CB5 RID: 36021
	[NonSerialized]
	public Vector2 TargetVector2;

	// Token: 0x04008CB6 RID: 36022
	private Vector3 m_startPosition;

	// Token: 0x04008CB7 RID: 36023
	private float m_startHeight;

	// Token: 0x04008CB8 RID: 36024
	private Vector3 m_targetLandPosition;

	// Token: 0x04008CB9 RID: 36025
	private float m_timer;

	// Token: 0x04008CBA RID: 36026
	private float m_totalDuration;

	// Token: 0x04008CBB RID: 36027
	private GameObject m_landingTarget;

	// Token: 0x04008CBC RID: 36028
	private SkyRocket.SkyRocketState m_state;

	// Token: 0x020016C5 RID: 5829
	public enum SkyRocketState
	{
		// Token: 0x04008CBE RID: 36030
		Ascend,
		// Token: 0x04008CBF RID: 36031
		Hang,
		// Token: 0x04008CC0 RID: 36032
		Descend
	}
}
