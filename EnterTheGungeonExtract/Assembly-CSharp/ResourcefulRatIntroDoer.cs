using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001070 RID: 4208
[RequireComponent(typeof(GenericIntroDoer))]
public class ResourcefulRatIntroDoer : SpecificIntroDoer
{
	// Token: 0x17000D96 RID: 3478
	// (get) Token: 0x06005C97 RID: 23703 RVA: 0x00237854 File Offset: 0x00235A54
	public override Vector2? OverrideIntroPosition
	{
		get
		{
			return new Vector2?(base.transform.position + new Vector3(2.4375f, 4f));
		}
	}

	// Token: 0x17000D97 RID: 3479
	// (get) Token: 0x06005C98 RID: 23704 RVA: 0x00237880 File Offset: 0x00235A80
	public override bool IsIntroFinished
	{
		get
		{
			return this.m_isFinished && base.IsIntroFinished;
		}
	}

	// Token: 0x06005C99 RID: 23705 RVA: 0x00237898 File Offset: 0x00235A98
	public override void StartIntro(List<tk2dSpriteAnimator> animators)
	{
		base.StartIntro(animators);
		base.StartCoroutine(this.DoIntro());
	}

	// Token: 0x06005C9A RID: 23706 RVA: 0x002378B0 File Offset: 0x00235AB0
	public IEnumerator DoIntro()
	{
		TextBoxManager.TIME_INVARIANT = true;
		bool multiline = false;
		string introKey = this.SelectIntroString(out multiline);
		yield return base.StartCoroutine(this.DoRatTalk(introKey, multiline));
		TextBoxManager.TIME_INVARIANT = false;
		this.m_isFinished = true;
		yield break;
	}

	// Token: 0x06005C9B RID: 23707 RVA: 0x002378CC File Offset: 0x00235ACC
	private string SelectIntroString(out bool multiline)
	{
		multiline = false;
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.BOSSKILLED_RESOURCEFULRAT))
		{
			return "#RATFIGHTINTRO_START_POSTVICTORY";
		}
		if (GameStatsManager.Instance.GetFlag(GungeonFlags.HAS_ATTEMPTED_RESOURCEFUL_RAT))
		{
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_COMPLETE) && !GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_INTRO_SIX_ALT))
			{
				GameStatsManager.Instance.SetFlag(GungeonFlags.RESOURCEFUL_RAT_INTRO_SIX_ALT, true);
				return "#RATFIGHTINTRO_6_NOTES_ATTEMPT_002";
			}
			return "#RATFIGHTINTRO_REPEATED_ATTEMPTS";
		}
		else
		{
			GameStatsManager.Instance.SetFlag(GungeonFlags.HAS_ATTEMPTED_RESOURCEFUL_RAT, true);
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_06))
			{
				multiline = true;
				return "#RATFIGHTINTRO_6_NOTES_ATTEMPT_001";
			}
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_05))
			{
				return "#RATFIGHTINTRO_5_NOTES";
			}
			if (GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_04))
			{
				return "#RATFIGHTINTRO_4_NOTES";
			}
			if (!GameStatsManager.Instance.GetFlag(GungeonFlags.RESOURCEFUL_RAT_NOTE_01))
			{
				multiline = true;
				return "#RATFIGHTINTRO_FOUND_ZERO_NOTES";
			}
			float value = UnityEngine.Random.value;
			if (value < 0.33f)
			{
				return "#RATFIGHTINTRO_3_OR_LESS_NOTES_03";
			}
			if (value < 0.66f)
			{
				return "#RATFIGHTINTRO_3_OR_LESS_NOTES_02";
			}
			return "#RATFIGHTINTRO_3_OR_LESS_NOTES_01";
		}
	}

	// Token: 0x06005C9C RID: 23708 RVA: 0x002379FC File Offset: 0x00235BFC
	public IEnumerator DoRatTalk(string stringKey, bool multiline)
	{
		base.GetComponent<GenericIntroDoer>().SuppressSkipping = true;
		if (multiline)
		{
			int numLines = StringTableManager.GetNumStrings(stringKey);
			for (int i = 0; i < numLines; i++)
			{
				yield return base.StartCoroutine(this.TalkRaw(StringTableManager.GetExactString(stringKey, i)));
			}
		}
		else
		{
			yield return base.StartCoroutine(this.TalkRaw(StringTableManager.GetString(stringKey)));
		}
		yield return null;
		base.GetComponent<GenericIntroDoer>().SuppressSkipping = false;
		yield break;
	}

	// Token: 0x06005C9D RID: 23709 RVA: 0x00237A28 File Offset: 0x00235C28
	private IEnumerator TalkRaw(string plaintext)
	{
		TextBoxManager.ShowTextBox(base.transform.position + new Vector3(2.25f, 7.5f, 0f), base.transform, 5f, plaintext, "ratboss", false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
		bool advancedPressed = false;
		while (!advancedPressed)
		{
			advancedPressed = BraveInput.GetInstanceForPlayer(0).WasAdvanceDialoguePressed() || BraveInput.GetInstanceForPlayer(1).WasAdvanceDialoguePressed();
			yield return null;
		}
		TextBoxManager.ClearTextBox(base.transform);
		yield break;
	}

	// Token: 0x0400563E RID: 22078
	private bool m_isFinished;
}
