using System;
using System.Collections;
using UnityEngine;

// Token: 0x020011DD RID: 4573
public class PlayerCommentInteractable : BraveBehaviour, IPlayerInteractable
{
	// Token: 0x06006606 RID: 26118 RVA: 0x0027A5F0 File Offset: 0x002787F0
	private void Start()
	{
	}

	// Token: 0x06006607 RID: 26119 RVA: 0x0027A5F4 File Offset: 0x002787F4
	private IEnumerator Do()
	{
		this.m_isDoing = true;
		if (this.OnInteractionBegan != null)
		{
			this.OnInteractionBegan();
		}
		Transform primaryTransform = GameManager.Instance.PrimaryPlayer.transform;
		Transform secondaryTransform = ((GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? null : GameManager.Instance.SecondaryPlayer.transform);
		Transform dogTransform = ((GameManager.Instance.PrimaryPlayer.companions.Count <= 0) ? null : GameManager.Instance.PrimaryPlayer.companions[0].transform);
		for (int i = 0; i < this.comments.Length; i++)
		{
			CommentModule currentModule = this.comments[i];
			Transform targetTransform = null;
			Vector3 targetOffset = Vector3.zero;
			string audioTag = string.Empty;
			CommentModule.CommentTarget target = currentModule.target;
			if (target != CommentModule.CommentTarget.PRIMARY)
			{
				if (target != CommentModule.CommentTarget.SECONDARY)
				{
					if (target == CommentModule.CommentTarget.DOG)
					{
						targetTransform = dogTransform;
						targetOffset = new Vector3(0.25f, 1f, 0f);
					}
				}
				else
				{
					targetTransform = secondaryTransform;
					targetOffset = new Vector3(0.5f, 1.5f, 0f);
					audioTag = ((GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? GameManager.Instance.PrimaryPlayer.characterAudioSpeechTag : GameManager.Instance.SecondaryPlayer.characterAudioSpeechTag);
				}
			}
			else
			{
				targetTransform = primaryTransform;
				targetOffset = new Vector3(0.5f, 1.5f, 0f);
				audioTag = GameManager.Instance.PrimaryPlayer.characterAudioSpeechTag;
			}
			if (targetTransform != null)
			{
				this.DoAmbientTalk(targetTransform, targetOffset, currentModule.stringKey, currentModule.duration, audioTag);
				yield return new WaitForSeconds(currentModule.delay);
			}
		}
		if (this.OnInteractionFinished != null)
		{
			this.OnInteractionFinished();
		}
		this.m_isDoing = false;
		yield break;
	}

	// Token: 0x06006608 RID: 26120 RVA: 0x0027A610 File Offset: 0x00278810
	public void DoAmbientTalk(Transform baseTransform, Vector3 offset, string stringKey, float duration, string overrideAudioTag = "")
	{
		string text;
		if (this.keyIsSequential)
		{
			text = StringTableManager.GetStringSequential(stringKey, ref this.m_seqIndex, false);
			for (int i = 0; i < this.linkedInteractables.Length; i++)
			{
				this.linkedInteractables[i].m_seqIndex++;
			}
		}
		else
		{
			text = StringTableManager.GetString(stringKey);
		}
		TextBoxManager.ShowThoughtBubble(baseTransform.position + offset, baseTransform, duration, text, false, false, overrideAudioTag);
	}

	// Token: 0x06006609 RID: 26121 RVA: 0x0027A68C File Offset: 0x0027888C
	public float GetDistanceToPoint(Vector2 point)
	{
		if (this.m_hasBeenTriggered && this.onlyTriggerOnce)
		{
			return 1000f;
		}
		if (this.m_isDoing)
		{
			return 1000f;
		}
		if (this.usesOverrideInteractionRegion)
		{
			return BraveMathCollege.DistToRectangle(point, base.transform.position.XY() + this.overrideRegionOffset * 0.0625f, this.overrideRegionDimensions * 0.0625f);
		}
		Bounds bounds = base.sprite.GetBounds();
		bounds.SetMinMax(bounds.min + base.transform.position, bounds.max + base.transform.position);
		float num = Mathf.Max(Mathf.Min(point.x, bounds.max.x), bounds.min.x);
		float num2 = Mathf.Max(Mathf.Min(point.y, bounds.max.y), bounds.min.y);
		return Mathf.Sqrt((point.x - num) * (point.x - num) + (point.y - num2) * (point.y - num2));
	}

	// Token: 0x0600660A RID: 26122 RVA: 0x0027A7E0 File Offset: 0x002789E0
	public void OnEnteredRange(PlayerController interactor)
	{
		SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
		SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.white);
	}

	// Token: 0x0600660B RID: 26123 RVA: 0x0027A800 File Offset: 0x00278A00
	public void OnExitRange(PlayerController interactor)
	{
		if (base.sprite)
		{
			SpriteOutlineManager.RemoveOutlineFromSprite(base.sprite, true);
			SpriteOutlineManager.AddOutlineToSprite(base.sprite, Color.black);
		}
	}

	// Token: 0x0600660C RID: 26124 RVA: 0x0027A830 File Offset: 0x00278A30
	public void ForceDisable()
	{
		this.m_hasBeenTriggered = true;
		this.onlyTriggerOnce = true;
	}

	// Token: 0x0600660D RID: 26125 RVA: 0x0027A840 File Offset: 0x00278A40
	public void Interact(PlayerController interactor)
	{
		if (this.m_hasBeenTriggered && this.onlyTriggerOnce)
		{
			return;
		}
		if (this.m_isDoing)
		{
			return;
		}
		for (int i = 0; i < this.linkedInteractables.Length; i++)
		{
			this.linkedInteractables[i].m_hasBeenTriggered = true;
		}
		this.m_hasBeenTriggered = true;
		base.StartCoroutine(this.Do());
	}

	// Token: 0x0600660E RID: 26126 RVA: 0x0027A8AC File Offset: 0x00278AAC
	public string GetAnimationState(PlayerController interactor, out bool shouldBeFlipped)
	{
		shouldBeFlipped = false;
		return string.Empty;
	}

	// Token: 0x0600660F RID: 26127 RVA: 0x0027A8B8 File Offset: 0x00278AB8
	public float GetOverrideMaxDistance()
	{
		return -1f;
	}

	// Token: 0x040061D5 RID: 25045
	public CommentModule[] comments;

	// Token: 0x040061D6 RID: 25046
	public bool onlyTriggerOnce;

	// Token: 0x040061D7 RID: 25047
	public PlayerCommentInteractable[] linkedInteractables;

	// Token: 0x040061D8 RID: 25048
	public bool keyIsSequential;

	// Token: 0x040061D9 RID: 25049
	[Header("Interactable Region")]
	public bool usesOverrideInteractionRegion;

	// Token: 0x040061DA RID: 25050
	[ShowInInspectorIf("usesOverrideInteractionRegion", false)]
	public Vector2 overrideRegionOffset = Vector2.zero;

	// Token: 0x040061DB RID: 25051
	[ShowInInspectorIf("usesOverrideInteractionRegion", false)]
	public Vector2 overrideRegionDimensions = Vector2.zero;

	// Token: 0x040061DC RID: 25052
	private bool m_isDoing;

	// Token: 0x040061DD RID: 25053
	private bool m_hasBeenTriggered;

	// Token: 0x040061DE RID: 25054
	public Action OnInteractionBegan;

	// Token: 0x040061DF RID: 25055
	public Action OnInteractionFinished;

	// Token: 0x040061E0 RID: 25056
	private int m_seqIndex;
}
