using UnityEngine;
using NUnit.Framework;

[Category("Item System")]
public static class InventoryTest
{
#region NoneTests
	private static Inventory inventory;

	private static Item itemOne;
	private static Item itemTwo;

	static InventoryTest()
	{
		CreateItemOne();
		CreateItemTwo();
	}

	private static void CreateItemOne()
	{
		itemOne                 = ScriptableObject.CreateInstance<Item>();
		itemOne.Name            = "Item One";
		itemOne.Cost            = 253;
		itemOne.ShowInInventory = false;
	}

	private static void CreateItemTwo()
	{
		itemTwo                 = ScriptableObject.CreateInstance<Item>();
		itemTwo.Name            = "Item Two";
		itemTwo.Cost            = 5673434;
		itemTwo.ShowInInventory = true;
	}

	private static void CreateItems()
	{
		DestroyItems();
		inventory = ScriptableObject.CreateInstance<Inventory>();

		if(itemOne == null)
			CreateItemOne();

		if(itemTwo == null)
			CreateItemTwo();
	}

	private static void DestroyItems()
	{
		inventory = null;
		itemOne   = null;
		itemTwo   = null;
	}
#endregion

	[Test(Author = "JMiles42")]
	public static void InventoryAddReturnValue()
	{
		CreateItems();

		var add = inventory.Add(itemOne);

		Assert.NotNull(add);
	}

	[Test(Author = "JMiles42")]
	public static void InventoryFindNothingEmptyInventory()
	{
		CreateItems();

		var found = inventory.FindStack(itemOne);

		Assert.IsNull(found);
	}

	[Test(Author = "JMiles42")]
	public static void InventoryFindNothing()
	{
		CreateItems();
		inventory.Add(itemOne);
		inventory.Add(itemOne);
		inventory.Add(itemOne);
		inventory.Add(itemOne);
		inventory.Add(itemOne);

		var found = inventory.FindStack(itemTwo);

		Assert.IsNull(found);
	}

	[Test(Author = "JMiles42")]
	public static void InventoryMultipleAddCheck()
	{
		CreateItems();
		inventory.Add(itemOne);
		inventory.Add(itemOne);
		inventory.Add(itemOne);
		inventory.Add(itemOne);
		inventory.Add(itemOne);

		var found = inventory.FindStack(itemOne);

		Assert.AreEqual(found.Amount, 5);
	}

	[Test(Author = "JMiles42")]
	public static void InventoryFindStack()
	{
		CreateItems();

		var add   = inventory.Add(itemOne);
		var found = inventory.FindStack(itemOne);

		Assert.AreEqual(add, found);
	}

	[Test(Author = "JMiles42")]
	public static void InventoryRemove()
	{
		CreateItems();

		inventory.Add(itemOne);
		inventory.Remove(itemOne);

		Assert.AreEqual(inventory.Count, 0);
	}

	[Test(Author = "JMiles42")]
	public static void InventoryRemoveOverAmount()
	{
		CreateItems();

		inventory.Add(itemOne);
		inventory.Remove(itemOne, 5);

		Assert.AreEqual(inventory.Count, 0);
	}

	[Test(Author = "JMiles42")]
	public static void InventoryRemoveWrongItem()
	{
		CreateItems();

		inventory.Add(itemOne);
		inventory.Remove(itemTwo);

		Assert.AreEqual(inventory.Count, 1);
	}
}
