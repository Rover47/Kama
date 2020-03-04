using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TSS;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoaderMap : MonoBehaviour
{
    public bool _flagLoad = false;
    public string _pathFromStreamingFolder;
    public Vector2 _sizeImage;
    public int _countArrayWidth;
    public int _countArrayHeight;
    public List<bool> _listReady = new List<bool>();
    public List<string> _attachFiles = new List<string>();

    public GameObject _contentTransform;
    public GameObject _prefabElementMap;

    public Transform _panelGrayMap;
    public Transform _panelWindowMap;
    public RectTransform _groupMinimap;

    public TSSProfile _directAlphaProfile;
    
    public void FunctionLoadContent()
    {
        StartCoroutine(LoadContent());
    }

    IEnumerator LoadContent()
    {
        GameObject emptyGO = new GameObject();
        string pathToFolder = Path.Combine(Application.streamingAssetsPath, _pathFromStreamingFolder);
        string[] dirs = Directory.GetDirectories(pathToFolder);

        string pathToFile = Path.Combine(dirs[0], "size.txt");
        if (!File.Exists(pathToFile)) { Debug.LogError("Size file not found!!!"); }

        string[] vextorSize = File.ReadAllLines(pathToFile);
        float.TryParse(vextorSize[0], out _sizeImage.x);
        float.TryParse(vextorSize[1], out _sizeImage.y);
        int.TryParse(vextorSize[2], out _countArrayWidth);
        int.TryParse(vextorSize[3], out _countArrayHeight);

        _groupMinimap.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sizeImage.x);
        _groupMinimap.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _sizeImage.y);

        RectTransform container = _contentTransform.GetComponent<RectTransform>();
        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sizeImage.x);
        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _sizeImage.y);
        
        TSSCore core = GetComponent<TSSCore>();
        _directAlphaProfile  = Resources.Load<TSSProfile>("DirectAlpha");
        bool lastFlag;

        for (int j = 0; j < dirs.Length; j++)
        {
            functionLoadForAttach(dirs[j]);

            List<string> currentListImage = _attachFiles;
            int currentArrayWidth = 0, currentArrayHeight = 0;
            lastFlag = j == dirs.Length - 1 ? true : false;

            GameObject groupObject = Instantiate(emptyGO, _contentTransform.transform);
            groupObject.name = j == 0 ? "Map" : groupObject.name = "Prep_" + j.ToString("00");
            groupObject.AddComponent<RectTransform>();
            groupObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sizeImage.x);
            groupObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _sizeImage.y);

            if (j ==0)
            {
                TSSItem newStateObject = new TSSItem();
                TSSState stateState = core.AddState(newStateObject, (j).ToString());
                stateState.AddSelectionKey((KeyCode)((int)KeyCode.Alpha0 ));
            }
            else
            {
                groupObject.AddComponent<CanvasGroup>();
                TSSItem newStateObject = groupObject.AddComponent<TSSItem>();
                TSSState stateState = core.AddState(newStateObject, (j).ToString());
                stateState.AddSelectionKey((KeyCode)((int)KeyCode.Alpha1 + j - 1));
                newStateObject.profile = _directAlphaProfile;

                TSSProfile.ProfileRevert(newStateObject, _directAlphaProfile);
            }

            foreach (var element in currentListImage)
            {
                using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(element))
                {
                    yield return uwr.SendWebRequest();
                    if (uwr.isNetworkError || uwr.isHttpError) { Debug.Log(uwr.error); }
                    else
                    {
                        var texture = DownloadHandlerTexture.GetContent(uwr);
                        texture.Compress(true);
                        texture.Apply(false, true);

                        GameObject _currentMapElement = Instantiate(_prefabElementMap, groupObject.transform);
                        _currentMapElement.GetComponent<RawImage>().texture = texture;
                        _currentMapElement.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, texture.width);
                        _currentMapElement.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, texture.height);
                        _currentMapElement.GetComponent<RectTransform>().anchoredPosition = new Vector2(currentArrayWidth * 8000, currentArrayHeight * -8000);

                        currentArrayWidth++;
                        if (currentArrayWidth >= _countArrayWidth)
                        {
                            currentArrayHeight++;
                            currentArrayWidth = 0;
                        }
                    }
                    uwr.Dispose();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            if (lastFlag) CreateMinimap();
        }

        core.useEvents = true;
        TSS.Base.TSSBehaviour.RefreshAndStart();
        Destroy(emptyGO);
        _flagLoad = true;
    }

    private void CreateMinimap()
    {
        GameObject _forGrayMap = Instantiate(_contentTransform, _panelGrayMap, false);
        Destroy(_forGrayMap.GetComponent<ResizeOnMouseWheel>());
        _forGrayMap.AddComponent<CanvasGroup>().alpha = 0.3f;

        for (int i = 1; i < _forGrayMap.transform.childCount; i++)
            Destroy(_forGrayMap.transform.GetChild(i).gameObject); 

        GameObject _forWindowMap = Instantiate(_contentTransform, _panelWindowMap, false);
        Destroy(_forWindowMap.GetComponent<ResizeOnMouseWheel>());


        TSSCore core = GetComponent<TSSCore>();
        for (int i = core.Count - 1; i > 0; i--)
        {
            core[i].AddItem(_forWindowMap.transform.GetChild(i).GetComponent<TSSItem>());
            _forWindowMap.transform.GetChild(i).SetParent(_forWindowMap.transform.parent.parent.parent, false);
        }
    }

    public void functionLoadForAttach(string _DirName)
    {
        string exts = ".png";
        DirectoryInfo directory = new DirectoryInfo(_DirName);

        var files = Directory.GetFiles(directory.FullName, "*.*", SearchOption.AllDirectories)
            .Where(s => exts.Any(x => s.EndsWith(x.ToString(), StringComparison.OrdinalIgnoreCase)));

        _attachFiles = files.ToList();
    }
}
