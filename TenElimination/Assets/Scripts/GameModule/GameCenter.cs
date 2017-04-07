using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameCenter {
    private CoreGame mCoreGame;
    private GameObject mBg;
    private ObjectPoolWithReset<BlockManager> mBlockPool;
    private BlockManager mCurrentBlock;
    private List<BlockManager> mFalledBlocks = new List<BlockManager>();
    private int mBlockID = 0;
    private List<StoreItemData> mPersonalItemDatas = new List<StoreItemData> ();
    private int mDataSum = -1;
    private float mCurrentBlockSpeed = 0f;
    private float mCurrentNumberChangeSpeed = 0f;
    private List<BaseSkill> mSkills = new List<BaseSkill>();
    private int mScore = 0;
    private GameMode mGameMode = GameMode.Null;

    public GameMode GameMode {
        get {
            return mGameMode;
        }
        set {
            mGameMode = value;
        }
    }

    public int Score {
        get {
            return mScore;
        }
        set {
            mScore = value;
        }
    }

    public float CurrentBlockSpeed {
        get {
            return mCurrentBlockSpeed;
        }
        set {
            mCurrentBlockSpeed = value;
        }
    }

    public float CurrentNumberChangeSpeed {
        get {
            return mCurrentNumberChangeSpeed;
        }
        set {
            mCurrentNumberChangeSpeed = value;
        }
    }

    public List<BlockManager> MFalledBlocks {
        get {
            return mFalledBlocks;
        }
    }

    public CoreGame MCoreGame {
        get {
            return mCoreGame;
        }
    }

    public GameCenter(CoreGame coregame) {
        mCoreGame = coregame;
    }

    public void OnSelectMode() {
        mBg.SetActive(true);
    }

    public void Init() {
        InitComponent();
    }

    public void Loading() {
        
        InitObjectPool();
        InitPersonalItems();
    }

    public void Ready() {
        
    }

    public void StartGame() {
        
        mCurrentBlock = mBlockPool.New();
        mCurrentBlock.Show();
    }

    public void ContinueGame() {
        mCurrentBlock.Continue();
    }

    public void PauseGame() {
        mCurrentBlock.Pause();
    }

    public void OnExit()
    {
        if (mBlockPool == null) {
            return;
        }
        mBlockPool.Clear();
        for (int i = 0; i < mFalledBlocks.Count; i++) {
            mFalledBlocks[i].DestroySelf();
        }
        mFalledBlocks.Clear();
        mBg.SetActive(false);
        mCurrentBlock.DestroySelf();
        mCurrentBlock = null;
        mBlockID = 0;
        mDataSum = -1;
        mPersonalItemDatas.Clear();
        mCurrentBlockSpeed = 0f;
        mSkills.Clear();
        mScore = 0;
        mGameMode = GameMode.Null;
    }


    public void Update(float deltaTime) {
        if (mPersonalItemDatas.Count == mDataSum) {
            InGameWindow window = mCoreGame.MUIManager.MInGameWindow as InGameWindow;
            for (int i = 0; i < mPersonalItemDatas.Count; i++) {
                BaseSkill skill = null;
                switch (mPersonalItemDatas[i].itemType) {
                    case StoreItemType.Block: skill = new Bomb(mPersonalItemDatas[i], this);
                        break;
                    case StoreItemType.Time : skill = new TimeCapsule(mPersonalItemDatas [i], this, mBg);
                        break;
                }
                mSkills.Add(skill);
            }
            window.SetSkill(mSkills);
            mDataSum = -1;
        }
        for (int i = 0; i < mSkills.Count; i++)
        {
            mSkills[i].Update(deltaTime);
        }
    }
    public void LateUpdate(float deltaTime) {
        
    }

    public void FixedUpdate(float deltaTime) {

    }


    private void InitComponent() {
        mBg = Helper.GetPrefabTypeOfGameObject("BG");
        mBg.transform.localPosition = new Vector3(0, 3.6f, 0);
    }

    private void InitObjectPool() {
        int poolSize = CommonSetting.maxBlockNumberPerColumn * CommonSetting.maxBlockNumberPerLine + 1;
        mBlockPool = new ObjectPoolWithReset<BlockManager>(poolSize, NewBlock, null, DestroyBlock);
    }

    private void InitPersonalItems() {
        //AVOSCloud.AVUser user = User.CurrentUser;
        //List<System.Object> list = user.Get<List <System.Object>>("itemsID");
        List<string> list = mCoreGame.MPlayerManager.MPlayerData.itemsID;
        mDataSum = list.Count;
        for (int i = 0; i < list.Count; i++)
        {
            string str = list[i].ToString();
            Quary.GetStoreItem(str, (isSuccess, data) =>
            {
                if (isSuccess)
                {
                    //Debug.LogWarning(data.itemType);
                    mPersonalItemDatas.Add(data);
                }
            });
        }
    }

    private BlockManager NewBlock() {
        BlockManager manager = null;
        GameObject g = Helper.GetPrefabTypeOfGameObject("block");
        manager = g.GetComponent<BlockManager> ();
        Block blockData = new Block(mBlockID++, Block.defaultID, BlockType.Normal);
        manager.Init(blockData, this);
        manager.OnEnterStopEvent += (BlockManager blockManager) => { 
            mCurrentBlock = mBlockPool.New();
            mCurrentBlock.Show();
            mFalledBlocks.Add(blockManager);
        };
        manager.OnHideEvent += (BlockManager blockManager) => { 
            mBlockPool.Store(blockManager); 
        };
        manager.OnEnterDeadEvent += (BlockManager blockManager) =>
        {
            mFalledBlocks.Remove(blockManager);
        };
        //g.SetActive(true);
        return manager;
    }

    private void DestroyBlock(BlockManager block) {
        block.DestroySelf();
    }

    public void LeftMoveCurrentBlock() {
        if (mCurrentBlock.GetCurrentState () == BlockState.Fall)
            mCurrentBlock.LeftMove();
    }

    public void RightMoveCurrentBlock() {
        if (mCurrentBlock.GetCurrentState () == BlockState.Fall)
            mCurrentBlock.RightMove();
    }

    public void DowmMoveCurrentBlock() {
        if (mCurrentBlock.GetCurrentState () == BlockState.Fall)
            mCurrentBlock.DownMove();
    }

    public bool HasBlock(BlockPosition pos) { 
        foreach (BlockManager manager in mFalledBlocks) {
            if (manager.MBlockData.position.Equals(pos)) {
                return true;
            }
        }
        return false;
    }

    public BlockManager GetBlock(BlockPosition pos) {
        foreach (BlockManager m in mFalledBlocks) {
            if (m.MBlockData.position.Equals (pos)) {
                return m;
            }
        }
        return null;
    }

    public bool GetBlocks(List <BlockPosition> poss, out List <BlockManager> managers) {
        bool isHas = false;
        managers = new List<BlockManager>();
        foreach (BlockManager m in mFalledBlocks) {
            for (int i = 0; i < poss.Count; i++) {
                if (poss[i].Equals(m.MBlockData.position)) {
                    managers.Add(m);
                    isHas = true;
                }
            }

        }
        return isHas;
    }

    public bool GetMaxUpPos(int column, out BlockPosition pos) {
        bool isHas = false;
        pos = new BlockPosition();
        List<BlockManager> thisYBlocks = new List<BlockManager>();
        foreach (BlockManager manager in mFalledBlocks) {
            if (manager.MBlockData.position.columnX == column) {
                thisYBlocks.Add(manager);
                isHas = true;
            }
        }
        if (isHas) {
            pos = thisYBlocks[0].MBlockData.position;
            if (thisYBlocks.Count > 1) {
                for (int i = 1; i < thisYBlocks.Count; i++) {
                    if (thisYBlocks[i].MBlockData.position.lineY > pos.lineY) {
                        pos = thisYBlocks[i].MBlockData.position;
                    }
                }
            }
        }
        return isHas;
    }

    public Vector3 GetRandom2x2Block(out List <BlockManager> list) {
        list = new List<BlockManager>();
        if (mFalledBlocks.Count == 0) {
            return Vector3.zero;
        }
        Vector3 centerPos;
        int index = Random.Range(0, mFalledBlocks.Count);
        BlockPosition pos = mFalledBlocks[index].MBlockData.position;
        int colomnIndex = pos.columnX - 1;
        int lineIndex = pos.lineY - 1;
        centerPos = mFalledBlocks[index].MTransform.position + new Vector3(-CommonSetting.blockSize.x / 2, -CommonSetting.blockSize.y / 2, 0);
        if (pos.columnX == BlockPosition.MinColumnX) {
            colomnIndex = pos.columnX + 1;
            centerPos.x = mFalledBlocks[index].MTransform.position.x + CommonSetting.blockSize.x / 2;
        }
        if (pos.lineY == BlockPosition.MinLineY) {
            lineIndex = pos.lineY + 1;
            centerPos.y = mFalledBlocks[index].MTransform.position.y + CommonSetting.blockSize.y / 2;
        }
        BlockManager m1 = GetBlock(new BlockPosition(pos.columnX, lineIndex));
        if (m1 != null) {
            list.Add(m1);
        }
        BlockManager m2 = GetBlock(new BlockPosition(colomnIndex, pos.lineY));
        if (m2 != null) {
            list.Add(m2);
        }
        BlockManager m3 = GetBlock(new BlockPosition(colomnIndex, lineIndex));
        if (m3 != null) {
            list.Add(m3);
        }
        list.Add(mFalledBlocks[index]);
        return centerPos;
    }

    public Vector3 GetCenter4x4Blocks(out List<BlockManager> list)
    {
        list = new List<BlockManager>();
        for (int i = 0; i < mFalledBlocks.Count; i++) {
            BlockPosition pos = mFalledBlocks[i].MBlockData.position;
            if (pos.columnX <= 4 && pos.columnX >= 1 && pos.lineY <= 3) {
                list.Add(mFalledBlocks[i]);
            }
        }
        Vector3 center = new BlockPosition(2, 1).ToVector3() + new Vector3(CommonSetting.blockSize.x / 2, CommonSetting.blockSize.y / 2);
        return center;
    }

    public Vector3 GetBlocksFromColumn(int Column, out List<BlockManager> list) {
        list = new List<BlockManager>();
        for (int i = 0; i < mFalledBlocks.Count; i++)
        {
            BlockPosition pos = mFalledBlocks[i].MBlockData.position;
            if (pos.columnX == Column)
            {
                list.Add(mFalledBlocks[i]);
            }
        }
        Vector3 center = new BlockPosition(Column, 2).ToVector3() + new Vector3(0, CommonSetting.blockSize.y / 2, 0);
        return center;
    }

    public void ThreeLines() {
        for (int i = 0; i < mFalledBlocks.Count; i++) {
            if (mFalledBlocks[i].MBlockData.position.lineY == 0) {
                mFalledBlocks[i].Hide();
                mFalledBlocks.RemoveAt(i);
                i--;
            } else if (mFalledBlocks[i].MBlockData.position.lineY < 3) {
                mFalledBlocks[i].MBlockData.position.lineY -= 1;
                mFalledBlocks[i].ResetPos();
            }
        }
    }

}

public enum GameMode { 
    CustomMode,
    TransformMode,
    TopSpeedMode,
    Null,
}
