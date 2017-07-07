using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using SmartLocalization;

public class BackMainButton : MonoBehaviour
{

	public void onClick (int SceneLevel)
	{
		SceneManager.LoadScene (SceneLevel);
		Time.timeScale = 1;
	}
}