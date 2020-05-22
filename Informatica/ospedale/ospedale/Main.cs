using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Figgle;
namespace ospedale
{
    class MainClass
    {
        #region var_globali
        // Contenitore di reparti
        public static List<string> reparti_cnt = new List<string>();
        public static List<Reparto> reparti = new List<Reparto>();
        #endregion

        #region inizio
        public static void Main(string[] args)
        {
            giallo();
            Console.Write(
            FiggleFonts.ThreePoint.Render("Ospedale Maggiore Di Novara"));
            Main_Menu();
            
        }
        #endregion

        #region menu
        public static void stampa_menu()
        {
            reset();
            Console.Write("a. Visualizzare per ogni reparto, il numero di pazienti ricoverati oggi per \"coronavirus\"\n" +
                          "b. Visualizzare il numero di dimessi guariti da coronavirus, data in input una data\n" +
                          "c. Dato un padiglione, mostrare i nomi dei reparti, ordinati per piano dell’edificio\n" +
                          "d. Data la tessera sanitaria di un paziente visualizzare il padiglione, il reparto,\n   la stanza e il letto dove si trova ricoverato ora e da quanti giorni è ricoverato\n" +
                          "e. Dato il nome di un reparto visualizzare il nome del suo primario\n" +
                          "Scelta: ");
        }
        public static void Main_Menu()
        {
            // Caricamento del csv
            carica_csv();
            Console.Clear();
            char scelta = 'a';
            while(scelta != 'x')
            {
                // Titolo
                giallo();
                Console.Write(
                FiggleFonts.ThreePoint.Render("Ospedale Maggiore Di Novara"));
                // Menu
                stampa_menu();
                scelta = Console.ReadLine()[0];
                // Divido
                divido();
                // Analizzo la scelta
                case_scelta(scelta);
                // Pulisco la console
                reset();
                divido();
                Console.WriteLine("Premere un tasto per continuare...");
                Console.ReadKey();
                Console.Clear();
            }

        }
        public static void divido()
        {
            Console.WriteLine("________________________");
            Console.WriteLine("________________________");
        }
        public static void case_scelta(char scelta)
        {
            switch(scelta)
            {
                // Reparto con i contagi
                case 'a':
                    conta_contagi();
                    break;
                // Dimessi data una data
                case 'b':
                    data_guariti();
                    break; 
                // Nome reparti ordinati per piani
                case 'c':
                    reparti_ordinati();
                    break;
                // Tessera sanitaria del paziente
                case 'd':
                    tessera_paziente();
                    break;
                // Primario
                case 'e':
                    cerca_primario();
                    break;
                case 'x':
                    rosso();
                    Console.WriteLine("Uscita in corso");
                    break;
                // errore
                default:
                    rosso();
                    Console.WriteLine("opzione {0} non esiste", scelta);
                    break;
            }
        }
        #endregion

        #region opzioni
        public static void tessera_paziente()
        {
            // Input
            giallo();
            Console.Write("Tessera paziente (frncsccppm145199772): ");
            string tessera = Console.ReadLine();
            // Ricerca
            var val = from i in reparti
                      from j in i.pazieni_
                      where j.Tessera_Sanitaria == tessera
                      select new
                      {
                          pad = i.padiglione,
                          rep = i.nome,
                          stanza = j.n_stanza,
                          letto = j.n_letto,
                          ric = j.Data_Ricovero
                      };
            // Output
            if ( val.ToArray().Length != 0 )
            {
                var ris = val.ToArray()[0];
                verde();
                Console.WriteLine("Padiglione {0} Reparto {1} Stanza {2} Letto {3} Giorni dall'ultimo ricovero {4}",ris.pad, ris.rep, ris.stanza, ris.letto, da_data(ris.ric));
            }else
            {
                // Errore
                rosso();
                Console.WriteLine("Tessera {0} inesistente", tessera);
            }
        }
        public static int da_data(string data)
        {
            DateTime now = DateTime.Now;
            DateTime prima = Convert.ToDateTime(data);
            return (int)(now - prima).TotalDays;
        }
        public static void data_guariti()
        {
            // Prendo in input
            giallo();
            Console.Write("Data (28/12/2020): ");
            string data = Console.ReadLine();
            // Cerco
            var cont = from i in reparti
                       from j in i.pazieni_
                       where j.Dimessione_Decesso.Split()[0] == "dimisso"
                       where j.Dimessione_Decesso.Split()[1] == data
                       select j;
            if ( cont.ToArray().Length != 0)
            {
                verde();
                Console.WriteLine("Giorno {0} dimessi " + cont.ToArray().Length, data);
            }else
            {
                // Errore
                rosso();
                Console.WriteLine("Non è stato trovato nessuno a data {0}", data);
            }
        }
        // Opzione a
        public static void conta_contagi()
        {
            // Itera per tutti i reparti e conta i contagi
            var n = from i in reparti
                    select new
                    {
                        nome = i.nome,
                        n = i.conta_contagi_in()
                    };
            // Stampa
            foreach(var rip in n.ToArray())
            {
                verde();
                Console.WriteLine("___{0}___", rip.nome);
                giallo();
                Console.WriteLine("Contagi: {0}", rip.n);
            }
        }
        // Opzione e
        public static void cerca_primario()
        {
            // Input
            giallo();
            Console.Write("Reparto: ");
            string reparto = Console.ReadLine();
            // Ricerca reparto
            var primario = from i in reparti
                           where i.nome.ToLower() == reparto.ToLower()
                           from j in i.medici_
                           where j.ruolo == "Capo reparto"
                           select j.nome + " " + j.cognome;
            // Se è stato trovato
            if ( primario.ToArray().Length != 0 )
            {
                verde();
                Console.WriteLine("___{0}___", reparto);
                giallo();
                Console.WriteLine("Primario: " + primario.ToArray()[0]);
            }
            else
            {
                // Se non è statot trovato
                rosso();
                Console.WriteLine("Reparto {0} non esistente", reparto);
            }
            
        }
        // Opzione c
        public static void reparti_ordinati()
        {
            // Input padiglione
            giallo();
            Console.Write("Padiglione: ");
            char pad = Console.ReadLine()[0];
            // Prima prendo tutti i padiglioni e poi ordino
            var pad_scelt = reparti.Where(elem => elem.padiglione == pad).Select(elem => elem).OrderBy(elem => elem.piano).ToArray();
            if (pad_scelt.Length != 0)
            {
                // Se è giusto
                verde();
                Console.WriteLine("___Padiglione {0}___");
                giallo();
                foreach (var sc in pad_scelt)
                {
                    Console.WriteLine("Reparto {0} piano " + sc.piano, sc.nome);
                }
            }
            else
            {
                // Senò errore
                rosso();
                Console.WriteLine("Padiglione {0} inesistente", pad);
            }

        }

