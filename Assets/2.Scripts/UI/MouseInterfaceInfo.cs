using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInterfaceInfo : MonoBehaviour {

    enum MovementType
    {
        NONE,
        DRAG,
        DRAG_AND_DROP
    }


    //정보
    [SerializeField]
    private Transform beginTr,endTr;
    [SerializeField]
    private bool bLbtn, bRbtn;
    [SerializeField]
    MovementType movementType;
    [SerializeField]
    private float moveSpeed = 200;
    [SerializeField]
    private float beginDelay = 0.4f;
    [SerializeField]
    private float endDelay = 1.0f;


    //타임라인
    private bool bArrived;
    private float timeLine;
    GameObject LBtn;
    GameObject RBtn;
    void Awake()
    {
        LBtn = transform.Find("L_Effect").gameObject;
        RBtn = transform.Find("R_Effect").gameObject;
    }
    //초기화작업
    void OnEnable()
    {
        timeLine = 0;
        transform.position = beginTr.position;
		bArrived = false;
		LBtn.SetActive(false);
		RBtn.SetActive(false);
    }
	void Update ()
    {
        if (timeLine < beginDelay)
        {
            timeLine += Time.deltaTime;
        }
        //도착할 때까지 이동
        else if(!bArrived)
        {
            switch(movementType)
            {
                case MovementType.NONE:
                    break;
				case MovementType.DRAG:
				case MovementType.DRAG_AND_DROP:
					if (bLbtn)
						LBtn.SetActive (true);
					else if (bRbtn)
						RBtn.SetActive (true);				
                    break;
			default:
				Debug.LogError ("UnknownMovementType : " + movementType);
				break;
            }
            if ((endTr.position - transform.position).magnitude < moveSpeed * Time.deltaTime)
                bArrived = true;
            transform.Translate((endTr.position - transform.position).normalized * moveSpeed * Time.deltaTime);
        }
        else
		{
			switch(movementType)
			{
				case MovementType.NONE:
					break;
				case MovementType.DRAG:				
					break;
				case MovementType.DRAG_AND_DROP:
					if (bLbtn)
						LBtn.SetActive (false);
					else if (bRbtn)
						RBtn.SetActive (false);				
					break;
				default:
					Debug.LogError ("UnknownMovementType : " + movementType);
					break;
			}
            timeLine += Time.deltaTime;
            if (timeLine >= beginDelay + endDelay)
                OnEnable();
        }
	}
}
