﻿#if PHOTON_BOLT
using UnityEngine;
using System.Collections;

namespace ICode.Actions.PhotonBolt{
	[Category("Photon Bolt")]    
	[Tooltip("Disable UPnP")]
	[System.Serializable]
	public class DisableUPnP : StateAction {

		public override void OnEnter ()
		{
			BoltNetwork.DisableUPnP ();
			Finish ();
		}
		
	}
}
#endif