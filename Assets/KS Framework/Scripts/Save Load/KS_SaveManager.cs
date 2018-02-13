using UnityEngine;
using System.Collections;

public class KS_SaveManager: MonoBehaviour {

    public enum EncryptionMethod
    {
        Default
    }

    KS_FileHelper fileHelper;

    public string saveExtension = ".sav";
    public string defaultSaveName = "save_";
    public string slotSaveName = "slot_";

    public bool useEncryption = false;
    public string encryptionKey;
    public EncryptionMethod encryptionMethod = EncryptionMethod.Default;

    private int currentSaves = 0;

    public int maximumSaves = 100;

    public int maximumAutoSaves = 2;
    private int currentAutoSaves = 0;

    private int test = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            test++;
            AutoSave("" + test);
        }
    }


	// Use this for initialization
	void Start () {
        fileHelper = KS_FileHelper.Instance;

        Setup();
	}

    void Setup()
    {
        string[] saves = fileHelper.GetFolderContents(KS_FileHelper.Folders.Saves);

        foreach(string s in saves)
        {
            if (s.Contains("Autosave_"))
            {
                currentAutoSaves++;
            }
            else
            {
                currentSaves++;
            }
        }
    }

    private string EncryptData(string data)
    {


        return data;
    }

    private string DecryptData(string data)
    {


        return data;
    }

    public bool AutoSave(string data)
    {
        if(currentAutoSaves >= maximumAutoSaves)
        {
            fileHelper.DeleteFile(KS_FileHelper.Folders.Saves, "Autosave_02" + saveExtension);
            fileHelper.RenameFile(KS_FileHelper.Folders.Saves, "Autosave_01" + saveExtension, "Autosave_02" + saveExtension);

            return fileHelper.SaveFile(KS_FileHelper.Folders.Saves, "Autosave_01" + saveExtension, data);
        }
        else
        {
            if (currentAutoSaves == 1)
            {
                fileHelper.RenameFile(KS_FileHelper.Folders.Saves, "Autosave_01" + saveExtension, "Autosave_02" + saveExtension);
            } 


            if (fileHelper.SaveFile(KS_FileHelper.Folders.Saves, "Autosave_01" + saveExtension, data))
            {
                currentAutoSaves++;
                return true;
            } 
            else
            {
                return false;
            }
        }

    }

    public bool SaveGame(string data)
    {
        if(fileHelper.SaveFile(KS_FileHelper.Folders.Saves, defaultSaveName + (currentSaves + 1) + saveExtension, data)){
            currentSaves++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SaveGame(string data, string name)
    {
        if (fileHelper.SaveFile(KS_FileHelper.Folders.Saves, name + saveExtension, data))
        {
            currentSaves++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SaveToSlot(string data, int slot)
    {
        if(fileHelper.SaveFile(KS_FileHelper.Folders.Saves, slotSaveName + slot, data)){
            return true;
        }
        else 
        {
            return false;
        }
    }

    public string LoadLatest()
    {


        return "";
    }

    public string LoadLatestSave()
    {
        return "";
    }

    public string LoadLatestAutoSave()
    {
        return "";
    }

    public string LoadSave(int index)
    {
        return "";
    }

    public string LoadSave(string name)
    {
        return "";
    }

    public string LoadSlot(int slot)
    {

        return "";
    }
}
