# FileReaderRenduCsharpLinq

Console app .NET pour lire, filtrer, trier, grouper et convertir des données entre CSV, JSON et XML à l’aide de LINQ.

## 🚀 Fonctionnalités

- 📂 Lecture de fichiers : CSV, JSON, XML  
- 🔍 Recherche (filtrage), tri et groupage via LINQ  
- 👀 Aperçu interactif dans la console  
- 💾 Export des résultats dans un format cible (CSV ↔ JSON, XML → JSON)  
- 🗂️ Choix dynamique des champs à exporter  
- 🛠️ Architecture générique modulable (readers, writers, processor, UI, orchestrateur)

## 📋 Prérequis

- [.NET 6 SDK](https://dotnet.microsoft.com/download) (ou version supérieure)  
- Visual Studio 2022 (ou VS Code / Rider)  
- Windows, macOS ou Linux

## 📥 Installation

1. Clone le dépôt :  
   ```bash
   git clone https://github.com/ton-organisation/FileReaderRenduCsharpLinq.git
   cd FileReaderRenduCsharpLinq
2. Ouvre le projet dans VS 22 ou VSC
3. Restaure les dépendances
   ```bash
   dotnet restore

  4. Compile
     ```bash
     dotnetbuild

  ## Utilisation
  Lance l'app depuis la console :
  ```bash
dotnet run --project FileReaderRenduCsharpLinq
````
Tu verras le menu :
  ```bash
1) CSV → JSON
2) JSON → CSV
3) XML → JSON
Votre choix (1, 2 ou 3) :
```
## Exemple CSV → JSON 
Choisis 1.

Entre le chemin du fichier .csv à lire.

Saisis tes critères de filtre, tri et groupage.

Visualise l’aperçu dans la console.

Sélectionne les champs à exporter (séparés par des virgules).

Indique un fichier (ou un dossier) de sortie pour le JSON.

Le fichier export.json (indenté) sera généré.

## Structure du projet 
```bash
/FileReaderRenduCsharpLinq
│
├─ Models
│   ├ User.cs
│   └ Car.cs
│
├─ Services
│   ├ IRecordReader.cs        # Interface de lecture
│   ├ IRecordWriter.cs        # Interface d’écriture
│   ├ IDataProcessor.cs       # Interface LINQ (filtre/tri/groupe)
│   ├ CsvRecordReader.cs
│   ├ JsonRecordReader.cs
│   ├ XmlRecordReader.cs
│   ├ CsvRecordWriter.cs
│   ├ JsonRecordWriter.cs
│   ├ LinqDataProcessor.cs
│   ├ ConsoleUi.cs
│   └ ConversionJob.cs
│
└─ Program.cs
```
## Fichiers de test préparés
Un dossier ainsi qu'une suite de fichiers de test sont disponibles dans le dossier racine du projet mais je recommande d'utiliser les vôtre. N'ayant pas tester tout les cas d'utilisation de manière extensive il peut rester des bugs que je n'ai pas encore rencontrés.
