﻿using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Random = UnityEngine.Random;

public enum FileType {
    Image, Video, Document, Chompymon
}
[Serializable]
public class Demon {
    
    // generated values
    public string Name;
    public int Hue;
    public int HueMod;
    public int EyeHue;
    public int FreqX;
    public int FreqY;
    public int ScrollX;
    public int ScrollY;
    public int Warp;
    public int Lerp10;

    // stored values
    public int Size;
    public int Multiplier;
    public FileType Craving;

    public readonly string filePath;

    public Demon(string filePath) {
        this.filePath = filePath;
    }

    static int getKey(DateTime creationTime, string uniqueId) {
        var k = 0;
        var time = (int)(creationTime.Ticks % 2070704031);
        for (var i = 0; i < uniqueId.Length; i++) {
            k += time * uniqueId[i];
        }
        return k;
    }
    
    public static Demon CreateDemon() {
        var hash = Random.value.GetHashCode().ToString();
        if (File.Exists(Application.persistentDataPath + $"/{hash}.chompymon")) {
            File.Delete(Application.persistentDataPath + $"/{hash}.chompymon");
        }
        var newFile = File.CreateText(Application.persistentDataPath + $"/{hash}.chompymon");
        newFile.Close();
        var info = new FileInfo(Application.persistentDataPath + $"/{hash}.chompymon");
        var key = getKey(info.CreationTimeUtc, SystemInfo.deviceUniqueIdentifier);
        using (var r = new WithRandomSeed(key.GetHashCode())) {
            var name = PREFIXES.Random() + SUFFIXES.Random();
            var path = Application.persistentDataPath + $"/{name}.chompymon";
            var i = 1;
            while (File.Exists(path)) {
                i++;
                path = Application.persistentDataPath + $"/{name}{i}.chompymon";
            }
            File.Move(Application.persistentDataPath + $"/{hash}.chompymon", path);
            var demon = new Demon(path) {
                Name = name,
                Hue = Mathf.FloorToInt(Mathf.Lerp(0, 360, Random.value)),
                HueMod = Random.value > 0.5f ? 1 : 0,
                EyeHue = Mathf.FloorToInt(Mathf.Lerp(0, 360, Random.value)),
                FreqX = Mathf.FloorToInt(Mathf.Lerp(15, 160, Random.value)),
                FreqY = Mathf.FloorToInt(Mathf.Lerp(15, 160, Random.value)),
                ScrollX = Mathf.FloorToInt(Mathf.Lerp(-8, 8, Random.value)),
                ScrollY = Mathf.FloorToInt(Mathf.Lerp(-8, 8, Random.value)),
                Warp = Mathf.FloorToInt(Mathf.Lerp(-30, 30, Random.value)),
                Lerp10 = Mathf.FloorToInt(Mathf.Lerp(0.45f, 0.8f, Random.value)),
            };
            Save(demon);
            return demon;
        }
    }

    public static void Save(Demon d) {
        var text = $"bunchoffillerstuffherethatdoesntreallymatterormaybeitdoesidunnojustdontchangeit|{d.Name}|{d.Hue}|{d.HueMod}|{d.EyeHue}|{d.FreqX}|{d.FreqY}|{d.ScrollX}|{d.ScrollY}|{d.Warp}|{d.Lerp10}|{d.Size}|{d.Multiplier}|{(int)d.Craving}";
        var info = new FileInfo(Application.persistentDataPath + $"/{d.Name}.chompymon");
        var key = getKey(info.CreationTimeUtc, SystemInfo.deviceUniqueIdentifier);
        var encryptedText = Encrypt(text, key.ToString());
        File.WriteAllText(d.filePath, encryptedText);
    }

    public static Demon Load(FileInfo info) {
        try {
            var key = getKey(info.CreationTimeUtc, SystemInfo.deviceUniqueIdentifier);
            var decryptedText = Decrypt(File.ReadAllText(info.FullName), key.ToString());
            var split = decryptedText.Split('|');
            if (info.Name != $"{split[1]}.chompymon" || info.Directory?.FullName.Replace("/", "").Replace("\\", "") != Application.persistentDataPath.Replace("/", "").Replace("\\", "")) {
                return null;
            }
            return new Demon(info.FullName) {
                Name = split[1],
                Hue = int.Parse(split[2]),
                HueMod = int.Parse(split[3]),
                EyeHue = int.Parse(split[4]),
                FreqX = int.Parse(split[5]),
                FreqY = int.Parse(split[6]),
                ScrollX = int.Parse(split[7]),
                ScrollY = int.Parse(split[8]),
                Warp = int.Parse(split[9]),
                Lerp10 = int.Parse(split[10]),
                Size = int.Parse(split[11]),
                Multiplier = int.Parse(split[12]),
                Craving = (FileType)int.Parse(split[13]),
            };
        } catch (Exception e) {
            return null;
        }
    }