        #endregion

        #region caricamento_file
        public static void carica_csv()
        {
            /*
              * Funzione figlia della funzione carica_csv.
              * E' l'algoritmo che legge il file csv e ne carica i dati.
              * Se lettura è true allora dobbiamo leggere i medici,
              * Se lettura è false allora dobbiamo leggere i pazienti
              */
            void alg_csv(string path_fin, bool lettura)
            {
                // Se il fine non esiste allora scaricalo
                if (!File.Exists(path_fin))
                    download_file(path_fin);
                string nome;
                if (path_fin.Contains("medici"))
                    nome = "medici";
                else
                    nome = "pazienti";
                // Inizia la lettura
                using (var reader = new StreamReader(path_fin))
                {
                    giallo();
                    Console.WriteLine("Caricamento {0} in corso", nome);
                    // Salta la prima riga
                    bool salto = false;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (salto)
                        {
                            var values = line.Split(',');

                            // Se è vero, allora stiamo analizzando i medici
                            if (lettura)
                            {
                                // E' diviso in:
                                // Nome[0],Cognome[1],Ruolo[2],padiglione[3],reparto[4],piano reparto[5],Numero di telefono[6],Abitazione,data di nascita[7]
                                // Se reparti non esiste
                                if (!reparti_cnt.Contains(values[4]))
                                {
                                    // Aggiungilo
                                    reparti_cnt.Add(values[4]);
                                    reparti.Add(new Reparto(values[4],values[5],values[3][0]));;
                                }
                                
                                int n_tel;
                                int.TryParse(values[6], out n_tel);
                                // Creo il tutto
                                reparti[reparti_cnt.IndexOf(values[4])].aggiungi_medico(values[0], values[1], values[2], n_tel, values[7], values[8], values[9]);


                            }
                            else
                            // Senò stiamo analizzando i pazienit
                            {
                                // E' diviso in:
                                // Nome[0],Cognome[1],sesso[2],Data di nascita[3],padiglione[4],reparto[5],data ricovero[6],numero della stanza[7],numero del letto[8],descrizione malattia[9],dimissione/decesso[10],tessera sanitaria[11]
                                int n_stanza,
                                    n_letto;
                                int.TryParse(values[8], out n_stanza);
                                int.TryParse(values[9], out n_letto);

                                reparti[reparti_cnt.IndexOf(values[5])].aggiungi_paziente(values[0], values[1], values[2][0], values[3], values[6], n_stanza, n_letto, values[9], values[10], values[11]);

                            }


                        }
                        // Siamo nel primo giro, salta il prossimo
                        else salto = true;
                    }
                }
                Thread.Sleep(400);
                verde();
                Console.WriteLine("Caricamento {0} completato", nome);
                Thread.Sleep(400);
            }

            // Prendo il path di esecuzione
            string path = AppDomain.CurrentDomain.BaseDirectory;
            // Prendo il root del processo
            path = path.Substring(0, path.LastIndexOf("ospedale") + 8) + "/dataset/";
            // Raccolgo tutti i medici
            alg_csv(string.Concat(path, "medici.csv"), true);
            // Raccolgo tutti i pazienti
            alg_csv(string.Concat(path, "pazienti.csv"), false);
        }

        #endregion

        #region download_file
        public static void download_file(string path)
        {
            // Apre
            using (var client = new WebClient())
            {
                string nome;
                string url;
                // Controlla se è medici
                if (path.Contains("medici"))
                {
                    nome = "medici";
                    // Scarica
                    url = "https://srv-file12.gofile.io/download/8QI8F6/medici.csv";
                }
                else
                {
                    // Senò è pazienti
                    nome = "pazienti";
                    // Scarica
                    url = "https://srv-file9.gofile.io/download/tVS1Oz/pazienti.csv";
                }
                rosso();
                Console.WriteLine("File {0} non esistente. Download in corso", nome);
                // Scarica i file
                client.DownloadFile(url, path);
                Thread.Sleep(500);
                verde();
                Console.WriteLine("Download completato");
                Thread.Sleep(500);
            }
        }
        #endregion

        #region colori
        // Tutti i colori che il testo di questo programma può avere
        public static void verde()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        public static void rosso()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        public static void giallo()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        public static void reset()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        #endregion
    }
}
