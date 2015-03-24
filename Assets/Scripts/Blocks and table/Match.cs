using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Match{

	public List<Block> match = new List<Block>(0);

	public string type;

	public bool isVertical = false;
	public bool isHorizontal = false;


	public Match(List<Block> blocks,string type){
		match = blocks;
		this.type = type;
	}
	public void joinMatches(Match otherMatch){
		foreach (Block newBlock in otherMatch.match) {
			if(!checkIfIsInsideMatch(newBlock)){
				match.Add(newBlock);
			}
		}
	}

	public int getAmmountOfElementsMatched(){
		return match.Count;
	}

	private bool checkIfIsInsideMatch(Block newMatchedBlock){
		foreach (Block block in match) {
			if((block.i == newMatchedBlock.i && block.j == newMatchedBlock.j)){
				return true;
			}
		}

		return false;
	}


	public bool checkIfMatchCollidesWithMatch(Match otherMatch){
		if (otherMatch.type.Equals (type)) {
			foreach (Block newMatchedBlock in otherMatch.match) {
				if (checkIfIsInsideMatch (newMatchedBlock)) {
					return true;
				}
			}
		}
		return false;
	}


}
