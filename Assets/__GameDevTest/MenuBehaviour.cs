using CleaveFramework.Core;
using UnityEngine;
using System.Collections;

public class MenuBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUp()
    {
        Debug.Log("Cube clicked.");
        Framework.PushCommand(new ChangeSceneCmd("Game"));
    }
}
