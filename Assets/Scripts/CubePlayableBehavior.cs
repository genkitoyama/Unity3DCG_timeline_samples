using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CubePlayableBehavior : PlayableBehaviour {

	Material mat;

	// Called when the owning graph starts playing
	//タイムライン開始時に呼ばれる
	public override void OnGraphStart(Playable playable) {
	
		mat = GameObject.Find("Cube").GetComponent<Renderer>().sharedMaterial;
	}

	// Called when the owning graph stops playing
	//タイムライン停止時に呼ばれる
	public override void OnGraphStop(Playable playable) {

	}

	// Called when the state of the playable is set to Play
	//クリップ開始時に呼ばれる
	public override void OnBehaviourPlay(Playable playable, FrameData info) {
		mat.color = new Color(1.0f, 0.0f, 0.0f);
		
	}

	// Called when the state of the playable is set to Paused
	//クリップ終了時に呼ばれる
	public override void OnBehaviourPause(Playable playable, FrameData info) {
		mat.color = new Color(0.0f, 0.0f, 1.0f);
	}

	// Called each frame while the state is set to Play
	//クリップ再生時に毎フレーム呼ばれる
	public override void PrepareFrame(Playable playable, FrameData info) {
		
	}
}
