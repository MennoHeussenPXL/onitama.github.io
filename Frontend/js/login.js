function hideVideo() {
    var video = document.getElementById('introVideo');
    fadeOut(video); // Roep de fadeOut-functie aan om de video te laten vervagen

}

function fadeOut(element) {
    var opacity = 1;
    var timer = setInterval(function () {
        if (opacity <= 0.01) {
            clearInterval(timer);
            element.style.opacity = 0;
            element.style.display = 'none'; // Zorg ervoor dat het element volledig verborgen is
        }
        element.style.opacity = opacity;
        opacity -= 0.005;
    }, 1); // Pas de snelheid van de fade-out aan door de waarde hier te wijzigen

}
document.addEventListener('DOMContentLoaded', function () {
    const urlParams = new URLSearchParams(window.location.search);
    const email = urlParams.get('email');
    document.getElementById('email').value = email;
    document.getElementById('buttonLogin').addEventListener('click', function (event) {
        event.preventDefault();
        var errorMessageContainer = document.getElementById("error-message");
        var email = document.getElementById("email").value
        var psw = document.getElementById("psw").value;
        if (email === '') {
            errorMessageContainer.textContent = 'Please enter an email.';
            return;
        }
        if (psw === '') {
            errorMessageContainer.textContent = 'Please enter a password.';
            return;
        }
        const formDataJSON = {
            "email": email,
            "password": psw,
        }
        fetch('https://localhost:5051/api/Authentication/token', {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            },
            body: JSON.stringify(formDataJSON),
        })
        .then(response => {
            if (!response.ok) {
                if (response.status == 400) {
                    errorMessageContainer.textContent = "The field Password must have a minimum length of 6.";
                    throw new Error("The field Password must have a minimum length of 6.");
                } else if (response.status == 401) {
                    errorMessageContainer.textContent = "Incorrect email or password.";
                    throw new Error("Incorrect email or password.");
                }
            } else {
                console.log('login succesful');
                return response.json();
            }
        }).then(data => {
            sessionStorage.setItem("id", data.user.id)
            sessionStorage.setItem("token", data.token)
            sessionStorage.setItem("warriorName", data.user.warriorName)
            window.location.href = 'lobby.html';
        });
    });
});