using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

//Version 1.1 - updated for Unity 5x

//PROBLEM:
//This script is provided because I found that I didn't want to soak up too much texture space with static props
//needing a lot of UV space in lightmaps. My work-around is to not have the props (currently the barrel and crate)
//generate a second set of UVs for lightmaps. But I DO flag them as static and throwing shadows so that their
//shadows are baked into the level lighmaps. However I have found that the mesh renderer component for the props
//after a lightmap bake become corrupt in some way and are over-bright, ignoring normal maps etc.

//SOlUTION:
//My solution is to delete the corrupt prop entirely, after having cloned it with this script first. The clone does not appear over bright.
//In UNity 4 I could simply delete the mesh renderer component but for some reason that doesn't work with Unity 5x.
//This script goes through the selection looking for items on a layer called 'Props' and will automatically clone then delete the selected prop
//when run from the editor 'tools' menu.

//METHOD:
//1.	Make sure all your props are on a layer called 'Props'. If you don't have one just make one. I include this
//		as a failsafe. since you don't want to do this to anything except props.
//2.	Select all your props that need to refresh.
//3.	Choose Tools/Renew Mesh Renderer from the editor 'Tools' menu.
//4.	That's it! Check to see it worked.

public class RefreshStaticProps : ScriptableObject
{

	[MenuItem ("Tools/Delete Old Props")]
	static void DeleteOldProps()
	{
		Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);
		foreach(Transform myTransform in transforms)
		{
			var selObject = myTransform.gameObject;
            //get the old name to re-use otherwise Unity appends '(clone)' to new objects name
            var oldName = selObject.name;
            var oldParent = selObject.transform.parent;
            //If the old object was parented to something then store the parent to give the new object the same relationship
            GameObject clone;
			if(LayerMask.LayerToName(selObject.layer)== "Props")
			{
				//Debug.Log ("Got a Prop!!");
				Debug.Log ("Current object = " + selObject.name);

				clone = Instantiate(selObject);
                //set the new object to have the old objects name
                clone.name = oldName;
                clone.transform.parent = oldParent;

                DestroyImmediate(selObject);
            }
		}

	}
}
