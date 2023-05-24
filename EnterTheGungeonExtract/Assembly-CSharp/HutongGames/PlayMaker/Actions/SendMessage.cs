using System;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	// Token: 0x02000ABC RID: 2748
	[Tooltip("Sends a Message to a Game Object. See Unity docs for SendMessage.")]
	[ActionCategory(ActionCategory.ScriptControl)]
	public class SendMessage : FsmStateAction
	{
		// Token: 0x06003A49 RID: 14921 RVA: 0x001289F0 File Offset: 0x00126BF0
		public override void Reset()
		{
			this.gameObject = null;
			this.delivery = SendMessage.MessageType.SendMessage;
			this.options = SendMessageOptions.DontRequireReceiver;
			this.functionCall = null;
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x00128A10 File Offset: 0x00126C10
		public override void OnEnter()
		{
			this.DoSendMessage();
			base.Finish();
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x00128A20 File Offset: 0x00126C20
		private void DoSendMessage()
		{
			GameObject ownerDefaultTarget = base.Fsm.GetOwnerDefaultTarget(this.gameObject);
			if (ownerDefaultTarget == null)
			{
				return;
			}
			object obj = null;
			string parameterType = this.functionCall.ParameterType;
			switch (parameterType)
			{
			case "bool":
				obj = this.functionCall.BoolParameter.Value;
				break;
			case "int":
				obj = this.functionCall.IntParameter.Value;
				break;
			case "float":
				obj = this.functionCall.FloatParameter.Value;
				break;
			case "string":
				obj = this.functionCall.StringParameter.Value;
				break;
			case "Vector2":
				obj = this.functionCall.Vector2Parameter.Value;
				break;
			case "Vector3":
				obj = this.functionCall.Vector3Parameter.Value;
				break;
			case "Rect":
				obj = this.functionCall.RectParamater.Value;
				break;
			case "GameObject":
				obj = this.functionCall.GameObjectParameter.Value;
				break;
			case "Material":
				obj = this.functionCall.MaterialParameter.Value;
				break;
			case "Texture":
				obj = this.functionCall.TextureParameter.Value;
				break;
			case "Color":
				obj = this.functionCall.ColorParameter.Value;
				break;
			case "Quaternion":
				obj = this.functionCall.QuaternionParameter.Value;
				break;
			case "Object":
				obj = this.functionCall.ObjectParameter.Value;
				break;
			case "Enum":
				obj = this.functionCall.EnumParameter.Value;
				break;
			case "Array":
				obj = this.functionCall.ArrayParameter.Values;
				break;
			}
			SendMessage.MessageType messageType = this.delivery;
			if (messageType == SendMessage.MessageType.SendMessage)
			{
				ownerDefaultTarget.SendMessage(this.functionCall.FunctionName, obj, this.options);
				return;
			}
			if (messageType == SendMessage.MessageType.SendMessageUpwards)
			{
				ownerDefaultTarget.SendMessageUpwards(this.functionCall.FunctionName, obj, this.options);
				return;
			}
			if (messageType != SendMessage.MessageType.BroadcastMessage)
			{
				return;
			}
			ownerDefaultTarget.BroadcastMessage(this.functionCall.FunctionName, obj, this.options);
		}

		// Token: 0x04002C7D RID: 11389
		[RequiredField]
		[Tooltip("GameObject that sends the message.")]
		public FsmOwnerDefault gameObject;

		// Token: 0x04002C7E RID: 11390
		[Tooltip("Where to send the message.\nSee Unity docs.")]
		public SendMessage.MessageType delivery;

		// Token: 0x04002C7F RID: 11391
		[Tooltip("Send options.\nSee Unity docs.")]
		public SendMessageOptions options;

		// Token: 0x04002C80 RID: 11392
		[RequiredField]
		public FunctionCall functionCall;

		// Token: 0x02000ABD RID: 2749
		public enum MessageType
		{
			// Token: 0x04002C83 RID: 11395
			SendMessage,
			// Token: 0x04002C84 RID: 11396
			SendMessageUpwards,
			// Token: 0x04002C85 RID: 11397
			BroadcastMessage
		}
	}
}
