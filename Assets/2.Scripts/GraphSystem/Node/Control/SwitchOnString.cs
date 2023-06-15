using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchOnString : ActivatableNode
{ 
    [SerializeField]
    private GameObject StringCasePrefab;

    public string input_value;

    public List<StringCase> cases = new List<StringCase>();
    public ActiveOutputModule default_output;

    public override void Awake()
    {
        base.Awake();
		for(int i =0; i < cases.Count; ++i)
		{
			cases [i].SetSwitchHead (this);
		}
    }

    public bool testBollean = false;
    public bool testBollean2 = false;
    public void Update()
    {
		input_value = null;
#if UNITY_EDITOR
        if (testBollean)
        {
            AddCase();
            testBollean = false;
        }
        if (testBollean2)
        {
            DeleteCase();
            testBollean2 = false;
        }
#endif
    }
    public void Input(string input, int unused = 0)
    {
        if (input != null)
        {
            input_value = input;
            CheckOutput();
        }
    }
    public override void CheckOutput()
    {
        if (isActive >= 2 && input_value != null)
        {
            bool isCaseNull = false;
            for (int i = 0; i < cases.Count; ++i)
            {
                if (cases[i].case_value == null)
                {
                    isCaseNull = true;
                    break;
                }
            }
            if(!isCaseNull)
            {
                //case 에의해 activeinputmoudle에 전달
                int i;
                for (i = 0; i < cases.Count; ++i)
                {
                    if (cases[i].case_value == input_value)
                    {
                        cases[i].Active();
                        cases[i].isActive--;
                        isActive--;
                        cases[i].active_output.Active();
                        break;
                    }
                }
                if (i == cases.Count)
                    default_output.Active();
            }
        }
    }
    public void AddCase()
    {
        GameObject _case = Instantiate(StringCasePrefab, 
            gameObject.transform.position + new Vector3(0,-3.2f - 2.4f * cases.Count, 0),
            Quaternion.identity,
            gameObject.transform);
		_case.GetComponent<StringCase> ().SetSwitchHead (this);
        cases.Add(_case.GetComponent<StringCase>());
    }
    public void DeleteCase()
    {
        if(cases.Count > 0)
        {
            EdgeManager.instance.RemoveEdgeFromStringCase(cases[cases.Count - 1]);
            Destroy(cases[cases.Count - 1].gameObject);
            cases.RemoveAt(cases.Count - 1);
        }
    }
}
