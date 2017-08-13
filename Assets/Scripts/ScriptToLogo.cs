using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScriptToLogo : MonoBehaviour {

	protected Renderer rend;
	GameObject logoFood;
	const float fieldSize = 5f;
	const float fieldSize2 = 2.5f;
	const float speed = 4;
	public InputField inputField;

	Transform target;

	Vector3 foodXYZ;
	Vector3 currPos2;
	int count;
	[SerializeField] GameObject foodPrefab;
	[SerializeField] Slider soundLevelSlider;
	[SerializeField] Slider fxLevelSlider;

	void Start () {
		soundLevelSlider.value= StaticClass.soundLevel;
		fxLevelSlider.value = StaticClass.fxLevel;

		inputField = GameObject.FindObjectOfType<InputField> ();
		count = 0;
		rend = GetComponent<Renderer>();
		foodXYZ = new Vector3 (Random.Range (fieldSize * -1, fieldSize), 0.0f, Random.Range (fieldSize2 * -1, fieldSize2));
		logoFood = GameObject.Instantiate (foodPrefab, foodXYZ, Quaternion.identity) as GameObject; 
	}
	
	// Update is called once per frame
	void Update () {
		StaticClass.soundLevel = soundLevelSlider.value;
		StaticClass.fxLevel = fxLevelSlider.value;

		CharColorChange ();
		MoveFunc();
	}
	protected void MoveFunc()
	{
		if (Input.GetKey (KeyCode.Return)) 
		{
			SceneManager.LoadScene ("MainScene");
		}
		logoFood = GameObject.FindGameObjectWithTag ("Food");
		target = logoFood.transform;
		transform.LookAt (target);
		transform.position = Vector3.MoveTowards (transform.position,logoFood.transform.position, Time.deltaTime * speed);

	}

	public void OnGUI()
	{
		if (inputField.text == "") 
		{
			StaticClass.namePlayer = "NoName";
		}
		else
		{
			StaticClass.namePlayer = inputField.text;
		}
	}
	public void ExitButton()
	{
		Application.Quit();
	}

	public void StartGame()
	{
		SceneManager.LoadScene (1);
	}
	protected void CharColorChange()
	{
		if(count == 1)
		{
			rend.material.color = Color.white;
		}
		else if(count == 2)
		{
			rend.material.color = Color.green;
		}
		else if(count == 3)
		{
			rend.material.color = Color.blue;
		}
		else if(count == 4)
		{
			rend.material.color = Color.magenta;
		}
		else if(count == 5)
		{
			rend.material.color = Color.yellow;
		}
		else if(count == 6)
		{
			rend.material.color = Color.red;
		}
		else if(count == 7)
		{
			count = 1;
		}
		else
		{
			return;
		}
	}
	protected void OnTriggerStay(Collider other)
	{
		if(other.CompareTag("Food"))
		{
			count++;
			currPos2 = new Vector3(Random.Range (fieldSize * -1, fieldSize), 0.0f, Random.Range (fieldSize2 * -1, fieldSize2));
			other.gameObject.transform.position = currPos2;
		}
	}
}
