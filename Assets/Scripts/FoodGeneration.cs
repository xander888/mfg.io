using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodGeneration : MonoBehaviour {

	public int foodNum; //количество еды (публичная потому что нужна для других классов)
	public GameObject[] currFood; //массив еды (публичная потому что нужна для других классов)

	const float fieldToFoodSize = 147.5f; //значение максимального положения создания еды

	[SerializeField] GameObject foodPrefab; //префаб еды
	Vector3 foodXYZ; //переменная координат для рандомного положения

	void Start()
	{
		FoodGener ();
	}

	void FoodGener() // метод создания еды
	{
		for (int i = 0; i < foodNum; i++) 
		{
			foodXYZ = new Vector3 (Random.Range (fieldToFoodSize * -1, fieldToFoodSize), 0.2f, Random.Range (fieldToFoodSize * -1, fieldToFoodSize)); //задаем переменной рандомное положение на поле
			currFood [i] = GameObject.Instantiate (foodPrefab, foodXYZ, Quaternion.identity) as GameObject; //создаем массив еды
			currFood[i].name = string.Format("Food[{0}]",i);//меняем имена клонов
		}
	}
}