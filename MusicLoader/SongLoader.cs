using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace MusicLoader
{
    public class SongLoader : MonoBehaviour
    {
        private AudioService audioService;
        private AudioClip clip;
        // Use this for initialization
        void Start()
        {
            audioService = AudioService.Instance;
            MusicLibrary library = audioService.musicLibrary;
            LoadMusic(library);
        }

        // Update is called once per frame
        public void LoadMusic (MusicLibrary library)
        {
            DirectoryInfo dir = new DirectoryInfo(BepInEx.Paths.GameRootPath + "/Music");
            Console.WriteLine("Getting Music at " + dir.FullName);
            foreach (FileInfo MusicFile in dir.GetFiles("*.wav"))
            {
                StartCoroutine(GetAudioClip(MusicFile.FullName, MusicFile.Name, library));

            }
        }

        IEnumerator GetAudioClip(string File, string fileName, MusicLibrary library)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(File, AudioType.WAV))
            {
                yield return www.SendWebRequest();

                     clip = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("loaded " + Path.GetFileNameWithoutExtension(fileName));
                Song song = new Song();
                SongInfo songInfo = new SongInfo();
                if (System.IO.File.Exists(BepInEx.Paths.GameRootPath + "/Music/" + Path.GetFileNameWithoutExtension(fileName) + ".json"))
                {
                    Debug.Log("load song");
                    songInfo = JsonUtility.FromJson<SongInfo>(System.IO.File.ReadAllText(BepInEx.Paths.GameRootPath + "/Music/" + Path.GetFileNameWithoutExtension(fileName) + ".json"));

                    song.id = songInfo.id;
                    song.name = songInfo.name;
                    song.bpm = songInfo.bpm;
                    song.audioClip = clip;
                    song.unlocked = true;
                    song.forcePlay = false;
                }
                else
                {
                    Debug.Log("making song file");
                    song.id = "BGM_" + Path.GetFileNameWithoutExtension(fileName);
                    songInfo.id = "BGM_" + Path.GetFileNameWithoutExtension(fileName);
                    song.name = Path.GetFileNameWithoutExtension(fileName);
                    songInfo.name = Path.GetFileNameWithoutExtension(fileName);
                    song.audioClip = clip;
                    song.unlocked = true;
                    song.bpm = 60;
                    songInfo.bpm = 60;
                    song.forcePlay = false;
                    System.IO.File.WriteAllText(BepInEx.Paths.GameRootPath + "/Music/" + Path.GetFileNameWithoutExtension(fileName) + ".json", JsonUtility.ToJson(songInfo));
                }
                library.songs.Add(song);
            }
        }
    }
}