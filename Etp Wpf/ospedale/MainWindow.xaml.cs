/*
 * Nome: MainWindow.xaml.cs
 * Progetto: Ospedale
 * Gruppo: 3
 * Partecipanti del gruppo: Condello Alessandro, Bergantin Andrea, Gavinelli Riccardo
 * Ultima modifica: 17/05/2020 
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
// Per i link ( https://stackoverflow.com/questions/10238694/example-using-hyperlink-in-wpf )
using System.Diagnostics;
using System.Windows.Navigation;
// Per il salvataggio dei file 
using System.IO;
using Microsoft.Win32;

namespace ospedale
{
   

    public partial class MainWindow : Window
    {
        /// Variabili
        // Varabile ospedale 
        ospedale_cls ospedale = new ospedale_cls();
        // Controlla se è stato caricato almeno una volta il dataset
        bool caricato = false;
        // lista degli errori
        Dictionary<string, string> Errori_Dict = new Dictionary<string, string>
        {
            {"reparto_mancante", "Non è stato selezionato nessun reparto" },
            {"medici", "Il dataset medici non è stato caricato" },
            {"pazienti", "Il dataset pazienti non è stato caricato" },
            {"noName", "Il file deve contenere nel nome o medici oppure pazienti" }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        // Per i link ( https://stackoverflow.com/questions/10238694/example-using-hyperlink-in-wpf )
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        // Funzione chiamata dal bottone "carica"
        private void btn_carica_via(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() => { carica(); });
        }

        // Funzione chiamata da "btn_carica_via"
        // Questa funzione è la responsabile del caricamento dei dati in memoria
        public void carica()
        {
            disattiva_bottoni();
            // Inizio il caricamento
            string ok = ospedale.carica_csv();
            if (ok == "medici")
                errore_out(Errori_Dict["medici"]);
            else if (ok == "pazienti")
                errore_out(Errori_Dict["pazienti"]);
            else
            {
                caricato = true;
                // Ricavo tutti i reparti
                string[] padiglioni = ospedale.getPadiglioni();
                // Aggiungo i padiglioni al combobox
                Dispatcher.Invoke(() =>
                {
                // Rimuovo
                input_reparto.Items.Clear();
                // Ciclo per ogni padiglione
                foreach (string padiglione in padiglioni)
                    // Aggiungo al combobox
                    input_reparto.Items.Add(padiglione);
                });
            }
            // Riattivo i bottoni
            attiva_bottoni();
        }

        // Funzione chiamata dal bottone avvia
        // Prende il nome del reparto selezionato e avvia la funzione avvia_
        private void brn_avvia_via(object sender, RoutedEventArgs e)
        {
            string nome = input_reparto.Text;
            Task.Factory.StartNew(() => { avvia_(nome); });
        }

        /*
         *  Avvia l'analisi dei dati
         *  Funzione chiamata dalla funzone btn_avvia_via
         *  Questa funzione ha 2 funzioni: 
         *  1) Ricercare del primario (1 solo task)
         *  2) Contare il numero di contagi per coronavirus (2 task)
         */
        public void avvia_(string nome)
        {
            disattiva_bottoni();
            // Nel caso sia stato prima selezionato un reparto
            if (nome != "")
            {
                string nome_primario = "";
                Task primari_task = Task.Factory.StartNew(() => { nome_primario = ospedale.capo_reparto(nome); });
                List<string> val = new List<string>();
                val = ospedale.coronavirus_cont();
                // Divido in 2 val
                List<string> prima = new List<string>(),
                             secondo = new List<string>();
                // Divido ed aggiungo
                int meta = val.Count / 2;
                for (int i = 0; i < val.Count; i++)
                {
                    if (i < meta)
                        prima.Add(val[i]);
                    else
                        secondo.Add(val[i]);
                }
                // Aspetto che finisce il task
                primari_task.Wait();
                Dispatcher.Invoke(() =>
                {
                    // Setto il nome del primario
                    output_primario.Content = nome_primario;
                    // Aggiungo i nomi alle listbox
                    foreach (string item in prima)
                    {
                        output_1.Items.Add(item);
                    }
                    foreach (string item in secondo)
                    {
                        output_2.Items.Add(item);
                    }
                });
            }
            // Senò manda un erroe
            else errore_out(Errori_Dict["reparto_mancante"]);
            attiva_bottoni();
        }

        // Mostra su schermo un messaggio (errore)
        public void errore_out(string error)
        {
            // Modifica il testo del label errore
            Dispatcher.Invoke(() =>
            {
                errore.Content = error;
            
            });
            // Aspetta 4 secondi
            Thread.Sleep(4000);
            // Resetta il testo
            Dispatcher.Invoke(() =>
            {
                errore.Content = "";

            });
        }

        // Attiva tutti i bottoni
        public void attiva_bottoni()
        {
            // Disattiva il bottone "avvia" solamente se il programma ha caricato almeno 1 volta
            Dispatcher.Invoke(() => { carica_btn.IsEnabled = true; if (caricato) avvia_btn.IsEnabled = true; aggiorna_dataset_btn.IsEnabled = true; });
        }

        // Disattiva tutti i bottoni
        public void disattiva_bottoni()
        {
            Dispatcher.Invoke(() => { carica_btn.IsEnabled = false; avvia_btn.IsEnabled = false; aggiorna_dataset_btn.IsEnabled = false; });
        }

        
        private void upload_file_via(object sender, RoutedEventArgs e)
        {
            // Apre il dialogo
            OpenFileDialog fd = new OpenFileDialog(); 
            // Richiedo un file csv
            fd.DefaultExt = ".csv";
            // Quando ha scelto
            if ( fd.ShowDialog() == true)
            {
                // Prendo il path di esecuzione
                string path1 = AppDomain.CurrentDomain.BaseDirectory;
                // Prendo il path della cartella
                string path = path1.Substring(0, path1.LastIndexOf("ospedale") + 8) + "/dataset/";
                // Se contiene "medici"
                if (fd.FileName.ToLower().Contains("medici"))
                    // Aggiungi medici al path
                    path = path + "medici.csv";
                else if (fd.FileName.ToLower().Contains("pazienti"))
                    // Senò aggiungi pazienti
                    path += "pazienti.csv";
                else
                    // Senò invia messaggi di errore
                    errore_out(Errori_Dict["noName"]);
                // Controlla se contiene l'estensione csv
                if ( path.Contains(".csv"))
                    using (StreamWriter writer = new StreamWriter(path))
                    {
                        // Scrive il file
                        writer.Write(System.IO.File.ReadAllText(fd.FileName));
                    }
            }

        }
        

    }
}
