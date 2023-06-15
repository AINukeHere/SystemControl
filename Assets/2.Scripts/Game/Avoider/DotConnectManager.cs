using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotConnectManager : MonoBehaviour
{
    private static DotConnectManager _instance;
    public static DotConnectManager instance
    {
        get
        {
            return _instance;
        }
    }
    public GameObject wall_prefab;
    struct DotPair
    {
        public int a, b;
        public GameObject wall;
        public DotPair(BigInt a, BigInt b, GameObject wall = null)
        {
            this.a = (int)a;
            this.b = (int)b;
            this.wall = wall;
        }
        public DotPair(int a, int b, GameObject wall = null)
        {
            this.a = a;
            this.b = b;
            this.wall = wall;
        }
        public static bool operator ==(DotPair x, DotPair y)
        {
            if ((x.a == y.a && x.b == y.b) ||
                (x.a == y.b && x.b == y.a))
                return true;
            else
                return false;
        }
        public static bool operator !=(DotPair x, DotPair y)
        {
            if ((x.a == y.a && x.b == y.b) ||
                (x.a == y.b && x.b == y.a))
                return false;
            else
                return true;
        }
        public override bool Equals(object obj)
        {
            Debug.Log("Equals");
            return this == (DotPair)obj;
        }
        public override int GetHashCode()
        {
            Debug.Log("GetHashCode");
            return a ^ b;
        }
    }
    //가비지 발생 방지
    class PairComarer : IEqualityComparer<DotPair>
    {
        bool IEqualityComparer<DotPair>.Equals(DotPair x, DotPair y)
        {
            if ((x.a == y.a && x.b == y.b) ||
                (x.a == y.b && x.b == y.a))
                return true;
            else
                return false;
        }
        int IEqualityComparer<DotPair>.GetHashCode(DotPair obj)
        {
            return obj.a.GetHashCode() ^ obj.b.GetHashCode();
        }
    }
    List<DotPair> DotAndWalls;

    List<Avoider_ToggleWall> walls;

    void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        DotAndWalls = new List<DotPair>();
        walls = new List<Avoider_ToggleWall>();
    }
    public void ResetWalls()
    {
        DotAndWalls.Clear();
        foreach (Avoider_ToggleWall info in walls)
        {
            info.bUsed = false;
        }
    }
    public void InitWallWithStageNum(int stageNum)
    {
        foreach (Dot_Default dot_default in dot_StageDefault[stageNum-1].dot_Defaults)
        {
            SetActiveWall(dot_default);
        }
    }

    //****************디버깅용*****************//
    public bool run = false;
    public int dot1Idx, dot2Idx;
    public bool bActive;
    void Update()
    {
        if(run)
        {
            run = false;
            SetActiveWall(dot1Idx, dot2Idx, bActive);
        }
    }
    //****************디버깅용*****************//


    //스테이지 시작할때 열리거나 닫힐 연결선들을 정의합니다.
    [System.Serializable]
    public class Dot_Default
    {
        public int a, b;
        public bool bActive;
    }
    [System.Serializable]
    public class Dot_StageDefault
    {
        [SerializeField]
        public Dot_Default[] dot_Defaults;
    }
    [SerializeField]
    Dot_StageDefault[] dot_StageDefault;
    public void SetActiveWallWithStageDefault()
    {
        int currentStageNum = (int)AvoidGameManager.instance.GetCurrentStageNum(gameObject);
        if (dot_StageDefault[currentStageNum] != null)
        {
            int len = dot_StageDefault[currentStageNum].dot_Defaults.Length;
            for (int i =0;i < len; ++i)
                SetActiveWall(dot_StageDefault[currentStageNum].dot_Defaults[i]);
        }
    }
    public void SetActiveWall(int dot1, int dot2, bool bActive)
    {
        GameObject Dot1Obj, Dot2Obj;
        Dot1Obj = GameObject.Find("Dot" + dot1);
        Dot2Obj = GameObject.Find("Dot" + dot2);
        //점두개가 실제로 있을 때만 벽이 생기네마네 함
        if (Dot1Obj != null && Dot2Obj != null)
        {
            GameObject temp_wall = null;
            DotPair tempDotPair = new DotPair(dot1, dot2);
            foreach (DotPair dp in DotAndWalls)
            {
                if (dp == tempDotPair)
                    temp_wall = dp.wall;
            }
            Debug.Log("TryGetValue : " + temp_wall);
            if (temp_wall == null)
            {
                //사용되지않는 벽이 있는가 체크
                int i = 0;
                for (i = 0; i < walls.Count; ++i)
                {
                    if (walls[i].bUsed == false)
                        break;
                }
                if (i != walls.Count)
                {
                    Debug.Log("Find unused Wall");
                    temp_wall = walls[i].gameObject;
                }
                else
                {
                    Debug.Log("Create new wall");
                    temp_wall = Instantiate(wall_prefab, AvoidGameManager.instance.GetCurrenStageObjectParent(gameObject).transform);
                    walls.Add(temp_wall.GetComponent<Avoider_ToggleWall>());
                }
                Debug.Log("Add wall");
                temp_wall.GetComponent<Avoider_ToggleWall>().SetDots(Dot1Obj, Dot2Obj);
                DotAndWalls.Add(new DotPair(dot1, dot2, temp_wall));
            }

            temp_wall.GetComponent<Avoider_ToggleWall>().SetActive(bActive);
        }
    }
    public void SetActiveWall(Dot_Default dot_default)
	{
		Debug.Log (dot_default.a + "과 " + dot_default.b + "을 이어라");
        GameObject Dot1Obj, Dot2Obj;
        Dot1Obj = GameObject.Find("Dot" + dot_default.a);
        Dot2Obj = GameObject.Find("Dot" + dot_default.b);
        //점두개가 실제로 있을 때만 벽이 생기네마네 함
        if (Dot1Obj != null && Dot2Obj != null)
        {
            GameObject temp_wall = null;
            DotPair tempDotPair = new DotPair(dot_default.a, dot_default.b);
            foreach (DotPair dp in DotAndWalls)
            {
                if (dp == tempDotPair)
                    temp_wall = dp.wall;
            }
            Debug.Log("TryGetValue : " + temp_wall);
            if (temp_wall == null)
            {
                //사용되지않는 벽이 있는가 체크
                int i = 0;
                for (i = 0; i < walls.Count; ++i)
                {
                    if (walls[i].bUsed == false)
                        break;
                }
                if (i != walls.Count)
                {
                    Debug.Log("Find unused Wall");
                    temp_wall = walls[i].gameObject;
                }
                else
                {
                    Debug.Log("Create new wall");
                    temp_wall = Instantiate(wall_prefab, AvoidGameManager.instance.GetCurrenStageObjectParent(gameObject).transform);
                    walls.Add(temp_wall.GetComponent<Avoider_ToggleWall>());
                }
				Debug.Log("Add wall");
				temp_wall.transform.SetParent (AvoidGameManager.instance.GetCurrenStageObjectParent (gameObject).transform);

                temp_wall.GetComponent<Avoider_ToggleWall>().SetDots(Dot1Obj, Dot2Obj);
                DotAndWalls.Add(new DotPair(dot_default.a, dot_default.b, temp_wall));
            }

            temp_wall.SetActive(dot_default.bActive);
        }
    }
}
