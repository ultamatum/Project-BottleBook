using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour {

	public Relay[] relays = new Relay[5];
	public Slider slider;

	void Update () 
	{
		slider.value = relays[0].BarHealth();
	}
}
