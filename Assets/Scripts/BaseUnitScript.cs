using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnitScript : MonoBehaviour
{
	public float speed;
	public const float scalePlus = 0.05f;
	public float rotationSpeed;
	const float fieldToFoodSize = 147.5f;

	protected Renderer rend;
	protected Vector3 currPos2;

	protected virtual void Start () 
	{
		rend = GetComponent<Renderer>();
	}

	protected virtual void FixedUpdate()
	{
		MoveFunc();
	}

	protected virtual void MoveFunc()
	{

			if(Input.GetKey(KeyCode.W))
		{
			transform.Translate (Vector3.forward * speed * Time.deltaTime);
		}
			if(Input.GetKey(KeyCode.S))
		{
			transform.Translate (Vector3.forward * -speed * Time.deltaTime);
		}
			if(Input.GetKey(KeyCode.D))
		{
			transform.Rotate (Vector3.up * rotationSpeed * Time.deltaTime);
		}
			if(Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.up*-1*rotationSpeed*Time.deltaTime);
		}
	}

	protected virtual void OnTriggerStay(Collider other)
	 {
		 if(other.CompareTag("Food"))
		 {
			transform.localScale += new Vector3(scalePlus, 0.0f, scalePlus);

			 if(speed>2.3f)
			 {
			 speed -= 0.01f;
			 }

			currPos2 = new Vector3(Random.Range(fieldToFoodSize*-1,fieldToFoodSize),0.2f,Random.Range(fieldToFoodSize*-1,fieldToFoodSize));
			other.gameObject.transform.position = currPos2;
		 }
	 }
}
