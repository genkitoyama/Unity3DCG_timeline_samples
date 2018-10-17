using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CubePlayableAsset : PlayableAsset {

	// Factory method that generates a playable based on this asset
	public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {

		//Playable Behavior を生成
		CubePlayableBehavior cpb = new CubePlayableBehavior();

		return ScriptPlayable<CubePlayableBehavior>.Create(graph, cpb);
		
		// return Playable.Create(graph); //ここはテンプレートにあるが削除する
	}
}
