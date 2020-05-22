try:
    ## Per semplificarci la vita con l'html
    from bs4 import BeautifulSoup
except:
    print("Devi installare bs4 per avviare questo programma.\nPer installarlo: pip install -U bs4")
try:
    ## Per ricevere il codice sorgente delle pagine
    import requests
except:
    print("Devi installare requests per avviare questo programma.\nPer installarlo: pip install -U requests")
## Per le varie casualità
import random
## Per la gestione dei file csv
import csv
## Per leggere il dataset dei nomi
import pandas as pd
## Per rimuovere le consonanti
import re


## Classe che gestisce il tutto
class gestore:
    def __init__(self):
        ## Preparo i dati
        self.cont = {"parole": set(), "cognomi": set(), "nomi" : set()}
        self.aggiungi_malattie()
        ## Preparo il dataset con i nomi
        self.getNomi_pre()
        ## Prendo tutti i reparti
        self.getReparti()

    def getNomi_pre(self):
        self.nomi = pd.read_csv('https://query.data.world/s/5xdo7ixiohmuq4yj5sdslgolah6qxh')
        self.nomi = self.nomi.replace("boy","maschio").replace("girl","femmina")

    ## Aggiungo le parole, i cognomi e i nomi
    def aggiungi(self):
        for i in self.cont.keys():
            ## Fà riferimento a una funzione (quella sotto)
            self.setting(i)

    def aggiungi_malattie(self):
        self.cont["malattie"] = []
        with open("./dataset/malattie.txt", "r") as rd:
            for i in rd.read().split("\n"):
                self.cont["malattie"].append(i)


        self.coronavirus = "La malattia da coronavirus (COVID-19) è una malattia infettiva causata da un nuovo virus. Il virus causa una malattia respiratoria (come l'influenza) con sintomi quali tosse febbre e nei casi più gravi difficoltà respiratorie. Puoi proteggerti lavandoti spesso le mani evitando di toccarti il viso ed evitando il contatto ravvicinato con persone che non stanno bene (mantieni un metro di distanza)."

    ## Prende la parola, la cerca nel dataset per poi metterla tutta nel set
    def setting(self, parola):
        if parola != "malattie":
            ## Aggiungo tutti i vari dataset ai rispettivi dizionari
            with open("./dataset/{}.txt".format(parola), "r") as rd:
                ## Apro, leggo, splitto
                for i in rd.read().split():
                    ## Aggiungo
                    self.cont[parola].add(i)

    ## Ritorna una stringa, un numero di telefono univoco
    def nuovoNum(self):
        ## Prendo il valore
        numero = self.getNumero()
        ## Continuo finchè non esiste
        while (self.numero_telefono.__contains__(numero)):
            numero = self.getNumero()
        ## Aggiungo
        self.numero_telefono.add(numero)
        ## ritorno
        return list(self.numero_telefono)[-1]

    ## Crea i file medici e pazienti
    def creaFile(self):
        ## Creo la lista univoca vuota
        self.numero_telefono = set()
        print("Inizia creazione file")
        ## Apro il file
        ## OLtre ai medici ci metto anche i pazienti però in un file separato
        ## Inizio con lo settings dei pazienti
        file_paz = open("./dataset/pazienti.csv", "w")
        writer_paz = csv.writer(file_paz, delimiter=',', quotechar='|', quoting=csv.QUOTE_MINIMAL)
        writer_paz.writerow(["Nome", "Cognome", "sesso", "Data di nascita", "padiglione", "reparto", "data ricovero",
                         "numero della stanza", "numero del letto", "descrizione malattia", "dimissione/decesso",
                         "tessera sanitaria"])
        with open("./dataset/medici.csv", "w") as filecsv:
            ## Settings predefiniti
            writer = csv.writer(filecsv, delimiter=',', quotechar='|', quoting=csv.QUOTE_MINIMAL)
            ## Prima riga
            writer.writerow(["Nome", "Cognome", "Ruolo", "padiglione", "reparto", "piano reparto", "Numero di telefono",
                             "Abitazione", "data di nascita", "tessera_sanitaria"])
            ## Trasformo tutti i dataset da set a lista
            for i in self.cont.keys():
                if not isinstance(i, list):
                    self.cont[i] = list(self.cont[i])
            ## Liste dei nomi e congomi
            nomi = []
            cognomi = []
            ## Contatore di dove siamo arrivati
            cont = 0
            ## Estraggo tutti i nomi e li divido
            for i in self.Nomi:
                j = 1
                if i[1].split()[1] == "dott.":
                    j = 2
                nomi.append(i[1].split()[j])
                cognomi.append(i[1].split()[-1])
            ## Itero in una lista numerata, divido in piani
            conto = 0
            for posto, i in enumerate(self.equip_medica):
                print("Scrittura del piano {0}".format(chr(posto+97)) )
                ## Itero in una lista numerata, divido in reparti
                for cont_,j in enumerate(i):
                    ## Scrivo i pazienti
                    self.scrivi_paziente(writer_paz,str(chr(posto + 97)), self.Nomi[conto][0])
                    self.writer_main(writer, nomi[conto],cognomi[conto],"Capo reparto",str(chr(posto + 97)), self.Nomi[conto][0],self.Nomi[conto][-1])
                    ## Itero dentro al reparto
                    for k in j:
                        if k.__contains__(","):
                            for h in k.split(","):
                                try:
                                    self.writer_main(writer, h.split()[0], h.split()[1], "Equip",
                                                 str(chr(posto + 97)), self.Nomi[conto][0], self.Nomi[conto][-1],)
                                except:
                                    pass
                            break
                        else:
                            self.writer_main(writer, k.split()[0], k.split()[1], "Equip",
                                             str(chr(posto + 97)), self.Nomi[conto][0], self.Nomi[conto][-1])
                    conto += 1
                print("Piano finito")


    def getNome(self):
        return self.nomi.sample().__array__()[0]

    def scrivi_paziente(self, writer, padiglione, reparto):
        malattia = ""
        for i in range(random.randint(5, 20)):
            ## Scriviamo
            nomi_g = self.getNome()
            cognome = self.cont["cognomi"][random.randint(0, len(self.cont["cognomi"]) - 1)]
            data_nascita = self.getData()
            ## Diamo 4/5 di possibilità che sia il coronavirus
            if random.randint(0,4) == 0:
                malattia = self.cont["malattie"][random.randint(0, len(self.cont["malattie"]) - 1)]
            else:
                malattia = self.coronavirus
            writer.writerow([nomi_g[0],
                             cognome,
                             nomi_g[1],
                             data_nascita,
                             padiglione,
                             reparto,
                             self.getDataRicovero(),
                             random.randint(0, 40),
                             random.randint(0, 10),
                             malattia.replace("|",""),
                             self.getDataRecDec(),
                             self.getTesseraSanitaria(nomi_g[0], cognome, nomi_g[1], data_nascita)])

    def getTesseraSanitaria(self,nome,cognome,sesso,data_nascita):
        if sesso == "maschio":
            sesso = "m"
        else:
            sesso = "f"
        return rem_vowel(nome).lower() + rem_vowel(cognome).lower() + sesso + data_nascita.replace("/","") + str(random.randint(0,100))


    ## Gestisce la scrittura del primo caso
    def writer_main(self, writer, nome, cognome, posizione, piano, reparto, piano_reparto):
        data_nascita = self.getData()
        sesso = "m"
        if random.randint(0,1) == 0:
            sesso = "f"
        writer.writerow([nome, cognome, posizione, piano, reparto,
                         piano_reparto, self.nuovoNum(), self.getVia(), data_nascita, self.getTesseraSanitaria(nome,cognome,sesso,data_nascita)])

    ## Serve per ottenere la via del medico
    def getVia(self):
        return "via " + self.cont["nomi"][random.randint(0, len(self.cont["nomi"]) - 1)] + " n^" + str(random.randint(0, 100))

    ## Serve per ottenere il numero di un medico
    def getNumero(self):
        return "39" + str(random.randint(3000000000, 3990000000))

    ## Serve per ottenere la data di nascita del medico
    def getData(self):
        return str(random.randint(1, 30)) + "/" + str(random.randint(1, 12)) + "/" + str(random.randint(1980, 2000))

    def getDataRicovero(self):
        return str(random.randint(1, 30)) + "/" + str(random.randint(1, 12)) + "/" + str(random.randint(2010, 2019))

    def getDataRecDec(self):
        out = ""
        if random.randint(0,1) == 0:
            out = "decesso "
        else:
            out = "dimisso "
        return out + str(random.randint(1, 30)) + "/" + str(random.randint(1, 12)) + "/2020"

    ## Prendo tutti i reparti direttamente dal sito ufficiale di novara
    def getReparti(self):
        ## Link base
        testo_base = "http://www.maggioreosp.novara.it/attivita-assistenziale/strutture-sanitarie/elenco-delle-strutture-sanitarie/padiglione-"
        ## Contenitore di: capo reparto, piano, reparto
        self.Nomi = []
        ## Contenitore dell'equip medica
        self.equip_medica = []
        ## Vari piani. il -2 è il piano complesso
        piani = {"terzo": 3, "seminterrato": -1, "terra": 0, "terreno": 0, "secondo": 2, "quarto": 4, "primo": 1,
                 "rialzato": 0}
        l = 0
        while (True):
            ## A = ascii 97
            lettera = chr(l + 97)
            ## Faccio richesta
            response = requests.get(testo_base + lettera)
            ## Trasformo
            soup = BeautifulSoup(response.content, "html.parser")
            ## Controllo se il sito esiste
            if soup.title.getText().__contains__("Pagina non trovata"):
                break
            else:
                ## Se si allora dì
                print("Controllo piano {0}".format(lettera))
            ## E dividi in sezioni
            prova = soup.findAll("div", {"class", "siteorigin-widget-tinymce textwidget"})
            ## la disposizione sarà: Reparto - Nome capo reparto - piani
            for i in prova[2:]:
                ## Il testo effettivo
                testo = i.getText().split("\n")
                ## Piano deffault
                piano = str(random.randint(-1,4))
                ## Ricavo il piano
                for j in piani.keys():
                    ## Se contiene la key
                    if testo[2].__contains__(j):
                        ## Scrivi il piano e esci
                        piano = piani[j]
                        break


                ## Uso try except siccome non voglio rendere ancora più complicato il codice.
                ## Il reparto "mensa" non contiene nessun gestore, allora gliene dò uno a mia scelta
                try:
                    ## Qui aggiungo alla lista Nomi la seguente cosa:
                    ## Il primo li aggiungo semplicemente il reparto
                    ## Prendo il testo, li tolgo tutti gli spazi vuoti, una volta fatto prendo la parte che ci interessa cioè il capo reparto.
                    ## Il piano
                    self.Nomi.append([testo[1], list(filter(lambda val: val.strip().__len__() != 0, testo))[2].split(":")[1], piano])
                ## Nel caso sia la mensa
                except IndexError:
                    self.Nomi.append([testo[1], " Dottor Lucreazia Grazie", piano])
            ## Estraggo l'equipe medica.
            ## per ogni link che abbiamo, te invialo a get_equip e poi aggiungilo alla lista. Bisogna togliere i primi due valori siccome non ci interessano
            self.equip_medica.append([self.get_equip(link) for link in [exc.find('a')["href"] for exc in prova[2:]]])
            l += 1

    ## La funzione prende come un input un link e restituisce una lista.
    def get_equip(self, link):
        ## La lista che restituiremo
        equip = []
        ## Faccio la richiesta
        response = requests.get(link)
        ## E' tutto in try e expect siccome potrebbe essere che non abbiamo nessuna equip. Per come è fatto, non possiamo
        ## Fare controlli. L'unica maniera è un try except
        try:
            ## Prendo il testo che contiene l'equipe
            prima_parte = \
            BeautifulSoup(response.content, "html.parser").find(id="accordion-content-equipe-%c2%bb").contents[
                1].getText()
            ## Se contiene i : allora vuol dire che è semplice come struttura
            if prima_parte.__contains__(":"):
                ## Per non fare un codice stra lungo ( e siccome sono solamente 2 le pagine quelle diverse ) faccio questa cosa dove le toglie i due casi speciali
                if prima_parte.__len__() < 3000 and prima_parte.__contains__("Antonio RAMPONI") == False:
                    ## Nel caso normale,
                    ## Prendo la prima parte, la divido in parti a seconda del :, prendo ciò che ci interessa e la ridivido per \n. Una volta questo,
                    ## Itero ogni parte della lista e ci toglio le cose in più per poi toglierli le celle vuote e le varie eccezioni.
                    ## Lo rendo una stringa per poi dividerlo ogni ,
                    equip = (list(filter( lambda val: val.split().__len__() != 1, list(filter(
                        lambda val: val.__len__() != 0 and val != "Struttura semplice" and val != "Strutture semplici" and val != "Coordinatore Infermieristico", list(
                            map(lambda val: val.strip(), prima_parte.split(":")[1].split("\n"))))))))
                ## In questi 2 casi, li aggiungo "manualmente"
                elif prima_parte.__contains__("Antonio RAMPONI"):
                    equip = ["Cristiana BOZZOLA", "Francesca FOTI", "Angela GIACALONE", "Monica LEUTNER", "Emanuela UGLIETTI", "Guido VALENTE"]
                else:
                    equip = ["Patrizia NOTARI", "Matteo VIDALI", "Vessellina KRUOMOVA", "Giuseppina ANTONINI", "Ilaria CRESPI", "Luisa DI TRAPANI", "Lucia FRANCHINI", "Roberta Rolla", "Marco Bagnati", "Patrizia PERGOLONI"]
            else:
                ## Nel caso non abbia i :, allora ce la caviamo semplicemente così
                equip = \
                    prima_parte.strip().split(",")
        except AttributeError:
            pass
        ## Per risolvere un errore
        if equip.__len__() == 1 and equip[0].__len__() > 20:
            equip = equip[0].split(',')
        ## Chiudo la connessione
        response.close()
        ## Ritorno l'array
        return equip


def rem_vowel(string):
    return (re.sub("[aeiouAEIOU]","",string))

def main():
    ## Creo le varie variabili
    main_class = gestore()
    ## Aggiungo i vari dati
    main_class.aggiungi()
    ## Creo i file
    main_class.creaFile()

## Avvio solamente se fatto da chiamata diretta
if __name__ == "__main__":
    main()
