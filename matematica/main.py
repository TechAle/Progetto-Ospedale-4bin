try:
    import numpy as np
except:
    print("Per avviare il programma, è necessario installare numpy: pip install -U numpy")
try:
    import pandas as pd
except:
    print("Per avviare il programma, è necessario installare pandas: pip install -U pandas")
try:
    from difflib import SequenceMatcher
except:
    print("Per avviare il programma, è necessario installare ddifflib: pip install -U difflib")
try:
    import matplotlib.pyplot as plt
except:
    print("Per avviare il programma, è necessario installare matplotlib: pip install -U matplotlib")
import urllib.request
try:
    import xlsxwriter
except:
    print("Per avviare il programma, è necessario installare xlswriter: pip install xlswriter")

class inizio_class():
    def __init__(self, citta):
        ## Apro il file
        citta = citta.lower()
        try:
            self.dati = pd.read_csv(urllib.request.urlopen("https://opendata.ecdc.europa.eu/covid19/casedistribution/csv"))
        except:
            print("Sei sicuro di essere connesso ad internet?")
            exit(0)
        ## Controllo dell'esistenza
        if np.where(self.dati["countriesAndTerritories"].str.lower() == citta)[0].__len__() == 0:
            self.citta = self.probabilita(citta)
        ## Raccolgo il tutto
        self.citta = self.dati.loc[self.dati["countriesAndTerritories"] == "Italy"][["dateRep", "cases", "deaths"]]
        self.citta = self.citta[:self.citta['cases'].index[self.citta['cases'].astype(int) > 0][-1] - self.citta.first_valid_index() + 1].iloc[::-1]

    def probabilita(self,prob_citta):
        # https://stackoverflow.com/questions/17388213/find-the-similarity-metric-between-two-strings
        def similar(a, b):
            return SequenceMatcher(None, a, b).ratio()
        ## Prendo gli stati
        stati = [val.lower for val in set(self.dati["countriesAndTerritories"])]
        ## creo array vuoto, 5 possibilita
        lista_finale = np.empty(5,dtype=int)
        lista_parole = np.empty(5, dtype="S20")
        for i, j in enumerate(stati):
            if i < lista_finale.__len__():
                lista_finale[i] = similar(prob_citta, stati[i])
                lista_parole[i] = stati[i]

            else:
                ## Controllo se è la prima volta che entra qua dentro
                if (i == lista_finale.__len__()):
                    ## Ordino le due liste. La seconda dipende dalla prima per questi che zippiamo
                    lista_finale, lista_parole = zip(*sorted((zip(lista_finale, lista_parole))))
                    lista_finale = list(lista_finale)
                    lista_parole = list(lista_parole)
                flag = similar(prob_citta, stati[i])
                if flag > lista_finale[0]:
                    lista_finale[0] = flag
                    lista_parole[0] = stati[i]
                    ## Ordino le due liste. La seconda dipende dalla prima per questi che zippiamo
                    lista_finale, lista_parole = zip(*sorted((zip(lista_finale, lista_parole))))
                    lista_finale = list(lista_finale)
                    lista_parole = list(lista_parole)

        print("La parola non esiste. Forse intendevi")
        lista_parole = list(reversed(lista_parole))
        for i,j in enumerate(lista_parole):
            print(f"{i+1}\t{j}")
        val = int(input("scelta: "))
        if val < 1 or val > lista_parole.__len__():
            print("Errore")
            exit(0)
        else:
            return lista_parole[val-1]



    def richiesta(self, richiesta):
        if richiesta == 'a':
            visualizza(self.citta)
        elif richiesta == 'b':
            crea_ex(self.citta)
        elif richiesta == 'c':
            pass

def crea_ex(dati):
    excel_file = xlsxwriter.Workbook('dati.xlsx')
    excel_pagina = excel_file.add_worksheet()
    for righe,i in enumerate(dati):
        for colonna,dato in enumerate(dati[i]):
            excel_pagina.write(colonna,righe,dato)
    excel_file.close()


def visualizza(dati):
    plt.plot([i for i in range(dati["cases"].__len__())], dati["cases"])
    plt.xlabel("Numeri")
    plt.ylabel("Quantità")
    plt.show()



def main():
    inizio = inizio_class(input("Inserire la città"))
    inizio.richiesta(input("a) Visualizzazione del grafico\nb) Esporta excele\nc) Predizione"))
if __name__ == "__main__":
    main()