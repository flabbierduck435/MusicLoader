using MusicLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanukiLoader;
using UnityEngine;

public class ML : mod
{
    public string Name => "MusicLoader";
    public string Version => "0.0.1";

    public void initialize()
    {
    }

    public void start(GameObject root)
    {
        root.AddComponent<SongLoader>();
    }
}
