using System;
using System.Linq;
public class Guitar
{
    private string[] _tuning;
    private int[] _midiTuning;
    public int[] midiTuning 
    {
        get => _midiTuning;
    }
    private Cursor cursor;
    

    public Guitar()
    {
        _tuning = new string[6] { "E5", "B4", "G4", "D4", "A3", "E3" };
        SetUp();
    }
    public Guitar(string[] altTuning)
    {
        _tuning = altTuning;
        SetUp();
    }

    private void SetUp() 
    {
        SetMidiTuning();
        cursor = new Cursor(this);
    }

    private void SetMidiTuning()
    {
        _midiTuning = _tuning.Select(pitch => MidiUtility.StringToMidi(pitch)).ToArray();
    }

    public void RetuneString(int i, string pitch)
    {
        _tuning[i] = pitch;
        SetMidiTuning();
    }
    public void PrintStrings()
    {
        for (int i = 0; i < _tuning.Length; i++)
        {
            Console.WriteLine($"String number {i} = {_tuning[i]}, MIDI = {_midiTuning[i]}");
        }
    }

    /// <summary>
    /// Tracks position on the guitar and should return the best fingering
    /// </summary>
    /// <param name="midiNote"></param>
    /// <returns>{string number, fret number}</returns>
    public int[] GetFingering(int midiNote)
    {
        return cursor.GetFingering(midiNote);
    }

    
}

public class Cursor 
{
    private int[] position = new int[2];
    private bool startAnywhere = true;
    // Used when a rest or open string allows the player to shift
    private Guitar parent;

    public Cursor(Guitar g)
    {
        parent = g;
    }

    public int[] GetFingering(int midi)
    {
        int[] tuning = parent.midiTuning;
        int idealString = 0;
        int idealFret = Int32.MaxValue;
        if (startAnywhere)
        {
            // for now, always start at the lowest possible spot on the neck
            for (int i = 0; i < tuning.Length; i++)
            {
                int curString = tuning[i];
                int fret = midi - curString;
                if (fret >= 0 && fret < idealFret)
                {
                    idealString = i;
                    idealFret = fret;
                }
            }

            // startAnywhere = false;
        }
        position[0] = idealString;
        position[1] = idealFret;

        return (position);
    }
}
