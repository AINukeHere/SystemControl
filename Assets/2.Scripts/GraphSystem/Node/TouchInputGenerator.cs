using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputGenerator : MonoBehaviour, IMovable, IExpandableDisplay {

    [SerializeField]
    private float gen_time;
    private float elapsed_time;

    private int isActive = 0;
    private SpriteRenderer myRenderer;
    private TextMesh textMesh;

    public bool isExpanded
    {
        get;set;
    }
    public bool isMoving
    {
        get;set;
    }
    void Awake()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        textMesh = GetComponentInChildren<TextMesh>();
    }
	void Update ()
    {
        elapsed_time += Time.deltaTime;
        Color temp = myRenderer.color;
        if (elapsed_time >= gen_time)
        {
            elapsed_time -= gen_time;
            Collider2D[] colls = Physics2D.OverlapAreaAll(transform.position - transform.lossyScale, transform.position + transform.lossyScale);
            foreach (Collider2D coll in colls)
            {
                TouchEvent touch_input = coll.GetComponent<TouchEvent>();
                if (touch_input != null)
                {
                    touch_input.Active();
                    isActive = 2;
                    break;
                }
            }
        }
        if (isActive >= 1)
        {
            temp.a = 1;
            isActive--;
        }
        else
            temp.a = 0.5f;
        myRenderer.color = temp;

        if (isExpanded)
            ExpandDisplay();
        else
            NormalDisplay();
    }
    public void Move(Vector2 pos, bool bGroup=false)
    {
        Collider2D[] colls = Physics2D.OverlapAreaAll((Vector3)pos - transform.lossyScale, (Vector3)pos + transform.lossyScale);
        int i;
        for (i = 0; i < colls.Length; ++i)
            if (colls[i].CompareTag("LockField"))
                break;
        if (i == colls.Length)
            transform.position = pos;
    }
    public void MoveEnd(Vector2 pos, bool bGroup = false) { }
    public void NormalDisplay()
    {
        textMesh.text= "";
    }
    public void ExpandDisplay()
    {
        textMesh.text = gen_time.ToString("N");
    }
}
