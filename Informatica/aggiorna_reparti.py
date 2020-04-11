try:
    from bs4 import BeautifulSoup
except:
    print("Devi installare bs4 per avviare questo programma.\nPer installarlo: pip install -U bs4")
try:
    import requests
except:
    print("Devi installare requests per avviare questo programma.\nPer installarlo: pip install -U requests")

testo_base = "http://www.maggioreosp.novara.it/attivita-assistenziale/strutture-sanitarie/elenco-delle-strutture-sanitarie/padiglione-"
Nomi = []
i = 0
prev = 0
while(True):
    lettera = chr(i+97)
    response = requests.get(testo_base + lettera)
    soup = BeautifulSoup(response.content, "html.parser")
    if soup.title.getText().__contains__("404"):
        break
    else:
        print("Piano {0}".format(lettera))
    prova = soup.findAll("div", {"class", "siteorigin-widget-tinymce textwidget"})
    Nomi += [[prova[i].getText().split("\n")[1],prova[i].getText().split("\n")[2].split(":")[1:], lettera] for i,j in enumerate(prova) if i > 1]
    i += 1

piani = {"terzo" : 3,"seminterrato" : -1, "terra" : 0, "terreno" : 0, "secondo" : 2, "quarto" : 4, "primo" : 1, "rialzato" : 0}
with open("./dataset/reparti.txt", "w") as wt:
    for i in Nomi:
        piano = -2
        print(i)
        for j in piani.keys():
            if i[1][0].__contains__(j):
                piano = piani[j]
                break
        wt.writelines(i[0] + "|" + str(piano) + "|" + i[2] + "\n")