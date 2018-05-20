using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSTestArray : MonoBehaviour {

    [SerializeField] private ComputeShader _computeShader;
    int CSFunction_testIndex;
    ComputeBuffer intComputeBuffer;

	// Use this for initialization
	void Start () {
        //カーネルインデックスの保存
        CSFunction_testIndex = _computeShader.FindKernel("CSFunction_test");

        intComputeBuffer = new ComputeBuffer(6, sizeof(int));   //ComputeShaderの初期化

        //ComputeShaderにどのカーネルバッファを設定するのか，カーネルバッファの名前の設定
        //C#側で使うComputeBufferの指定，これを再び定義してあげることで
        //同一ComputeShader内の別カーネルの計算をComputeShader側の変数を
        //共有のまま計算命令できる ****ここ結構重要****
        _computeShader.SetBuffer(CSFunction_testIndex, "intBuffer", intComputeBuffer);

        _computeShader.SetInt("intValue", 1);   //ComputeShaderに値を渡す
        _computeShader.Dispatch(CSFunction_testIndex, 1, 1, 1);  //ComputeShaderの実行

        int[] result = new int[6];  //ComputeShaderの結果受け取り用
        intComputeBuffer.GetData(result);   //結果の取り出し

        for (int i = 0; i < 6; i++) {
            Debug.Log(result[i]);   //結果を出力
        }

        intComputeBuffer.Release();
	}
}
