using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item System/New Item", order = 0)]
public class Item: ScriptableObject
{
	//*==================================================================================*//
	//WARNING:If you add anymore data, it won't be reflected in the Item Browser
	//But it will show up on the Inspector
	//*==================================================================================*//
	public string  Name;
	public string  Description;
	public Texture Icon;
	public int     Cost = 10;
	public bool    ShowInInventory = true;
	//*==================================================================================*//
	//WARNING:If you add anymore data, it won't be reflected in the Item Browser
	//But it will show up on the Inspector
	//*==================================================================================*//
}
