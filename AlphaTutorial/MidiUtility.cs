using System;
using System.Collections;
public class MidiUtility
{
    static Hashtable pitchInts = new Hashtable() {
            {'C', 0}, {'D', 2}, {'E', 4}, {'F', 5}, {'G', 7}, {'A', 9}, {'B', 11}
        };

    public static int StringToMidi(string pitch)
    {
        int mod = 0;
        if (pitch.Length == 7) // Acount for SHARP i.e. CSharp5
        {
            Console.WriteLine(pitch.Substring(1, 5));
            mod = 1;
        }
        char basePitch = pitch[0];
        int b = (int)pitchInts[basePitch];
        int register = (int)Char.GetNumericValue(pitch[pitch.Length - 1]);
        int result = b + (register * 12) + 12 + mod;
        return result;
    }
}
