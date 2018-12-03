using System;
using UnityEngine;
using System.Collections;
using System.IO;
using Crosstales.FB;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FileEater : MonoBehaviour {

    public const bool ACTUALLY_DELETE = false;

    public static readonly string[] IMAGE_EXTS = {
        "ani",
        "anim",
        "apng",
        "art",
        "bmp",
        "bpg",
        "bsave",
        "cal",
        "cin",
        "cpc",
        "cpt",
        "dds",
        "dpx",
        "ecw",
        "exr",
        "fits",
        "flic",
        "flif",
        "fpx",
        "gif",
        "hdri",
        "hevc",
        "icer",
        "icns",
        "ico",
        "cur",
        "ics",
        "ilbm",
        "jbig",
        "jbig2",
        "jng",
        "jpeg",
        "jpeg-ls",
        "jpeg",
        "2000",
        "jpeg",
        "xr",
        "jpeg",
        "xt",
        "jpeg-hdr",
        "kra",
        "mng",
        "miff",
        "nrrd",
        "ora",
        "pam",
        "pbm",
        "pgm",
        "ppm",
        "pnm",
        "pcx",
        "pgf",
        "pictor",
        "png",
        "psd",
        "psb",
        "psp",
        "qtvr",
        "ras",
        "rgbe",
        "logluv",
        "tiff",
        "sgi",
        "tga",
        "tiff",
        "ufo",
        "ufp",
        "wbmp",
        "webp",
        "xbm",
        "xcf",
        "xpm",
        "xwd",
        "raw",
        "ciff",
        "dng",
        "vector",
        "ai",
        "cdr",
        "cgm",
        "dxf",
        "eva",
        "emf",
        "gerber",
        "hvif",
        "iges",
        "pgml",
        "svg",
        "vml",
        "wmf",
        "xar",
        "compound",
        "cdf",
        "djvu",
        "eps",
        "pdf",
        "pict",
        "ps",
        "swf",
        "xaml",
    };
    public static readonly string[] VIDEO_EXTS = {
        ".webm",
        ".mkv",
        ".flv",
        ".flv",
        ".vob",
        ".ogv",
        ".ogg",
        ".drc",
        ".gif",
        ".gifv",
        ".mng",
        ".avi",
        ".mts",
        ".m2ts",
        ".mov",
        ".qt",
        ".wmv",
        ".yuv",
        ".rm",
        ".rmvb",
        ".asf",
        ".amv",
        ".mp4",
        ".m4v",
        ".mpg",
        ".mp2",
        ".mpeg",
        ".mpe",
        ".mpv",
        ".mpg",
        ".mpeg",
        ".m2v",
        ".m4v",
        ".svi",
        ".3gp",
        ".3g2",
        ".mxf",
        ".roq",
        ".nsv",
        ".flv",
        ".f4v",
        ".f4p",
        ".f4a",
        ".f4b",
    };

    public string FileToEat;
    public FileType? Type;
    public long Kilobytes;
    public float DaysOld;
    public int TotalSize;
    public GameObject ConfirmDialog;
    public Text FileText;
    public Text InfoText;

    public FloatNear Teeth1;
    public float Teeth1Y;
    public FloatNear Teeth2;
    public float Teeth2Y;

    bool chomping;

    void updateFile() {
        if (!string.IsNullOrEmpty(FileToEat)) {
            var ext = FileToEat.Split('.')[1].ToLower();
            Type = ext == "chompymon" ? FileType.Chompymon : IMAGE_EXTS.IndexOf(ext) >= 0 ? FileType.Image : VIDEO_EXTS.IndexOf(ext) >= 0 ? FileType.Video : FileType.Document;

            var info = new FileInfo(FileToEat);
            Kilobytes = info.Length / 1024;
            DaysOld = (float)(DateTime.Today - info.CreationTimeUtc).TotalDays;

            var mbBase = Mathf.Pow(Kilobytes, 0.3f);
            var ageMultiplier = Mathf.Max(1, Mathf.Log(Mathf.Max(DaysOld, 1), 3));
            var levelMultiplier = Mathf.Pow(2, DemonManager.Inst.CurrentDemon.Multiplier);
            TotalSize = (int) Mathf.Pow(mbBase * ageMultiplier, levelMultiplier);

            FileText.text = FileToEat;
            InfoText.text = $"{Kilobytes}kb (+{mbBase:0.0}) with {DaysOld} days old age multiplier (x{ageMultiplier:0.0}) and level (^{levelMultiplier:0)}) = {TotalSize/100}.{TotalSize%100}kg Chompymon growth";
        } else {
            Type = null;
            Kilobytes = 0;
            DaysOld = 0;
            TotalSize = 0;
        }

        ConfirmDialog.SetActive(!string.IsNullOrEmpty(FileToEat) && !chomping);
    }
    
    void Update() {
        if (Input.GetMouseButtonDown(0) && string.IsNullOrEmpty(FileToEat)) {
            try {
                var file = FileBrowser.OpenSingleFile("File to DESTROY FOREVER by feeding to " + DemonManager.Inst.CurrentDemon.Name, Application.persistentDataPath, "");
                FileToEat = file;
            } catch (Exception e) {
                FileToEat = null;
            }
            updateFile();
        }
    }

    public void DeleteForever() {
        if (!string.IsNullOrEmpty(FileToEat)) {
            StartCoroutine(chomp());
        }
    }
    
    public void Cancel() {
        FileToEat = null;
        updateFile();
    }

    IEnumerator chomp() {
        chomping = true;
        updateFile();
        var prev1Y = Teeth1.BaseTarget.y;
        var prev2Y = Teeth2.BaseTarget.y;
        var speed1 = Teeth1.ConstantSpeed;
        var speed2 = Teeth2.ConstantSpeed;
        Teeth1.BaseTarget.y = Teeth1Y;
        Teeth2.BaseTarget.y = Teeth2Y;
        Teeth1.ConstantSpeed = 10;
        Teeth2.ConstantSpeed = 10;
        yield return new WaitForSeconds(1.15f);
        Teeth1.BaseTarget.y = prev1Y;
        Teeth2.BaseTarget.y = prev2Y;
        DemonManager.Inst.CurrentDemon.Size += TotalSize;
        var cravingHit = false;
        if (Type == DemonManager.Inst.CurrentDemon.Craving) {
            if (DemonManager.Inst.CurrentDemon.Craving == FileType.Chompymon) {
                var demon = Demon.Load(new FileInfo(FileToEat));
                cravingHit = demon.Multiplier >= DemonManager.Inst.CurrentDemon.Multiplier;
            } else {
                var sizeRequired = Mathf.Pow(2, DemonManager.Inst.CurrentDemon.Multiplier);
                cravingHit = Kilobytes >= sizeRequired;
            }
        }
        if (cravingHit) {
            DemonManager.Inst.CurrentDemon.Multiplier++;
            if (DemonManager.Inst.CurrentDemon.Multiplier > 0 && DemonManager.Inst.CurrentDemon.Multiplier % 5 == 0) {
                DemonManager.Inst.CurrentDemon.Craving = FileType.Chompymon;
            } else {
                DemonManager.Inst.CurrentDemon.Craving = (FileType) Mathf.FloorToInt(Random.value * 3);
            }
            DemonSetter.Inst.UpdateWindow();
        }
        if (ACTUALLY_DELETE) {
            File.Delete(FileToEat);
        }
        Demon.Save(DemonManager.Inst.CurrentDemon);
        FileToEat = null;
        updateFile();
        yield return new WaitForSeconds(0.7f);
        Teeth1.ConstantSpeed = speed1;
        Teeth2.ConstantSpeed = speed2;
        chomping = false;
    }
}
