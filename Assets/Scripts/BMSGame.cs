using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using UnityEngine;
using BMS;
using NAudio.Wave;
using PlaynomicsPlugin;
using SimpleBMSPlayer;
using Ude;
using UnityEngine.UI;
using ThreadPriority = UnityEngine.ThreadPriority;

public class BMSGame : MonoBehaviour
{

    public Dictionary<long, AudioClip> resources = new Dictionary<long, AudioClip>(); 
    public string Path;
    public Text ChartTitle, ChartArtist, ChartGenre, LoadingText;
    Chart chart;
    private FileInfo fileinfo;

    // Use this for initialization
    void Start () {
         fileinfo = new FileInfo(Path);
        string filecontent = LoadFileContent(fileinfo);
        switch (fileinfo.Extension.ToLower())
        {
            case ".bms":
            case ".bme":
            case ".bml":
            case ".pms":
                chart = new BMSChart(filecontent);
                break;
            case ".bmson":
                chart = new BmsonChart(filecontent);
                break;
            default:
                Console.WriteLine("Unknown file type {0}.", fileinfo.Extension);
                return;
        }
        chart.Parse(ParseType.Header | ParseType.Content | ParseType.Resources);
        ChartTitle.text = chart.Title;
        ChartArtist.text = chart.Artist;
        ChartGenre.text = chart.Genre;
        StartCoroutine(LoadBMS());
    }

    void OnBmsEvent(BMSEvent bmsEvent)
    {
        Debug.Log("BMS EVENT (" + bmsEvent.type.ToString() + ")");
        AudioClip sound;
        switch (bmsEvent.type)
        {
                case BMSEventType.WAV:
                if (resources.TryGetValue(bmsEvent.data2, out sound))
                {
                    GetComponent<AudioSource>().PlayOneShot(sound);
                }
                break;

            case BMSEventType.Note:
                if (resources.TryGetValue(bmsEvent.data2, out sound))
                {
                    GetComponent<AudioSource>().PlayOneShot(sound);
                }
                break;

            case BMSEventType.LongNoteStart:
                if (resources.TryGetValue(bmsEvent.data2, out sound))
                {
                    GetComponent<AudioSource>().PlayOneShot(sound);
                }
                break;
        }


    }

    IEnumerator LoadBMS()
    {
        yield return LoadKeysounds(chart, fileinfo);
        Chart.EventDispatcher dispatcher = chart.GetEventDispatcher();
        dispatcher.BMSEvent += OnBmsEvent;
        yield return PlayChart(dispatcher);
    }

    IEnumerator LoadKeysounds(Chart chart, FileInfo fileInfo)
    {
        int maxResources = chart.IterateResourceData(ResourceType.wav).Count();
        int c = 0;
        LoadingText.text = "0 OUT OF " + maxResources;
        foreach (BMSResourceData resourceData in chart.IterateResourceData(ResourceType.wav))
        {
            FileInfo resFileInfo = FindRes(fileInfo.DirectoryName, resourceData.dataPath, ".wav");
            if (resFileInfo != null)
            {
                WWW www = new WWW("file://" + resFileInfo.FullName);
                while (!www.isDone)
                {
                    
                }
                AudioClip clip = www.audioClip;
                resources[(int)resourceData.resourceId] = clip;
                c++;
                LoadingText.text = c + " OUT OF " + maxResources;
                yield return null;
            }
        }


    }

    IEnumerator PlayChart(object obj)
    {
        Debug.Log("Starting BMS");
        Chart.EventDispatcher dispatcher = obj as Chart.EventDispatcher;
        DateTime startDateTime = DateTime.Now;
        for (TimeSpan current = TimeSpan.Zero, end = dispatcher.EndTime + TimeSpan.FromSeconds(1); current <= end; current = DateTime.Now - startDateTime)
        {
            dispatcher.Seek(current);
            Thread.Sleep(0);
            yield return null;
        }
        yield return null;
    }

    static FileInfo FindRes(string basePath, string originalPath, string checkType)
    {
        var finfo = new FileInfo(System.IO.Path.Combine(basePath, originalPath));
        if (finfo.Exists)
            return finfo;
        if (!finfo.Directory.Exists)
            return null;
        string path = finfo.Name;
        string extension = finfo.Extension;
        if (extension.Equals(checkType, StringComparison.OrdinalIgnoreCase))
        {
            var files = finfo.Directory.GetFiles(path.Substring(0, path.Length - extension.Length) + ".*");
            if (files.Length > 0)
                return files[0];
        }
        return null;
    }

    static string LoadFileContent(FileInfo fileInfo)
    {
        string result;
        Encoding encoding;
        try
        {
            encoding = Encoding.GetEncoding(932);
        }
        catch (Exception e)
        {
            encoding = Encoding.UTF8;
        }
        using (Stream stream = fileInfo.OpenRead())
        {
            CharsetDetector detector = new CharsetDetector();
            detector.Feed(stream);
            detector.DataEnd();
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream, encoding))
                result = reader.ReadToEnd();
        }
        return result;
    }


    // Update is called once per frame
    void Update () {
		
	}

    
}
