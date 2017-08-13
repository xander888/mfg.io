using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BotScript : BaseUnitScript//наследуем от базового класса
{
	[SerializeField] Transform heroObj; //позиция героя
	[SerializeField] float targetDist; //переменная для дистанции между героем и ботом
	[SerializeField] float attackDist; //дистанция для нападения или отступления
	[SerializeField] Text LoseText; //текстовое поле для панели поражения
	[SerializeField] GameObject LosePanel; //панель поражения
	//[SerializeField] int x = 0;
	//[SerializeField] int y = 0;

	string[] nameBots = {"AliskaSosiska" , "ChumiBumi" , "Sanuch" , "Kanapeshka", "Velikan" , "[PROUA] Atines"}; //массив имен ботов
	public Text countText2; //текст для отображения очков бота
	FoodGeneration foodScript; //переменная для скрипта 
	HeroScript heroScript; //переменная для скрипта
	public float botCount; //количество очков бота
	public Transform targetToFood; //переменная для определения положения цели
	public GameObject clossFood; //переменная для ближайшей еды из цикла выборки
	int randomNumToNameBot; //переменная для рандомного числа, которое выберет рандомное имя бота

	protected override void Start () 
	{
		base.Start();
		LinkMethod ();
		attackDist = 15.0f;
		randomNumToNameBot = Random.Range(0,nameBots.Length); //генерация рандомного числа
		botCount = 0;
	}

	void LinkMethod()//ссылки
	{
		heroScript = GameObject.Find("Hero").GetComponent<HeroScript> ();
		foodScript = GameObject.Find ("EmptyGameHelper").GetComponent<FoodGeneration>();
	}

	protected override void FixedUpdate ()
	{
		BotMove ();
		CharColorChange ();
	}

	protected void BotMove() //передвижение бота если рядом герой
	{
		if (!heroObj) //если героя убили (null)
			return;
		
		targetDist = Vector3.Distance (transform.position,heroObj.position); //дистанция между ботом и позицией героя

		if (targetDist < attackDist && heroScript.count<botCount/*&& x == 0 && y == 0*/) //проверка дистанции для атаки + проверка очков
		{
			//	x = 1;
			//	y = 1;

			//if (x = 1) 
			//{
			//	StartCoroutine ("TimerPlus");
			//}
		
			//else{
			TargetHelper ();
			transform.position = Vector3.MoveTowards (transform.position, heroObj.position, Time.deltaTime * speed);//атака
			//}
		}
		else if (targetDist < attackDist && heroScript.count>botCount) //проверка дистанции для отступления + проверка очков
		{
			TargetHelper ();
			transform.position = Vector3.MoveTowards (transform.position, heroObj.position, Time.deltaTime * -speed);
			//transform.Translate (Vector3.back * speed * Time.deltaTime); //отступление
		}
		else 
		{
			MoveFunc ();
		}	
	}

	protected IEnumerator TimerPlus()//нужно доработать
	{
		TargetHelper ();
		transform.position = Vector3.MoveTowards (transform.position, heroObj.position, Time.deltaTime * 2 * speed);
		yield return new WaitForSeconds(heroScript.speedUpTime);
		TargetHelper ();
		transform.position = Vector3.MoveTowards (transform.position, heroObj.position, Time.deltaTime * speed);
		yield return new WaitForSeconds (heroScript.reloadTime);
		speed = 0;

		StopCoroutine("TimerPlus");
	}

	void TargetHelper()
	{
		Transform target2 = heroObj.transform; //определяем позицию героя

		transform.LookAt (target2); //смотрим на героя
	}
	protected override void MoveFunc() //передвижение бота к еде
	{
		if (botCount > 1000) 
		{
			Destroy (gameObject);
		}
		//Поиск ближайшей еды
		clossFood = foodScript.currFood[0]; //присваиваем переменной нулевой элемент массива еды
		//если дистанция от еды до нашего обьекта меньше чем от ближайшей еды(нулевой элемент массива) до нашего обьекта, то записываем в пустую перменную где будет ближайшая еда
		for(int i=0;i<foodScript.foodNum;i++)
		{
			if (Vector3.Distance (foodScript.currFood[i].transform.position , transform.position)<Vector3.Distance(clossFood.transform.position, transform.position))
			{
				clossFood = foodScript.currFood[i];
			}

		}
		targetToFood = clossFood.transform; //записываем координаты ближайшей еды в переменную

		transform.LookAt (targetToFood); //смотрим на ближайшую еду
		transform.Translate (Vector3.forward * speed * Time.deltaTime); //идем к еде
	}	

	protected void CharColorChange() //Смена цвета обьекта в зависимости от количества очков
	{
		if(botCount == 10)
		{
			rend.material.color = Color.white;
		}
		else if(botCount == 25)
		{
			rend.material.color = Color.green;
		}
		else if(botCount == 50)
		{
			rend.material.color = Color.blue;
		}
		else if(botCount == 100)
		{
			rend.material.color = Color.magenta;
		}
		else if(botCount == 150)
		{
			rend.material.color = Color.yellow;
		}
		else if(botCount == 190)
		{
			rend.material.color = Color.red;
		}
		else
		{
			return;
		}
	}

	public void SetCountBotText() //считаем и показываем очки бота
	{
		countText2.text = nameBots[randomNumToNameBot] + ": " + botCount.ToString(); //показываем очки бота с рандомным именем
	}
	protected override void OnTriggerStay(Collider other) //метод взаимодествия с другими обьектами
	{
		base.OnTriggerStay(other);

		if (other.CompareTag ("Food")) //если это еда
		{
			botCount++; //+1 очко боту
			SetCountBotText (); //обновляем очки
		}

		if(other.CompareTag("Hero") && botCount>heroScript.count) //если это герой и у него очков меньше чем у бота то игра проиграна
		{
			Cursor.visible = true; //включаем курсор
			transform.localScale += new Vector3(heroScript.count * scalePlus, 0.0f, heroScript.count * scalePlus); //увеличиваем размер бота
			botCount += heroScript.count; //добавляем очки героя боту
			LoseText.text = "Очки: " + heroScript.count; //записываем очки в панель
			LosePanel.SetActive (true); //активируем панель поражения

			Destroy (other.gameObject); //уничтожаем обьект героя
		}
		else if(other.CompareTag("Hero") && botCount<heroScript.count) //если это герой и у него больше очков чем у нас, записываем в строку бота что он уничтожен 
		{
			countText2.text = "Уничтожен"; //дестрой обьекта бота происходит в скрипте героя
		}
	}
}

