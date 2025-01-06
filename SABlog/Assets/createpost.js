function onFileUpload(input) {
    var tempFiles = $("#TempFiles")[0]
    var files = tempFiles.files
    $("#errorFileText").empty()

    let list = new DataTransfer();
    var tmpFileArray = Array.from($("#UploadedFiles")[0].files || [])
    var limit = (files.length + tmpFileArray.length <= 5) ? files.length : 5 - tmpFileArray.length
    for (var j = 0; j < limit; j++) {
        if (tmpFileArray.filter(f => f.name == files[j].name && f.size == files[j].size).length == 0) {
            list.items.add(files[j])
        } 
    }
    files = list.files

    if (files.length > 5 || files.length + $("#UploadedFiles")[0].files.length > 5) {
        $("#errorFile").removeClass("invisible")
        var err = $(`
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                  Keni ngarkuar më shumë se 5 skedarë. U morën 5 të parët.
                  <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            `)
        $("#errorFileText").append(err)
    }



    if ($("#UploadedFiles")[0].files.length == 0) {
        $("#fileListBody").hide()
        $("#UploadedFiles")[0].setCustomValidity('Not one file selected!')
    } else {
        $("#UploadedFiles")[0].setCustomValidity('')
    }

    if(files.length == 0) return

    var imageExtensions =
        /(\.jpg|\.jpeg|\.png|\.gif|\.webp)$/;
    var documentExtensions = /(\.xls|\.xlsx|\.pdf|\.doc|\.docx|\.ppt|\.pptx)$/;
    for (var i = 0; i < files.length; i++) {
        var input = tempFiles.files[i];
        var uploadButton = $("#uploadButton")
        var filePath = input.name;
        
        if (input.size > 5000000) {
            $("#errorFile").removeClass("invisible")
            var err = $(`
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                  ${filePath} është më i madh se 5MB. Ngarkoni skedarë më të vegjël.
                  <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            `)
            $("#errorFileText").append(err)
            continue;
        }

        if (imageExtensions.exec(filePath) == null && documentExtensions.exec(filePath) == null) {
            $("#errorFile").removeClass("invisible")
            var err = $(`
                <div class="alert alert-danger alert-dismissible fade show" role="alert">
                  ${filePath} është skedar i pa suportuar. Ngarko foto ose dokumente Word/Excel/Powerpoint/PDF
                  <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                </div>
            `)
            $("#errorFileText").append(err)
            continue;
        }

        if ($("#errorFileText").children.length == 0) $("#errorFileText").hide()
        $("#fileListBody").show()

        let list = new DataTransfer();
        for (var j = 0; j < $("#UploadedFiles")[0].files.length; j++) {
            list.items.add($("#UploadedFiles")[0].files[j])
        }
        list.items.add(input);

        $("#UploadedFiles")[0].files = list.files
        if ($("#UploadedFiles")[0].files.length > 0) {
            $("#UploadedFiles")[0].setCustomValidity('')
        } else {
            $("#UploadedFiles")[0].setCustomValidity('Invalid')
        }

        var fileCount = $("#UploadedFiles")[0].files.length


        if (imageExtensions.exec(filePath) != null) {
            var card = $(`
                            <div class="d-flex justify-content-between py-2 align-items-start" id="fileCard-${fileCount-1}">
                                <div class="ratio ratio-16x9" id="fileUploadImage-${fileCount-1}">
                                    <img src=${(window.URL ? URL : webkitURL).createObjectURL(input)} class="img-fluid h-100 w-100 rounded-2" alt="mobile screen" style="object-fit: cover">
                                </div>
                                <div class="col-6 px-2">
                                    <p>${filePath}</p>
                                </div>
                                <button type="button" class="text-end btn btn-danger" id="delete-${fileCount - 1}" onclick="removeFile(${fileCount - 1})">Fshi</button>
                            </div>
                            `)

            $("#fileList").append(card)
        } else {
            var card = $(`
                        <div class="d-flex justify-content-between py-2 align-items-start" id="fileCard-${fileCount - 1}">
                            <div class="ratio ratio-16x9" id="fileUploadImage-${fileCount - 1}">
                                <img src="${window.location.origin}/Content/Images/document.png" class="img-fluid h-100 w-100 rounded-2" alt="mobile screen" style="object-fit: cover">
                            </div>
                            <div class="col-6 px-2">
                                <p>${filePath}</p>
                            </div>
                            <button type="button" class="text-end btn btn-danger" id="delete-${fileCount - 1}" onclick="removeFile(${fileCount - 1})">Fshi</button>
                        </div>
                    `)

            $("#fileList").append(card)
        }
    }

    if ($("#UploadedFiles")[0].files.length >= 5) $("#uploadButton").addClass("disabled")
    document.getElementById("TempFiles").value = null;
}


function removeFile(index) {
    let list = new DataTransfer();
    for (var j = 0; j < $("#UploadedFiles")[0].files.length; j++) {
        if (j == index) continue;
        list.items.add($("#UploadedFiles")[0].files[j])
    }

    $("#UploadedFiles")[0].files = list.files

    $(`#fileCard-${index}`).remove()

    var k = 0;
    if ($("#UploadedFiles")[0].files.length == 0) {
        $("#fileListBody").hide()
        $("#UploadedFiles")[0].setCustomValidity('Invalid')
    } else {
        $("#UploadedFiles")[0].setCustomValidity('')
        $("#fileList").children().each(function () {
            var currID = $(this).attr("id").replace("fileCard-", "")
            $(this).attr("id", `fileCard-${k}`)
            $(`#delete-${currID}`).attr("id", `delete-${k}`)
            $(`#delete-${k}`).attr("onclick", `removeFile(${k})`)
            k++
        })
    }

    if ($("#UploadedFiles")[0].files.length < 5) $("#uploadButton").removeClass("disabled")

}



document.addEventListener("DOMContentLoaded", function (event) {
    var forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                checkIfOneCheckboxIsChecked()
                var errorElements = document.querySelectorAll(
                    "input.form-control:invalid");

                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                    if (errorElements.length > 0) $('html, body').animate({
                        scrollTop: ($(errorElements[0]).offset().top < 400) ? 0 : $(errorElements[0]).offset().top
                    }, 200);
                } else if (checkIfOneCheckboxIsChecked() == false) {
                    event.preventDefault()
                    event.stopPropagation()
                    if (errorElements.length > 0) $('html, body').animate({
                        scrollTop: ($(errorElements[0]).offset().top < 400) ? 0 : $(errorElements[0]).offset().top
                    }, 200);
                }

                

                form.classList.add('was-validated')
            }, false)
        })

    $("#categoriesCard input[type=checkbox]").each(function () {
        this.addEventListener('change', function (event) {
            checkIfOneCheckboxIsChecked();
        });
    });


    var files = $("#UploadedFiles")[0]?.files
    if (files?.length == 0) {
        $("#fileListBody").hide()
    } else {
        $("#fileListBody").show()
    }
});


function checkIfOneCheckboxIsChecked() {
    var checkedBoxes = $("#categoriesCard input[type=checkbox]:checked");
    if (checkedBoxes.length == 0) {
        $("#categoriesCard input[type=checkbox]").each(function () {
            this.setCustomValidity('Not one checked')
        });
        return false;
    } else {
        $("#categoriesCard input[type=checkbox]").each(function () {
            this.setCustomValidity('')
        });
        return true;
    }
}