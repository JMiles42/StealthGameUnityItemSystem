using System;
using ForestOfChaosLib.Extensions;
using UnityEngine;

namespace JMiles42.ItemSystem
{
	[Serializable]
	public class ItemStack: IEquatable<ItemStack>
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


		public static ItemStack operator +(ItemStack stack, int increaseAmount)
		{
			stack.amount += increaseAmount;

			return stack;
		}

		public static ItemStack operator -(ItemStack stack, int decreaseAmount)
		{
			stack.amount -= decreaseAmount;

			return stack;
		}

		public static ItemStack operator ++(ItemStack stack)
		{
			stack.Amount++;

			return stack;
		}

		public static ItemStack operator --(ItemStack stack)
		{
			stack.Amount--;

			return stack;
		}

		public static implicit operator Item(ItemStack input)
		{
			return input.Item;
		}

		public static implicit operator ItemStack(Item input)
		{
			return new ItemStack(input);
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


		public bool Equals(ItemStack other)
		{
			if(ReferenceEquals(null, other))
				return false;

			if(ReferenceEquals(this, other))
				return true;

			return item == other.item && amount == other.amount;
		}


		public override bool Equals(object obj)
		{
			if(ReferenceEquals(null, obj))
				return false;

			if(ReferenceEquals(this, obj))
				return true;

			if(obj.GetType() != this.GetType())
				return false;

			return Equals((ItemStack)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((item != null? item.GetHashCode() : 0) * 397) ^ amount;
			}
		}

		public static bool operator ==(ItemStack left, ItemStack right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(ItemStack left, ItemStack right)
		{
			return !Equals(left, right);
		}
	}
}