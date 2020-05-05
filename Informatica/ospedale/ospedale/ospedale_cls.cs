/*
 * Nome: ospedale_cls.cs
 * Progetto: Ospedale
 * Gruppo: 3
 * Partecipanti del gruppo: Condello Alessandro, Bergantin Andrea, Gavinelli Riccardo
 * Ultima modifica: 05/05/2020 
 */
using System;
using System.IO;
namespace ospedale
{
    public class ospedale_cls
    {
        // Contenitore di tutti i reparti
        public padiglione_cls[] padiglioni = new padiglione_cls[27];

        /*
         * Ricerca di un reparto.
         * Restituisce una stringa:
         * Diversa da no se è stata trova.
         * Funzione secondaria: Chiamata da capo_reparto
         */
        private string Ricerca_(string val)
        {

            foreach (padiglione_cls padiglione in padiglioni)
            {
                foreach(reparto_cls reparto in padiglione.reparti)
                {
                    if ( string.Compare(reparto.nome.ToLower(), val.ToLower()) == 0 )
                    {
                        return reparto.capo_reparto;
                    }
                }
            }
            return "no";
        }

        // Richiesta della ricerca del primario in un reparto. Funzione iniziale
        public void capo_reparto()
        {
            

            Console.Write("Nome reparto: ");
            string nome = Console.ReadLine();

            string ris = Ricerca_(nome);
            if (string.Compare(ris, "no") == 0)
            {
                Console.WriteLine("Controlla di aver scritto bene il nome");
            }
            else Console.WriteLine("Nome del primario: " + ris);
        }

        // Conta il numero di coronavirus. Funzione iniziale
        public void coronavirus_cont()
        {
            foreach(padiglione_cls padiglione in padiglioni)
            {
                if (padiglione != null)
                {
                    Console.WriteLine("Padiglione: " + padiglione.padiglione_car);
                    padiglione.cont_coronavirus();
                }
            }
        }

        // Carica il file csv. Funzione iniziale
        public void carica_csv()
        {

            /*
             * Funzione figlia della funzione carica_csv.
             * E' l'algoritmo che legge il file csv e ne carica i dati.
             * Se lettura è true allora dobbiamo leggere i medici,
             * Se lettura è false allora dobbiamo leggere i pazienti
             */
            void alg_csv(string path_fin, bool lettura)
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
                                
                                padiglioni[values[4][0] - 97].aggiungi_paziente(values[0],values[1],values[2][0], values[3],values[5],values[6],n_stanza,n_letto,values[9],values[10],values[11]);
                                
                            }


                        }
                        // Siamo nel primo giro, salta il prossimo
                        else salto = true;
                    }
                }
            }

            // Prendo il path di esecuzione
            string path = AppDomain.CurrentDomain.BaseDirectory;
            // Prendo il root del processo
            path = path.Substring(0, path.LastIndexOf("Informatica") + 11) + "/dataset/";
            
            // Raccolgo tutti i medici
            alg_csv(string.Concat(path, "medici.csv"), true);
            // Raccolgo tutti i pazienti
            alg_csv(string.Concat(path, "pazienti.csv"), false);

        }
    }
}
