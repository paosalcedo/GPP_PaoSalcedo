using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class WordEmitter : StreamReaderScript
{
//	private float emissionInterval = 0f;
//	private List<GameObject> wordGameObjects = new List<GameObject>();
	
	private int wordIndex = 0;
	public Font newFont;

	public void Setup()
	{
		for(wordIndex = 0; wordIndex<words.Count; ++wordIndex)
		{
			GameObject newWord = new GameObject("new word");
			newWord.AddComponent<TextMeshPro>();
			newWord.AddComponent<Word>();
			TextMeshPro newTextMeshPro = newWord.GetComponent<TextMeshPro>();
			newTextMeshPro.font = TextUtilities.GetTmpFontAsset("BKANT SDF");
			newTextMeshPro.fontSize *= 3;
			newTextMeshPro.enableWordWrapping = false;
			newTextMeshPro.text = words[wordIndex];
			newTextMeshPro.color = Color.black;
 			newWord.transform.position = Player.instance.gameObject.transform.position + Random.insideUnitSphere * 1000f;
 		}
	}

//	public void Update()
//	{
//		emissionInterval += Time.deltaTime;
//		
//		if (emissionInterval >= 0.1f)
//		{
//			if (wordIndex < words.Count)
//			{
//				GameObject newWord = new GameObject("new word");
////				newWord.AddComponent<TextMesh>();
////				TextMesh newTextMesh = newWord.GetComponent<TextMesh>();
//////				newTextMesh.font = TextUtilities.GetFont("CENSCBK");
////				newTextMesh.characterSize *= 5f;
////				newTextMesh.fontSize = 96;
////				newTextMesh.text = words[wordIndex];
////				newWord.AddComponent<Rigidbody>();
////				newWord.GetComponent<Rigidbody>().useGravity = true;			
//				newWord.AddComponent<TextMeshPro>();
//				TextMeshPro newTextMeshPro = newWord.GetComponent<TextMeshPro>();
//				newTextMeshPro.font = TextUtilities.GetTmpFontAsset("BKANT SDF");
//				newTextMeshPro.text = words[wordIndex];
////				newWord.AddComponent<Rigidbody>();
////				newWord.GetComponent<Rigidbody>().useGravity = true;
//				newWord.transform.position = Player.instance.gameObject.transform.position + new Vector3(Random.Range(-100, 100), Random.Range(-100f, 100f), Random.Range(-100, 100));
//				++wordIndex;
//				emissionInterval = 0;
//			}
//		}	
//	}
}


