using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200168C RID: 5772
public class TrackInputDirectionalPad : BraveBehaviour
{
	// Token: 0x0600869F RID: 34463 RVA: 0x0037B8C4 File Offset: 0x00379AC4
	private void Awake()
	{
		this.m_gun = base.GetComponent<Gun>();
		this.m_trackedInput = new List<TrackInputDirectionalPad.TrackedKeyInput>();
		this.grappleModule.sourceGameObject = base.gameObject;
	}

	// Token: 0x060086A0 RID: 34464 RVA: 0x0037B8F0 File Offset: 0x00379AF0
	private void Update()
	{
		if (this.m_gun && this.m_gun.CurrentOwner && this.m_gun.CurrentOwner.CurrentGun == this.m_gun)
		{
			this.AddNewInputs();
			this.CheckSequences();
			this.DropOldInputs();
		}
	}

	// Token: 0x060086A1 RID: 34465 RVA: 0x0037B954 File Offset: 0x00379B54
	private void CheckSequences()
	{
		if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.END_TIMES)
		{
			return;
		}
		for (int i = 0; i < this.m_trackedInput.Count; i++)
		{
			TrackInputDirectionalPad.TrackedKeyInput trackedKeyInput = this.m_trackedInput[i];
			if (trackedKeyInput.sourceKey == TrackInputDirectionalPad.TrackInputSequenceKey.DOWN && i < this.m_trackedInput.Count - 2 && this.m_trackedInput[i + 1].sourceKey == TrackInputDirectionalPad.TrackInputSequenceKey.RIGHT && this.m_trackedInput[i + 2].sourceKey == TrackInputDirectionalPad.TrackInputSequenceKey.A)
			{
				this.m_trackedInput.RemoveAt(i + 2);
				this.m_trackedInput.RemoveAt(i + 1);
				this.m_trackedInput.RemoveAt(i);
				i--;
				this.m_hadoukenCounter = 0;
				Gun gun = this.m_gun;
				gun.OnPreFireProjectileModifier = (Func<Gun, Projectile, ProjectileModule, Projectile>)Delegate.Combine(gun.OnPreFireProjectileModifier, new Func<Gun, Projectile, ProjectileModule, Projectile>(this.HadoukenPrefireProjectileModifier));
			}
			if (trackedKeyInput.sourceKey == TrackInputDirectionalPad.TrackInputSequenceKey.LEFT && i < this.m_trackedInput.Count - 2 && this.m_trackedInput[i + 1].sourceKey == TrackInputDirectionalPad.TrackInputSequenceKey.LEFT && this.m_trackedInput[i + 2].sourceKey == TrackInputDirectionalPad.TrackInputSequenceKey.A)
			{
				this.m_trackedInput.RemoveAt(i + 2);
				this.m_trackedInput.RemoveAt(i + 1);
				this.m_trackedInput.RemoveAt(i);
				i--;
				this.grappleModule.ForceEndGrappleImmediate();
				this.grappleModule.Trigger(this.m_gun.CurrentOwner as PlayerController);
			}
		}
	}

	// Token: 0x060086A2 RID: 34466 RVA: 0x0037BAFC File Offset: 0x00379CFC
	private Projectile HadoukenPrefireProjectileModifier(Gun sourceGun, Projectile sourceProjectile, ProjectileModule sourceModule)
	{
		this.m_hadoukenCounter++;
		if (this.m_gun && this.m_hadoukenCounter >= sourceGun.Volley.projectiles.Count)
		{
			Gun gun = this.m_gun;
			gun.OnPreFireProjectileModifier = (Func<Gun, Projectile, ProjectileModule, Projectile>)Delegate.Remove(gun.OnPreFireProjectileModifier, new Func<Gun, Projectile, ProjectileModule, Projectile>(this.HadoukenPrefireProjectileModifier));
		}
		return this.HadoukenProjectile;
	}

	// Token: 0x060086A3 RID: 34467 RVA: 0x0037BB70 File Offset: 0x00379D70
	private void DropOldInputs()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		while (this.m_trackedInput.Count > 0 && realtimeSinceStartup - this.m_trackedInput[0].sourceTime > this.InputLifetime)
		{
			this.m_trackedInput.RemoveAt(0);
		}
	}

	// Token: 0x060086A4 RID: 34468 RVA: 0x0037BBC8 File Offset: 0x00379DC8
	private void AddNewInputs()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (this.m_trackedInput.Count == 0)
		{
			this.m_lastInput = null;
		}
		bool flag = false;
		TrackInputDirectionalPad.TrackedKeyInput trackedKeyInput = new TrackInputDirectionalPad.TrackedKeyInput(TrackInputDirectionalPad.TrackInputSequenceKey.A, realtimeSinceStartup);
		BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer((this.m_gun.CurrentOwner as PlayerController).PlayerIDX);
		if (instanceForPlayer && instanceForPlayer.ActiveActions != null)
		{
			GungeonActions activeActions = instanceForPlayer.ActiveActions;
			if (activeActions.ShootAction.WasPressed)
			{
				flag = true;
				trackedKeyInput = new TrackInputDirectionalPad.TrackedKeyInput(TrackInputDirectionalPad.TrackInputSequenceKey.A, realtimeSinceStartup);
			}
			if (activeActions.DodgeRollAction.WasPressed)
			{
				flag = true;
				trackedKeyInput = new TrackInputDirectionalPad.TrackedKeyInput(TrackInputDirectionalPad.TrackInputSequenceKey.B, realtimeSinceStartup);
			}
			if (!flag)
			{
				Vector2 vector = activeActions.Move.Vector;
				Vector2 majorAxis = BraveUtility.GetMajorAxis(vector);
				if (Mathf.Abs(vector.x) < 0.1f && Mathf.Abs(vector.y) < 0.1f)
				{
					this.m_hasNulledInput = true;
				}
				else if (majorAxis.x > 0f)
				{
					flag = true;
					trackedKeyInput = new TrackInputDirectionalPad.TrackedKeyInput(TrackInputDirectionalPad.TrackInputSequenceKey.RIGHT, realtimeSinceStartup);
				}
				else if (majorAxis.x < 0f)
				{
					flag = true;
					trackedKeyInput = new TrackInputDirectionalPad.TrackedKeyInput(TrackInputDirectionalPad.TrackInputSequenceKey.LEFT, realtimeSinceStartup);
				}
				else if (majorAxis.y > 0f)
				{
					flag = true;
					trackedKeyInput = new TrackInputDirectionalPad.TrackedKeyInput(TrackInputDirectionalPad.TrackInputSequenceKey.UP, realtimeSinceStartup);
				}
				else if (majorAxis.y < 0f)
				{
					flag = true;
					trackedKeyInput = new TrackInputDirectionalPad.TrackedKeyInput(TrackInputDirectionalPad.TrackInputSequenceKey.DOWN, realtimeSinceStartup);
				}
			}
		}
		if (flag && !this.m_hasNulledInput && this.m_lastInput != null && this.m_lastInput.Value.sourceKey == trackedKeyInput.sourceKey)
		{
			flag = false;
		}
		if (flag)
		{
			this.m_trackedInput.Add(trackedKeyInput);
			this.m_lastInput = new TrackInputDirectionalPad.TrackedKeyInput?(trackedKeyInput);
			this.m_hasNulledInput = false;
		}
	}

	// Token: 0x04008B98 RID: 35736
	public float InputLifetime = 2f;

	// Token: 0x04008B99 RID: 35737
	public Projectile HadoukenProjectile;

	// Token: 0x04008B9A RID: 35738
	public GrappleModule grappleModule;

	// Token: 0x04008B9B RID: 35739
	private List<TrackInputDirectionalPad.TrackedKeyInput> m_trackedInput;

	// Token: 0x04008B9C RID: 35740
	private TrackInputDirectionalPad.TrackedKeyInput? m_lastInput;

	// Token: 0x04008B9D RID: 35741
	private bool m_hasNulledInput;

	// Token: 0x04008B9E RID: 35742
	private Gun m_gun;

	// Token: 0x04008B9F RID: 35743
	private int m_hadoukenCounter;

	// Token: 0x0200168D RID: 5773
	public enum TrackInputSequenceKey
	{
		// Token: 0x04008BA1 RID: 35745
		UP,
		// Token: 0x04008BA2 RID: 35746
		RIGHT,
		// Token: 0x04008BA3 RID: 35747
		DOWN,
		// Token: 0x04008BA4 RID: 35748
		LEFT,
		// Token: 0x04008BA5 RID: 35749
		A,
		// Token: 0x04008BA6 RID: 35750
		B
	}

	// Token: 0x0200168E RID: 5774
	protected struct TrackedKeyInput
	{
		// Token: 0x060086A5 RID: 34469 RVA: 0x0037BDB8 File Offset: 0x00379FB8
		public TrackedKeyInput(TrackInputDirectionalPad.TrackInputSequenceKey key, float t)
		{
			this.sourceKey = key;
			this.sourceTime = t;
		}

		// Token: 0x04008BA7 RID: 35751
		public TrackInputDirectionalPad.TrackInputSequenceKey sourceKey;

		// Token: 0x04008BA8 RID: 35752
		public float sourceTime;
	}
}
