﻿using UnityEngine;
using System.Collections;

namespace ICode.Actions.UnityRectTransform{
	[Category(Category.RectTransform)]    
	[Tooltip("The size of this RectTransform relative to the distances between the anchors.")]
	[HelpUrl("http://docs.unity3d.com/ScriptReference/RectTransform-sizeDelta.html")]
	[System.Serializable]
	public class GetSizeDelta : RectTransformAction {
		[Shared]
		[Tooltip("Store the result.")]
		public FsmVector2 store;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;
		
		public override void OnEnter ()
		{
			base.OnEnter ();
			store.Value = transform.sizeDelta;
			if (!everyFrame) {
				Finish ();
			}
		}
		
		public override void OnUpdate ()
		{
			store.Value = transform.sizeDelta;
		}
	}
}