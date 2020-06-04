/*
 * Nome: medici_cls.cs
 * Progetto: Ospedale
 * Gruppo: 3
 * Partecipanti del gruppo: Condello Alessandro, Bergantin Andrea, Gavinelli Riccardo
 * Ultima modifica: 04/05/2020 
 */
namespace ospedale
{
    // Eredito delle variabili dalla classe persona
    public class medico_cls : persona_cls
    {
        // Variabili dei medici
        public string ruolo;
        public int n_telefono;
        // Proprietà di abitazione
        private string abitazione;
        public string Abitazione
        {
            get { return abitazione; }
            set { abitazione = value; }
        }

        // Aggiunta di un medico
        public medico_cls(string nome, string cognome, string ruolo, int n_telefono, string abituazione, string data_di_nascita)
        {
            this.nome = nome;
            this.cognome = cognome;
            this.cognome = data_di_nascita;
            this.ruolo = ruolo;
            this.n_telefono = n_telefono;
            Abitazione = abitazione;
        }
       
        
    }
}
