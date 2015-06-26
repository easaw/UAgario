using UnityEngine;
using System.Collections;
using UnityEditor;
[InitializeOnLoad]
public class HierarchQuickSetActive {
	private static bool isEnabled = true;
	/// <summary>
	/// Initializer <see cref="HierarchQuickSetActive"/> class.
	/// </summary>
	static HierarchQuickSetActive ()
	{
		EditorApplication.hierarchyWindowItemOnGUI += hierarchWindowOnGUI;
	//	EditorApplication.projectWindowItemOnGUI += projectWindowOnGUI;
		isEnabled = EditorPrefs.GetBool("quick_setactive",true);
	}
	[MenuItem("Tools/QuickSetActive - Toggle")]
	static void toggleEnable(){
		isEnabled = !isEnabled;
		EditorPrefs.SetBool("quick_setactive",isEnabled);
	}
	/// <summary>
	/// Editor delegate callback
	/// </summary>
	/// <param name="instanceID">Instance id.</param>
	/// <param name="selectionRect">Selection rect.</param>
	static void hierarchWindowOnGUI (int instanceID, Rect selectionRect)
	{
		if(!isEnabled )return;
		// make rectangle
		Rect r = new Rect (selectionRect); 
		r.x = r.width - 10;
		r.width = 18;
		// get objects
		Object o = EditorUtility.InstanceIDToObject(instanceID);
		GameObject g = (GameObject)o as GameObject;
		// drag toggle gui
		if(g)g.SetActive(GUI.Toggle(r,g.activeSelf,string.Empty));

		// Testing some stuff

//		if(g.GetComponent<MeshRenderer>()!=null)
//		{
//			Texture2D mIcon = AssetPreview.GetMiniTypeThumbnail(typeof(MeshRenderer));
//			Rect icon = new Rect (selectionRect); 
//			Rect rend = new Rect (selectionRect); 
//			icon.x = icon.width - 65;
//			icon.width = 18;
//			rend.x = rend.width - 48;
//			rend.width = 18;
//			GUI.Label(icon,mIcon);
//			g.renderer.enabled = GUI.Toggle(rend,g.renderer.enabled,string.Empty);
//			GUI.Label(rend,"---");
//		}
	}

	static void projectWindowOnGUI (string guid, Rect selectionRect)
	{
		string s = AssetDatabase.GUIDToAssetPath(guid);
//		Debug.Log(s);
		if(s.Contains("Audio")){
			Rect r = new Rect (selectionRect); 
			r.x = r.width - 10;
			r.width = 18;
			// create a style with same padding as normal labels. 
			GUIStyle style = new GUIStyle(((GUIStyle)"Hi Label")); 
			style.padding.left = EditorStyles.label.padding.left+16;
			// choose new color 
			style.normal.textColor = Color.red;
			// draw the new colored label over the old one 
			GUI.Label(selectionRect, "Audio", style); 
		}
		GUI.color = Color.white;
	}
}
