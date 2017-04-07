using UnityEngine;
using System.Collections;

public class MusicManager {
    private CoreGame mCoreGame;
    private MusicPlayer mMusicPlayer;
    private static MusicManager mMusicManager;

    public MusicPlayer MMusicPlayer {
        get {
            return mMusicPlayer;
        }
    }

	public MusicManager () {
        Init();
    }

    public static MusicManager GetInstance() {
        if (mMusicManager == null) {
            mMusicManager = new MusicManager();
        }
        return mMusicManager;
    }

    private void Init() {
        mMusicPlayer = Helper.GetPrefabTypeOfGameObject("MusicPlayer").GetComponent<MusicPlayer>();
    }
}
