using System.Runtime.InteropServices;
using UnityEngine;

public class MicrophoneManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void initMicrophone();

    void Start()
    {
        // Memanggil fungsi JavaScript untuk memulai microphone
#if UNITY_WEBGL && !UNITY_EDITOR
            initMicrophone();
#endif
    }
}
