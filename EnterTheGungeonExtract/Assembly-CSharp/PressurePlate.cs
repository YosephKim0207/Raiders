using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020011E0 RID: 4576
public class PressurePlate : BraveBehaviour
{
	// Token: 0x0600661E RID: 26142 RVA: 0x0027AD6C File Offset: 0x00278F6C
	private void Start()
	{
		this.m_currentDepressors = new HashSet<GameObject>();
		SpeculativeRigidbody specRigidbody = base.specRigidbody;
		specRigidbody.OnEnterTrigger = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(specRigidbody.OnEnterTrigger, new SpeculativeRigidbody.OnTriggerDelegate(this.HandleEnterTriggerCollision));
		SpeculativeRigidbody specRigidbody2 = base.specRigidbody;
		specRigidbody2.OnExitTrigger = (SpeculativeRigidbody.OnTriggerExitDelegate)Delegate.Combine(specRigidbody2.OnExitTrigger, new SpeculativeRigidbody.OnTriggerExitDelegate(this.HandleExitTriggerCollision));
	}

	// Token: 0x0600661F RID: 26143 RVA: 0x0027ADD4 File Offset: 0x00278FD4
	private void HandleEnterTriggerCollision(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody, CollisionData collisionData)
	{
		int count = this.m_currentDepressors.Count;
		if (this.PlayersCanTrigger)
		{
			PlayerController component = specRigidbody.GetComponent<PlayerController>();
			if (component != null)
			{
				if (component.IsDodgeRolling && !component.IsGrounded && !component.IsFlying && GameManager.Instance.IsFoyer)
				{
					this.m_queuedDepressors.Add(component);
					return;
				}
				this.m_currentDepressors.Add(specRigidbody.gameObject);
				if (!this.m_pressed)
				{
					AkSoundEngine.PostEvent("Play_OBJ_plate_press_01", base.gameObject);
				}
			}
		}
		if (this.EnemiesCanTrigger && specRigidbody.GetComponent<AIActor>() != null)
		{
			this.m_currentDepressors.Add(specRigidbody.gameObject);
		}
		if (this.ArbitraryObjectsCanTrigger)
		{
			this.m_currentDepressors.Add(specRigidbody.gameObject);
		}
		int count2 = this.m_currentDepressors.Count;
		this.m_pressed = true;
		if (count == 0 && count2 > 0)
		{
			if (!string.IsNullOrEmpty(this.depressAnimationName))
			{
				base.spriteAnimator.Play(this.depressAnimationName);
			}
			if (this.OnPressurePlateDepressed != null)
			{
				this.OnPressurePlateDepressed(this);
			}
		}
	}

	// Token: 0x06006620 RID: 26144 RVA: 0x0027AF1C File Offset: 0x0027911C
	private void Update()
	{
		if (this.m_queuedDepressors.Count > 0)
		{
			for (int i = 0; i < this.m_queuedDepressors.Count; i++)
			{
				if (!this.m_queuedDepressors[i].IsDodgeRolling || this.m_queuedDepressors[i].IsGrounded)
				{
					this.HandleEnterTriggerCollision(this.m_queuedDepressors[i].specRigidbody, base.specRigidbody, null);
					this.m_queuedDepressors.RemoveAt(i);
					i--;
				}
			}
		}
	}

	// Token: 0x06006621 RID: 26145 RVA: 0x0027AFB0 File Offset: 0x002791B0
	private void HandleExitTriggerCollision(SpeculativeRigidbody specRigidbody, SpeculativeRigidbody sourceSpecRigidbody)
	{
		if (!this.CanUnpress)
		{
			return;
		}
		PlayerController component = specRigidbody.GetComponent<PlayerController>();
		if (component && this.m_queuedDepressors.Contains(component))
		{
			this.m_queuedDepressors.Remove(component);
			return;
		}
		int count = this.m_currentDepressors.Count;
		if (this.m_currentDepressors.Contains(specRigidbody.gameObject))
		{
			this.m_currentDepressors.Remove(specRigidbody.gameObject);
		}
		int count2 = this.m_currentDepressors.Count;
		if (count > 0 && count2 == 0)
		{
			this.m_pressed = false;
			if (!string.IsNullOrEmpty(this.unpressAnimationName))
			{
				base.spriteAnimator.Play(this.unpressAnimationName);
			}
			if (this.OnPressurePlateUnpressed != null)
			{
				this.OnPressurePlateUnpressed(this);
			}
		}
	}

	// Token: 0x06006622 RID: 26146 RVA: 0x0027B088 File Offset: 0x00279288
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x040061EE RID: 25070
	public bool PlayersCanTrigger = true;

	// Token: 0x040061EF RID: 25071
	public bool EnemiesCanTrigger;

	// Token: 0x040061F0 RID: 25072
	public bool ArbitraryObjectsCanTrigger;

	// Token: 0x040061F1 RID: 25073
	public bool CanUnpress = true;

	// Token: 0x040061F2 RID: 25074
	public string depressAnimationName = string.Empty;

	// Token: 0x040061F3 RID: 25075
	public string unpressAnimationName = string.Empty;

	// Token: 0x040061F4 RID: 25076
	public Action<PressurePlate> OnPressurePlateDepressed;

	// Token: 0x040061F5 RID: 25077
	public Action<PressurePlate> OnPressurePlateUnpressed;

	// Token: 0x040061F6 RID: 25078
	private HashSet<GameObject> m_currentDepressors;

	// Token: 0x040061F7 RID: 25079
	private List<PlayerController> m_queuedDepressors = new List<PlayerController>();

	// Token: 0x040061F8 RID: 25080
	private bool m_pressed;
}
