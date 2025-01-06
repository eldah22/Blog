document.addEventListener("DOMContentLoaded", function (event) {
    var forms = document.querySelectorAll('.needs-validation')
    const password = document.getElementById("Password");
    const confirmPassword = document.getElementById("ConfirmPassword");

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                } else if ($("#Password").hasClass('is-invalid')) {
                    event.preventDefault()
                    event.stopPropagation()
                }
                form.classList.add('was-validated')
            }, false)
        })

    //check password

    const passwordAlert = document.getElementById("password-alert");
    const requirements = document.querySelectorAll(".requirements");
    let lengBoolean, bigLetterBoolean, numBoolean;
    let leng = document.querySelector(".leng");
    let bigLetter = document.querySelector(".big-letter");
    let num = document.querySelector(".num");
    const numbers = "0123456789";

    requirements.forEach((element) => element.classList.add("wrong"));

    password.addEventListener("focus", () => {
        passwordAlert.classList.remove("d-none");
        if (!password.classList.contains("is-valid")) {
            password.classList.add("is-invalid");
        }
    });

    password.addEventListener("input", () => {
        let value = password.value;
        if (value.length < 8) {
            lengBoolean = false;
        } else if (value.length > 7) {
            lengBoolean = true;
        }

        if (value.toLowerCase() == value) {
            bigLetterBoolean = false;
        } else {
            bigLetterBoolean = true;
        }

        numBoolean = false;
        for (let i = 0; i < value.length; i++) {
            if ($.inArray(value[i], numbers) != -1) {
                numBoolean = true;
            }
        }


        if (lengBoolean == true && bigLetterBoolean == true && numBoolean == true) {
            password.classList.remove("is-invalid");
            password.classList.add("is-valid");
            password.setCustomValidity('')

            requirements.forEach((element) => {
                element.classList.remove("wrong");
                element.classList.add("good");
            });
            passwordAlert.classList.remove("alert-warning");
            passwordAlert.classList.add("alert-success");
        } else {
            password.classList.remove("is-valid");
            password.classList.add("is-invalid");
            password.setCustomValidity('Password is not correct')

            passwordAlert.classList.add("alert-warning");
            passwordAlert.classList.remove("alert-success");

            if (lengBoolean == false) {
                leng.classList.add("wrong");
                leng.classList.remove("good");
            } else {
                leng.classList.add("good");
                leng.classList.remove("wrong");
            }

            if (bigLetterBoolean == false) {
                bigLetter.classList.add("wrong");
                bigLetter.classList.remove("good");
            } else {
                bigLetter.classList.add("good");
                bigLetter.classList.remove("wrong");
            }

            if (numBoolean == false) {
                num.classList.add("wrong");
                num.classList.remove("good");
            } else {
                num.classList.add("good");
                num.classList.remove("wrong");
            }
        }
    });

    password.addEventListener("blur", () => {
        passwordAlert.classList.add("d-none");
    });

    confirmPassword.addEventListener("focus", () => {
        if (!confirmPassword.classList.contains("is-valid")) {
            confirmPassword.classList.add("is-invalid");
        }
    });

    confirmPassword.addEventListener("input", () => {
        if ($(confirmPassword).val() == $(password).val()) {
            if (password.classList.contains("is-valid")) {
                confirmPassword.classList.remove("is-invalid");
                confirmPassword.classList.add("is-valid");
                confirmPassword.setCustomValidity('')
            }
        } else {
            confirmPassword.classList.remove("is-valid");
            confirmPassword.classList.add("is-invalid");
            confirmPassword.setCustomValidity('Password is not correct')
        }
        
    });
});
