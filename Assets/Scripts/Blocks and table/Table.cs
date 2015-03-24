using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour {
	
	private Block[][] mainMatrix;
	public int width;
	public int height;
	public GameObject[] blocksPrefabs;
	public float gridSize = 1f;
	public Vector3 startingPositionGrid;

	void Start(){
		//Initialize the matrix
		mainMatrix = new Block[width][];
		for (int i = 0; i<width; i++) {
			mainMatrix[i] = new Block[height];
		}
		
		for (int i = 0; i<width; i++) {
			for(int j = 0;j<height;j++){
				mainMatrix[i][j] = getRandomBlock(i,j);
			}
		}
	}


	private Block getRandomBlock(int i,int j){
		
		//We get a random block from the prefabs
		GameObject newObject = GameObject.Instantiate (blocksPrefabs [Random.Range (0, blocksPrefabs.Length)]) as GameObject;
		newObject.transform.position = startingPositionGrid + new Vector3 (i * gridSize, j * gridSize, 0f);
		newObject.transform.parent = transform;
		
		Block block = new Block (newObject,i,j,newObject.tag);
		
		return block;
	}


	public List<Match> checkForMatches(){
		List<Match> matches = new List<Match> (0);
		
		//Check horizontal matches
		for(int j = 0;j<height;j++){
			List<Block> horizontalMatch = new List<Block>(0);
			string actualType = "";
				for (int i = 0; i<width; i++) {
				Block actualBlock = mainMatrix[i][j];
				if(actualBlock.type.Equals (actualType)){
					horizontalMatch.Add(actualBlock);
				}else{
					if(horizontalMatch.Count>=3){
						Match newMatch = new Match(horizontalMatch,actualType);
						newMatch.isHorizontal = true;
						//In the horizontal we don't have to check for multiple ocurrences
						matches.Add (newMatch);
					}
					horizontalMatch = new List<Block>(0);
					horizontalMatch.Add(actualBlock);
					actualType = actualBlock.type;
				}
			}
			if(horizontalMatch.Count>=3){
				Match newMatch = new Match(horizontalMatch,actualType);
				newMatch.isHorizontal = true;
				//In the horizontal we don't have to check for multiple ocurrences
				matches.Add (newMatch);
			}
		}
		
		//Check vertical matches
		for(int i = 0;i<width;i++){
			List<Block> verticalMatch = new List<Block>(0);
			string actualType = "";
			for (int j = 0; j<height; j++) {
				Block actualBlock = mainMatrix[i][j];
				if(actualBlock.type.Equals (actualType)){
					verticalMatch.Add(actualBlock);
				}else{
					if(verticalMatch.Count>=3){
						Match newMatch = new Match(verticalMatch,actualType);
						//We check for collidingOcurrences
						newMatch.isVertical = true;
						checkForHorizontalMatchesColliding(matches,newMatch);
					}
					verticalMatch = new List<Block>(0);
					verticalMatch.Add(actualBlock);
					actualType = actualBlock.type;
				}
			}
			if(verticalMatch.Count>=3){
				Match newMatch = new Match(verticalMatch,actualType);
				//We check for collidingOcurrences
				newMatch.isVertical = true;
				checkForHorizontalMatchesColliding(matches,newMatch);
			}
		}
		cleanMatches (matches);


		//Debug the result
		/*foreach (Match match in matches) {
			Debug.Log("One match of "+match.match[0].type+" with "+match.match.Count+" ocurrences ");
		}
		DebugMatrixWithMatches (matches);*/

		return matches;
	}

	public Vector3 getPosition(int i, int j){
		return startingPositionGrid + new Vector3 (i * gridSize, j * gridSize, 0f);
	}

	public void getCorrespondantIndexPosition(out int i,out int j,Vector3 position){
		i = (int)((position.x - startingPositionGrid.x + gridSize/2f) / gridSize);
		j = (int)((position.y - startingPositionGrid.y + gridSize/2f) / gridSize);
	}

	public bool insideBounds(int i,int j){
		if (i >= 0 && i < width && j >= 0 && j < height) {
			return true;
		}
		return false;
	}

	public Block getBlock(int i,int j){
		return mainMatrix[i][j];
	}

	public void replaceBlock(int i,int j,Block block){
		mainMatrix[i][j] = block;
		block.i = i;
		block.j = j;
	}

	public void destroyBlock(Block block){
		Destroy (block.gameObject);
		mainMatrix [block.i] [block.j] = null;
		block = null;
	}

	public void destroyAndAddNewRandomBlock(Block block){
		int i = block.i;
		int j = block.j;

		destroyBlock (block);

		mainMatrix [i][j] = getRandomBlock (i,j);
	}



	private void DebugMatrixWithMatches(List<Match> matches){
		
		string toWrite = "";
		for (int j = height-1; j>=0; --j) {
			for (int i = 0; i<width; ++i) {
				if(isBlockInsideAnyMatch(matches,mainMatrix[i][j])){
					toWrite+="x";
				}else{
					toWrite+="o";
				}
			}
			toWrite+="\n";
		}
		Debug.Log (toWrite);
	}


	
	private bool isBlockInsideAnyMatch(List<Match> matches,Block block){
		foreach (Match match in matches) {
			if(match.match.Contains(block)){
				return true;
			}
		}
		return false;
	}

	private void checkForHorizontalMatchesColliding(List<Match> matches,Match newMatch){
		bool hasbeenFused = false;
		foreach (Match match in matches) {
			if(match.isHorizontal){
				if(match.checkIfMatchCollidesWithMatch(newMatch)){
					match.joinMatches(newMatch);
					hasbeenFused = true;
					break;
				}
			}
		}
		if (!hasbeenFused) {
			matches.Add(newMatch);
		}
	}

	//Cleans the matches to make sure there are not repeated blocks
	private void cleanMatches(List<Match> matches){
		bool clean = false;
		while (!clean) {
			clean = true;
			for(int i = 0;i<matches.Count-1;++i){
				for(int j = i+1;j<matches.Count;++j){
					if(matches[i].checkIfMatchCollidesWithMatch(matches[j])){
						matches[i].joinMatches(matches[j]);
						matches.Remove(matches[j]);
						clean = false;
						break;
					}
				}
			}
		}
	}

}
