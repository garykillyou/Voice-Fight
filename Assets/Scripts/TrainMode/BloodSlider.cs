using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodSlider: MonoBehaviour {

	public float MaxHP;
	public float HP;
	public Image bar;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		bar.fillAmount = HP / MaxHP;
	}
}
