using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Item System/New Inventory", order = 0)]
public class Inventory: ScriptableObject
{
	[SerializeField] private List<ItemStack> items = new List<ItemStack>();

	/// <summary>
	/// It is recommended not to edit this list e.g. add items, as this could cause duplicate stacks
	/// Use this to loop through the list
	/// </summary>
	public List<ItemStack> Items
	{
		get { return items; }
	}

	/// <summary>
	/// Adds the item stack to the existing stack, or creates a new stack
	/// </summary>
	/// <param name="item"></param>
	/// <returns>The Added or Modified Stack</returns>
	public ItemStack Add(Item item)
	{
		var iStack = FindStack(item);

		if(iStack)
		{
			iStack++;

			return iStack;
		}

		var stack = new ItemStack(item);
		items.Add(stack);

		return stack;
	}

	/// <summary>
	/// Adds the item stack to the existing stack, or creates a new stack
	/// It adds the Amount of the stack as well
	/// </summary>
	/// <param name="item"></param>
	/// <returns>The Added or Modified Stack</returns>
	public ItemStack Add(ItemStack item)
	{
		var iStack = FindStack(item);

		if(iStack)
		{
			iStack.Amount += item.Amount;

			return iStack;
		}

		var stack = new ItemStack(item);
		items.Add(stack);

		return stack;
	}

	/// <summary>
	/// Finds stack that contains the Item
	/// </summary>
	/// <param name="item"></param>
	/// <returns>The Found Stack</returns>
	public ItemStack FindStack(Item item)
	{
		foreach(var itemStack in items)
		{
			if(itemStack.Item == item)
			{
				return itemStack;
			}
		}

		return null;
	}

	/// <summary>
	/// Finds stack that contains BOTH the Item and >= Amount
	/// </summary>
	/// <param name="item"></param>
	/// <returns>The Found Stack</returns>
	public ItemStack FindStack(ItemStack item)
	{
		foreach(var itemStack in items)
		{
			if((itemStack.Item == item) && (itemStack.Amount >= item.Amount))
			{
				return itemStack;
			}
		}

		return null;
	}
}