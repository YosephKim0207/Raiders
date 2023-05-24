using System;
using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x020017A1 RID: 6049
public class IntroSequenceManager : MonoBehaviour
{
	// Token: 0x06008D96 RID: 36246 RVA: 0x003B8FB8 File Offset: 0x003B71B8
	private IEnumerator Start()
	{
		if (SceneManager.GetActiveScene().name == "Outro_Demo")
		{
			GameManager.Instance.ClearActiveGameData(false, false);
			UnityEngine.Object.Destroy(GameManager.Instance.DungeonMusicController);
			AkSoundEngine.PostEvent("Stop_SND_All", base.gameObject);
			AkSoundEngine.ClearPreparedEvents();
			AkSoundEngine.StopAll();
		}
		yield return new WaitForSeconds(0.5f);
		base.StartCoroutine(this.HandleElement(0));
		BraveCameraUtility.GenerateBackgroundCamera(Camera.main);
		yield break;
	}

	// Token: 0x06008D97 RID: 36247 RVA: 0x003B8FD4 File Offset: 0x003B71D4
	private void Update()
	{
		BraveCameraUtility.MaintainCameraAspect(Camera.main);
	}

	// Token: 0x06008D98 RID: 36248 RVA: 0x003B8FE0 File Offset: 0x003B71E0
	private IEnumerator HandleElement(int index)
	{
		yield return null;
		IntroSequenceElement element = this.elements[index];
		element.panel.IsVisible = true;
		if (index == 0)
		{
			IntroMovieClipPlayer component = element.panel.GetComponent<IntroMovieClipPlayer>();
			if (component != null)
			{
				component.TriggerMovie();
			}
		}
		if (index == 1 && SceneManager.GetActiveScene().name.Contains("Intro"))
		{
			GameManager.AttemptSoundEngineInitialization();
			AkSoundEngine.PostEvent("Play_MUS_Dungeon_state_loopA", base.gameObject);
			AkSoundEngine.PostEvent("Play_MUS_space_intro_01", base.gameObject);
		}
		float elapsed = 0f;
		element.panel.Opacity = 0f;
		for (int i = 0; i < element.additionalElements.Length; i++)
		{
			element.additionalElements[i].IsVisible = true;
			element.additionalElements[i].Opacity = 0f;
		}
		while (elapsed < element.fadeInTime)
		{
			if (Input.anyKeyDown && index > 0)
			{
				if (!element.waitsForInput)
				{
					goto IL_5F8;
				}
				yield return null;
				element.fadeOutTime = 0.5f;
			}
			else
			{
				if (((InputManager.ActiveDevice == null || !InputManager.ActiveDevice.LeftStickButton.IsPressed || !InputManager.ActiveDevice.RightStickButton.WasPressed) && (!InputManager.ActiveDevice.RightStickButton.IsPressed || !InputManager.ActiveDevice.LeftStickButton.WasPressed) && (!InputManager.ActiveDevice.RightStickButton.WasPressed || !InputManager.ActiveDevice.LeftStickButton.WasPressed)) || !element.waitsForInput)
				{
					element.panel.Opacity = elapsed / element.fadeInTime;
					for (int j = 0; j < element.additionalElements.Length; j++)
					{
						element.additionalElements[j].Opacity = elapsed / element.fadeInTime;
					}
					elapsed += BraveTime.DeltaTime;
					yield return null;
					continue;
				}
				yield return null;
				element.fadeOutTime = 0.5f;
			}
			IL_4ED:
			elapsed = 0f;
			while (elapsed < element.fadeOutTime)
			{
				if (Input.anyKeyDown && index > 0)
				{
					break;
				}
				element.panel.Opacity = 1f - elapsed / element.fadeOutTime;
				for (int k = 0; k < element.additionalElements.Length; k++)
				{
					if (!(element.additionalElements[k] == this.postVideoPanel))
					{
						element.additionalElements[k].Opacity = 1f - elapsed / element.fadeOutTime;
					}
				}
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
			IL_5F8:
			element.panel.IsVisible = false;
			for (int l = 0; l < element.additionalElements.Length; l++)
			{
				if (!(element.additionalElements[l] == this.postVideoPanel))
				{
					element.additionalElements[l].IsVisible = false;
				}
			}
			if (this.elements.Count > index + 1)
			{
				base.StartCoroutine(this.HandleElement(index + 1));
			}
			else if (!string.IsNullOrEmpty(this.nextSceneName))
			{
				Cursor.visible = true;
				if (this.nextSceneName == "MainMenu" && GameManager.Instance != null)
				{
					GameManager.Instance.LoadMainMenu();
				}
				else
				{
					AkSoundEngine.PostEvent("Stop_SND_All", base.gameObject);
					AkSoundEngine.ClearPreparedEvents();
					AkSoundEngine.StopAll();
					SceneManager.LoadScene(this.nextSceneName);
				}
			}
			yield break;
		}
		elapsed = 0f;
		element.panel.Opacity = 1f;
		float targetTime = ((!element.waitsForInput) ? element.hangTime : float.MaxValue);
		while (elapsed < targetTime)
		{
			if (Input.anyKeyDown && index > 0)
			{
				if (element.waitsForInput)
				{
					yield return null;
					element.fadeOutTime = 0.5f;
					break;
				}
				goto IL_5F8;
			}
			else
			{
				if (((InputManager.ActiveDevice != null && InputManager.ActiveDevice.LeftStickButton.IsPressed && InputManager.ActiveDevice.RightStickButton.WasPressed) || (InputManager.ActiveDevice.RightStickButton.IsPressed && InputManager.ActiveDevice.LeftStickButton.WasPressed) || (InputManager.ActiveDevice.RightStickButton.WasPressed && InputManager.ActiveDevice.LeftStickButton.WasPressed)) && element.waitsForInput)
				{
					yield return null;
					element.fadeOutTime = 0.5f;
					break;
				}
				elapsed += BraveTime.DeltaTime;
				yield return null;
			}
		}
		goto IL_4ED;
	}

	// Token: 0x0400954D RID: 38221
	public List<IntroSequenceElement> elements;

	// Token: 0x0400954E RID: 38222
	public dfControl postVideoPanel;

	// Token: 0x0400954F RID: 38223
	public string nextSceneName;

	// Token: 0x04009550 RID: 38224
	private AsyncOperation async;
}
