using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000AAF RID: 2735
	[Tooltip("Base class for actions that want to run a sub FSM.")]
	public abstract class RunFSMAction : FsmStateAction
	{
		// Token: 0x060039FD RID: 14845 RVA: 0x00127970 File Offset: 0x00125B70
		public override void Reset()
		{
			this.runFsm = null;
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x0012797C File Offset: 0x00125B7C
		public override bool Event(FsmEvent fsmEvent)
		{
			if (this.runFsm != null && (fsmEvent.IsGlobal || fsmEvent.IsSystemEvent))
			{
				this.runFsm.Event(fsmEvent);
			}
			return false;
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x001279AC File Offset: 0x00125BAC
		public override void OnEnter()
		{
			if (this.runFsm == null)
			{
				base.Finish();
				return;
			}
			this.runFsm.OnEnable();
			if (!this.runFsm.Started)
			{
				this.runFsm.Start();
			}
			this.CheckIfFinished();
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x001279EC File Offset: 0x00125BEC
		public override void OnUpdate()
		{
			if (this.runFsm != null)
			{
				this.runFsm.Update();
				this.CheckIfFinished();
			}
			else
			{
				base.Finish();
			}
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00127A18 File Offset: 0x00125C18
		public override void OnFixedUpdate()
		{
			if (this.runFsm != null)
			{
				this.runFsm.FixedUpdate();
				this.CheckIfFinished();
			}
			else
			{
				base.Finish();
			}
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x00127A44 File Offset: 0x00125C44
		public override void OnLateUpdate()
		{
			if (this.runFsm != null)
			{
				this.runFsm.LateUpdate();
				this.CheckIfFinished();
			}
			else
			{
				base.Finish();
			}
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x00127A70 File Offset: 0x00125C70
		public override void DoTriggerEnter(Collider other)
		{
			if (this.runFsm.HandleTriggerEnter)
			{
				this.runFsm.OnTriggerEnter(other);
			}
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x00127A90 File Offset: 0x00125C90
		public override void DoTriggerStay(Collider other)
		{
			if (this.runFsm.HandleTriggerStay)
			{
				this.runFsm.OnTriggerStay(other);
			}
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x00127AB0 File Offset: 0x00125CB0
		public override void DoTriggerExit(Collider other)
		{
			if (this.runFsm.HandleTriggerExit)
			{
				this.runFsm.OnTriggerExit(other);
			}
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x00127AD0 File Offset: 0x00125CD0
		public override void DoCollisionEnter(Collision collisionInfo)
		{
			if (this.runFsm.HandleCollisionEnter)
			{
				this.runFsm.OnCollisionEnter(collisionInfo);
			}
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x00127AF0 File Offset: 0x00125CF0
		public override void DoCollisionStay(Collision collisionInfo)
		{
			if (this.runFsm.HandleCollisionStay)
			{
				this.runFsm.OnCollisionStay(collisionInfo);
			}
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x00127B10 File Offset: 0x00125D10
		public override void DoCollisionExit(Collision collisionInfo)
		{
			if (this.runFsm.HandleCollisionExit)
			{
				this.runFsm.OnCollisionExit(collisionInfo);
			}
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x00127B30 File Offset: 0x00125D30
		public override void DoParticleCollision(GameObject other)
		{
			if (this.runFsm.HandleParticleCollision)
			{
				this.runFsm.OnParticleCollision(other);
			}
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x00127B50 File Offset: 0x00125D50
		public override void DoControllerColliderHit(ControllerColliderHit collisionInfo)
		{
			this.runFsm.OnControllerColliderHit(collisionInfo);
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x00127B60 File Offset: 0x00125D60
		public override void DoTriggerEnter2D(Collider2D other)
		{
			if (this.runFsm.HandleTriggerEnter)
			{
				this.runFsm.OnTriggerEnter2D(other);
			}
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x00127B80 File Offset: 0x00125D80
		public override void DoTriggerStay2D(Collider2D other)
		{
			if (this.runFsm.HandleTriggerStay)
			{
				this.runFsm.OnTriggerStay2D(other);
			}
		}

		// Token: 0x06003A0D RID: 14861 RVA: 0x00127BA0 File Offset: 0x00125DA0
		public override void DoTriggerExit2D(Collider2D other)
		{
			if (this.runFsm.HandleTriggerExit)
			{
				this.runFsm.OnTriggerExit2D(other);
			}
		}

		// Token: 0x06003A0E RID: 14862 RVA: 0x00127BC0 File Offset: 0x00125DC0
		public override void DoCollisionEnter2D(Collision2D collisionInfo)
		{
			if (this.runFsm.HandleCollisionEnter)
			{
				this.runFsm.OnCollisionEnter2D(collisionInfo);
			}
		}

		// Token: 0x06003A0F RID: 14863 RVA: 0x00127BE0 File Offset: 0x00125DE0
		public override void DoCollisionStay2D(Collision2D collisionInfo)
		{
			if (this.runFsm.HandleCollisionStay)
			{
				this.runFsm.OnCollisionStay2D(collisionInfo);
			}
		}

		// Token: 0x06003A10 RID: 14864 RVA: 0x00127C00 File Offset: 0x00125E00
		public override void DoCollisionExit2D(Collision2D collisionInfo)
		{
			if (this.runFsm.HandleCollisionExit)
			{
				this.runFsm.OnCollisionExit2D(collisionInfo);
			}
		}

		// Token: 0x06003A11 RID: 14865 RVA: 0x00127C20 File Offset: 0x00125E20
		public override void OnGUI()
		{
			if (this.runFsm != null && this.runFsm.HandleOnGUI)
			{
				this.runFsm.OnGUI();
			}
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x00127C48 File Offset: 0x00125E48
		public override void OnExit()
		{
			if (this.runFsm != null)
			{
				this.runFsm.Stop();
			}
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x00127C60 File Offset: 0x00125E60
		protected virtual void CheckIfFinished()
		{
			if (this.runFsm == null || this.runFsm.Finished)
			{
				base.Finish();
			}
		}

		// Token: 0x04002C3A RID: 11322
		protected Fsm runFsm;
	}
}
