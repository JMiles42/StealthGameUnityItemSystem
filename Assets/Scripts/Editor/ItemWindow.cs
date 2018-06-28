using ForestOfChaosLib.AdvVar.Editor;
using ForestOfChaosLib.Editor;
using ForestOfChaosLib.Editor.Windows;
using ForestOfChaosLib.Extensions;
using ForestOfChaosLib.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ItemWindow: FoCsWindow<ItemWindow>
{
	[MenuItem("Window/" + TITLE)]
	private static void Init()
	{
		GetWindowAndShow();
		Window.titleContent.text = TITLE;
		Window.minSize           = Window.minSize.SetX(ASSET_PANEL_MIN_WIDTH + EDIT_PANEL_WIDTH);
	}

	private const string GUI_SELECTION_LABEL   = "ItemWindowSelectItemID";
	private const string TITLE                 = "Item Browser";
	private const float  ASSET_PANEL_MIN_WIDTH = 150;
	private const float  ASSET_PANEL_PERCENT   = 0.2f;
	private const float  EDIT_PANEL_WIDTH      = 450;
	private const float  FOOTER_HEIGHT         = 18;

	public                  List<Item>        ItemAssets;
	private                 int               ActiveIndex;
	private static readonly GUILayoutOption[] ToggleOp = {GUILayout.ExpandWidth(true), GUILayout.Height(18)};

	private void OnEnable()
	{
		RefreshAssets();
	}

	private void Update()
	{
		if(mouseOverWindow)
		{
			Repaint();
		}
	}

	protected override void OnGUI()
	{
		using(FoCsEditor.Disposables.HorizontalScope())
		{
			using(FoCsEditor.Disposables.VerticalScope(GUILayout.Width(Screen.width * ASSET_PANEL_PERCENT), GUILayout.MinWidth(ASSET_PANEL_MIN_WIDTH)))
			{
				if(DrawItemAssetPanel())
					return;
			}

			using(FoCsEditor.Disposables.VerticalScope())
			{
				if(ItemAssets.IsNullOrEmpty())
				{
					DrawNothingFound();
				}

				if(ItemAssets.InRange(ActiveIndex))
					if(DrawItemEditPanel(ItemAssets[ActiveIndex]))
						return;
			}

			using(FoCsEditor.Disposables.VerticalScope(FoCsGUI.Styles.Toolbar, GUILayout.Width(4)))
			{
				FoCsGUI.Layout.Label("", FoCsGUI.Styles.Toolbar, GUILayout.Height(16), GUILayout.Width(4));
			}
		}
	}

	private bool DrawItemAssetPanel()
	{
		using(FoCsEditor.Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, GUILayout.Height(16)))
			FoCsGUI.Layout.Label("Items", FoCsGUI.Styles.ToolbarButton, GUILayout.Height(16));

		using(FoCsEditor.Disposables.VerticalScope(GUILayout.ExpandHeight(true)))
		{
			for(var i = 0; i < ItemAssets.Count; i++)
				DrawItemLabel(i);
		}

		CreateNewItemGUI();

		return false;
	}

	private void CreateNewItemGUI()
	{
		using(FoCsEditor.Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, GUILayout.Height(FOOTER_HEIGHT), GUILayout.ExpandWidth(true)))
		{
			if(FoCsGUI.Layout.Button("Create New Item", FoCsGUI.Styles.ToolbarButton, GUILayout.Height(FOOTER_HEIGHT)))
			{
				CreateAssetsFolder();
				CreateNewItem();
			}
			if(FoCsGUI.Layout.Button("Refresh List", FoCsGUI.Styles.ToolbarButton, GUILayout.Height(FOOTER_HEIGHT)))
			{
				RefreshAssets();
			}
		}
	}

	private void DrawItemLabel(int i)
	{
		GUI.SetNextControlName(GUI_SELECTION_LABEL);

		using(var cc = FoCsEditor.Disposables.ChangeCheck())
		{
			var @event = FoCsGUI.Layout.Toggle(ItemAssets[i].name.SplitCamelCase(), ActiveIndex == i, FoCsGUI.Styles.ToolbarButton, ToggleOp);

			if(cc.changed && @event)
			{
				GUI.FocusControl(GUI_SELECTION_LABEL);

				ActiveIndex = i;
			}
		}
	}

	private bool DrawItemEditPanel(Item item)
	{
		using(FoCsEditor.Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, GUILayout.Height(16)))
			FoCsGUI.Layout.Label(string.Format("Editing: {0}", item.name), FoCsGUI.Styles.ToolbarButton, GUILayout.Height(16));


		using(var cc = FoCsEditor.Disposables.ChangeCheck())
		{
			FoCsGUI.GUIEvent<Texture> Icon;
			FoCsGUI.GUIEvent<string>  Name;
			FoCsGUI.GUIEvent<string>  Description;
			FoCsGUI.GUIEvent<int>     Cost;
			FoCsGUI.GUIEvent<bool>    ShowInInventory;

			using(FoCsEditor.Disposables.LabelFieldSetWidth(200f))
			{
				using(FoCsEditor.Disposables.HorizontalScope(GUILayout.Height(16 * 5), GUILayout.Height(16)))
				{
					EditorGUILayout.GetControlRect(false, 16, GUILayout.Width(5));

					using(FoCsEditor.Disposables.VerticalScope(GUILayout.Width(64)))
					{
						FoCsGUI.Layout.Label("Icon", GUILayout.Width(64));
						Icon = FoCsGUI.Layout.ObjectField<Texture>(GUIContent.none, item.Icon, false, GUILayout.Width(64), GUILayout.Height(16 * 4));
					}

					using(FoCsEditor.Disposables.VerticalScope(GUILayout.ExpandWidth(true)))
					{
						FoCsGUI.Layout.Label();
						Name            = FoCsGUI.Layout.TextField(new GUIContent("Name"), item.Name, GUILayout.Height(16));
						Cost            = FoCsGUI.Layout.IntField(new GUIContent("Cost"), item.Cost, GUILayout.Height(16));
						ShowInInventory = FoCsGUI.Layout.ToggleField(new GUIContent("Show In Inventory"), item.ShowInInventory, GUILayout.Height(16));
					}
				}

				using(FoCsEditor.Disposables.HorizontalScope())
				{
					using(FoCsEditor.Disposables.VerticalScope(GUILayout.ExpandHeight(true)))
					{
						FoCsGUI.Layout.Label("Description");
						Description = FoCsGUI.Layout.TextArea(item.Description, GUILayout.Height(16 * 3));
					}
				}

				using(FoCsEditor.Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, GUILayout.Height(FOOTER_HEIGHT)))
				{
					using(FoCsEditor.Disposables.ColorChanger(Color.red))
					{
						if(FoCsGUI.Layout.Button("Delete item: Warning this is an unrecoverable action. As it deletes the related Item Asset", FoCsGUI.Styles.ToolbarButton, GUILayout.Height(18)))
						{
							if(EditorUtility.DisplayDialog(string.Format("Delete Item:{0}", item.name), string.Format("Are you sure you want to delete Item:{0}\nAsset Name:{1}", item.Name, item.name), "Yes, Delete.", "No, Cancel."))
							{
								AssetDatabase.DeleteAsset(FoCsEditor.AssetPath(item));
								RefreshAssets();
							}
						}
					}
				}
			}

			if(cc.changed)
			{
				item.Icon            = Icon;
				item.Name            = Name;
				item.Description     = Description;
				item.Cost            = Cost;
				item.ShowInInventory = ShowInInventory;
				EditorUtility.SetDirty(item);
			}
		}

		return false;
	}

	private void CreateNewItem()
	{
		AdvPopupWindow.SetUpInstance(new AdvPopupWindowArguments {WindowTitle = "Create New Item", OnGUI = DrawCreateNewItem, OnClose = (a) => RefreshAssets()});
	}

	private static Item newItem = null;

	public static void DrawCreateNewItem(AdvPopupWindowArguments args)
	{
		if(newItem == null)
		{
			newItem      = CreateInstance<Item>();
			newItem.name = "New Item Name";
		}

		newItem.name            = FoCsGUI.Layout.TextField(new GUIContent("Asset Name"), newItem.name);
		newItem.Name            = FoCsGUI.Layout.TextField(new GUIContent("Item Name"),  newItem.Name);
		newItem.Icon            = FoCsGUI.Layout.ObjectField<Texture>(new GUIContent("Icon"), newItem.Icon, false, GUILayout.Height(16));
		newItem.Cost            = FoCsGUI.Layout.IntField(new GUIContent("Cost"), newItem.Cost);
		newItem.ShowInInventory = FoCsGUI.Layout.ToggleField(new GUIContent("Show In Inventory"), newItem.ShowInInventory);

		FoCsGUI.Layout.Label("Assets will be saved to \"Assets/Resources/Items\"");

		var path    = string.Format("Assets/Resources/Items/{0}.asset", newItem.name);
		var hasFile = AssetDatabase.AssetPathToGUID(path).IsNullOrEmpty();
		FoCsGUI.Layout.Label(hasFile? "Valid Filepath" : "Their is a conflict with the asset path + name");

		using(FoCsEditor.Disposables.ColorChanger(hasFile? Color.green : Color.red))
		{
			using(FoCsGUI.Layout.HorizontalScope())
			{
				if(FoCsGUI.Layout.Button("Save Asset & Create Another"))
				{
					DoSaveAssetToDisk(path, hasFile);
					newItem = CreateInstance<Item>();
					Window.RefreshAssets();
				}

				if(FoCsGUI.Layout.Button("Save Asset & Close"))
				{
					DoSaveAssetToDisk(path, hasFile);
					args.Window.Close();
				}
			}
		}
	}

	private static void DoSaveAssetToDisk(string path, bool hasFile)
	{
		if(!hasFile)
		{
			if(EditorUtility.DisplayDialog(string.Format("Overwrite Asset:{0}", path), string.Format("Are you sure you want to overwrite the asset at path:{0}", path), "Yes, overwrite.", "No, Cancel."))
			{
				AssetDatabase.DeleteAsset(path);
				AssetDatabase.CreateAsset(newItem, string.Format("Assets/Resources/Items/{0}.asset", newItem.name));
			}
		}
		else
		{
			AssetDatabase.CreateAsset(newItem, string.Format("Assets/Resources/Items/{0}.asset", newItem.name));
		}
	}

	private void RefreshAssets()
	{
		if(ItemAssets == null)
		{
			ItemAssets = new List<Item>(FoCsAssetFinder.FindAssetsByType<Item>());
		}
		else
		{
			ItemAssets.Clear();
			ItemAssets.AddRange(FoCsAssetFinder.FindAssetsByType<Item>());
		}

		if(ItemAssets.IsNullOrEmpty())
			ActiveIndex = 0;
		else
			ActiveIndex = ItemAssets.InRange(ActiveIndex)? ActiveIndex : ItemAssets.Count - 1;
	}

	private static void CreateAssetsFolder()
	{
		if(!AssetDatabase.IsValidFolder("Assets/Resources"))
			AssetDatabase.CreateFolder("Assets", "Resources");

		if(!AssetDatabase.IsValidFolder("Assets/Resources/Items"))
			AssetDatabase.CreateFolder("Assets/Resources", "Items");
	}

	private static bool DrawNothingFound()
	{
		using(FoCsEditor.Disposables.HorizontalScope(FoCsGUI.Styles.Toolbar, GUILayout.Height(16)))
			FoCsGUI.Layout.Label("Nothing Found", FoCsGUI.Styles.ToolbarButton, GUILayout.Height(16));

		FoCsGUI.Layout.WarningBox("No Item Assets Found.");

		return false;
	}
}
