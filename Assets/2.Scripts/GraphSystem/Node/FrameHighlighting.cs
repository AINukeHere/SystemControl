using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameHighlighting : MonoBehaviour
{
    //Assigned
    public Transform targetTr;

    [SerializeField]
    private Transform UpFrame;
    [SerializeField]
    private Transform DownFrame;
    [SerializeField]
    private Transform LeftFrame;
    [SerializeField]
    private Transform RightFrame;

    [SerializeField]
    private Color frameColor;

    public float frameWidth = 0.2f;



    public bool flashEffect = false;
    public int flashCount = -1;
    private float timeLine = 0;

    void OnEnable()
    {
        UpFrame.GetComponent<SpriteRenderer>().color = frameColor;
        DownFrame.GetComponent<SpriteRenderer>().color = frameColor;
        LeftFrame.GetComponent<SpriteRenderer>().color = frameColor;
		RightFrame.GetComponent<SpriteRenderer>().color = frameColor;
		if (flashCount != -1) {
			UpFrame.gameObject.SetActive (false);
			DownFrame.gameObject.SetActive (false);
			LeftFrame.gameObject.SetActive (false);
			RightFrame.gameObject.SetActive (false);
		}
		FixPosition();
    }
    void Update()
    {
        FixPosition();

        if (flashEffect)
        {
            if (timeLine <= 0)
            {
                if (flashCount == -1)
                {
                    timeLine = 1.0f;
                }
                else if (flashCount > 0)
                {
                    flashCount--;
                    timeLine = 1.0f;
                }
            }
            timeLine -= Time.deltaTime;
            if (timeLine > 0.5f)
            {
                UpFrame.gameObject.SetActive(false);
                DownFrame.gameObject.SetActive(false);
                LeftFrame.gameObject.SetActive(false);
                RightFrame.gameObject.SetActive(false);
            }
            else
            {
                UpFrame.gameObject.SetActive(true);
                DownFrame.gameObject.SetActive(true);
                LeftFrame.gameObject.SetActive(true);
                RightFrame.gameObject.SetActive(true);
            }
        }
    }
    void FixPosition()
    {
        if (targetTr != null)
        {
            //위치 : 타겟위치 + 타겟y스케일 + 프레임두깨*0.5
            UpFrame.position = targetTr.position + (targetTr.lossyScale.y + frameWidth) * targetTr.up;
            UpFrame.localScale = new Vector3(targetTr.lossyScale.x + frameWidth*2, frameWidth, 1.0f);
            UpFrame.rotation = targetTr.rotation;

            DownFrame.position = targetTr.position - (targetTr.lossyScale.y + frameWidth ) * targetTr.up;
            DownFrame.localScale = new Vector3(targetTr.lossyScale.x + frameWidth * 2, frameWidth, 1.0f);
            DownFrame.rotation = targetTr.rotation;

            LeftFrame.position = targetTr.position - (targetTr.lossyScale.x + frameWidth ) * -targetTr.right;
            LeftFrame.localScale = new Vector3(frameWidth, targetTr.lossyScale.y + frameWidth * 2, 1.0f);
            LeftFrame.rotation = targetTr.rotation;

            RightFrame.position = targetTr.position + (targetTr.lossyScale.x + frameWidth ) * -targetTr.right;
            RightFrame.localScale = new Vector3(frameWidth, targetTr.lossyScale.y + frameWidth * 2, 1.0f);
            RightFrame.rotation = targetTr.rotation;
        }
    }
}
