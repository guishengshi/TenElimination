using UnityEngine;
using System.Collections;

public interface ICommonState {
    void Awake();
    void Start();
    void Update(float timeDelta);
    void LateUpdate(float timeDelta);
    void FixedUpdate(float timeDelta);
    void OnEnable();
    void OnDisable();
}
