//made by Anders Nørgaard, contact ajes1337@gmail.com for info. 2013-2020
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

public class InGameFrameTimer : MonoBehaviour {

    private Material _mat;
    private readonly List<Vector2> _startPos = new List<Vector2>();
    private readonly List<Vector2> _middlePos = new List<Vector2>();
    private readonly List<Vector2> _stopPos = new List<Vector2>();
    private readonly List<bool> _gcRanThatFrame = new List<bool>();
    private readonly List<float> _times = new List<float>();
    private readonly Stopwatch _renderTime = Stopwatch.StartNew();
    private float _renderTimeLastFrame = 0;
    private long _lastGcAmount;
    private int _currentRefreshRate = 60;
    private readonly List<int> _gcCollected = new List<int>();
    private readonly List<int> _gcCollectedPosX = new List<int>();
    private long _memUsed = 0;
    private string _avgFpS = "";
    private readonly Stopwatch _sw = Stopwatch.StartNew();
    private string _targetFrameRateString;
    private string _avgString = "Avg.";
    public int BottomLeftPosX = 10;
    public int BottomLeftPosY = 40;
    public int GraphLength = 200;
    public float GraphScale = 10;
    public bool ShowGcCollectedAmount = true;
    public bool DisableVsync = true;
    public bool ActivatedOnF2 = true;
    public TargetFrameRateSetting ChooseTargetFrameRate = TargetFrameRateSetting.ToMonitorHz;//TripleMonitorHz
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
    [DllImport("__Internal")]
    static extern long mono_gc_get_used_size();  //Amount Used
    [DllImport("__Internal")]
    static extern long mono_gc_get_heap_size(); //Total Amount
#endif
    private Camera cam;

    public enum TargetFrameRateSetting {
        DoNotChange,
        NoLimit,
        ToMonitorHz,
        DoubleMonitorHz,
        TripleMonitorHz
    }

    void Start() {
        _currentRefreshRate = Screen.currentResolution.refreshRate;
        if (DisableVsync) {
            QualitySettings.vSyncCount = 0;
        }

        switch (ChooseTargetFrameRate) {
            case TargetFrameRateSetting.DoNotChange:
                break;
            case TargetFrameRateSetting.NoLimit:
                Application.targetFrameRate = 0;
                break;
            case TargetFrameRateSetting.ToMonitorHz:
                Application.targetFrameRate = _currentRefreshRate;
                break;
            case TargetFrameRateSetting.DoubleMonitorHz:
                Application.targetFrameRate = _currentRefreshRate * 2;
                break;
            case TargetFrameRateSetting.TripleMonitorHz:
                Application.targetFrameRate = _currentRefreshRate * 3;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        _targetFrameRateString = Application.targetFrameRate.ToString();

        var shader = Shader.Find("Hidden/Internal-Colored");
        _mat = new Material(shader);

        cam = gameObject.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Depth;
        cam.cullingMask = 0 << 0;

        _times.Add(1);
        _times.Add(1);
        _times.Add(1);
        _times.Add(1);
        _times.Add(1);

        _gcAllocPrFrame.Add(1);
        _gcAllocPrFrame.Add(2);
        _heapSizeDirectly = "1";
        _memUsedDirectly = "1";
        LastmemUsedDirectlyFloat = 1f;
    }

    private string _targetFpsString = "Target FPS";
    private string _mbString = "MB";
    private string _skraaStreg = "/";
    private string _memUsedDirectly;
    private string _heapSizeDirectly;
    private float LastmemUsedDirectlyFloat;
    private readonly List<float> _gcAllocPrFrame = new List<float>();
    private string _gcAllocbad = "<- GC Alloc (bad)";

    private void OnGUI() {
        if (!ActivatedOnF2) {
            return;
        }
        GUI.skin.label.alignment = TextAnchor.UpperLeft;

        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5, Screen.height - (1 + BottomLeftPosY + (1f / (_currentRefreshRate) * 1000) * GraphScale) - 30, 100, 20), _avgFpS);
        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5 + 25, Screen.height - (1 + BottomLeftPosY + (1f / (_currentRefreshRate) * 1000) * GraphScale) - 30, 100, 20), _avgString);

        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5, Screen.height - (1 + BottomLeftPosY + (1f / (_currentRefreshRate) * 1000) * GraphScale) - 10, 100, 20), fpsString1);
        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5, Screen.height - (1 + BottomLeftPosY + (1f / (_currentRefreshRate * 2) * 1000) * GraphScale) - 10, 100, 20), fpsString2);
        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5, Screen.height - (1 + BottomLeftPosY + (1f / (_currentRefreshRate * 3) * 1000) * GraphScale) - 10, 100, 20), fpsString3);

        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5, Screen.height - BottomLeftPosY - 10, 100, 20), _memUsedDirectly);
        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5 + 30, Screen.height - BottomLeftPosY - 10, 100, 20), _skraaStreg);
        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5 + 40, Screen.height - BottomLeftPosY - 10, 100, 20), _heapSizeDirectly);
        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5 + 70, Screen.height - BottomLeftPosY - 10, 100, 20), _mbString);

        //LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5, Screen.height - BottomLeftPosY + 10, 100, 20), _avgGcAlloc);
        LabelWithShadow(new Rect(BottomLeftPosX + GraphLength + 5, Screen.height - BottomLeftPosY + 10, 200, 20), _gcAllocbad);

        LabelWithShadow(new Rect(50, Screen.height - 24, 100, 20), _targetFpsString);
        string s = GUI.TextField(new Rect(10, Screen.height - 23, 40, 20), _targetFrameRateString);
        if (s != "" && _targetFrameRateString != s) {
            if (IsDigitsOnly(s)) {
                Application.targetFrameRate = Convert.ToInt32(s);
                _targetFrameRateString = Application.targetFrameRate.ToString();
            }
        }
        if (Convert.ToString(Application.targetFrameRate) != _targetFrameRateString) {
            _targetFrameRateString = Application.targetFrameRate.ToString();
        }

        if (ShowGcCollectedAmount) {
            for (int i = 0; i < _gcCollected.Count; i++) {
                int collectedMb = _gcCollected[i];
                LabelWithShadow(new Rect(BottomLeftPosX + _gcCollectedPosX[i], Screen.height - BottomLeftPosY, 100, 20), collectedMb.ToString());
                LabelWithShadow(new Rect(BottomLeftPosX + _gcCollectedPosX[i] + 20, Screen.height - BottomLeftPosY, 100, 20), _mbString);

            }
        }
    }

    private string fpsString1;
    private string fpsString2;
    private string fpsString3;
    private int _lastCurrentRefreshRate;

    private void OnRefreshRateChange() {
        fpsString1 = _currentRefreshRate + " fps";
        fpsString2 = _currentRefreshRate * 2 + " fps";
        fpsString3 = _currentRefreshRate * 3 + " fps";
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F2)) {
            ActivatedOnF2 = !ActivatedOnF2;
        }
        if (!ActivatedOnF2) {
            return;
        }


