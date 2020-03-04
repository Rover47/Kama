using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadBigImage : MonoBehaviour
{
    public Texture2D sprt;

    IEnumerator asd()
    {
        string url = Path.Combine(Application.streamingAssetsPath, "05.png");
        Debug.Log(url);
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Debug.Log(uwr.downloadHandler.data.GetValue(0));
                var texture = DownloadHandlerTexture.GetContent(uwr);
                sprt = texture;
            }
        }
    }
}
