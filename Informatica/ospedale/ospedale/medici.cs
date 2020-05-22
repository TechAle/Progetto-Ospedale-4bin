using System;
namespace ospedale
{
    public class medici : persona_cls
    {
        #region variabili
        // Variabili dei medici
        public string ruolo;
        public int n_telefono;
        public string tessera_sanitaria;
        // Proprietà di abitazione
        private string abitazione;
        public string Abitazione
        {
            get { return abitazione; }
            set { abitazione = value; }
        }
        #endregion

        #region costruttore
        public medici(string nome, string cognome, string ruolo, int n_telefono, string abituazione, string data_di_nascita, string tessera)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.data_nascita = data_di_nascita;
            this.ruolo = ruolo;
            this.n_telefono = n_telefono;
            Abitazione = abitazione;
            this.tessera_sanitaria = tessera;
        }
        #endregion
    }
}
