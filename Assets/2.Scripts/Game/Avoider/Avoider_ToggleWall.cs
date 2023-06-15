using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avoider_ToggleWall : MonoBehaviour
{
    public float lerpScale = 0.1f;
    private Transform dot1, dot2;
    private SpriteRenderer s_renderer;
    private AudioSource audioSrc;
    private Collider2D collider2d;

    //사용되고있는가 : 
    public bool bUsed
    {
        get;
        set;
    }
    //사라지고 있는 상태인가.
    private bool bDisappear = false;
    public void SetActive(bool bActive)
    {
        if(bActive)
            gameObject.SetActive(true);
        else
        {
            bDisappear = true;
        }
    }
    public void SetDots(GameObject a, GameObject b)
    {
        dot1 = a.transform;
        dot2 = b.transform;
        bUsed = true;

    }
    void Awake()
    {
        s_renderer = GetComponent<SpriteRenderer>();
        audioSrc = GetComponent<AudioSource>();
        collider2d = GetComponent<Collider2D>();
    }
    void OnEnable()
    {
        //Color Setting
        Color result = Color.red;
        result.a = 0.5f;
        s_renderer.color = result;

        //yScale Setting
        Vector3 myScale = transform.localScale;
        myScale.y = 0.075f;
        transform.localScale = myScale;

        bDisappear = false;
        collider2d.enabled = false;
    }
    void Update()
    {
        if (bUsed)
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);

            //Color와 x스케일 갱신 : 벽이 켜지는 이펙트
            UpdateColor(bDisappear);
            UpdateScaleX(bDisappear);

            //위치와 y스케일 갱신 : 점 두개에 맞추기
            UpdatePosition();
            UpdateScaleY();


            //이 오브젝트가 활성화되어있는동안은 무조건 충돌판정
			if(collider2d.enabled == false)
			{
				collider2d.enabled = true;
				audioSrc.Play();
			}
			//옛날 코드
            //Color temp = (s_renderer.color - Color.black);
			//Color.black과의 오차가 거의 없으면
            //if ((temp.r * temp.r + temp.g * temp.g + temp.b * temp.b + temp.a * temp.a) < 0.01)
            //{
                //if(collider2d.enabled == false)
                //{
                    //collider2d.enabled = true;
                    //audioSrc.Play();
                //}
            //}
            //else
               	//collider2d.enabled = false;
            if (bDisappear)
            {
                Color temp2 = s_renderer.color - new Color(1,0,0,0.5f);
                if ((temp2.r* temp2.r + temp2.g* temp2.g + temp2.b* temp2.b + temp2.a* temp2.a) < 0.01)
                    gameObject.SetActive(false);
            }
        }
        else
            gameObject.SetActive(false);
    }
    void UpdateColor(bool bDisappear)
    {
        if(bDisappear)
            s_renderer.color = Color.Lerp(s_renderer.color, new Color(1, 0, 0, 0.5f), lerpScale);
        else
            s_renderer.color = Color.Lerp(s_renderer.color, Color.black, lerpScale);
    }
    void UpdateScaleX(bool bDisappear)
    {
        float originScaleY = transform.localScale.y;
        Vector3 resultScale = transform.localScale;
        if (bDisappear)
        {
            resultScale.y = Mathf.Lerp(originScaleY, 0.075f, lerpScale);
            transform.localScale = resultScale;
        }
        else
        {
            resultScale.y = Mathf.Lerp(originScaleY, 0.15f, lerpScale);
            transform.localScale = resultScale;
        }
    }
    void UpdatePosition()
    {
        Vector2 v = dot2.position - dot1.position;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(v.y, v.x) / Mathf.Deg2Rad);
        Vector3 new_pos = Vector2.Lerp(dot2.position, dot1.position, 0.5f);
        new_pos.z = dot1.position.z;
        transform.position = new_pos;
    }
    void UpdateScaleY()
    {
        float value = Vector2.Distance(dot1.localPosition, dot2.localPosition) / 2;
        transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);
    }
}
