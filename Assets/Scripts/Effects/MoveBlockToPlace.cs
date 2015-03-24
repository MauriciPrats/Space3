using UnityEngine;
using System.Collections;

public class MoveBlockToPlace : EffectTakingPlace {

	float timer;
	float timeItTakes;

	Block block;


	Vector3 origin,objective;

	bool finished = false;

	int originalI;
	int originalJ;

	int objectiveI;
	int objectiveJ;


	public void Initialize(float timeItTakes,int  objectiveI,int objectiveJ,Block block){
		this.isBlockingMovingBlocks = false;
		this.makesMatch = false;
		this.timeItTakes = timeItTakes;
		this.block = block;
		this.objectiveI = objectiveI;
		this.objectiveJ = objectiveJ;

		origin = block.gameObject.transform.position;
		objective = Almighty.gameManager.getTable().getPosition(objectiveI,objectiveJ);

		timer = 0f;
	}

	// Use this for initialization
	public override void Update(){

		timer += Time.deltaTime;
		if (timer <= timeItTakes) {
			Vector3 actualPosition = Vector3.Lerp (origin, objective, (timer / timeItTakes));
			block.gameObject.transform.position = actualPosition;
		} else {
			block.gameObject.transform.position = Almighty.gameManager.getTable().getPosition(objectiveI, objectiveJ);

			finished = true;
		}
	}
	
	public override bool isFinished(){
		return finished;
	}
}
