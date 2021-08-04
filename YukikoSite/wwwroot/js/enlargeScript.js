const body = document.getElementById("layout-body");
var newImage;

function enlargeImg(img, scaleValue = 3) {
    if (isMobile(img.width))
        return;

    if (newImage != undefined)
        newImage.remove();

    newImage = new Image();
    newImage.src = img.src;
    newImage.width = img.width;
    newImage.style.position = "fixed";
    newImage.style.top = "50%";
    newImage.style.left = "50%";
    newImage.style.transform = "translate(-50%, -50%) scale(" + scaleValue + ")";
    newImage.onclick = function () { newImage.remove(); }
    body.appendChild(newImage);
}
function isMobile(width) {
    return screen.width * 0.6 < width;
}