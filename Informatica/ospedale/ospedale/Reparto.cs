using System;
using System.Collections.Generic;
namespace ospedale
{
    public class Reparto
    {
        #region variabili
        public string nome;
        public int piano;
        public char padiglione;
        public List<medici> medici_ = new List<medici>();
        public List<pazienti> pazieni_ = new List<pazienti>();
        #endregion

        #region costruttore
        public Reparto(string nome, string piano, char padiglione)
        {
            this.nome = nome;
            int.TryParse(piano, out this.piano);
            this.padiglione = padiglione;
        }
        #endregion

        #region aggiungi_medico
        public void aggiungi_medico(string nome, string cognome, string ruolo, int n_telefono, string abituazione, string data_di_nascita, string tessera)
        {
            medici_.Add(new medici(nome, cognome, ruolo, n_telefono, abituazione, data_di_nascita, tessera));
        }
        #endregion

        #region aggiungi_paziente
        public void aggiungi_paziente(string nome, string cognome, char sesso, string data_nascita, string data_ricovero, int n_stanza, int n_letto, string malattia, string dim_dec, string tessera_sanitaria)
        {
            pazieni_.Add(new pazienti(nome, cognome, sesso, data_nascita, data_ricovero, n_stanza, n_letto, malattia, dim_dec, tessera_sanitaria));
        }
        #endregion

        #region conta_contagi
        public int conta_contagi_in()
        {
            int cont = 0;
            // Itero per tutti i pazienti
            foreach(var paziente in pazieni_)
            {
                // Aggiungi se lo contiene
                if (paziente.Malattia.Contains("coronavirus"))
                    cont++;
            }
            return cont;
        }
        #endregion
    }
}
