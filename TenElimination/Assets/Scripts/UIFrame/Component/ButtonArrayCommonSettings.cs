using UnityEngine;
using System.Collections;

[System.Serializable]
public class ButtonArrayCommonSettings {
    public enum TransitionMode { 
        ColorTint = 1,
        SpriteSwap = 2,
    }
    public TransitionMode mode;
    public Color normalColor;
    public Color highlightedColor;
    public Color pressedColor;
    public Color disabledColor;
    public Sprite highlightedSprite;
    public Sprite pressedSprite;
    public Sprite disabledSprite;
}
