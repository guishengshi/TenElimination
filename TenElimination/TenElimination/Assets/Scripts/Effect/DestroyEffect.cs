using UnityEngine;
using System.Collections;

public class DestroyEffect : MonoBehaviour {
    public float timer;

	void Start () {
        StartCoroutine(Time());
	}

    IEnumerator Time() {
        yield return new WaitForSeconds(timer);
        GameObject.Destroy(gameObject);
    }
}
