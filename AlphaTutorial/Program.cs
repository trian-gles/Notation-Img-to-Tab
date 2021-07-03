using System;
using System.Linq;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Standards;
using Melanchall.DryWetMidi.Tools;
using Melanchall.DryWetMidi.Devices;
using Melanchall.DryWetMidi.Composing;
using Melanchall.DryWetMidi.Interaction;
using System.Collections.Generic;
using System.IO;

namespace Notation_to_Tab
{

    public class Program
    {
        
        public static string MidiToTabTex()
        {
            Guitar g = new Guitar();
            var midiNotes = ParseMidiEvents(@"C:\Users\bkier\source\repos\AlphaTutorial\AlphaTutorial\DonnaLeeSolo.mid");
            var fingerings =
                    from mn in midiNotes select g.GetFingering(mn);

            string tex = @"\title 'Test' .";
            int beat = 0;
            int measure = 0;
            foreach (int[] f in fingerings) 
            {
                tex += $" {f[1]}.{f[0] + 1}.{8}";
                beat++;
                if (beat == 8) 
                {
                    beat = 0;
                    tex += " |";
                    measure++;
                    if (measure == 4) 
                    {
                        measure = 0;
                        // tex += " N";
                    }
                }
            }

            // Set a variable to the Documents path.
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Append text to an existing file named "WriteLines.txt".
            //using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "WriteLines.txt"), true))
            //{
             //   outputFile.WriteLine(tex);
            //}

            return tex;
        }

        /// <summary>takes a MIDI file and spits out all the pitches.  Eventually should include duration and rests</summary>
        static IEnumerable<int> ParseMidiEvents(string filename)
        {
            MidiFile file = MidiFile.Read(filename, new ReadingSettings 
            {
                NotEnoughBytesPolicy = NotEnoughBytesPolicy.Ignore
            }
            );
            TempoMap tempoMap = file.GetTempoMap();
            var chunks = file.GetTrackChunks();
            var notes = chunks.GetNotes();
            var midiNotes =
                from n in notes select (int) n.NoteNumber;
            foreach (Note note in notes) 
            {
                BarBeatTicksTimeSpan musicalTime = note.LengthAs<BarBeatTicksTimeSpan>(tempoMap);
                Console.WriteLine(musicalTime);
            }



            return midiNotes;
        }
    }

}
