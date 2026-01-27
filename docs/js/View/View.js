class View {
    constructor() {
        const token = sessionStorage.getItem("token");

        // Fonction pour gérer la redirection vers la page de login
        const redirectToLogin = () => {
            if (window.location.href.endsWith("index.html")) {
                window.location = 'pages/login.html';
            } else {
                window.location = 'login.html';
            }
        };

        // Si pas de token et pas sur login/register => redirection
        if (!window.location.href.endsWith("login.html") &&
            !window.location.href.endsWith("register.html") &&
            !token) {
            redirectToLogin();
        } else {
            // Crée le header dynamique
            this.DisplayHeader();

            // Démarrage du chronomètre de session
            this.StartTimer();

            // Réinitialisation du timer à chaque clic
            window.addEventListener("click", () => this.ResetTimer());
        }
    }

    // Méthode pour créer le header dynamiquement
   DisplayHeader() {
        // Crée le header et le nav
        let header = document.createElement("header");
        let nav = document.createElement("nav");
        nav.className = "navbar navbar-expand-lg navbar-light bg-light";

        // Crée le container et son HTML
        let div = document.createElement("div");
        div.className = "container-fluid";

        let a = document.createElement("a");
        a.classList.add("navbar-brand");
        let img = document.createElement("img");
        
        img.alt="Logo IUT";
        img.style="height:40px; margin-right:10px;";

        if (window.location.href.endsWith("index.html")) {
            a.href="index.html";
            img.src="images/logo.png";
        } else {
            a.href="../index.html";
            img.src="../images/logo.png";
        }

        a.appendChild(img);
        a.innerHTML += "RAPPORT IUT";

        div.appendChild(a);

        if (!window.location.href.endsWith("login.html") && !window.location.href.endsWith("register.html")) {
               
            let button = document.createElement("button");
            button.className = "navbar-toggler";
            button.type = "button";
            button.setAttribute("data-bs-toggle", "collapse");
            button.setAttribute("data-bs-target", "#navbarMenu");
            button.setAttribute("aria-controls", "navbarMenu");
            button.setAttribute("aria-expanded", "false");
            button.setAttribute("aria-label", "Toggle navigation");
            
            let span = document.createElement("span");
            span.className = "navbar-toggler-icon";
            button.appendChild(span);
            
            let collapseDiv = document.createElement("div");
            collapseDiv.className = "collapse navbar-collapse";
            collapseDiv.id = "navbarMenu";
            
            let ul = document.createElement("ul");
            ul.className = "navbar-nav ms-auto mb-2 mb-lg-0";
            
            let li1 = document.createElement("li");
            li1.className = "nav-item";
            let a1 = document.createElement("a");
            a1.className = "nav-link";
            
            a1.textContent = "Déposer un rapport";
            
            
            let li2 = document.createElement("li");
            li2.className = "nav-item";
            let a2 = document.createElement("a");
            a2.className = "nav-link";
            a2.textContent = "Bibliothèque de rapports";
            
            if (window.location.href.endsWith("index.html")) {
                a2.href = "pages/visualisationAll.html";
                a1.href = "pages/depot.html";
            } else {
                a2.href = "visualisationAll.html";
                a1.href = "depot.html";
            }
            

            li1.appendChild(a1);
            ul.appendChild(li1);

            li2.appendChild(a2);
            ul.appendChild(li2);
            
            let li3 = document.createElement("li");
            li3.className = "nav-item d-flex align-items-center";
            li3.id = "user-container";
            ul.appendChild(li3);
            
            collapseDiv.appendChild(ul);
            
            div.appendChild(button);
            div.appendChild(collapseDiv);
        }
        // Ajoute le container au nav, puis le nav au header
        nav.appendChild(div);
        header.appendChild(nav);

        // Ajoute le header au début du body
        document.body.prepend(header);

        // Ajoute le nom de l'utilisateur et le bouton logout dynamiquement
        const userContainer = document.getElementById("user-container");

        const username = sessionStorage.getItem("user_Login") || "Invité";
        const nameSpan = document.createElement("span");
        nameSpan.className = "me-2 fw-bold"; // marge à droite
        nameSpan.textContent = username;

        const logoutBtn = document.createElement("button");
        logoutBtn.className = "btn btn-outline-dark";
        logoutBtn.id = "logout";
        logoutBtn.textContent = "Déconnexion";

        userContainer.appendChild(nameSpan);
        userContainer.appendChild(logoutBtn);

        // Gestion de la déconnexion
        this.btnDeconnexion = logoutBtn;
        this.btnDeconnexion.addEventListener("click", () => {
            sessionStorage.clear();
            localStorage.clear();
            window.location = "login.html";
        });
    }


    // Déconnexion automatique après timeout
    Disconect() {
        if (!window.location.href.endsWith("login.html") &&
            !window.location.href.endsWith("register.html")) {
            localStorage.clear();
            sessionStorage.clear();
            alert("Timeout : vous avez été déconnecté.");
            window.location = "login.html";
        }
    }

    // Démarrage du chronomètre
    StartTimer() {
        this.timer = setTimeout(() => this.Disconect(), 5 * 60 * 1000); // 5 minutes
    }

    // Réinitialisation du chronomètre
    ResetTimer() {
        clearTimeout(this.timer);
        this.StartTimer();
    }
}

// Initialisation au chargement
window.onload = () => {
    new View();
};
