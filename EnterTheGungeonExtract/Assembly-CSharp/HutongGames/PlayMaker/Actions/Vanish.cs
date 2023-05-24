using System;
using Dungeonator;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000CDC RID: 3292
	[Tooltip("Removes the NPC with flair.")]
	[ActionCategory(".NPCs")]
	public class Vanish : FsmStateAction
	{
		// Token: 0x060045E2 RID: 17890 RVA: 0x0016AFC4 File Offset: 0x001691C4
		public override void Reset()
		{
			this.delay = 0f;
			this.vanishAnim = string.Empty;
			this.itemsToLeaveBehind = new FsmGameObject[0];
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x0016AFF4 File Offset: 0x001691F4
		public override string ErrorCheck()
		{
			string text = string.Empty;
			tk2dSpriteAnimator component = base.Owner.GetComponent<tk2dSpriteAnimator>();
			AIAnimator component2 = base.Owner.GetComponent<AIAnimator>();
			if (!component && !component2)
			{
				return "Requires a 2D Toolkit animator or an AI Animator.\n";
			}
			if (component2)
			{
				if (!component2.HasDirectionalAnimation(this.vanishAnim.Value))
				{
					text = text + "Unknown animation " + this.vanishAnim.Value + ".\n";
				}
			}
			else if (component && component.GetClipByName(this.vanishAnim.Value) == null)
			{
				text = text + "Unknown animation " + this.vanishAnim.Value + ".\n";
			}
			return text;
		}

		// Token: 0x060045E4 RID: 17892 RVA: 0x0016B0BC File Offset: 0x001692BC
		public override void OnEnter()
		{
			if (this.delay.Value <= 0f)
			{
				this.DoVanish();
				base.Finish();
			}
			else
			{
				this.m_vanishTimer = this.delay.Value;
			}
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x0016B0F8 File Offset: 0x001692F8
		public override void OnUpdate()
		{
			this.m_vanishTimer -= BraveTime.DeltaTime;
			if (this.m_vanishTimer <= 0f)
			{
				this.DoVanish();
				base.Finish();
			}
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x0016B128 File Offset: 0x00169328
		private void DoVanish()
		{
			TalkDoerLite component = base.Owner.GetComponent<TalkDoerLite>();
			RoomHandler roomFromPosition = GameManager.Instance.Dungeon.GetRoomFromPosition(component.transform.position.IntXY(VectorConversions.Floor));
			roomFromPosition.DeregisterInteractable(component);
			component.CloseTextBox(true);
			if (component.specRigidbody != null)
			{
				component.specRigidbody.enabled = false;
			}
			for (int i = 0; i < this.itemsToLeaveBehind.Length; i++)
			{
				this.itemsToLeaveBehind[i].Value.transform.parent = component.transform.parent;
			}
			for (int j = 0; j < component.itemsToLeaveBehind.Count; j++)
			{
				component.itemsToLeaveBehind[j].transform.parent = component.transform.parent;
			}
			if (component.aiAnimator)
			{
				component.aiAnimator.PlayUntilFinished(this.vanishAnim.Value, false, null, -1f, false);
				UnityEngine.Object.Destroy(component.gameObject, component.spriteAnimator.CurrentClip.BaseClipLength);
			}
			else if (component.spriteAnimator && component.spriteAnimator.GetClipByName(this.vanishAnim.Value) != null)
			{
				component.spriteAnimator.PlayAndDestroyObject(this.vanishAnim.Value, null);
			}
			else
			{
				UnityEngine.Object.Destroy(component.gameObject);
			}
		}

		// Token: 0x04003824 RID: 14372
		[Tooltip("Seconds to wait before vanishing (not including the vanish animation).")]
		public FsmFloat delay;

		// Token: 0x04003825 RID: 14373
		[Tooltip("Animation to play before vanishing.")]
		public FsmString vanishAnim;

		// Token: 0x04003826 RID: 14374
		[Tooltip("Add GameObjects here to leave behind after vanishing.")]
		public FsmGameObject[] itemsToLeaveBehind;

		// Token: 0x04003827 RID: 14375
		private float m_vanishTimer;
	}
}
