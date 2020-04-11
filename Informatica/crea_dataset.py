import random
import csv
## Classe che gestisce il tutto
class gestore:
    def __init__(self):
        ## Preparo i dati
        self.cont = {"parole" : set(), "cognomi" : set(), "nomi" : set()}
        self.getReparti()

    def aggiungi(self):
        for i in self.cont.keys():
            self.setting(i)

    def setting(self, parola):
        with open("./dataset/{}.txt".format(parola), "r") as rd:
            for i in rd.read().split():
                self.cont[parola].add(i)

    def creaFile(self):
        with open("./dataset/medici.csv", "w") as filecsv:
            writer = csv.writer(filecsv, delimiter=',', quotechar='|', quoting=csv.QUOTE_MINIMAL)
            writer.writerow(["Nome", "Cognome", "Ruolo", "padiglione", "reparto", "piano reparto", "Numero di telefono", "Abitazione", "data di nascita"])
            numero_telefono = set()
            for i in self.cont.keys():
                self.cont[i] = list(self.cont[i])
            for j in self.reparti:
                for i in range(10):
                    if i == 0:
                        ruolo = "capo_reparto"
                    else:
                        ruolo = "equipe_medica"

                    numero = self.getNumero()
                    while(numero_telefono.__contains__(numero)):
                        numero = self.getNumero()
                    numero_telefono.add(numero)

                    writer.writerow([self.cont["nomi"][random.randint(0,len(self.cont["nomi"])-1)], self.cont["cognomi"][random.randint(0,len(self.cont["cognomi"])-1)],ruolo,j[-1] ,j[0], j[1] ,numero , self.getVia(), self.getData()])
        with open("./dataset/pazienti.csv", "w") as filecsv:
            writer = csv.writer(filecsv, delimiter=',', quotechar='|', quoting=csv.QUOTE_MINIMAL)
            writer.writerow(["Nome", "Cognome", "Data di nascita"])
            for i in range(random.randint(400,600)):
                writer.writerow([self.cont["nomi"][random.randint(0,len(self.cont["nomi"])-1)], self.cont["cognomi"][random.randint(0,len(self.cont["cognomi"])-1)],self.getData()])


    def getVia(self):
        return "via " +  self.cont["nomi"][random.randint(0,len(self.cont["nomi"])-1)] + " n^" + str(random.randint(0,100))
    def getNumero(self):
        return "39" + str(random.randint(3000000000, 3990000000))

    def getData(self):
        return str(random.randint(1, 30)) + "/" + str(random.randint(1, 12)) + "/" + str(random.randint(1980, 2000))

    def getReparti(self):
        self.reparti = []
        with open("./dataset/reparti.txt", "r") as rt:
            for line in rt:
                self.reparti += [[line.split("|")[0],line.split("|")[1],line.split("|")[2].replace("\n","")]]
        print(self.reparti)


def main():
    main_class = gestore()
    main_class.aggiungi()
    main_class.creaFile()


if __name__ == "__main__":
    main()