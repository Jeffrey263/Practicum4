using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practicum4
{
    public class CD
    {
        public string title { get; set; }
        public string artist { get; set; }    
        public List<Track> tracks { get; set; }

        public CD(string title, string artist)
        {
            tracks = new List<Track>();
            this.title = title;
            this.artist = artist;
        }

        public void AddTrack(Track t)
        {
            this.tracks.Add(t);
        }

        public String ToString()
        {
            string s = "title: " + this.title + ",\nartist: " + this.artist + ",\ntracks:\n";

            foreach(Track t in this.tracks)
            {
                s +="\t" + t.ToString()+",\n";
            }

            return s;
        }
    }

    public class Track
    {
        public string title { get; set; }
        public string artist { get; set; }
        public TimeSpan length { get; set; }

        public Track(string title, string artist, TimeSpan length)
        {
            this.title = title;
            this.artist = artist;
            this.length = length;
        }

        public String ToString()
        {
            return "[" + this.title + ", " + this.artist + ", " + this.length + "]";
        }
    }
}
