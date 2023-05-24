using System;
using Dungeonator;
using UnityEngine;

// Token: 0x020012D3 RID: 4819
[Serializable]
public class FlippableSubElement
{
	// Token: 0x06006BE8 RID: 27624 RVA: 0x002A72D0 File Offset: 0x002A54D0
	public void Trigger(DungeonData.Direction flipDirection, tk2dBaseSprite sourceTable)
	{
		if (this.requiresDirection && this.requiredDirection != flipDirection)
		{
			return;
		}
		if (this.elementStyle == FlippableSubElement.SubElementStyle.ANIMATOR)
		{
			this.targetAnimator.gameObject.SetActive(true);
			string empty = string.Empty;
			switch (flipDirection)
			{
			case DungeonData.Direction.NORTH:
				empty = this.northAnimation;
				break;
			case DungeonData.Direction.EAST:
				empty = this.eastAnimation;
				break;
			case DungeonData.Direction.SOUTH:
				empty = this.southAnimation;
				break;
			case DungeonData.Direction.WEST:
				empty = this.westAnimation;
				break;
			}
			if (string.IsNullOrEmpty(empty))
			{
				this.targetAnimator.Play();
			}
			else
			{
				this.targetAnimator.Play(empty);
			}
			tk2dSpriteAnimator tk2dSpriteAnimator = this.targetAnimator;
			tk2dSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Combine(tk2dSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleted));
		}
		else if (this.elementStyle == FlippableSubElement.SubElementStyle.GOOP)
		{
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(this.goopToUse);
			Vector2 vector = sourceTable.WorldCenter;
			if (flipDirection == DungeonData.Direction.EAST || flipDirection == DungeonData.Direction.WEST)
			{
				vector += new Vector2(0f, -0.5f);
			}
			goopManagerForGoopType.TimedAddGoopArc(vector, this.goopConeLength, this.goopConeArc, DungeonData.GetIntVector2FromDirection(flipDirection).ToVector2(), this.goopDuration, this.goopCurve);
		}
	}

	// Token: 0x06006BE9 RID: 27625 RVA: 0x002A7434 File Offset: 0x002A5634
	private void AnimationCompleted(tk2dSpriteAnimator source, tk2dSpriteAnimationClip clerp)
	{
		tk2dSpriteAnimator tk2dSpriteAnimator = this.targetAnimator;
		tk2dSpriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(tk2dSpriteAnimator.AnimationCompleted, new Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>(this.AnimationCompleted));
		source.Sprite.IsPerpendicular = false;
		source.Sprite.HeightOffGround = -1f + this.additionalHeightModification;
		source.Sprite.UpdateZDepth();
	}

	// Token: 0x040068D4 RID: 26836
	public FlippableSubElement.SubElementStyle elementStyle;

	// Token: 0x040068D5 RID: 26837
	public bool isMandatory;

	// Token: 0x040068D6 RID: 26838
	public bool onlyOneOfThese;

	// Token: 0x040068D7 RID: 26839
	public float spawnChance = 1f;

	// Token: 0x040068D8 RID: 26840
	public float flipDelay;

	// Token: 0x040068D9 RID: 26841
	public bool requiresDirection;

	// Token: 0x040068DA RID: 26842
	public DungeonData.Direction requiredDirection;

	// Token: 0x040068DB RID: 26843
	[ShowInInspectorIf("elementStyle", 0, false)]
	public tk2dSpriteAnimator targetAnimator;

	// Token: 0x040068DC RID: 26844
	[ShowInInspectorIf("elementStyle", 0, false)]
	public string northAnimation;

	// Token: 0x040068DD RID: 26845
	[ShowInInspectorIf("elementStyle", 0, false)]
	public string eastAnimation;

	// Token: 0x040068DE RID: 26846
	[ShowInInspectorIf("elementStyle", 0, false)]
	public string southAnimation;

	// Token: 0x040068DF RID: 26847
	[ShowInInspectorIf("elementStyle", 0, false)]
	public string westAnimation;

	// Token: 0x040068E0 RID: 26848
	[ShowInInspectorIf("elementStyle", 0, false)]
	public float additionalHeightModification;

	// Token: 0x040068E1 RID: 26849
	[ShowInInspectorIf("elementStyle", 1, false)]
	public GoopDefinition goopToUse;

	// Token: 0x040068E2 RID: 26850
	[ShowInInspectorIf("elementStyle", 1, false)]
	public float goopConeLength = 5f;

	// Token: 0x040068E3 RID: 26851
	[ShowInInspectorIf("elementStyle", 1, false)]
	public float goopConeArc = 45f;

	// Token: 0x040068E4 RID: 26852
	[ShowInInspectorIf("elementStyle", 1, false)]
	public AnimationCurve goopCurve;

	// Token: 0x040068E5 RID: 26853
	[ShowInInspectorIf("elementStyle", 1, false)]
	public float goopDuration = 0.5f;

	// Token: 0x020012D4 RID: 4820
	public enum SubElementStyle
	{
		// Token: 0x040068E7 RID: 26855
		ANIMATOR,
		// Token: 0x040068E8 RID: 26856
		GOOP
	}
}
