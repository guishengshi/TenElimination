using UnityEngine;
using System.Collections;

public class BlockDeadState : IState {
    private BlockManager mBlockManager;
    private uint mID;

    public BlockDeadState(BlockManager manager, BlockState state) {
        mBlockManager = manager;
        mID = (uint)state;
    }

    public uint GetStateID()
    {
        return mID;
    }

    public void OnEnter(StateManager manager, IState prevState, object para1, object para2)
    {
        mBlockManager.OnEnterDeadState();
        mBlockManager.MGameCenter.MCoreGame.MMusicManager.MMusicPlayer.PlayDestroyMusic();
        mBlockManager.blockCollider.enabled = false;
        mBlockManager.blockDeadEffect.gameObject.SetActive(true);
        mBlockManager.StartCoroutine(DarkGradually(mBlockManager.blockSprite, 1f, () => {
            mBlockManager.blockDeadEffect.gameObject.SetActive(false);
            mBlockManager.Hide(); 
        }));
        AwardScore();
    }

    public void OnExit(IState nextIState, object para1, object para2)
    {
        
    }

    public void OnUpdate(float timeDelta)
    {
        
    }

    public void OnFixUpdate(float timeDelta)
    {
        
    }

    public void OnLateUpdate(float timeDelta)
    {
        
    }

    private void AwardScore() {
        InGameWindow inGameWindow = mBlockManager.MGameCenter.MCoreGame.MUIManager.MInGameWindow as InGameWindow;
        mBlockManager.MGameCenter.Score += CommonSetting.scorePerBlock;
        inGameWindow.MScoreShowController.AddScore(CommonSetting.scorePerBlock);
    }

    public delegate void OnEndCall();

    IEnumerator DarkGradually(SpriteRenderer image, float time, OnEndCall endCall = null)
    {
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
        if (endCall != null) {
            endCall();
        }
    }
}
