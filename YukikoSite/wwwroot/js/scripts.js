const imgInput = document.getElementById("img-input");
const imgPreview = document.getElementById("img-preview");
const titleInput = document.getElementById("title-input");
const titlePreview = document.getElementById("title-preview");
const descInput = document.getElementById("desc-input");
const descPreview = document.getElementById("desc-preview");

imgInput.addEventListener("change", function () {
    getImgData();
});

titleInput.addEventListener("change", function () {
    titlePreview.innerText = titleInput.value;
});

descInput.addEventListener("change", function () {
    descPreview.innerText = descInput.value;
});

function getImgData() {
    const files = imgInput.files[0];
    if (files) {
        const fileReader = new FileReader();
        fileReader.readAsDataURL(files);
        fileReader.addEventListener("load", function () {
            imgPreview.src = fileReader.result;
        });
    }
}