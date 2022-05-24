using System.Configuration;
using System;

/*
Si vuole progettare un sistema per la gestione di una biblioteca.
Gli utenti registrati al sistema, fornendo 
- cognome, 
- nome, 
- email, 
- password, 
- recapito telefonico,
Possono effettuare dei prestiti sui documenti che sono di vario tipo (libri, DVD).
I documenti sono caratterizzati da 
- un codice identificativo di tipo stringa (ISBN per i libri, numero seriale per i DVD), 
- titolo, 
- anno, 
- settore (storia, matematica, economia, …),
- stato (In Prestito, Disponibile), 
- uno scaffale in cui è posizionato, 
- un elenco di autori (Nome, Cognome).
Per i libri si ha in aggiunta
- il numero di pagine,
mentre per i dvd 
- la durata.
L’utente deve poter eseguire delle ricerche per 
- codice o per 
- titolo
e, eventualmente, effettuare dei prestiti registrando 
- il periodo (Dal/Al) del prestito 
- il documento.
Il sistema per ogni prestito determina
- un numero progressivo di tipo alfanumerico.
Deve essere possibile effettuare la ricerca dei prestiti dato 
- nome e cognome di un utente.

*/

//Da completare nell'esercizio
//NB: la chiave di un dizionario non deve necessarimente essere una stringa. ome abbiamo visto nelle procedure di ordinamento,
//questa può essere anche una tupl
//Esempio:  var dict = new Dictionary<Tuple<int, double, string>, string>();


namespace csharp_biblioteca
{
    internal class Program
    {

        static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }

