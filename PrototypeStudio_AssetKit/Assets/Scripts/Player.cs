using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	private Main main;
	public static Player instance;

	public Rewired.Player player;
	public int playerId = 0;
	// Use this for initialization
	private Rigidbody rb;
	[SerializeField] private float speed;
	[SerializeField]private Vector3 moveVector;
	[SerializeField] private Vector2 lookVector;

	private int wordsWritten = 0;
	
	void Start ()
	{
		main = FindObjectOfType<Main>();
		if(instance == null){
			instance = this;
 		} else {
			Destroy(gameObject);
		}

		player = ReInput.players.GetPlayer(playerId);
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		switch (main.gameState)
		{
			case Main.GameState.Intro:
				break;
			case Main.GameState.Game:
				GetInput();
				ProcessInput();
				break;
			case Main.GameState.End:
				if (Input.GetKeyDown(KeyCode.R))
				{
					SceneManager.LoadScene("main");
				}
				break;
			default:
				break;
		}

	}

	private void GetInput()
	{
		moveVector.x = player.GetAxis("MoveX");
		moveVector.y = player.GetAxis("MoveY");
		moveVector.z = player.GetAxis("MoveZ");
		lookVector.x += player.GetAxis("LookX");
		lookVector.y += player.GetAxis("LookY");
	}

	private void ProcessInput()
	{
 		rb.AddRelativeForce(moveVector * speed);
 		rb.MoveRotation(Quaternion.Euler(lookVector));

		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene("main");
		}
 	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.GetComponent<Word>() != null)
		{	
			TextMeshPro wordHit = other.gameObject.GetComponent<TextMeshPro>();
			string text = wordHit.text;
			wordHit.color = Color.yellow;
			AudioManager.instance.PlaySFXOnHit();
			if (wordsWritten <= 4 && wordsWritten > 0)
			{
				TextUtilities.WriteStringToFile(Application.dataPath, Main.title + "_" + Main.author, wordHit.text + " ", false);
				++wordsWritten;
				StartCoroutine(ChangeTextColorOnHit(wordHit));
			}
			else if (wordsWritten == 0)
			{
				TextUtilities.WriteStringToFile(Application.dataPath, Main.title + "_" + Main.author, "\n\n" + wordHit.text + " ", false);
				++wordsWritten;
				StartCoroutine(ChangeTextColorOnHit(wordHit));
			}
			else if (wordsWritten > 4)
			{
				TextUtilities.WriteStringToFile(Application.dataPath, Main.title + "_" + Main.author, wordHit.text + " ", true);
				wordsWritten = 1;
				StartCoroutine(ChangeTextColorOnHit(wordHit));
			} 


//			System.IO.File.WriteAllText(@"C:\Users\Pao Salcedo\Desktop\WriteText.txt", text);	
		}
	}

	private IEnumerator ChangeTextColorOnHit(TextMeshPro someText)
	{
		yield return new WaitForSeconds(3f);
		someText.color = Color.black;
	}
}
