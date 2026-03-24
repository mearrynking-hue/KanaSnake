using System;
using System.Collections.Generic;

namespace KanaSnake;

//for making kana objects in MainWindow
public class KanaPair
{
    public string Romaji{get; set;}
    public string Kana {get; set;}

    public KanaPair(string romaji, string kana)
    {
        Romaji = romaji;
        Kana = kana;
    }
}

//getting random kana
public class KanaManager
{
    //alphabet list
    private List<KanaPair> _kanaList = new List<KanaPair>
    {
        new KanaPair("A", "あ"), new KanaPair("I", "い"), new KanaPair("U", "う"), new KanaPair("E", "え"), new KanaPair("O", "お"),
        new KanaPair("KA", "か"), new KanaPair("KI", "き"), new KanaPair("KU", "く"), new KanaPair("KE", "け"), new KanaPair("KO", "こ"),
        new KanaPair("SA", "さ"), new KanaPair("SHI", "し"), new KanaPair("SU", "す"), new KanaPair("SE", "せ"), new KanaPair("SO", "そ"),
        new KanaPair("TA", "た"), new KanaPair("CHI", "ち"), new KanaPair("TSU", "つ"), new KanaPair("TE", "て"), new KanaPair("TO", "と"),
        new KanaPair("NA", "な"), new KanaPair("NI", "に"), new KanaPair("NU", "ぬ"), new KanaPair("NE", "ね"), new KanaPair("NO", "の"),
        new KanaPair("HA", "は"), new KanaPair("HI", "ひ"), new KanaPair("FU", "ふ"), new KanaPair("HE", "へ"), new KanaPair("HO", "ほ"),
        new KanaPair("MA", "ま"), new KanaPair("MI", "み"), new KanaPair("MU", "む"), new KanaPair("ME", "め"), new KanaPair("MO", "も"),
        new KanaPair("YA", "や"), new KanaPair("YU", "ゆ"), new KanaPair("YO", "よ"),
        new KanaPair("RA", "ら"), new KanaPair("RI", "り"), new KanaPair("RU", "る"), new KanaPair("RE", "れ"), new KanaPair("RO", "ろ"),
        new KanaPair("WA", "わ"), new KanaPair("WO", "を"), new KanaPair("N", "ん")
    };

    //random generator
    private Random _random = new Random();

    //getting random kana
    public KanaPair GetRandomKana()
    {
        int index = _random.Next(_kanaList.Count);
        return _kanaList[index];
    }
}