#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        _heapSizeDirectly = ((int)(mono_gc_get_heap_size() / 1024000)).ToString();//mono_gc_get_heap_size
        int memUsedDirectlyInteger = (int)(mono_gc_get_used_size() / 1024000);//mono_gc_get_used_size
        _memUsedDirectly = memUsedDirectlyInteger.ToString();

        float memUsedDirectlyFloat = (mono_gc_get_used_size() / 1024000f);//mono_gc_get_used_size
        float usedLastFrame = memUsedDirectlyFloat - LastmemUsedDirectlyFloat;
        LastmemUsedDirectlyFloat = memUsedDirectlyFloat;

        if (usedLastFrame >= 0) {
            _gcAllocPrFrame.Add(usedLastFrame);
        } else {
            float toAdd = 0;
            for (int i = 0; i < _gcAllocPrFrame.Count; i++) {
                toAdd += _gcAllocPrFrame[i];
            }
            toAdd /= _gcAllocPrFrame.Count;
            _gcAllocPrFrame.Add(toAdd);
        }
#endif


        if (_currentRefreshRate != _lastCurrentRefreshRate) {
            OnRefreshRateChange();
            _lastCurrentRefreshRate = _currentRefreshRate;
        }

        while (_startPos.Count > GraphLength) {
            _startPos.RemoveAt(0);
            _middlePos.RemoveAt(0);
            _stopPos.RemoveAt(0);
            _gcRanThatFrame.RemoveAt(0);
            _times.RemoveAt(0);
            _gcAllocPrFrame.RemoveAt(0);
        }

        for (int i = _gcCollected.Count - 1; i >= 0; i--) {
            _gcCollectedPosX[i]--;
            if (_gcCollectedPosX[i] <= 0) {
                _gcCollected.RemoveAt(i);
                _gcCollectedPosX.RemoveAt(i);
            }
        }

        _memUsed = GC.GetTotalMemory(false);
        if (_memUsed < _lastGcAmount) {
            int lastGcRemovedAmount = (int)((_lastGcAmount - _memUsed) / 1024000);
            if (ShowGcCollectedAmount) {
                _gcCollected.Add(lastGcRemovedAmount);
                _gcCollectedPosX.Add(GraphLength);
            }
            _gcRanThatFrame.Add(true);
        } else {
            _gcRanThatFrame.Add(false);
        }
        _lastGcAmount = _memUsed;


        float width = Screen.width;
        float height = Screen.height;

        float msDeltaTime = Time.deltaTime * 1000;

        _times.Add(msDeltaTime);
        _startPos.Add(new Vector3((GraphLength + BottomLeftPosX + 0.5f) / width, (BottomLeftPosY + 0.5f) / height));
        _middlePos.Add(new Vector3((GraphLength + BottomLeftPosX + 0.5f) / width, (BottomLeftPosY + 0.5f + (msDeltaTime - _renderTimeLastFrame) * GraphScale) / height));
        _stopPos.Add(new Vector3((GraphLength + BottomLeftPosX + 0.5f) / width, (BottomLeftPosY + 0.5f + msDeltaTime * GraphScale) / height));


        if (_sw.Elapsed.TotalSeconds > 1f) {
            _sw.Reset();
            _sw.Start();
            float totalTime = 0f;

            for (int i = _times.Count - 5; i < _times.Count; i++) {//for avg over 5 frames
                totalTime += _times[i];
            }
            _avgFpS = ((int)(1f / ((totalTime / 5) * 0.001f))).ToString();

        }

    }

    private void LateUpdate() {
        if (!ActivatedOnF2) {
            return;
        }
        _renderTime.Start();
    }

    private IEnumerator OnPostRender() {
        if (!ActivatedOnF2) {
            yield break;
        }

        yield return new WaitForEndOfFrame();

        float width = Screen.width;
        float height = Screen.height;

        GL.PushMatrix();
        _mat.SetPass(0);
        GL.LoadOrtho();
        GL.Begin(GL.LINES);

        for (int i = 0; i < _startPos.Count; i++) {
            Vector3 start = _startPos[i];
            Vector3 middle = _middlePos[i];
            Vector3 stop = _stopPos[i];

            Color greenColor;
            Color blueColor;
            if (_gcRanThatFrame[i]) {
                blueColor = Color.magenta;
                greenColor = Color.magenta;
            } else {
                blueColor = Color.blue;
                greenColor = Color.green;
            }

            GL.Color(blueColor);
            GL.Vertex(start);
            GL.Vertex(middle);
            GL.Color(greenColor);
            GL.Vertex(middle);
            GL.Vertex(stop);

            GL.Color(new Color(1, 0, 1, 0.5f));
            GL.Vertex(new Vector3((GraphLength + BottomLeftPosX + 0.5f - (_startPos.Count - i)) / width, (BottomLeftPosY + 0.5f) / height));
            GL.Vertex(new Vector3((GraphLength + BottomLeftPosX + 0.5f - (_startPos.Count - i)) / width, (BottomLeftPosY + 0.5f - _gcAllocPrFrame[i] * 100) / height));

            _startPos[i] = new Vector3(start.x - (1 / width), start.y);
            _middlePos[i] = new Vector3(middle.x - (1 / width), middle.y);
            _stopPos[i] = new Vector3(stop.x - (1 / width), stop.y);
        }

        GL.Color(Color.yellow);

        GL.Vertex(new Vector3((BottomLeftPosX + 0.5f) / width, (0.5f + BottomLeftPosY) / height));
        GL.Vertex(new Vector3((BottomLeftPosX + GraphLength + 1f) / width, (0.5f + BottomLeftPosY) / height));

        GL.Vertex(new Vector3((BottomLeftPosX + 0.5f) / width, (1.5f + BottomLeftPosY + (1f / (_currentRefreshRate * 3) * 1000) * GraphScale) / height));
        GL.Vertex(new Vector3((BottomLeftPosX + GraphLength + 1f) / width, (1.5f + BottomLeftPosY + (1f / (_currentRefreshRate * 3) * 1000) * GraphScale) / height));

        GL.Vertex(new Vector3((BottomLeftPosX + 0.5f) / width, (1.5f + BottomLeftPosY + (1f / (_currentRefreshRate * 2) * 1000) * GraphScale) / height));
        GL.Vertex(new Vector3((BottomLeftPosX + GraphLength + 1f) / width, (1.5f + BottomLeftPosY + (1f / (_currentRefreshRate * 2) * 1000) * GraphScale) / height));

        GL.Vertex(new Vector3((BottomLeftPosX + 0.5f) / width, (1.5f + BottomLeftPosY + (1f / _currentRefreshRate * 1000) * GraphScale) / height));
        GL.Vertex(new Vector3((BottomLeftPosX + GraphLength + 1f) / width, (1.5f + BottomLeftPosY + (1f / _currentRefreshRate * 1000) * GraphScale) / height));

        GL.End();
        GL.PopMatrix();

        _renderTime.Stop();
        _renderTimeLastFrame = (float)_renderTime.Elapsed.TotalMilliseconds;
        _renderTime.Reset();
    }

    private void LabelWithShadow(Rect rect, string s) {
        Color oldColor = GUI.color;
        GUI.color = Color.black;
        GUI.Label(new Rect(rect.x + 1, rect.y + 1, rect.width, rect.height), s);
        GUI.color = oldColor;
        GUI.Label(new Rect(rect.x, rect.y, rect.width, rect.height), s);
    }

    private bool IsDigitsOnly(string str) {
        return str.All(c => c >= '0' && c <= '9');
    }
}
