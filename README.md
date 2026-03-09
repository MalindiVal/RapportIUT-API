![Docker](https://github.com/MalindiVal/S3_B2_ReMOVe/actions/workflows/docker.yml/badge.svg)
![Publish](https://github.com/MalindiVal/S3_B2_ReMOVe/actions/workflows/publish.yml/badge.svg)
# RapportIUT‑API

API REST backend en ASP.NET pour l’application web RapportIUT.
Elle sert de service central pour stocker, authentifier et exposer des rapports, tags, et utilisateurs à la partie front‑end (RapportIUT‑Client).

## 🧩 Description

Ce projet implémente une API web en C# avec ASP.NET, connectée à une base de données SQLite.
Elle fournit des routes pour gérer les entités principales :

### 📌 Controllers

- ✔️ RapportController – Gestion des rapports
- ✔️ TagController – Gestion des tags associés aux rapports
- ✔️ UserController – Gestion des utilisateurs et authentification

### 📌 Services

- ✔️ RapportService
- ✔️ TagService
- ✔️ UserService
- ✔️ JWTService – Gestion du token d’authentification (JWT)

## 🗂 Structure du projet
```
RapportIUT‑API/
├── API/                          # Code de l’API ASP.NET
│   ├── Controllers/             # Endpoints de l’API
│   ├── Services/                # Logique métier et services
│   └── Models/                  # Modèles de données Entity Framework
├── .github/workflows/           # CI/CD pour tests / déploiement
├── docker-compose.yml           # Pour exécuter API + DB en local
├── Serveur.sln                  # Solution Visual Studio
└── README.md                   # Ce fichier
```

## 🚀 Installation & Usage
### 🔧 Prérequis
- .NET 8 SDK ou version compatible
- SQLite (inclus via EF Core)
- Docker

### 💻 Exécuter en local

#### Clone le dépôt :
```
git clone https://github.com/MalindiVal/RapportIUT‑API.git
cd RapportIUT‑API
```

#### Construis et lance l’API :
```
dotnet restore
dotnet build
dotnet run --project API
```
L’API tournera par défaut sur http://localhost:5000 ou https://localhost:5001.

### 🐋 Avec Docker (optionnel)

Si docker est installé, tu peux utiliser le docker‑compose pour lancer API + DB :

```
docker compose up --build
```

## 🛡️ Authentification

Cette API utilise JWT (JSON Web Tokens) pour sécuriser les routes privées.
Le token est généré par le contrôleur d’authentification et doit être envoyé dans l’en‑tête Authorization: Bearer <token> pour accéder aux endpoints sécurisés.

## 🔗 Liens utiles

🔹 API backend : https://github.com/MalindiVal/RapportIUT‑API

🔹 Client frontend : https://github.com/MalindiVal/RapportIUT‑Client
