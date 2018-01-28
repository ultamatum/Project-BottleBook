using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public Relay[] relays = new Relay[5];
	public HomeBase homeBase;
	public Slider[] sliders = new Slider[5];
	public Text text;

	void Update () 
	{
		sliders[0].value = relays[0].BarHealth();
		sliders[1].value = relays[1].BarHealth();
		sliders[2].value = relays[2].BarHealth();
		sliders[3].value = relays[3].BarHealth();
		sliders[4].value = relays[4].BarHealth();
		sliders[5].value = homeBase.BarHealth ();

		float minutes = Mathf.Floor (Manager.GetTimeLeft() / 60);
		int seconds = (int)(Manager.GetTimeLeft() % 60);

		text.text = minutes + ":" + seconds.ToString("D2");
	}
}
