using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001720 RID: 5920
public class BasicTrapController : TrapController, IPlaceConfigurable
{
	// Token: 0x1700146F RID: 5231
	// (get) Token: 0x0600897F RID: 35199 RVA: 0x003927DC File Offset: 0x003909DC
	// (set) Token: 0x06008980 RID: 35200 RVA: 0x003927E4 File Offset: 0x003909E4
	protected BasicTrapController.State state
	{
		get
		{
			return this.m_state;
		}
		set
		{
			if (this.m_state != value)
			{
				this.EndState(this.m_state);
				this.m_state = value;
				this.BeginState(this.m_state);
			}
		}
	}

	// Token: 0x06008981 RID: 35201 RVA: 0x00392814 File Offset: 0x00390A14
	public virtual void Awake()
	{
		if (base.specRigidbody)
		{
			SpeculativeRigidbody specRigidbody = base.specRigidbody;
			specRigidbody.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnTriggerCollision));
		}
		if (this.animateChildren)
		{
			this.m_childrenAnimators = base.GetComponentsInChildren<tk2dSpriteAnimator>();
		}
		if (this.triggerOnBlank || this.triggerOnExplosion)
		{
			StaticReferenceManager.AllTriggeredTraps.Add(this);
		}
	}

	// Token: 0x06008982 RID: 35202 RVA: 0x00392890 File Offset: 0x00390A90
	public override void Start()
	{
		this.m_parentRoom = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(base.transform.position.IntXY(VectorConversions.Round));
		this.m_cachedPosition = base.transform.position.IntXY(VectorConversions.Floor);
		this.m_cachedPixelMin = this.m_cachedPosition * PhysicsEngine.Instance.PixelsPerUnit + new IntVector2(this.footprintBuffer.left, this.footprintBuffer.bottom);
		this.m_cachedPixelMax = (this.m_cachedPosition + new IntVector2(this.placeableWidth, this.placeableHeight)) * PhysicsEngine.Instance.PixelsPerUnit - IntVector2.One - new IntVector2(this.footprintBuffer.right, this.footprintBuffer.top);
		if (this.triggerMethod == BasicTrapController.TriggerMethod.Timer)
		{
			this.m_triggerTimerDelayArray = new List<float>();
			if (this.triggerTimerDelay != 0f)
			{
				this.m_triggerTimerDelayArray.Add(this.triggerTimerDelay);
			}
			if (this.triggerTimerDelay1 != 0f)
			{
				this.m_triggerTimerDelayArray.Add(this.triggerTimerDelay1);
			}
			if (this.m_triggerTimerDelayArray.Count == 0)
			{
				this.m_triggerTimerDelayArray.Add(0f);
			}
			this.m_triggerTimer = this.triggerTimerOffset;
		}
		for (int i = 0; i < this.activeVfx.Count; i++)
		{
			if (this.activeVfx[i])
			{
				this.activeVfx[i].onlyDisable = true;
				this.activeVfx[i].Disable();
			}
		}
		base.Start();
	}

	// Token: 0x06008983 RID: 35203 RVA: 0x00392A58 File Offset: 0x00390C58
	public virtual void Update()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}
		if (!GameManager.Instance.PlayerIsNearRoom(this.m_parentRoom))
		{
			return;
		}
		this.m_stateTimer = Mathf.Max(0f, this.m_stateTimer - BraveTime.DeltaTime) * this.LocalTimeScale;
		this.m_triggerTimer -= BraveTime.DeltaTime * this.LocalTimeScale;
		this.m_disabledTimer = Mathf.Max(0f, this.m_disabledTimer - BraveTime.DeltaTime * this.LocalTimeScale);
		if (this.triggerMethod == BasicTrapController.TriggerMethod.Timer && this.m_triggerTimer < 0f)
		{
			this.TriggerTrap(null);
		}
		this.UpdateState();
	}

	// Token: 0x06008984 RID: 35204 RVA: 0x00392B14 File Offset: 0x00390D14
	protected override void OnDestroy()
	{
		if (this.triggerOnBlank || this.triggerOnExplosion)
		{
			StaticReferenceManager.AllTriggeredTraps.Remove(this);
		}
		base.OnDestroy();
	}

	// Token: 0x06008985 RID: 35205 RVA: 0x00392B40 File Offset: 0x00390D40
	public override GameObject InstantiateObject(RoomHandler targetRoom, IntVector2 loc, bool deferConfiguration = false)
	{
		return base.InstantiateObject(targetRoom, loc, deferConfiguration);
	}

	// Token: 0x06008986 RID: 35206 RVA: 0x00392B4C File Offset: 0x00390D4C
	private void OnTriggerCollision(SpeculativeRigidbody rigidbody, SpeculativeRigidbody source, CollisionData collisionData)
	{
		PlayerController component = rigidbody.GetComponent<PlayerController>();
		if (component)
		{
			bool flag = component.spriteAnimator.QueryGroundedFrame() && !component.IsFlying;
			if (this.triggerMethod == BasicTrapController.TriggerMethod.SpecRigidbody && this.m_state == BasicTrapController.State.Ready && flag)
			{
				this.TriggerTrap(rigidbody);
			}
			if (this.damageMethod == BasicTrapController.DamageMethod.SpecRigidbody && this.m_state == BasicTrapController.State.Active && (flag || this.damagesFlyingPlayers))
			{
				this.Damage(rigidbody);
			}
		}
	}

	// Token: 0x06008987 RID: 35207 RVA: 0x00392BDC File Offset: 0x00390DDC
	public void Trigger()
	{
		this.TriggerTrap(null);
	}

	// Token: 0x06008988 RID: 35208 RVA: 0x00392BE8 File Offset: 0x00390DE8
	protected virtual void TriggerTrap(SpeculativeRigidbody target)
	{
		if (this.m_disabledTimer > 0f)
		{
			return;
		}
		if (this.m_state == BasicTrapController.State.Ready)
		{
			this.state = BasicTrapController.State.Triggered;
			if (this.damageMethod == BasicTrapController.DamageMethod.OnTrigger)
			{
				this.Damage(target);
			}
		}
	}

	// Token: 0x06008989 RID: 35209 RVA: 0x00392C20 File Offset: 0x00390E20
	protected bool ArePlayersNearby()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].CurrentRoom == this.m_parentRoom)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600898A RID: 35210 RVA: 0x00392C80 File Offset: 0x00390E80
	protected bool ArePlayersSortOfNearby()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			if (GameManager.Instance.AllPlayers[i] && GameManager.Instance.AllPlayers[i].CurrentRoom != null && GameManager.Instance.AllPlayers[i].CurrentRoom.connectedRooms != null && GameManager.Instance.AllPlayers[i].CurrentRoom.connectedRooms.Contains(this.m_parentRoom))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600898B RID: 35211 RVA: 0x00392D1C File Offset: 0x00390F1C
	protected virtual void BeginState(BasicTrapController.State newState)
	{
		bool flag = this.ArePlayersNearby();
		bool flag2 = flag || this.ArePlayersSortOfNearby();
		if (this.m_state == BasicTrapController.State.Triggered)
		{
			this.PlayAnimation(this.triggerAnimName);
			this.m_stateTimer = this.triggerDelay;
			if (this.triggerMethod == BasicTrapController.TriggerMethod.Timer)
			{
				this.m_triggerTimer += this.GetNextTriggerTimerDelay();
			}
			if (this.m_stateTimer == 0f)
			{
				this.state = BasicTrapController.State.Active;
			}
			if (flag)
			{
				AkSoundEngine.PostEvent("Play_ENV_trap_trigger", base.gameObject);
			}
		}
		else if (this.m_state == BasicTrapController.State.Active)
		{
			this.PlayAnimation(this.activeAnimName);
			if (flag2)
			{
				this.SpawnVfx(this.activeVfx);
			}
			this.m_stateTimer = this.activeTime;
			if (this.m_stateTimer == 0f)
			{
				this.state = BasicTrapController.State.Resetting;
			}
			if (flag)
			{
				AkSoundEngine.PostEvent("Play_ENV_trap_active", base.gameObject);
			}
		}
		else if (this.m_state == BasicTrapController.State.Resetting)
		{
			this.PlayAnimation(this.resetAnimName);
			this.m_stateTimer = this.resetDelay;
			if (this.m_stateTimer == 0f)
			{
				this.state = BasicTrapController.State.Ready;
			}
			if (flag)
			{
				AkSoundEngine.PostEvent("Play_ENV_trap_reset", base.gameObject);
			}
		}
	}

	// Token: 0x0600898C RID: 35212 RVA: 0x00392E74 File Offset: 0x00391074
	protected virtual void UpdateState()
	{
		if (this.m_state == BasicTrapController.State.Ready)
		{
			if (this.triggerMethod == BasicTrapController.TriggerMethod.PlaceableFootprint)
			{
				SpeculativeRigidbody playerRigidbodyInFootprint = this.GetPlayerRigidbodyInFootprint();
				if (playerRigidbodyInFootprint)
				{
					bool flag = playerRigidbodyInFootprint.spriteAnimator.QueryGroundedFrame();
					if (playerRigidbodyInFootprint.gameActor != null)
					{
						flag = flag && !playerRigidbodyInFootprint.gameActor.IsFlying;
					}
					if (flag)
					{
						this.TriggerTrap(null);
					}
				}
			}
		}
		else if (this.m_state == BasicTrapController.State.Triggered)
		{
			if (this.m_stateTimer == 0f)
			{
				this.state = BasicTrapController.State.Active;
			}
		}
		else if (this.m_state == BasicTrapController.State.Active)
		{
			if (this.damageMethod == BasicTrapController.DamageMethod.PlaceableFootprint)
			{
				SpeculativeRigidbody playerRigidbodyInFootprint2 = this.GetPlayerRigidbodyInFootprint();
				if (playerRigidbodyInFootprint2)
				{
					bool flag2 = playerRigidbodyInFootprint2.spriteAnimator.QueryGroundedFrame();
					if (playerRigidbodyInFootprint2.gameActor != null)
					{
						flag2 = flag2 && !playerRigidbodyInFootprint2.gameActor.IsFlying;
					}
					if (flag2 || this.damagesFlyingPlayers)
					{
						this.Damage(playerRigidbodyInFootprint2);
					}
				}
			}
			if (this.IgnitesGoop)
			{
				DeadlyDeadlyGoopManager.IgniteGoopsCircle(base.sprite.WorldCenter, 1f);
			}
			if (this.m_stateTimer == 0f)
			{
				this.state = BasicTrapController.State.Resetting;
			}
		}
		else if (this.m_state == BasicTrapController.State.Resetting && this.m_stateTimer == 0f)
		{
			this.state = BasicTrapController.State.Ready;
		}
	}

	// Token: 0x0600898D RID: 35213 RVA: 0x00392FF0 File Offset: 0x003911F0
	protected virtual void EndState(BasicTrapController.State newState)
	{
	}

	// Token: 0x0600898E RID: 35214 RVA: 0x00392FF4 File Offset: 0x003911F4
	public void TemporarilyDisableTrap(float disableTime)
	{
		this.m_disabledTimer = Mathf.Max(disableTime, this.m_disabledTimer);
	}

	// Token: 0x0600898F RID: 35215 RVA: 0x00393008 File Offset: 0x00391208
	public Vector2 CenterPoint()
	{
		if (base.specRigidbody)
		{
			return base.specRigidbody.UnitCenter;
		}
		if (this.triggerMethod == BasicTrapController.TriggerMethod.PlaceableFootprint)
		{
			return new Vector2((float)(this.m_cachedPixelMin.x + this.m_cachedPixelMax.x), (float)(this.m_cachedPixelMin.y + this.m_cachedPixelMax.y)) / 32f;
		}
		return base.transform.position;
	}

	// Token: 0x06008990 RID: 35216 RVA: 0x00393090 File Offset: 0x00391290
	protected virtual void PlayAnimation(string animationName)
	{
		if (string.IsNullOrEmpty(animationName))
		{
			return;
		}
		if (this.animateChildren)
		{
			if (this.m_childrenAnimators != null)
			{
				for (int i = 0; i < this.m_childrenAnimators.Length; i++)
				{
					if (base.spriteAnimator != this.m_childrenAnimators[i])
					{
						this.m_childrenAnimators[i].Play(animationName);
					}
				}
			}
		}
		else
		{
			base.spriteAnimator.Play(animationName);
		}
	}

	// Token: 0x06008991 RID: 35217 RVA: 0x00393110 File Offset: 0x00391310
	protected virtual void SpawnVfx(List<SpriteAnimatorKiller> vfx)
	{
		for (int i = 0; i < vfx.Count; i++)
		{
			if (vfx[i])
			{
				vfx[i].Restart();
			}
		}
	}

	// Token: 0x06008992 RID: 35218 RVA: 0x00393154 File Offset: 0x00391354
	protected virtual SpeculativeRigidbody GetPlayerRigidbodyInFootprint()
	{
		for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
		{
			PlayerController playerController = GameManager.Instance.AllPlayers[i];
			if (!(playerController == null))
			{
				PixelCollider primaryPixelCollider = playerController.specRigidbody.PrimaryPixelCollider;
				if (primaryPixelCollider != null)
				{
					if (this.m_cachedPixelMin.x <= primaryPixelCollider.MaxX && this.m_cachedPixelMax.x >= primaryPixelCollider.MinX && this.m_cachedPixelMin.y <= primaryPixelCollider.MaxY && this.m_cachedPixelMax.y >= primaryPixelCollider.MinY)
					{
						return playerController.specRigidbody;
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06008993 RID: 35219 RVA: 0x00393218 File Offset: 0x00391418
	protected virtual void Damage(SpeculativeRigidbody rigidbody)
	{
		if (this.damage > 0f && rigidbody && rigidbody.healthHaver && rigidbody.healthHaver.IsVulnerable)
		{
			if (rigidbody.gameActor && rigidbody.gameActor.IsFalling)
			{
				return;
			}
			rigidbody.healthHaver.ApplyDamage(this.damage, Vector2.zero, StringTableManager.GetEnemiesString("#TRAP", -1), this.damageTypes, DamageCategory.Normal, false, null, false);
		}
	}

	// Token: 0x06008994 RID: 35220 RVA: 0x003932AC File Offset: 0x003914AC
	protected float GetNextTriggerTimerDelay()
	{
		float num = this.m_triggerTimerDelayArray[this.m_triggerTimerDelayIndex];
		this.m_triggerTimerDelayIndex = (this.m_triggerTimerDelayIndex + 1) % this.m_triggerTimerDelayArray.Count;
		return num;
	}

	// Token: 0x06008995 RID: 35221 RVA: 0x003932E8 File Offset: 0x003914E8
	public void ConfigureOnPlacement(RoomHandler room)
	{
		IntVector2 intVector = base.transform.position.IntXY(VectorConversions.Floor);
		for (int i = 0; i < this.placeableWidth; i++)
		{
			for (int j = 0; j < this.placeableHeight; j++)
			{
				IntVector2 intVector2 = new IntVector2(i, j) + intVector;
				GameManager.Instance.Dungeon.data[intVector2].cellVisualData.containsObjectSpaceStamp = true;
				GameManager.Instance.Dungeon.data[intVector2].cellVisualData.containsWallSpaceStamp = true;
			}
		}
		room.ForcePreventChannels = true;
	}

	// Token: 0x04008FB2 RID: 36786
	public BasicTrapController.TriggerMethod triggerMethod;

	// Token: 0x04008FB3 RID: 36787
	[DwarfConfigurable]
	[ShowInInspectorIf("triggerMethod", 2, false)]
	public float triggerTimerDelay = 1f;

	// Token: 0x04008FB4 RID: 36788
	[ShowInInspectorIf("triggerMethod", 2, false)]
	[DwarfConfigurable]
	public float triggerTimerDelay1;

	// Token: 0x04008FB5 RID: 36789
	[ShowInInspectorIf("triggerMethod", 2, false)]
	[DwarfConfigurable]
	public float triggerTimerOffset;

	// Token: 0x04008FB6 RID: 36790
	public BasicTrapController.PlaceableFootprintBuffer footprintBuffer;

	// Token: 0x04008FB7 RID: 36791
	public bool damagesFlyingPlayers;

	// Token: 0x04008FB8 RID: 36792
	public bool triggerOnBlank;

	// Token: 0x04008FB9 RID: 36793
	public bool triggerOnExplosion;

	// Token: 0x04008FBA RID: 36794
	[Header("Animations")]
	public bool animateChildren;

	// Token: 0x04008FBB RID: 36795
	[CheckAnimation(null)]
	public string triggerAnimName;

	// Token: 0x04008FBC RID: 36796
	public float triggerDelay;

	// Token: 0x04008FBD RID: 36797
	[CheckAnimation(null)]
	public string activeAnimName;

	// Token: 0x04008FBE RID: 36798
	public List<SpriteAnimatorKiller> activeVfx;

	// Token: 0x04008FBF RID: 36799
	public float activeTime;

	// Token: 0x04008FC0 RID: 36800
	[CheckAnimation(null)]
	public string resetAnimName;

	// Token: 0x04008FC1 RID: 36801
	public float resetDelay;

	// Token: 0x04008FC2 RID: 36802
	[Header("Damage")]
	public BasicTrapController.DamageMethod damageMethod;

	// Token: 0x04008FC3 RID: 36803
	[FormerlySerializedAs("activeDamage")]
	public float damage;

	// Token: 0x04008FC4 RID: 36804
	public CoreDamageTypes damageTypes;

	// Token: 0x04008FC5 RID: 36805
	[Header("Goop Interactions")]
	public bool IgnitesGoop;

	// Token: 0x04008FC6 RID: 36806
	[NonSerialized]
	public float LocalTimeScale = 1f;

	// Token: 0x04008FC7 RID: 36807
	private RoomHandler m_parentRoom;

	// Token: 0x04008FC8 RID: 36808
	private BasicTrapController.State m_state;

	// Token: 0x04008FC9 RID: 36809
	protected float m_stateTimer;

	// Token: 0x04008FCA RID: 36810
	protected float m_triggerTimer;

	// Token: 0x04008FCB RID: 36811
	protected float m_disabledTimer;

	// Token: 0x04008FCC RID: 36812
	protected IntVector2 m_cachedPosition;

	// Token: 0x04008FCD RID: 36813
	protected IntVector2 m_cachedPixelMin;

	// Token: 0x04008FCE RID: 36814
	protected IntVector2 m_cachedPixelMax;

	// Token: 0x04008FCF RID: 36815
	protected tk2dSpriteAnimator[] m_childrenAnimators;

	// Token: 0x04008FD0 RID: 36816
	protected List<float> m_triggerTimerDelayArray;

	// Token: 0x04008FD1 RID: 36817
	protected int m_triggerTimerDelayIndex;

	// Token: 0x02001721 RID: 5921
	public enum TriggerMethod
	{
		// Token: 0x04008FD3 RID: 36819
		SpecRigidbody,
		// Token: 0x04008FD4 RID: 36820
		PlaceableFootprint,
		// Token: 0x04008FD5 RID: 36821
		Timer,
		// Token: 0x04008FD6 RID: 36822
		Script
	}

	// Token: 0x02001722 RID: 5922
	public enum DamageMethod
	{
		// Token: 0x04008FD8 RID: 36824
		SpecRigidbody,
		// Token: 0x04008FD9 RID: 36825
		PlaceableFootprint,
		// Token: 0x04008FDA RID: 36826
		OnTrigger
	}

	// Token: 0x02001723 RID: 5923
	protected enum State
	{
		// Token: 0x04008FDC RID: 36828
		Ready,
		// Token: 0x04008FDD RID: 36829
		Triggered,
		// Token: 0x04008FDE RID: 36830
		Active,
		// Token: 0x04008FDF RID: 36831
		Resetting
	}

	// Token: 0x02001724 RID: 5924
	[Serializable]
	public class PlaceableFootprintBuffer
	{
		// Token: 0x04008FE0 RID: 36832
		public int left;

		// Token: 0x04008FE1 RID: 36833
		public int bottom;

		// Token: 0x04008FE2 RID: 36834
		public int right;

		// Token: 0x04008FE3 RID: 36835
		public int top;
	}
}