    public static string Encrypt(string decryptedText, string key) {
        var sb = new StringBuilder(100);
        var i = 0;
        foreach (var c in decryptedText) {
            sb.Append((char)(c + key[i]));
            i = (i + 1) % key.Length;
        }
        return sb.ToString();
    }

    public static string Decrypt(string encryptedText, string key) {
        var sb = new StringBuilder(100);
        var i = 0;
        foreach (var c in encryptedText) {
            sb.Append((char)(c - key[i]));
            i = (i + 1) % key.Length;
        }
        return sb.ToString();
    }

    public static readonly string[] PREFIXES = {
        "AA",
        "ABA",
        "ABEZE",
        "ABRA",
        "ABY",
        "AD",
        "ADRAM",
        "AES",
        "AGA",
        "AGAR",
        "AGI",
        "AG",
        "AH",
        "AI",
        "AKA",
        "AK",
        "AKO",
        "AL",
        "ALA",
        "ALL",
        "AMAY",
        "AMDU",
        "AMY",
        "ANAM",
        "AND",
        "ANREA",
        "ANDRO",
        "AN",
        "ANTI",
        "AR",
        "ARM",
        "ARU",
        "AS",
        "ASA",
        "ASB",
        "ASMO",
        "ASTA",
        "ASU",
        "AZ",
        "AZA",
        "AZI",
        "BAAL",
        "BA",
        "BAL",
        "BAN",
        "BAP",
        "BAR",
        "BAT",
        "BEEL",
        "BE",
        "BEHE",
        "BEL",
        "BH",
        "BI",
        "BO",
        "BU",
        "BUSH",
        "CAAC",
        "CASI",
        "CA",
        "CHA",
        "CHE",
        "CHORO",
        "CIME",
        "CLASS",
        "COR",
        "CRO",
        "CUL",
        "DAE",
        "DA",
        "DAJ",
        "DAN",
        "DECA",
        "DEM",
        "DEMO",
        "DE",
        "DI",
        "DRE",
        "DZOA",
        "EIS",
        "EL",
        "ELI",
        "FLAU",
        "FOCA",
        "FOR",
        "FO",
        "FUR",
        "GA",
        "GAD",
        "GAM",
        "GH",
        "GLA",
        "GOR",
        "GRE",
        "GRI",
        "GUA",
        "GUS",
        "HAA",
        "HAL",
        "HI",
        "IB",
        "IF",
        "INCU",
        "IP",
        "JIKI",
        "JI",
        "KAB",
        "KA",
        "KAS",
        "KI",
        "KO",
        "KRA",
        "KRO",
        "KUK",
        "KUM",
        "LEC",
        "LEG",
        "LEM",
        "LEO",
        "LER",
        "LEV",
        "LEY",
        "LIL",
        "LJU",
        "LOU",
        "LUC",
        "MAL",
        "MAM",
        "MAR",
        "MAS",
        "MAT",
        "MEP",
        "MER",
        "MOL",
        "MON",
        "MO",
        "MU",
        "NA",
        "NI",
        "ON",
        "OR",
        "OS",
        "PAI",
        "PAZ",
        "PEL",
        "PEN",
        "PHE",
        "PIT",
        "POC",
        "PON",
        "PRE",
        "PRU",
        "PUL",
        "RAH",
        "RAK",
        "RAN",
        "RA",
        "RO",
        "RU",
        "SA",
        "SE",
        "SH",
        "SHE",
        "SIL",
        "SIT",
        "STH",
        "STI",
        "STO",
        "SUA",
        "SUC",
        "SUR",
        "TAN",
        "TIT",
        "TOY",
        "TUC",
        "UKO",
        "VAL",
        "VAN",
        "VAP",
        "VAS",
        "VEP",
        "VIN",
        "WEN",
        "YEQ",
        "ZAG",
        "ZAH",
        "ZEP",
        "ZIM",
        "ZU",
    };
    public static readonly string[] SUFFIXES = {
        "MON",
        "DDON",
        "THIBOU",
        "XAS",
        "ZOU",
        "MELECH",
        "HMA",
        "LIAREPT",
        "ES",
        "EL",
        "RAT",
        "RIMAN",
        "M",
        "EM",
        "MAN",
        "VAN",
        "A",
        "AL",
        "STOR",
        "OCER",
        "MON",
        "SIAS",
        "MELECH",
        "HAKA",
        "RAS",
        "LPHUS",
        "MALIUS",
        "GRA",
        "CHRIST",
        "CHON",
        "AROS",
        "NASURA",
        "AG",
        "KKU",
        "MODAI",
        "DEUS",
        "ROTH",
        "RA",
        "AZ",
        "ZEL",
        "BI",
        "KASURA",
        "KU",
        "LAM",
        "BERITH",
        "LI",
        "SHEE",
        "HOMET",
        "BAS",
        "BATOS",
        "RONG",
        "THIN",
        "HYM",
        "ZEBUB",
        "HEMOTH",
        "RIT",
        "LETH",
        "LIAL",
        "PHEGOR",
        "RITH",
        "OOT",
        "FRONS",
        "RUTA",
        "TIS",
        "ER",
        "KAVAC",
        "NE",
        "YASTA",
        "RINOLAS",
        "MOLAR",
        "IM",
        "MIO",
        "RUN",
        "MOSH",
        "NZON",
        "IES",
        "JES",
        "ABOLAS",
        "SON",
        "CELL",
        "SU",
        "VA",
        "GON",
        "JAL",
        "JAL",
        "TALION",
        "RABIA",
        "IURGE",
        "GORGON",
        "VIL",
        "V",
        "KAVAC",
        "VITS",
        "HETH",
        "GOS",
        "ROS",
        "LOR",
        "AII",
        "RAS",
        "CAS",
        "NEUS",
        "RAS",
        "CAS",
        "FUR",
        "AP",
        "REEL",
        "IGIN",
        "OUL",
        "SSIA",
        "GON",
        "MORY",
        "GORI",
        "LICHU",
        "YOTA",
        "ION",
        "OYN",
        "GENTI",
        "PHAS",
        "NN",
        "LIS",
        "RIT",
        "BUS",
        "OS",
        "NINKI",
        "NN",
        "ANDHA",
        "HANDA",
        "LI",
        "ADYA",
        "MARIS",
        "KABIEL",
        "MPUS",
        "NI",
        "UDH",
        "KARNA",
        "HIES",
        "ION",
        "PO",
        "NARD",
        "AIE",
        "AJE",
        "IATHAN",
        "AK",
        "IM",
        "IN",
        "ITH",
        "BI",
        "VIERS",
        "IFER",
        "IFUGE",
        "APHAR",
        "EPHAR",
        "PHAS",
        "THUS",
        "MON",
        "A",
        "CHOSIAS",
        "ICHA",
        "THIM",
        "IH",
        "TEMA",
        "HIM",
        "PHELES",
        "IHEM",
        "OCH",
        "TPELIER",
        "RAX",
        "RMUR",
        "AMAH",
        "BERIUS",
        "MTAR",
        "NURTA",
        "I",
        "OSKELIS",
        "CUS",
        "IAS",
        "OBAS",
        "E",
        "MON",
        "UZU",
        "ESIT",
        "EMUE",
        "NEX",
        "HIUS",
        "ONG",
        "TIANAK",
        "TA",
        "FLAS",
        "OMAN",
        "AB",
        "SHASA",
        "GDA",
        "UM",
        "NOVE",
        "SALKA",
        "BNOCK",
        "LEMENCE",
        "LEOS",
        "MAEL",
        "TAN",
        "IR",
        "MYAZA",
        "AITAN",
        "AX",
        "DIM",
        "VER",
        "RI",
        "ENNO",
        "HI",
        "LAS",
        "NGGI",
        "CUBUS",
        "GAT",
        "NIN",
        "IVILLUS",
        "OL",
        "HULCHA",
        "BACH",
        "AC",
        "EFAR",
        "TH",
        "ULA",
        "SAGO",
        "AR",
        "E",
        "DIGO",
        "ON",
        "AN",
        "HAK",
        "AR",
        "INIAR",
    };
}