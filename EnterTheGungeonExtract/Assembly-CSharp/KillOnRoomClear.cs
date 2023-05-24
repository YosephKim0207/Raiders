using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020010A9 RID: 4265
public class KillOnRoomClear : BraveBehaviour
{
	// Token: 0x06005E08 RID: 24072 RVA: 0x00241170 File Offset: 0x0023F370
	public void Start()
	{
		RoomHandler parentRoom = base.aiActor.ParentRoom;
		parentRoom.OnEnemiesCleared = (Action)Delegate.Combine(parentRoom.OnEnemiesCleared, new Action(this.RoomCleared));
	}

	// Token: 0x06005E09 RID: 24073 RVA: 0x002411A0 File Offset: 0x0023F3A0
	protected override void OnDestroy()
	{
		if (base.aiActor && base.aiActor.ParentRoom != null)
		{
			RoomHandler parentRoom = base.aiActor.ParentRoom;
			parentRoom.OnEnemiesCleared = (Action)Delegate.Remove(parentRoom.OnEnemiesCleared, new Action(this.RoomCleared));
		}
		base.OnDestroy();
	}

	// Token: 0x06005E0A RID: 24074 RVA: 0x00241200 File Offset: 0x0023F400
	private void RoomCleared()
	{
		if (!string.IsNullOrEmpty(this.overrideDeathAnim) && base.aiAnimator)
		{
			bool flag = false;
			for (int i = 0; i < base.aiAnimator.OtherAnimations.Count; i++)
			{
				if (base.aiAnimator.OtherAnimations[i].name == "death")
				{
					base.aiAnimator.OtherAnimations[i].anim.Type = DirectionalAnimation.DirectionType.Single;
					base.aiAnimator.OtherAnimations[i].anim.Prefix = this.overrideDeathAnim;
					flag = true;
				}
			}
			if (!flag)
			{
				AIAnimator.NamedDirectionalAnimation namedDirectionalAnimation = new AIAnimator.NamedDirectionalAnimation();
				namedDirectionalAnimation.name = "death";
				namedDirectionalAnimation.anim = new DirectionalAnimation();
				namedDirectionalAnimation.anim.Type = DirectionalAnimation.DirectionType.Single;
				namedDirectionalAnimation.anim.Prefix = this.overrideDeathAnim;
				namedDirectionalAnimation.anim.Flipped = new DirectionalAnimation.FlipType[1];
				base.aiAnimator.OtherAnimations.Add(namedDirectionalAnimation);
			}
		}
		if (this.preventExplodeOnDeath)
		{
			ExplodeOnDeath component = base.GetComponent<ExplodeOnDeath>();
			if (component)
			{
				component.enabled = false;
			}
		}
		base.healthHaver.PreventAllDamage = false;
		base.healthHaver.ApplyDamage(100000f, Vector2.zero, "Death on Room Claer", CoreDamageTypes.None, DamageCategory.Unstoppable, true, null, false);
	}

	// Token: 0x04005820 RID: 22560
	[CheckAnimation(null)]
	public string overrideDeathAnim;

	// Token: 0x04005821 RID: 22561
	public bool preventExplodeOnDeath;
}
