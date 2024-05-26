document.addEventListener('DOMContentLoaded', function () {
    document.getElementById('button').addEventListener('click', function (event) {
        event.preventDefault();
        var errorMessageContainer = document.getElementById("error-message");
        var email = document.getElementById("email").value
        var psw = document.getElementById("psw").value;
        var pswRepeat = document.getElementById("psw-repeat").value;
        var wariorName = document.getElementById("wariorName").value
        if (wariorName === '') {
            errorMessageContainer.textContent = 'Please enter a warior name.';
            return;
        }
        if (email === '') {
            errorMessageContainer.textContent = 'Please enter an email.';
            return;
        }
        if (psw === '') {
            errorMessageContainer.textContent = 'Please enter a password.';
            return;
        }
        if (pswRepeat === '') {
            errorMessageContainer.textContent = 'Please repeat password.';
            return;
        }
        if (psw !== pswRepeat) {
            errorMessageContainer.textContent = 'Passwords do not match!';
            return;
        }
        if (psw.length < 6) {
            errorMessageContainer.textContent = 'Password must be at least 6 characters long!';
            return;
        }
        const formDataJSON = {
            "email": email,
            "password": psw,
            "wariorName": wariorName
        }
        fetch('https://localhost:5051/api/Authentication/register', {
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
            return response.json()
        } else {
            console.log('registerd')
            window.location.href = 'index.html?email=' + encodeURIComponent(email);
        };
        }).then(data => {
            console.log(data.message);
            errorMessageContainer.textContent = data.message;
        });
    });
});