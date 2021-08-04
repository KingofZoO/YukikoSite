const imgInput = document.getElementById("img-input");
const imgPreview = document.getElementById("img-preview");
const titleInput = document.getElementById("title-input");
const titlePreview = document.getElementById("title-preview");
const descInput = document.getElementById("desc-input");
const descPreview = document.getElementById("desc-preview");
const filesInput = document.getElementById("files-input");
const filesPreview = document.getElementById("files-preview");

imgInput.addEventListener("change", function () {
    getImgData();
});

if (titleInput != null) {
    titleInput.addEventListener("change", function () {
        titlePreview.innerText = titleInput.value;
    });
}

descInput.addEventListener("change", function () {
    descPreview.innerText = descInput.value;
});

if (filesInput != null) {
    filesInput.addEventListener("change", function () {
        getFilesData();
    })
}

function getImgData() {
    var files = imgInput.files[0];
    if (files) {
        var fileReader = new FileReader();
        fileReader.readAsDataURL(files);
        fileReader.addEventListener("load", function () {
            imgPreview.src = fileReader.result;
        });
    }
}

function getFilesData() {
    var files = filesInput.files;
    if (files) {
        for (let i = 0; i < files.length; i++) {
            let previewElement;

            if (isImage(files[i]))
                previewElement = new Image();
            else {
                previewElement = document.createElement("video");
                previewElement.setAttribute("controls", "controls");
            }

            previewElement.height = '300px';
            previewElement.src = URL.createObjectURL(files[i]);
            filesPreview.appendChild(previewElement);
        }
    }
}

function isImage(file) {
    return file && file['type'].split('/')[0] === 'image';
}