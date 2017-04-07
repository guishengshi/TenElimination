using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 游戏物体方块的控制类
/// </summary>
public class BlockManager : MonoBehaviour, IResetable {
    public float blockSpeed;
    public Vector3 startPoint = new Vector3(0, 35.2f, 0);
    public float endPostionY = 0f;
    public SpriteRenderer numberSprite;
    public SpriteRenderer blockSprite;
    public BoxCollider2D blockCollider;
    public Vector2 blockSize = new Vector2(4.4f, 4.4f);
    public float leftBorder = -13f;
    public float rightBorder = 13f;
    public GameObject blockDeadEffect;

    public delegate void OnEnterStopStateCall(BlockManager manager);
    public event OnEnterStopStateCall OnEnterStopEvent;
    public delegate void OnEnterDeadStateCall(BlockManager manager);
    public event OnEnterDeadStateCall OnEnterDeadEvent;
    public delegate void OnHideCall(BlockManager manager);
    public event OnHideCall OnHideEvent;

    private StateManager mStateManager;
    private GameObject mGameObject;
    private Transform mTransform;
    //private List<BlockManager> mCollisionedBlocks = new List<BlockManager>();
    private Block mBlockData;
    private GameCenter mGameCenter;
    private Sprite[] mNumberSpriteArray;
    private Sprite[] mBlockSpriteArray;

    public GameCenter MGameCenter {
        get {

            return mGameCenter;
        }
    }


    public GameObject MGameObject
    {
        get { return mGameObject; }
        set { mGameObject = value; }
    }

    public Transform MTransform
    {
        get {
            return mTransform; 
        }
        set { mTransform = value; }
    }


    public StateManager MStateManager
    {
        get { return mStateManager; }
        set { mStateManager = value; }
    }

    //public List<BlockManager> MCollisionedBlocks {
    //    get {
    //        return mCollisionedBlocks;
    //    }
    //}

    public Block MBlockData { 
        get {
            return mBlockData;
        }
    }


    public void Hide() {
        mGameObject.SetActive(false);
        if (OnHideEvent != null)
        {
            OnHideEvent(this);
        }
    }

    public void Show() {
        mGameObject.SetActive(true);
    }

