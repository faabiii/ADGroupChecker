# ADGroupChecker

ADGroupChecker ist ein Tool, das E-Mail-aktivierte Gruppen im Active Directory (AD) ausliest und den Filestore überprüft, um die Verwendung dieser Gruppen zu identifizieren. Dieses Tool ist nützlich, um Verzeichnisse zu finden, in denen Verteilergruppen statt Berechtigungsgruppen verwendet werden.

Idee von Chief Wiggum (https://www.fachinformatiker.de/profile/2162-chief-wiggum/)

## Funktionen

- Liest E-Mail-aktivierte Gruppen (Verteilergruppen) aus dem Active Directory aus.
- Überprüft den Filestore auf die Verwendung dieser Gruppen.

## Voraussetzungen

- Windows-Betriebssystem

## Installation

### 1. Download

Laden Sie die neueste Version von der [Releases-Seite](https://github.com/faabiii/ADGroupChecker/releases) herunter. 

1. Gehen Sie zur [Releases-Seite](https://github.com/faabiii/ADGroupChecker/releases).
2. Klicken Sie auf die neueste Version.
3. Laden Sie die Datei `ADGroupChecker.zip` herunter.

### 2. Entpacken

Entpacken Sie die heruntergeladene ZIP-Datei an einen beliebigen Ort auf Ihrem PC.

## Ausführung

Navigieren Sie zum entpackten Verzeichnis und führen Sie die `ADGroupChecker.exe` aus:

1. Öffnen Sie die Eingabeaufforderung oder PowerShell.
2. Navigieren Sie zu dem Verzeichnis, in dem Sie die Datei entpackt haben.
3. Führen Sie den folgenden Befehl aus:

```sh
.\ADGroupChecker.exe
```

## Verwendung
Beim Starten der Anwendung werden Sie aufgefordert, die folgenden Informationen einzugeben:

1. Domain: Die Domäne Ihres Active Directory.
2. LDAP-Pfad: Der LDAP-Pfad zum Durchsuchen der E-Mail-aktivierten Gruppen.
3. Filestore-Pfad: Der Pfad zum Filestore, den Sie überprüfen möchten.

### Beispiel:
```sh
Bitte geben Sie die Domain ein: mydomain.local
Bitte geben Sie den LDAP-Pfad ein: LDAP://mydomain.local/DC=mydomain,DC=local
Bitte geben Sie den Pfad zum Filestore ein: C:\
```
