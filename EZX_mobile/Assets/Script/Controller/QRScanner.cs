using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using UnityEngine.UI;
using TMPro;

public class QRScanner : MonoBehaviour
{
    [SerializeField]
    private RawImage _rawImageBackground;
    [SerializeField]
    private AspectRatioFitter _aspectRationFitter;
    [SerializeField]
    private RectTransform _scanZone;
    [SerializeField]
    private TextMeshProUGUI Text;

    private bool _isCamAvaible;
    public string result { get; set; }
    private WebCamTexture _cameratexture;
    // Start is called before the first frame update
    void Start()
    {
        SetUpCamera();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCameraRender();

        Scan();
    }

    private void SetUpCamera()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.Log(devices.Length);
        if (devices.Length <= 0)
        {
            _isCamAvaible = false;
            return;
        }
        for (int i = 0; i < devices.Length; i++)
        {
            // if (!devices[i].isFrontFacing)
            // {
                _cameratexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, -(int)_scanZone.rect.height);
                Debug.Log(devices[i].name + "[" + i + "]");
                if(_cameratexture) break;
            // }
        }
        _rawImageBackground.texture = _cameratexture;
        _isCamAvaible = true;
        _cameratexture.Play();
    }

    private void UpdateCameraRender()
    {
        if (!_isCamAvaible)
        {
            return;
        }
        float ratio = (float)_cameratexture.width / (float)_cameratexture.height;
        _aspectRationFitter.aspectRatio = ratio;

        int orientation = -_cameratexture.videoRotationAngle;
        _rawImageBackground.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    private void Scan()
    {
        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_cameratexture.GetPixels32(), _cameratexture.width, _cameratexture.height);
            if (result != null)
            {
                this.result = result.Text;
            }
            else
            {
                this.result = null;
            }
        }
        catch
        {
            this.result = null;
        }
        Text.text = this.result;
    }
}
