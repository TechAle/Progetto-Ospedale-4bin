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
        citta = citta[0].upper() + citta[1:].lower()
        try:
            self.dati = pd.read_csv(urllib.request.urlopen("https://data.humdata.org/hxlproxy/api/data-preview.csv?url=https%3A%2F%2Fraw.githubusercontent.com%2FCSSEGISandData%2FCOVID-19%2Fmaster%2Fcsse_covid_19_data%2Fcsse_covid_19_time_series%2Ftime_series_covid19_confirmed_global.csv&filename=time_series_covid19_confirmed_global.csv"))
            self.ricoverati = pd.read_csv(urllib.request.urlopen("https://data.humdata.org/hxlproxy/api/data-preview.csv?url=https%3A%2F%2Fraw.githubusercontent.com%2FCSSEGISandData%2FCOVID-19%2Fmaster%2Fcsse_covid_19_data%2Fcsse_covid_19_time_series%2Ftime_series_covid19_recovered_global.csv&filename=time_series_covid19_recovered_global.csv"))
            self.decessi = pd.read_csv(urllib.request.urlopen("https://data.humdata.org/hxlproxy/api/data-preview.csv?url=https%3A%2F%2Fraw.githubusercontent.com%2FCSSEGISandData%2FCOVID-19%2Fmaster%2Fcsse_covid_19_data%2Fcsse_covid_19_time_series%2Ftime_series_covid19_deaths_global.csv&filename=time_series_covid19_deaths_global.csv"))
        except:
            print("Sei sicuro di essere connesso ad internet?")
            exit(0)
        if not self.dati["Country/Region"].str.contains(citta).any():
            self.citta = self.probabilita(citta)
        self.casi = self.dati.loc[self.dati["Country/Region"] == citta].values[0][4:]
        self.decessi = self.decessi.loc[self.decessi["Country/Region"] == citta].values[0][4:]
        self.ricoverati = self.ricoverati.loc[self.ricoverati["Country/Region"] == citta].values[0][4:]
        self.giorni = self.dati.keys()[4:]

    def probabilita(self,prob_citta):
        # https://stackoverflow.com/questions/17388213/find-the-similarity-metric-between-two-strings
        def similar(a, b):
            return SequenceMatcher(None, a, b).ratio()
        ## Prendo gli stati
        stati = [val for val in set(self.dati["Country/Region"])]
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
            crea_ex(self.casi,self.decessi,self.ricoverati,self.giorni)
        elif richiesta == 'c':
            pass


def crea_ex(infetti,decessi,ricoverati,giorni):
    excel_file = xlsxwriter.Workbook('dati.xlsx')
    excel_pagina = excel_file.add_worksheet()
    excel_pagina.write(0,0,"Giorno_Cont")
    excel_pagina.write(0, 1, "Giorno")
    excel_pagina.write(0,2,"Infetti")
    excel_pagina.write(0,3,"Decessi")
    excel_pagina.write(0,4,"Ricoverati")
    for i in range(len(infetti)):
        excel_pagina.write(i+1, 0, i)
        excel_pagina.write(i + 1, 1, giorni[i])
        excel_pagina.write(i+1,2,infetti[i])
        excel_pagina.write(i+1, 3, decessi[i])
        excel_pagina.write(i+1, 4, ricoverati[i])
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