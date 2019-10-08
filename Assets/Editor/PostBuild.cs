using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class PostBuild
{
    [PostProcessBuildAttribute(1)]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        Debug.Log(pathToBuiltProject);
        Debug.Log(target.ToString());
        copyLevelPreviews(pathToBuiltProject);
    }

    static void copyLevelPreviews(string buildPath)
    {
        try
        {
            Debug.Log(Application.dataPath);
            string path = Application.dataPath + "/LevelPreviews";
            buildPath =  buildPath.Substring(0 , buildPath.LastIndexOf("/")) + "/StirlingEclipse_Data/LevelPreviews/";
            if(Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                Directory.CreateDirectory(buildPath);

                foreach(string file in files)
                {
                    string targetPath = buildPath + Path.GetFileNameWithoutExtension(file);
                    Debug.Log("Copying " + file + " to " + targetPath);
                    File.Copy(file, targetPath);
                }
            }
            else
            {
                Debug.LogError("LevelPreview is missing in Assets: " + path);
            }
        }
        catch(IOException e)
        {
            Debug.LogError(e.Message);
        }
        
    }
}
