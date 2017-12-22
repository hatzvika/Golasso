using System.Collections;
using UnityEngine;

public static class FirstMenuManager
{


	private static string firstMenuName = "Main Interface";

	public static string GetFirstMenuName ()
	{
		return firstMenuName;
	}

	public static void SetFirstMenuName(string menuName){
		firstMenuName = menuName;
	}
}