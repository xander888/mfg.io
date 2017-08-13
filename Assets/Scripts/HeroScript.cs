using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class HeroScript : BaseUnitScript 
{
	[SerializeField] int x=0;
	[SerializeField] int y=0;
	int timerSec;
	int timerMin;
	int botKill;
	float interpPower;
	public float count;
	public string playName;
	BotScript botScript;

	[SerializeField] int numberOfPointsToWin;
	public int speedUpTime = 3;
	public int reloadTime = 5;
	[SerializeField] AudioSource foodSound;
	[SerializeField] Text WinText;
	[SerializeField] Text GameTimeToPanel;
	[SerializeField] Text countText;
	[SerializeField] Text skillText;
	[SerializeField] Text timerText;
	[SerializeField] GameObject WinPanel;
	[SerializeField] GameObject cameraObj;
	[SerializeField] Slider soundLevelSlider;
	[SerializeField] Slider fxLevelSlider;

	protected override void Start () 
	{
		base.Start ();
		soundLevelSlider.value= StaticClass.soundLevel;
		fxLevelSlider.value = StaticClass.fxLevel;
		foodSound = GetComponent<AudioSource>();
		skillText.text = "Пробел для ускорения";
		botScript = GameObject.Find ("Bot").GetComponent<BotScript> ();
		numberOfPointsToWin = 200;
		interpPower = 5f;
		StartCoroutine ("Timer");
		SetCountText();	
	}
	void Update()
	{
		StaticClass.soundLevel = soundLevelSlider.value;
		StaticClass.fxLevel = fxLevelSlider.value;
	}

	protected override void FixedUpdate()
	{
		CameraFunc();
		MoveFunc();
		CharColorChange();		
		TimerHelper ();
		CursorHelper ();
		WinMethod ();

	}

	protected void CursorHelper()
	{
		if (Input.anyKey && !Input.GetMouseButton(0)) 
		{
			Cursor.visible = false;
		}
		else
			Cursor.visible = true;
	}

	protected void TimerHelper()
	{
		if (timerSec == 60)
		{
			timerMin++;
			timerSec = 0;
		} 
		else if (timerSec < 10) 
		{
			timerText.text = "Таймер: " + timerMin + ":0" + timerSec;
		}
		else 
			timerText.text = "Таймер: " + timerMin + ":" + timerSec;
	}

	IEnumerator Timer()
	{
		while(true)
		{
			yield return new WaitForSeconds(1);
			timerSec++;
		}
	}

	protected override void MoveFunc()
	{
		base.MoveFunc();

		if(Input.GetKey(KeyCode.Space)&& x == 0 && y == 0)
		{
			x = 1;
			y = 1;
		}

		if (x == 1) 
		{
			StartCoroutine ("TimerPlus");
		}
	}

	protected IEnumerator TimerPlus()//нужно доработать
	{
		transform.Translate (Vector3.forward * speed * 2 * Time.deltaTime);
		skillText.text = "Ускорение";
		yield return new WaitForSeconds(speedUpTime);
		x = 0;
		skillText.text = "Откат " + reloadTime + " секунд";
		yield return new WaitForSeconds (reloadTime);
		y = 0;
		skillText.text = "Пробел для ускорения";
		StopCoroutine("TimerPlus");
	}

	void WinMethod()
	{
		if (count >= numberOfPointsToWin) 
		{
			WinText.text = "Очки: " + numberOfPointsToWin + "! Убийств: " + botKill;
			WinPanel.SetActive (true);
			Destroy (gameObject);
			Cursor.visible = true;
			if (timerSec < 10) {
				GameTimeToPanel.text = "Время игры: " + timerMin + ":0" + timerSec;
			} else
				GameTimeToPanel.text = "Время игры: " + timerMin + ":" + timerSec;
			StopCoroutine ("Timer");

			if (!botScript)// если бот уничтожен и ссылка будет null
				return;
			botScript.transform.position = new Vector3(botScript.transform.position.x, -5f,botScript.transform.position.z);

		}
	}

	void CameraFunc()//передвижение камеры
	{
		Vector3 targePos = new Vector3(transform.position.x, cameraObj.transform.position.y, transform.position.z);
		cameraObj.transform.position = Vector3.Lerp(cameraObj.transform.position, targePos, Time.deltaTime * interpPower);
	}

	protected void CharColorChange()//изменение цвета материала в зависимости от количества очков
	{
		if(count == 10)
		{
			rend.material.color = Color.white;
		}
		else if(count == 25)
		{
			rend.material.color = Color.green;
		}
		else if(count == 50)
		{
			rend.material.color = Color.blue;
		}
		else if(count == 100)
		{
			rend.material.color = Color.magenta;
		}
		else if(count == 150)
		{
			rend.material.color = Color.yellow;
		}
		else if(count == 190)
		{
			rend.material.color = Color.red;
		}
		else
		{
			return;
		}
	}
		
	protected override void OnTriggerStay(Collider other)
	{
		 if(other.CompareTag("Food"))
		 {
			base.OnTriggerStay(other);
			foodSound.Play ();
			CameraUpHelper (0.1f);
			count++;

			SetCountText();		 
		}

		if(other.CompareTag("Bot") && count>botScript.botCount)
		{
			botKill++;
			transform.localScale += new Vector3(botScript.botCount * scalePlus, 0.0f, botScript.botCount * scalePlus);
			CameraUpHelper (botScript.botCount * 0.1f);
			count += botScript.botCount;
			Destroy (other.gameObject);
		}
	 }

	public void CameraUpHelper(float cameraDistance)
	 {
		cameraObj.transform.position = new Vector3(cameraObj.transform.position.x, cameraObj.transform.position.y + cameraDistance, cameraObj.transform.position.z);
	 }
	 
	public void SetCountText()
	{
		countText.text = StaticClass.namePlayer + ": " + count.ToString ();
	}
}
