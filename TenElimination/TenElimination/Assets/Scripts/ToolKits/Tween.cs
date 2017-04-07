using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 插值动画类
/// </summary>

public class Tween {
    public delegate bool EndCall();
    public delegate void OnEndCall();

    public static void StraightTransform(Vector3 start, Vector3 end, float distancePerSecond, MonoBehaviour m)
    {
        float distance = Vector3.Distance(start, end);
        float sumTimer = distance / distancePerSecond;
        m.StartCoroutine(StraightTransformFromSumTime(m.gameObject, start, end, sumTimer));
    }

    public static void StraightTransform(Vector3 start,Vector3 direction ,EndCall endCall, float distancePerSecond, MonoBehaviour m) {
        m.StartCoroutine(StraightTransformFromCall (m, start, direction, distancePerSecond, endCall));
    }

    public static void StraightTransform(Vector3 start, Vector3 end, MonoBehaviour m, float sumTime) {
        m.StartCoroutine(StraightTransformFromSumTime(m.gameObject, start, end, sumTime));
    }

    //只控制一个方向的参数，及另两个方向外部程序可控
    public static void TransformOnlyControlOneDirection(Vector3 direction, EndCall endCall, float distancePerSecond, MonoBehaviour m) {
        m.StartCoroutine(StraightTransformFromCallOnlyControllerOneDirection (m, direction, distancePerSecond, endCall));
    }

    public static void PingPong(MonoBehaviour m, Image image, float sumTime, float onceTime, bool startIsShow, OnEndCall endCall = null)
    {
        m.StartCoroutine(PingPongIEnumerator(m, image, sumTime, onceTime, startIsShow, endCall));
    }

    public static void EnLarge(MonoBehaviour m, Transform transform, float sumTime, Vector3 startScale, Vector3 endScale, OnEndCall endCall = null)
    {
        m.StartCoroutine(EnLargeIenumerator(transform, sumTime, startScale, endScale, endCall));
    }

    static IEnumerator EnLargeIenumerator(Transform t, float sumTime, Vector3 startScale, Vector3 endScale, OnEndCall endCall = null)
    {
        t.localScale = startScale;
        float counter = 0;
        while (counter < sumTime) {
            counter += Time.deltaTime;
            t.localScale = Vector3.Lerp(startScale, endScale, counter / sumTime);
            yield return null;
        }
        t.localScale = endScale;
        if (endCall != null) {
            endCall();
        }
    }

    static IEnumerator PingPongIEnumerator(MonoBehaviour m, Image image, float sumTime, float onceTime, bool startIsShow, OnEndCall endCall = null)
    {
        if (startIsShow) {
            Color c = image.color;
            c.a = 1f;
            image.color = c;
        } else {
            Color c = image.color;
            c.a = 0f;
            image.color = c;
        }
        for (float timer = 0f; timer < sumTime; timer += onceTime / 2) {
            if (image.color.a == 1f) {
                yield return m.StartCoroutine (DarkGradually(image, onceTime / 2f));
                continue;
            }
            if (image.color.a == 0f) {
                yield return m.StartCoroutine (LightGradually(image, onceTime / 2f));
                continue;
            }
        }
        if (endCall != null) {
            endCall();
        }
    }

    static IEnumerator LightGradually(Image image, float time) {
        float counter = 0;
        Color c = image.color;
        while (counter <= time) {
            counter += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, counter / time);
            image.color = c;
            yield return null;
        }
        c.a = 1f;
        image.color = c;
    }

    static IEnumerator DarkGradually(Image image, float time) {
        float counter = 0;
        Color c = image.color;
        while (counter <= time)
        {
            counter += Time.deltaTime;
            c.a = Mathf.Lerp(1f, 0f, counter / time);
            image.color = c;
            yield return null;
        }
        c.a = 0f;
        image.color = c;
    }

    static IEnumerator StraightTransformFromCallOnlyControllerOneDirection(MonoBehaviour m, Vector3 direction, float distancePerSecond, EndCall call)
    {
        Vector3 start = Vector3.zero;
        Vector3 endPerSecond = start + direction * distancePerSecond;
        while (!call())
        {
            yield return m.StartCoroutine(StraightTransformFromSumTimeOnlyControllerOneDirection(m.gameObject, start, endPerSecond, 1f, call));
        }
    }

    static IEnumerator StraightTransformFromSumTimeOnlyControllerOneDirection(GameObject g, Vector3 start, Vector3 end, float sumTime, EndCall call = null, OnEndCall onEndCall = null)
    {
        Vector3 preMid = Vector3.zero;
        float counter = 0;
        while ((counter <= sumTime && call == null) || (counter <= sumTime && !call()))
        {
            counter += Time.deltaTime;
            Vector3 mid = Vector3.Lerp(start, end, counter / sumTime);
            g.transform.position += (mid - preMid);
            preMid = mid;
            yield return null;
        }
        if (call == null || !call())
        {
            if (onEndCall != null)
            {
                onEndCall();
            }
        }
    }

    static IEnumerator StraightTransformFromCall(MonoBehaviour m, Vector3 start, Vector3 direction, float distancePerSecond, EndCall call) {
        Vector3 endPerSecond = start + direction * distancePerSecond;
        while (!call ()) {
            yield return m.StartCoroutine(StraightTransformFromSumTime (m.gameObject, start, endPerSecond, 1f, call, () => { 
            start = endPerSecond;
            endPerSecond = start + direction * distancePerSecond;
        }));
        }
    }

    static IEnumerator StraightTransformFromSumTime(GameObject g, Vector3 start, Vector3 end, float sumTime, EndCall call = null, OnEndCall onEndCall = null) {
        float counter = 0;
        while ((counter <= sumTime && call == null) || (counter <= sumTime && !call()))
        {
            counter += Time.deltaTime;
            Vector3 mid = Vector3.Lerp(start, end, counter / sumTime);
            g.transform.position = mid;
            yield return null;
        }
        if (call == null || !call())
        {
            g.transform.position = end;
            if (onEndCall != null) {
                onEndCall();
            }
        }
    }
}