    public void Init(Block data, GameCenter gameCenter) {
        mBlockData = data;
        mGameCenter = gameCenter;
        InitComponent();
        InitSprites();
        InitStateManager();
        //Show();
        //Reset();
    }

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        mStateManager.OnUpdate(Time.deltaTime);
    }

    void LateUpdate()
    {
        mStateManager.OnLateUpdate(Time.deltaTime);
    }

    void FixedUpdate()
    {
        mStateManager.OnFixUpdate(Time.deltaTime);
    }

    void OnEnable()
    {
        Reset();
    }

    void OnDisable()
    {
        
    }

    private List<BlockManager> mBlockColliders = new List<BlockManager>();

    public List <BlockManager> BlockColliders {
        get {
            return mBlockColliders;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (mStateManager.CurrentStateID != (uint)BlockState.Fall) {
            return;
        }
        BlockManager colliderBlockManager = coll.gameObject.GetComponent <BlockManager>();
        mBlockColliders.Add(colliderBlockManager);
        if (colliderBlockManager.mBlockData.position.columnX == this.mBlockData.position.columnX)
        {
            this.mBlockData.position.lineY = colliderBlockManager.mBlockData.position.lineY + 1;
            mStateManager.SwitchState((uint)BlockState.Stop, mBlockData.position, mBlockColliders);
            
        }
    
    } 

    private void InitStateManager() {
        mStateManager = new StateManager();
        mStateManager.RigisterState(new BlockFallState(this, BlockState.Fall));
        mStateManager.RigisterState(new BlockStopState(this, BlockState.Stop));
        mStateManager.RigisterState(new BlockDeadState (this, BlockState.Dead));
        mStateManager.RigisterState(new BlockPauseState(this, BlockState.Pause));
    }

    private void InitSprites() {
        mNumberSpriteArray = new Sprite[9];
        mBlockSpriteArray = new Sprite[9];
        for (int i = 0; i < 9; i++) {
            mNumberSpriteArray[i] = Helper.GetSprite((i + 1).ToString());
            mBlockSpriteArray[i] = Helper.GetSprite("fangkuai" + (i + 1).ToString());
        }
    }

    private void InitComponent() {
        mGameObject = gameObject;
        mTransform = transform;
    }

    private void AddNumToSprite() {
        blockSprite.sprite = mBlockSpriteArray[mBlockData.number - 1];
        numberSprite.sprite = mNumberSpriteArray[mBlockData.number - 1];
    }

    private void ResetNumBer() {
        int randomNum = NumberRandomArithmetic.RandomNumber();
        mBlockData.number = randomNum;
    }

    private void ResetBlockData() {
        mBlockData.position.columnX = CommonSetting.startBlockPosition.columnX;
        mBlockData.position.lineY = CommonSetting.startBlockPosition.lineY;
    }

    private void ResetBlockSprite() {
        Color c = blockSprite.color;
        c.a = 1;
        blockSprite.color = c;
    }

    public void Reset()
    {
        blockSpeed = mGameCenter.CurrentBlockSpeed;
        mTransform.position = CommonSetting.startBlockPosition.ToVector3();
        blockCollider.enabled = true;
        ResetBlockSprite();
        ResetBlockData();
        ResetNumBer();
        AddNumToSprite();
        Show();
        mStateManager.SwitchState((uint)BlockState.Fall, null, null);
    }

    public void ChangeNumber() {
        ResetNumBer();
        AddNumToSprite();
    }

    public void OnEnterStopState()
    {
        if (mGameCenter.GameMode == GameMode.CustomMode || mGameCenter.GameMode == GameMode.TransformMode) 
            mGameCenter.CurrentBlockSpeed += CommonSetting.blockSpeedInsertAmount;
        if (OnEnterStopEvent != null)
            OnEnterStopEvent(this);
    }

    public void OnEnterDeadState() {
        if (OnEnterDeadEvent != null)
            OnEnterDeadEvent(this);
    }

    public void LeftMove() {
        if (mBlockData.position.columnX <= BlockPosition.MinColumnX) {
            return;
        }
        float f = mTransform.position.y / CommonSetting.blockSize.y;
        int floor = (int)f;
        if (mGameCenter.HasBlock(new BlockPosition(mBlockData.position.columnX - 1, floor)))
        {
            return;
        }
        if ((mTransform.position.y % CommonSetting.blockSize.y > CommonSetting.errorRange) && mGameCenter.HasBlock(new BlockPosition(mBlockData.position.columnX - 1, floor + 1))) {
            return;
        }
        this.mBlockData.position.columnX -= 1;
        mTransform.position = new Vector3(mBlockData.position.ToVector3().x, mTransform.position.y, 0);
    }

    public void RightMove() {
        if (mBlockData.position.columnX >= BlockPosition.MaxColumnX)
        {
            return;
        }
        float f = mTransform.position.y / blockSize.x;
        int floor = (int)f;
        if (mGameCenter.HasBlock(new BlockPosition(mBlockData.position.columnX + 1, floor)))
        {
            return;
        }
        if ((mTransform.position.y % CommonSetting.blockSize.y > CommonSetting.errorRange) && mGameCenter.HasBlock(new BlockPosition(mBlockData.position.columnX + 1, floor + 1)))
        {
            return;
        }
        this.mBlockData.position.columnX += 1;
        mTransform.position = new Vector3(mBlockData.position.ToVector3().x, mTransform.position.y, 0);
    }

    public void DownMove() {
        if (mTransform.position.y <= endPostionY) {
            return;
        }
        BlockPosition bottomPos = null;
        if (mGameCenter.GetMaxUpPos(mBlockData.position.columnX, out bottomPos))
        {
            mBlockData.position.lineY = bottomPos.lineY + 1;
        }
        else {
            mBlockData.position.lineY = 0;
        }
        mStateManager.SwitchState((uint)BlockState.Stop, mBlockData.position, null);
    }

    public BlockState GetCurrentState() {
        return (BlockState)mStateManager.CurrentStateID;
    }

    public void ResetPos() {
        mTransform.position = mBlockData.position.ToVector3();
    }

    public void Dead() {
        mStateManager.SwitchState((uint)BlockState.Dead, null, null);
    }

    public void DestroySelf() {
        GameObject.Destroy(mGameObject);
    }

    public void Pause() {
        if (mStateManager.CurrentStateID != (uint)BlockState.Fall) {
            return;
        }
        mStateManager.SwitchState((uint)BlockState.Pause, null, null);
    }

    public void Continue() {
        if (mStateManager.CurrentStateID != (uint)BlockState.Pause) {
            return;
        }
        mStateManager.SwitchState((uint)BlockState.Fall, null, null);
    }

}
