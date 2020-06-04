// Controllo l'esistenza dei dati nella memoria locale
function check_dati() {
    dati = localStorage.getItem("medici")
    if ( dati != null )
    {
        medici = JSON.parse(dati);
        // Allora abbiamo dei dati
        reparti()
    }
}

// Prendo il file e lo inizio ad elaborare
function caricaDati(event) {
    let file_path = document.getElementById("file");
    let file = file_path.files[0].text().then(function(result) { elaboraCSV(result)})
}

// Elaboro
function elaboraCSV(f) {
    // Elaboro i dati
    elabora_dati(f.split('\n'))
    // Transformo i valori
    medici = JSON.stringify(medici)
    localStorage.setItem("medici", medici)
    // Li ri-trasformo per l'elaborazione
    medici = JSON.parse(medici)
    reparti()
}


// Salvo i dati in memoria
function elabora_dati(testo) {
    // Resetto i medici
    medici.nome = medici.padiglione = medici.reparto = medici.ruolo = [];
    let nomi = [],
        padiglione = [],
        ruolo = [],
        reparto = [];
    // Itero ogni riga
    for ( let i = 1; i < testo.length - 1; i++) {
        // Divido
        txt_cs = testo[i].split(',');
        // Salvo
        nomi.push(txt_cs[0] + " " + txt_cs[1]);
        ruolo.push(txt_cs[2]);
        padiglione.push(txt_cs[3]);
        reparto.push(txt_cs[4]);
    }
    medici["nome"] = nomi;
    medici["padiglione"] = padiglione;
    medici["ruolo"] = ruolo;
    medici["reparto"] = reparto;

}

// Creo il select
function reparti() {
    // Setto l'ultimo padiglione e reparto trovato
    let last_pad = '0';
    let last_rep = '0';
    // Prendo il select che andrò a modificare
    var select_mod = document.getElementById("scelta");
    // Separo
    select_mod.innerHTML = ""
    for(let i = 0; i < medici["nome"].length;i++)
    {
        if ( medici["padiglione"][i] != last_pad )
        {
            if ( medici["padiglione"][i] != undefined ) {
                last_pad = medici["padiglione"][i];
                select_mod.innerHTML += "<optgroup label=\"" + last_pad + "\">"
            }
        }
        // Controllo per il reparto
        if ( medici["reparto"][i] != last_rep )
        {
            if ( medici["reparto"][i] != undefined ) {
                last_rep = medici["reparto"][i];
                select_mod.innerHTML += "<option value=\""+last_rep+"\">"+last_rep+"</option>"
            }
        }
    }

}

// Inizio output
function output() {
    // Prendo il select
    let val = document.getElementById("scelta");
    // Il suo valore
    val = val[val.selectedIndex].innerHTML;
    // Prendo l'output
    let output = document.getElementById("output_div");
    // Per il primario
    let prima = false;
    // Resetto l'output
    output.innerHTML = ""
    for(let i = 0; i < medici.nome.length; i++)
    {
        // Se è il reparto che stiamo cercando
        if ( medici.reparto[i] == val ) {
            // Prima parte dei medici
            output.innerHTML += "<div class='medici'> "
            // Se non è un primario
            if (prima) {
                output.innerHTML += "<i class=\"fas fa-laptop-medical\"></i>"
            } else {
                // Se è un primario
                prima = true;
                output.innerHTML += "<i class=\"fas fa-notes-medical\"></i>"
            }
            output.innerHTML += " " + medici.nome[i];
            // Ultima parte dei medici
            output.innerHTML += "</div><br>"
        }

    }
}
// Setto la variabile
let medici = {
    nome:[],
    padiglione:[],
    ruolo:[],
    reparto: []
}