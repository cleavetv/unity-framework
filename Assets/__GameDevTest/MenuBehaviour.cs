using CleaveFramework.Core;
using UnityEngine;
using System.Collections;
using __GameDevTest;

public class MenuBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    Framework.InjectAsSingleton(new Foo());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUp()
    {
        var foo = Framework.ResolveSingleton<Foo>() as Foo;
        Debug.Log(foo.Value);
        Debug.Log(foo.Word);
       // Framework.PushCommand(new ChangeSceneCmd("Game"));
    }
}
