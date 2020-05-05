/*
 * Nome: padiglione_cls.cs
 * Progetto: Ospedale
 * Gruppo: 3
 * Partecipanti del gruppo: Condello Alessandro, Bergantin Andrea, Gavinelli Riccardo
 * Ultima modifica: 05/05/2020 
 */
using System;
using System.Collections.Generic;
namespace ospedale
{
    public class padiglione_cls
    {
        // Contiene tutti i reparti di un padiglione. Viene aggiornato pian pianino
        // Che un padiglione viene aggiunto
        public List<string> reparti_cnt = new List<string>();
        public char padiglione_car;
        // Contenitore dei reparti
        public List<reparto_cls> reparti = new List<reparto_cls>();

        public padiglione_cls(char car)
        {
            this.padiglione_car = car;
        }
        // Aggiunge un medico
        public void aggiungi_medico(string nome, string cognome, string ruolo, string reparto_nome_in, string piano, int n_telefono, string abitazione, string data_nascita)
        {
            // Aggiunta del reparto nel caso non esista
            if (! reparti_cnt.Contains(reparto_nome_in) )
            {
                // Allora aggiungi il reparto
                this.reparti_cnt.Add(reparto_nome_in);
                this.reparti.Add(new reparto_cls(reparto_nome_in, piano));
                
            }
            // Aggiungi il medico al reparto
            this.reparti[this.reparti_cnt.IndexOf(reparto_nome_in)].aggiungi_medico(nome,cognome,ruolo,n_telefono,abitazione,data_nascita);

        }
        // Aggiungi un paziente a un reparto
        public void aggiungi_paziente(string nome, string cognome, char sesso, string data_nascita, string reparto, string data_ricovero, int n_stanza, int n_letto, string malattia, string dim_dec, string tessera)
        {
            this.reparti[this.reparti_cnt.IndexOf(reparto)].aggiungi_paziente(nome, cognome, sesso, data_nascita, data_ricovero, n_stanza, n_letto, malattia, dim_dec, tessera);
        }
        // Conta il numero di persone col coronavirus
        public void cont_coronavirus()
        {
            int i = 0;
            // itera per ogni reparto
            foreach(string reparto in reparti_cnt)
            {
                Console.WriteLine("Reparto {0} contagi: {1}",reparto, reparti[i].conta_virus() );
            }
        }
    }
}
