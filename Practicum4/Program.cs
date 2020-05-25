using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Xml.Linq;

namespace Practicum4
{
    class Program
    {
        static void Main(string[] args)
        {
            bool running = true;

            //program loop
            while (running)
            {
                // aanmaken van de CD klasse "ghost"
                CD ghost = new CD("Opus Eponymous", "Ghost");
                ghost.AddTrack(new Track("Con Clavi Con Dio", "Ghost", new TimeSpan(00, 3, 33)));
                ghost.AddTrack(new Track("Satan Prayer", "Ghost", new TimeSpan(00, 4, 38)));
                ghost.AddTrack(new Track("Ritual", "Ghost", new TimeSpan(00, 4, 28)));

                PrintTitle("Aanmaken van klassen");
                Console.WriteLine(ghost.ToString());

                //CD klasse "ghost" omzetten naar XML
                XDocument ghostXML = CDtoXML(ghost);

                PrintTitle("\nKlassen naar XML");
                Console.WriteLine(ghostXML);

                //"Echte" album ophalen en de tracks die missen in een List<Track> stoppen
                XDocument fetchedXMLDoc = FetchAlbum();
                var query = from t in fetchedXMLDoc.Descendants("track")
                            let trackName = t.Element("name").Value
                            let trackArtist = t.Element("artist").Element("name").Value
                            let length = t.Element("duration").Value

                            where !(from tLocal in ghost.tracks
                                    select tLocal.title).Contains(trackName)

                            select new Track(trackName, trackArtist, TimeSpan.FromSeconds(int.Parse(length))); ;

                PrintTitle("\nOpgehaalde missende nummers als klassen");

                //itereer door de List<Track>
                foreach (Track t in query)
                {
                    // print elke missende track uit
                    Console.WriteLine(t.ToString());

                    // voeg de missende track toe aan het CD object "ghost"
                    ghost.AddTrack(t);
                }
                
                PrintTitle("\nDe upgedate XML met alle nummers");

                //Genereer en print ghost XML opnieuw met de missende tracks erbij.
                ghostXML = CDtoXML(ghost);
                Console.WriteLine(ghostXML);
                Console.ReadLine();
            }
        }

        static XDocument CDtoXML(CD c)
        {
            var cdXml = new XDocument(new XElement("cd",
                                    new XAttribute("artist", c.artist),
                                    new XAttribute("name", c.title),
                                    new XElement("tracks", from track in c.tracks
                                                           select new XElement("track",
                                                           new XElement("artist", track.artist),
                                                           new XElement("name", track.title),
                                                           new XElement("length", track.length.ToString())))));
            return cdXml;
        }

        static XDocument FetchAlbum()
        {
            String xmlString;
            using (WebClient wc = new WebClient())
            {
                xmlString = wc.DownloadString(@"http://ws.audioscrobbler.com/2.0/?method=album.getinfo&api_key=b5cbf8dcef4c6acfc5698f8709841949&artist=Ghost&album=Opus+Eponymous");
            }
            XDocument myXMLDoc = XDocument.Parse(xmlString);

            return myXMLDoc;
        }

        static void PrintTitle(String s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + s);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}