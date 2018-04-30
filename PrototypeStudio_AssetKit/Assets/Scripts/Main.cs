using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{

	public enum GameState
	{
		Intro,
		Game,
		End
	}

	public GameState gameState; 

	public InputField titleField;

	public InputField nameField;

	public GameObject restartButton;

	public GameObject controlsTextGO;
	
	public static string title;
	public static string author;

	public TextMeshProUGUI poem;
	public TextMeshProUGUI poemBG;

	private GameObject wordsHolder;
	private StreamReaderScript srScript;
	private WordEmitter wordEmitter;
	private Vector3 poemPosition;
	private float poemY = 0;
	
	// Use this for initialization
	void Start ()
	{
 		wordEmitter = FindObjectOfType<WordEmitter>();
		wordsHolder = GameObject.Find("Words");
		srScript = FindObjectOfType<StreamReaderScript>();
		gameState = GameState.Intro;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		switch (gameState)
		{
			case GameState.Intro:
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				break;
			case GameState.Game:
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
				if (Input.GetKeyDown(KeyCode.Return))
				{
					ViewPoem();	
				}
				break;
			case GameState.End:
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
				ScrollPoem();
 				break;
		}
	}
	
	private void ScrollPoem()
	{
		poemY = poem.rectTransform.position.y;
 		poemY += Player.instance.player.GetAxis("ScrollText") * 5f;
 		poem.rectTransform.position = new Vector3(poem.rectTransform.position.x, poemY, poem.rectTransform.position.z);
	}
	
	public void RecordTitleAndName()
	{
		title = titleField.text;
		author = nameField.text;
		TextUtilities.WriteStringToFile(Application.dataPath, title + "_" + author, titleField.text, true);
		TextUtilities.WriteStringToFile(Application.dataPath, title + "_" + author, "\nA poem by " + nameField.text, true);
		srScript.ReadPoem();
		wordEmitter.Setup();
		gameState = GameState.Game;
	}

	public void ViewPoem()
	{
		controlsTextGO.SetActive(false);
		gameState = GameState.End;
		wordsHolder.SetActive(false);
		restartButton.SetActive(true);
		poem.text = TextUtilities.ReadTextFromFile(Application.dataPath, title + "_" + author);
		
		Player.instance.transform.eulerAngles = Vector3.right * -90f;
		Camera.main.transform.localPosition = new Vector3(29.41f, -5.3f, -46.2f);
		poemBG.text = "Mousewheel Up/Down to scroll up/down";
	}

	public void RestartGame()
	{
		SceneManager.LoadScene("main");
	}

} 