        static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "Not Found";
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return "";
            }
        }

        static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        static void Main(string[] args)
        {
            Biblioteca b = new Biblioteca("Civica");



#if NONDEFINITO
            //per prima cosa, se ci sono, leggo gli utenti da file!!!
            //Questo potrebbe essere il posto migliore: b.RestoreUtenti(nomefile);

            //Lettore
            List<string> ls = new List<string>();
            if (File.Exists("pippo.dat"))
            {
                StreamReader sr = new StreamReader("pippo.dat");
                string? letta;
                while ((letta = sr.ReadLine()) != null)
                {
                    //manipolo la stringa per estrarre i valori che mi servono
                    //e poi aggiungo alla lista
                    ls.Add(letta);
                }
                sr.Close();
            }

            //Scrittore
            //Prma di scrivere, se la lista è vuota, aggiungo qualcosa
            if (ls.Count() == 0)
            {
                ls.Add("uno");
                ls.Add("due");
                ls.Add("tre");
            }

            //ora scrivo
            StreamWriter sw = new StreamWriter("pippo.dat");
            foreach(String s in ls)
            {
                sw.WriteLine(s);
            }
            sw.Close();
            Environment.Exit(0);
#endif

            /***** PER GESTIRE App.config ***********
            AddUpdateAppSettings("chiave1", "valore1");
            AddUpdateAppSettings("chiave2", "11");
            AddUpdateAppSettings("violino", "12");
            AddUpdateAppSettings("pianoforte", "spinetta o affini");
            ReadAllSettings();
            Environment.Exit(0); */

            /*********** Alcune funzioni sui file ******************/
            //File.GetCreationTime(...)
            //File.GetLastAccessTime(...)

            /*********** Date e similari ******************/
            var adesso = DateTime.Now;
            var annifa = new DateTime(1930, 2, 11);

            //differenza tra due tempi?
            var differenza = adesso - annifa;

            TimeSpan altradiff = new DateTime(2022, 4, 19, 11, 12, 11) - new DateTime(1930, 11, 21, 21, 34, 45);

            //Di che tipo è la differenza tra date? di tipo TimeSpan
            Console.WriteLine(altradiff);  
            Console.WriteLine(altradiff.Seconds); //Cosa stampa? 
            //33386.13:37:26
            //26 
            Console.WriteLine(altradiff.TotalSeconds); //E ora cosa stampa? 
                                                       //33386*24*3600+13*3600+37*60+26 => 2884599446

            using (var stream = File.Open("pippo.dat", FileMode.Create))
            {
                using (var bw = new BinaryWriter(stream))
                {
                    Int32 uno = 1;
                    Int64 due = 2;
                    bw.Write(uno);
                    bw.Write(due);
                    bw.Write('3');
                }
            }

            Environment.Exit(0);

            Scaffale s1 = new Scaffale("S001");
            Scaffale s2 = new Scaffale("S002");
            Scaffale s3 = new Scaffale("S003");

#region "Libro 1"
            Libro l1 = new Libro("ISBN1", "Titolo 1", 2009, "Storia", 220);
            Autore a1 = new Autore("Nome 1", "Cognome 1");
            Autore a2 = new Autore("Nome 2", "Cognome 2");
            l1.Autori.Add(a1);
            l1.Autori.Add(a2);
            l1.Scaffale = s1;

            b.Documenti.Add(l1);
#endregion

#region "Libro 2"
            Libro l2 = new Libro("ISBN2", "Titolo 2", 2009, "Storia", 130);
            Autore a3 = new Autore("Nome 3", "Cognome 3");
            Autore a4 = new Autore("Nome 4", "Cognome 4");
            l2.Autori.Add(a3);
            l2.Autori.Add(a4);
            l2.Scaffale = s2;

            b.Documenti.Add(l2);
#endregion

#region "DVD"
            DVD dvd1 = new DVD("Codice1", "Titolo 3", 2019, "Storia", 130);
            dvd1.Autori.Add(a3);
            dvd1.Scaffale = s3;

            b.Documenti.Add(dvd1);
#endregion

            Utente u1 = new Utente("Nome 1", "Cognome 1", "Telefono 1", "Email 1", "Password 1");
            b.Utenti.Add(u1);
            //Questo potrebbe essere il posto migliore: b.SalvaUtenti(nomefile);


            Prestito p1 = new Prestito("P00001", new DateTime(2019, 1, 20), new DateTime(2019, 2, 20), u1, l1);
            Prestito p2 = new Prestito("P00002", new DateTime(2019, 3, 20), new DateTime(2019, 4, 20), u1, l2);

            b.Prestiti.Add(p1);
            b.Prestiti.Add(p2);

            Console.WriteLine("\n\nSearchByCodice: ISBN1\n\n");

            List<Documento> results = b.SearchByCodice("ISBN1");

            foreach (Documento doc in results)
            {
                Console.WriteLine(doc.ToString());

                if (doc.Autori.Count > 0)
                {
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Autori");
                    Console.WriteLine("--------------------------");
                    foreach (Autore a in doc.Autori)
                    {
                        Console.WriteLine(a.ToString());
                        Console.WriteLine("--------------------------");
                    }
                }
            }

            Console.WriteLine("\n\nSearchPrestiti: Nome 1, Cognome 1\n\n");

            List<Prestito> prestiti = b.SearchPrestiti("Nome 1", "Cognome 1");

            foreach (Prestito p in prestiti)
            {
                Console.WriteLine(p.ToString());
                Console.WriteLine("--------------------------");
            }

            //Come ultima istruzione del programma oppure ogni volta che aggiungete un nuovo utente,
            //salvate gli utenti su file.

        }


        enum Stato { Disponibile, Prestito }

        class Persona
        {
            public string Nome { get; set; }
            public string Cognome { get; set; }

            public Persona(string Nome, string Cognome)
            {
                this.Nome = Nome;
                this.Cognome = Cognome;
            }

            public override string ToString()
            {
                return string.Format("Nome:{0}\nCognome:{1}",
                    this.Nome,
                    this.Cognome);
            }
        }


        class ListaUtenti
        {
            private Dictionary<Tuple<string, string, string>, Utente> MyDictionary;

            public ListaUtenti()
            {
                MyDictionary = new Dictionary<Tuple<string,string,string>, Utente>();
            }


            //Tutti i metodi per inserire, cercare e fare tutto nel dizionario
            public void AggiungiUtente(Utente uUtente)
            {
                var chiave = new Tuple<string, string, string>(uUtente.Nome, uUtente.Cognome, uUtente.Email);
                MyDictionary.Add(chiave, uUtente);
                //string sChiaveUtente = 
                //    uUtente.Nome.ToLower() + ";" + 
                //    uUtente.Cognome.ToLower() + ";" + 
                //    uUtente.Email.ToLower();
                //MyDictionary.Add(sChiaveUtente, uUtente);
            }
        }

        class Biblioteca
        {
            public string Nome { get; set; }
            public List<Documento> Documenti { get; set; }
            public List<Prestito> Prestiti { get; set; }


            public List<Utente> Utenti { get; set; }

            //public Dictionary<string,Utente> MioDizionario;

            public ListaUtenti MiaListaUtenti { get; set; }

            //Chiave     Nome;Cognome;email
            //MioDizionario.Add(
            //MiaListaUtenti.AggiungiUtente(sMioNome, sMioCognome, sMiaMail,.......)
            //UtentePresente = MioDizionario["Mario;Rossi;mariorossi@gmail.com"];
            //MioDizionario(chiave, valore)

            public Biblioteca(string Nome)
            {
                this.Nome = Nome;
                this.Documenti = new List<Documento>();
                this.Prestiti = new List<Prestito>();
                this.Utenti = new List<Utente>();
            }
            public bool SaveUtenti(string filename)
            {
                //salva gli utenti sul file filename
                return true;
            }
            public bool RestoreUtenti(string filename)
            {
                //Ricostruisce la lista degli utenti leggendo il file su cui sono stati scritti
                return true;
            }

            public List<Documento> SearchByCodice(string Codice)
            {
                return this.Documenti.Where(d => d.Codice == Codice).ToList();
            }

            public List<Documento> SearchByTitolo(string Titolo)
            {
                return this.Documenti.Where(d => d.Titolo == Titolo).ToList();
            }

            public List<Prestito> SearchPrestiti(string Numero)
            {
                return this.Prestiti.Where(p => p.Numero == Numero).ToList();
            }

            public List<Prestito> SearchPrestiti(string Nome, string Cognome)
            {
                return this.Prestiti.Where(p => p.Utente.Nome == Nome && p.Utente.Cognome == Cognome).ToList();
            }
        }

        class Scaffale
        {
            public string Numero { get; set; }

            public Scaffale(string Numero)
            {
                this.Numero = Numero;
            }
        }

        class Documento
        {
            public string Codice { get; set; }
            public string Titolo { get; set; }
            public int Anno { get; set; }
            public string Settore { get; set; }
            public Stato Stato { get; set; }
            public List<Autore> Autori { get; set; }
            public Scaffale Scaffale { get; set; }

            public Documento(string Codice, string Titolo, int Anno, string Settore)
            {
                this.Codice = Codice;
                this.Titolo = Titolo;
                this.Settore = Settore;
                this.Autori = new List<Autore>();
                this.Stato = Stato.Disponibile;
            }

            public override string ToString()
            {
                return string.Format("Codice:{0}\nTitolo:{1}\nSettore:{2}\nStato:{3}\nScaffale numero:{4}",
                    this.Codice,
                    this.Titolo,
                    this.Settore,
                    this.Stato,
                    this.Scaffale.Numero);
            }

            public void ImpostaInPrestito()
            {
                this.Stato = Stato.Prestito;
            }

            public void ImpostaDisponibile()
            {
                this.Stato = Stato.Disponibile;
            }
        }

        class Libro : Documento
        {
            public int NumeroPagine { get; set; }
            public Libro(string Codice, string Titolo, int Anno, 
                         string Settore, int NumeroPagine) 
                : base(Codice, Titolo, Anno, Settore)
            {
                this.NumeroPagine = NumeroPagine;
            }
            public override string ToString()
            {
                return string.Format("{0}\nNumeroPagine:{1}", base.ToString(), this.NumeroPagine);
            }
        }

        class DVD : Documento
        {
            public int Durata { get; set; }
            public DVD(string Codice, string Titolo, int Anno, string Settore, int Durata) 
                : base(Codice, Titolo, Anno, Settore)
            {
                this.Durata = Durata;
            }
            public override string ToString()
            {
                return string.Format("{0}\nDurata:{1}", base.ToString(), this.Durata);
            }
        }

        class Autore : Persona
        {
            public Autore(string Nome, string Cognome) : base(Nome, Cognome)
            {
            }
        }

        class Utente : Persona
        {
            public string Telefono { get; set; }
            public string Email { get; set; }
            public string Password { private get; set; }

            public Utente(string Nome, string Cognome, string Telefono, string Email, string Password) : base(Nome, Cognome)
            {
                this.Telefono = Telefono;
                this.Email = Email;
                this.Password = Password;
            }

            public override string ToString()
            {
                return string.Format("Nome:{0}\nCognome:{1}\nTelefono:{2}\nEmail:{3}\nPassword:{4}",
                    this.Nome,
                    this.Cognome,
                    this.Telefono,
                    this.Email,
                    this.Password);
            }
        }

        class Prestito
        {
            public String Numero { get; set; }
            public DateTime Dal { get; set; }
            public DateTime Al { get; set; }
            public Utente Utente { get; set; }
            public Documento Documento { get; set; }

            public Prestito(String Numero, DateTime Dal, DateTime Al, Utente Utente, Documento Documento)
            {
                this.Numero = Numero;
                this.Dal = Dal;
                this.Al = Al;
                this.Utente = Utente;
                this.Documento = Documento;
                this.Documento.Stato = Stato.Prestito;
            }

            public override string ToString()
            {
                return string.Format("Numero:{0}\nDal:{1}\nAl:{2}\nStato:{3}\nUtente:\n{4}\nDocumento:\n{5}",
                    this.Numero,
                    this.Dal,
                    this.Al,
                    this.Documento.Stato,
                    this.Utente.ToString(),
                    this.Documento.ToString());
            }
        }
    }
}
