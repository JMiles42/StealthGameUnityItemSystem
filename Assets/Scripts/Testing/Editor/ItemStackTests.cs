using NUnit.Framework;
using UnityEngine;
[Category("Item System")]
public static class ItemStackTests
{
#region NoneTests
	private static Item itemOne;
	private static Item itemTwo;

	static ItemStackTests()
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
#endregion
	[Test(Author = "JMiles42")]
	public static void ItemStackCtorSingleItem()
	{
		var stack = new ItemStack(itemOne);
		Assert.AreEqual(stack.Amount, 1);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackCtorSingleItemWithAmount()
	{
		var stack = new ItemStack(itemOne, 2);
		Assert.AreEqual(stack.Amount, 2);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackCtorOtherItemStack()
	{
		var stack  = new ItemStack(itemOne, 2);
		var stack2 = new ItemStack(stack);
		Assert.AreEqual(stack2, stack);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackPostfixIncrement()
	{
		var stack = new ItemStack(itemOne, 2);
		stack++;
		Assert.AreEqual(stack.Amount, 3);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackPostfixDecrement()
	{
		var stack = new ItemStack(itemOne, 2);
		stack--;
		Assert.AreEqual(stack.Amount, 1);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackPrefixIncrement()
	{
		var stack = new ItemStack(itemOne, 5);
		++stack;
		Assert.AreEqual(stack.Amount, 6);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackPrefixDecrement()
	{
		var stack = new ItemStack(itemOne, 2);
		--stack;
		Assert.AreEqual(stack.Amount, 1);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackAdditionOperator()
	{
		var stack = new ItemStack(itemOne, 5);
		stack = stack + 5;
		Assert.AreEqual(stack.Amount, 10);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackSubtractionOperator()
	{
		var stack = new ItemStack(itemOne, 25);
		stack = stack - 5;
		Assert.AreEqual(stack.Amount, 20);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackAdditionEqualsOperator()
	{
		var stack = new ItemStack(itemOne, 5);
		stack += 5;
		Assert.AreEqual(stack.Amount, 10);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackSubtractionEqualsOperator()
	{
		var stack = new ItemStack(itemOne, 25);
		stack -= 5;
		Assert.AreEqual(stack.Amount, 20);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackCastFromItemNullCheck()
	{
		var stack = (ItemStack)itemOne;
		Assert.NotNull(stack);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackCastFromItemVsCtor()
	{
		var stack  = (ItemStack)itemOne;
		var stack2 = new ItemStack(itemOne);
		Assert.AreEqual(stack, stack2);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackComparisonFailDifferentItems()
	{
		var stack  = new ItemStack(itemOne);
		var stack2 = new ItemStack(itemTwo);
		Assert.AreNotEqual(stack, stack2);
	}

	[Test(Author = "JMiles42")]
	public static void ItemStackComparisonFailDifferentAmounts()
	{
		var stack  = new ItemStack(itemOne);
		var stack2 = new ItemStack(itemOne, 7);
		Assert.AreNotEqual(stack, stack2);
	}

}
