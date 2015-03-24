using UnityEngine;
using System.Collections;

public class MatchLine1 : EffectTakingPlace{

	float timeToShine;
	float timer;
	bool finished;

	GameObject[] particles;
	Match actualMatch;

	Vector3 originalScale;


	public void Initialize(Match match,float timeToShine){
		this.isBlockingMovingBlocks = true;
		this.makesMatch = true;
		this.timeToShine = timeToShine;
		particles = new GameObject[match.match.Count];
		actualMatch = match;
		originalScale = match.match [0].gameObject.transform.localScale;
		for (int i = 0; i<match.match.Count; i++) {
			GameObject newParticles = GameObject.Instantiate(Almighty.gameManager.getPrefabsManager().matchEffect1) as GameObject;
			newParticles.transform.position = Almighty.gameManager.getTable().getPosition(match.match[i].i,match.match[i].j);
			particles[i] = newParticles;

		}
		timer = 0f;
		finished = false;
	}
	
	// Use this for initialization
	public override void Update(){
		timer += Time.deltaTime;
		if (timer >= timeToShine && !finished) {
			finished = true;
			foreach (GameObject go in particles) {
				GameObject.Destroy (go);
			}
			foreach(Block block in actualMatch.match){
				Almighty.gameManager.getTable().destroyAndAddNewRandomBlock(block);
			}
		} else if(!finished) {
			float ratio = 1f - (timer/timeToShine);
			foreach(Block block in actualMatch.match){
				block.gameObject.transform.localScale = originalScale * ratio;
			}
		}

	}
	
	public override bool isFinished(){
		return finished;
	}
}
