/*
 * Nome: ospedale_cls.cs
 * Progetto: Ospedale
 * Gruppo: 3
 * Partecipanti del gruppo: Condello Alessandro, Bergantin Andrea, Gavinelli Riccardo
 * Ultima modifica: 17/05/2020 
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Threading;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ospedale
{
    public class ospedale_cls
    {
        // Contenitore di tutti i reparti
        public padiglione_cls[] padiglioni = new padiglione_cls[27];


        // Restituisce un vettore di stringhe contenente tutti i padiglioni
        public string[] getPadiglioni()
        {
            var pad = from i in padiglioni
                      where i != null
                      from j in i.reparti
                      select j.nome;
            return pad.ToArray();
      
        }

        /*
         * Ricerca di un reparto.
         * Restituisce una stringa:
         * Funzione secondaria: Chiamata da capo_reparto
         */
        private string Ricerca_(string val)
        {

            foreach (padiglione_cls padiglione in padiglioni)
            {

                foreach (reparto_cls reparto in padiglione.reparti)
                {
                    if (string.Compare(reparto.nome.ToLower(), val.ToLower()) == 0)
                    {
                        return reparto.capo_reparto;
                    }
                }
            }
            return "";

        }

        // Richiesta della ricerca del primario in un reparto. Funzione iniziale
        public string capo_reparto(string nome)
        {
            foreach (padiglione_cls padiglione in padiglioni)
            {

                foreach (reparto_cls reparto in padiglione.reparti)
                {
                    if (string.Compare(reparto.nome.ToLower(), nome.ToLower()) == 0)
                    {
                        return reparto.capo_reparto;
                    }
                }
            }
            // Per silenziare l'errore
            return "";
        }

        // Conta il numero di coronavirus. Funzione iniziale
        public List<string> coronavirus_cont()
        {
            // Siccome voglio dividere il lavoro su 2 task, devo avere 2 liste
            List<string> nomi1 = new List<string>();
            List<string> nomi2 = new List<string>();
            // Ricavo tutti padiglioni
            List<int> lista_pad_disp = new List<int>();
            for (int i = 0; i < padiglioni.Length; i++)
            {
                if (padiglioni[i] != null)
                    lista_pad_disp.Add(i);
            }

            // Divido l'array in 2 parti: 1 è responsabile per il task1
            // L'altra per il task 2
            List<int> prima = new List<int>(),
                      dopo = new List<int>();
            int meta = lista_pad_disp.Count / 2;
            for (int i = 0; i < lista_pad_disp.Count; i++)
            {
                if (i < meta)
                    prima.Add(lista_pad_disp[i]);
                else
                    dopo.Add(lista_pad_disp[i]);
            }

            Task secondo = Task.Factory.StartNew(() =>
            {
                nomi2 = analisi_suddivisa(dopo);
            });

            nomi1 = analisi_suddivisa(prima);

            secondo.Wait();
            nomi1.AddRange(nomi2);
            return nomi1;
        }

        public List<string> analisi_suddivisa(List<int> array)
        {
            List<string> output = new List<string>();

            foreach(int idx in array)
            {
                Console.WriteLine("Padiglione: " + padiglioni[idx].padiglione_car);
                output.AddRange(padiglioni[idx].cont_coronavirus());
            }

            return output;
        }

        // Carica il file csv. Funzione iniziale
        // Ritorna false se è stato trovato un problema
        public string carica_csv()
        {

            /*
             * Funzione figlia della funzione carica_csv.
             * E' l'algoritmo che legge il file csv e ne carica i dati.
             * Se lettura è true allora dobbiamo leggere i medici,
             * Se lettura è false allora dobbiamo leggere i pazienti
             */
            Boolean alg_csv(string path_fin, bool lettura)
            {
                // System.IO.FileNotFoundException: 
                Console.WriteLine("Prova");
                try
                {
                    using (var reader = new StreamReader(path_fin))
                    {
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
                                    // Nome[0],Cognome[1],Ruolo,padiglione[2],reparto[3],piano reparto[4],Numero di telefono[5],Abitazione,data di nascita[6]
                                    // Se la lettera del padiglione non esiste, allora aggiungila
                                    if (padiglioni[values[3][0] - 97] == null)
                                        // Allora vuol dire che il padiglione non è ancora stato creato
                                        padiglioni[values[3][0] - 97] = new padiglione_cls(values[3][0]);
                                    int n_tel;
                                    int.TryParse(values[6], out n_tel);
                                    // Creo il tutto
                                    padiglioni[values[3][0] - 97].aggiungi_medico(values[0], values[1], values[2], values[4], values[5], n_tel, values[7], values[8]);


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

                                    padiglioni[values[4][0] - 97].aggiungi_paziente(values[0], values[1], values[2][0], values[3], values[5], values[6], n_stanza, n_letto, values[9], values[10], values[11]);

                                }


                            }
                            // Siamo nel primo giro, salta il prossimo
                            else salto = true;
                        }
                    }
                }catch (FileNotFoundException e)
                {
                    return false;
                };
                return true;
            }

            // Prendo il path di esecuzione
            string path = AppDomain.CurrentDomain.BaseDirectory;
            // Prendo il root del processo
            path = path.Substring(0, path.LastIndexOf("ospedale") + 8) + "/dataset/";

            Boolean successo = new Boolean();
            // Raccolgo tutti i medici
            successo = alg_csv(string.Concat(path, "medici.csv"), true);
            if (!successo)
                return "medici";
            // Raccolgo tutti i pazienti
            successo = alg_csv(string.Concat(path, "pazienti.csv"), false);
            if (!successo)
                return "pazienti";
            return "";

        }
    }
}
