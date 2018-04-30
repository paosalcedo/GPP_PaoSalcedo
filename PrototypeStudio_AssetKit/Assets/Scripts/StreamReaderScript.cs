using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class StreamReaderScript : MonoBehaviour
{

	private string[] fileNames = { "stories.txt"};

	private int fileNum = 0;
	
  	protected List<string> words = new List<string>();
	protected List<GameObject> textmeshGOs = new List<GameObject>();
 	
 	public void ReadPoem ()
	{
		string fileName = fileNames[fileNum];
		string path = Application.dataPath + "/" + fileName;

		StreamReader sr = new StreamReader(path);	
 		
		while (!sr.EndOfStream)
		{
			string line = sr.ReadLine();
			string someWord = "";

			for (int i = 0; i < line.Length; i++)
			{
				//if it's not a blank space,
				if (line[i] != ' ')
				{
					//or it's a letter or a character, append it to the string someWord
					someWord = someWord + line[i].ToString();
				} else if (line[i] == ' ')
				{
					//if the StreamReader hits a blank space, it's probably the end of a word
					//so instead of appending the blank space to someWord, 
					//add it to the list of words to be projected into world space
 					words.Add(someWord);		
					//clear the string someWord for the next characters			
					someWord = "";		
				}
			}
		}
		
		sr.Close();
	}

}
