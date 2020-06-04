/*
 * Nome: reparto_cls.cs
 * Progetto: Ospedale
 * Gruppo: 3
 * Partecipanti del gruppo: Condello Alessandro, Bergantin Andrea, Gavinelli Riccardo
 * Ultima modifica: 17/05/2020 
 */
using System.Collections.Generic;
namespace ospedale
{
    public class reparto_cls
    {
        // Informazione del reparto
        public string nome;
        public string piano;
        public string capo_reparto;
        // Equip medica
        List<medico_cls> equip_medica = new List<medico_cls>();
        // Lista pazienti
        List<paziente_cls> pazienti = new List<paziente_cls>();
        // Quando si aggiunge un reparto
        public reparto_cls(string nome, string piano)
        {
            this.nome = nome;
            this.piano = piano;
        }

        // Aggiunta del medico
        public void aggiungi_medico(string nome, string cognome, string ruolo, int n_telefono, string abitazione, string data_nascita)
        {
            // Se è il capo reparto allora scrivilo
            if (ruolo == "Capo reparto")
                this.capo_reparto = string.Concat(nome, " ", cognome);
            // Aggiungilo all'equipe medica
            this.equip_medica.Add(new medico_cls(nome, cognome, ruolo, n_telefono, abitazione, data_nascita));
        }
        // Aggiunta di un paziente
        public void aggiungi_paziente(string nome, string cognome, char sesso, string data_nascita, string data_ricovero, int n_stanza, int n_letto, string malattia, string dim_dec, string tessera)
        {
            this.pazienti.Add(new paziente_cls(nome, cognome, sesso, data_nascita, data_ricovero, n_stanza, n_letto, malattia, dim_dec, tessera));
        }

        // Resistuisce il numero di persone col coronavirus 
        public int conta_virus()
        {
            // inizializzo il contatore
            int cont = 0;
            // Itero per ogni paziente
            foreach(paziente_cls paziente in pazienti)
            {
                // Se la descrizione della malattia contiene "coronavirus"
                if (paziente.Malattia.Contains("coronavirus"))
                    cont += 1;
            }

            return cont;
        }

    }
}
