using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CStestTexture : MonoBehaviour {

    [SerializeField] private ComputeShader _computeShaderTextureTest;
    [SerializeField] private GameObject _plane;
    private RenderTexture _renderTexture;
    private Renderer _renderer;
    private int _kernelIndex_PanelColor;

    struct ThreadSize {
        public int x;
        public int y;
        public int z;

        public ThreadSize(uint x, uint y, uint z) {
            this.x = (int)x;
            this.y = (int)y;
            this.z = (int)z;
        }
    }

    private ThreadSize _kernelThreadSize_PanelColor;

    // Use this for initialization
    void Start () {
        //Initialized RenderTexture
        _renderTexture = new RenderTexture(512, 512, 0, RenderTextureFormat.ARGB32);

        _renderTexture.enableRandomWrite = true;
        _renderTexture.Create();

        //ComputeShader settings
        _kernelIndex_PanelColor = _computeShaderTextureTest.FindKernel("PanelColor");

        uint threadSizeX;
        uint threadSizeY;
        uint threadSizeZ;

        _computeShaderTextureTest.GetKernelThreadGroupSizes
            (_kernelIndex_PanelColor, out threadSizeX, out threadSizeY, out threadSizeZ);

        _kernelThreadSize_PanelColor = new ThreadSize(threadSizeX, threadSizeY, threadSizeZ);

        //Output Setting
        _computeShaderTextureTest.SetTexture
            (_kernelIndex_PanelColor, "Result", _renderTexture);

        _renderer = _plane.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        _computeShaderTextureTest.SetFloats("Time", Time.frameCount);   //Send frame time for computeShader
        _computeShaderTextureTest.Dispatch(_kernelIndex_PanelColor,
                                           _renderTexture.width / _kernelThreadSize_PanelColor.x,
                                           _renderTexture.height / _kernelThreadSize_PanelColor.y,
                                           _kernelThreadSize_PanelColor.z);

        _renderer.material.mainTexture = _renderTexture;
	}
}
