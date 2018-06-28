using System;
using ForestOfChaosLib.Extensions;
using UnityEngine;

[Serializable]
public class ItemStack
{
	[SerializeField] private Item item;
	[SerializeField] private int  amount;

	public Item Item
	{
		get { return item; }
		set { item = value; }
	}

	public int Amount
	{
		get { return amount; }
		set { amount = value.Clamp(0, int.MaxValue); }
	}

	public ItemStack(Item _item, int _amount = 1)
	{
		item   = _item;
		amount = _amount;
	}

	public ItemStack(ItemStack _stack): this(_stack.item, _stack.amount) { }

	/// <summary>
	/// Adds one from amount
	/// </summary>
	/// <param name="stack"></param>
	/// <returns></returns>
	public static ItemStack operator ++(ItemStack stack)
	{
		stack.Amount++;

		return stack;
	}

	/// <summary>
	/// Remove one from amount
	/// </summary>
	/// <param name="stack"></param>
	/// <returns></returns>
	public static ItemStack operator --(ItemStack stack)
	{
		stack.Amount--;

		return stack;
	}

	public static implicit operator Item(ItemStack input)
	{
		return input.Item;
	}

	public static implicit operator int(ItemStack input)
	{
		return input.Amount;
	}

	/// <summary>
	/// Similar to Unity's Object cast to bool, will return FALSE if it is null, TRUE otherwise
	/// </summary>
	/// <param name="input"></param>
	public static implicit operator bool(ItemStack input)
	{
		if(input == null)
			return false;

		return true;
	}
}
