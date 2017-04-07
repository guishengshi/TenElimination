using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewBehaviourScript : MonoBehaviour {
    public GameObject cube;
    public GameObject other;
	// Use this for initialization
	void Start () {
        InversionString("I am a good man !");
        Test();
        Debug.LogWarning (N(8));
	}
	
	// Update is called once per frame
	void Update () {
        cube.transform.Rotate(other.transform.up, 30f * Time.deltaTime);
	}

    public void InversionString(string str) {
        string[] strs = str.Split(' ');
        Stack <string> s = new Stack <string>();
        for (int i = 0; i < strs.Length; i++) {
            s.Push(strs [i]);
        }
        System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
        while (s.Count > 0) {
            sb2.Append(s.Pop());
            sb2.Append(" ");
        }
        sb2.Remove(sb2.Length - 1, 1);
        Debug.LogWarning(sb2.ToString());
    }

    public void Test() {
        int i1 = 2;
        int i2 = i1;
        int i3 = i1;
        Debug.LogWarning (Object.ReferenceEquals(i2, i3));
    }
    public int N(int n) {
        if (n == 1) {
            return 1;
        }
        if (n == 2) {
            return 1;
        }
        if (n > 2) {
            return N(n - 2) + N(n - 1);
        }
        return 0;
    }
}

