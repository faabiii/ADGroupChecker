# ADGroupChecker

ADGroupChecker ist ein Tool, das E-Mail-aktivierte Gruppen im Active Directory (AD) ausliest und den Filestore überprüft, um die Verwendung dieser Gruppen zu identifizieren. Dieses Tool ist nützlich, um Verzeichnisse zu finden, in denen Verteilergruppen statt Berechtigungsgruppen verwendet werden.

Idee von Chief Wiggum (https://www.fachinformatiker.de/profile/2162-chief-wiggum/)

## Funktionen

- Liest E-Mail-aktivierte Gruppen (Verteilergruppen) aus dem Active Directory aus.
- Überprüft den Filestore auf die Verwendung dieser Gruppen.
- Zeigt eine Fortschrittsanzeige während der Überprüfung an.
- Gibt die Ergebnisse in einer übersichtlichen Form aus.

## Voraussetzungen

- .NET SDK 8.0 oder höher

## Installation

### 1. .NET SDK installieren

Stellen Sie sicher, dass das .NET SDK auf Ihrem System installiert ist. Sie können es von der [offiziellen .NET-Website](https://dotnet.microsoft.com/download) herunterladen und installieren.

### 2. Projekt klonen

Klonen Sie das Repository:

```sh
git clone https://github.com/faabiii/ADGroupChecker.git
cd ADGroupChecker
